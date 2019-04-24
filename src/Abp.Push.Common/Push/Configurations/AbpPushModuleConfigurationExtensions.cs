﻿using Abp.Configuration.Startup;

namespace Abp.Push.Configurations
{
    public static class AbpPushConfigurationExtensions
    {
        /// <summary>
        /// Used to configure ABP Push module.
        /// </summary>
        public static IAbpPushConfiguration AbpPush(this IModuleConfigurations configurations)
        {
            return configurations.AbpConfiguration.Get<IAbpPushConfiguration>();
        }
    }
}