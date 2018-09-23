using System;
using Abp.Dependency;
using Abp.Domain.Repositories;

namespace Abp.Push.Devices
{
    /// <summary>
    /// Implements <see cref="IPushDeviceStore"/> using repositories.
    /// </summary>
    internal class AbpPushDeviceStore : AbpPushDeviceStore<PushDevice>, IPushDeviceStore, ITransientDependency
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbpPushDeviceStore"/> class.
        /// </summary>
        public AbpPushDeviceStore(
            IRepository<PushDevice, long> deviceRepository
        ) : base(
            deviceRepository
        )
        {
        }
    }
}