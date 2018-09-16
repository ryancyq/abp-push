using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Services;

namespace Abp.Push.Devices
{
    public interface IPushDeviceManager<TDevice> : IDomainService
        where TDevice : PushDeviceBase
    {
        /// <summary>
        /// Adds a push device.
        /// </summary>
        /// <param name="device">The device.</param>
        Task<bool> AddAsync(TDevice device);

        /// <summary>
        /// Find a push device by service provider & service provider key.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="serviceProviderKey">The service provider key.</param>
        /// <returns>The push device, if it is found</returns>
        Task<TDevice> FindAsync(string serviceProvider, string serviceProviderKey);

        /// <summary>
        /// Gets all push devices.
        /// </summary>
        Task<IReadOnlyList<TDevice>> GetAllAsync(int? skipCount = null, int? maxResultCount = null);

        /// <summary>
        /// Removes a push device
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>True, if a push device is removed</returns>
        Task<bool> RemoveAsync(TDevice device);

        /// <summary>
        /// Removes a push device by device identifier.
        /// </summary>
        /// <param name="deviceIdentifier">The device identifier.</param>
        /// <returns>True, if a push device is removed</returns>
        Task<bool> RemoveAsync(IDeviceIdentifier deviceIdentifier);

        /// <summary>
        /// Removes a push device by provider & provider key.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="serviceProviderKey">The service provider key.</param>
        /// <returns>True, if a push device is removed</returns>
        Task<bool> RemoveAsync(string serviceProvider, string serviceProviderKey);
    }
}