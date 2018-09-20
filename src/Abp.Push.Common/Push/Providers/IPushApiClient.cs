using System.Threading.Tasks;
using Abp.Push.Devices;

namespace Abp.Push.Providers
{
    public interface IPushApiClient
    {
        ServiceProviderInfo ProviderInfo { get; }

        void Initialize(ServiceProviderInfo providerInfo);

        Task<bool> IsValidDeviceAsync(string providerKey);

        Task<TDeviceInfo> GetDeviceInfoAsync<TDeviceInfo>(string providerKey) where TDeviceInfo : PushDeviceInfo;

        Task PushAsync<TPayload>(TPayload payload) where TPayload : PushPayload;
    }
}
