using Newtonsoft.Json;

namespace MapCallKafkaConsumer.Model
{
    public class SystemDelivery
    {
        [JsonProperty("Header")]
        public SystemDeliveryHeader SystemDeliveryHeader { get; set; }
        
        [JsonProperty("Value")]
        public SystemDeliveryEntry SystemDeliveryEntry { get; set; }
    }
}
