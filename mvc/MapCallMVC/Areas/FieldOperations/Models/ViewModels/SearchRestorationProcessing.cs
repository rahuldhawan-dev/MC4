using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class SearchRestorationProcessing : SearchWorkOrder
    {
        #region Properties

        [DropDown, RequiredWhen(nameof(Id), ComparisonType.EqualTo, null)]
        public override int? OperatingCenter
        {
            get => base.OperatingCenter; 
            set => base.OperatingCenter = value;
        }

        [SearchAlias("Restorations", "PartialRestorationDate")]
        public DateRange InitialDate { get; set; }

        [SearchAlias("Restorations", "FinalRestorationDate")]
        public DateRange FinalDate { get; set; }

        [SearchAlias("WorkOrderDocuments", "CreatedAt")]
        public DateRange DocumentDate { get; set; }
        
        #endregion
    }
}