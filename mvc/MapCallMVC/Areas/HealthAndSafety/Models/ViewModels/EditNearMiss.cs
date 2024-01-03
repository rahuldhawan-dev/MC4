using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.HealthAndSafety.Models.ViewModels
{
    public class EditNearMiss : ViewModel<NearMiss>
    {
        #region Private Members

        private NearMiss _displayNearMiss;

        #endregion

        #region Properties

        [CheckBox]
        public bool? HaveReviewedNearMiss { get; set; }

        [Required, DropDown, EntityMustExist(typeof(State)), EntityMap(MapDirections.None)]
        public int? State { get; set; }

        [DropDown(
            "",
            "OperatingCenter",
            "ByStateId",
            DependsOn = nameof(State),
            PromptText = "Select a State above")]
        [Required, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [DropDown(
            "",
            "Town",
            "ByOperatingCenterId",
            DependsOn = nameof(OperatingCenter), PromptText = "Select an Operating Center above")]
        [EntityMap, EntityMustExist(typeof(Town))]
        public int? Town { get; set; }

        [DropDown(
            "",
            "Facility",
            "ByOperatingCenterId",
            DependsOn = nameof(OperatingCenter),
            PromptText = "Select an OperatingCenter above")]
        [EntityMap, EntityMustExist(typeof(Facility))]
        public int? Facility { get; set; }

        [EntityMap, EntityMustExist(typeof(ActionTakenType)), DropDown]
        [RequiredWhen(
            nameof(Type),
            ComparisonType.EqualTo,
            NearMissType.Indices.SAFETY,
            FieldOnlyVisibleWhenRequired = true)]
        public int? ActionTakenType { get; set; }

        [StringLength(NearMiss.StringLengths.ACTION_TAKEN, MinimumLength = 5)]
        public string ActionTaken { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(WorkOrderType))]
        public int? WorkOrderType { get; set; }

        [EntityMap, EntityMustExist(typeof(WorkOrder))]
        public int? WorkOrder { get; set; }

        [EntityMap, EntityMustExist(typeof(ProductionWorkOrder))]
        public int? ProductionWorkOrder { get; set; }

        public int? ShortCycleWorkOrderNumber { get; set; }

        [StringLength(NearMiss.StringLengths.WORK_ORDER_NUMBER)]
        public string WorkOrderNumber { get; set; }

        [StringLength(NearMiss.StringLengths.LOCATION_DETAILS)]
        public string LocationDetails { get; set; }

        [EntityMustExist(typeof(Coordinate)), EntityMap]
        [Coordinate(AddressField = "LocationDetails", IconSet = IconSets.NearMiss)]
        public int? Coordinate { get; set; }

        [CheckBox]
        public bool? SeriousInjuryOrFatality { get; set; } = false;

        [DropDown(DependsOn = nameof(Type)), EntityMap, EntityMustExist(typeof(LifeSavingRuleType))]
        public int? LifeSavingRuleType { get; set; }

        [CheckBox]
        [ClientCallback(
            "NearMiss.validateSeverity",
            ErrorMessage = "Was a Stop Work Authority Performed? is required.")] 
        public bool? StopWorkAuthorityPerformed { get; set; }

        [RequiredWhen(
            nameof(StopWorkAuthorityPerformed),
            ComparisonType.EqualTo,
            true,
            FieldOnlyVisibleWhenRequired = true)]
        [DropDown, EntityMap, EntityMustExist(typeof(StopWorkUsageType))]
        public int? StopWorkUsageType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(SeverityType))]
        [RequiredWhen(
            nameof(Type),
            ComparisonType.EqualTo,
            NearMissType.Indices.SAFETY,
            FieldOnlyVisibleWhenRequired = true)]
        public string Severity { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(NearMissType))]
        public int? Type { get; set; }

        [Required, Multiline, StringLength(int.MaxValue)]
        public string Description { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(NearMissCategory))]
        [DropDown("HealthAndSafety", "NearMissCategory", "ByType", DependsOn = nameof(Type))]
        public int? Category { get; set; }

        [RequiredWhen(
            nameof(Category),
            ComparisonType.EqualTo,
            NearMissCategory.Indices.OTHER,
            FieldOnlyVisibleWhenRequired = true)]
        [StringLength(NearMiss.StringLengths.DESCRIBE_OTHER)]
        public string DescribeOther { get; set; }

        [DropDown("HealthAndSafety", "NearMissSubCategory", "ByCategory", DependsOn = nameof(Category))]
        [EntityMap, EntityMustExist(typeof(NearMissSubCategory))]
        public int? SubCategory { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(SystemType))]
        [RequiredWhen(
            nameof(Type),
            ComparisonType.EqualTo,
            NearMissType.Indices.ENVIRONMENTAL,
            FieldOnlyVisibleWhenRequired = true)]
        public int? SystemType { get; set; }

        [DropDown(
            "",
            "PublicWaterSupply",
            "GetSystemNameByOperatingCenter",
            DependsOn = nameof(OperatingCenter),
            PromptText = "Select an Operating Center above")]
        [EntityMap, EntityMustExist(typeof(PublicWaterSupply))]
        public int? PublicWaterSupply { get; set; }

        [DropDown(
            "Environmental",
            "WasteWaterSystem",
            "GetSystemNameByOperatingCenter",
            DependsOn = nameof(OperatingCenter),
            PromptText = "Select an Operating Center above")]
        [EntityMap, EntityMustExist(typeof(WasteWaterSystem))]
        public int? WasteWaterSystem { get; set; }

        [RequiredWhen(
            nameof(Type),
            ComparisonType.EqualTo,
            NearMissType.Indices.ENVIRONMENTAL,
            FieldOnlyVisibleWhenRequired = true)]
        public bool? CompletedCorrectiveActions { get; set; }

        [CheckBox]
        public bool? ReportedToRegulator { get; set; }

        public DateTime? DateCompleted { get; set; }

        [DoesNotAutoMap]
        public NearMiss DisplayNearMiss =>
            _displayNearMiss ??
            (_displayNearMiss = _container.GetInstance<IRepository<NearMiss>>().Find(Id));

        #endregion

        #region Constructors

        public EditNearMiss(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override void Map(NearMiss entity)
        {
            if (entity.OperatingCenter != null)
            {
                State = entity.OperatingCenter.State.Id;
            }

            if (!string.IsNullOrEmpty(entity.Severity))
            {
                Severity = _container.GetInstance<ISeverityTypeRepository>()
                                     .Where(x => x.Description == entity.Severity)
                                     .FirstOrDefault()?
                                     .Id.ToString();
            }

            base.Map(entity);
        }

        public override NearMiss MapToEntity(NearMiss entity)
        {
            entity = base.MapToEntity(entity);

            switch (SystemType)
            {
                case MapCall.Common.Model.Entities.SystemType.Indices.DRINKING_WATER:
                    entity.WasteWaterSystem = null;
                    break;
                case MapCall.Common.Model.Entities.SystemType.Indices.WASTE_WATER:
                    entity.PublicWaterSupply = null;
                    break;
                default:
                    entity.PublicWaterSupply = null;
                    entity.WasteWaterSystem = null;
                    break;
            }

            if (!string.IsNullOrEmpty(Severity))
            {
                entity.Severity = _container.GetInstance<ISeverityTypeRepository>()
                                            .Find(Convert.ToInt32(Severity))?
                                            .Description;
            }

            if (HaveReviewedNearMiss == true)
            {
                entity.ReviewedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
                entity.ReviewedDate = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            }

            return entity;
        }

        #endregion
    }
}