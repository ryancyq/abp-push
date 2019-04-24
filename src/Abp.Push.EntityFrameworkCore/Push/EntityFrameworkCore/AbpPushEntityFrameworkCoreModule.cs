using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Abp.Push
{
    [DependsOn(typeof(AbpPushModule))]
    public class AbpPushEntityFrameworkCoreModule : AbpModule
    {
        /// <inheritdoc/>
        public override void PreInitialize()
        {
        }

        /// <inheritdoc/>
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AbpPushEntityFrameworkCoreModule).GetAssembly());
        }
    }
}
