using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using StructureMap;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.GeneralWorkOrder;
using MMSINC.Validation;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Authentication;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Utilities;
using MapCall.Common.Model.Repositories;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderFinalization
{
    public class EditWorkOrderFinalization : EditWorkOrderAdditional
    {
        #region Constants

        public const string MAIN_BREAK_INFO_ERROR_MESSAGE =
                                "A main break work order cannot be finalized without main break information. Please enter some main break information in the Main Break tab.",
                            OPEN_CREW_ASSIGNMENTS_ERROR_MESSAGE =
                                "This order has one or more Crew Assignments that are not closed.  Please ensure that all end times are recorded.",
                            SCHEDULE_OF_VALUES_ERROR_MESSAGE = 
                                "This work order cannot be finalized without schedule of value information.  Please enter schedule of value information in the Schedule of Values tab.",
                            ERROR_METER_LOCATION_REQUIRED = "The MeterLocation field is required.";

        #endregion

        #region Constructor

        public EditWorkOrderFinalization(IContainer container) : base(container) { }

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
            nameof(ServiceLineRenewalWorkDescriptions), typeof(EditWorkOrderAdditional))]
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceMaterial))]
        public int? CompanyServiceLineMaterial { get; set; }

        [RequiredWhen(nameof(FinalWorkDescription), ComparisonType.EqualToAny,
            nameof(ServiceLineRenewalWorkDescriptions), typeof(EditWorkOrderAdditional))]
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

        #region Finalize

        [Required, AutoMap("DateCompleted"), View("Date Completed")]
        public DateTime? CompletedDate { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(WorkOrderFlushingNoticeType))]
        public int? FlushingNoticeType { get; set; }

        [RequiredWhen(nameof(DigitalAsBuiltRequired), true)]
        public bool? DigitalAsBuiltCompleted { get; set; }

        [AutoMap(MapDirections.None)]
        public bool? DigitalAsBuiltRequired => WorkOrder?.DigitalAsBuiltRequired;

        [DropDown, EntityMap, EntityMustExist(typeof(MeterLocation))]
        [RequiredWhen(nameof(AssetTypeId), AssetType.Indices.SERVICE)]
        [ClientCallback("WorkOrderFinalization.validateMeterLocation", ErrorMessage = ERROR_METER_LOCATION_REQUIRED)]
        public int? MeterLocation { get; set; }

        #endregion

        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidateMainBreakInfo()
        {
            if (FinalWorkDescription != null && 
                WorkDescription.GetMainBreakWorkDescriptions().Contains(FinalWorkDescription.Value) &&
                WorkOrder != null && !WorkOrder.MainBreaks.Any())
            {
                yield return new ValidationResult(MAIN_BREAK_INFO_ERROR_MESSAGE);
            }
        }

        private IEnumerable<ValidationResult> ValidateCrewAssignments()
        {
            if (WorkOrder != null && WorkOrder.CrewAssignments.Any(x => x.DateStarted != null & x.DateEnded == null))
            {
                yield return new ValidationResult(OPEN_CREW_ASSIGNMENTS_ERROR_MESSAGE);
            }
        }

        private IEnumerable<ValidationResult> ValidateScheduleOfValues()
        {
            if (WorkOrder != null && WorkOrder.OperatingCenter.HasWorkOrderInvoicing && !WorkOrder.WorkOrdersScheduleOfValues.Any())
            {
                yield return new ValidationResult(SCHEDULE_OF_VALUES_ERROR_MESSAGE);
            }
        }

        private IEnumerable<ValidationResult> ValidateMeterLocation()
        {
            if (WorkOrder != null && AssetTypeId == AssetType.Indices.SERVICE && (MeterLocation == null
                    || MeterLocation.Value == MapCall.Common.Model.Entities.MeterLocation.Indices.UNKNOWN))
            {
                yield return new ValidationResult(ERROR_METER_LOCATION_REQUIRED);
            }
        }

        #endregion

        #region Public Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext)
                       .Concat(ValidateMainBreakInfo())
                       .Concat(ValidateCrewAssignments())
                       .Concat(ValidateScheduleOfValues())
                       .Concat(ValidateMeterLocation());
        }

        public override WorkOrder MapToEntity(WorkOrder entity)
        {
            base.MapToEntity(entity);

            if (InitialServiceLineFlushTime.HasValue)
            {
                entity.InitialFlushTimeEnteredBy =
                    _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
                entity.InitialFlushTimeEnteredAt = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            }

            entity.CompletedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;

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
