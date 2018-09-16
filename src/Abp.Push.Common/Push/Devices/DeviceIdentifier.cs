using System;
using System.Reflection;
using Abp.Extensions;

namespace Abp.Push.Devices
{
    /// <summary>
    /// Used to identify a device.
    /// </summary>
    [Serializable]
    public class DeviceIdentifier : IDeviceIdentifier
    {
        /// <summary>
        /// Tenant Id of the device.
        /// Can be null for host devices in a multi tenant application.
        /// </summary>
        public int? TenantId { get; protected set; }

        /// <summary>
        /// Id of the device.
        /// </summary>
        public Guid DeviceId { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceIdentifier"/> class.
        /// </summary>
        protected DeviceIdentifier()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceIdentifier"/> class.
        /// </summary>
        /// <param name="tenantId">Tenant Id of the device.</param>
        /// <param name="deviceId">Id of the device.</param>
        public DeviceIdentifier(int? tenantId, Guid deviceId)
        {
            TenantId = tenantId;
            DeviceId = deviceId;
        }

        /// <summary>
        /// Parses given string and creates a new <see cref="DeviceIdentifier"/> object.
        /// </summary>
        /// <param name="deviceIdentifierString">
        /// Should be formatted one of the followings:
        /// 
        /// - "deviceId@tenantId". Ex: "42@3" (for tenant devices).
        /// - "deviceId". Ex: 1 (for host devices)
        /// </param>
        public static DeviceIdentifier Parse(string deviceIdentifierString)
        {
            if (deviceIdentifierString.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(deviceIdentifierString), "deviceAtTenant can not be null or empty!");
            }

            var splitted = deviceIdentifierString.Split('@');
            if (splitted.Length == 1)
            {
                return new DeviceIdentifier(null, splitted[0].To<Guid>());

            }

            if (splitted.Length == 2)
            {
                return new DeviceIdentifier(splitted[1].To<int>(), splitted[0].To<Guid>());
            }

            throw new ArgumentException("deviceAtTenant is not properly formatted", nameof(deviceIdentifierString));
        }

        /// <summary>
        /// Creates a string represents this <see cref="DeviceIdentifier"/> instance.
        /// Formatted one of the followings:
        /// 
        /// - "deviceId@tenantId". Ex: "42@3" (for tenant devices).
        /// - "deviceId". Ex: 1 (for host devices)
        /// 
        /// Returning string can be used in <see cref="Parse"/> method to re-create identical <see cref="DeviceIdentifier"/> object.
        /// </summary>
        public string ToDeviceIdentifierString()
        {
            if (TenantId == null)
            {
                return DeviceId.ToString();
            }

            return DeviceId + "@" + TenantId;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is DeviceIdentifier))
            {
                return false;
            }

            //Same instances must be considered as equal
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            //Transient objects are not considered as equal
            var other = (DeviceIdentifier)obj;

            //Must have a IS-A relation of types or must be same type
            var typeOfThis = GetType();
            var typeOfOther = other.GetType();
            if (!typeOfThis.GetTypeInfo().IsAssignableFrom(typeOfOther) && !typeOfOther.GetTypeInfo().IsAssignableFrom(typeOfThis))
            {
                return false;
            }

            return TenantId == other.TenantId && DeviceId == other.DeviceId;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var bigInteDeviceId = new System.Numerics.BigInteger(DeviceId.ToByteArray());
            return TenantId == null ? (int)bigInteDeviceId : (int)(TenantId.Value ^ bigInteDeviceId);
        }

        /// <inheritdoc/>
        public static bool operator ==(DeviceIdentifier left, DeviceIdentifier right)
        {
            if (Equals(left, null))
            {
                return Equals(right, null);
            }

            return left.Equals(right);
        }

        /// <inheritdoc/>
        public static bool operator !=(DeviceIdentifier left, DeviceIdentifier right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return ToDeviceIdentifierString();
        }
    }
}
