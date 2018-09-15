namespace Abp.Push.Devices
{
    /// <summary>
    /// Interface to get a device identifier.
    /// </summary>
    public interface IDeviceIdentifier
    {
        /// <summary>
        /// Tenant Id. Can be null for host device.
        /// </summary>
        int? TenantId { get; }

        /// <summary>
        /// Id of the device.
        /// </summary>
        long DeviceId { get; }
    }
}