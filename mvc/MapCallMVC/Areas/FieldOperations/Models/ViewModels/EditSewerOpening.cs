using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.ClassExtensions.WorkOrderAssetViewModelExtensions;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class EditSewerOpening : SewerOpeningViewModel
    {
        #region Constants

        public const string ERROR_OPENING_NUMBER_ALREADY_USED = "A opening already exists for this operating center with the given opening number.",
                            ERROR_OPENING_NUMBERS_ALREADY_EXIST = "More than one opening exists for this operating center with the given opening number",
                            ERROR_WBS_NUMBER_REQUIRED = "Work Order Number is required for retired, removed and active sewer openings.";

        #endregion

        private readonly AssetStatusNumberDuplicationValidator _numberValidator;

        #region Private Members

        // There are a lot of properties that aren't editable by some users so this easier
        // than making a hundred display properties.
        private SewerOpening _displaySewerOpening;

        #endregion

        #region Properties
        
        [DropDown("", "Street", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        [Required, EntityMap, EntityMustExist(typeof(Street))]
        public int? Street { get; set; }

        [DropDown("", "Street", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        [EntityMap, EntityMustExist(typeof(Street))]
        public int? IntersectingStreet { get; set; }

        [StringLength(SewerOpening.StringLengths.OPENING_NUMBER)]
        public string OpeningNumber { get; set; }

        [StringLength(SewerOpening.StringLengths.NPDES_PERMIT_NUMBER)]
        public string NpdesPermitNumber { get; set; }

        [StringLength(SewerOpening.StringLengths.OUTFALL_NUMBER)]
        public string OutfallNumber { get; set; }

        [DropDown("", "BodyOfWater", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center")]
        [EntityMap, EntityMustExist(typeof(BodyOfWater))]
        public int? BodyOfWater { get; set; }

        public int? OpeningSuffix { get; set; }

        [RoleSecured(SewerOpeningController.ROLE, RoleActions.UserAdministrator, AppliesToAdmins = false)]
        [DropDown("FieldOperations", "FunctionalLocation", "ActiveByTownIdForSewerOpeningAssetType", DependsOn = "Town", PromptText = "Select a town above")]
        [EntityMap, EntityMustExist(typeof(FunctionalLocation))]
        public override int? FunctionalLocation
        {
            get
            {
                return base.FunctionalLocation;
            }

            set
            {
                base.FunctionalLocation = value;
            }
        }

        public int? SAPEquipmentId { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(RecurringFrequencyUnit))]
        public override int? InspectionFrequencyUnit
        {
            get => base.InspectionFrequencyUnit; set => base.InspectionFrequencyUnit = value;
        }

        [DoesNotAutoMap("Display only")]
        public SewerOpening DisplaySewerOpening
        {
            get
            {
                if (_displaySewerOpening == null)
                {
                    _displaySewerOpening = _container.GetInstance<ISewerOpeningRepository>().Find(Id);
                }
                return _displaySewerOpening;
            }
        }

        [AutoMap(MapDirections.None)] // Needed for cascades to work
        public int? Town => DisplaySewerOpening?.Town.Id;

        public int? OperatingCenter => DisplaySewerOpening?.OperatingCenter.Id;

        [StringLength(SewerOpening.StringLengths.TASK_NUMBER)]
        [ClientCallback("SewerOpenings.validateTaskNumber", ErrorMessage = ERROR_WBS_NUMBER_REQUIRED)]
        public string TaskNumber { get; set; }

        #endregion

        #region Constructors

        public EditSewerOpening(IContainer container, AssetStatusNumberDuplicationValidator numberValidator) :
            base(container)
        {
            _numberValidator = numberValidator;
        }

        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidateOpeningNumber()
        {
            var repo = _container.GetInstance<ISewerOpeningRepository>();
            var entity = repo.Find(Id);
            if (entity == null)
                yield break; // for unit testing purposes

            var existing = repo.FindByOperatingCenterAndOpeningNumber(entity.OperatingCenter, OpeningNumber)
                               .Where(sm => sm.Id != Id);

            if ((!Status.HasValue && existing.Any()) ||
                (Status.HasValue && !_numberValidator.IsValid(Status.Value, existing)))
            {
                yield return new ValidationResult(
                    string.Format(
                        "The generated opening number '{0}' is not unique to the operating center '{1}'",
                        OpeningNumber, entity.OperatingCenter), new[] { nameof(OpeningNumber) });
            }
        }

        private IEnumerable<ValidationResult> ValidateFunctionalLocation()
        {
            if (FunctionalLocation == null 
                && !DisplaySewerOpening.OperatingCenter.IsContractedOperations 
                && DisplaySewerOpening.OperatingCenter.SAPEnabled)
            {
                yield return new ValidationResult("The Functional Location field is required.", new[] { nameof(FunctionalLocation) });
            }
        }

        private void SetSendNotificationOnSave(SewerOpening entity)
        {
            SendNotificationOnSave = false;

            var vmSewerOpeningStatus = Status.GetValueOrDefault();
            var SewerOpeningStatusRepository = _container.GetInstance<AssetStatusRepository>();
            var activeStatus = SewerOpeningStatusRepository.GetActiveStatus();
            var retiredStatus = SewerOpeningStatusRepository.GetRetiredStatus();

            if ((entity.Status == activeStatus && vmSewerOpeningStatus != activeStatus.Id) ||
                (entity.Status != activeStatus && vmSewerOpeningStatus == activeStatus.Id))
            {
                SendNotificationOnSave = true;
            }
            else if ((entity.Status == retiredStatus && vmSewerOpeningStatus != retiredStatus.Id) ||
                     (entity.Status != retiredStatus && vmSewerOpeningStatus == retiredStatus.Id))
            {
                SendNotificationOnSave = true;
            }
            else
            {
                // Bug 2432: A notification needs to be sent out when any non-adminonly status gets set.
                var notificationStatuses = SewerOpeningStatusRepository.GetAll().Where(x => !x.IsUserAdminOnly).Select(x => x.Id);
                var entityValStatus = entity.Status != null ? entity.Status.Id : 0;
                if (notificationStatuses.Contains(vmSewerOpeningStatus) && vmSewerOpeningStatus != entityValStatus)
                {
                    SendNotificationOnSave = true;
                }
            }
        }

        private IEnumerable<ValidationResult> ValidateTaskNumber()
        {
            var sewerOpening = _container.GetInstance<ISewerOpeningRepository>().Find(Id);

            if (Status.HasValue && AssetStatus.WBS_STATUS_IDS.Contains(Status.Value) && TaskNumber == null && sewerOpening.Status.Id != Status.Value)
            {
                yield return new ValidationResult(ERROR_WBS_NUMBER_REQUIRED, new[] { "TaskNumber" });
            }
        }

        #endregion

        #region Exposed Methods

        public override SewerOpening MapToEntity(SewerOpening entity)
        {
            SetSendNotificationOnSave(entity);

            this.MaybeCancelWorkOrders(entity, _container.GetInstance<IRepository<WorkOrderCancellationReason>>(),
                t => t.DateRetired);

            return base.MapToEntity(entity);
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext)
                       .Concat(ValidateOpeningNumber())
                       .Concat(ValidateFunctionalLocation()
                       .Concat(ValidateTaskNumber()));
        }

        #endregion
    }
}
