using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Linq;

namespace Abp.Push.Devices
{
    /// <summary>
    /// Implements <see cref="IPushDeviceStore{TDevice}"/> using repositories.
    /// </summary>
    public abstract class AbpPushDeviceStore<TDevice> : AbpServiceBase, IPushDeviceStore<TDevice> where TDevice : PushDevice
    {
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        protected readonly IRepository<TDevice, Guid> DeviceRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbpPushDeviceStore{TDevice}"/> class.
        /// </summary>
        protected AbpPushDeviceStore(
            IRepository<TDevice, Guid> deviceRepository)
        {
            DeviceRepository = deviceRepository;
        }

        public async virtual Task DeleteDeviceAsync(TDevice device)
        {
            await DeviceRepository.DeleteAsync(device);
        }

        public async virtual Task DeleteDeviceAsync(IDeviceIdentifier deviceIdentifier)
        {
            await DeviceRepository.DeleteAsync(d => d.TenantId == deviceIdentifier.TenantId && d.Id == deviceIdentifier.DeviceId);
        }

        public async virtual Task DeleteDevicesByPlatformAsync(string devicePlatform)
        {
            await DeviceRepository.DeleteAsync(d => d.DevicePlatform == devicePlatform);
        }

        public async virtual Task DeleteDevicesByProviderAsync(string serviceProvider)
        {
            await DeviceRepository.DeleteAsync(d => d.ServiceProvider == serviceProvider);
        }

        public async virtual Task DeleteDevicesByProviderAsync(string serviceProvider, string serviceProviderKey)
        {
            await DeviceRepository.DeleteAsync(d => d.ServiceProvider == serviceProvider && d.ServiceProviderKey == serviceProviderKey);
        }

        public async virtual Task DeleteDevicesByUserAsync(IUserIdentifier userIdentifier)
        {
            await DeviceRepository.DeleteAsync(d => d.TenantId == userIdentifier.TenantId && d.UserId == userIdentifier.UserId);
        }

        public async virtual Task DeleteDevicesByUserPlatformAsync(IUserIdentifier userIdentifier, string devicePlatform)
        {
            await DeviceRepository.DeleteAsync(d => d.TenantId == userIdentifier.TenantId && d.UserId == userIdentifier.UserId &&
                                               d.DevicePlatform == devicePlatform);
        }

        public async virtual Task DeleteDevicesByUserProviderAsync(IUserIdentifier userIdentifier, string serviceProvider)
        {
            await DeviceRepository.DeleteAsync(d => d.TenantId == userIdentifier.TenantId && d.UserId == userIdentifier.UserId &&
                                               d.ServiceProvider == serviceProvider);
        }

        public async virtual Task<TDevice> GetDeviceOrNullAsync(string serviceProvider, string serviceProviderKey)
        {
            return await DeviceRepository
                .FirstOrDefaultAsync(d => d.ServiceProvider == serviceProvider && d.ServiceProviderKey == serviceProviderKey);
        }

        public async virtual Task<TDevice> GetUserDeviceOrNullAsync(IUserIdentifier userIdentifier, string serviceProvider, string serviceProviderKey)
        {
            return await DeviceRepository
                .FirstOrDefaultAsync(d => d.TenantId == userIdentifier.TenantId && d.UserId == userIdentifier.UserId &&
                                     d.ServiceProvider == serviceProvider && d.ServiceProviderKey == serviceProviderKey);
        }

        public async virtual Task<List<TDevice>> GetDevicesAsync(int? skipCount = null, int? maxResultCount = null)
        {
            var query = DeviceRepository.GetAll();
            query = ApplySkipTake(query, skipCount, maxResultCount);

            return await AsyncQueryableExecuter.ToListAsync(query);
        }

        public async virtual Task<List<TDevice>> GetDevicesByPlatformAsync(string devicePlatform, int? skipCount = null, int? maxResultCount = null)
        {
            var query = DeviceRepository.GetAll();
            query = ApplyPlatform(query, devicePlatform);
            query = ApplySkipTake(query, skipCount, maxResultCount);

            return await AsyncQueryableExecuter.ToListAsync(query);
        }

        public async virtual Task<List<TDevice>> GetDevicesByProviderAsync(string serviceProvider, int? skipCount = null, int? maxResultCount = null)
        {
            var query = DeviceRepository.GetAll();
            query = ApplyProvider(query, serviceProvider);
            query = ApplySkipTake(query, skipCount, maxResultCount);

            return await AsyncQueryableExecuter.ToListAsync(query);
        }

        public async virtual Task<List<TDevice>> GetDevicesByUserAsync(IUserIdentifier userIdentifier, int? skipCount = null, int? maxResultCount = null)
        {
            var query = DeviceRepository.GetAll();
            query = ApplyUser(query, userIdentifier);
            query = ApplySkipTake(query, skipCount, maxResultCount);

            return await AsyncQueryableExecuter.ToListAsync(query);
        }

        public async virtual Task<List<TDevice>> GetDevicesByUserPlatformAsync(IUserIdentifier userIdentifier, string devicePlatform, int? skipCount = null, int? maxResultCount = null)
        {
            var query = DeviceRepository.GetAll();
            query = ApplyPlatform(query, devicePlatform);
            query = ApplyUser(query, userIdentifier);
            query = ApplySkipTake(query, skipCount, maxResultCount);

            return await AsyncQueryableExecuter.ToListAsync(query);
        }

        public async virtual Task<List<TDevice>> GetDevicesByUserProviderAsync(IUserIdentifier userIdentifier, string serviceProvider, int? skipCount = null, int? maxResultCount = null)
        {
            var query = DeviceRepository.GetAll();
            query = ApplyProvider(query, serviceProvider);
            query = ApplyUser(query, userIdentifier);
            query = ApplySkipTake(query, skipCount, maxResultCount);

            return await AsyncQueryableExecuter.ToListAsync(query);
        }

        public async virtual Task<int> GetDeviceCountByUserAsync(IUserIdentifier userIdentifier)
        {
            var query = DeviceRepository.GetAll();
            query = ApplyUser(query, userIdentifier);

            return await AsyncQueryableExecuter.CountAsync(query);
        }

        public async virtual Task<int> GetDeviceCountByUserPlatformAsync(IUserIdentifier userIdentifier, string devicePlatform)
        {
            var query = DeviceRepository.GetAll();
            query = ApplyPlatform(query, devicePlatform);
            query = ApplyUser(query, userIdentifier);

            return await AsyncQueryableExecuter.CountAsync(query);
        }

        public async virtual Task<int> GetDeviceCountByUserProviderAsync(IUserIdentifier userIdentifier, string serviceProvider)
        {
            var query = DeviceRepository.GetAll();
            query = ApplyProvider(query, serviceProvider);
            query = ApplyUser(query, userIdentifier);

            return await AsyncQueryableExecuter.CountAsync(query);
        }

        public async virtual Task InsertDeviceAsync(TDevice device)
        {
            await DeviceRepository.InsertAsync(device);
        }

        public async virtual Task InsertOrUpdateDeviceAsync(TDevice device)
        {
            await DeviceRepository.InsertOrUpdateAsync(device);
        }

        public async virtual Task UpdateDeviceAsync(TDevice device)
        {
            await DeviceRepository.UpdateAsync(device);
        }

        protected virtual IQueryable<TDevice> ApplyUser(IQueryable<TDevice> query, IUserIdentifier userIdentifier)
        {
            return query.Where(d => d.TenantId == userIdentifier.TenantId && d.UserId == userIdentifier.UserId);
        }

        protected virtual IQueryable<TDevice> ApplyPlatform(IQueryable<TDevice> query, string devicePlatform)
        {
            return query.Where(d => d.DevicePlatform == devicePlatform);
        }

        protected virtual IQueryable<TDevice> ApplyProvider(IQueryable<TDevice> query, string serviceProvider)
        {
            return query.Where(d => d.ServiceProvider == serviceProvider);
        }

        protected virtual IQueryable<TDevice> ApplyProviderIdentity(IQueryable<TDevice> query, string serviceProvider, string serviceProviderKey)
        {
            return ApplyProvider(query, serviceProvider).Where(pl => pl.ServiceProviderKey == serviceProviderKey);
        }

        protected virtual IQueryable<TDevice> ApplySkipTake(IQueryable<TDevice> query, int? skipCount, int? maxResultCount)
        {
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
    }
}