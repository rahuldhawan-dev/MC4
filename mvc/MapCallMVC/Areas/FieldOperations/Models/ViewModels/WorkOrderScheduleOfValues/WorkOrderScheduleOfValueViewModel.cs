using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;
using System.ComponentModel.DataAnnotations;
using MMSINC.Utilities.ObjectMapping;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderScheduleOfValues
{
    public class WorkOrderScheduleOfValueViewModel : ViewModel<WorkOrderScheduleOfValue>
    {
        #region Constructors

        public WorkOrderScheduleOfValueViewModel(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [Required, EntityMap, EntityMustExist(typeof(WorkOrder))]
        public int? WorkOrder { get; set; }

        [Required, DropDown, EntityMap(MapDirections.None), EntityMustExist(typeof(ScheduleOfValueCategory))]
        public int? ScheduleOfValueCategory { get; set; }

        [DropDown("FieldOperations", "ScheduleOfValue", "ByScheduleOfValueCategoryId", DependsOn = "ScheduleOfValueCategory", PromptText = "Please select a schedule of value category above")]
        [Required, EntityMap, EntityMustExist(typeof(ScheduleOfValue))]
        public int? ScheduleOfValue { get; set; }

        [CheckBox]
        public bool? IsOvertime { get; set; }

        public decimal? Total { get; set; }

        [RequiredWhen(nameof(ScheduleOfValueCategory), ComparisonType.EqualTo, MapCall.Common.Model.Entities.ScheduleOfValueCategory.Indices.OTHER)]
        public string OtherDescription { get; set; }

        #endregion
    }
}
