using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Push.Configuration;
using Abp.Push.Providers;

namespace Abp.Push.Requests
{
    /// <summary>
    /// Used to distribute push requests to users.
    /// </summary>
    public class AbpPushRequestDistributor : AbpServiceBase, IPushRequestDistributor, ITransientDependency
    {
        protected readonly IPushRequestStore RequestStore;
        protected readonly IPushDefinitionManager DefinitionManager;
        protected readonly IAbpPushConfiguration Configuration;
        protected readonly IIocResolver IocResolver;
        protected readonly IGuidGenerator GuidGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbpPushRequestDistributor"/> class.
        /// </summary>
        public AbpPushRequestDistributor(
            IPushRequestStore pushRequestStore,
            IPushDefinitionManager pushDefinitionManager,
            IAbpPushConfiguration pushConfiguration,
            IIocResolver iocResolver,
            IGuidGenerator guidGenerator)
        {
            RequestStore = pushRequestStore;
            DefinitionManager = pushDefinitionManager;
            Configuration = pushConfiguration;
            IocResolver = iocResolver;
            GuidGenerator = guidGenerator;
        }

        [UnitOfWork]
        public virtual async Task DistributeAsync(Guid pushRequestId)
        {
            PushRequest pushRequest = null;
            // set tenantId = null, because push request only store in host side
            using (UnitOfWorkManager.Current.SetTenantId(null))
            {
                pushRequest = await RequestStore.GetRequestOrNullAsync(pushRequestId);
                if (pushRequest == null)
                {
                    Logger.WarnFormat("PushRequestDistributionJob can not continue since could not found push request by id: {0} ", pushRequestId);
                    return;
                }
            }

            // TODO: change GetUsers() to GetDevices()
            var users = await GetUsers(pushRequest);
            if (users.IsNullOrEmpty())
            {
                Logger.WarnFormat("Push Request with id: {0} does not have any target user", pushRequest.Id);
            }

            try
            {
                foreach (var providerInfo in Configuration.ServiceProviders)
                {
                    // TODO: allow PushRequest to store target providers
                    using (var provider = CreateProvider(providerInfo.Name))
                    {
                        await provider.Object.PushAsync(users, pushRequest);
                    }
                }

                await RequestStore.DeleteRequestAsync(pushRequest.Id);
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.ToString(), ex);
            }
        }

        [UnitOfWork]
        protected virtual async Task<UserIdentifier[]> GetUsers(PushRequest request)
        {
            var userIds = new List<UserIdentifier>();

            if (request.UserIds.IsNullOrEmpty())
            {
                //Get subscribed users

                List<PushRequestSubscription> subscriptions;

                if (request.TenantIds.IsNullOrWhiteSpace() ||
                    request.TenantIds.Trim() == PushRequest.AllTenantIdsString)
                {
                    //Get all subscribed users of all tenants
                    subscriptions = await RequestStore.GetSubscriptionsAsync(
                        request.Name,
                        request.EntityTypeName,
                        request.EntityId
                        );
                }
                else
                {
                    var tenantIds = PushRequest.GetTenantIds(request.TenantIds);

                    //Get all subscribed users of specified tenant(s)
                    subscriptions = await RequestStore.GetSubscriptionsAsync(
                        tenantIds,
                        request.Name,
                        request.EntityTypeName,
                        request.EntityId
                        );
                }

                //Remove invalid subscriptions
                var invalidSubscriptions = new Dictionary<Guid, PushRequestSubscription>();

                var subscriptionGroups = subscriptions.GroupBy(s => s.TenantId);
                foreach (var subscriptionGroup in subscriptionGroups)
                {
                    using (CurrentUnitOfWork.SetTenantId(subscriptionGroup.Key))
                    {
                        foreach (var subscription in subscriptionGroup)
                        {
                            if (!await DefinitionManager.IsAvailableAsync(request.Name, new UserIdentifier(subscription.TenantId, subscription.UserId)) ||
                                // TODO: exclude system push request from checking user setting
                                !SettingManager.GetSettingValueForUser<bool>(AbpPushSettingNames.Receive, subscription.TenantId, subscription.UserId))
                            {
                                invalidSubscriptions[subscription.Id] = subscription;
                            }
                        }
                    }
                }

                subscriptions.RemoveAll(s => invalidSubscriptions.ContainsKey(s.Id));

                //Get user ids
                userIds = subscriptions
                    .Select(s => new UserIdentifier(s.TenantId, s.UserId))
                    .ToList();
            }
            else
            {
                //Directly get from UserIds
                userIds = request
                    .UserIds
                    .Split(",")
                    .Select(uidAsStr => UserIdentifier.Parse(uidAsStr))
                    // TODO: exclude system push request from checking user setting
                    .Where(uid => SettingManager.GetSettingValueForUser<bool>(AbpPushSettingNames.Receive, uid.TenantId, uid.UserId))
                    .ToList();
            }

            if (!request.ExcludedUserIds.IsNullOrEmpty())
            {
                //Exclude specified users.
                var excludedUserIds = request
                    .ExcludedUserIds
                    .Split(",")
                    .Select(uidAsStr => UserIdentifier.Parse(uidAsStr))
                    .ToList();

                userIds.RemoveAll(uid => excludedUserIds.Any(euid => euid.Equals(uid)));
            }

            return userIds.ToArray();
        }

        public virtual IDisposableDependencyObjectWrapper<IPushServiceProvider> CreateProvider(string provider)
        {
            var providerInfo = Configuration.ServiceProviders.FirstOrDefault(s => s.Name == provider);
            if (providerInfo == null)
            {
                throw new Exception("Unknown push provider: " + provider);
            }

            var pushProvider = IocResolver.ResolveAsDisposable<IPushServiceProvider>(providerInfo.ProviderType);
            pushProvider.Object.Initialize(providerInfo);
            return pushProvider;
        }
    }
}
