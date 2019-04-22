using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Linq.Extensions;

namespace Abp.Push.Requests
{
    /// <summary>
    /// Implements <see cref="IPushRequestStore"/> using in memory storage (thread-safe).
    /// </summary>
    public class AbpInMemoryPushRequestStore : AbpServiceBase, IPushRequestStore
    {
        protected readonly ConcurrentDictionary<int?, ConcurrentDictionary<Guid, PushRequestSubscription>> MultiTenantSubscriptions;
        // push request only defined on Host side
        protected readonly ConcurrentDictionary<Guid, PushRequest> Requests;
        protected readonly IGuidGenerator GuidGenerator;

        // Set the initial capacity to some prime number above that, to ensure that
        // the ConcurrentDictionary does not need to be resized while initializing it.
        protected readonly int MultiTenancyInitialCapacity = 101;
        protected readonly int RequestInitialCapacity = 257;
        protected readonly int SubscriptionInitialCapacity = 1009;

        protected int TimeoutMiliseconds = 5000;
        protected readonly int ConcurrencyLevel;

        private readonly object _syncObj = new object();

        public AbpInMemoryPushRequestStore(IGuidGenerator guidGenerator)
        {
            GuidGenerator = guidGenerator;

            // The higher the concurrencyLevel, the higher the theoretical number of operations
            // that could be performed concurrently on the ConcurrentDictionary.  However, global
            // operations like resizing the dictionary take longer as the concurrencyLevel rises. 
            // For the purposes of this example, we'll compromise at numCores * 2.
            int numProcs = Environment.ProcessorCount;
            ConcurrencyLevel = numProcs * 2;

            // above is copied from https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2?view=netcore-2.0

            Requests = new ConcurrentDictionary<Guid, PushRequest>(ConcurrencyLevel, RequestInitialCapacity);
            MultiTenantSubscriptions = new ConcurrentDictionary<int?, ConcurrentDictionary<Guid, PushRequestSubscription>>(ConcurrencyLevel, MultiTenancyInitialCapacity);
        }

        private ConcurrentDictionary<Guid, PushRequestSubscription> CreateSubscriptionCollection()
        {
            return new ConcurrentDictionary<Guid, PushRequestSubscription>(ConcurrencyLevel, SubscriptionInitialCapacity);
        }

        #region Subscriptions

        public virtual Task InsertSubscriptionAsync(PushRequestSubscription subscription)
        {
            var pushSubscriptions = MultiTenantSubscriptions.GetOrAdd(subscription.TenantId, CreateSubscriptionCollection());
            if (pushSubscriptions.ContainsKey(subscription.Id))
            {
                throw new AbpException(string.Format("Subscription {0} already exists", subscription.Id));
            }

            subscription.Id = GuidGenerator.Create();
            if (!pushSubscriptions.TryAdd(subscription.Id, subscription)){
                throw new AbpException(string.Format("Failed to insert subscription {0}:{1}", subscription.PushRequestName, subscription.Id));
            }
            return Task.CompletedTask;
        }

        public virtual Task DeleteSubscriptionAsync(IUserIdentifier user, string pushRequestName, string entityTypeName, string entityId)
        {
            var pushSubscriptions = MultiTenantSubscriptions.GetOrAdd(user.TenantId, CreateSubscriptionCollection());
            var deleteSubscriptions = pushSubscriptions.Where(ps =>
            {
                var subscription = ps.Value;
                return ps.Value.TenantId == user.TenantId &&
                         ps.Value.UserId == user.UserId &&
                         ps.Value.PushRequestName == pushRequestName &&
                         ps.Value.EntityTypeName == entityTypeName &&
                         ps.Value.EntityId == entityId;
            }).Select(ps => ps.Key);

            foreach(var key in deleteSubscriptions){
                pushSubscriptions.TryRemove(key, out PushRequestSubscription subscription);
            }
            return Task.CompletedTask;
        }

        public virtual Task<List<PushRequestSubscription>> GetSubscriptionsAsync(string pushRequestName, string entityTypeName, string entityId, int skipCount = 0, int maxResultCount = int.MaxValue)
        {
            var allPushSubscriptions = MultiTenantSubscriptions.Values;
            var subscriptions = allPushSubscriptions.SelectMany(dict =>
            {
                return dict.Values.Where(ps => ps.PushRequestName == pushRequestName &&
                                         ps.EntityTypeName == entityTypeName &&
                                         ps.EntityId == entityId);
            })
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToList();
            return Task.FromResult(subscriptions);
        }

        public virtual Task<List<PushRequestSubscription>> GetSubscriptionsAsync(int?[] tenantIds, string pushRequestName, string entityTypeName, string entityId, int skipCount = 0, int maxResultCount = int.MaxValue)
        {
            var tenantIdsList = new List<int?>();
            if (tenantIds != null && tenantIds.Length > 0)
            {
                tenantIdsList.AddRange(tenantIds);
            }

            var allPushSubscriptions = MultiTenantSubscriptions
                .Where(ms => tenantIdsList.Contains(ms.Key))
                .Select(ms => ms.Value);
            var subscriptions = allPushSubscriptions.SelectMany(dict =>
            {
                return dict.Values.Where(ps => ps.PushRequestName == pushRequestName &&
                                         ps.EntityTypeName == entityTypeName &&
                                         ps.EntityId == entityId);
            })
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToList();
            return Task.FromResult(subscriptions);
        }

        public virtual Task<List<PushRequestSubscription>> GetSubscriptionsAsync(IUserIdentifier user, int skipCount = 0, int maxResultCount = int.MaxValue)
        {
            var pushSubscriptions = MultiTenantSubscriptions.GetOrAdd(user.TenantId, CreateSubscriptionCollection());
            var subscriptions = pushSubscriptions.Values
                                                 .Skip(skipCount)
                                                 .Take(maxResultCount)
                                                 .ToList();
            return Task.FromResult(subscriptions);
        }

        public virtual Task<bool> IsSubscribedAsync(IUserIdentifier user, string pushRequestName, string entityTypeName, string entityId)
        {
            var pushSubscriptions = MultiTenantSubscriptions.GetOrAdd(user.TenantId, CreateSubscriptionCollection());
            var hasSubscription = pushSubscriptions.Values
                                                   .Any(ps => ps.UserId == user.UserId &&
                                                          ps.PushRequestName == pushRequestName &&
                                                          ps.EntityTypeName == entityTypeName &&
                                                          ps.EntityId == entityId
                                                       );
            return Task.FromResult(hasSubscription);
        }

        #endregion

        public virtual Task DeleteRequestAsync(Guid requestId)
        {
            Requests.TryRemove(requestId, out PushRequest request);
            return Task.CompletedTask;
        }

        public virtual Task InsertRequestAsync(PushRequest request)
        {
            if (Requests.ContainsKey(request.Id))
            {
                throw new AbpException(string.Format("Request {0} already exists", request.Id));
            }

            request.Id = GuidGenerator.Create();
            if (!Requests.TryAdd(request.Id, request))
            {
                throw new AbpException(string.Format("Failed to insert request {0}:{1}", request.Name, request.Id));
            }
            return Task.CompletedTask;
        }

        public virtual Task<PushRequest> GetRequestOrNullAsync(Guid requestId)
        {
            if(Requests.TryGetValue(requestId, out PushRequest request)){
                return Task.FromResult(request);
            }
            return Task.FromResult(null as PushRequest);
        }

        public virtual Task<List<PushRequest>> GetRequestsAsync(PushRequestPriority? priority = null, int skipCount = 0, int maxResultCount = int.MaxValue)
        {
            var requests = Requests.Values
                                   .WhereIf(priority.HasValue, pr => pr.Priority == priority.Value)
                                   .Skip(skipCount)
                                   .Take(maxResultCount)
                                   .ToList();
            return Task.FromResult(requests);
        }

        public virtual Task<int> GethRequestCountAsync(PushRequestPriority? priority = null)
        {
            var requestCount = Requests.Values
                                       .WhereIf(priority.HasValue, pr => pr.Priority == priority.Value)
                                       .Count();
            return Task.FromResult(requestCount);
        }

        public virtual Task UpdateRequestPriorityAsync(Guid requestId, PushRequestPriority priority)
        {
            if (Requests.TryGetValue(requestId, out PushRequest request))
            {
                request.Priority = priority;
                Requests.AddOrUpdate(requestId, request, (key, oldValue) =>
                {
                    oldValue.Priority = request.Priority;
                    return oldValue;
                });
            }
            return Task.CompletedTask;
        }
    }
}