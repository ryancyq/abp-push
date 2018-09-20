using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Push.Devices;
using Abp.Push.Requests;

namespace Abp.Push.Providers
{
    public abstract class PushServiceProviderBase : AbpServiceBase, IPushServiceProvider, ITransientDependency
    {
        public ServiceProviderInfo ProviderInfo { get; set; }

        public void Initialize(ServiceProviderInfo providerInfo)
        {
            ProviderInfo = providerInfo;
        }

        public abstract Task PushAsync(IUserIdentifier[] userIdentifiers, PushRequest pushRequest);

        public abstract Task PushAsync(IDeviceIdentifier[] deviceIdentifiers, PushRequest pushRequest);
    }
}
