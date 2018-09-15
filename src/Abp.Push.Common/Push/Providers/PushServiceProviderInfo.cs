using System;

namespace Abp.Push.Providers
{
    public class PushServiceProviderInfo
    {
        public string Name { get; set; }

        public Type ProviderType { get; set; }

        public PushServiceProviderInfo(string name, Type providerType)
        {
            Name = name;
            ProviderType = providerType;
        }
    }
}
