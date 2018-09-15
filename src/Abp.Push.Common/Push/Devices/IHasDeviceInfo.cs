namespace Abp.Push.Devices
{
    /// <summary>
    /// Represents information of a device
    /// </summary>
    public interface IHasDeviceInfo<TIdentifier>
    {
        string DevicePlatform { get; }

        /// <summary>
        /// Device name
        /// </summary>
        string DeviceName { get; }

        /// <summary>
        /// Normalized device name
        /// </summary>
        string NormalizedDeviceName { get; }

        /// <summary>
        /// Device identifier
        /// </summary>
        TIdentifier DeviceIdentifier { get; }


    }
}