using System.Collections.Generic;
using Abp.Collections;
using Abp.Configuration.Startup;
using Abp.Push.Devices;
using Abp.Push.Providers;

namespace Abp.Push.Configurations
{
    /// <summary>
    /// Used to configure push system.
    /// </summary>
    public interface IAbpPushConfiguration
    {
        /// <summary>
        /// Gets the ABP configuration object.
        /// </summary>
        IAbpStartupConfiguration AbpConfiguration { get; }

        /// <summary>
        /// Gets the list of push definition providers.
        /// </summary>
        ITypeList<PushDefinitionProvider> Providers { get; }

        /// <summary>
        /// Gets the list of push service providers.
        /// </summary>
        List<ServiceProviderInfo> ServiceProviders { get; }

        /// <summary>
        /// Gets the list of device platforms.
        /// </summary>
        List<DevicePlatformInfo> DevicePlatforms { get; }

        int MaxUserCountForForegroundDistribution { get; }
    }
}
