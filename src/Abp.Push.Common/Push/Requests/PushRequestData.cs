using System;
using System.Collections.Generic;
using Abp.Collections.Extensions;
using Abp.Json;

namespace Abp.Push.Requests
{
    /// <summary>
    /// Used to store data for a push request.
    /// It can be directly used or can be derived.
    /// </summary>
    [Serializable]
    public abstract class PushRequestData
    {
        /// <summary>
        /// Gets push request data type name.
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
        /// Can be used to add custom properties to this request.
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
        /// Createa a new RequestData object.
        /// </summary>
        protected PushRequestData()
        {
            _properties = new Dictionary<string, object>();
        }

        public override string ToString()
        {
            // TODO: loop reference during serialization
            return this.ToJsonString();
        }
    }
}