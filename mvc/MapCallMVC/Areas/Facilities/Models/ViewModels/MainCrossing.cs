using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.ClassExtensions.WorkOrderAssetViewModelExtensions;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using DataType = System.ComponentModel.DataAnnotations.DataType;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels
{
    public class MainCrossingViewModel : ViewModel<MainCrossing>
    {
        #region Properties

        public virtual bool? IsCompanyOwned { get; set; }
        [DisplayName("Length of GIS Main Segment")]
        public virtual decimal? LengthOfSegment { get; set; }
        public virtual bool? IsCriticalAsset { get; set; }
        [DisplayName("Maximum Daily Flow(MGD)")]
        public virtual decimal? MaximumDailyFlow { get; set; }
        [DataType(DataType.MultilineText)]
        public virtual string Comments { get; set; }

        [DisplayName("Operating Center"), DropDown, Required]
        [EntityMustExist(typeof(OperatingCenter))]
        [EntityMap]
        public virtual int? OperatingCenter { get; set; }

        [DisplayName("Town")]
        [DropDown("Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center", Area = "")]
        [EntityMustExist(typeof(Town))]
        [EntityMap]
        public virtual int? Town { get; set; }

        [Coordinate, DisplayName("Coordinates")]
        [EntityMustExist(typeof(Coordinate))]
        [EntityMap("Coordinate")]
        public virtual int? CoordinateId { get; set; }

        [DisplayName("Body Of Water")]
        [DropDown("BodyOfWater", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center", Area = "")]
        [EntityMustExist(typeof(BodyOfWater))]
        [EntityMap]
        public virtual int? BodyOfWater { get; set; }

        [DisplayName("Crossing Category"), DropDown, Required]
        [EntityMustExist(typeof(CrossingCategory))]
        [EntityMap]
        public virtual int? CrossingCategory { get; set; }

        [DisplayName("Pressure Zone"), DropDown]
        [EntityMustExist(typeof(PressureZone))]
        [EntityMap]
        public virtual int? PressureZone { get; set; }
        
        [DisplayName("Number of Customers"), DropDown]
        [EntityMustExist(typeof(CustomerRange))]
        [EntityMap]
        public virtual int? CustomerRange { get; set; }

        [DisplayName("PWSID")]
        [DropDown("", "PublicWaterSupply", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above.")]
        [EntityMustExist(typeof(PublicWaterSupply))]
        [EntityMap("PWSID")]
        public virtual int? PWSID { get; set; }

        [DisplayName("Pipe Material"), DropDown]
        [EntityMustExist(typeof(PipeMaterial))]
        [EntityMap]
        public virtual int? MainMaterial { get; set; }

        [DisplayName("Pipe Diameter"), DropDown]
        [EntityMustExist(typeof(PipeDiameter))]
        [EntityMap]
        public virtual int? MainDiameter { get; set; }

        [DisplayName("Support Structure"), DropDown]
        [EntityMustExist(typeof(SupportStructure))]
        [EntityMap]
        [RequiredWhen("CrossingType", "GetElevatedCrossingType", typeof(MainCrossingViewModel))]
        public virtual int? SupportStructure { get; set; }

        [DisplayName("Crossing Type"), DropDown]
        [EntityMustExist(typeof(CrossingType))]
        [EntityMap]
        public virtual int? CrossingType { get; set; }

        [DisplayName("Construction Type"), DropDown]
        [EntityMustExist(typeof(ConstructionType))]
        [EntityMap]
        public virtual int? ConstructionType { get; set; }

        [Required]
        public int? InspectionFrequency { get; set; }

        [Required, DropDown]
        [EntityMustExist(typeof(RecurringFrequencyUnit))]
        [EntityMap]
        public int? InspectionFrequencyUnit { get; set; }

        [Required, DropDown, EntityMap]
        [EntityMustExist(typeof(MainCrossingStatus))]
        public int? MainCrossingStatus { get; set; }

        [DropDown, EntityMap]
        [EntityMustExist(typeof(MainCrossingInspectionType))]
        public int? InspectionType { get; set; }

        [AutoMap(MapDirections.ToPrimary)]
        public virtual bool RequiresInspection { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(MainInCasingStatus))]
        [RequiredWhen("CrossingCategory", ComparisonType.EqualToAny, "GetHighwayOrRailroadCrossingCategoryIds", typeof(MainCrossingViewModel), ErrorMessage = "The Main in Casing field is required.")]
        public virtual int? MainInCasing { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(RailwayOwnerType))]
        public int? RailwayOwnerType { get; set; }

        [StringLength(MainCrossing.StringLengths.RAILWAY_CROSSING_ID)]
        public string RailwayCrossingId { get; set; }

        [StringLength(MainCrossing.StringLengths.EMERGENCY_PHONE_NUMBER)]
        public string EmergencyPhoneNumber { get; set; }

        [BoolFormat("Yes", "No", "Operations update required")]
        public bool? IsolationValves { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(AssetCategory))]
        public int? AssetCategory { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(MainCrossingConsequenceOfFailureType))]
        public int? ConsequenceOfFailure { get; set; }

        [MultiSelect, EntityMap, EntityMustExist(typeof(MainCrossingImpactToType))]
        public virtual int[] ImpactTo { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(PressureSurgePotentialType))]
        public int? PressureSurgePotentialType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(TypicalOperatingPressureRange))]
        public int? TypicalOperatingPressureRange { get; set; }

        #endregion

        #region Constructors

        public MainCrossingViewModel(IContainer container) : base(container) { }

        #endregion

        public static int GetElevatedCrossingType()
        {
            return MapCall.Common.Model.Entities.CrossingType.Indices.ELEVATED;
        }

        public static int[] GetHighwayOrRailroadCrossingCategoryIds()
        {
            return new[] {
                MapCall.Common.Model.Entities.CrossingCategory.Indices.HIGHWAY,
                MapCall.Common.Model.Entities.CrossingCategory.Indices.RAILROAD
            };
        }
    }

    public class SearchMainCrossing : SearchSet<MainCrossing>
    {
        #region Properties

        [Search(CanMap = false)]
        public bool IsRelatedAssetSearch { get; set; }

        [DisplayName("Main Crossing ID")]
        public int? EntityId { get; set; }

        [MultiSelect]
        public int[] OperatingCenter { get; set; }

        [DropDown("Town", "ByOperatingCenterIds", DependsOn = "OperatingCenter", PromptText = "Please select at least one operating center above.", Area= "")]
        public int? Town { get; set; }
        [DisplayName("Body Of Water")]
        //[Cascading("BodyOfWater", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center", Area = "")]
        [DropDown]
        public virtual int? BodyOfWater { get; set; }

        [DropDown("Street", "ByTownId", DependsOn = "Town", PromptText = "Please select a town above.", Area = "")]
        public virtual int? Street { get; set; }

        public bool? IsCriticalAsset { get; set; }

        public bool? RequiresInspection { get; set; }

        public DateRange LastInspectedOn { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(MainCrossingStatus))]
        public int? MainCrossingStatus { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(CrossingCategory))]
        public int? CrossingCategory { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(MainCrossingConsequenceOfFailureType))]
        public int? ConsequenceOfFailure { get; set; }

        [DropDown]
        public int? InspectionType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(MainInCasingStatus))]
        public int? MainInCasing { get; set; }
        public bool? HasWorkOrder { get; set; }

        public bool? IsolationValves { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(PressureSurgePotentialType))]
        public int? PressureSurgePotentialType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(TypicalOperatingPressureRange))]
        public int? TypicalOperatingPressureRange { get; set; }
        
        [DropDown, EntityMap, EntityMustExist(typeof(CrossingType))]
        public int? CrossingType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(SupportStructure))]
        public int? SupportStructure { get; set; }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }

    public class CreateMainCrossing : MainCrossingViewModel
    {
        #region Properties


        [DisplayName("Closest Cross Street"), Required]
        [DropDown("Street", "GetActiveByTownId", DependsOn = "Town", PromptText = "Please select a Town.", Area = "")]
        [EntityMustExist(typeof(Street))]
        [EntityMap]
        public virtual int? ClosestCrossStreet { get; set; }

        [DisplayName("Street"), Required]
        [DropDown("Street", "GetActiveByTownId", DependsOn = "Town", PromptText = "Please select a Town.", Area = "")]
        [EntityMustExist(typeof(Street))]
        [EntityMap]
        public virtual int? Street { get; set; }

        #endregion

        #region Constructors

        public CreateMainCrossing(IContainer container) : base(container)
        {
            // A new Main Crossing needs to default to 1 Year. Pre-selecting it.
            InspectionFrequency = 1;
            InspectionFrequencyUnit = _container.GetInstance<IRecurringFrequencyUnitRepository>().GetYear().Id;
        }

        #endregion
    }

    public class EditMainCrossing : MainCrossingViewModel
    {
        #region Properties
        
        [DisplayName("Closest Cross Street"), Required]
        [DropDown("Street", "ByTownId", DependsOn = "Town", PromptText = "Please select a Town.", Area = "")]
        [EntityMustExist(typeof(Street))]
        [EntityMap]
        public virtual int? ClosestCrossStreet { get; set; }

        [DisplayName("Street"), Required]
        [DropDown("Street", "ByTownId", DependsOn = "Town", PromptText = "Please select a Town.", Area = "")]
        [EntityMustExist(typeof(Street))]
        [EntityMap]
        public virtual int? Street { get; set; }

        public DateTime? DateRetired { get; set; }

        #endregion

        #region Constructors

        public EditMainCrossing(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override MainCrossing MapToEntity(MainCrossing entity)
        {
            this.MaybeCancelWorkOrders(entity, _container.GetInstance<IRepository<WorkOrderCancellationReason>>(),
                t => t.DateRetired);

            return base.MapToEntity(entity);
        }

        #endregion
    }
}