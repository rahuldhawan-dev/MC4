using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.ViewModels
{
    public class OpenIssuedServicesReportItem
    {
        public int Id { get; set; }

        [DisplayName("Priority")]
        public ServicePriority ServicePriority { get; set; }

        public ServiceRestorationContractor WorkIssuedTo { get; set; }
        public DateTime? DateIssuedToField { get; set; }
        public virtual long? ServiceNumber { get; set; }
        public virtual string CompleteStAddress { get; set; }
        public virtual Town Town { get; set; }
        public ServiceCategory ServiceCategory { get; set; }

        [DisplayName("Purpose of Installation")]
        public virtual ServiceInstallationPurpose ServiceInstallationPurpose { get; set; }

        public virtual Street CrossStreet { get; set; }
        public virtual string PermitNumber { get; set; }

        [DisplayName("Job Notes")]
        public virtual string TapOrderNotes { get; set; }

        public virtual string PurchaseOrderNumber { get; set; }

        [DoesNotExport]
        public virtual string RecordUrl { get; set; }
    }

    public class ServiceQualityAssuranceReportItem
    {
        public OperatingCenter OperatingCenter { get; set; }
        public ServiceCategory Category { get; set; }
        public int Total { get; set; }
        public int CustomerSideMaterialUnknown { get; set; }
        public int PreviousServiceMaterialUnknown { get; set; }
        public int ServiceMaterialUnknown { get; set; }

        public int MissingTapImages { get; set; }
        public bool IsTapImageReport { get; set; }
        public bool IsRenewal { get; set; }
    }

    public interface ISearchServiceQAReport : ISearchSet<ServiceQualityAssuranceReportItem>
    {
        DateRange DateInstalled { get; set; }
    }
}
