using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class RedTagPermitAuthorizationViewModel : ViewModel<ProductionWorkOrder>
    {
        #region Constants

        public const string USER_MUST_HAVE_EMPLOYEE_RECORD = "Your user account must have an associated employee record before you can authorize this record.";

        #endregion

        #region Properties

        [Required]
        public virtual bool? NeedsRedTagPermitAuthorization { get; set; }

        [DoesNotAutoMap("Mapped manually"),
         EntityMustExist(typeof(Employee))]
        public virtual int? NeedsRedTagPermitAuthorizedBy { get; set; }

        [DoesNotAutoMap("Mapped manually")]
        public virtual DateTime? NeedsRedTagPermitAuthorizedOn { get; set; }

        #endregion

        #region Constructors

        public RedTagPermitAuthorizationViewModel(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override ProductionWorkOrder MapToEntity(ProductionWorkOrder entity)
        {
            entity = base.MapToEntity(entity);
            entity.NeedsRedTagPermitAuthorizedOn = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            entity.NeedsRedTagPermitAuthorizedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser?.Employee;
            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext)
                       .Concat(ValidateUserHasEmployeeRecordWhenAuthorizing());
        }
        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidateUserHasEmployeeRecordWhenAuthorizing()
        {
            if (_container.GetInstance<IAuthenticationService<User>>().CurrentUser.Employee != null)
            {
                yield break;
            }

            yield return new ValidationResult(USER_MUST_HAVE_EMPLOYEE_RECORD, new[] { nameof(NeedsRedTagPermitAuthorization) });
        }
        
        #endregion
    }
}