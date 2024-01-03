using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.SAP.Model.Entities;
using Newtonsoft.Json;

namespace MapCall.SAP.Model.ViewModels.SAPDeviceDetail
{
    public class SAPDeviceDetailResponse : SAPDeviceDetailRequest
    {
        public virtual string MaterialNumber { get; set; }
        public virtual string DeviceCategory { get; set; }
        public virtual string ReadType { get; set; }
        public virtual string Installation { get; set; }
        public virtual string SourceIdentifier { get; set; }
        public virtual IEnumerable<SAPRegisterStructure> RegisterDetails { get; set; }
        public virtual string ManufacturerSerial { get; set; }
        public virtual string Size { get; set; }
        public virtual string Manufacturer { get; set; }
        public virtual string ServiceType { get; set; }
        public virtual string MeterPosition { get; set; }
        public virtual string MeterDirection { get; set; }
        public virtual string MeterSupplemental { get; set; }
        [JsonProperty(PropertyName = "ReadDirLoc")]
        public virtual string ReadingDeviceDirectionalLocation { get; set; }
        [JsonProperty(PropertyName = "ReadPosLoc")]
        public virtual string ReadingDevicePositionalLocation { get; set; }
        [JsonProperty(PropertyName = "ReadSuppLoc")]
        public virtual string ReadingDeviceSupplementalLocation { get; set; }
        public virtual string DeviceLocationNotes { get; set; }
        public virtual string PointOfControl { get; set; }
        [JsonProperty(PropertyName = "MIU1")]
        public virtual string MUINumber1 { get; set; }
        [JsonProperty(PropertyName = "MIU2")]
        public virtual string MUINumber2 { get; set; }
        public virtual string MIU1DeviceCategory { get; set; }
        public virtual string MIU2DeviceCategory { get; set; }
        [JsonProperty(PropertyName = "MIU1WSdate")]
        public virtual string MUI1WsDate { get; set; }
        [JsonProperty(PropertyName = "MIU2WSdate")]
        public virtual string MUI2WsDate { get; set; }
        [JsonProperty(PropertyName = "MIU1WEdate")]
        public virtual string MUI1WeDate { get; set; }
        [JsonProperty(PropertyName = "MIU2WEdate")]
        public virtual string MUI2WeDate { get; set; }
        public virtual string MUI1DataPlan { get; set; }
        public virtual string MUI2DataPlan { get; set; }
        public virtual string EncoderId1 { get; set; }
        public virtual string EncoderId2 { get; set; }
        public virtual string RFMIU { get; set; }
        public virtual string TPEncoder { get; set; }
        public virtual string ReadTypeDescription { get; set; }
        public virtual string RegisterGroup { get; set; }
        public virtual string SystemStatus { get; set; }
        public virtual string UserStatus { get; set; }
        public virtual string SecureAccess { get; set; }
        public virtual string AMIDevice { get; set; }
        public virtual IEnumerable<SAPStatus> ReturnStatuses { get; set; }

        #region Logical Properties

        public virtual string MapCallServiceType
        {
            get
            {
                switch (ServiceType)
                {
                    case "FS":
                        return "Fire Service";
                    case "SW":
                        return "Waste Water";
                    case "WT":
                        return "Water Service";
                    default:
                        return string.Empty;
                }
            }
        }

        public virtual string MapCallMeterSize
        {
            get
            {
                switch (Size)
                {
                    case "0001":
                        return "5/8\" Water Meter Size";
                    case "0002":
                        return "3/4\" Water Meter Size";
                    case "0003":
                        return "1\" Water Meter Size";
                    case "0004":
                        return "1-1/2\" Water Meter Si";
                    case "0005":
                        return "2\" Water Meter Size";
                    case "0006":
                        return "3\" Water Meter Size";
                    case "0007":
                        return "4\" Water Meter Size";
                    case "0008":
                        return "6\" Water Meter Size";
                    case "0009":
                        return "8\" Water Meter Size";
                    case "0010":
                        return "10\" Water Meter Size";
                    case "0011":
                        return "12\" Water Meter Size";
                    case "0012":
                        return "1/2\" Water Meter Size";
                    case "0013":
                        return "14\" Water Meter Size";
                    case "0014":
                        return "16\" Water Meter Size";
                    case "0015":
                        return "5\" Water Meter Size";

                    default:
                        return string.Empty;
                }
            }
        }

        #endregion
    }
}
