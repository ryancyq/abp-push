using System.Collections.Generic;
using Abp.Domain.Entities;
using Abp.Threading;

namespace Abp.Push.Requests
{
    public static class PushRequestSubscriptionManagerExtensions
    {
        /// <summary>
        /// Subscribes to a push request.
        /// </summary>
        /// <param name="pushRequestSubscriptionManager">Push request subscription manager</param>
        /// <param name="user">User.</param>
        /// <param name="pushRequestName">Name of the push request.</param>
        /// <param name="entityIdentifier">entity identifier</param>
        public static void Subscribe(this IPushRequestSubscriptionManager pushRequestSubscriptionManager, IUserIdentifier user, string pushRequestName, EntityIdentifier entityIdentifier = null)
        {
            AsyncHelper.RunSync(() => pushRequestSubscriptionManager.SubscribeAsync(user, pushRequestName, entityIdentifier));
        }

        /// <summary>
        /// Subscribes to all available push requests for given user.
        /// It does not subscribe entity related push requests.
        /// </summary>
        /// <param name="pushRequestSubscriptionManager">Push request subscription manager</param>
        /// <param name="user">User.</param>
        public static void SubscribeToAllAvailablePushRequets(this IPushRequestSubscriptionManager pushRequestSubscriptionManager, UserIdentifier user)
        {
            AsyncHelper.RunSync(() => pushRequestSubscriptionManager.SubscribeToAllAvailablePushRequetsAsync(user));
        }

        /// <summary>
        /// Unsubscribes from a push request.
        /// </summary>
        /// <param name="pushRequestSubscriptionManager">Push request subscription manager</param>
        /// <param name="user">User.</param>
        /// <param name="pushRequestName">Name of the push request.</param>
        /// <param name="entityIdentifier">entity identifier</param>
        public static void Unsubscribe(this IPushRequestSubscriptionManager pushRequestSubscriptionManager, UserIdentifier user, string pushRequestName, EntityIdentifier entityIdentifier = null)
        {
            AsyncHelper.RunSync(() => pushRequestSubscriptionManager.UnsubscribeAsync(user, pushRequestName, entityIdentifier));
        }

        /// <summary>
        /// Gets all subscribtions for given push request.
        /// Can work only for single database approach
        /// </summary>
        /// <param name="pushRequestSubscriptionManager">Push request subscription manager</param>
        /// <param name="pushRequestName">Name of the push request.</param>
        /// <param name="entityIdentifier">entity identifier</param>
        public static List<PushRequestSubscription> GetSubscriptions(this IPushRequestSubscriptionManager pushRequestSubscriptionManager, string pushRequestName, EntityIdentifier entityIdentifier = null, int skipCount = 0, int maxResultCount = int.MaxValue)
        {
            return AsyncHelper.RunSync(() => pushRequestSubscriptionManager.GetSubscriptionsAsync(pushRequestName, entityIdentifier, skipCount, maxResultCount));
        }

        /// <summary>
        /// Gets all subscribtions for given push request.
        /// </summary>
        /// <param name="pushRequestSubscriptionManager">Push request subscription manager</param>
        /// <param name="tenantId">Tenant id. Null for the host.</param>
        /// <param name="pushRequestName">Name of the push request.</param>
        /// <param name="entityIdentifier">entity identifier</param>
        public static List<PushRequestSubscription> GetSubscriptions(this IPushRequestSubscriptionManager pushRequestSubscriptionManager, int? tenantId, string pushRequestName, EntityIdentifier entityIdentifier = null, int skipCount = 0, int maxResultCount = int.MaxValue)
        {
            return AsyncHelper.RunSync(() => pushRequestSubscriptionManager.GetSubscriptionsAsync(tenantId, pushRequestName, entityIdentifier, skipCount, maxResultCount));
        }

        /// <summary>
        /// Gets subscribed push requests for a user.
        /// </summary>
        /// <param name="pushRequestSubscriptionManager">Push request subscription manager</param>
        /// <param name="user">User.</param>
        public static List<PushRequestSubscription> GetSubscribedPushRequests(this IPushRequestSubscriptionManager pushRequestSubscriptionManager, IUserIdentifier user, int skipCount = 0, int maxResultCount = int.MaxValue)
        {
            return AsyncHelper.RunSync(() => pushRequestSubscriptionManager.GetSubscribedPushRequestsAsync(user, skipCount, maxResultCount));
        }

        /// <summary>
        /// Checks if a user subscribed for a push request.
        /// </summary>
        /// <param name="pushRequestSubscriptionManager">Push request subscription manager</param>
        /// <param name="user">User.</param>
        /// <param name="pushRequestName">Name of the push request.</param>
        /// <param name="entityIdentifier">entity identifier</param>
        public static bool IsSubscribed(this IPushRequestSubscriptionManager pushRequestSubscriptionManager, IUserIdentifier user, string pushRequestName, EntityIdentifier entityIdentifier = null)
        {
            return AsyncHelper.RunSync(() => pushRequestSubscriptionManager.IsSubscribedAsync(user, pushRequestName, entityIdentifier));
        }
    }
}