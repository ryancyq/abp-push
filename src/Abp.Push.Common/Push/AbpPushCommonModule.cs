using Abp.AutoMapper;
using Abp.Modules;
using Abp.Push.Localization;
using Abp.Reflection.Extensions;

namespace Abp.Push
{
    [DependsOn(
        typeof(AbpKernelModule),
        typeof(AbpAutoMapperModule)
        )]
    public class AbpPushCommonModule : AbpModule
    {
        /// <inheritdoc/>
        public override void PreInitialize()
        {
            //Configure settings
            Configuration.Settings.Providers.Add<AbpPushSettingProvider>();

            //Configure localizations
            AbpPushLocalizationConfigurer.Configure(Configuration.Localization);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg =>
            {
                cfg.AddProfiles(typeof(AbpPushCommonModule).GetAssembly());
            });
        }

        /// <inheritdoc/>
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AbpPushCommonModule).GetAssembly());
        }
    }
}
