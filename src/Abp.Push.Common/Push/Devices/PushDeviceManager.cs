using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Linq;
using Abp.Push.Configurations;
using Abp.UI;

namespace Abp.Push.Devices
{
    public abstract class PushDeviceManager<TDevice> : DomainService, IPushDeviceManager<TDevice>
        where TDevice : PushDeviceBase
    {
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        protected readonly IRepository<TDevice, Guid> DeviceRepository;
        protected readonly IPushConfiguration Configuration;
        protected readonly IGuidGenerator GuidGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="PushDeviceManager{TDevice}"/> class.
        /// </summary>
        protected PushDeviceManager(
            IRepository<TDevice, Guid> entityRepository,
            IPushConfiguration pushConfiguration,
            IGuidGenerator guidGenerator
            )
        {
            DeviceRepository = entityRepository;
            Configuration = pushConfiguration;
            GuidGenerator = guidGenerator;

            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;

            LocalizationSourceName = AbpPushConsts.LocalizationSourceName;
        }

        /// <summary>
        /// Adds a push device.
        /// </summary>
        /// <param name="entity">The device.</param>
        public async virtual Task<bool> AddAsync(TDevice entity)
        {
            Check.NotNull(entity, nameof(entity));

            try
            {
                ValidateServiceProvider(entity);

                ValidateDevicePlatform(entity);

                using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                {
                    await CreateOrUpdateAsync(entity);
                    await uow.CompleteAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("AddAsync failed, device: " + entity?.ToString(), ex);
            }
            return false;
        }

        protected async virtual Task CreateOrUpdateAsync(TDevice entity)
        {

            var existingDevice = await FindAsync(entity.ServiceProvider, entity.ServiceProviderKey);
            if (existingDevice == null)
            {
                Logger.Debug("Device push existed " + entity.ToString());
                existingDevice = MapToPushDevice(existingDevice, entity);
                await DeviceRepository.UpdateAsync(existingDevice);
            }
            else
            {
                var newDevice = MapToPushDevice(entity);
                await DeviceRepository.InsertAsync(newDevice);
            }
        }

        /// <summary>
        /// Find a push device by provider & provider key.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="serviceProviderKey">The service provider key.</param>
        /// <returns>The push device, if it is found</returns>
        public async virtual Task<TDevice> FindAsync(string serviceProvider, string serviceProviderKey)
        {
            return await DeviceRepository
                .FirstOrDefaultAsync(d => d.ServiceProvider == serviceProvider && d.ServiceProviderKey == serviceProviderKey);
        }

        protected virtual TDevice MapToPushDevice(TDevice entity)
        {
            entity.DeviceIdentifier = GuidGenerator.Create();
            entity.SetNormalizedNames();
            return entity;
        }

        protected virtual TDevice MapToPushDevice(TDevice existingEntity, TDevice entity)
        {
            existingEntity.ServiceProviderKey = entity.ServiceProviderKey;
            existingEntity.DevicePlatform = entity.DevicePlatform;
            existingEntity.DeviceIdentifier = entity.DeviceIdentifier;
            existingEntity.DeviceName = entity.DeviceName;
            existingEntity.SetNormalizedNames();
            return existingEntity;
        }

        protected virtual void ValidateServiceProvider(TDevice entity)
        {
            var serviceProvider = Configuration.ServiceProviders
                                               .FirstOrDefault(p => p.Name == entity.ServiceProvider);
            if (serviceProvider == null)
            {
                throw new UserFriendlyException(L("Push.ServiceProvider.Invalid"));
            }
        }

        protected virtual void ValidateDevicePlatform(TDevice entity)
        {
            var devicePlatform = Configuration.DevicePlatforms
                                              .FirstOrDefault(p => p.Name == entity.DevicePlatform);
            if (devicePlatform == null)
            {
                throw new UserFriendlyException(L("Push.DevicePlatform.Invalid"));
            }
        }

        /// <summary>
        /// Gets all push devices.
        /// </summary>
        public async virtual Task<IReadOnlyList<TDevice>> GetAllAsync(int? skipCount = null, int? maxResultCount = null)
        {
            var query = DeviceRepository.GetAll();
            query = ApplySkipTake(query, skipCount, maxResultCount);

            var devices = await AsyncQueryableExecuter.ToListAsync(query);
            return devices.ToImmutableList();
        }

        /// <summary>
        /// Removes a push device
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>True, if a push device is removed</returns>
        public async virtual Task<bool> RemoveAsync(TDevice device)
        {
            var query = DeviceRepository.GetAll()
                                        .Where(d => d.Id == device.Id);
            var count = await AsyncQueryableExecuter.CountAsync(query);
            if (count <= 0)
            {
                return false;
            }

            try
            {
                using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                {
                    await DeviceRepository.DeleteAsync(device);
                    await uow.CompleteAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("RemoveAsync failed, device: " + device?.ToString(), ex);
            }
            return false;
        }

        /// <summary>
        /// Removes a push device by device identifier.
        /// </summary>
        /// <param name="deviceIdentifier">The device identifier.</param>
        /// <returns>True, if a push device is removed</returns>
        public async virtual Task<bool> RemoveAsync(IDeviceIdentifier deviceIdentifier)
        {
            var query = DeviceRepository.GetAll().Where(d => d.Id == deviceIdentifier.DeviceId);
            var count = await AsyncQueryableExecuter.CountAsync(query);
            if (count <= 0)
            {
                return false;
            }

            try
            {
                using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                {
                    await DeviceRepository.DeleteAsync(deviceIdentifier.DeviceId);
                    await uow.CompleteAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("RemoveAsync failed, device identifier: " + deviceIdentifier?.ToString(), ex);
            }
            return false;
        }

        /// <summary>
        /// Removes a push device by provider & provider key.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="serviceProviderKey">The service provider key.</param>
        /// <returns>True, if a push device is removed</returns>
        public async virtual Task<bool> RemoveAsync(string serviceProvider, string serviceProviderKey)
        {
            var query = DeviceRepository.GetAll()
                                        .Where(d => d.ServiceProvider == serviceProvider && d.ServiceProviderKey == serviceProviderKey);
            var count = await AsyncQueryableExecuter.CountAsync(query);
            if (count <= 0)
            {
                return false;
            }

            try
            {
                using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                {
                    await DeviceRepository
                        .DeleteAsync(d => d.ServiceProvider == serviceProvider && d.ServiceProviderKey == serviceProviderKey);
                    await uow.CompleteAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("RemoveAsync failed, service provider: " + serviceProvider + ", service provider key: " + serviceProviderKey, ex);
            }
            return false;
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