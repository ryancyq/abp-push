using System;

namespace Abp.Push.Providers
{
    public class ServiceProviderInfo
    {
        public string Name { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public Type ProviderType { get; set; }

        public Type ApiClientType { get; set; }

        public ServiceProviderInfo(string name, Type providerType, Type apiClientType, string clientId, string clientSecret)
        {
            Name = name;
            ProviderType = providerType;
            ApiClientType = apiClientType;
            ClientId = clientId;
            ClientSecret = clientSecret;
        }
    }
}
