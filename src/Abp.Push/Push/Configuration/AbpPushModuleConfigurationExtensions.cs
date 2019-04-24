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
        public static void UsePersistentRequestStore(this IAbpPushConfiguration configuration)
        {
            configuration.AbpConfiguration.ReplaceService<IPushRequestStore, AbpPersistentPushRequestStore>();
        }

        /// <summary>
        /// Configures persistent push device store.
        /// </summary>
        public static void UsePersistentDeviceStore<TDevice>(this IAbpPushConfiguration configuration)
            where TDevice : AbpPushDevice, new()
        {
            configuration.AbpConfiguration.ReplaceService<IPushDeviceStore<TDevice>, AbpPersistentPushDeviceStore<TDevice>>();
        }
    }
}
