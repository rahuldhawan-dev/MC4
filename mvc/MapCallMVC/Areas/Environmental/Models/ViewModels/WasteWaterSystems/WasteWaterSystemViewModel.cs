using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.WasteWaterSystems
{
    public class WasteWaterSystemViewModel : ViewModel<WasteWaterSystem>
    {
        #region Properties

        [DropDown, Required, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [DropDown("", nameof(BusinessUnit), "FindByOperatingCenterIdForWasteWaterOrCFS", DependsOn = "OperatingCenter"), Required, EntityMap, EntityMustExist(typeof(BusinessUnit))]
        public int? BusinessUnit { get; set; }

        [Required, StringLength(WasteWaterSystem.StringLengths.WASTE_WATER_SYSTEM_NAME)]
        public string WasteWaterSystemName { get; set; }

        [Required, StringLength(WasteWaterSystem.StringLengths.PERMIT_NUMBER)]
        public string PermitNumber { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(WasteWaterSystemStatus))]
        public int? Status { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(WasteWaterSystemOwnership))]
        public int? Ownership { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(WasteWaterSystemType))]
        public int? Type { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(WasteWaterSystemSubType))]
        public int? SubType { get; set; }

        public DateTime? DateOfOwnership { get; set; }

        public DateTime? DateOfResponsibility { get; set; }

        public int? GravityLength { get; set; }

        public int? ForceLength { get; set; }

        public int? NumberOfLiftStations { get; set; }

        [StringLength(WasteWaterSystem.StringLengths.TREATMENT_DESCRIPTION)]
        public string TreatmentDescription { get; set; }

        public int? NumberOfCustomers { get; set; }

        public int? PeakFlowMGD { get; set; }

        public bool? IsCombinedSewerSystem { get; set; }

        [Required]
        public bool? HasConsentOrder { get; set; }

        [RequiredWhen(nameof(HasConsentOrder), ComparisonType.EqualTo, true)]
        public DateTime? ConsentOrderStartDate { get; set; }

        [RequiredWhen(nameof(ConsentOrderStartDate), ComparisonType.NotEqualTo, null)]
        public DateTime? ConsentOrderEndDate { get; set; }

        public DateTime? NewSystemInitialSafetyAssessmentCompleted { get; set; }

        public DateTime? DateSafetyAssessmentActionItemsCompleted { get; set; }

        public DateTime? NewSystemInitialWQEnvAssessmentCompleted { get; set; }

        public DateTime? DateWQEnvAssessmentActionItemsCompleted { get; set; }
        
        [DoesNotAutoMap("Done manually in Map. Needed for cascading.")]
        public int[] PlanningPlant { get; set; }

        #endregion

        #region Constructors

        public WasteWaterSystemViewModel(IContainer container) : base(container) { }

        #endregion
        
        #region Public Methods

        public override void Map(WasteWaterSystem entity)
        {
            base.Map(entity);
            
            PlanningPlant = entity.PlanningPlantWasteWaterSystems.Select(x => x.PlanningPlant.Id).ToArray();
        }

        #endregion
    }
}
