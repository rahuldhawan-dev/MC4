using Newtonsoft.Json;

namespace MapCallKafkaConsumer.Model
{
    public class SystemDeliveryFacilityEntry
    {
        [JsonProperty("unit")]
        public string UnitOfMeasure { get; set; }

        [JsonProperty("system")]
        public int SystemDeliveryType { get; set; }

        [JsonProperty("last_change_timestamp")]
        public long EntryDate { get; set; } // var dateTime = DateTimeOffset.FromUnixTimeSeconds(EntryDate);

        [JsonProperty("name")]
        public string FacilityName { get; set; }

        [JsonProperty("type")]
        public int SystemDeliveryEntryType { get; set; }

        [JsonProperty("value")]
        public string EntryValue { get; set; }

        [JsonProperty("mapcallid")]
        public int? FacilityId { get; set; }
    }
}
