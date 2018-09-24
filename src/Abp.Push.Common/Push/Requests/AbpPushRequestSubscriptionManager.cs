using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Json;

namespace Abp.Push.Requests
{
    /// <summary>
    /// Implements <see cref="IPushRequestSubscriptionManager"/>.
    /// </summary>
    public class AbpPushRequestSubscriptionManager : IPushRequestSubscriptionManager, ITransientDependency
    {
        protected readonly IPushRequestStore RequestStore;
        protected readonly IPushDefinitionManager DefinitionManager;
        protected readonly IGuidGenerator GuidGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbpPushRequestSubscriptionManager"/> class.
        /// </summary>
        public AbpPushRequestSubscriptionManager(
            IPushRequestStore pushRequestStore,
            IPushDefinitionManager pushDefinitionManager,
            IGuidGenerator guidGenerator)
        {
            RequestStore = pushRequestStore;
            DefinitionManager = pushDefinitionManager;
            GuidGenerator = guidGenerator;
        }

        public virtual async Task SubscribeAsync(IUserIdentifier user, string pushRequestName, EntityIdentifier entityIdentifier = null)
        {
            if (await IsSubscribedAsync(user, pushRequestName, entityIdentifier))
            {
                return;
            }

            await RequestStore.InsertSubscriptionAsync(
                new PushRequestSubscription(
                    GuidGenerator.Create(),
                    user.TenantId,
                    user.UserId,
                    pushRequestName,
                    entityIdentifier
                    )
                );
        }

        public virtual async Task SubscribeToAllAvailablePushRequestsAsync(IUserIdentifier user)
        {
            var pushRequestDefinitions = (await DefinitionManager
                .GetAllAvailableAsync(user))
                .Where(nd => nd.EntityType == null)
                .ToList();

            foreach (var notificationDefinition in pushRequestDefinitions)
            {
                await SubscribeAsync(user, notificationDefinition.Name);
            }
        }

        public virtual async Task UnsubscribeAsync(IUserIdentifier user, string pushRequestName, EntityIdentifier entityIdentifier = null)
        {
            await RequestStore.DeleteSubscriptionAsync(
                user,
                pushRequestName,
                entityIdentifier?.Type.FullName,
                entityIdentifier?.Id.ToJsonString()
                );
        }

        // Can work only for single database approach
        public virtual async Task<List<PushRequestSubscription>> GetSubscriptionsAsync(string pushRequestName, EntityIdentifier entityIdentifier = null, int skipCount = 0, int maxResultCount = int.MaxValue)
        {
            return await RequestStore.GetSubscriptionsAsync(pushRequestName,
                                                            entityIdentifier?.Type.FullName,
                                                            entityIdentifier?.Id.ToJsonString(),
                                                            skipCount: skipCount,
                                                            maxResultCount: maxResultCount
                                                            );
        }

        public virtual async Task<List<PushRequestSubscription>> GetSubscriptionsAsync(int? tenantId, string pushRequestName, EntityIdentifier entityIdentifier = null, int skipCount = 0, int maxResultCount = int.MaxValue)
        {
            return await RequestStore.GetSubscriptionsAsync(new[] { tenantId },
                                                            pushRequestName,
                                                            entityIdentifier?.Type.FullName,
                                                            entityIdentifier?.Id.ToJsonString(),
                                                            skipCount: skipCount,
                                                            maxResultCount: maxResultCount
                                                            );
        }

        public virtual async Task<List<PushRequestSubscription>> GetSubscribedPushRequestsAsync(IUserIdentifier user, int skipCount = 0, int maxResultCount = int.MaxValue)
        {
            return await RequestStore.GetSubscriptionsAsync(user,
                                                            skipCount: skipCount,
                                                            maxResultCount: maxResultCount);
        }

        public virtual Task<bool> IsSubscribedAsync(IUserIdentifier user, string pushRequestName, EntityIdentifier entityIdentifier = null)
        {
            return RequestStore.IsSubscribedAsync(
                user,
                pushRequestName,
                entityIdentifier?.Type.FullName,
                entityIdentifier?.Id.ToJsonString()
                );
        }
    }
}