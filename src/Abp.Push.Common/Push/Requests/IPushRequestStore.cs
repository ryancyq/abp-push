using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abp.Push.Requests
{
    public interface IPushRequestStore
    {
        /// <summary>
        /// Inserts a push request subscription.
        /// </summary>
        Task InsertSubscriptionAsync(PushRequestSubscription subscription);

        /// <summary>
        /// Deletes a push request subscription.
        /// </summary>
        Task DeleteSubscriptionAsync(IUserIdentifier user, string pushRequestName, string entityTypeName, string entityId);

        /// <summary>
        /// Gets subscriptions for a push request.
        /// </summary>
        Task<List<PushRequestSubscription>> GetSubscriptionsAsync(string pushRequestName, string entityTypeName, string entityId);

        /// <summary>
        /// Gets subscriptions for a push request for specified tenant(s).
        /// </summary>
        Task<List<PushRequestSubscription>> GetSubscriptionsAsync(int?[] tenantIds, string pushRequestName, string entityTypeName, string entityId);

        /// <summary>
        /// Gets subscriptions for a user.
        /// </summary>
        Task<List<PushRequestSubscription>> GetSubscriptionsAsync(IUserIdentifier user);

        /// <summary>
        /// Checks if a user subscribed for a push request
        /// </summary>
        Task<bool> IsSubscribedAsync(IUserIdentifier user, string pushRequestName, string entityTypeName, string entityId);

        /// <summary>
        /// Inserts a push request.
        /// </summary>
        /// <param name="request">The push request.</param>
        Task InsertRequestAsync(PushRequest request);

        /// <summary>
        /// Gets a push request.
        /// </summary>
        /// <param name="requestId">The push request id.</param>
        Task<PushRequest> GetRequestOrNullAsync(Guid requestId);

        /// <summary>
        /// Gets push requests.
        /// </summary>
        /// <param name="user">User.</param>
        /// <param name="priority">Push request priority</param>
        Task<List<PushRequest>> GetRequestsAsync(IUserIdentifier user, PushRequestPriority? priority = null);

        /// <summary>
        /// Gets push request count.
        /// </summary>
        /// <param name="user">User.</param>
        /// <param name="priority">Push request priority</param>
        Task<int> GethRequestCountAsync(IUserIdentifier user, PushRequestPriority? priority = null);

        /// <summary>
        /// Deletes a push request.
        /// </summary>
        /// <param name="requestId">The push request id.</param>
        Task DeleteRequestAsync(Guid requestId);

        /// <summary>
        /// Updates a push request priority.
        /// </summary>
        Task UpdateRequestPriorityAsync(int? tenantId, Guid pushRequestId, PushRequestPriority priority);

        /// <summary>
        /// Updates all push request priorities for a user.
        /// </summary>
        Task UpdateAllRequestPrioritiesAsync(IUserIdentifier user, PushRequestPriority priority);
    }
}
