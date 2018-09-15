using System.Collections.Generic;
using Abp.Configuration;
using Abp.Localization;

namespace Abp.Push
{
    public class AbpPushSettingProvider : SettingProvider
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

        private static LocalizableString L(string name)
        {
            return new LocalizableString(name, AbpPushConsts.LocalizationSourceName);
        }
    }
}