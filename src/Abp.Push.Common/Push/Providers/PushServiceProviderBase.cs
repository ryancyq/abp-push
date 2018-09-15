using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Push.Requests;
using Castle.Core.Logging;

namespace Abp.Push.Providers
{
    public abstract class PushServiceProviderBase : IPushServiceProvider, ITransientDependency
    {
        public PushServiceProviderInfo ProviderInfo { get; set; }

        public ILogger Logger { protected get; set; }

        public void Initialize(PushServiceProviderInfo providerInfo)
        {
            ProviderInfo = providerInfo;

            Logger = NullLogger.Instance;
        }

        public abstract Task PushAsync(IUserIdentifier[] userIdentifiers, PushRequestInfo pushRequestInfo);
    }
}
