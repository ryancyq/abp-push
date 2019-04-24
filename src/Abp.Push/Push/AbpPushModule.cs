using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Modules;
using Abp.Push.Requests;
using Abp.Reflection.Extensions;

namespace Abp.Push
{
    [DependsOn(typeof(AbpPushCommonModule))]
    public class AbpPushModule : AbpModule
    {
        /// <inheritdoc/>
        public override void PreInitialize()
        {
            Configuration.ReplaceService<IPushRequestStore, AbpInMemoryPushRequestStore>(DependencyLifeStyle.Transient);
        }

        /// <inheritdoc/>
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AbpPushModule).GetAssembly());
        }
    }
}
