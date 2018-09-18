using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Push.Devices;

namespace Abp.Push
{
    /// <summary>
    /// Used to store (persist) push devices.
    /// </summary>
    public interface IPushDeviceStore<TDevice> where TDevice : PushDeviceBase
    {
        /// <summary>
        /// Inserts a push device.
        /// </summary>
        Task InsertDeviceAsync(TDevice device);

        /// <summary>
        /// Updates a push device.
        /// </summary>
        Task UpdateDeviceAsync(TDevice device);

        /// <summary>
        /// Inserts or updates a push device.
        /// </summary>
        Task InsertOrUpdateDeviceAsync(TDevice device);

        /// <summary>
        /// Delete a push device.
        /// </summary>
        Task DeleteDeviceAsync(TDevice device);

        /// <summary>
        /// Delete a push device by device identifier.
        /// </summary>
        Task DeleteDeviceAsync(IDeviceIdentifier deviceIdentifier);

        /// <summary>
        /// Delete all push devices by service provider.
        /// <param name="serviceProvider">The service provider.</param>
        /// </summary>
        Task DeleteDevicesByProviderAsync(string serviceProvider);

        /// <summary>
        /// Delete all push devices by device platform.
        /// <param name="devicePlatform">The device platform.</param>
        /// </summary>
        Task DeleteDevicesByPlatformAsync(string devicePlatform);

        /// <summary>
        /// Get a push device by service provider and service provider key.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="serviceProviderKey">The service provider key.</param>
        /// <returns>The push device, if it is found</returns>
        Task<TDevice> GetDeviceOrNullAsync(string serviceProvider, string serviceProviderKey);

        /// <summary>
        /// Gets all push devices.
        /// </summary>
        Task<List<TDevice>> GetDevicesAsync(int? skipCount = null, int? maxResultCount = null);

        /// <summary>
        /// Gets all push devices by service provider.
        /// <param name="serviceProvider">The service provider.</param>
        /// </summary>
        Task<List<TDevice>> GetDevicesByProviderAsync(string serviceProvider, int? skipCount = null, int? maxResultCount = null);

        /// <summary>
        /// Gets all push devices by device platform.
        /// <param name="devicePlatform">The device platform.</param>
        /// </summary>
        Task<List<TDevice>> GetDevicesByPlatformAsync(string devicePlatform, int? skipCount = null, int? maxResultCount = null);
    }
}