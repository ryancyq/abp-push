namespace Abp.Push.Devices
{
    public abstract class PushDeviceBase : IHasDeviceInfo<string>
    {
        /// <summary>
        /// Device Platform.
        /// </summary>
        public virtual string DevicePlatform { get; set; }

        /// <summary>
        /// Device identifier.
        /// </summary>
        public virtual string DeviceIdentifier { get; set; }

        /// <summary>
        /// Normalized device identifier.
        /// </summary>
        public virtual string NormalizedDeviceIdentifier { get; private set; }

        /// <summary>
        /// Device name.
        /// </summary>
        public virtual string DeviceName { get; set; }

        /// <summary>
        /// Normalized device name.
        /// </summary>
        public virtual string NormalizedDeviceName { get; private set; }

        /// <summary>
        /// Provider type.
        /// </summary>
        public virtual string Provider { get; set; }

        /// <summary>
        /// Provider key.
        /// Follows convention in <see cref="https://msdn.microsoft.com/en-us/library/microsoft.aspnet.identity.userclientinfo.providerkey(v=vs.108).aspx"/>
        /// </summary>
        public virtual string ProviderKey { get; set; }

        /// <summary>
        /// data payload as JSON string. (optional)
        /// </summary>
        public virtual PushDeviceData Data { get; set; }

        /// <summary>
        /// Type of the JSON serialized <see cref="Data"/>.
        /// It's AssemblyQualifiedName of the type.
        /// </summary>
        public virtual string DataTypeName { get; set; }
    }
}
