using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using IContainer = StructureMap.IContainer;
using AssetTypeIndices = MapCall.Common.Model.Entities.AssetType.Indices;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class CreateWorkOrder : WorkOrderViewModel
    {
        #region Constants

        public const string ECHOSHORE_NOTE = "Created from Echoshore Leak Alert {0} for Hydrants: {1}, {2}. At site: {3}. Notes: {4}. Distance from Hydrant 1: {5}. Distance from Hydrant 2: {6}",
                            SMART_COVER_ALERT_NOTE = "Created from Smart Cover Alert {0} for SewerOpening: {1}. Notes: Application Description - {2}, Alarm Types: {3}, Alerts: {4}",
                            PLANNED_COMPLETION_DATE = "Planned Completion Date (back office use only)",
                            PLANNED_COMPLETION_DATE_ERROR_MESSAGE = "The field Planned Completion Date must be greater than or equal to today for an Emergency priority. For all other priorities it must be between 2 days from now and 12/31/9999 11:59:59 PM.";

        #endregion

        #region Properties

        [DropDown(
            "",
            nameof(TownSection),
            "ActiveByTownId",
            DependsOn = nameof(Town),
            PromptText = "Select a town from above")]
        [EntityMap, EntityMustExist(typeof(TownSection))]
        public int? TownSection { get; set; }
        
        [AutoComplete(
            "",
            nameof(Street),
            "GetActiveByTownIdAndPartialStreetName",
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

        [StringLength(WorkOrder.StringLengths.ZIP_CODE)]
        public string ZipCode { get; set; }

        // Sewer Main
        // Main

        //Purpose
        [Required, DropDown, EntityMap, EntityMustExist(typeof(WorkOrderPurpose))]
        public int? Purpose { get; set; }

        //Priority
        [Required, DropDown, EntityMap, EntityMustExist(typeof(WorkOrderPriority))]
        public int? Priority { get; set; }

        // Customer - CustomerName
        [StringLength(StringLengths.CUSTOMER_NAME)]
        [RequiredWhen(nameof(RequestedBy), ComparisonType.EqualTo, WorkOrderRequester.Indices.CUSTOMER)]
        public string CustomerName { get; set; }

        // Customer - Phone Number
        [StringLength(StringLengths.PHONE_NUMBER)]
        [RequiredWhen(nameof(RequestedBy), ComparisonType.EqualTo, WorkOrderRequester.Indices.CUSTOMER)]
        public string PhoneNumber { get; set; }

        // Customer - 2nd Phone Number
        [StringLength(StringLengths.SECONDARY_PHONE_NUMBER)]
        public string SecondaryPhoneNumber { get; set; }

        // Employee - Employee Name
        [RequiredWhen(nameof(RequestedBy), ComparisonType.EqualTo, WorkOrderRequester.Indices.EMPLOYEE)]
        [AutoComplete(
            "",
            nameof(User),
            "GetByOperatingCenterIdAndPartialNameMatchForTDWorkOrders",
            DisplayProperty = nameof(User.FullName),
            DependsOn = nameof(OperatingCenter))]
        [EntityMap, EntityMustExist(typeof(User))]
        public int? RequestingEmployee { get; set; }

        // MainBreak -- EstimatedCustomerImpact
        [RequiredWhen(
            nameof(WorkDescription),
            ComparisonType.EqualToAny,
            "GetMainBreakWorkDescriptions",
            typeof(WorkDescription))]
        [DropDown, EntityMap, EntityMustExist(typeof(CustomerImpactRange))]
        public int? EstimatedCustomerImpact { get; set; }

        // MainBreak -- AnticipatedRepairTime
        [RequiredWhen(
            nameof(WorkDescription),
            ComparisonType.EqualToAny,
            "GetMainBreakWorkDescriptions",
            typeof(WorkDescription))]
        [DropDown, EntityMap, EntityMustExist(typeof(RepairTimeRange))]
        public int? AnticipatedRepairTime { get; set; }

        // MainBreak -- AlertIssued
        [RequiredWhen(
            nameof(WorkDescription),
            ComparisonType.EqualToAny,
            "GetMainBreakWorkDescriptions",
            typeof(WorkDescription))]
        public bool? AlertIssued { get; set; }

        [AutoMap(MapDirections.None)]
        [View(DisplayName = "Initial or Revisit?")]
        [Required, BoolFormat("Revisit", "Initial")]
        public bool? IsRevisit{ get; set; }

        // WorkDescription
        [DropDown(
            "FieldOperations",
            nameof(WorkDescription),
            "ActiveByAssetTypeIdForCreate",
            DependsOn = nameof(AssetType) + "," + nameof(IsRevisit),
            PromptText = "Please select an asset type above.")]
        [Required, EntityMap, EntityMustExist(typeof(WorkDescription))]
        public override int? WorkDescription { get; set; }

        // MainBreak -- SignificantTrafficImpact
        [RequiredWhen(
            nameof(WorkDescription),
            ComparisonType.EqualToAny,
            "GetMainBreakWorkDescriptions",
            typeof(WorkDescription))]
        public bool? SignificantTrafficImpact { get; set; }

        // Markout Requirement
        [DropDown, Required, EntityMap, EntityMustExist(typeof(MarkoutRequirement))]
        public int? MarkoutRequirement { get; set; }

        [EntityMap, EntityMustExist(typeof(EchoshoreLeakAlert))]
        public int? EchoshoreLeakAlert { get; set; }

        [EntityMustExist(typeof(SmartCoverAlert))]
        public int? SmartCoverAlert { get; set; }

        // Traffic Control Required
        public bool TrafficControlRequired { get; set; }

        // Street Opening Permit Required
        public bool StreetOpeningPermitRequired { get; set; }
        
        // Notes
        [Multiline, StringLength(int.MaxValue)]
        public string Notes { get; set; }

        [Multiline, StringLength(int.MaxValue)]
        public string SpecialInstructions { get; set; }

        // Date Received
        [View(FormatStyle.Date)]
        public DateTime? DateReceived { get; set; }

        // Planned Completion Date
        [View(PLANNED_COMPLETION_DATE, FormatStyle.Date)]
        [ClientCallback("WorkOrders.validatePlannedCompletionDate", ErrorMessage = PLANNED_COMPLETION_DATE_ERROR_MESSAGE)]
        public DateTime? PlannedCompletionDate { get; set; }
        
        // SAP Notification #
        public long? SAPNotificationNumber { get; set; }

        // SAP Work Order #
        public long? SAPWorkOrderNumber { get; set; }

        // REVISIT
        [RequiredWhen(nameof(IsRevisit), ComparisonType.EqualTo, true)]
        [EntityMap, EntityMustExist(typeof(WorkOrder))]
        public int? OriginalOrderNumber { get; set; }
 
        public virtual bool? HasSampleSite { get; set; }

        //[RequiredWhen("GetOperatingCenterIsSAPWorkOrdersEnabled", ComparisonType.EqualTo, true)]
        [ClientCallback("WorkOrders.validateServiceForSap", ErrorMessage = "The DeviceLocation field is required.")]
        public long? DeviceLocation { get; set; }

        [ClientCallback("WorkOrders.validateServiceForSap", ErrorMessage = "The Installation field is required.")]
        public long? Installation { get; set; }

        #endregion

        #region Constructors

        [DefaultConstructor]
        public CreateWorkOrder(IContainer container) : base(container) {}

        public CreateWorkOrder(IContainer container, MainCrossing mainCrossing) : this(container) 
        {
            OperatingCenter = mainCrossing.OperatingCenter?.Id;
            Town = mainCrossing.Town?.Id;
            AssetType = MapCall.Common.Model.Entities.AssetType.Indices.MAIN_CROSSING;
            Street = mainCrossing.Street?.Id;
            NearestCrossStreet = mainCrossing.ClosestCrossStreet?.Id;
            MainCrossing = mainCrossing.Id;
            CoordinateId = mainCrossing.Coordinate?.Id;
        }

        public CreateWorkOrder(IContainer container, Service service) : this(container)
        {
            OperatingCenter = service.OperatingCenter?.Id;
            Town = service.Town?.Id;
            TownSection = service.TownSection?.Id;
            StreetNumber = service.StreetNumber;
            ApartmentAddtl = service.ApartmentNumber;
            Street = service.Street?.Id;
            NearestCrossStreet = service.CrossStreet?.Id;
            ZipCode = service.Zip;
            AssetType = (service.ServiceCategory != null && service.ServiceCategory.IsSewerCategory)
                ? MapCall.Common.Model.Entities.AssetType.Indices.SEWER_LATERAL
                : MapCall.Common.Model.Entities.AssetType.Indices.SERVICE;
            CoordinateId = service.Coordinate?.Id;
            Priority = DeterminePriority(service);
            Service = service.Id;
            ServiceNumber = service.ServiceNumber.ToString();
            PremiseNumber = service.PremiseNumber;
            SAPNotificationNumber = service.SAPNotificationNumber;
            if (!string.IsNullOrWhiteSpace(service.DeviceLocation) &&
                long.TryParse(service.DeviceLocation, out var location))
            {
                DeviceLocation = location;
            }
        }

        public CreateWorkOrder(IContainer container, Hydrant hydrant) : this(container)
        {
            OperatingCenter = hydrant.OperatingCenter?.Id;
            Town = hydrant.Town?.Id;
            TownSection = hydrant.TownSection?.Id;
            StreetNumber = hydrant.StreetNumber;
            Street = hydrant.Street?.Id;
            NearestCrossStreet = hydrant.CrossStreet?.Id;
            AssetType = MapCall.Common.Model.Entities.AssetType.Indices.HYDRANT;
            CoordinateId = hydrant.Coordinate?.Id;
            Hydrant = hydrant.Id;
        }

        public CreateWorkOrder(IContainer container, Valve valve) : this(container)
        {
            OperatingCenter = valve.OperatingCenter?.Id;
            Town = valve.Town?.Id;
            TownSection = valve.TownSection?.Id;
            StreetNumber = valve.StreetNumber;
            Street = valve.Street?.Id;
            NearestCrossStreet = valve.CrossStreet?.Id;
            AssetType = MapCall.Common.Model.Entities.AssetType.Indices.VALVE;
            CoordinateId = valve.Coordinate?.Id;
            Valve = valve.Id;
        }

        public CreateWorkOrder(IContainer container, SewerOpening sewerOpening) : this(container)
        {
            OperatingCenter = sewerOpening.OperatingCenter?.Id;
            Town = sewerOpening.Town?.Id;
            TownSection = sewerOpening.TownSection?.Id;
            NearestCrossStreet = sewerOpening.IntersectingStreet?.Id;
            StreetNumber = sewerOpening.StreetNumber;
            Street = sewerOpening.Street?.Id;
            AssetType = MapCall.Common.Model.Entities.AssetType.Indices.SEWER_OPENING;
            CoordinateId = sewerOpening.Coordinate?.Id;
            SewerOpening = sewerOpening.Id;
        }

        public CreateWorkOrder(IContainer container, Equipment equipment) : this(container)
        {
            OperatingCenter = equipment.Facility?.OperatingCenter?.Id;
            Town = equipment.Facility?.Town?.Id;
            TownSection = equipment.Facility?.TownSection?.Id;
            StreetNumber = equipment.Facility?.StreetNumber;
            //TODO: Street, Cross Street once normalized.
            AssetType = MapCall.Common.Model.Entities.AssetType.Indices.EQUIPMENT;
            CoordinateId = equipment.Coordinate?.Id;
            Equipment = equipment.Id;
        }
        
        public CreateWorkOrder(IContainer container, EchoshoreLeakAlert alert) : this(container)
        {
            OperatingCenter = alert.EchoshoreSite?.OperatingCenter?.Id;
            // Hydrants for a leak alert can be linked to towns other than the one identified for
            // the leak alert, the work order needs to be linked to the Hydrant's town.
            Town = alert.Hydrant1?.Town?.Id ?? alert.EchoshoreSite?.Town?.Id;
            if (alert.Hydrant1 != null)
            {
                Hydrant = alert.Hydrant1.Id;
                StreetNumber = alert.Hydrant1.StreetNumber;
                Street = alert.Hydrant1.Street?.Id;
                NearestCrossStreet = alert.Hydrant1.CrossStreet?.Id;
                TownSection = alert.Hydrant1.TownSection?.Id;
                CoordinateId = alert.Hydrant1.Coordinate?.Id;
            }
            Purpose = (int?)WorkOrderPurpose.Indices.SAFETY;
            AssetType = MapCall.Common.Model.Entities.AssetType.Indices.HYDRANT;
            DateReceived = alert.FieldInvestigationRecommendedOn;
            Notes = string.Format(
                ECHOSHORE_NOTE,
                alert.PersistedCorrelatedNoiseId,
                alert.Hydrant1Text,
                alert.Hydrant2Text,
                alert.EchoshoreSite,
                alert.Note,
                alert.DistanceFromHydrant1,
                alert.DistanceFromHydrant2);
            EchoshoreLeakAlert = alert.Id;
        }

        public CreateWorkOrder(IContainer container, SmartCoverAlert alert) : this(container)
        {
            IsRevisit = false;
            OperatingCenter = alert.SewerOpening?.OperatingCenter?.Id;
            Town = alert.SewerOpening?.Town?.Id;
            TownSection = alert.SewerOpening?.TownSection?.Id;
            StreetNumber = alert.SewerOpening?.StreetNumber;
            Street = alert.SewerOpening?.Street?.Id;
            NearestCrossStreet = alert.SewerOpening?.IntersectingStreet?.Id;
            AssetType = MapCall.Common.Model.Entities.AssetType.Indices.SEWER_OPENING;
            SewerOpening = alert.SewerOpening?.Id;
            RequestedBy = WorkOrderRequester.Indices.ACOUSTIC_MONITORING;
            AcousticMonitoringType =
                MapCall.Common.Model.Entities.AcousticMonitoringType.Indices.SMART_COVER;
            Priority = (int?)WorkOrderPriority.Indices.EMERGENCY;
            DateReceived = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            Notes = string.Format(
                SMART_COVER_ALERT_NOTE,
                alert.AlertId,
                alert.SewerOpening,
                alert.ApplicationDescription,
                alert.SmartCoverAlertAlarms,
                alert.SmartCoverAlertSmartCoverAlertTypes);
            SmartCoverAlert = alert.Id;
        }

        public CreateWorkOrder(IContainer container, Premise premise) : this(container)
        {
            // NOTE: This logic was originally done in MC-3528 as part of Premise/Show.cshtml.
            OperatingCenter = premise.OperatingCenter?.Id;
            Town = premise.ServiceCity?.Id;
            Street = premise.Street?.Id;
            StreetNumber = premise.ServiceAddressHouseNumber;
            ApartmentAddtl = premise.ServiceAddressApartment;
            ZipCode = premise.ServiceZip;
            CoordinateId = premise.Coordinate?.Id;

            if (premise.ServiceUtilityType == null)
            {
                return;
            }

            PremiseNumber = premise.PremiseNumber;
            if (!string.IsNullOrWhiteSpace(premise.DeviceLocation) &&
                long.TryParse(premise.DeviceLocation, out var location))
            {
                DeviceLocation = location;
            }

            if (!string.IsNullOrWhiteSpace(premise.Installation) &&
                long.TryParse(premise.Installation, out var installation))
            {
                Installation = installation;
            }

            if (!string.IsNullOrWhiteSpace(premise.Equipment) &&
                long.TryParse(premise.Equipment, out var equipment))
            {
                SAPEquipmentNumber = equipment;
            }
            MeterSerialNumber = premise.MeterSerialNumber;
            ServiceUtilityType = premise.ServiceUtilityType.Description;

            // This needs to be the non-normalized data for now so users can verify the raw data against
            // what we have in MapCall.
            PremiseAddress =
                $"{premise.FullStreetAddress}, " +
                $"{premise.ServiceCity?.ShortName}, " +
                $"{premise.ServiceCity?.State.Abbreviation} " +
                premise.ServiceZip;

            var serviceUtilityTypesForSewerLateral = new[] {
                MapCall.Common.Model.Entities.ServiceUtilityType.Indices.DOMESTIC_WASTEWATER,
                MapCall.Common.Model.Entities.ServiceUtilityType.Indices.NON_POTABLE,
                MapCall.Common.Model.Entities.ServiceUtilityType.Indices.WASTE_WATER_WITH_DEDUCT_SERVICE,
            };

            AssetType = serviceUtilityTypesForSewerLateral.Contains(premise.ServiceUtilityType.Id)
                ? MapCall.Common.Model.Entities.AssetType.Indices.SEWER_LATERAL
                : MapCall.Common.Model.Entities.AssetType.Indices.SERVICE;
        }

        #endregion

        #region Private Methods

        private int? DeterminePriority(Service service)
        {
            if (service.ServicePriority != null)
            {
                switch (service.ServicePriority.Id)
                {
                    case ServicePriority.Indices.EMERGENCY:
                        return (int)WorkOrderPriority.Indices.EMERGENCY;
                    case ServicePriority.Indices.RUSH_THREE_DAY:
                        return (int)WorkOrderPriority.Indices.HIGH_PRIORITY;
                    case ServicePriority.Indices.ROUTINE:
                        return (int)WorkOrderPriority.Indices.ROUTINE;
                }
            }

            return null;
        }

        private IEnumerable<ValidationResult> ValidatePlannedCompletionDate()
        {
            var today = _container.GetInstance<IDateTimeProvider>().GetCurrentDate().BeginningOfDay();

            if (Priority == (int)WorkOrderPriority.Indices.EMERGENCY)
            {
                if (PlannedCompletionDate?.BeginningOfDay() < today)
                {
                    yield return new ValidationResult(CreateWorkOrder.PLANNED_COMPLETION_DATE_ERROR_MESSAGE,
                        new[] { "PlannedCompletionDate" });
                }
            }
            else if (PlannedCompletionDate?.BeginningOfDay() < today.AddDays(2))
            {
                yield return new ValidationResult(
                    CreateWorkOrder.PLANNED_COMPLETION_DATE_ERROR_MESSAGE,
                    new[] { "PlannedCompletionDate" });
            }
        }

        private IEnumerable<ValidationResult> ValidateBRBWorkDescriptions() 
        {
            if (SendToSAP &&
                WorkDescription.HasValue &&
                MapCall.Common.Model.Entities.WorkDescription.BRB_PMAT_DESCRIPTIONS
                       .Contains(WorkDescription.Value) &&
                !PlantMaintenanceActivityTypeOverride.HasValue)
                yield return new ValidationResult(
                    ErrorMessages.BRB_PLANT_MAINTENANCE_ACTIVITY_CODE,
                    new[] { "PlantMaintenanceActivityTypeOverride" });
        }

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
                entity.CompanyServiceLineMaterial = recentService.ServiceMaterial;
            }
            entity.CompanyServiceLineSize = recentService.ServiceSize;
            if (recentService?.CustomerSideMaterial?.Description.ToUpper() != ServiceMaterial.Descriptions.UNKNOWN)
            {
                entity.CustomerServiceLineMaterial = recentService.CustomerSideMaterial;
            }
            entity.CustomerServiceLineSize = recentService.CustomerSideSize;
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

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext)
                       .Concat(ValidateServiceForSap())
                       .Concat(ValidatePlannedCompletionDate())
                       .Concat(ValidateBRBWorkDescriptions());
        }

        public override WorkOrder MapToEntity(WorkOrder entity)
        {
            if (RequestedBy != WorkOrderRequester.Indices.EMPLOYEE)
            {
                RequestingEmployee = null;
            }

            entity = base.MapToEntity(entity);

            if (Service.HasValue)
            {
                var service = _container.GetInstance<IServiceRepository>().Find(Service.Value);
                PremiseNumber = service.PremiseNumber;
                ServiceNumber = service.ServiceNumber.ToString();
            }
            if (SmartCoverAlert.HasValue)
            {
                var smartCoverAlert = _container
                                     .GetInstance<IRepository<SmartCoverAlert>>()
                                     .Find(SmartCoverAlert.Value);
                entity.SmartCoverAlert = smartCoverAlert;
                entity.SmartCoverAlert.Acknowledged = true;
                entity.SmartCoverAlert.NeedsToSync = true;
            }

            if (AssetType == MapCall.Common.Model.Entities.AssetType.Indices.SERVICE &&
                MapCall.Common.Model.Entities.WorkDescription.SERVICE_LINE_RENEWALS
                       .Contains(WorkDescription.Value))
            {
                MaybeMapRecentServiceSizesAndMaterials(entity);
            }

            return entity;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            var currentUser = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
            if (OperatingCenter == null)
            {
                OperatingCenter = currentUser.DefaultOperatingCenter.Id;
            }
            RequestingEmployee = currentUser.Id;
        }

        #endregion
    }
}
