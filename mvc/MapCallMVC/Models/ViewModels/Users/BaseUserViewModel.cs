using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels.Users
{
    public abstract class BaseUserViewModel : ViewModel<User>
    {
        #region Properties

        [Required]
        [DropDown, EntityMap, EntityMustExist(typeof(UserType))]
        public int? UserType { get; set; }

        [Required]
        [MMSINC.Validation.EmailAddress, StringLength(User.StringLengths.EMAIL)]
        public string Email { get; set; }

        [Required]
        public bool? HasAccess { get; set; }

        // This was not required previously, but this value is used for display
        // in many areas.
        [Required] 
        [StringLength(User.StringLengths.MAX_FULL_NAME)]
        public string FullName { get; set; }

        // NOTE: This is not required. Not all users are employees! Some are automation accounts.
        // and some are linked to records that existed well before we ever linked users to employees.
        [DoesNotAutoMap]
        [View("Employee ID")]
        [StringLength(User.StringLengths.MAX_EMPLOYEE_ID)]
        public string EmployeeNumber { get; set; }

        [Required]
        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? DefaultOperatingCenter { get; set; }

        [Required]
        public bool? IsUserAdmin { get; set; }

        [StringLength(User.StringLengths.ADDRESS)]
        public string Address { get; set; }

        [StringLength(User.StringLengths.CITY)]
        public string City { get; set; }

        [StringLength(User.StringLengths.STATE)]
        public string State { get; set; }

        [StringLength(User.StringLengths.ZIP_CODE)]
        public string ZipCode { get; set; }

        [StringLength(User.StringLengths.ALL_PHONE_NUMBERS)]
        public string PhoneNumber { get; set; }

        [StringLength(User.StringLengths.ALL_PHONE_NUMBERS)]
        public string CellPhoneNumber { get; set; }

        [StringLength(User.StringLengths.ALL_PHONE_NUMBERS)]
        public string FaxNumber { get; set; }

        #endregion

        #region Constructor

        public BaseUserViewModel(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override User MapToEntity(User entity)
        {
            base.MapToEntity(entity);
            
            // TODO: Storing the LastName seems pretty useless. We could have a specific
            // search query for this instead. LastName isn't actually used by anything other
            // than search.
            if (!string.IsNullOrWhiteSpace(entity.FullName))
            {
                entity.LastName = entity.FullName.Split(' ').Last();
            }
            else
            {
                entity.LastName = null;
            }

            if (EmployeeNumber != null)
            {
                entity.Employee = _container.GetInstance<IRepository<Employee>>().GetByEmployeeId(EmployeeNumber.Trim());
            }
            else
            {
                entity.Employee = null; 
            }

            return entity;
        }

        private IEnumerable<ValidationResult> ValidateEmployeeNumber()
        {
            // EmployeeNumber is not required.
            if (string.IsNullOrWhiteSpace(EmployeeNumber))
            {
                yield break; 
            }

            var employee = _container.GetInstance<IRepository<Employee>>().GetByEmployeeId(EmployeeNumber.Trim());
            if (employee == null)
            {
                yield return new ValidationResult("Employee number must match an existing Employee record.",
                    new[] { nameof(EmployeeNumber) });
            }
            else if (employee.User != null && employee.User.Id != Id)
            {
                yield return new ValidationResult($"The user '{employee.User.UserName}' is already linked to this employee.",
                    new[] { nameof(EmployeeNumber) });
            }
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateEmployeeNumber());
        }

        #endregion
    }
}