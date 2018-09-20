using System;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Push.Devices;

namespace Abp.Push.Providers
{
    public abstract class PushApiClientBase : AbpServiceBase, IPushApiClient, ITransientDependency
    {
        public ServiceProviderInfo ProviderInfo { get; set; }

        public void Initialize(ServiceProviderInfo providerInfo)
        {
            ProviderInfo = providerInfo;
        }

        public virtual async Task<bool> IsValidDeviceAsync(string providerKey)
        {
            if (string.IsNullOrWhiteSpace(providerKey))
            {
                return false;
            }
            var deviceInfo = await GetDeviceInfoAsync<PushDeviceInfo>(providerKey);
            return deviceInfo.ProviderKey == providerKey;
        }

        public abstract Task<TDeviceInfo> GetDeviceInfoAsync<TDeviceInfo>(string providerKey) where TDeviceInfo : PushDeviceInfo;

        public abstract Task PushAsync<TPayload>(TPayload payload) where TPayload : PushPayload;

        protected virtual string ToBase64Utf8(string source)
        {
            return ToBase64(source, Encoding.UTF8);
        }

        protected virtual string ToBase64(string source, Encoding encoding)
        {
            return Convert.ToBase64String(encoding.GetBytes(source));
        }
    }
}
