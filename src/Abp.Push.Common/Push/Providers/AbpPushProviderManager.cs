using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Services;
using Abp.Push.Configurations;
using Abp.Push.Devices;

namespace Abp.Push.Providers
{
    public class AbpPushProviderManager : DomainService, IPushProviderManager, ITransientDependency
    {
        protected readonly IIocResolver IocResolver;
        protected readonly IPushConfiguration Configuration;

        public AbpPushProviderManager(
            IIocResolver iocResolver,
            IPushConfiguration pushConfiguration
            )
        {
            IocResolver = iocResolver;
            Configuration = pushConfiguration;
        }

        public virtual Task<bool> IsValidDeviceAsync(string provider, string providerKey)
        {
            using (var apiClient = CreateApiClient(provider))
            {
                return apiClient.Object.IsValidDeviceAsync(providerKey);
            }
        }

        public virtual Task<TDeviceInfo> GetDeviceInfoAsync<TDeviceInfo>(string provider, string providerKey)
            where TDeviceInfo : PushDeviceInfo
        {
            using (var apiClient = CreateApiClient(provider))
            {
                return apiClient.Object.GetDeviceInfoAsync<TDeviceInfo>(providerKey);
            }
        }

        public virtual Task PushAsync<TPayload>(string provider, TPayload payload)
            where TPayload : PushPayload
        {
            using (var providerApi = CreateApiClient(provider))
            {
                return providerApi.Object.PushAsync(payload);
            }
        }

        protected virtual IDisposableDependencyObjectWrapper<IPushApiClient> CreateApiClient(string provider)
        {
            var providerInfo = Configuration.ServiceProviders.FirstOrDefault(p => p.Name == provider);
            if (providerInfo == null)
            {
                throw new Exception("Unknown push service provider: " + provider);
            }

            var providerApi = IocResolver.ResolveAsDisposable<IPushApiClient>(providerInfo.ApiClientType);
            providerApi.Object.Initialize(providerInfo);
            return providerApi;
        }
    }
}
