using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MapCallScheduler.JobHelpers.SmartCoverAlert.Models
{
    public class SmartCoverAlertResultAlarms
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public decimal? Value { get; set; }
        public string Date { get; set; }
    }
}
