using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Contractors.Models.ViewModels
{
    public abstract class ContractorUserViewModel : ViewModel<ContractorUser>
    {
        #region Properties

        [Required, DropDown, EntityMap, EntityMustExist(typeof(Contractor))]
        public int? Contractor { get; set; }

        [Required]
        public bool? IsAdmin { get; set; }

        [Required]
        public bool? IsActive { get; set; }

        [Required, StringLength(ContractorUser.QUESTION_MAX_LENGTH, MinimumLength = 5)]
        public string PasswordQuestion { get; set; }

        [AutoMap(MapDirections.None)] // Should not be displayed, also needs to be hashed so automapping won't work.
        public virtual string PasswordAnswer { get; set; }

        [ContractorUserPasswordValidation]
        [DoesNotAutoMap("Should not be displayed, also needs to be hashed so automapping won't work.")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password), Compare("ConfirmPassword")]
        public virtual string Password { get; set; }

        [DoesNotAutoMap("Display only")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password), Compare("Password")]
        public virtual string ConfirmPassword { get; set; }

        #endregion

        #region Constructors

        protected ContractorUserViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateContractorUser : ContractorUserViewModel
    {
        #region Properties

        // They weren't able to edit the email address on the contractors portal, so not implementing that here for right now.
        [AutoMap(MapDirections.None)] // Needs extra value modification during MapToEntity.
        [Required, MMSINC.Validation.EmailAddress, StringLength(ContractorUser.EMAIL_MAX_LENGTH, MinimumLength = 5)]
        public string Email { get; set; }

        [Required]
        public override string PasswordAnswer { get; set; }

        [Required]
        public override string Password { get; set; }

        [Required]
        public override string ConfirmPassword { get; set; }

        #endregion

        #region Constructor

        public CreateContractorUser(IContainer container) : base(container) { }

        #endregion

        #region Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>(base.Validate(validationContext));

            if (_container.GetInstance<IContractorUserRepository>().TryGetUserByEmail(Email) != null)
            {
                results.Add(
                    new ValidationResult(
                        String.Format("User with email '{0}' already exists.",
                            Email)));
            }

            return results;
        }

        public override ContractorUser MapToEntity(ContractorUser entity)
        {
            base.MapToEntity(entity);
            entity.Email = Email.SanitizeAndDowncase();
            entity.PasswordSalt = Guid.NewGuid();
            entity.Password = Password.Salt(entity.PasswordSalt);
            entity.PasswordAnswer = PasswordAnswer.SanitizeAndDowncase().Salt(entity.PasswordSalt);
            return entity;
        }

        #endregion
    }

    public class EditContractorUser : ContractorUserViewModel
    {
        #region Properties

        [View(Description = "Leave blank if the answer is not being changed.")]
        public override string PasswordAnswer { get; set; }

        [View(Description = "Leave blank if the password is not being changed.")]
        [RequiredWhen(nameof(ConfirmPassword), ComparisonType.NotEqualTo, null)]
        public override string Password { get; set; }

        [View(Description = "Leave blank if the password is not being changed.")]
        [RequiredWhen(nameof(Password), ComparisonType.NotEqualTo, null)]
        public override string ConfirmPassword { get; set; }

        [AutoMap(MapDirections.ToViewModel)] // Display only
        public string Email { get; set; }

        #endregion

        #region Constructor

        public EditContractorUser(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override ContractorUser MapToEntity(ContractorUser entity)
        {
            base.MapToEntity(entity);

            // Do not replace the password salt here as we can't re-salt the existing passwords/answers if
            // they aren't both being changed.

            if (!string.IsNullOrWhiteSpace(Password))
            {
                entity.Password = Password.Salt(entity.PasswordSalt);
            }
            if (!string.IsNullOrWhiteSpace(PasswordAnswer))
            {
                entity.PasswordAnswer = PasswordAnswer.SanitizeAndDowncase().Salt(entity.PasswordSalt);
            }

            if (entity.IsActive)
            {
                // Reset this so user can login again if their account had been locked out.
                entity.FailedLoginAttemptCount = 0;
            }

            return entity;
        }

        #endregion
    }

    public class SearchContractorUser : SearchSet<ContractorUser>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(Contractor))]
        public int? Contractor { get; set; }
        public string Email { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? IsActive { get; set; }

        #endregion
    }
}