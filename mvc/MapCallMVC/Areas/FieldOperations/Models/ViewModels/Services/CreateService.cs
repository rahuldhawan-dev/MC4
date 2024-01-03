using System.ComponentModel.DataAnnotations;
using System.Linq;
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
using ServicePriorityEntity = MapCall.Common.Model.Entities.ServicePriority;
using ServiceCategoryEntity = MapCall.Common.Model.Entities.ServiceCategory;
using ServiceInstallationPurposeEntity = MapCall.Common.Model.Entities.ServiceInstallationPurpose;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services
{
    public class CreateService : ServiceViewModel
    {
        #region Properties

        [DropDown("", "OperatingCenter", "ActiveByStateIdForFieldServicesAssets", DependsOn = "State", PromptText = "Select a state above"), EntityMap, EntityMustExist(typeof(OperatingCenter)), Required]
        public int? OperatingCenter { get; set; }

        [EntityMap, EntityMustExist(typeof(Street))]
        [DropDown("", "Street", "GetActiveByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        [RequiredWhen("Block", ComparisonType.EqualTo, "", ErrorMessage = ErrorMessages.BLOCK_AND_LOT_OR_STREET_ADDRESS)]
        [RequiredWhen("DateInstalled", ComparisonType.NotEqualTo, null)]
        public int? Street { get; set; }

        [EntityMap, EntityMustExist(typeof(Street))]
        [DropDown("", "Street", "GetActiveByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        [RequiredWhen("RetiredDate", ComparisonType.NotEqualTo, null)]
        public int? CrossStreet { get; set; }

        [EntityMap, EntityMustExist(typeof(Service)), Secured]
        public int? RenewalOf { get; set; }

        [AutoMap(MapDirections.None)]
        public bool? Copy { get; set; }
        [AutoMap(MapDirections.None)]
        public bool? WithServiceNumber { get; set; }
        [AutoMap(MapDirections.None)]
        public bool? ForSewer { get; set; }

        [DoesNotAutoMap("Not an actual view property, set by MapToEntity and used by controller.")]
        public bool SendRenewalInstalledNotificationOnSave { get; set; }
        [DoesNotAutoMap("Not an actual view property, set by MapToEntity and used by controller.")]
        public bool SendServiceInstallationNotificationOnSave { get; set; }
        [DoesNotAutoMap("Not an actual view property, set by MapToEntity and used by controller.")]
        public bool SendRenewalAtServiceWithSampleSiteNotificationOnSave { get; set; }
        
        [DoesNotAutoMap("Not an actual view property, set by MapToEntity and used by controller.")]
        public bool SendServiceWithSampleSitesNotificationOnSave { get; set; }
        
        [DoesNotAutoMap]
        public bool IsExistingOrRenewal { get; set; }
       
        public long? ServiceNumber { get; set; }

        [EntityMap, EntityMustExist(typeof(ServiceMaterial)), View(Service.DisplayNames.SERVICE_MATERIAL)]
        [DropDown("", "ServiceMaterial", "ByOperatingCenterIdNewServices", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        [RequiredWhen("DateInstalled", ComparisonType.NotEqualTo, null)]
        public int? ServiceMaterial { get; set; }

        // This is a hidden field used when a service is being created from a workorder.
        [DoesNotAutoMap("Set manually in MapToEntity")]
        [EntityMustExist(typeof(WorkOrder))]
        public int? WorkOrder { get; set; }

        [DropDown("", "TownSection", "ActiveByTownId", DependsOn = "Town", PromptText = "Select a town from above")]
        [EntityMap, EntityMustExist(typeof(TownSection))]
        public override int? TownSection { get; set; }

        [DropDown("", "User", "ActiveFieldServicesAssetsUsersByOperatingCenter", DependsOn = "OperatingCenter", PromptText = "Select an operating center")]
        public override int? ProjectManager { get => base.ProjectManager; set => base.ProjectManager = value; }

        [Multiline]
        public string JobNotes { get; set; }

        [DoesNotAutoMap]
        public virtual bool HasLinkedSampleSiteByInstallation => _container.GetInstance<IRepository<Service>>().Any(x => x.Installation == Installation && x.OperatingCenter.Id == OperatingCenter);

        #endregion

        #region Constructors

        [DefaultConstructor]
        public CreateService(IContainer container) : base(container) { }

        public CreateService(IContainer container, WorkOrder workOrder) : this(container)
        {
            WorkOrder = workOrder.Id;
            OperatingCenter = workOrder.OperatingCenter.Id;
            Town = workOrder.Town.Id;
            TownSection = workOrder.TownSection?.Id;
            StreetNumber = workOrder.StreetNumber;
            ApartmentNumber = workOrder.ApartmentAddtl;
            Street = workOrder.Street?.Id;
            CrossStreet = workOrder.NearestCrossStreet?.Id;
            Zip = workOrder.ZipCode;
            PremiseNumber = workOrder.PremiseNumber;
            SAPNotificationNumber = workOrder.SAPNotificationNumber;
            SAPWorkOrderNumber = workOrder.SAPWorkOrderNumber;
            State = workOrder.Town.State.Id;
            //if (!WorkDescriptionRepository.NEW_SERVICE_INSTALLATIONS.Contains(workOrder.WorkDescription.Id))
            IsExistingOrRenewal = true; // Service number is required in this case.
            DeviceLocation = workOrder.DeviceLocation?.ToString();
            TaskNumber1 = workOrder.AccountCharged;
            Installation = workOrder.Installation?.ToString();

            long servNum;
            if (long.TryParse(workOrder.ServiceNumber, out servNum))
            {
                ServiceNumber = servNum;
            }

            // Not really a fan of saving things to the database here.
            var clonedCoord = _container.GetInstance<ICoordinateRepository>().CloneAndSave(workOrder.Coordinate);
            Coordinate = clonedCoord.Id;

            if (workOrder.WorkDescription != null &&
                WorkDescription.AUTO_CREATE_SERVICE_WORK_DESCRIPTIONS.Contains(workOrder.WorkDescription.Id))
            {
                ServiceMaterial = workOrder.CompanyServiceLineMaterial?.Id;
                ServiceSize = workOrder.CompanyServiceLineSize?.Id;
                CustomerSideMaterial = workOrder.CustomerServiceLineMaterial?.Id;
                CustomerSideSize = workOrder.CustomerServiceLineSize?.Id;
                ServiceInstallationPurpose = ServiceInstallationPurposeEntity.Indices.NEW_SERVICE;

                if (workOrder.Priority?.Id == (int)WorkOrderPriority.Indices.EMERGENCY)
                {
                    ServicePriority = ServicePriorityEntity.Indices.EMERGENCY;
                }
                else if (workOrder.Priority?.Id == (int)WorkOrderPriority.Indices.HIGH_PRIORITY)
                {
                    ServicePriority = ServicePriorityEntity.Indices.RUSH_THREE_DAY;
                }
                else if (workOrder.Priority?.Id == (int)WorkOrderPriority.Indices.ROUTINE)
                {
                    ServicePriority = ServicePriorityEntity.Indices.ROUTINE;
                }

                switch (workOrder.WorkDescription.Id)
                {
                    case (int)WorkDescription.Indices.SERVICE_LINE_INSTALLATION:
                    case (int)WorkDescription.Indices.SERVICE_LINE_INSTALLATION_PARTIAL:
                        ServiceCategory = ServiceCategoryEntity.Indices.WATER_SERVICE_NEW_DOMESTIC;
                        break;
                    case (int)WorkDescription.Indices.FIRE_SERVICE_INSTALLATION:
                        ServiceCategory = ServiceCategoryEntity.Indices.FIRE_SERVICE_INSTALLATION;
                        break;
                    case (int)WorkDescription.Indices.IRRIGATION_INSTALLATION:
                        ServiceCategory = ServiceCategoryEntity.Indices.IRRIGATION_NEW;
                        break;
                    case (int)WorkDescription.Indices.SEWER_LATERAL_INSTALLATION:
                        ServiceCategory = ServiceCategoryEntity.Indices.SEWER_SERVICE_NEW;
                        break;
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// If it wasn't installed and it's now being installed
        /// and it's either Sewer or Water Service Renewal 
        /// send out that notification
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void SetSendRenewalInstalledNotificationOnSave(Service entity)
        {
            SendRenewalInstalledNotificationOnSave = false;
            if (entity.ServiceCategory != null
                //&& !PreviousDateInstalled.HasValue // this will always be empty for newly created records
                && DateInstalled.HasValue
                &&
                (
                    entity.ServiceCategory.Id == MapCall.Common.Model.Entities.ServiceCategory.Indices.SEWER_SERVICE_RENEWAL
                    ||
                    entity.ServiceCategory.Id == MapCall.Common.Model.Entities.ServiceCategory.Indices.WATER_SERVICE_RENEWAL
                ))
            {
                SendRenewalInstalledNotificationOnSave = true;
            }
        }

        protected virtual void SetSendServiceInstallationNotificationOnSave(Service entity)
        {
            SendServiceInstallationNotificationOnSave = false;
            if (entity.ServiceCategory != null
                && MapCall.Common.Model.Entities.ServiceCategory.GetNewServiceCategories().Any(x => x == entity.ServiceCategory.Id)
                && DateInstalled.HasValue)
            {
                SendServiceInstallationNotificationOnSave = true;
            }
        }

        protected virtual void SetSendRenewalAtServiceWithSampleSiteNotificationOnSave(Service entity)
        {
            SendRenewalAtServiceWithSampleSiteNotificationOnSave = false;

            if (entity.RenewalOf?.Premise != null && 
                entity.ServiceCategory != null && 
                MapCall.Common
                       .Model
                       .Entities
                       .ServiceCategory
                       .GetRenewalSampleSiteServiceCategories()
                       .Any(x => x == entity.ServiceCategory.Id) && 
                DateInstalled.HasValue)
            {
                if (entity.RenewalOf.Premise.SampleSites.Any() || 
                    HasLinkedSampleSiteByInstallation)
                {
                    SendRenewalAtServiceWithSampleSiteNotificationOnSave = true;
                }
            }
        }
        
        protected virtual void SetSendServiceWithSampleSiteNotificationOnSave(Service entity)
        {
            if (entity.ServiceCategory != null && HasLinkedSampleSiteByInstallation)
            {
                SendServiceWithSampleSitesNotificationOnSave = true;
            }
        }

        #endregion

        #region Exposed Methods

        public override Service MapToEntity(Service entity)
        {
            base.MapToEntity(entity);
            SetSendRenewalInstalledNotificationOnSave(entity);
            SetSendServiceInstallationNotificationOnSave(entity);
            SetSendRenewalAtServiceWithSampleSiteNotificationOnSave(entity);
            if (!SendRenewalAtServiceWithSampleSiteNotificationOnSave)
            {
                SetSendServiceWithSampleSiteNotificationOnSave(entity);
            }

            entity.IsActive = true;
            entity.Initiator = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
            var operatingCenter = _container.GetInstance<IRepository<OperatingCenter>>().Find(OperatingCenter.Value);
            SendToSAP = operatingCenter.CanSyncWithSAP;

            if (!ServiceNumber.HasValue && !IsExistingOrRenewal)
            {
                entity.ServiceNumber = _container.GetInstance<IServiceRepository>().GetNextServiceNumber(operatingCenter.OperatingCenterCode);
            }

            if (CustomerSideSLReplacement.HasValue
                && CustomerSideSLReplacement == CustomerSideSLReplacementOfferStatus.Indices.ACCEPTED
                && operatingCenter != null
                && operatingCenter.DefaultServiceReplacementWBSNumber != null)
            {
                entity.CustomerSideReplacementWBSNumber = _container.GetInstance<IRepository<WBSNumber>>().Find(operatingCenter.DefaultServiceReplacementWBSNumber.Id);
            }

            if (WorkOrder.HasValue)
            {
                var wo = _container.GetInstance<IWorkOrderRepository>().Find(WorkOrder.Value);
                entity.WorkOrders.Add(wo);
            }

            // Set NeedsToSync to true so can be published to W1V
            if (entity.Premise != null)
            {
                entity.NeedsToSync = true;
            }
            
            return entity;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            
            if (RenewalOf.HasValue && (!Copy.HasValue || Copy == false))
            {
                // As far as I can tell, this block only gets hit when coming from the "Add a Renewal".
                // This is used in conjunction with the Service/New action where no automatic mapping is done for some reason?

                var service = _container.GetInstance<IServiceRepository>().Find(RenewalOf.Value);
                if (service != null)
                {
                    OperatingCenter = service.OperatingCenter?.Id;
                    PremiseNumber = service.PremiseNumber;
                    StreetNumber = service.StreetNumber;
                    Street = service.Street?.Id;
                    Town = service.Town?.Id;
                    State = service.State?.Id;
                    Zip = service.Zip;
                    CrossStreet = service.CrossStreet?.Id;
                    ContactDate = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
                    IsExistingOrRenewal = true;
                    ServiceNumber = service.ServiceNumber;

                    // TODO: Bug 2726 - Require fields
                    // cat of serv, purp install, serv size, prev serv mat, prev serv size
                }
            }
            if (Copy == true)
            {
                if (WithServiceNumber.HasValue && WithServiceNumber == true)
                {
                    IsExistingOrRenewal = true;
                    ServiceCategory = null;
                }
                else if (ForSewer.HasValue && ForSewer.Value)
                {
                    ClearNumber5Stuff();
                    ClearNumber11Stuff();
                    DeviceLocation = null; // Is this necessary? It's set to null outside of this if block anyway.
                }
                else
                {
                    IsExistingOrRenewal = false;
                    ServiceNumber = null;
                    PremiseNumber = null;
                }
                DeviceLocation = null;
            }
        }

        private void ClearNumber5Stuff()
        {
            ServiceCategory = null;
            ServiceInstallationPurpose = null;
            TaskNumber1 = null;
            SAPNotificationNumber = null;
            SAPWorkOrderNumber = null;
            DeveloperServicesDriven = null;
            Agreement = null;
            MainType = null;
            MainSize = null;
            CustomerSideMaterial = null;
            CustomerSideSize = null;
            LeadAndCopperCommunicationProvided = false;
            TapOrderNotes = null;
            BureauOfSafeDrinkingWaterPermitRequired = null;
            ParentTaskNumber = null;
            TaskNumber2 = null;
            MeterSettingRequirement = null;
            MeterSettingSize = null;
            ServiceMaterial = null;
            ServiceSize = null;
        }

        private void ClearNumber11Stuff()
        {
            InstallationInvoiceNumber = null;
            PurchaseOrderNumber = null;
            InstallationInvoiceDate = null;
            InstallationCost = null;
        }

        #endregion
    }
}
