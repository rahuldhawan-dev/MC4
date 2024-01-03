using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using AssetTypeIndices = MapCall.Common.Model.Entities.AssetType.Indices;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class EditWorkOrder : WorkOrderViewModel
    {
        #region Private Members

        private WorkOrder _displayWorkOrder;

        #endregion

        #region Properties

        [DoesNotAutoMap("Display only")]
        public WorkOrder DisplayWorkOrder
        {
            get
            {
                if (_displayWorkOrder == null)
                {
                    _displayWorkOrder = _container.GetInstance<IWorkOrderRepository>().Find(Id);
                }
                return _displayWorkOrder;
            }
        }

        //TODO: This garbage is from 271 WorkOrder model and needs to be maintained in two places.
        [DoesNotAutoMap]
        public bool WorkDescriptionEditable =>
            (!IsSAPUpdatableWorkOrder || !Approved) &&
            !IsNewServiceInstallation;

        // TODO: This can be pulled directly from the entity. There's no reason to duplicate
        // the logic in the view model since this is an edit model.
        [DoesNotAutoMap]
        public bool IsSAPUpdatableWorkOrder =>
            OperatingCenter != null &&
            ShouldSendToSAP(DisplayWorkOrder.OperatingCenter);

        [DoesNotAutoMap]
        public bool Approved => DisplayWorkOrder.ApprovedOn != null;

        // TODO: This should also be able to be pulled from the entity.
        [DoesNotAutoMap]
        public bool IsNewServiceInstallation =>
            IsSAPUpdatableWorkOrder &&
            DisplayWorkOrder.AssetType.Id == MapCall.Common.Model.Entities.AssetType.Indices.SERVICE &&
            MapCall.Common.Model.Entities.WorkDescription.NEW_SERVICE_INSTALLATION
                   .Contains(DisplayWorkOrder.WorkDescription.Id) &&
            PremiseNumber != PREMISE_NUMBER_PLACEHOLDER;

        public DateTime? MaterialsApprovedOn { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(User))]
        public int? MaterialsApprovedBy { get; set; }

        [AutoMap(MapDirections.None)]
        public bool? IsRevisit { get; set; }
        // WorkDescription
        [DropDown(
            "FieldOperations",
            nameof(WorkDescription),
            "ActiveByAssetTypeIdAndIsRevisit",
            DependsOn = nameof(AssetType) + "," + nameof(IsRevisit),
            PromptText = "Please select an asset type above.")]
        [Required, EntityMap, EntityMustExist(typeof(WorkDescription))]
        public override int? WorkDescription { get; set; }
        
        [AutoComplete(
            "",
            nameof(Street),
            "GetByTownIdAndPartialStreetName",
            DependsOn = nameof(Town),
            PlaceHolder = "Please select a Town and enter more than 2 characters",
            DisplayProperty = nameof(MapCall.Common.Model.Entities.Street.FullStName))]
        [Required, EntityMap, EntityMustExist(typeof(Street))]
        public int? Street { get; set; }

        [AutoComplete(
            "",
            nameof(Street),
            "GetActiveByTownIdAndPartialStreetName",
            DependsOn = nameof(Town),
            PlaceHolder = "Please select a Town and enter more than 2 characters",
            DisplayProperty = nameof(MapCall.Common.Model.Entities.Street.FullStName))]
        [Required, EntityMap, EntityMustExist(typeof(Street))]
        public int? NearestCrossStreet { get; set; }

        public virtual bool? HasSampleSite { get; set; }

        //[RequiredWhen("GetOperatingCenterIsSAPWorkOrdersEnabled", ComparisonType.EqualTo, true)]
        [ClientCallback("WorkOrders.validateServiceForSap", ErrorMessage = "The DeviceLocation field is required.")]
        public long? DeviceLocation { get; set; }

        [ClientCallback("WorkOrders.validateServiceForSap", ErrorMessage = "The Installation field is required.")]
        public long? Installation { get; set; }

        #endregion

        #region Constructors

        public EditWorkOrder(IContainer container) : base(container) {}

        #endregion

        #region Private Methods

        private void MaybeMapRecentServiceSizesAndMaterials(WorkOrder entity)
        {
            var recentServices = _container
                                .GetInstance<IRepository<MostRecentlyInstalledService>>()
                                .ByInstallationNumberAndOperatingCenter(
                                     Installation?.ToString(),
                                     OperatingCenter.Value);

            if (recentServices.Count() != 1)
            {
                return;
            }

            var recentService = recentServices.Single();
            if (recentService?.ServiceMaterial?.Description.ToUpper() != ServiceMaterial.Descriptions.UNKNOWN)
            {
                entity.CompanyServiceLineMaterial =
                    entity.CompanyServiceLineMaterial ?? recentService.ServiceMaterial;
            }
            entity.CompanyServiceLineSize = entity.CompanyServiceLineSize ?? recentService.ServiceSize;
            if (recentService?.CustomerSideMaterial?.Description.ToUpper() != ServiceMaterial.Descriptions.UNKNOWN)
            {
                entity.CustomerServiceLineMaterial =
                    entity.CustomerServiceLineMaterial ?? recentService.CustomerSideMaterial;
            }
            entity.CustomerServiceLineSize =
                entity.CustomerServiceLineSize ?? recentService.CustomerSideSize;
        }

        private IEnumerable<ValidationResult> ValidateServiceForSap()
        {
            if (AssetType.GetValueOrDefault() == AssetTypeIndices.SERVICE ||
                AssetType.GetValueOrDefault() == AssetTypeIndices.SEWER_LATERAL)
            {
                var opc = _container.GetInstance<IRepository<OperatingCenter>>()
                                    .Find(OperatingCenter.GetValueOrDefault());
                if (opc != null && !opc.IsContractedOperations && PremiseNumber != PREMISE_NUMBER_PLACEHOLDER)
                {
                    if (!DeviceLocation.HasValue)
                    {
                        yield return new ValidationResult("The Device Location field is required.", new[] { nameof(DeviceLocation) });
                    }

                    if (!Installation.HasValue)
                    {
                        yield return new ValidationResult("The Installation field is required.", new[] { nameof(Installation) });
                    }
                }
            }
        }

        #endregion

        #region Exposed Methods

        public override void Map(WorkOrder entity)
        {
            base.Map(entity);
            // If we're working with a workorder not linked to an asset we need to get a coordinateId
            // TODO: make this work properly with coordinateIds after gutting all of 271
            if (!entity.AssetType.IncludesCoordinate &&
                entity.Coordinate.Id == 0 &&
                entity.Latitude != null &&
                entity.Longitude != null)
            {
                var coordinate = _container.GetInstance<IRepository<Coordinate>>()
                                           .Where(x => x.Latitude == entity.Latitude &&
                                                       x.Longitude == entity.Longitude)
                                           .FirstOrDefault();
                if (coordinate != null)
                {
                    CoordinateId = coordinate.Id;
                }
            }

            IsRevisit = entity.WorkDescription?.Revisit;
        }

        public override WorkOrder MapToEntity(WorkOrder entity)
        {
            if (AssetType == AssetTypeIndices.SERVICE &&
                MapCall.Common.Model.Entities.WorkDescription.SERVICE_LINE_RENEWALS
                       .Contains(WorkDescription.Value) &&
                !MapCall.Common.Model.Entities.WorkDescription.SERVICE_LINE_RENEWALS
                        .Contains(entity.WorkDescription.Id))
            {
                MaybeMapRecentServiceSizesAndMaterials(entity);
            }

            base.MapToEntity(entity);

            entity.MeterLocation = _container.GetInstance<IRepository<Premise>>()
                                             .FindActivePremiseByPremiseNumberDeviceLocationAndInstallation(PremiseNumber,
                                                  DeviceLocation?.ToString(), Installation?.ToString())
                                             .FirstOrDefault()?.MeterLocation;

            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext)
                       .Concat(ValidateServiceForSap());
        }

        #endregion
    }
}