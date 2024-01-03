using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.SewerOverflows
{
    public class SewerOverflowViewModel : ViewModel<SewerOverflow>
    {
        #region Properties

        [Required,
         DropDown,
         DoesNotAutoMap, 
         EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [Required, 
         EntityMap, 
         EntityMustExist(typeof(OperatingCenter)),
         DropDown("", 
             "OperatingCenter", 
             "ActiveByStateIdOrAll", 
             DependsOn = nameof(State), 
             PromptText = "Select a state above")]
        public int? OperatingCenter { get; set; }

        [Required,
         EntityMap,
         EntityMustExist(typeof(WasteWaterSystem)),
         DropDown("Environmental",
             "WasteWaterSystem",
             "ByOperatingCenter",
             DependsOn = nameof(OperatingCenter),
             PromptText = "Select an operating center above")]
        public int? WasteWaterSystem { get; set; }

        [Required]
        public DateTime? IncidentDate { get; set; }

        [StringLength(SewerOverflow.StringLengths.TALKED_TO)]
        public string TalkedTo { get; set; }

        [StringLength(SewerOverflow.StringLengths.STREET_NUMBER)]
        public string StreetNumber { get; set; }

        [Required,
         EntityMap,
         EntityMustExist(typeof(Town)),
         DropDown("Town",
             "ByOperatingCenterId",
             DependsOn = nameof(OperatingCenter),
             PromptText = "Please select an operating center",
             Area = "")]
        public int? Town { get; set; }

        [EntityMap, EntityMustExist(typeof(Street))]
        [DropDown("Street", "ByTownId", DependsOn = nameof(Town), PromptText = "Please select a town", Area = "")]
        public int? Street { get; set; }

        [EntityMap, EntityMustExist(typeof(Street))]
        [DropDown("Street", "ByTownId", DependsOn = nameof(Town), PromptText = "Please select a town", Area = "")]
        public int? CrossStreet { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(Coordinate))]
        [Coordinate(AddressCallback = "SewerOverflows.getAddress", IconSet = IconSets.SingleDefaultIcon)]
        public int? Coordinate { get; set; }

        [Required]
        public int? GallonsOverflowedEstimated { get; set; }

        [Required]
        public int? SewageRecoveredGallons { get; set; }
        
        [Required, DropDown, EntityMap, EntityMustExist(typeof(SewerOverflowDischargeLocation))]
        public int? DischargeLocation { get; set; }
        
        [RequiredWhen(nameof(DischargeLocation), ComparisonType.EqualTo, SewerOverflowDischargeLocation.Indices.OTHER, FieldOnlyVisibleWhenRequired = true)]
        public string DischargeLocationOther { get; set; }
        
        [Required, DropDown, EntityMap, EntityMustExist(typeof(DischargeWeatherRelatedType))]
        public int? WeatherType { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(SewerOverflowType))]
        public int? OverflowType { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(SewerOverflowCause))]
        public int? OverflowCause { get; set; }

        [EntityMap,
         EntityMustExist(typeof(BodyOfWater)),
         DropDown("BodyOfWater",
             "ByOperatingCenterId",
             DependsOn = nameof(OperatingCenter),
             PromptText = "Please select an operating center.",
             Area = ""),
        RequiredWhen(nameof(DischargeLocation), ComparisonType.EqualTo, SewerOverflowDischargeLocation.Indices.BODY_OF_WATER, FieldOnlyVisibleWhenRequired = true)]
        public int? BodyOfWater { get; set; }

        [RequiredWhen(nameof(DischargeLocation), ComparisonType.EqualTo, SewerOverflowDischargeLocation.Indices.BODY_OF_WATER)]
        public int? GallonsFlowedIntoBodyOfWater { get; set; }

        [StringLength(SewerOverflow.StringLengths.ENFORCING_AGENCY_CASE_NUMBER)]
        [ClientCallback("SewerOverflows.validateEnforcingAgencyCaseNumber", ErrorMessage = "Required")]
        public string EnforcingAgencyCaseNumber { get; set; }

        [Required, DateTimePicker]
        public DateTime? CallReceived { get; set; }

        [Required, DateTimePicker]
        public DateTime? CrewArrivedOnSite { get; set; }

        [Required, DateTimePicker]
        public DateTime? SewageContained { get; set; }

        [Required, DateTimePicker]
        public DateTime? StoppageCleared { get; set; }

        [Required, DateTimePicker]
        public DateTime? WorkCompleted { get; set; }

        [Required, StringLength(SewerOverflow.StringLengths.LOCATION_OF_STOPPAGE)]
        public string LocationOfStoppage { get; set; }

        [EntityMap, EntityMustExist(typeof(SewerClearingMethod)), DropDown]
        public int? SewerClearingMethod { get; set; }

        public bool? OverflowCustomers { get; set; }

        [EntityMap, EntityMustExist(typeof(SewerOverflowArea)), DropDown]
        public int? AreaCleanedUpTo { get; set; }

        [EntityMap, EntityMustExist(typeof(ZoneType)), DropDown]
        public int? ZoneType { get; set; }

        [EntityMap,
         EntityMustExist(typeof(WorkOrder)),
         DropDown("FieldOperations",
             "WorkOrder",
             "ByTownId",
             PromptText = "Please select a town above.",
             DependsOn = nameof(Town))]
        public int? WorkOrder { get; set; }

        [AutoMap(MapDirections.None)]
        public string CriticalNotes => "N/A";

        #endregion

        #region Constructors

        public SewerOverflowViewModel(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override void Map(SewerOverflow entity)
        {
            base.Map(entity);
            
            // Old records can have a BodyOfWater without a DischargeLocation. The system can infer DischargeLocation from this, and set it for the user.
            if (DischargeLocation == null && BodyOfWater != null)
            {
                DischargeLocation = SewerOverflowDischargeLocation.Indices.BODY_OF_WATER;
            }
        }

        public override SewerOverflow MapToEntity(SewerOverflow entity)
        {
            // Ensure Discharge Locations "Other" and "Body of Water" remain mutually exclusive
            switch (DischargeLocation)
            {
                case SewerOverflowDischargeLocation.Indices.BODY_OF_WATER:
                    DischargeLocationOther = null;
                    break;
                case SewerOverflowDischargeLocation.Indices.OTHER:
                    BodyOfWater = null;
                    break;
            }

            return base.MapToEntity(entity);
        }

        #endregion
        
        #region Validation

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext) => 
            base.Validate(validationContext).Concat(ValidateEnforcingAgencyCaseNumber());

        private IEnumerable<ValidationResult> ValidateEnforcingAgencyCaseNumber()
        {
            // if the operating center has maximum gallons specified
            // and they have entered more than that into gallons flowed into the body of water they
            // must enter a enforcing agency case number.
            if (!OperatingCenter.HasValue)
            {
                yield break;
            }

            var operatingCenter = _container.GetInstance<IOperatingCenterRepository>().Find(OperatingCenter.Value);
            if (!operatingCenter.MaximumOverflowGallons.HasValue)
            {
                yield break;
            }

            if (GallonsOverflowedEstimated > operatingCenter.MaximumOverflowGallons &&
                string.IsNullOrWhiteSpace(EnforcingAgencyCaseNumber))
            {
                yield return new ValidationResult("Please enter the Enforcing Agency Case #", new[] {
                    nameof(EnforcingAgencyCaseNumber)
                });
            }
        }

        #endregion
    }
}
