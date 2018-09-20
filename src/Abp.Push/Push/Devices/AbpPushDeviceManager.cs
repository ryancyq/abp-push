using Abp.Push.Configurations;

namespace Abp.Push.Devices
{
    public class AbpPushDeviceManager : AbpPushDeviceManager<PushDevice>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbpPushDeviceManager"/> class.
        /// </summary>
        protected AbpPushDeviceManager(
            IPushDeviceStore<PushDevice> deviceStore,
            IPushConfiguration pushConfiguration
        ) : base(
            deviceStore,
            pushConfiguration
        )
        {
        }
    }
}