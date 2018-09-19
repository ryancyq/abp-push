using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abp.Push.Requests
{
    /// <summary>
    /// Null pattern implementation of <see cref="IPushRequestStore"/>.
    /// </summary>
    public class NullPushRequestStore : IPushRequestStore
    {
        public Task InsertSubscriptionAsync(PushRequestSubscription subscription)
        {
            return Task.FromResult(0);
        }

        public Task DeleteSubscriptionAsync(IUserIdentifier user, string pushRequestName, string entityTypeName, string entityId)
        {
            return Task.FromResult(0);
        }

        public Task<List<PushRequestSubscription>> GetSubscriptionsAsync(string pushRequestName, string entityTypeName, string entityId)
        {
            return Task.FromResult(new List<PushRequestSubscription>());
        }

        public Task<List<PushRequestSubscription>> GetSubscriptionsAsync(int?[] tenantIds, string pushRequestName, string entityTypeName, string entityId)
        {
            return Task.FromResult(new List<PushRequestSubscription>());
        }

        public Task<List<PushRequestSubscription>> GetSubscriptionsAsync(IUserIdentifier user)
        {
            return Task.FromResult(new List<PushRequestSubscription>());
        }

        public Task<bool> IsSubscribedAsync(IUserIdentifier user, string pushRequestName, string entityTypeName, string entityId)
        {
            return Task.FromResult(false);
        }

        public Task DeleteRequestAsync(Guid requestId)
        {
            return Task.FromResult(0);
        }

        public Task InsertRequestAsync(PushRequest request)
        {
            return Task.FromResult(0);
        }

        public Task<PushRequest> GetRequestOrNullAsync(Guid requestId)
        {
            return Task.FromResult(null as PushRequest);
        }

        public Task<List<PushRequest>> GetRequestsAsync(IUserIdentifier user, PushRequestPriority? priority = null)
        {
            throw new NotImplementedException();
        }

        public Task<int> GethRequestCountAsync(IUserIdentifier user, PushRequestPriority? priority = null)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAllRequestPrioritiesAsync(IUserIdentifier user, PushRequestPriority priority)
        {
            return Task.FromResult(0);
        }

        public Task UpdateRequestPriorityAsync(int? tenantId, Guid pushRequestId, PushRequestPriority priority)
        {
            return Task.FromResult(0);
        }
    }
}