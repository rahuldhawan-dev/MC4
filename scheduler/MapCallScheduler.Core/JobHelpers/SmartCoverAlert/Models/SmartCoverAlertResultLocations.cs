using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCallScheduler.JobHelpers.SmartCoverAlert.Models
{
    public class SmartCoverAlertResultLocations
    {
        public string Id { get; set; }
        [JsonProperty(PropertyName = "custom_key")]
        public string CustomKey { get; set; }
        public string Description { get; set; }
        public string Application { get; set; }
        [JsonProperty(PropertyName = "application_description")]
        public string ApplicationDescription { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Elevation { get; set; }
        [JsonProperty(PropertyName = "sensor_to_bottom")]
        public decimal? SensorToBottom { get; set; }
        [JsonProperty(PropertyName = "manhole_depth")]
        public decimal? ManholeDepth { get; set; }
        [JsonProperty(PropertyName = "data_types")]
        public SmartCoverAlertResultDataTypes[] DataTypes { get; set; }
        public SmartCoverAlertResultAlarms[] Alarms { get; set; }
        public SmartCoverAlertResultAlerts[] Alerts { get; set; }
        [JsonProperty(PropertyName = "high_alarm_threshold")]
        public decimal? HighAlarmThreshold { get; set; }
    }
}