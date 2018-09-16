using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Json;
using Abp.Push.Configurations;
using Abp.UI;
using Newtonsoft.Json;

namespace Abp.Push.Devices
{
    public abstract class AbpPushDeviceManager<TDevice> : PushDeviceManager<TDevice>, IAbpPushDeviceManager<TDevice>
        where TDevice : AbpPushDevice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbpPushDeviceManager{TDevice}"/> class.
        /// </summary>
        protected AbpPushDeviceManager(
            IRepository<TDevice, long> entityRepository,
            IPushConfiguration pushConfiguration,
            IGuidGenerator guidGenerator
        ) : base(
            entityRepository,
            pushConfiguration,
            guidGenerator
        )
        {
        }

        /// <summary>
        /// Gets all push device by user identifier.
        /// </summary>
        public Task<IReadOnlyList<TDevice>> GetAllByUserIdAsync(IUserIdentifier user, int? skipCount = null, int? maxResultCount = null){

        }

        /// <summary>
        /// Gets all push devices by user id and provider.
        /// </summary>
        Task<IReadOnlyList<TDevice>> GetAllByUserIdProviderAsync(IUserIdentifier user, string provider, int? skipCount = null, int? maxResultCount = null);

        /// <summary>
        /// Gets total count of all push devices by user id.
        /// </summary>
        Task<int> GetDeviceCountByUserIdAsync(IUserIdentifier user);

        /// <summary>
        /// Gets total count of all push devices by user id and provider.
        /// </summary>
        Task<int> GetDeviceCountByUserIdProviderAsync(IUserIdentifier user, string provider);

        /// <summary>
        /// Removes all push devices by user identifier.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        Task RemoveAllByUserIdAsync(IUserIdentifier userIdentifier);

        [UnitOfWork]
        protected virtual TDevice GetByUserIdProviderOrNull(int? tenantId, long userId, string provider, string providerKey)
        {
            var query = GetQuery();
            query = ApplyUserId(query, tenantId, userId);
            query = ApplyProviderIdentity(query, provider, providerKey);
            return query.FirstOrDefault();
        }

        protected virtual IQueryable<PushClientInfo> ApplyUserId(IQueryable<PushClientInfo> query, int? tenantId, long userId)
        {
            return query.Where(pl => pl.TenantId == tenantId && pl.UserId == userId);
        }

        protected virtual IQueryable<PushClientInfo> ApplyProvider(IQueryable<PushClientInfo> query, string provider)
        {
            return query.Where(pl => pl.Provider.ToUpperInvariant() == provider.ToUpperInvariant());
        }

        protected virtual IQueryable<PushClientInfo> ApplyProviderIdentity(IQueryable<PushClientInfo> query, string provider, string providerKey)
        {
            return ApplyProvider(query, provider).Where(pl => pl.ProviderKey == providerKey);
        }

        protected virtual IQueryable<PushClientInfo> ApplySkipTake(IQueryable<PushClientInfo> query, int? skipCount, int? maxResultCount)
        {
            query.OrderByDescending(e => e.Id);

            if (skipCount.HasValue)
            {
                query = query.Skip(skipCount.Value);
            }

            if (maxResultCount.HasValue)
            {
                query = query.Take(maxResultCount.Value);
            }

            return query;
        }

        /// <summary>
        /// Gets all push clients.
        /// </summary>
        [UnitOfWork]
        public virtual IReadOnlyList<IPushClient> GetAllClients(int? skipCount = null, int? maxResultCount = null)
        {
            var query = GetQuery();
            query = ApplySkipTake(query, skipCount, maxResultCount);

            var clients = query.ToList().Select(MapToPushClient);
            return clients.ToImmutableList();
        }

        [CanBeNull]
        protected virtual IPushClient MapToPushClient([CanBeNull] PushClientInfo pushClientInfo)
        {
            if (pushClientInfo == null)
            {
                return null;
            }

            var providerInfo = _externalPushConfiguration.Providers.FirstOrDefault(p => p.Name.ToUpperInvariant() == pushClientInfo.Provider.ToUpperInvariant());
            var localizedProvider = L("Push.Provider." + (providerInfo == null ? "Invalid" : providerInfo.Name));
            return new PushClient
            {
                Platform = pushClientInfo.Platform,
                DeviceId = pushClientInfo.DeviceId,
                NormalizedDeviceId = pushClientInfo.NormalizedDeviceId,
                DeviceName = pushClientInfo.DeviceName,
                NormalizedDeviceName = pushClientInfo.NormalizedDeviceName,
                Provider = localizedProvider,
                ProviderKey = pushClientInfo.ProviderKey,
                TenantId = pushClientInfo.TenantId,
                UserId = pushClientInfo.UserId,
                Data = pushClientInfo.Data.IsNullOrEmpty() ? null :
                    JsonConvert.DeserializeObject(pushClientInfo.Data, Type.GetType(pushClientInfo.DataTypeName)) as PushClientData,
                DataTypeName = pushClientInfo.DataTypeName
            };
        }

        /// <summary>
        /// Gets all push clients by user id.
        /// </summary>
        [NotNull]
        public virtual IReadOnlyList<IPushClient> GetAllByUserId([NotNull] IUserIdentifier user, int? skipCount = null, int? maxResultCount = null)
        {
            Check.NotNull(user, nameof(user));

            var query = GetQuery();
            query = ApplyUserId(query, user.TenantId, user.UserId);
            query = ApplySkipTake(query, skipCount, maxResultCount);

            var clients = query.ToList().Select(MapToPushClient);
            return clients.ToImmutableList();
        }

        /// <summary>
        /// Gets all push clients by user id and provider.
        /// </summary>
        [NotNull]
        public virtual IReadOnlyList<IPushClient> GetAllByUserIdProvider([NotNull] IUserIdentifier user, string provider, int? skipCount = null, int? maxResultCount = null)
        {
            Check.NotNull(user, nameof(user));

            var query = GetQuery();
            query = ApplyProvider(query, provider);
            query = ApplyUserId(query, user.TenantId, user.UserId);
            query = ApplySkipTake(query, skipCount, maxResultCount);

            var clients = query.ToList().Select(MapToPushClient);
            return clients.ToImmutableList();
        }

        /// <summary>
        /// Gets total count of all push clients by user id and provider.
        /// </summary>
        [NotNull]
        public virtual int GetTotalCountByUserIdProvider([NotNull] IUserIdentifier user, string provider)
        {
            Check.NotNull(user, nameof(user));

            var query = GetQuery();
            query = ApplyProvider(query, provider);
            query = ApplyUserId(query, user.TenantId, user.UserId); ;
            return query.Count();
        }
    }
}