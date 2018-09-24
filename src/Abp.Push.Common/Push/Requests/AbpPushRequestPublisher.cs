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
        /// Indicates all tenants.
        /// </summary>
        public static int[] AllTenants
        {
            get
            {
                return new[] { PushRequest.AllTenantIds.To<int>() };
            }
        }

        /// <summary>
        /// Reference to ABP session.
        /// </summary>
        public IAbpSession AbpSession { get; set; }

        private readonly IPushRequestStore _pushRequestStore;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IPushRequestDistributer _pushRequestDistributer;
        private readonly IGuidGenerator _guidGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbpPushRequestPublisher"/> class.
        /// </summary>
        public AbpPushRequestPublisher(
            IPushRequestStore pushRequestStore,
            IBackgroundJobManager backgroundJobManager,
            IPushRequestDistributer pushRequestDistributer,
            IGuidGenerator guidGenerator
        )
        {
            _pushRequestStore = pushRequestStore;
            _backgroundJobManager = backgroundJobManager;
            _pushRequestDistributer = pushRequestDistributer;
            _guidGenerator = guidGenerator;

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

            var pushRequest = new PushRequest(_guidGenerator.Create())
            {
                Name = pushRequestName,
                EntityTypeName = entityIdentifier?.Type.FullName,
                EntityTypeAssemblyQualifiedName = entityIdentifier?.Type.AssemblyQualifiedName,
                EntityId = entityIdentifier?.Id.ToJsonString(),
                Priority = priority,
                UserIds = userIds.IsNullOrEmpty() ? null : userIds.Select(uid => uid.ToUserIdentifier().ToUserIdentifierString()).JoinAsString(","),
                ExcludedUserIds = excludedUserIds.IsNullOrEmpty() ? null : excludedUserIds.Select(uid => uid.ToUserIdentifier().ToUserIdentifierString()).JoinAsString(","),
                TenantIds = tenantIds.IsNullOrEmpty() ? null : tenantIds.JoinAsString(","),
                Data = data?.ToJsonString(),
                DataTypeName = data?.GetType().AssemblyQualifiedName
            };

            await _pushRequestStore.InsertRequestAsync(pushRequest);

            await CurrentUnitOfWork.SaveChangesAsync(); //To get Id of the notification

            if (userIds != null && userIds.Length <= MaxUserCountToDirectlyDistributeARequest)
            {
                //We can directly distribute the notification since there are not much receivers
                await _pushRequestDistributer.DistributeAsync(pushRequest.Id);
            }
            else
            {
                //We enqueue a background job since distributing may get a long time
                await _backgroundJobManager.EnqueueAsync<PushRequestDistributionJob, PushRequestDistributionJobArgs>(
                    new PushRequestDistributionJobArgs(
                        pushRequest.Id
                        )
                    );
            }
        }
    }
}