using Newtonsoft.Json;
using System;

namespace Abp.Push.Requests
{
    [Serializable]
    public class PushRequestEntityData : PushRequestData
    {
        /// <summary>
        /// Converted from EntityTypeName
        /// </summary>
        [JsonProperty(PropertyName = "name", NullValueHandling = NullValueHandling.Ignore)]
        public string EntityName { get; set; }

        [JsonProperty(PropertyName = "id", NullValueHandling = NullValueHandling.Ignore)]
        public string EntityId { get; set; }

        [JsonProperty(PropertyName = "event", NullValueHandling = NullValueHandling.Ignore)]
        public string EntityEvent { get; set; }
    }
}
