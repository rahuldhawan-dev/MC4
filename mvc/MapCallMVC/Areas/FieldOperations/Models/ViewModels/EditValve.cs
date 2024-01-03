using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using MapCall.Common.ClassExtensions.WorkOrderAssetViewModelExtensions;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Metadata;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class EditValve : ValveViewModel
    {
        #region Constants

        public const string ERROR_VALVE_NUMBER_ALREADY_USED = "A valve already exists for this operating center with the given valve number.",
            ERROR_MORE_THAN_ONE_VALVE = "More than one active valve exists for this operating center with the given valve number.";

        private const string ERROR_WBS_NUMBER_REQUIRED =
            "Work Order Number is required for retired, removed and active valves.";
        #endregion

        #region Private Members

        // There are a lot of properties that aren't editable by some users so this easier
        // than making a hundred display properties.
        private Valve _displayValve;
        private AssetStatusNumberDuplicationValidator _numberValidator;

        #endregion

        #region Properties

        // This is needed for proper cascading of the MainCrossings property. This isn't
        // editable by the user.
        [EntityMap(MapDirections.ToPrimary)]
        public override int? OperatingCenter { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(Street))]
        [View(DisplayName=Valve.Display.STREET)]
        [DropDown("", "Street", "ByTownId", DependsOn = "Town", PromptText = "Please select a town above")]
        public virtual int? Street { get; set; }

        [EntityMap, EntityMustExist(typeof(Street))]
        [DropDown("", "Street", "ByTownId", DependsOn = "Town", PromptText = "Please select a town above")]
        public virtual int? CrossStreet { get; set; }

        [MultiSelect, EntityMap(MapDirections.None /* Mapped manually */), EntityMustExist(typeof(OperatingCenter))]
        public int[] MainCrossingOperatingCenter { get; set; }

        [RequiredWhen("ControlsCrossing", true)]
        [MultiSelect("Facilities", "MainCrossing", "ByOperatingCenterIds", DependsOn = "MainCrossingOperatingCenter", PromptText = "Please select an one operating center above.")]
        [EntityMap, EntityMustExist(typeof(MainCrossing))]
        public int[] MainCrossings { get; set; }

        [EntityMap(MapDirections.ToPrimary), Secured]
        public int? Town { get; set; }

        [Required, RoleSecured(ValveController.ROLE, RoleActions.UserAdministrator, AppliesToAdmins = false)]
        [StringLength(Valve.StringLengths.VALVE_NUMBER)]
        public virtual string ValveNumber { get; set; }

        [RoleSecured(ValveController.ROLE, RoleActions.UserAdministrator, AppliesToAdmins = false)]
        public virtual int ValveSuffix { get; set; }

        [RoleSecured(ValveController.ROLE, RoleActions.UserAdministrator, AppliesToAdmins = false)]
        [StringLength(Valve.StringLengths.LEGACY_ID)]
        public string LegacyId { get; set; }

        [EntityMap, EntityMustExist(typeof(ValveBilling))]
        [DisplayName(Valve.Display.VALVE_BILLING)]
        [DropDown, Required, RoleSecured(ValveController.ROLE, RoleActions.UserAdministrator, AppliesToAdmins = false)]
        public override int? ValveBilling { get; set; }

        [DisplayName(Valve.Display.BPUKPI), RoleSecured(ValveController.ROLE, RoleActions.UserAdministrator, AppliesToAdmins = false)]
        public override bool BPUKPI { get; set; }

        //  [EntityMap, EntityMustExist(typeof(FunctionalLocation))]
        [DropDown("FieldOperations", "FunctionalLocation", "ActiveByTownIdForValveAssetType", DependsOn = "Town,ValveControls", PromptText = "Please select a town and valve control above")]
        [RoleSecured(ValveController.ROLE, RoleActions.UserAdministrator, AppliesToAdmins = false)]
        [ClientCallback("Valve.validateFunctionalLocation", ErrorMessage = "The Functional Location field is required.")]
        public override int? FunctionalLocation { get; set; }

        //Does not stink as much as Ross thinks -arr
        // I disagree -Ross
        [DoesNotAutoMap]
        public bool SendNotificationsOnSave { get; set; }

        // Bug 2589: Only user admins can edit coordinate.
        [RoleSecured(ValveController.ROLE, RoleActions.UserAdministrator, AppliesToAdmins = false)]
        public override int? Coordinate
        {
            get
            {
                return base.Coordinate;
            }
            set
            {
                base.Coordinate = value;
            }
        }

        [DoesNotAutoMap("Display only")]
        public Valve DisplayValve
        {
            get
            {
                if (_displayValve == null)
                {
                    _displayValve = _container.GetInstance<IValveRepository>().Find(Id);
                }
                return _displayValve;
            }
        }

        public int? SAPEquipmentId { get; set; }

        [DoesNotAutoMap("Display only. Needed for view logic.")]
        public bool CanEditInspectionFrequency
        {
            get
            {
                if (!OperatingCenter.HasValue)
                {
                    return false;
                }
                var opc = _container.GetInstance<IOperatingCenterRepository>().Find(OperatingCenter.Value);
                return opc.UsesValveInspectionFrequency;
            }
        }

        [StringLength(Valve.StringLengths.WORK_ORDER_NUMBER)]
        [View(Valve.Display.WORK_ORDER_NUMBER)]
        [ClientCallback("Valve.validateWorkOrderNumber", ErrorMessage = ERROR_WBS_NUMBER_REQUIRED)]
        public virtual string WorkOrderNumber { get; set; }

        #endregion

        #region Constructors

        public EditValve(IContainer container, AssetStatusNumberDuplicationValidator numberValidator) : base(container)
        {
            _numberValidator = numberValidator;
        }

        #endregion

        #region Private Methods

        private void SetSendNotificationsOnSave(Valve entity)
        {
            SendNotificationsOnSave = false;

            var vmValStatus = Status.GetValueOrDefault();
            var valveStatusRepository = _container.GetInstance<AssetStatusRepository>();
            var activeStatus = valveStatusRepository.GetActiveStatus();
            var retiredStatus = valveStatusRepository.GetRetiredStatus();

            if ((entity.Status == activeStatus && vmValStatus != activeStatus.Id) ||
                (entity.Status != activeStatus && vmValStatus == activeStatus.Id))
            {
                SendNotificationsOnSave = true;
            }
            else if ((entity.Status == retiredStatus && vmValStatus != retiredStatus.Id) ||
                     (entity.Status != retiredStatus && vmValStatus == retiredStatus.Id))
            {
                SendNotificationsOnSave = true;
            }
            else
            {
                // Bug 2432: A notification needs to be sent out when any non-adminonly status gets set.
                var notificationStatuses = valveStatusRepository.GetAll().Where(x => !x.IsUserAdminOnly).Select(x => x.Id);
                var entityValStatus = entity.Status != null ? entity.Status.Id : 0;
                if (notificationStatuses.Contains(vmValStatus) && vmValStatus != entityValStatus)
                {
                    SendNotificationsOnSave = true;
                }
            }

        }

        private IEnumerable<ValidationResult> ValidateFunctionalLocation()
        {
            if (!FunctionalLocation.HasValue && !DisplayValve.OperatingCenter.IsContractedOperations && DisplayValve.OperatingCenter.SAPEnabled)
            {
                yield return new ValidationResult("The Functional Location field is required.", new[] { "FunctionalLocation" });
            }
        }

        private IEnumerable<ValidationResult> ValidateValveSuffixAndNumberMatch()
        {
            var regex = new Regex(Valve.VALVE_NUMBER_PATTERN, RegexOptions.IgnoreCase);
            var match = regex.Match(ValveNumber);

            if (match.Groups.Count != 4 || match.Groups[3].ToString() != ValveSuffix.ToString())
                yield return new ValidationResult(Valve.VALVE_NUMBER_PATTERN_ERROR, new[] { "ValveNumber" });
        }

        private IEnumerable<ValidationResult> ValidateValveNumber()
        {
            var repo = _container.GetInstance<IRepository<Valve>>();
            var entity = repo.Find(Id);
            if (entity == null)
            {
                yield break;
            }

            var matches = repo.FindByOperatingCenterAndValveNumber(entity.OperatingCenter, ValveNumber).Where(v => v.Id != Id);
            if (!matches.Any())
            {
                // This implies that the valve number has been changed to an unused number. And that's OK.
                yield break;
            }


            if ((!Status.HasValue && matches.Any()) ||
                (Status.HasValue && !_numberValidator.IsValid(Status.Value, matches)))
            {
                yield return new ValidationResult(ERROR_VALVE_NUMBER_ALREADY_USED, new[] { "ValveNumber" });
            }
        }

        private IEnumerable<ValidationResult> ValidateWorkOrderNumber()
        {
            var valve = _container.GetInstance<IValveRepository>().Find(Id);

            if (Status.HasValue && AssetStatus.WBS_STATUS_IDS.Contains(Status.Value) && WorkOrderNumber == null && valve.Status.Id != Status.Value)
            {
                yield return new ValidationResult(ERROR_WBS_NUMBER_REQUIRED, new[] { "WorkOrderNumber" });
            }
        }

        #endregion

        #region Exposed Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override Valve MapToEntity(Valve entity)
        {
            SetSendNotificationsOnSave(entity);

            this.MaybeCancelWorkOrders(entity, _container.GetInstance<IRepository<WorkOrderCancellationReason>>(),
                t => t.DateRetired);

            SetInspectionFrequency();
            return base.MapToEntity(entity);
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext)
                .Concat(ValidateValveSuffixAndNumberMatch())
                .Concat(ValidateFunctionalLocation())
                .Concat(ValidateValveNumber()
                .Concat(ValidateWorkOrderNumber()));
        }

        public override void Map(Valve entity)
        {
            base.Map(entity);
            var mainCrossingOperatingCenters = entity.MainCrossings.Select(x => x.OperatingCenter.Id);
            MainCrossingOperatingCenter = mainCrossingOperatingCenters.ToArray();
        }

        #endregion
    }
}