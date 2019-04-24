using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Abp.Push.Devices
{
    [Table("AbpPushDevices")]
    public abstract class AbpPushDevice : CreationAuditedEntity<long>, IHasDeviceInfo<Guid>, IMayHaveTenant
    {
        /// <summary>
        /// Maximum length of <see cref="DeviceName"/> property.
        /// Value: 512.
        /// </summary>
        public const int MaxDeviceNameLength = 512;

        /// <summary>
        /// Maximum length of <see cref="DevicePlatform"/> property.
        /// Value: 512.
        /// </summary>
        public const int MaxDevicePlatformLength = 256;

        /// <summary>
        /// Maximum length of <see cref="DeviceIdentifier"/> property.
        /// Value: 1024 (1 kB).
        /// </summary>
        public const int MaxDeviceIdentifierLength = 1024;

        /// <summary>
        /// Maximum length of <see cref="ServiceProvider"/> property.
        /// Value: 1024 (1 kB).
        /// </summary>
        public const int MaxProviderLength = 512;

        /// <summary>
        /// Maximum length of <see cref="ServiceProviderKey"/> property.
        /// Value: 1024 (1 kB).
        /// </summary>
        public const int MaxProviderKeyLength = 1024;

        /// <summary>
        /// Maximum length of <see cref="Data"/> property.
        /// Value: 1048576 (1 MB).
        /// </summary>
        public const int MaxDataLength = 1024 * 1024;

        /// <summary>
        /// Maximum length of <see cref="DataTypeName"/> property.
        /// Value: 512.
        /// </summary>
        public const int MaxDataTypeNameLength = 512;

        /// <summary>
        /// Tenant Id.
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// User Id.
        /// </summary>
        public virtual long? UserId { get; set; }

        /// <summary>
        /// Device Platform.
        /// </summary>
        [MaxLength(MaxDevicePlatformLength)]
        public virtual string DevicePlatform { get; set; }

        /// <summary>
        /// Device name.
        /// </summary>
        [MaxLength(MaxDeviceNameLength)]
        public virtual string DeviceName { get; set; }

        /// <summary>
        /// Normalized device name.
        /// </summary>
        [MaxLength(MaxDeviceNameLength)]
        public virtual string NormalizedDeviceName { get; private set; }

        /// <summary>
        /// Device identifier.
        /// </summary>
        public virtual Guid DeviceIdentifier { get; set; }

        /// <summary>
        /// Service Provider type.
        /// </summary>
        [MaxLength(MaxProviderLength)]
        public virtual string ServiceProvider { get; set; }

        /// <summary>
        /// Service Provider key.
        /// Follows convention in <see cref="https://msdn.microsoft.com/en-us/library/microsoft.aspnet.identity.userclientinfo.providerkey(v=vs.108).aspx"/>
        /// </summary>
        [MaxLength(MaxProviderKeyLength)]
        public virtual string ServiceProviderKey { get; set; }

        /// <summary>
        /// data payload as JSON string. (optional)
        /// </summary>
        [MaxLength(MaxDataLength)]
        public virtual PushDeviceData Data { get; set; }

        /// <summary>
        /// Expiration time.
        /// </summary>
        public virtual DateTime? ExpirationTime { get; set; }

        /// <summary>
        /// Last Access time.
        /// </summary>
        public virtual DateTime? LastAccessTime { get; set; }

        /// <summary>
        /// Type of the JSON serialized <see cref="Data"/>.
        /// It's AssemblyQualifiedName of the type.
        /// </summary>
        public virtual string DataTypeName { get; set; }

        public virtual void SetNormalizedNames()
        {
            NormalizedDeviceName = DeviceName?.ToUpperInvariant();
        }

        public override string ToString()
        {
            return $"tenantId: { TenantId } , userId: { UserId }, platform: { DevicePlatform } , identifier: { DeviceIdentifier } , serviceProvider: { ServiceProvider } , serviceProviderKey: { ServiceProviderKey }";
        }
    }
}
