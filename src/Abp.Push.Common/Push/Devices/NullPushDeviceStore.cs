using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abp.Push.Devices
{
    /// <summary>
    /// Null pattern implementation of <see cref="IPushDeviceStore{TDevice}"/>.
    /// </summary>
    public class NullPushDeviceStore<TDevice> : IPushDeviceStore<TDevice>
        where TDevice : PushDeviceBase
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

        public Task<TDevice> GetDeviceOrNullAsync(string serviceProvider, string serviceProviderKey)
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
    }
}