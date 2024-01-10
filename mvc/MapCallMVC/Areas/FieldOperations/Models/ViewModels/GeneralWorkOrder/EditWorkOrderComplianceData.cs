using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using WorkDescriptionEntity = MapCall.Common.Model.Entities.WorkDescription;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.GeneralWorkOrder
{
    public class EditWorkOrderComplianceData : ViewModel<WorkOrder>, IWorkOrderComplianceData
    {
        #region Constructors

        public EditWorkOrderComplianceData(IContainer container) : base(container) { }

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

        public int? InitialServiceLineFlushTime { get; set; }
        
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

        [RequiredWhen(nameof(HasPitcherFilterBeenProvidedToCustomer), ComparisonType.EqualTo, true)]
        public bool? IsThisAMultiTenantFacility { get; set; }

        [RequiredWhen(nameof(IsThisAMultiTenantFacility), ComparisonType.EqualTo, true)]
        public int? NumberOfPitcherFiltersDelivered { get; set; }

        [RequiredWhen(nameof(IsThisAMultiTenantFacility), ComparisonType.EqualTo, true)]
        public string DescribeWhichUnits { get; set; }

        #endregion

        #region Private Methods

        public static int[] ServiceLineRenewalWorkDescriptions() => WorkDescriptionEntity.SERVICE_LINE_RENEWALS;

        private IEnumerable<ValidationResult> ValidateComplianceData()
        {
            if (WorkOrder.WorkDescription != null)
            {
                if (ServiceLineRenewalWorkDescriptions().Contains(WorkOrder.WorkDescription.Id) &&
                    !InitialServiceLineFlushTime.HasValue)
                {
                    yield return new ValidationResult("The InitialServiceLineFlushTime field is required.", new [] { nameof(InitialServiceLineFlushTime)});
                }

                if (ServiceLineRenewalWorkDescriptions().Contains(WorkOrder.WorkDescription.Id) &&
                    !HasPitcherFilterBeenProvidedToCustomer.HasValue)
                {
                    yield return new ValidationResult("The HasPitcherFilterBeenProvidedToCustomer field is required.", new[] { nameof(HasPitcherFilterBeenProvidedToCustomer) });
                }
            }
        }

        #endregion

        #region Public Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext)
                       .Concat(ValidateComplianceData());
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

            return entity;
        }

        #endregion
    }

    public interface IWorkOrderComplianceData
    {
        int? InitialServiceLineFlushTime { get; set; }

        bool? HasPitcherFilterBeenProvidedToCustomer { get; set; }

        DateTime? DatePitcherFilterDeliveredToCustomer { get; set; }

        int? PitcherFilterCustomerDeliveryMethod { get; set; }

        string PitcherFilterCustomerDeliveryOtherMethod { get; set; }

        DateTime? DateCustomerProvidedAWStateLeadInformation { get; set; }

        bool? IsThisAMultiTenantFacility { get; set; }

        int? NumberOfPitcherFiltersDelivered { get; set; }

        string DescribeWhichUnits { get; set; }
    }
}