using Abp.Configuration.Startup;
using Abp.Dependency;

namespace Abp.Push.Configuration
{
    internal class AbpPushDeviceStoreConfiguration : IAbpPushDeviceStoreConfiguration, ISingletonDependency
    {
        public IAbpStartupConfiguration AbpConfiguration { get; private set; }

        public AbpPushDeviceStoreConfiguration(IAbpStartupConfiguration abpConfiguration)
        {
            AbpConfiguration = abpConfiguration;
        }
    }
}
