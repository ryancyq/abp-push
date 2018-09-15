using System;
using System.Collections.Generic;
using Abp.Collections.Extensions;
using Abp.Json;

namespace Abp.Push.Devices
{
    /// <summary>
    /// Used to store info for a push device.
    /// It can be directly used or can be derived.
    /// </summary>
    [Serializable]
    public class PushDeviceData
    {
        /// <summary>
        /// Gets push device info data type name.
        /// It returns the full class name by default.
        /// </summary>
        public virtual string Type => GetType().FullName;

        /// <summary>
        /// Shortcut to set/get <see cref="Properties"/>.
        /// </summary>
        public object this[string key]
        {
            get { return Properties.GetOrDefault(key); }
            set { Properties[key] = value; }
        }

        /// <summary>
        /// Can be used to add custom properties to this push device.
        /// </summary>
        public Dictionary<string, object> Properties
        {
            get { return _properties; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                /* Not assign value, but add dictionary items. This is required for backward compability. */
                foreach (var keyValue in value)
                {
                    if (!_properties.ContainsKey(keyValue.Key))
                    {
                        _properties[keyValue.Key] = keyValue.Value;
                    }
                }
            }
        }
        private readonly Dictionary<string, object> _properties;

        /// <summary>
        /// Createa a new PushDeviceData object.
        /// </summary>
        public PushDeviceData()
        {
            _properties = new Dictionary<string, object>();
        }

        public override string ToString()
        {
            return this.ToJsonString();
        }
    }
}
