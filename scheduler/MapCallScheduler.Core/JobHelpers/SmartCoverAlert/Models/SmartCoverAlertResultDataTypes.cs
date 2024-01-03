using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCallScheduler.JobHelpers.SmartCoverAlert.Models
{
    public class SmartCoverAlertResultDataTypes
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        [JsonProperty(PropertyName = "last_reading")]
        public string[] LastReading { get; set; }
    }
}
