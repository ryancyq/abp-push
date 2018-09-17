using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Push.Configurations;

namespace Abp.Push.Devices
{
    public abstract class AbpPushDeviceManager<TDevice> : PushDeviceManager<TDevice>, IAbpPushDeviceManager<TDevice>
        where TDevice : AbpPushDevice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbpPushDeviceManager{TDevice}"/> class.
        /// </summary>
        protected AbpPushDeviceManager(
            IRepository<TDevice, Guid> entityRepository,
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
        public async virtual Task<IReadOnlyList<TDevice>> GetAllByUserIdAsync(IUserIdentifier user, int? skipCount = null, int? maxResultCount = null)
        {
            var query = DeviceRepository.GetAll();
            query = ApplyUserIdentifier(query, user);
            query = ApplySkipTake(query, skipCount, maxResultCount);

            var devices = await AsyncQueryableExecuter.ToListAsync(query);
            return devices.ToImmutableList();
        }

        /// <summary>
        /// Gets all push devices by user id and provider.
        /// </summary>
        public async virtual Task<IReadOnlyList<TDevice>> GetAllByUserIdProviderAsync(IUserIdentifier userIdentifier, string serviceProvider, int? skipCount = null, int? maxResultCount = null)
        {
            var query = DeviceRepository.GetAll();
            query = ApplyProvider(query, serviceProvider);
            query = ApplyUserIdentifier(query, userIdentifier);
            query = ApplySkipTake(query, skipCount, maxResultCount);

            var devices = await AsyncQueryableExecuter.ToListAsync(query);
            return devices.ToImmutableList();
        }

        /// <summary>
        /// Gets total count of all push devices by user id.
        /// </summary>
        public async virtual Task<int> GetDeviceCountByUserIdAsync(IUserIdentifier userIdentifier)
        {
            var query = DeviceRepository.GetAll();
            query = ApplyUserIdentifier(query, userIdentifier);

            return await AsyncQueryableExecuter.CountAsync(query);
        }

        /// <summary>
        /// Gets total count of all push devices by user id and provider.
        /// </summary>
        public async virtual Task<int> GetDeviceCountByUserIdProviderAsync(IUserIdentifier userIdentifier, string serviceProvider)
        {
            var query = DeviceRepository.GetAll();
            query = ApplyProvider(query, serviceProvider);
            query = ApplyUserIdentifier(query, userIdentifier);

            return await AsyncQueryableExecuter.CountAsync(query);
        }

        /// <summary>
        /// Removes all push devices by user identifier.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        public async virtual Task RemoveAllByUserIdAsync(IUserIdentifier userIdentifier)
        {
            await DeviceRepository.DeleteAsync(d => d.TenantId == userIdentifier.TenantId && d.UserId == userIdentifier.UserId);
        }

        protected virtual IQueryable<TDevice> ApplyUserIdentifier(IQueryable<TDevice> query, IUserIdentifier userIdentifier)
        {
            return query.Where(d => d.TenantId == userIdentifier.TenantId && d.UserId == userIdentifier.UserId);
        }
    }
}