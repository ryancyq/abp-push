using System;

namespace Abp.Push.Devices
{
    public class DevicePlatformInfo
    {
        public string Name { get; set; }

        public Type PlatformResolverType { get; set; }

        public DevicePlatformInfo(string name, Type platformResolverType)
        {
            Name = name;
            PlatformResolverType = platformResolverType;
        }
    }
}
