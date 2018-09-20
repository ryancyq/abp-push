namespace Abp.Push.Devices
{
    public class PushDeviceInfo
    {
        public static PushDeviceInfo Empty = new PushDeviceInfo();

        public string ProviderKey { get; set; }
    }
}
