using System;

namespace Abp.Push.Providers
{
    public class PushDevicePlatformInfo
    {
        public string Name { get; set; }

        public Type PlatformResolverType { get; set; }

        public PushDevicePlatformInfo(string name, Type platformResolverType)
        {
            Name = name;
            PlatformResolverType = platformResolverType;
        }
    }
}
