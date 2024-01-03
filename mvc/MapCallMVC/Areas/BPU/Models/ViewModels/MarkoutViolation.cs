using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.BPU.Models.ViewModels
{
    public class MarkoutViolationViewModel : ViewModel<MarkoutViolation>
    {
        #region Constructors

        public MarkoutViolationViewModel(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [StringLength(MarkoutViolation.StringLengths.MARKOUT_VIOLATION_STATUS)]
        public string MarkoutViolationStatus { get; set; }
        [Required, DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }
        [EntityMap, EntityMustExist(typeof(Town))]
        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        public int? Town { get; set; }
        [StringLength(MarkoutViolation.StringLengths.VIOLATION)]
        public string Violation { get; set; }
        public DateTime? DateOfViolationNotice { get; set; }
        [StringLength(MarkoutViolation.StringLengths.MARKOUT_REQUEST_NUMBER)]
        public string MarkoutRequestNumber { get; set; }
        [StringLength(MarkoutViolation.StringLengths.OC_NUMBER)]
        public string OCNumber { get; set; }
        [StringLength(MarkoutViolation.StringLengths.OPERATOR_OF_FACILITY)]
        public string OperatorOfFacility { get; set; }
        [StringLength(MarkoutViolation.StringLengths.LOCATION)]
        public string Location { get; set; }
        public DateTime? DateOfProbableViolation { get; set; }
        [StringLength(MarkoutViolation.StringLengths.MARKOUT_PERFORMED_BY)]
        public string MarkoutPerformedBy { get; set; }
        [StringLength(MarkoutViolation.StringLengths.ROOT_CAUSE)]
        public string RootCause { get; set; }
        public bool? Contest { get; set; }
        public decimal? FineAmount { get; set; }
        [EntityMap, EntityMustExist(typeof(WorkOrder))]
        [DropDown("FieldOperations", "WorkOrder", "ByTownId", PromptText = "Please select a town above.", DependsOn = "Town")]
        public int? WorkOrder { get; set; }
        [Coordinate, EntityMap, EntityMustExist(typeof(Coordinate))]
        public int? Coordinate { get; set; }

        #endregion
    }

    public class CreateMarkoutViolation : MarkoutViolationViewModel
    {
        #region Constructors

        public CreateMarkoutViolation(IContainer container) : base(container) { }

        #endregion
    }
    public class EditMarkoutViolation : MarkoutViolationViewModel
    {
        #region Constructors

        public EditMarkoutViolation(IContainer container) : base(container) { }

        #endregion
    }

    public class SearchMarkoutViolation : SearchSet<MarkoutViolation>
    {
        [View(DisplayName = "Id")]
        public int? EntityId { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int? Town { get; set; }

        public DateRange DateOfViolationNotice { get; set; }

        public SearchString Violation { get; set; }
        public SearchString MarkoutViolationStatus { get; set; }

    }
}
