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

        public Task<List<PushRequestSubscription>> GetSubscriptionsAsync(string pushRequestName, string entityTypeName, string entityId, int skipCount = 0, int maxResultCount = int.MaxValue)
        {
            return Task.FromResult(new List<PushRequestSubscription>());
        }

        public Task<List<PushRequestSubscription>> GetSubscriptionsAsync(int?[] tenantIds, string pushRequestName, string entityTypeName, string entityId, int skipCount = 0, int maxResultCount = int.MaxValue)
        {
            return Task.FromResult(new List<PushRequestSubscription>());
        }

        public Task<List<PushRequestSubscription>> GetSubscriptionsAsync(IUserIdentifier user, int skipCount = 0, int maxResultCount = int.MaxValue)
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

        public Task<List<PushRequest>> GetRequestsAsync(PushRequestPriority? priority = null, int skipCount = 0, int maxResultCount = int.MaxValue)
        {
            return Task.FromResult(new List<PushRequest>());
        }

        public Task<int> GethRequestCountAsync(PushRequestPriority? priority = null)
        {
            return Task.FromResult(0);
        }

        public Task UpdateRequestPriorityAsync(Guid pushRequestId, PushRequestPriority priority)
        {
            return Task.FromResult(0);
        }
    }
}