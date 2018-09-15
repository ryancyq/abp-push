namespace Abp.Push.Devices
{
    public class PushDevice : PushDeviceBase
    {
        /// <summary>
        /// Tenant Id.
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// User Id.
        /// </summary>
        public virtual long? UserId { get; set; }

        public override string ToString()
        {
            return $"tenantId: { TenantId } , userId: { UserId }, " + base.ToString();
        }
    }
}