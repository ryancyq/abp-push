using System;
using Abp.Configuration.Startup;
using Abp.Push.Requests;
using Abp.Push.Devices;

namespace Abp.Push.Configuration
{
    public static class AbpPushModuleConfigurationExtensions
    {
        /// <summary>
        /// Configures persistent push request store.
        /// </summary>
        public static void ConfigStore(this IAbpPushConfiguration configuration, Action<IAbpPushStoreConfiguration> configureAction)
        {
            configureAction(configuration.AbpConfiguration.Modules.AbpPush().StoreConfiguration);
        }

        /// <summary>
        /// Configures persistent push store.
        /// </summary>
        public static void UsePersistentStore<TDevice>(this IAbpPushStoreConfiguration configuration)
            where TDevice : AbpPushDevice, new()
        {
            configuration.DeviceStore.UsePersistentStore<TDevice>();
            configuration.RequestStore.UsePersistentStore();
        }

        /// <summary>
        /// Configures persistent push device store.
        /// </summary>
        public static void UsePersistentStore(this IAbpPushRequestStoreConfiguration configuration)
        {
            configuration.AbpConfiguration.ReplaceService<IPushRequestStore, AbpPersistentPushRequestStore>();
        }

        /// <summary>
        /// Configures persistent push device store.
        /// </summary>
        public static void UsePersistentStore<TDevice>(this IAbpPushDeviceStoreConfiguration configuration)
            where TDevice : AbpPushDevice, new()
        {
            configuration.AbpConfiguration.ReplaceService<IPushDeviceStore<TDevice>, AbpPersistentPushDeviceStore<TDevice>>();
        }
    }
}
