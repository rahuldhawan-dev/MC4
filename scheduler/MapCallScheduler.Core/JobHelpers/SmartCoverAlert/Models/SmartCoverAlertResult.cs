using Newtonsoft.Json;

namespace MapCallScheduler.JobHelpers.SmartCoverAlert.Models
{
    public class SmartCoverAlertResult
    {
        [JsonProperty(PropertyName = "response_code")]
        public int? ResponseCode { get; set; }
        public SmartCoverAlertResultLocations[] Locations { get; set; }
    }
}
