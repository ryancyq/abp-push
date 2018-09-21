using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
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

        [UnitOfWork]
        public virtual async Task DeleteDeviceAsync(TDevice device)
        {
            using (UnitOfWorkManager.Current.SetTenantId(device.TenantId))
            {
                await DeviceRepository.DeleteAsync(device);
                await UnitOfWorkManager.Current.SaveChangesAsync();
            }
        }

        [UnitOfWork]
        public virtual async Task DeleteDeviceAsync(IDeviceIdentifier deviceIdentifier)
        {
            using (UnitOfWorkManager.Current.SetTenantId(deviceIdentifier.TenantId))
            {
                await DeviceRepository.DeleteAsync(d => d.TenantId == deviceIdentifier.TenantId &&
                                                   d.Id == deviceIdentifier.DeviceId);
                await UnitOfWorkManager.Current.SaveChangesAsync();
            }
        }

        [UnitOfWork]
        public virtual async Task DeleteDevicesByPlatformAsync(int? tenantId, string devicePlatform)
        {
            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                await DeviceRepository.DeleteAsync(d => d.DevicePlatform == devicePlatform);
                await UnitOfWorkManager.Current.SaveChangesAsync();
            }
        }

        [UnitOfWork]
        public virtual async Task DeleteDevicesByProviderAsync(int? tenantId, string serviceProvider)
        {
            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                await DeviceRepository.DeleteAsync(d => d.ServiceProvider == serviceProvider);
                await UnitOfWorkManager.Current.SaveChangesAsync();
            }
        }

        [UnitOfWork]
        public virtual async Task DeleteDevicesByProviderAsync(int? tenantId, string serviceProvider, string serviceProviderKey)
        {
            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                await DeviceRepository.DeleteAsync(d => d.ServiceProvider == serviceProvider &&
                                                   d.ServiceProviderKey == serviceProviderKey);
                await UnitOfWorkManager.Current.SaveChangesAsync();
            }
        }

        [UnitOfWork]
        public virtual async Task DeleteDevicesByUserAsync(IUserIdentifier userIdentifier)
        {
            using (UnitOfWorkManager.Current.SetTenantId(userIdentifier.TenantId))
            {
                await DeviceRepository.DeleteAsync(d => d.UserId == userIdentifier.UserId);
                await UnitOfWorkManager.Current.SaveChangesAsync();
            }
        }

        [UnitOfWork]
        public virtual async Task DeleteDevicesByUserPlatformAsync(IUserIdentifier userIdentifier, string devicePlatform)
        {
            using (UnitOfWorkManager.Current.SetTenantId(userIdentifier.TenantId))
            {
                await DeviceRepository.DeleteAsync(d => d.UserId == userIdentifier.UserId &&
                                                   d.DevicePlatform == devicePlatform);
                await UnitOfWorkManager.Current.SaveChangesAsync();
            }
        }

        [UnitOfWork]
        public virtual async Task DeleteDevicesByUserProviderAsync(IUserIdentifier userIdentifier, string serviceProvider)
        {
            using (UnitOfWorkManager.Current.SetTenantId(userIdentifier.TenantId))
            {
                await DeviceRepository.DeleteAsync(d => d.UserId == userIdentifier.UserId &&
                                                   d.ServiceProvider == serviceProvider);
                await UnitOfWorkManager.Current.SaveChangesAsync();
            }
        }

        [UnitOfWork]
        public virtual async Task<TDevice> GetDeviceOrNullAsync(int? tenantId, string serviceProvider, string serviceProviderKey)
        {
            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                return await DeviceRepository.FirstOrDefaultAsync(d => d.ServiceProvider == serviceProvider &&
                                                                  d.ServiceProviderKey == serviceProviderKey);
            }
        }

        [UnitOfWork]
        public virtual async Task<TDevice> GetUserDeviceOrNullAsync(IUserIdentifier userIdentifier, string serviceProvider, string serviceProviderKey)
        {
            using (UnitOfWorkManager.Current.SetTenantId(userIdentifier.TenantId))
            {
                return await DeviceRepository.FirstOrDefaultAsync(d => d.UserId == userIdentifier.UserId &&
                                                                  d.ServiceProvider == serviceProvider &&
                                                                  d.ServiceProviderKey == serviceProviderKey);
            }

        }

        [UnitOfWork]
        public virtual async Task<List<TDevice>> GetDevicesAsync(int? tenantId, int? skipCount = null, int? maxResultCount = null)
        {
            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                var query = DeviceRepository.GetAll();
                query = ApplySkipTake(query, skipCount, maxResultCount);

                return await AsyncQueryableExecuter.ToListAsync(query);
            }
        }

        [UnitOfWork]
        public virtual async Task<List<TDevice>> GetDevicesByPlatformAsync(int? tenantId, string devicePlatform, int? skipCount = null, int? maxResultCount = null)
        {
            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                var query = DeviceRepository.GetAll();
                query = ApplyPlatform(query, devicePlatform);
                query = ApplySkipTake(query, skipCount, maxResultCount);

                return await AsyncQueryableExecuter.ToListAsync(query);
            }
        }

        [UnitOfWork]
        public virtual async Task<List<TDevice>> GetDevicesByProviderAsync(int? tenantId, string serviceProvider, int? skipCount = null, int? maxResultCount = null)
        {
            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                var query = DeviceRepository.GetAll();
                query = ApplyProvider(query, serviceProvider);
                query = ApplySkipTake(query, skipCount, maxResultCount);

                return await AsyncQueryableExecuter.ToListAsync(query);
            }
        }

        [UnitOfWork]
        public virtual async Task<List<TDevice>> GetDevicesByUserAsync(IUserIdentifier userIdentifier, int? skipCount = null, int? maxResultCount = null)
        {
            using (UnitOfWorkManager.Current.SetTenantId(userIdentifier.TenantId))
            {
                var query = DeviceRepository.GetAll();
                query = ApplyUser(query, userIdentifier);
                query = ApplySkipTake(query, skipCount, maxResultCount);

                return await AsyncQueryableExecuter.ToListAsync(query);
            }
        }

        [UnitOfWork]
        public virtual async Task<List<TDevice>> GetDevicesByUserPlatformAsync(IUserIdentifier userIdentifier, string devicePlatform, int? skipCount = null, int? maxResultCount = null)
        {
            using (UnitOfWorkManager.Current.SetTenantId(userIdentifier.TenantId))
            {
                var query = DeviceRepository.GetAll();
                query = ApplyPlatform(query, devicePlatform);
                query = ApplyUser(query, userIdentifier);
                query = ApplySkipTake(query, skipCount, maxResultCount);

                return await AsyncQueryableExecuter.ToListAsync(query);
            }
        }

        [UnitOfWork]
        public virtual async Task<List<TDevice>> GetDevicesByUserProviderAsync(IUserIdentifier userIdentifier, string serviceProvider, int? skipCount = null, int? maxResultCount = null)
        {
            using (UnitOfWorkManager.Current.SetTenantId(userIdentifier.TenantId))
            {
                var query = DeviceRepository.GetAll();
                query = ApplyProvider(query, serviceProvider);
                query = ApplyUser(query, userIdentifier);
                query = ApplySkipTake(query, skipCount, maxResultCount);

                return await AsyncQueryableExecuter.ToListAsync(query);
            }
        }

        [UnitOfWork]
        public virtual async Task<int> GetDeviceCountByUserAsync(IUserIdentifier userIdentifier)
        {
            using (UnitOfWorkManager.Current.SetTenantId(userIdentifier.TenantId))
            {
                var query = DeviceRepository.GetAll();
                query = ApplyUser(query, userIdentifier);

                return await AsyncQueryableExecuter.CountAsync(query);
            }
        }

        [UnitOfWork]
        public virtual async Task<int> GetDeviceCountByUserPlatformAsync(IUserIdentifier userIdentifier, string devicePlatform)
        {
            using (UnitOfWorkManager.Current.SetTenantId(userIdentifier.TenantId))
            {
                var query = DeviceRepository.GetAll();
                query = ApplyPlatform(query, devicePlatform);
                query = ApplyUser(query, userIdentifier);

                return await AsyncQueryableExecuter.CountAsync(query);
            }
        }

        [UnitOfWork]
        public virtual async Task<int> GetDeviceCountByUserProviderAsync(IUserIdentifier userIdentifier, string serviceProvider)
        {
            using (UnitOfWorkManager.Current.SetTenantId(userIdentifier.TenantId))
            {
                var query = DeviceRepository.GetAll();
                query = ApplyProvider(query, serviceProvider);
                query = ApplyUser(query, userIdentifier);

                return await AsyncQueryableExecuter.CountAsync(query);
            }
        }

        [UnitOfWork]
        public virtual async Task InsertDeviceAsync(TDevice device)
        {
            using (UnitOfWorkManager.Current.SetTenantId(device.TenantId))
            {
                await DeviceRepository.InsertAsync(device);
                await UnitOfWorkManager.Current.SaveChangesAsync();
            }
        }

        [UnitOfWork]
        public virtual async Task InsertOrUpdateDeviceAsync(TDevice device)
        {
            using (UnitOfWorkManager.Current.SetTenantId(device.TenantId))
            {
                await DeviceRepository.InsertOrUpdateAsync(device);
                await UnitOfWorkManager.Current.SaveChangesAsync();
            }
        }

        [UnitOfWork]
        public virtual async Task UpdateDeviceAsync(TDevice device)
        {
            using (UnitOfWorkManager.Current.SetTenantId(device.TenantId))
            {
                await DeviceRepository.UpdateAsync(device);
                await UnitOfWorkManager.Current.SaveChangesAsync();
            }
        }

        protected virtual IQueryable<TDevice> ApplyUser(IQueryable<TDevice> query, IUserIdentifier userIdentifier)
        {
            return query.Where(d => d.UserId == userIdentifier.UserId);
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