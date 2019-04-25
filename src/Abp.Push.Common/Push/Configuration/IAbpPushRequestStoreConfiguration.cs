using Abp.Configuration.Startup;

namespace Abp.Push.Configuration
{
    /// <summary>
    /// Used to configure push reequest store.
    /// </summary>
    public interface IAbpPushRequestStoreConfiguration
    {
        /// <summary>
        /// Gets the ABP configuration object.
        /// </summary>
        IAbpStartupConfiguration AbpConfiguration { get; }
    }
}
