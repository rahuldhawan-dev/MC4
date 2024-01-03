using MMSINC.Data;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.ViewModels.SAPDeviceDetail;

namespace MapCallMVC.Areas.SAP.Models.ViewModels
{
    public class SearchSAPDeviceDetail : SearchSet<SAPDeviceDetailRequest>
    {
        public int WorkOrderID { get; set; }
        public string MeterSerialNumber { get; set; }
        public string ActionCode => "I";
        public string DeviceLocation { get; set; }
        public string DeviceType { get; set; }
    }
}