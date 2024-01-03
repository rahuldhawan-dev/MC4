using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;
using Entities = MapCall.Common.Model.Entities;

namespace MapCallIntranet.Models.ViewModels
{
    public class CreateNearMiss : ViewModel<NearMiss>
    {
        #region Constants

        public const string IOC_CONTACT_NUMBER = RedTagPermitNotification.IOC_CONTACT_NUMBER;

        #endregion

        #region Private Members

        private NearMiss _displayNearMiss;

        #endregion

        #region Properties

        [DropDown(
            "",
            "OperatingCenter",
            "ByStateId",
            DependsOn = nameof(State),
            PromptText = "Select a State above")]
        [Required, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [Required, DropDown, EntityMap(MapDirections.None), EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [DropDown(
            "",
            "Town",
            "ByOperatingCenterId",
            DependsOn = nameof(OperatingCenter),
            PromptText = "Select an Operating Center above")]
        [EntityMap, EntityMustExist(typeof(Town))]
        public int? Town { get; set; }

        [DropDown(
            "",
            "Facility",
            "GetFacilityBy",
            DependsOn = nameof(OperatingCenter),
            PromptText = "Select an Operating Center above")]
        [EntityMap, EntityMustExist(typeof(Facility))]
        [RequiredWhen(
            nameof(NotCompanyFacility),
            ComparisonType.EqualTo,
            true,
            FieldOnlyVisibleWhenRequired = true)]
        public int? Facility { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(WorkOrderType))]
        public int? WorkOrderType { get; set; }

        [EntityMap, EntityMustExist(typeof(WorkOrder))]
        public int? WorkOrder { get; set; }

        [EntityMap, EntityMustExist(typeof(ProductionWorkOrder))]
        public int? ProductionWorkOrder { get; set; }

        public int? ShortCycleWorkOrderNumber { get; set; }

        [StringLength(NearMiss.StringLengths.WORK_ORDER_NUMBER)]
        public string WorkOrderNumber { get; set; }

        [StringLength(NearMiss.StringLengths.REPORTED_BY)]
        [RequiredWhen(
            nameof(ReportAnonymously),
            ComparisonType.EqualTo,
            false,
            FieldOnlyVisibleWhenRequired = true)]
        public string ReportedBy { get; set; } = HttpContext.Current?.User?.Identity?.Name;

        [StringLength(NearMiss.StringLengths.LOCATION_DETAILS)]
        public string LocationDetails { get; set; }

        [EntityMustExist(typeof(Coordinate)), EntityMap]
        [Coordinate(AddressField = "LocationDetails", IconSet = IconSets.NearMiss)]
        public int? Coordinate { get; set; }

        [CheckBox]
        public bool? RelatedToContractor { get; set; }

        [RequiredWhen(
            nameof(RelatedToContractor),
            ComparisonType.EqualTo,
            true,
            FieldOnlyVisibleWhenRequired = true)]
        [StringLength(NearMiss.StringLengths.CONTRACTOR_COMPANY)]
        public string ContractorCompany { get; set; }

        [CheckBox]
        [ClientCallback(
            "NearMiss.validateSeverity",
            ErrorMessage = "The 'Was a Stop Work Authority Performed?' is required.")]
        public virtual bool? StopWorkAuthorityPerformed { get; set; }

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

        [Required, EntityMap, EntityMustExist(typeof(NearMissCategory))]
        [DropDown("", "NearMissCategory", "GetByTypeId", DependsOn = nameof(Type))]
        public int? Category { get; set; }

        [RequiredWhen(
            nameof(Category),
            ComparisonType.EqualTo,
            NearMissCategory.Indices.OTHER,
            FieldOnlyVisibleWhenRequired = true)]
        [StringLength(NearMiss.StringLengths.DESCRIBE_OTHER)]
        public string DescribeOther { get; set; }

        [DropDown("", "NearMissSubCategory", "ByCategory", DependsOn = nameof(Category))]
        [EntityMap, EntityMustExist(typeof(NearMissSubCategory))]
        public int? SubCategory { get; set; }

        [Required]
        public bool? NotCompanyFacility { get; set; }

        [CheckBox]
        public bool? ReportAnonymously { get; set; } = false;

        [Required, DateTimePicker]
        public DateTime? OccurredAt { get; set; }

        [EntityMap, EntityMustExist(typeof(ActionTakenType)), DropDown]
        [RequiredWhen(
            nameof(Type),
            ComparisonType.EqualTo,
            NearMissType.Indices.SAFETY,
            FieldOnlyVisibleWhenRequired = true)]
        public int? ActionTakenType { get; set; }

        [StringLength(NearMiss.StringLengths.ACTION_TAKEN, MinimumLength = 5)]
        public string ActionTaken { get; set; }

        [Required, Multiline, AllowHtml, StringLength(int.MaxValue)]
        public string Description { get; set; }

        [View("Include attachment 1 here"), DoesNotAutoMap]
        public AjaxFileUpload FileUpload { get; set; }

        [View("Include attachment 2 here"), DoesNotAutoMap]
        public AjaxFileUpload FileUpload1 { get; set; }

        [View("Include attachment 3 here"), DoesNotAutoMap]
        public AjaxFileUpload FileUpload2 { get; set; }

        public AjaxFileUpload[] FileUploads => new[] {
            FileUpload, FileUpload1, FileUpload2
        };

        [DropDown, EntityMap, EntityMustExist(typeof(SystemType))]
        [ClientCallback("NearMiss.validateSystemType", ErrorMessage = "The System Type field is required.")]
        public virtual int? SystemType { get; set; }

        [DropDown(
            "",
            "PublicWaterSupply",
            "GetSystemNameByOperatingCenter",
            DependsOn = nameof(OperatingCenter),
            PromptText = "Select an Operating Center above")]
        [EntityMap, EntityMustExist(typeof(PublicWaterSupply))]
        public virtual int? PublicWaterSupply { get; set; }

        [DropDown(
            "",
            "WasteWaterSystem",
            "GetSystemNameByOperatingCenter",
            DependsOn = nameof(OperatingCenter),
            PromptText = "Select an Operating Center above")]
        [EntityMap, EntityMustExist(typeof(WasteWaterSystem))]
        public virtual int? WasteWaterSystem { get; set; }

        [RequiredWhen(
            nameof(Type),
            ComparisonType.EqualTo,
            NearMissType.Indices.ENVIRONMENTAL,
            FieldOnlyVisibleWhenRequired = true)]
        public virtual bool? CompletedCorrectiveActions { get; set; }

        [CheckBox] 
        public bool? SubmittedOnBehalfOfAnotherEmployee { get; set; } = false;

        [DropDown(
            "",
            nameof(Employee),
            "ActiveEmployeesByOperatingCenter",
            DependsOn = nameof(OperatingCenter),
            PromptText = "Select an Operating Center above")]
        [EntityMap, EntityMustExist(typeof(Employee))]
        [RequiredWhen(
            nameof(SubmittedOnBehalfOfAnotherEmployee),
            ComparisonType.EqualTo,
            true,
            FieldOnlyVisibleWhenRequired = true)]
        public int? Employee { get; set; }

        #endregion

        [DoesNotAutoMap]
        public NearMiss DisplayNearMiss =>
            _displayNearMiss ??
            (_displayNearMiss = _container.GetInstance<IRepository<NearMiss>>().Find(Id));

        #region Constructors

        public CreateNearMiss(IContainer container) : base(container) { }

        #endregion

        #region Map Methods

        public override void Map(NearMiss entity)
        {
            if (entity.OperatingCenter != null)
            {
                State = entity.OperatingCenter.State.Id;
            }

            base.Map(entity);
        }

        public override NearMiss MapToEntity(NearMiss entity)
        {
            if (ReportAnonymously == true)
            {
                ReportedBy = null;
            }

            switch (SystemType)
            {
                case Entities.SystemType.Indices.DRINKING_WATER:
                    entity.WasteWaterSystem = null;
                    break;
                case Entities.SystemType.Indices.WASTE_WATER:
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

            return base.MapToEntity(entity);
        }

        #endregion
    }
}