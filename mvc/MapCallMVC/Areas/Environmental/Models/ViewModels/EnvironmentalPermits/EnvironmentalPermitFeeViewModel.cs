using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DataAnnotationsExtensions;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalPermits
{
    public class EnvironmentalPermitFeeViewModel : ViewModel<EnvironmentalPermitFee>
    {
        #region Properties

        [Secured, Required, EntityMap, EntityMustExist(typeof(EnvironmentalPermit))]
        public virtual int? EnvironmentalPermit { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(EnvironmentalPermitFeeType))]
        public int? EnvironmentalPermitFeeType { get; set; }

        [Required, Min(0)]
        public decimal? Fee { get; set; }

        [Secured, DoesNotAutoMap("This is used for server-side cascading only. Also it needs to be set manually.")]
        public int[] OperatingCenter { get; set; }

        public DateTime? PaymentEffectiveDate { get; set; }

        [DropDown("", "Employee", "ActiveEmployeesByOperatingCenterIds", DependsOn = nameof(OperatingCenter))]
        [EntityMap, EntityMustExist(typeof(Employee))] 
        public int? PaymentOwner { get; set; }

        [Range(1, 10)] // Why 10? No idea. MC-1836
        public int? PaymentDueInterval { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(RecurringFrequencyUnit))]
        public int? PaymentDueFrequencyUnit { get; set; }

        [StringLength(EnvironmentalPermitFee.StringLengths.PAYMENT_ORGANIZATION_NAME)]
        public string PaymentOrganizationName { get; set; }

        [Multiline]
        public string PaymentOrganizationContactInfo { get; set; }

        [Required]
        [DropDown, EntityMap, EntityMustExist(typeof(EnvironmentalPermitFeePaymentMethod))]
        public int? PaymentMethod { get; set; }

        [AutoMap(MapDirections.ToViewModel)]
        [Multiline, RequiredWhen(nameof(PaymentMethod), EnvironmentalPermitFeePaymentMethod.Indices.MAIL)]
        public string PaymentMethodMailAddress { get; set; }

        [AutoMap(MapDirections.ToViewModel)]
        [RequiredWhen(nameof(PaymentMethod), EnvironmentalPermitFeePaymentMethod.Indices.PHONE)]
        [StringLength(EnvironmentalPermitFee.StringLengths.PAYMENT_METHOD_PHONE)]
        public string PaymentMethodPhone { get; set; }

        [DataAnnotationsExtensions.Url]
        [AutoMap(MapDirections.ToViewModel)]
        [RequiredWhen(nameof(PaymentMethod), EnvironmentalPermitFeePaymentMethod.Indices.URL)]
        [StringLength(EnvironmentalPermitFee.StringLengths.PAYMENT_METHOD_URL)]
        public string PaymentMethodUrl { get; set; }

        #endregion

        #region Constructor

        public EnvironmentalPermitFeeViewModel(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        protected EnvironmentalPermit GetEnvironmentalPermit()
        {
            return _container.GetInstance<IRepository<EnvironmentalPermit>>().Find(EnvironmentalPermit.Value);
        }

        private IEnumerable<ValidationResult> ValidatePermitRequiresFees()
        {
            if (!EnvironmentalPermit.HasValue)
            {
                // Cut out early. Required validation will handle this.
                yield break;
            }

            var permit = GetEnvironmentalPermit();
            if (permit == null)
            {
                // Cut out early here too. Same reason as above but with EntityMustExist.
                yield break;
            }

            if (!permit.RequiresFees)
            {
                yield return new ValidationResult($"Permit#{EnvironmentalPermit} must require fees in order to add or edit a fee.",
                    new[] { nameof(EnvironmentalPermit) });
            }
        }

        #endregion

        #region Public Methods

        public override EnvironmentalPermitFee MapToEntity(EnvironmentalPermitFee entity)
        {
            base.MapToEntity(entity);

            // There can only be one payment method, so it makes sense to blank out the non-selected payment methods
            // if they had been set previously.
            entity.PaymentMethodMailAddress = PaymentMethod.Value == EnvironmentalPermitFeePaymentMethod.Indices.MAIL ? PaymentMethodMailAddress : null;
            entity.PaymentMethodPhone = PaymentMethod.Value == EnvironmentalPermitFeePaymentMethod.Indices.PHONE ? PaymentMethodPhone : null;
            entity.PaymentMethodUrl = PaymentMethod.Value == EnvironmentalPermitFeePaymentMethod.Indices.URL ? PaymentMethodUrl : null;

            return entity;
        }
        
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidatePermitRequiresFees());
        }

        #endregion
    }
}