using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abp.Push.Devices
{
    /// <summary>
    /// Used to store (persist) push devices.
    /// </summary>
    public interface IPushDeviceStore<TDevice> where TDevice : AbpPushDevice
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
        /// <param name="tenantId">The tenant id.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// </summary>
        Task DeleteDevicesByProviderAsync(int? tenantId, string serviceProvider);

        /// <summary>
        /// Delete all push devices by service provider and service provider key.
        /// <param name="tenantId">The tenant id.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="serviceProviderKey">The service provider key.</param>
        /// </summary>
        Task DeleteDevicesByProviderAsync(int? tenantId, string serviceProvider, string serviceProviderKey);

        /// <summary>
        /// Delete all push devices by device platform.
        /// <param name="tenantId">The tenant id.</param>
        /// <param name="devicePlatform">The device platform.</param>
        /// </summary>
        Task DeleteDevicesByPlatformAsync(int? tenantId, string devicePlatform);

        /// <summary>
        /// Delete a push device by user identifier.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        Task DeleteDevicesByUserAsync(IUserIdentifier userIdentifier);

        /// <summary>
        /// Delete all push devices by user identifier and service provider.
        /// <param name="userIdentifier">The user identifier.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// </summary>
        Task DeleteDevicesByUserProviderAsync(IUserIdentifier userIdentifier, string serviceProvider);

        /// <summary>
        /// Delete all push devices by user identifier and device platform.
        /// <param name="userIdentifier">The user identifier.</param>
        /// <param name="devicePlatform">The device platform.</param>
        /// </summary>
        Task DeleteDevicesByUserPlatformAsync(IUserIdentifier userIdentifier, string devicePlatform);

        /// <summary>
        /// Get a push device by service provider and service provider key.
        /// </summary>
        /// <param name="tenantId">The tenant id.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="serviceProviderKey">The service provider key.</param>
        /// <returns>The push device, if it is found</returns>
        Task<TDevice> GetDeviceOrNullAsync(int? tenantId, string serviceProvider, string serviceProviderKey);

        /// <summary>
        /// Gets all push devices.
        /// </summary>
        /// <param name="tenantId">The tenant id.</param>
        Task<List<TDevice>> GetDevicesAsync(int? tenantId, int? skipCount = null, int? maxResultCount = null);

        /// <summary>
        /// Gets all push devices by service provider.
        /// <param name="tenantId">The tenant id.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// </summary>
        Task<List<TDevice>> GetDevicesByProviderAsync(int? tenantId, string serviceProvider, int? skipCount = null, int? maxResultCount = null);

        /// <summary>
        /// Gets all push devices by device platform.
        /// <param name="tenantId">The tenant id.</param>
        /// <param name="devicePlatform">The device platform.</param>
        /// </summary>
        Task<List<TDevice>> GetDevicesByPlatformAsync(int? tenantId, string devicePlatform, int? skipCount = null, int? maxResultCount = null);

        /// <summary>
        /// Get a push device by user identifier and service provider and service provider key.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="serviceProviderKey">The service provider key.</param>
        /// <returns>The push device, if it is found</returns>
        Task<TDevice> GetUserDeviceOrNullAsync(IUserIdentifier userIdentifier, string serviceProvider, string serviceProviderKey);

        /// <summary>
        /// Gets all push devices by user identifier.
        /// <param name="userIdentifier">The user identifier.</param>
        /// </summary>
        Task<List<TDevice>> GetDevicesByUserAsync(IUserIdentifier userIdentifier, int? skipCount = null, int? maxResultCount = null);

        /// <summary>
        /// Gets all push devices by user identifier and service provider.
        /// <param name="userIdentifier">The user identifier.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// </summary>
        Task<List<TDevice>> GetDevicesByUserProviderAsync(IUserIdentifier userIdentifier, string serviceProvider, int? skipCount = null, int? maxResultCount = null);

        /// <summary>
        /// Gets all push devices by user identifier and device platform.
        /// <param name="userIdentifier">The user identifier.</param>
        /// <param name="devicePlatform">The device platform.</param>
        /// </summary>
        Task<List<TDevice>> GetDevicesByUserPlatformAsync(IUserIdentifier userIdentifier, string devicePlatform, int? skipCount = null, int? maxResultCount = null);

        /// <summary>
        /// Gets push devices count by user identifier.
        /// <param name="userIdentifier">The user identifier.</param>
        /// </summary>
        Task<int> GetDeviceCountByUserAsync(IUserIdentifier userIdentifier);

        /// <summary>
        /// Gets push device count by user identifier and service provider.
        /// <param name="userIdentifier">The user identifier.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// </summary>
        Task<int> GetDeviceCountByUserProviderAsync(IUserIdentifier userIdentifier, string serviceProvider);

        /// <summary>
        /// Gets push device count by user identifier and device platform.
        /// <param name="userIdentifier">The user identifier.</param>
        /// <param name="devicePlatform">The device platform.</param>
        /// </summary>
        Task<int> GetDeviceCountByUserPlatformAsync(IUserIdentifier userIdentifier, string devicePlatform);
    }
}