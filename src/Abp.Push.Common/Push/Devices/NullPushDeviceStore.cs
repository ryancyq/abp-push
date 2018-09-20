using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abp.Push.Devices
{
    /// <summary>
    /// Null pattern implementation of <see cref="IPushDeviceStore{TDevice}"/>.
    /// </summary>
    public class NullPushDeviceStore : NullPushDeviceStore<PushDevice>, IPushDeviceStore
    {

    }

    /// <summary>
    /// Null pattern implementation of <see cref="IPushDeviceStore{TDevice}"/>.
    /// </summary>
    public class NullPushDeviceStore<TDevice> : IPushDeviceStore<TDevice>
        where TDevice : PushDevice
    {
        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static NullPushDeviceStore<TDevice> Instance { get { return SingletonInstance; } }
        private static readonly NullPushDeviceStore<TDevice> SingletonInstance = new NullPushDeviceStore<TDevice>();
        protected NullPushDeviceStore()
        {
        }

        public Task InsertDeviceAsync(TDevice device)
        {
            return Task.FromResult(0);
        }

        public Task UpdateDeviceAsync(TDevice device)
        {
            return Task.FromResult(0);
        }

        public Task InsertOrUpdateDeviceAsync(TDevice device)
        {
            return Task.FromResult(0);
        }

        public Task DeleteDeviceAsync(TDevice device)
        {
            return Task.FromResult(0);
        }

        public Task DeleteDeviceAsync(IDeviceIdentifier deviceIdentifier)
        {
            return Task.FromResult(0);
        }

        public Task DeleteDevicesByProviderAsync(string serviceProvider)
        {
            return Task.FromResult(0);
        }

        public Task DeleteDevicesByProviderAsync(string serviceProvider, string serviceProviderKey)
        {
            return Task.FromResult(0);
        }

        public Task DeleteDevicesByPlatformAsync(string devicePlatform)
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

        public Task<TDevice> GetDeviceOrNullAsync(string serviceProvider, string serviceProviderKey)
        {
            return Task.FromResult(null as TDevice);
        }

        public Task<TDevice> GetUserDeviceOrNullAsync(IUserIdentifier userIdentifier, string serviceProvider, string serviceProviderKey)
        {
            return Task.FromResult(null as TDevice);
        }

        public Task<List<TDevice>> GetDevicesAsync(int? skipCount = null, int? maxResultCount = null)
        {
            return Task.FromResult(new List<TDevice>());
        }

        public Task<List<TDevice>> GetDevicesByProviderAsync(string serviceProvider, int? skipCount = null, int? maxResultCount = null)
        {
            return Task.FromResult(new List<TDevice>());
        }

        public Task<List<TDevice>> GetDevicesByPlatformAsync(string devicePlatform, int? skipCount = null, int? maxResultCount = null)
        {
            return Task.FromResult(new List<TDevice>());
        }
        public Task DeleteDevicesByUserAsync(IUserIdentifier userIdentifier)
        {
            return Task.FromResult(0);
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