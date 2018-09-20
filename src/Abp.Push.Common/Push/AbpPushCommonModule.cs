using Abp.Dependency;
using Abp.Modules;
using Abp.Push.Localization;
using Abp.Push.Devices;
using Abp.Push.Requests;
using Abp.Reflection.Extensions;

namespace Abp.Push
{
    [DependsOn(
        typeof(AbpKernelModule)
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
        }

        /// <inheritdoc/>
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AbpPushCommonModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.RegisterIfNot<IPushDeviceStore, NullPushDeviceStore>(DependencyLifeStyle.Singleton);
            IocManager.RegisterIfNot<IPushRequestStore, NullPushRequestStore>(DependencyLifeStyle.Singleton);

            IocManager.Resolve<PushDefinitionManager>().Initialize();
        }
    }
}
