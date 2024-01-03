using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class CreateSewerOpening : CreateSewerOpeningBase
    {
        private readonly AssetStatusNumberDuplicationValidator _numberValidator;

        #region Properties

        [DoesNotAutoMap("Users can not set this value during creation. It's set in MapToEntity instead.")]
        public override int? InspectionFrequency { get => base.InspectionFrequency; set => base.InspectionFrequency = value; }

        [DoesNotAutoMap("Users can not set this value during creation. It's set in MapToEntity instead.")]
        public override int? InspectionFrequencyUnit { get => base.InspectionFrequencyUnit; set => base.InspectionFrequencyUnit = value; }

        #endregion

        public override SewerOpening MapToEntity(SewerOpening entity)
        {
            entity = base.MapToEntity(entity);
            var openingRepo = _container.GetInstance<IRepository<SewerOpening>>();
            var abbrRepo = _container.GetInstance<IAbbreviationTypeRepository>();
            var rawRepo =
                _container.GetInstance<RepositoryBase<SewerOpening>>();

            if (Town.HasValue)
            {
                var openingNumber = openingRepo.GenerateNextOpeningNumber(abbrRepo, rawRepo, entity.OperatingCenter, entity.Town, entity.TownSection);
                entity.OpeningNumber = openingNumber.FormattedNumber;
                entity.OpeningSuffix = openingNumber.Suffix;

                var existing = openingRepo
                   .FindByOperatingCenterAndOpeningNumber(entity.OperatingCenter, openingNumber.FormattedNumber);

                if ((!Status.HasValue && existing.Any()) ||
                    (Status.HasValue && !_numberValidator.IsValid(Status.Value, existing)))
                {
                    throw ExceptionHelper.Format<InvalidOperationException>(
                        "The generated opening number '{0}' is not unique to the operating center '{1}'",
                        openingNumber.FormattedNumber, entity.OperatingCenter);
                }
            }
            // MC-1447 requests this work the same as hydrants.
            // Users can not set these values during creation. They must come from
            // the operating center.
            // This must be done after the base.MapToEntity is called so we can use the entity's OperatingCenter.
            SetInspectionFrequencyFromOperatingCenter(entity);

            return entity;
        }

        public CreateSewerOpening(IContainer container, AssetStatusNumberDuplicationValidator numberValidator) :
            base(container)
        {
            _numberValidator = numberValidator;
        }

        public bool RequiresNotification()
        {
            var SewerOpeningStatusRepository = _container.GetInstance<AssetStatusRepository>();
            var activeStatus = SewerOpeningStatusRepository.GetActiveStatus();
            var retiredStatus = SewerOpeningStatusRepository.GetRetiredStatus();

            return (Status.HasValue && (Status.Value == activeStatus.Id || Status.Value == retiredStatus.Id));
        }

        #region Private Methods

        protected virtual void SetInspectionFrequencyFromOperatingCenter(SewerOpening entity)
        {
            // This is not editable during creation, it's supposed to be autopopulated by op center.
            // Defaults to 1 Year if the OperatingCenter does not have anything set.
            entity.InspectionFrequency = entity.OperatingCenter.SewerOpeningInspectionFrequency;
            entity.InspectionFrequencyUnit = entity.OperatingCenter.SewerOpeningInspectionFrequencyUnit;
        }

        #endregion
    }
}
