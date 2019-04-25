using Abp.Configuration.Startup;
using Abp.Dependency;

namespace Abp.Push.Configuration
{
    internal class AbpPushRequestStoreConfiguration : IAbpPushRequestStoreConfiguration, ISingletonDependency
    {
        public IAbpStartupConfiguration AbpConfiguration { get; private set; }

        public AbpPushRequestStoreConfiguration(IAbpStartupConfiguration abpConfiguration)
        {
            AbpConfiguration = abpConfiguration;
        }
    }
}
