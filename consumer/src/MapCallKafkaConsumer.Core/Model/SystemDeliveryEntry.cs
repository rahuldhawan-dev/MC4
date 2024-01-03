using Newtonsoft.Json;

namespace MapCallKafkaConsumer.Model
{
    /// <summary>
    /// Naming class as its mvc equivalent. Would be nice to
    /// rename kafka msg prop names too.
    /// <see cref="MapCall.Common.Model.Entities.SystemDeliveryEntry"/>
    /// </summary>
    public class SystemDeliveryEntry
    {
        [JsonProperty("tags")]
        public SystemDeliveryFacilityEntry[] FacilityEntries { get; set; }
        
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }
}
