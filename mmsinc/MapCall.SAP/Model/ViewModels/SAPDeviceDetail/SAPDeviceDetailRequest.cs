using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MapCall.SAP.Model.ViewModels.SAPDeviceDetail
{
    public class SAPDeviceDetailRequest
    {
        [JsonProperty(PropertyName = "MSN")]
        public virtual string MeterSerialNumber { get; set; }
        public virtual string EquipmentId { get; set; }
        public virtual string ActionCode { get; set; }
        public virtual string DeviceLocation { get; set; }
        public virtual string DeviceType { get; set; }
    }
}
