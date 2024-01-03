using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels
{
    public class EmergencyResponsePlanViewModel : ViewModel<EmergencyResponsePlan>
    {
        #region Properties

        // State
        [DropDown, Required]
        [EntityMustExist(typeof(State))]
        [EntityMap]
        public int? State { get; set; }

        [DisplayName("Operating Center"), DropDown, Required]
        [EntityMustExist(typeof(OperatingCenter))]
        [EntityMap]
        public int? OperatingCenter { get; set; }

        // Facility
        [EntityMustExist(typeof(Facility))]
        [EntityMap]
        [DropDown("", "Facility", "GetByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center.")]
        public int? Facility { get; set; }

        [DropDown, View("Plan Category")]
        [Required, EntityMustExist(typeof(EmergencyPlanCategory)), EntityMap]
        public int? EmergencyPlanCategory { get; set; }

        [Required, StringLength(EmergencyResponsePlan.StringLengths.TITLE)]
        public string Title { get; set; }

        [Required, Multiline]
        public string Description { get; set; }

        [DropDown]
        [EntityMustExist(typeof(ReviewFrequency)), EntityMap]
        public int? ReviewFrequency { get; set; }

        #endregion

        #region Constructors

        public EmergencyResponsePlanViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateEmergencyResponsePlan : EmergencyResponsePlanViewModel
    {
        #region Constructors

        public CreateEmergencyResponsePlan(IContainer container) : base(container) { }

        #endregion
    }

    public class EditEmergencyResponsePlan : EmergencyResponsePlanViewModel
    {
        #region Constructors

        public EditEmergencyResponsePlan(IContainer container) : base(container) { }

        #endregion
    }

    public class SearchEmergencyResponsePlan : SearchSet<EmergencyResponsePlan>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        public int? State { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }
        [DropDown, View("Plan Category"), EntityMap, EntityMustExist(typeof(EmergencyPlanCategory))]
        public int? EmergencyPlanCategory { get; set; }
        public string Title { get; set; }
        [DropDown]
        public int? ReviewFrequency { get; set; }

        #endregion
    }
}