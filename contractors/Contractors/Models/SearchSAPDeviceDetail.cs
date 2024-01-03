using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.ViewModels.SAPDeviceDetail;
using MMSINC.Data;

namespace Contractors.Models
{
    public class SearchSAPDeviceDetail : SearchSet<SAPDeviceDetailRequest>
    {
        public int WorkOrderID { get; set; }
        public string MeterSerialNumber { get; set; }
        public string ActionCode => "I";
        public long? DeviceLocation { get; set; }
        public string DeviceType { get; set; }
    }
}