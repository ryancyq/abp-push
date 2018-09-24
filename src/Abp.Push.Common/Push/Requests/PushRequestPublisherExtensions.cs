using Abp.Domain.Entities;
using Abp.Threading;

namespace Abp.Push.Requests
{
    public static class PushRequestPublisherExtensions
    {
        /// <summary>
        /// Publishes a new push request.
        /// </summary>
        /// <param name="pushRequestPublisher">Push Request publisher</param>
        /// <param name="pushRequestName">Unique push request name</param>
        /// <param name="data">Push request data (optional)</param>
        /// <param name="entityIdentifier">The entity identifier if this push request is related to an entity</param>
        /// <param name="priority">Push request priority</param>
        /// <param name="userIds">Target user id(s). Used to send push request to specific user(s). If this is null/empty, the notification is sent to all subscribed users</param>
        public static void Publish(this IPushRequestPublisher pushRequestPublisher, string pushRequestName, PushRequestData data = null, EntityIdentifier entityIdentifier = null, PushRequestPriority priority = PushRequestPriority.Normal, IUserIdentifier[] userIds = null)
        {
            AsyncHelper.RunSync(() => pushRequestPublisher.PublishAsync(pushRequestName, data, entityIdentifier, priority, userIds));
        }
    }
}