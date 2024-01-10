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
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Authentication;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderFinalization
{
    public class EditWorkOrderFinalization : ViewModel<WorkOrder>, IServiceLineInfo, IWorkOrderAdditional, IWorkOrderComplianceData
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

        #region Fields

        private WorkOrder _original;

        #endregion

        #region Properties

        [DoesNotAutoMap]
        public WorkOrder WorkOrder
        {
            get
            {
                if (_original == null)
                {
                    _original = Original ?? _container.GetInstance<IRepository<WorkOrder>>().Find(Id);
                }
                return _original;
            }
        }

        #region Additional Details

        [DropDown, EntityMap("WorkDescription"), EntityMustExist(typeof(WorkDescription))]
        public int? FinalWorkDescription { get; set; }
        public int? LostWater { get; set; }
        public double? DistanceFromCrossStreet { get; set; }

        [DropDown, EntityMap("EstimatedCustomerImpact"), EntityMustExist(typeof(CustomerImpactRange))]
        public int? CustomerImpact { get; set; }

        [DropDown, EntityMap("AnticipatedRepairTime"), EntityMustExist(typeof(RepairTimeRange))]
        public int? RepairTime { get; set; }

        [AutoMap("SignificantTrafficImpact")]
        public bool? TrafficImpact { get; set; }

        public bool? AlertIssued { get; set; }

        [DoesNotAutoMap]
        public string AppendNotes { get; set; }

        #endregion

        #region Service Line Info

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceMaterial))]
        public int? PreviousServiceLineMaterial { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceSize))]
        public int? PreviousServiceLineSize { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceMaterial))]
        public int? CompanyServiceLineMaterial { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceSize))]
        public int? CompanyServiceLineSize { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceMaterial))]
        public int? CustomerServiceLineMaterial { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceSize))]
        public int? CustomerServiceLineSize { get; set; }

        public DateTime? DoorNoticeLeftDate { get; set; }

        #endregion

        #region Compliance Data

        public int? InitialServiceLineFlushTime { get; set; }
        public bool? HasPitcherFilterBeenProvidedToCustomer { get; set; }
        public DateTime? DatePitcherFilterDeliveredToCustomer { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(PitcherFilterCustomerDeliveryMethod))]
        public int? PitcherFilterCustomerDeliveryMethod { get; set; }
        public string PitcherFilterCustomerDeliveryOtherMethod { get; set; }
        public DateTime? DateCustomerProvidedAWStateLeadInformation { get; set; }
        
        [RequiredWhen(nameof(HasPitcherFilterBeenProvidedToCustomer), ComparisonType.EqualTo, true)]
        public bool? IsThisAMultiTenantFacility { get; set; }
        
        [RequiredWhen(nameof(IsThisAMultiTenantFacility), ComparisonType.EqualTo, true)]
        public int? NumberOfPitcherFiltersDelivered { get; set; }
        
        [RequiredWhen(nameof(IsThisAMultiTenantFacility), ComparisonType.EqualTo, true)]
        public string DescribeWhichUnits { get; set; }

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

        [AutoMap(MapDirections.None)]
        public int? AssetTypeId => WorkOrder?.AssetType?.Id;

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

            if (!string.IsNullOrWhiteSpace(AppendNotes))
            {
                if (!string.IsNullOrWhiteSpace(entity.Notes))
                {
                    entity.Notes += Environment.NewLine;
                }

                entity.Notes += $"{_container.GetInstance<IAuthenticationService<User>>().CurrentUser.FullName} " +
                                _container.GetInstance<IDateTimeProvider>().GetCurrentDate().ToString(CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE_FOR_WEBFORMS) +
                                $" {AppendNotes}";
            }

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
