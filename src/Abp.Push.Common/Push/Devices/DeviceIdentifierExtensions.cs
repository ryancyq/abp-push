namespace Abp.Push.Devices
{
    /// <summary>
    /// Extension methods for <see cref="DeviceIdentifier"/> and <see cref="IDeviceIdentifier"/>.
    /// </summary>
    public static class DeviceIdentifierExtensions
    {
        /// <summary>
        /// Creates a new <see cref="DeviceIdentifier"/> object from any object implements <see cref="IDeviceIdentifier"/>.
        /// </summary>
        /// <param name="deviceIdentifier">Device identifier.</param>
        public static DeviceIdentifier ToDeviceIdentifier(this IDeviceIdentifier deviceIdentifier)
        {
            return new DeviceIdentifier(deviceIdentifier.TenantId, deviceIdentifier.DeviceId);
        }
    }
}