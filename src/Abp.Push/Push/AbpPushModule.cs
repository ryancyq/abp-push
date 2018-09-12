using Abp;
using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Abp.Push
{
    [DependsOn(
        typeof(AbpKernelModule),
        typeof(AbpAutoMapperModule)
        )]
    public class AbpPushModule : AbpModule
    {
        /// <inheritdoc/>
        public override void PreInitialize()
        {
            //Configure settings
            Configuration.Settings.Providers.Add<PushSettingProvider>();

            //Configure localizations
            AbpPushLocalizationConfigurer.Configure(Configuration.Localization);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg =>
            {
                cfg.AddProfiles(typeof(AbpPushModule).GetAssembly());
            });
        }

        /// <inheritdoc/>
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AbpPushModule).GetAssembly());
        }
    }
}
