using System.Collections.Generic;
using Abp.Collections;
using Abp.Push.Providers;

namespace Abp.Push.Configurations
{
    /// <summary>
    /// Used to configure push system.
    /// </summary>
    public interface IPushConfiguration
    {
        ITypeList<PushDefinitionProvider> DefinitionProviders { get; }

        List<PushServiceProviderInfo> ServiceProviders { get; }
    }
}
