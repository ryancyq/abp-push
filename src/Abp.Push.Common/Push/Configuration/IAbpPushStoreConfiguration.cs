namespace Abp.Push.Configuration
{
    /// <summary>
    /// Used to configure push system.
    /// </summary>
    public interface IAbpPushStoreConfiguration
    {
        IAbpPushDeviceStoreConfiguration DeviceStore { get; }

        IAbpPushRequestStoreConfiguration RequestStore { get; }
    }
}
