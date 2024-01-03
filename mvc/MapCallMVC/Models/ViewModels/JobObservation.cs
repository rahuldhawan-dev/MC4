using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using MapCall.Common.Metadata;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Models.ViewModels
{
    public class JobObservationViewModel : ViewModel<JobObservation>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(JobCategory))]
        [Required]
        public virtual int? Department { get; set; }
        [Required, Coordinate(AddressField = "Location"), EntityMap]
        [EntityMustExist(typeof(Coordinate))]
        public virtual int? Coordinate { get; set; }
        [EntityMustExist(typeof(OverallSafetyRating)), EntityMap, DropDown]
        [Required]
        public virtual int? OverallSafetyRating { get; set; }
        [EntityMustExist(typeof(OverallQualityRating)), EntityMap, DropDown]
        [Required]
        public virtual int? OverallQualityRating { get; set; }
        [Required, EntityMustExist(typeof(OperatingCenter)), EntityMap, DropDown]
        public virtual int? OperatingCenter { get; set; }
        [AutoComplete("Employee", "HealthAndSafetyActiveEmployeesByRoleAndPartial", DisplayProperty = nameof(EmployeeDisplayItem.Display)), Description("Enter the employee's first or last name to populate autocomplete list")]
        [EntityMap, EntityMustExist(typeof(Employee))]
        public int? JobObservedBy { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime? ObservationDate { get; set; }
        [Required, StringLength(JobObservation.StringLengths.ADDRESS)]
        public virtual string Address { get; set; }
        [Required, StringLength(JobObservation.StringLengths.DESCRIPTION)]
        public virtual string TaskObserved { get; set; }

        [EntityMap, EntityMustExist(typeof(WorkOrder))]
        public virtual int? WorkOrder { get; set; }

        [EntityMap, EntityMustExist(typeof(ProductionWorkOrder))]
        public virtual int? ProductionWorkOrder { get; set; }

        [StringLength(JobObservation.StringLengths.SUGGESTIONS_TO_EMPLOYEES)]
        public virtual string WhyWasTaskSafeOrAtRisk { get; set; }
        [StringLength(JobObservation.StringLengths.DEFICIENCIES)]
        public virtual string Deficiencies { get; set; }
        [StringLength(JobObservation.StringLengths.COMMENTS)]
        public virtual string RecommendSolutions { get; set; }
        public virtual bool? EqTruckForkliftsHoistsLadders { get; set; }
        public virtual bool? EqFrontEndLoaderOrBackhoe { get; set; }
        public virtual bool? EqOther { get; set; }
        public virtual bool? CsPreEntryChecklistOrEntryPermit { get; set; }
        public virtual bool? CsAtmosphereContinuouslyMonitored { get; set; }
        public virtual bool? CsRetrievalEquipmentTripodHarnessWinch { get; set; }
        public virtual bool? CsVentilationEquipment { get; set; }
        public virtual bool? PpeHardHat { get; set; }
        public virtual bool? PpeReflectiveVest { get; set; }
        public virtual bool? PpeEyeProtection { get; set; }
        public virtual bool? PpeEarProtection { get; set; }
        public virtual bool? PpeFootProtection { get; set; }
        public virtual bool? PpeGloves { get; set; }
        public virtual bool? TcBarricadesConesBarrels { get; set; }
        public virtual bool? TcAdvancedWarningSigns { get; set; }
        public virtual bool? TcLightsArrowBoard { get; set; }
        public virtual bool? TcPoliceFlagman { get; set; }
        public virtual bool? TcWorkZoneInCompliance { get; set; }
        public virtual bool? PsWalkwaysClear { get; set; }
        public virtual bool? PsMaterialStockpile { get; set; }
        public virtual bool? ExMarkoutRequestedForWorkSite { get; set; }
        public virtual bool? ExWorkSiteSafetyCheckListUtilized { get; set; }
        public virtual bool? ExUtilitiesSupportedProtected { get; set; }
        public virtual bool? ExAtmosphereTestingPerformed { get; set; }
        public virtual bool? ExSpoilPile2FeetFromEdgeOfExcavation { get; set; }
        public virtual bool? ExLadderUsedIfGreaterThan4FeetDeep { get; set; }
        public virtual bool? ExShoringNecessaryOver5FeetDeep { get; set; }
        public virtual bool? ExProtectiveSystemInUseOver5Feet { get; set; }
        public virtual bool? ExWaterControlSystemInUse { get; set; }
        public virtual bool? ErChecklistUtilized { get; set; }
        public virtual bool? ErErgonomicFactorsProhibitingGoodBodyMechanics { get; set; }
        public virtual bool? ErToolsEquipmentUsedCorrectly { get; set; }

        #endregion

        #region Constructors

        public JobObservationViewModel(IContainer container) : base(container) {}

        #endregion

        #region Public Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext)
                       .Concat(ValidateOperatingCenter());
        }

        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidateOperatingCenter()
        {
            if (WorkOrder.HasValue)
            {
                var wo = _container.GetInstance<IWorkOrderRepository>().Find(WorkOrder.Value);
                if (wo != null && wo.OperatingCenter.Id != OperatingCenter.Value)
                {
                    yield return new ValidationResult("The Job Observation Operating Center must match the Work Order Operating Center.", new[] { nameof(OperatingCenter) });
                }
            }
        }

        #endregion
    }

    public class CreateJobObservation : JobObservationViewModel
    {
        public CreateJobObservation(IContainer container) : base(container) { }

        [AutoMap(MapDirections.None)]
        public string CoordinateCreateUrl { get; set; }

        public override void SetDefaults()
        {
            ObservationDate = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            if (WorkOrder.HasValue) // tested via the controller new method
            {
                var wo = _container.GetInstance<IWorkOrderRepository>().Find(WorkOrder.Value);
                if (wo != null)
                {
                    var coordinate = _container.GetInstance<IRepository<Coordinate>>().Save(new Coordinate {
                        Latitude = Convert.ToDecimal(wo.Latitude),
                        Longitude = Convert.ToDecimal(wo.Longitude),
                        Icon = _container.GetInstance<IIconSetRepository>().GetDefaultIconSet(_container.GetInstance<IRepository<MapIcon>>()).DefaultIcon
                    });

                    OperatingCenter = wo.OperatingCenter.Id;
                    Address = string.Format("{0} {1}", wo.StreetAddress, wo.TownAddress);
                    Coordinate = coordinate.Id;
                    TaskObserved = wo.WorkDescription.ToString();
                    if (wo.AssignedContractor != null)
                        Department = JobCategory.Indices.CONTRACTOR;
                    else
                        Department = JobCategory.Indices.T_AND_D;
                }
            }
            if (ProductionWorkOrder.HasValue)
            {
                var pwo = _container.GetInstance<IProductionWorkOrderRepository>()
                    .Find(ProductionWorkOrder.Value);
                if (pwo != null)
                {
                    var coordinate = _container.GetInstance<IRepository<Coordinate>>().Save(new Coordinate
                    {
                        Latitude = Convert.ToDecimal(pwo.Coordinate.Latitude),
                        Longitude = Convert.ToDecimal(pwo.Coordinate.Longitude),
                        Icon = _container.GetInstance<IIconSetRepository>().GetDefaultIconSet(_container.GetInstance<IRepository<MapIcon>>()).DefaultIcon
                    });

                    Department = MapCall.Common.Model.Entities.JobCategory.Indices.PRODUCTION;
                    OperatingCenter = pwo.OperatingCenter.Id;
                    Address = pwo.Facility.ToString();
                    Coordinate = coordinate.Id;
                    TaskObserved = pwo.ProductionWorkDescription.ToString();
                }
            }
        }
    }

    public class EditJobObservation : JobObservationViewModel
    {
        public EditJobObservation(IContainer container) : base(container) { }
    }

    public class SearchJobObservation : SearchSet<JobObservation>
    {
        #region Properties

        [DisplayName("JobObservationID")]
        public int? EntityId { get; set; }

        [EntityMap, EntityMustExist(typeof(State))]
        [DropDown, SearchAlias("OperatingCenter", "State.Id", Required = true)]
        public virtual int? State { get; set; }

        [MultiSelect("", "OperatingCenter", "ByStateIdForHealthAndSafety", DependsOn = "State", DependentsRequired = DependentRequirement.None)]
        [EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int[] OperatingCenter { get; set; }

        [AutoComplete("Employee", "HealthAndSafetyActiveEmployeesByRoleAndPartial"), Description("Enter the employee's first or last name to populate autocomplete list")]
        [EntityMap, EntityMustExist(typeof(Employee))]
        public int? JobObservedBy { get; set; }

        [EntityMap, EntityMustExist(typeof(OperatingCenter))]
        [DropDown, SearchAlias("CreatedBy", "cb", "DefaultOperatingCenter.Id")]
        public int? CreatedByOperatingCenter { get; set; }

        [DropDown("", "User", "GetAllByOperatingCenterId", DependsOn = "CreatedByOperatingCenter", PromptText = "Please select an operating center above")]
        public int? CreatedBy { get; set; }

        public DateRange ObservationDate { get; set; }
        [DropDown]
        public int? OverallSafetyRating { get; set; }
        public string Address { get; set; }
        public string TaskObserved { get; set; }
        [DropDown]
        public int? Department { get; set; }

        #endregion
    }
}