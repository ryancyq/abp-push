using System.Collections.Generic;
using Abp.Collections;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Push.Devices;
using Abp.Push.Providers;

namespace Abp.Push.Configuration
{
    internal class AbpPushConfiguration : IAbpPushConfiguration, ISingletonDependency
    {
        public IAbpStartupConfiguration AbpConfiguration { get; private set; }

        public IAbpPushStoreConfiguration StoreConfiguration { get; private set; }

        public ITypeList<PushDefinitionProvider> Providers { get; private set; }

        public List<ServiceProviderInfo> ServiceProviders { get; private set; }

        public List<DevicePlatformInfo> DevicePlatforms { get; private set; }

        public int MaxUserCountForForegroundDistribution { get; set; }

        public AbpPushConfiguration(
            IAbpStartupConfiguration abpConfiguration,
            AbpPushStoreConfiguration pushStoreConfiguration
            )
        {
            AbpConfiguration = abpConfiguration;
            StoreConfiguration = pushStoreConfiguration;
            Providers = new TypeList<PushDefinitionProvider>();
            ServiceProviders = new List<ServiceProviderInfo>();
            DevicePlatforms = new List<DevicePlatformInfo>();
            MaxUserCountForForegroundDistribution = 5;
        }
    }
}
