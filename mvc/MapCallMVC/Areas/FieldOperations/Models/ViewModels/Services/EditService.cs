using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services
{
    public class EditService : ServiceViewModel
    {
        #region Properties

        #region Regular

        [Range(Service.Range.SAP_RANGE_MIN, Service.Range.SAP_RANGE_MAX)]
        [Secured(AppliesToAdmins = false)]
        public override long? SAPWorkOrderNumber { get; set; }

        [Range(Service.Range.SAP_RANGE_MIN, Service.Range.SAP_RANGE_MAX)]
        [Secured(AppliesToAdmins = false)]
        public override long? SAPNotificationNumber { get; set; }

        public long? ServiceNumber { get; set; }

        [DoesNotAutoMap("Set in MapToEntity. This property shouldn't exist since it's not used outside of this class.")]
        public DateTime? PreviousDateInstalled { get; set; }

        #endregion

        #region Other

        [DoesNotAutoMap("Not an actual view property, set by MapToEntity and used by controller.")]
        public bool SendLargeServiceOrFireNotificationOnSave { get; set; }
        [DoesNotAutoMap("Not an actual view property, set by MapToEntity and used by controller.")]
        public bool SendRenewalInstalledNotificationOnSave { get; set; }
        [DoesNotAutoMap("Not an actual view property, set by MapToEntity and used by controller.")]
        public bool SendServiceInstallationNotificationOnSave { get; set; }
        [DoesNotAutoMap("Not an actual view property, set by MapToEntity and used by controller.")]
        public bool SendDeactivatedServiceWithSampleSiteOnSave { get; set; }

        [DoesNotAutoMap("Not an actual view property, set by MapToEntity and used by controller.")]
        public bool SendServiceWithSampleSitesNotificationOnSave { get; set; }

        #endregion

        #region Associations

        [EntityMap, EntityMustExist(typeof(ServiceMaterial)), View(Service.DisplayNames.SERVICE_MATERIAL)]
        [DropDown("", "ServiceMaterial", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        [RequiredWhen("DateInstalled", ComparisonType.NotEqualTo, null)]
        public int? ServiceMaterial { get; set; }
        
        [EntityMap, EntityMustExist(typeof(Street))]
        [DropDown("", "Street", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        [RequiredWhen("Block", ComparisonType.EqualTo, "", ErrorMessage = ErrorMessages.BLOCK_AND_LOT_OR_STREET_ADDRESS)]
        [RequiredWhen("DateInstalled", ComparisonType.NotEqualTo, null)]
        public int? Street { get; set; }

        [EntityMap, EntityMustExist(typeof(Street))]
        [DropDown("", "Street", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        [RequiredWhen("RetiredDate", ComparisonType.NotEqualTo, null)]
        public int? CrossStreet { get; set; }

        #endregion

        #region Logicalish

        private Service _displayService;

        [DoesNotAutoMap("Display only")]
        public Service DisplayService => _displayService ?? (_displayService = _container.GetInstance<IServiceRepository>().Find(Id));

        [AutoMap(MapDirections.None)]
        public int OperatingCenter => DisplayService.OperatingCenter.Id;

        [AutoMap(MapDirections.None)]
        public bool HasLinkedSampleSiteByInstallation => _container.GetInstance<IRepository<Service>>().Any(x => x.Installation == Installation && x.OperatingCenter.Id == OperatingCenter);

        [AutoMap(MapDirections.ToEntity)]
        public int? PermitType => DisplayService.PermitType?.Id;
        [AutoMap(MapDirections.ToEntity)]
        public DateTime? PermitExpirationDate => DisplayService.PermitExpirationDate;
        [AutoMap(MapDirections.ToEntity)]
        public string PermitNumber => DisplayService.PermitNumber;
        [AutoMap(MapDirections.ToEntity)]
        public DateTime? PermitReceivedDate => DisplayService.PermitReceivedDate;
        [AutoMap(MapDirections.ToEntity)]
        public DateTime? PermitSentDate => DisplayService.PermitSentDate;
        [AutoMap(MapDirections.ToEntity)]
        public string JobNotes => DisplayService.JobNotes;

        #endregion

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
                && !PreviousDateInstalled.HasValue
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void SetSendLargeServiceOrFireNotificationsOnSave(Service entity)
        {
            SendLargeServiceOrFireNotificationOnSave = false;

            if (entity.ServiceCategory != null
                && entity.ServiceCategory.Id == MapCall.Common.Model.Entities.ServiceCategory.Indices.FIRE_SERVICE_RENEWAL
                && DateInstalled.HasValue && !PreviousDateInstalled.HasValue)
            {
                SendLargeServiceOrFireNotificationOnSave = true;
            }
            if (entity.ServiceCategory != null
                &&
                (entity.ServiceCategory.Id == MapCall.Common.Model.Entities.ServiceCategory.Indices.WATER_SERVICE_NEW_COMMERCIAL
                 ||
                 entity.ServiceCategory.Id == MapCall.Common.Model.Entities.ServiceCategory.Indices.WATER_SERVICE_NEW_DOMESTIC)
                && ServiceSize.HasValue && _container.GetInstance<IRepository<ServiceSize>>().Find(ServiceSize.Value).Size >= 3m
                && DateInstalled.HasValue && !PreviousDateInstalled.HasValue
                )
            {
                SendLargeServiceOrFireNotificationOnSave = true;
            }
        }

        protected virtual void SetSampleSitesInActiveIfServiceRetired(Service entity)
        {
            var sampleSitesToInactivate = entity.Premise?
                                                .SampleSites
                                                .Where(x => x.Status.Id != SampleSiteStatus.Indices.INACTIVE)
                                                .ToList();

            if (sampleSitesToInactivate != null && sampleSitesToInactivate.Any())
            {
                var inactiveSampleSiteStatus = _container.GetInstance<IRepository<SampleSiteStatus>>()
                                                         .Find(SampleSiteStatus.Indices.INACTIVE);
                var sampleSiteRepository = _container.GetInstance<IRepository<SampleSite>>();

                foreach (var sampleSite in sampleSitesToInactivate)
                {
                    sampleSite.Status = inactiveSampleSiteStatus;
                    sampleSiteRepository.Save(sampleSite);
                }
            }
        }

        protected virtual void SetSendServiceInstallationNotificationOnSave(Service entity)
        {
            SendServiceInstallationNotificationOnSave = entity.ServiceCategory != null &&
                                                        MapCall.Common
                                                               .Model
                                                               .Entities
                                                               .ServiceCategory
                                                               .GetNewServiceCategories()
                                                               .Contains(entity.ServiceCategory.Id) &&
                                                        !PreviousDateInstalled.HasValue &&
                                                        DateInstalled.HasValue;
        }

        protected virtual void SetSendDeactivatedServiceWithSampleSiteOnSave(Service entity)
        {
            SendDeactivatedServiceWithSampleSiteOnSave = entity.IsActive && 
                                                         IsActive.HasValue && 
                                                         !IsActive.Value && 
                                                         (HasLinkedSampleSiteByInstallation || 
                                                          (entity.Premise != null && entity.Premise.SampleSites.Any()));
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
            var service = _container.GetInstance<IServiceRepository>().Find(entity.Id);
            PreviousDateInstalled = service.DateInstalled;
            SetSendLargeServiceOrFireNotificationsOnSave(entity);
            SetSendRenewalInstalledNotificationOnSave(entity);
            SetSendServiceInstallationNotificationOnSave(entity);
            SetSendDeactivatedServiceWithSampleSiteOnSave(entity);
            
            if (!service.RetiredDate.HasValue && RetiredDate.HasValue)
            {
                SetSampleSitesInActiveIfServiceRetired(entity);
            }

            if (!SendDeactivatedServiceWithSampleSiteOnSave)
            {
                SetSendServiceWithSampleSiteNotificationOnSave(entity);
            }

            // we don't want them resetting them if they've already been set. The may have changed for the operating
            // center since it was set.
            var operatingCenter = _container.GetInstance<IOperatingCenterRepository>().Find(OperatingCenter);
            if (entity.CustomerSideReplacementWBSNumber == null && CustomerSideSLReplacement.HasValue 
                                                                && CustomerSideSLReplacement == CustomerSideSLReplacementOfferStatus.Indices.ACCEPTED)
            {
                if (operatingCenter.DefaultServiceReplacementWBSNumber != null)
                {
                    CustomerSideReplacementWBSNumber = operatingCenter.DefaultServiceReplacementWBSNumber.Id;
                }
            }
            SendToSAP = operatingCenter.CanSyncWithSAP;
            if (PreviousDateInstalled.HasValue)
                SendToSAP = false;
                        
            // Set NeedsToSync to true if customer/company materials have changes
            if (entity.Premise != null && (entity.CustomerSideMaterial?.Id != this.CustomerSideMaterial ||
                entity.ServiceMaterial?.Id != this.ServiceMaterial))
            {
                entity.NeedsToSync = true;
            }
            
            return base.MapToEntity(entity);
        }

        #endregion

        #region Constructors

        public EditService(IContainer container) : base(container) {}

        #endregion
    }
}