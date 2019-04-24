namespace Abp.Push.Devices
{
    public static class PushDeviceExtensions
    {
        public static UserIdentifier ToUserIdentifierOrNull(this AbpPushDevice device)
        {
            return device.UserId.HasValue ? new UserIdentifier(device.TenantId, device.UserId.Value) : null;
        }
    }
}