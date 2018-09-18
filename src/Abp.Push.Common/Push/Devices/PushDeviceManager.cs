using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Abp.Domain.Services;
using Abp.Push.Configurations;
using Abp.UI;

namespace Abp.Push.Devices
{
    public abstract class PushDeviceManager<TDevice> : DomainService
        where TDevice : PushDeviceBase
    {
        protected readonly IPushDeviceStore<TDevice> DeviceStore;
        protected readonly IPushConfiguration Configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="PushDeviceManager{TDevice}"/> class.
        /// </summary>
        protected PushDeviceManager(
            IPushDeviceStore<TDevice> deviceStore,
            IPushConfiguration pushConfiguration
            )
        {
            DeviceStore = deviceStore;
            Configuration = pushConfiguration;

            LocalizationSourceName = AbpPushConsts.LocalizationSourceName;
        }

        /// <summary>
        /// Adds a push device.
        /// </summary>
        /// <param name="device">The device.</param>
        public async virtual Task<bool> AddAsync(TDevice device)
        {
            try
            {
                Check.NotNull(device, nameof(device));

                ValidateServiceProvider(device);
                ValidateDevicePlatform(device);

                using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                {
                    await CreateOrUpdateAsync(device);
                    await uow.CompleteAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("AddAsync failed, device: " + device?.ToString(), ex);
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
                await DeviceStore.UpdateDeviceAsync(existingDevice);
            }
            else
            {
                var newDevice = MapToPushDevice(entity);
                await DeviceStore.InsertDeviceAsync(newDevice);
            }
        }

        /// <summary>
        /// Find a push device by provider and provider key.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="serviceProviderKey">The service provider key.</param>
        /// <returns>The push device, if it is found</returns>
        public async virtual Task<TDevice> FindAsync(string serviceProvider, string serviceProviderKey)
        {
            return await DeviceStore.GetDeviceOrNullAsync(serviceProvider, serviceProviderKey);
        }

        protected virtual TDevice MapToPushDevice(TDevice entity)
        {
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
            var devices = await DeviceStore.GetDevicesAsync(skipCount, maxResultCount);
            return devices.ToImmutableList();
        }

        /// <summary>
        /// Removes a push device
        /// </summary>
        /// <param name="device">The device.</param>
        public async virtual Task RemoveAsync(TDevice device)
        {
            try
            {
                Check.NotNull(device, nameof(device));

                using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                {
                    await DeviceStore.DeleteDeviceAsync(device);
                    await uow.CompleteAsync();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("RemoveAsync failed, device: " + device?.ToString(), ex);
            }
        }

        /// <summary>
        /// Removes a push device by device identifier.
        /// </summary>
        /// <param name="deviceIdentifier">The device identifier.</param>
        public async virtual Task RemoveAsync(IDeviceIdentifier deviceIdentifier)
        {
            try
            {
                using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                {
                    await DeviceStore.DeleteDeviceAsync(deviceIdentifier);
                    await uow.CompleteAsync();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("RemoveAsync failed, device identifier: " + deviceIdentifier?.ToString(), ex);
            }
        }

        /// <summary>
        /// Removes a push device by provider & provider key.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="serviceProviderKey">The service provider key.</param>
        public async virtual Task RemoveAsync(string serviceProvider, string serviceProviderKey)
        {
            try
            {
                using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                {
                    await DeviceStore.DeleteDevicesByProviderAsync(serviceProvider, serviceProviderKey);
                    await uow.CompleteAsync();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("RemoveAsync failed, service provider: " + serviceProvider + ", service provider key: " + serviceProviderKey, ex);
            }
        }
    }
}