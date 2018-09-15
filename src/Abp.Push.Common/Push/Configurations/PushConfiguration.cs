using System.Collections.Generic;
using Abp.Collections;
using Abp.Dependency;
using Abp.Push.Providers;

namespace Abp.Push.Configurations
{
    internal class PushConfiguration : IPushConfiguration, ISingletonDependency
    {
        public ITypeList<PushDefinitionProvider> DefinitionProviders { get; private set; }

        public List<PushServiceProviderInfo> ServiceProviders { get; private set; }

        public PushConfiguration()
        {
            DefinitionProviders = new TypeList<PushDefinitionProvider>();
            ServiceProviders = new List<PushServiceProviderInfo>();
        }
    }
}
