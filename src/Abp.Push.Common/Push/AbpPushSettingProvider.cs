using System.Collections.Generic;
using Abp.Configuration;
using Abp.Localization;

namespace Abp.Push
{
    internal class AbpPushSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition(
                    AbpPushSettingNames.Receive,
                    "true",
                    L("AbpPushSettingNames.Receive"),
                    scopes: SettingScopes.User,
                    isVisibleToClients: true)
            };
        }

        protected virtual LocalizableString L(string name)
        {
            return new LocalizableString(name, AbpPushConsts.LocalizationSourceName);
        }
    }
}