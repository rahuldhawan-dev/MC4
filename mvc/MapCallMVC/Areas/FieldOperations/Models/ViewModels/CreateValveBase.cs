using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using DataAnnotationsExtensions;
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
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{ 
    public class CreateValveBase : ValveViewModel
    {
        #region Fields

        private readonly AssetStatusNumberDuplicationValidator _numberValidator;

        #endregion

        #region Properties

        [DropDown, SearchAlias("Town", "State.Id")]
        public int? State { get; set; }

        [Required, DropDown("", "OperatingCenter", "ActiveByStateIdOrAll", DependsOn = "State", PromptText = "Select a state above"), EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public override int? OperatingCenter { get; set; }

        [RequiredWhen("ControlsCrossing", true)]
        [MultiSelect("Facilities", "MainCrossing", "ByOperatingCenterIds", DependsOn = "OperatingCenter", PromptText = "Please select an one operating center above.")]
        [EntityMap, EntityMustExist(typeof(MainCrossing))]
        public int[] MainCrossings { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(Town))]
        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        public int? Town { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(Street))]
        [View(DisplayName=Valve.Display.STREET)]
        [DropDown("", "Street", "GetActiveByTownId", DependsOn = "Town", PromptText = "Please select a town above")]
        public virtual int? Street { get; set; }

        [EntityMap, EntityMustExist(typeof(Street))]
        [DropDown("", "Street", "GetActiveByTownId", DependsOn = "Town", PromptText = "Please select a town above")]
        public virtual int? CrossStreet { get; set; }

        [DoesNotAutoMap("Not an entity property.")]
        public bool IsFoundValve { get; set; }

        [Remote("ValidateUnusedFoundValveSuffix", "Valve", "FieldOperations", AdditionalFields = "OperatingCenter, Town, TownSection")]
        [RequiredWhen("IsFoundValve", true, ErrorMessage = ErrorMessages.SUFFIX_REQUIRED)]
        [Min(1, ErrorMessage = ErrorMessages.SUFFIX_GREATER_THAN_ZERO)]
        public int? ValveSuffix { get; set; }

        [StringLength(Valve.StringLengths.LEGACY_ID)]
        public string LegacyId { get; set; }

        [EntityMap, EntityMustExist(typeof(User))]
        public virtual int? Initiator { get; set; }

        [EntityMap, EntityMustExist(typeof(TownSection))]
        [DropDown("", "TownSection", "ActiveByTownId", DependsOn = "Town", PromptText = "Please select a town above")]
        public override int? TownSection { get; set; }

        [DropDown("FieldOperations", "FunctionalLocation", "ActiveByTownIdForValveAssetType", DependsOn = "Town,ValveControls", PromptText = "Please select a town above")]
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

        [StringLength(Valve.StringLengths.WORK_ORDER_NUMBER)]
        [View(Valve.Display.WORK_ORDER_NUMBER)]
        [Required]
        public virtual string WorkOrderNumber { get; set; }

        #endregion

        #region Constructors

        public CreateValveBase(IContainer container, AssetStatusNumberDuplicationValidator numberValidator) :
            base(container)
        {
            _numberValidator = numberValidator;
        }

        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidateSAPFunctionalLocation()
        {
            if (!FunctionalLocation.HasValue)
            {
                var opc = _container.GetInstance<IRepository<OperatingCenter>>().Find(OperatingCenter.Value);
                if (!opc.IsContractedOperations && opc.SAPEnabled)
                {
                    yield return new ValidationResult("The Functional Location field is required.", new[] { "FunctionalLocation" });
                }
            }
        }

        protected virtual bool ValveNumberIsUniqueToOperatingCenter(IRepository<Valve> valveRepository, OperatingCenter operatingCenter, ValveNumber number)
        {
            return !valveRepository.FindByOperatingCenterAndValveNumber(operatingCenter, number.FormattedNumber).Any();
        }

        #endregion

        #region Exposed Methods

        public override Valve MapToEntity(Valve entity)
        {
            base.MapToEntity(entity);
            entity.Initiator = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;

            if (Town.HasValue)
            {
                var valRepo = _container.GetInstance<IRepository<Valve>>();
                ValveNumber valNum;

                if (IsFoundValve)
                {
                    valNum = new ValveNumber {
                        Prefix = valRepo.GenerateValvePrefix(entity.OperatingCenter, entity.Town, entity.TownSection),
                        Suffix = ValveSuffix.Value
                    };
                }
                else
                {
                    valNum = valRepo.GenerateNextValveNumber(_container.GetInstance<RepositoryBase<Valve>>(),
                        entity.OperatingCenter, entity.Town, entity.TownSection);
                }

                if (!ValveNumberIsUniqueToOperatingCenter(valRepo, entity.OperatingCenter, valNum))
                {
                    throw ExceptionHelper.Format<InvalidOperationException>("The generated valve number '{0}' is not unique to the operating center '{1}'", valNum.FormattedNumber, entity.OperatingCenter);
                }

                entity.ValveNumber = valNum.FormattedNumber;
                entity.ValveSuffix = valNum.Suffix;
            }

            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext)
                .Concat(ValidateValveSuffixForFoundValve())
                .Concat(ValidateSAPFunctionalLocation());
        }

        public virtual IEnumerable<ValidationResult> ValidateValveSuffixForFoundValve()
        {
            if (!IsFoundValve) { yield break; }

            var repo = _container.GetInstance<IRepository<Valve>>();
            var rawRepo = _container.GetInstance<RepositoryBase<Valve>>();
            var opc = _container.GetInstance<IRepository<OperatingCenter>>().Find(OperatingCenter.GetValueOrDefault());
            var town = _container.GetInstance<IRepository<Town>>().Find(Town.GetValueOrDefault());
            var townSection = _container.GetInstance<IRepository<TownSection>>().Find(TownSection.GetValueOrDefault());

            if (town == null || opc == null) { yield break; }

            var maxVal = repo.GetMaxValveNumber(rawRepo, opc, town, townSection);
            if (ValveSuffix > maxVal.Suffix)
            {
                yield return new ValidationResult(ErrorMessages.SUFFIX_TOO_LARGE, new[] { "ValveSuffix" });
            }
            else
            {
                maxVal.Suffix = ValveSuffix.GetValueOrDefault();
                var existing = repo.FindByOperatingCenterAndValveNumber(opc, maxVal.FormattedNumber);
                if ((!Status.HasValue && existing.Any()) ||
                    (Status.HasValue && !_numberValidator.IsValid(Status.Value, existing)))
                {
                    yield return new ValidationResult(ErrorMessages.SUFFIX_ALREADY_EXISTS, new[] { "ValveSuffix" });
                }
            }
        }

        public bool RequiresNotification()
        {
            var valveStatusRepository = _container.GetInstance<AssetStatusRepository>();
            var activeStatus = valveStatusRepository.GetActiveStatus();
            var retiredStatus = valveStatusRepository.GetRetiredStatus();

            return (Status.HasValue && (Status.Value == activeStatus.Id || Status.Value == retiredStatus.Id));
        }

        #endregion
    }
}