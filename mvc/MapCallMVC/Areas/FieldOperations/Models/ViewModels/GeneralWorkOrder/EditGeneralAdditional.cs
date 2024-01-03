using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.GeneralWorkOrder
{
    public class EditGeneralAdditional : EditWorkOrderAdditional
    {
        #region Constructor

        public EditGeneralAdditional(IContainer container) : base(container) { }

        #endregion

        #region Properties

        #region Service Line Info

        [RequiredWhen(nameof(FinalWorkDescription), ComparisonType.EqualToAny,
            nameof(ServiceLineRenewalWorkDescriptions), typeof(EditWorkOrderAdditional))]
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceMaterial))]
        public int? PreviousServiceLineMaterial { get; set; }

        [RequiredWhen(nameof(FinalWorkDescription), ComparisonType.EqualToAny,
            nameof(ServiceLineRenewalWorkDescriptions), typeof(EditWorkOrderAdditional))]
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceSize))]
        public int? PreviousServiceLineSize { get; set; }

        [RequiredWhen(nameof(FinalWorkDescription), ComparisonType.EqualToAny,
            nameof(ServiceLineInfoWorkDescriptions), typeof(EditWorkOrderAdditional))]
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceMaterial))]
        public int? CompanyServiceLineMaterial { get; set; }

        [RequiredWhen(nameof(FinalWorkDescription), ComparisonType.EqualToAny,
            nameof(ServiceLineInfoWorkDescriptions), typeof(EditWorkOrderAdditional))]
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceSize))]
        public int? CompanyServiceLineSize { get; set; }

        [RequiredWhen(nameof(FinalWorkDescription), ComparisonType.EqualToAny,
            nameof(ServiceLineRenewalWorkDescriptions), typeof(EditWorkOrderAdditional))]
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceMaterial))]
        public int? CustomerServiceLineMaterial { get; set; }

        [RequiredWhen(nameof(FinalWorkDescription), ComparisonType.EqualToAny,
            nameof(ServiceLineRenewalWorkDescriptions), typeof(EditWorkOrderAdditional))]
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceSize))]
        public int? CustomerServiceLineSize { get; set; }

        [RequiredWhen(nameof(FinalWorkDescription), ComparisonType.EqualToAny,
            nameof(ServiceLineRenewalWorkDescriptions), typeof(EditWorkOrderAdditional))]
        public DateTime? DoorNoticeLeftDate { get; set; }

        #endregion

        #region Compliance Info

        [RequiredWhen(nameof(FinalWorkDescription), ComparisonType.EqualToAny,
            nameof(ServiceLineRenewalWorkDescriptions), typeof(EditWorkOrderAdditional),
            ErrorMessage = "The InitialServiceLineFlushTime field is required.")]
        public int? InitialServiceLineFlushTime { get; set; }

        [RequiredWhen(nameof(FinalWorkDescription), ComparisonType.EqualToAny,
            nameof(ServiceLineRenewalWorkDescriptions), typeof(EditWorkOrderAdditional))]
        public bool? HasPitcherFilterBeenProvidedToCustomer { get; set; }

        [RequiredWhen(nameof(HasPitcherFilterBeenProvidedToCustomer), ComparisonType.EqualTo, true)]
        public DateTime? DatePitcherFilterDeliveredToCustomer { get; set; }

        [RequiredWhen(nameof(HasPitcherFilterBeenProvidedToCustomer), ComparisonType.EqualTo, true)]
        [DropDown, EntityMap, EntityMustExist(typeof(PitcherFilterCustomerDeliveryMethod))]
        public int? PitcherFilterCustomerDeliveryMethod { get; set; }

        [RequiredWhen(nameof(PitcherFilterCustomerDeliveryMethod), ComparisonType.EqualTo,
            MapCall.Common.Model.Entities.PitcherFilterCustomerDeliveryMethod.Indices.OTHER)]
        public string PitcherFilterCustomerDeliveryOtherMethod { get; set; }

        public DateTime? DateCustomerProvidedAWStateLeadInformation { get; set; }

        #endregion

        #endregion

        #region Public Methods

        public override WorkOrder MapToEntity(WorkOrder entity)
        {
            base.MapToEntity(entity);

            if (entity.Service != null)
            {
                var service = entity.Service;
                if (entity.PreviousServiceLineMaterial != null)
                {
                    service.PreviousServiceMaterial = entity.PreviousServiceLineMaterial;
                }
                if (entity.PreviousServiceLineSize != null)
                {
                    service.PreviousServiceSize = entity.PreviousServiceLineSize;
                }
                if (entity.CustomerServiceLineMaterial != null)
                {
                    // set sync if values are different
                    if (service.Premise != null && service.CustomerSideMaterial?.Id != entity.CustomerServiceLineMaterial?.Id)
                    {
                        service.NeedsToSync = true;
                    }
                    service.CustomerSideMaterial = entity.CustomerServiceLineMaterial;
                }
                if (entity.CustomerServiceLineSize != null)
                {
                    service.CustomerSideSize = entity.CustomerServiceLineSize;
                }
                if (entity.CompanyServiceLineMaterial != null)
                {
                    // set sync if values are different
                    if (service.Premise != null && service.ServiceMaterial?.Id != entity.CompanyServiceLineMaterial?.Id)
                    {
                        service.NeedsToSync = true;
                    }
                    service.ServiceMaterial = entity.CompanyServiceLineMaterial;
                }
                if (entity.CompanyServiceLineSize != null)
                {
                    service.ServiceSize = entity.CompanyServiceLineSize;
                }

                var serviceRepo = _container.GetInstance<IServiceRepository>();
                serviceRepo.Save(service);
            }

            return entity;
        }

        #endregion
    }
}