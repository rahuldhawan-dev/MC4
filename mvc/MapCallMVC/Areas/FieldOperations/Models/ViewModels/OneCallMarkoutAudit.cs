using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class SearchOneCallMarkoutAudit : SearchSet<OneCallMarkoutAudit>
    {
        #region Properties

        public DateRange DateTransmitted { get; set; }
        public DateRange DateReceived { get; set; }
        public bool? Success { get; set; }

        #endregion
	}
}