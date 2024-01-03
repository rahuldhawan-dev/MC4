using Newtonsoft.Json;

namespace MapCallKafkaConsumer.Model
{
    /// <summary>
    /// This has no mvc equivalent. Not sure if we need to store this info for anything. 
    /// </summary>
    public class SystemDeliveryHeader
    {
        [JsonProperty("producertime")]
        public string ProducerTime { get; set; }

        [JsonProperty("sub_category")]
        public string SubCategory { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("producerSvr")]
        public string ProducerServer { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }
    }
}
