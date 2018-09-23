using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace Abp.Push.Requests
{
    /// <summary>
    /// Used to manage subscriptions for push requests.
    /// </summary>
    public interface IPushRequestSubscriptionManager
    {
        /// <summary>
        /// Subscribes to a push request for given user and push request informations.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="pushRequestName">Name of the push request.</param>
        /// <param name="entityIdentifier">entity identifier</param>
        Task SubscribeAsync(IUserIdentifier user, string pushRequestName, EntityIdentifier entityIdentifier = null);

        /// <summary>
        /// Subscribes to all available push request for given user.
        /// It does not subscribe entity related push request.
        /// </summary>
        /// <param name="user">User.</param>
        Task SubscribeToAllAvailablePushRequetsAsync(UserIdentifier user);

        /// <summary>
        /// Unsubscribes from a push request.
        /// </summary>
        /// <param name="user">User.</param>
        /// <param name="pushRequestName">Name of the push request.</param>
        /// <param name="entityIdentifier">entity identifier</param>
        Task UnsubscribeAsync(IUserIdentifier user, string pushRequestName, EntityIdentifier entityIdentifier = null);

        /// <summary>
        /// Gets all subscribtions for given push request (including all tenants).
        /// This only works for single database approach in a multitenant application!
        /// </summary>
        /// <param name="pushRequestName">Name of the push request.</param>
        /// <param name="entityIdentifier">entity identifier</param>
        Task<List<PushRequestSubscription>> GetSubscriptionsAsync(string pushRequestName, EntityIdentifier entityIdentifier = null, int skipCount = 0, int maxResultCount = int.MaxValue);

        /// <summary>
        /// Gets all subscribtions for given push request.
        /// </summary>
        /// <param name="tenantId">Tenant id. Null for the host.</param>
        /// <param name="pushRequestName">Name of the push request.</param>
        /// <param name="entityIdentifier">entity identifier</param>
        Task<List<PushRequestSubscription>> GetSubscriptionsAsync(int? tenantId, string pushRequestName, EntityIdentifier entityIdentifier = null, int skipCount = 0, int maxResultCount = int.MaxValue);

        /// <summary>
        /// Gets subscribed push requests for a user.
        /// </summary>
        /// <param name="user">User.</param>
        Task<List<PushRequestSubscription>> GetSubscribedPushRequestsAsync(IUserIdentifier user, int skipCount = 0, int maxResultCount = int.MaxValue);

        /// <summary>
        /// Checks if a user subscribed for a push request.
        /// </summary>
        /// <param name="user">User.</param>
        /// <param name="pushRequestName">Name of the push request.</param>
        /// <param name="entityIdentifier">entity identifier</param>
        Task<bool> IsSubscribedAsync(IUserIdentifier user, string pushRequestName, EntityIdentifier entityIdentifier = null);
    }
}
