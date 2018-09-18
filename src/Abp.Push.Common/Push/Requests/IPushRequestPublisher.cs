using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace Abp.Push.Requests
{
    /// <summary>
    /// Used to publish push request.
    /// </summary>
    public interface IPushRequestPublisher
    {
        /// <summary>
        /// Publishes a new push request.
        /// </summary>
        /// <param name="pushRequestName">Unique push request name</param>
        /// <param name="data">Push Request data (optional)</param>
        /// <param name="entityIdentifier">The entity identifier if this request is related to an entity</param>
        /// <param name="priority">The push request priority</param>
        /// <param name="userIds">
        /// Target user id(s). 
        /// Used to send push request to specific user(s) (without checking the subscription). 
        /// If this is null/empty, the request is sent to subscribed users.
        /// </param>
        /// <param name="excludedUserIds">
        /// Excluded user id(s).
        /// This can be set to exclude some users while publishing requests to subscribed users.
        /// It's normally not set if <see cref="userIds"/> is set.
        /// </param>
        /// <param name="tenantIds">
        /// Target tenant id(s).
        /// Used to send push request to subscribed users of specific tenant(s).
        /// This should not be set if <see cref="userIds"/> is set.
        /// <see cref="PushRequestPublisher.AllTenants"/> can be passed to indicate all tenants.
        /// But this can only work in a single database approach (all tenants are stored in host database).
        /// If this is null, then it's automatically set to the current tenant on <see cref="IAbpSession.TenantId"/>. 
        /// </param>
        Task PublishAsync(
            string pushRequestName,
            PushRequestData data = null,
            EntityIdentifier entityIdentifier = null,
            PushRequestPriority priority = PushRequestPriority.Normal,
            UserIdentifier[] userIds = null,
            UserIdentifier[] excludedUserIds = null,
            int?[] tenantIds = null);
    }
}