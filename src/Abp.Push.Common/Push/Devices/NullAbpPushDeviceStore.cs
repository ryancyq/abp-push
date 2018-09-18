using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abp.Push.Devices
{
    /// <summary>
    /// Null pattern implementation of <see cref="IAbpPushDeviceStore{TDevice}"/>.
    /// </summary>
    public class NullAbpPushDeviceStore<TDevice> : NullPushDeviceStore<TDevice>, IAbpPushDeviceStore<TDevice>
        where TDevice : AbpPushDevice
    {
        protected NullAbpPushDeviceStore() : base()
        {
        }

        public Task DeleteDevicesByUserAsync(IUserIdentifier userIdentifier)
        {
            return Task.FromResult(0);
        }

        public Task DeleteDevicesByUserPlatformAsync(IUserIdentifier userIdentifier, string devicePlatform)
        {
            return Task.FromResult(0);
        }

        public Task DeleteDevicesByUserProviderAsync(IUserIdentifier userIdentifier, string serviceProvider)
        {
            return Task.FromResult(0);
        }

        public Task<TDevice> GetUserDeviceOrNullAsync(IUserIdentifier userIdentifier, string serviceProvider, string serviceProviderKey)
        {
            return Task.FromResult(null as TDevice);
        }

        public Task<List<TDevice>> GetDevicesByUserAsync(IUserIdentifier userIdentifier, int? skipCount = null, int? maxResultCount = null)
        {
            return Task.FromResult(new List<TDevice>());
        }

        public Task<List<TDevice>> GetDevicesByUserPlatformAsync(IUserIdentifier userIdentifier, string devicePlatform, int? skipCount = null, int? maxResultCount = null)
        {
            return Task.FromResult(new List<TDevice>());
        }

        public Task<List<TDevice>> GetDevicesByUserProviderAsync(IUserIdentifier userIdentifier, string serviceProvider, int? skipCount = null, int? maxResultCount = null)
        {
            return Task.FromResult(new List<TDevice>());
        }

        public Task<int> GetDeviceCountByUserAsync(IUserIdentifier userIdentifier)
        {
            return Task.FromResult(0);
        }

        public Task<int> GetDeviceCountByUserPlatformAsync(IUserIdentifier userIdentifier, string devicePlatform)
        {
            return Task.FromResult(0);
        }

        public Task<int> GetDeviceCountByUserProviderAsync(IUserIdentifier userIdentifier, string serviceProvider)
        {
            return Task.FromResult(0);
        }

    }
}