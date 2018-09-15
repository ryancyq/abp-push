namespace Abp.Push.Devices
{
    /// <summary>
    /// Represents information of a device
    /// </summary>
    public interface IHasDeviceInfo<TKey>
    {
        string DevicePlatform { get; }

        /// <summary>
        /// Device id
        /// </summary>
        TKey DeviceId { get; }

        /// <summary>
        /// Device name
        /// </summary>
        string DeviceName { get; }

        /// <summary>
        /// Normalized device name
        /// </summary>
        string NormalizedDeviceName { get; }
    }
}