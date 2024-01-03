using Newtonsoft.Json;

namespace MapCallScheduler.JobHelpers.SmartCoverAlert.Models
{
    public class SmartCoverAlertAcknowledgeResult
    {
        [JsonProperty(PropertyName = "response_code")]
        public int ResponseCode { get; set; }
        [JsonProperty(PropertyName = "response_text")]
        public string ResponseText { get; set; }
    }
}
