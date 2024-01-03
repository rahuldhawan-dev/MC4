using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using StructureMap;

namespace MapCallMVC.Areas.FleetManagement.Models
{
    public class VehicleEZPassViewModel : ViewModel<VehicleEZPass>
    {
        #region Properties

        [Required, StringLength(VehicleEZPass.StringLengths.SERIAL_NUMBER)]
        public string EZPassSerialNumber { get; set; }

        [Required, StringLength(VehicleEZPass.StringLengths.BILLING_INFO)]
        public string BillingInfo { get; set; }

        #endregion

        #region Constructors

        public VehicleEZPassViewModel(IContainer container) : base(container) { }

        #endregion
    }
}