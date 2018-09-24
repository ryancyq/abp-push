using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Json;
using Abp.Push.Configurations;
using Abp.Runtime.Session;

namespace Abp.Push.Requests
{
    /// <summary>
    /// Implements <see cref="IPushRequestPublisher"/>.
    /// </summary>
    public class AbpPushRequestPublisher : AbpServiceBase, IPushRequestPublisher, ITransientDependency
    {
        public int MaxUserCountToDirectlyDistributeARequest { get; protected set; } = 5;

        /// <summary>
        /// Reference to ABP session.
        /// </summary>
        public IAbpSession AbpSession { get; set; }

        protected readonly IPushRequestStore RequestStore;
        protected readonly IBackgroundJobManager BackgroundJobManager;
        protected readonly IPushRequestDistributer RequestDistributer;
        protected readonly IPushConfiguration Configuration;
        protected readonly IGuidGenerator GuidGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbpPushRequestPublisher"/> class.
        /// </summary>
        public AbpPushRequestPublisher(
            IPushRequestStore pushRequestStore,
            IBackgroundJobManager backgroundJobManager,
            IPushRequestDistributer pushRequestDistributer,
            IPushConfiguration pushConfiguration,
            IGuidGenerator guidGenerator
        )
        {
            RequestStore = pushRequestStore;
            BackgroundJobManager = backgroundJobManager;
            RequestDistributer = pushRequestDistributer;
            Configuration = pushConfiguration;
            GuidGenerator = guidGenerator;

            AbpSession = NullAbpSession.Instance;
        }

        [UnitOfWork]
        public virtual async Task PublishAsync(
            string pushRequestName,
            PushRequestData data = null,
            EntityIdentifier entityIdentifier = null,
            PushRequestPriority priority = PushRequestPriority.Normal,
            IUserIdentifier[] userIds = null,
            IUserIdentifier[] excludedUserIds = null,
            int?[] tenantIds = null)
        {
            if (pushRequestName.IsNullOrEmpty())
            {
                throw new ArgumentException("PushRequestName can not be null or whitespace!", nameof(pushRequestName));
            }

            if (!tenantIds.IsNullOrEmpty() && !userIds.IsNullOrEmpty())
            {
                throw new ArgumentException("tenantIds can be set only if userIds is not set!", nameof(tenantIds));
            }

            if (tenantIds.IsNullOrEmpty() && userIds.IsNullOrEmpty())
            {
                tenantIds = new[] { AbpSession.TenantId };
            }

            var pushRequest = new PushRequest(GuidGenerator.Create())
            {
                Name = pushRequestName,
                EntityTypeName = entityIdentifier?.Type.FullName,
                EntityTypeAssemblyQualifiedName = entityIdentifier?.Type.AssemblyQualifiedName,
                EntityId = entityIdentifier?.Id.ToJsonString(),
                Priority = priority,
                UserIds = userIds.IsNullOrEmpty() ? null : userIds.Select(uid => uid.ToUserIdentifier().ToUserIdentifierString()).JoinAsString(","),
                ExcludedUserIds = excludedUserIds.IsNullOrEmpty() ? null : excludedUserIds.Select(uid => uid.ToUserIdentifier().ToUserIdentifierString()).JoinAsString(","),
                TenantIds = PushRequest.ToTenantIds(tenantIds),
                Data = data?.ToJsonString(),
                DataTypeName = data?.GetType().AssemblyQualifiedName
            };

            await RequestStore.InsertRequestAsync(pushRequest);

            await CurrentUnitOfWork.SaveChangesAsync(); //To get Id of the push request

            if (userIds != null && userIds.Length <= Configuration.MaxUserCountForForegroundDistribution)
            {
                //We can directly distribute the push request since there are not much receivers
                await RequestDistributer.DistributeAsync(pushRequest.Id);
            }
            else
            {
                //We enqueue a background job since distributing may get a long time
                await BackgroundJobManager.EnqueueAsync<PushRequestDistributionJob, PushRequestDistributionJobArgs>(
                    new PushRequestDistributionJobArgs(
                        pushRequest.Id
                        )
                    );
            }
        }
    }
}