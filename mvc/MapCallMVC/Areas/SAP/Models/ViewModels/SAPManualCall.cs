using System;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.SAP.Models.ViewModels
{
    public class SAPManualCall
    {
        public virtual DateTime? ManualCallDate { get; set; }
        public virtual string MaintenancePlan { get; set; }
        [DropDown]
        public virtual string MaintenancePackage { get; set; }

    }
}
