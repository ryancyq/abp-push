using System.Collections.Generic;
using Abp.Collections;
using Abp.Push.Devices;
using Abp.Push.Providers;

namespace Abp.Push.Configurations
{
    /// <summary>
    /// Used to configure push system.
    /// </summary>
    public interface IPushConfiguration
    {
        ITypeList<PushDefinitionProvider> Providers { get; }

        List<ServiceProviderInfo> ServiceProviders { get; }

        List<DevicePlatformInfo> DevicePlatforms { get; }
    }
}
