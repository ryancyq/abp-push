using System.Threading.Tasks;
using Abp.Push.Devices;

namespace Abp.Push.Providers
{
    public interface IPushProviderManager
    {
        Task<bool> IsValidDeviceAsync(string provider, string providerKey);

        Task<TDeviceInfo> GetDeviceInfoAsync<TDeviceInfo>(string provider, string providerKey) where TDeviceInfo : PushDeviceInfo;

        Task PushAsync<TPayload>(string provider, TPayload payload) where TPayload : PushPayload;
    }
}
