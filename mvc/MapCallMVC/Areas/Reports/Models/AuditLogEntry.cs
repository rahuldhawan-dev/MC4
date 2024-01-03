using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchAuditLogEntryUnion : SearchSet<AuditLogEntry>
    {
        #region Properties

        [DropDown]
        public int? User { get; set; }
        
        public DateRange Timestamp { get; set; }

        #endregion
    }
}