using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Push.Configurations;

namespace Abp.Push.Devices
{
    public abstract class AbpPushDeviceManager<TDevice> : PushDeviceManager<TDevice>
        where TDevice : AbpPushDevice
    {
        protected readonly new IAbpPushDeviceStore<TDevice> DeviceStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbpPushDeviceManager{TDevice}"/> class.
        /// </summary>
        protected AbpPushDeviceManager(
            IAbpPushDeviceStore<TDevice> deviceStore,
            IPushConfiguration pushConfiguration
        ) : base(
            deviceStore,
            pushConfiguration
        )
        {
            DeviceStore = deviceStore;
        }

        /// <summary>
        /// Gets all push device by user identifier.
        /// </summary>
        public async virtual Task<IReadOnlyList<TDevice>> GetAllByUserAsync(IUserIdentifier userIdentifier, int? skipCount = null, int? maxResultCount = null)
        {
            var devices = await DeviceStore.GetDevicesByUserAsync(userIdentifier, skipCount, maxResultCount);
            return devices.ToImmutableList();
        }

        /// <summary>
        /// Gets all push devices by user identifier and provider.
        /// </summary>
        public async virtual Task<IReadOnlyList<TDevice>> GetAllByUserProviderAsync(IUserIdentifier userIdentifier, string serviceProvider, int? skipCount = null, int? maxResultCount = null)
        {
            var devices = await DeviceStore.GetDevicesByUserPlatformAsync(userIdentifier, serviceProvider, skipCount, maxResultCount);
            return devices.ToImmutableList();
        }

        /// <summary>
        /// Gets total count of push devices by user identifier.
        /// </summary>
        public virtual Task<int> GetCountByUserAsync(IUserIdentifier userIdentifier)
        {
            return DeviceStore.GetDeviceCountByUserAsync(userIdentifier);
        }

        /// <summary>
        /// Gets total count of push devices by user identifier and provider.
        /// </summary>
        public virtual Task<int> GetCountByUserIdProviderAsync(IUserIdentifier userIdentifier, string serviceProvider)
        {
            return DeviceStore.GetDeviceCountByUserProviderAsync(userIdentifier, serviceProvider);
        }

        /// <summary>
        /// Gets total count of push devices by user identifier and platform.
        /// </summary>
        public virtual Task<int> GetCountByUserIdPlatformAsync(IUserIdentifier userIdentifier, string devicePlatform)
        {
            return DeviceStore.GetDeviceCountByUserPlatformAsync(userIdentifier, devicePlatform);
        }

        /// <summary>
        /// Removes all push devices by user identifier.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        public async virtual Task RemoveAllByUserIdAsync(IUserIdentifier userIdentifier)
        {
            await DeviceStore.DeleteDevicesByUserAsync(userIdentifier);
        }
    }
}