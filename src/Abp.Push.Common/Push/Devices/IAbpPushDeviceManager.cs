using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abp.Push.Devices
{
    public interface IAbpPushDeviceManager<TDevice> : IPushDeviceManager<TDevice>
        where TDevice : AbpPushDevice
    {
        /// <summary>
        /// Gets all push device by user identifier.
        /// </summary>
        Task<IReadOnlyList<TDevice>> GetAllByUserIdAsync(IUserIdentifier userIdentifier, int? skipCount = null, int? maxResultCount = null);

        /// <summary>
        /// Gets all push devices by user id and provider.
        /// </summary>
        Task<IReadOnlyList<TDevice>> GetAllByUserIdProviderAsync(IUserIdentifier userIdentifier, string serviceProvider, int? skipCount = null, int? maxResultCount = null);

        /// <summary>
        /// Gets total count of all push devices by user id.
        /// </summary>
        Task<int> GetDeviceCountByUserIdAsync(IUserIdentifier userIdentifier);

        /// <summary>
        /// Gets total count of all push devices by user id and provider.
        /// </summary>
        Task<int> GetDeviceCountByUserIdProviderAsync(IUserIdentifier userIdentifier, string serviceProvider);

        /// <summary>
        /// Removes all push devices by user identifier.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        Task RemoveAllByUserIdAsync(IUserIdentifier userIdentifier);
    }
}