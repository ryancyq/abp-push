using Abp.Configuration.Startup;

namespace Abp.Push.Configuration
{
    /// <summary>
    /// Used to configure push device store.
    /// </summary>
    public interface IAbpPushDeviceStoreConfiguration
    {
        /// <summary>
        /// Gets the ABP configuration object.
        /// </summary>
        IAbpStartupConfiguration AbpConfiguration { get; }
    }
}
