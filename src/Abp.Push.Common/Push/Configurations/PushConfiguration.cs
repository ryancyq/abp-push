using System.Collections.Generic;
using Abp.Collections;
using Abp.Dependency;
using Abp.Push.Devices;
using Abp.Push.Providers;

namespace Abp.Push.Configurations
{
    internal class PushConfiguration : IPushConfiguration, ISingletonDependency
    {
        public ITypeList<PushDefinitionProvider> Providers { get; private set; }

        public List<ServiceProviderInfo> ServiceProviders { get; private set; }

        public List<DevicePlatformInfo> DevicePlatforms { get; private set; }

        public PushConfiguration()
        {
            Providers = new TypeList<PushDefinitionProvider>();
            ServiceProviders = new List<ServiceProviderInfo>();
            DevicePlatforms = new List<DevicePlatformInfo>();
        }
    }
}
