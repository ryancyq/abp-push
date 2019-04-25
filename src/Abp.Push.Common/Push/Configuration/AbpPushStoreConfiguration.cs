using Abp.Dependency;

namespace Abp.Push.Configuration
{
    internal class AbpPushStoreConfiguration : IAbpPushStoreConfiguration, ISingletonDependency
    {
        public IAbpPushDeviceStoreConfiguration DeviceStore { get; private set; }

        public IAbpPushRequestStoreConfiguration RequestStore { get; private set; }

        public AbpPushStoreConfiguration(
            IAbpPushDeviceStoreConfiguration deviceStoreConfiguration,
            IAbpPushRequestStoreConfiguration requestStoreConfiguration
            )
        {
            DeviceStore = deviceStoreConfiguration;
            RequestStore = requestStoreConfiguration;
        }
    }
}
