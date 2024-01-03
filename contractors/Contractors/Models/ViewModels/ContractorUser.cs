using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Contractors.Data.Library;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace Contractors.Models.ViewModels
{
    // This doesn't need to be a VieWModel because it will never
    // be used to merge info into a ContractorUser object.
    public class ContractorUserLogOn : ViewModel, IValidatableObject
    {
        #region Fields

        private readonly IAuthenticationRepository<ContractorUser> _authRepo;

        #endregion

        #region Properties

        [Required, StringLength(ContractorUser.EMAIL_MAX_LENGTH, MinimumLength = 5),
         MMSINC.Validation.EmailAddress(ErrorMessage = ContractorUser.INVALID_EMAIL_ERROR)]
        public virtual string Email { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Password), Required]
        public virtual string Password { get; set; }

        public string ReturnUrl { get; set; }

        /// <summary>
        /// Returns the UserLoginAttemptStatus from validation. If this is null it's because
        /// the Validate method was not called.
        /// </summary>
        public UserLoginAttemptStatus? LoginAttemptResult { get; internal set; }

        #endregion

        public ContractorUserLogOn(
            IAuthenticationRepository<ContractorUser> authRepo)
        {
            _authRepo = authRepo;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            LoginAttemptResult = _authRepo.ValidateUser(Email, Password);

            switch (LoginAttemptResult)
            {
                case UserLoginAttemptStatus.Success:
                case UserLoginAttemptStatus.SuccessRequiresPasswordChange:
                    // Nothing to return for a successful result.
                    break;

                case UserLoginAttemptStatus.InvalidEmail:
                    // Leaving this here as extra security, but it's never going to get hit. The Regex validator
                    // on the Email field will fail and then this validate method will not be called.
                    yield return new ValidationResult("You must enter a valid email address.", new[] {nameof(Email)});
                    break;

                case UserLoginAttemptStatus.UnknownUser:
                case UserLoginAttemptStatus.BadPassword:
                    yield return new ValidationResult("User does not exist or password is incorrect.", new[] { "GenericLoginError" });
                    break;

                case UserLoginAttemptStatus.AccessDisabled:
                    yield return new ValidationResult("Access is not enabled.", new[] { "Access" });
                    break;

                default:
                    throw new NotSupportedException();
            }

        }
    }

    public class ForgotPasswordIndexContractorUser : ViewModel, IValidatableObject
    {
        #region Fields

        private readonly IForgotPasswordRepository<ContractorUser>
            _forgotPasswordRepo;

        #endregion

        #region Properties

        [Required, StringLength(ContractorUser.EMAIL_MAX_LENGTH, MinimumLength = 5),
         MMSINC.Validation.EmailAddress(ErrorMessage = ContractorUser.INVALID_EMAIL_ERROR)]
        public string Email { get; set; }

        #endregion

        public ForgotPasswordIndexContractorUser(
            IForgotPasswordRepository<ContractorUser> forgotPasswordRepo)
        {
            _forgotPasswordRepo = forgotPasswordRepo;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (_forgotPasswordRepo.GetUser(Email) == null)
            {
                yield return new ValidationResult("User does not exist.", new [] { nameof(Email) });
            }
        }
    }

    public class ForgotPasswordVerifyContractorUser : ViewModel<ContractorUser>
    {
        #region Constants

        public const string INCORRECT_PASSWORD_ANSWER =
            "That answer is incorrect.";

        #endregion

        #region Properties

        [Required, StringLength(ContractorUser.EMAIL_MAX_LENGTH, MinimumLength = 5),
         MMSINC.Validation.EmailAddress(ErrorMessage = ContractorUser.INVALID_EMAIL_ERROR)]
        public string Email { get; set; }

        [StringLengthNotRequired("Display field")]
        public string PasswordQuestion { get; set; }

        [Required, StringLengthNotRequired("PasswordAnswer is hashed to a guid")]
        public string PasswordAnswer { get; set; }

        [ContractorUserPasswordValidation]
        [Required, DataType(System.ComponentModel.DataAnnotations.DataType.Password), System.ComponentModel.DataAnnotations.Compare("ConfirmPassword")]
        public string Password { get; set; }

        [DoesNotAutoMap]
        [Required, DataType(System.ComponentModel.DataAnnotations.DataType.Password), System.ComponentModel.DataAnnotations.Compare("Password")]
        public string ConfirmPassword { get; set; }

        #endregion

        #region Constructor

        // Required by MVC.
        public ForgotPasswordVerifyContractorUser(IContainer container) : base(container) { }

        #endregion

        #region Methods

        public override void Map(ContractorUser entity)
        {
            base.Map(entity);
            // We need to null these out since the auto mapper will set them to the hashed values.
            Password = null;
            PasswordAnswer = null;
        }

        public override ContractorUser MapToEntity(ContractorUser entity)
        {
            AuthenticationService.ThrowIfInvalidEmail(Email);
            Password.ThrowIfNullOrWhiteSpace("Password");
            entity.Password = Password.Salt(entity.PasswordSalt);
            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var repo = _container.GetInstance<IForgotPasswordRepository<ContractorUser>>();

            if (repo.GetUser(Email) == null)
            {
                yield return new ValidationResult("User does not exist.", new [] { nameof(Email) });
            }
            else if (!repo.ValidatePasswordQuestionAnswer(Email, PasswordAnswer))
            {
                yield return new ValidationResult(INCORRECT_PASSWORD_ANSWER);
            }
        }

        #endregion
    }

    public class ChangePasswordContractorUser : ViewModel<ContractorUser>
    {
        #region Properties

        [AutoMap(MapDirections.None)]
        public override int Id
        {
            get
            {
                // This override is necessary because the Id is not passed back for this particular model, 
                // but we need the Id value to work with ActionHelper.
                return _container.GetInstance<IAuthenticationService<ContractorUser>>().CurrentUser.Id;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        // The email is needed for display purposes and validation, but it is not
        // editable by users.
        [StringLengthNotRequired]
        [AutoMap(MapDirections.None)] 
        public string Email { get { return _container.GetInstance<IAuthenticationService<ContractorUser>>().CurrentUser.Email; } }

        [DoesNotAutoMap]
        [Required, DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string CurrentPassword { get; set; }

        [DoesNotAutoMap]
        [ContractorUserPasswordValidation]
        [Required, DataType(System.ComponentModel.DataAnnotations.DataType.Password), Compare("ConfirmNewPassword")]
        public string NewPassword { get; set; }

        [DoesNotAutoMap]
        [Required, DataType(System.ComponentModel.DataAnnotations.DataType.Password), Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }

        #endregion

        #region Constructors

        public ChangePasswordContractorUser(IContainer container) : base(container) { }

        #endregion

        #region Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = _container
                .GetInstance<IAuthenticationService<ContractorUser>>()
                .ValidateUser(Email, CurrentPassword);
            switch (result)
            {
                // Handle both success as valid because we're validating their current password is correct, regardless
                // of password requirements.
                case UserLoginAttemptStatus.Success:
                case UserLoginAttemptStatus.SuccessRequiresPasswordChange: 
                    break;
                case UserLoginAttemptStatus.BadPassword:
                    yield return new ValidationResult("Incorrect password.");
                    yield break;
                default:
                    throw new NotSupportedException(
                        String.Format(
                            "Not sure how to handle UserLoginAttemptStatus.{0}.",
                            result));
            }
        }

        public override ContractorUser MapToEntity(ContractorUser entity)
        {
            // Do not use base.MapToEntity here as the only change being made is 
            // the salted password.
            entity.Password = NewPassword.Salt(entity.PasswordSalt);
            return entity;
        }

        #endregion
    }

    public class ChangePasswordQuestionAndAnswerContractorUser : ViewModel<ContractorUser>
    {
        #region Properties

        [AutoMap(MapDirections.None)]
        public override int Id
        {
            get
            {
                // This override is necessary because the Id is not passed back for this particular model, 
                // but we need the Id value to work with /ActionHelper.
                return _container.GetInstance<IAuthenticationService<ContractorUser>>().CurrentUser.Id;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        // The email is needed for display purposes and validation, but it is not
        // editable by users.
        [StringLengthNotRequired]
        [AutoMap(MapDirections.None)]
        public string Email { get { return _container.GetInstance<IAuthenticationService<ContractorUser>>().CurrentUser.Email; } }

        [Required, DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Password { get; set; }

        [Required, View("Question"), StringLength(ContractorUser.QUESTION_MAX_LENGTH, MinimumLength = 5)]
        public string PasswordQuestion { get; set; }

        [Required, View("Answer"), StringLengthNotRequired("PasswordAnswer is hashed to a guid")]
        public string PasswordAnswer { get; set; }

        #endregion

        #region Constructor

        public ChangePasswordQuestionAndAnswerContractorUser(IContainer container) : base(container) { }

        #endregion

        #region Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var ret = new List<ValidationResult>();
            var status = _container
                .GetInstance<IAuthenticationService<ContractorUser>>()
                .ValidateUser(Email, Password);

            switch (status)
            {
                case UserLoginAttemptStatus.Success:
                    break;
                case UserLoginAttemptStatus.BadPassword:
                    ret.Add(
                        new ValidationResult("Incorrect password."));
                    break;
                default:
                    throw new NotSupportedException(
                        String.Format(
                            "UserLoginAttemptStatus.{0} is not supported.",
                            status));
            }

            return ret;
        }

        public override void Map(ContractorUser entity)
        {
            base.Map(entity);

            // These need to be null since we don't want display
            // the hashed values.
            PasswordAnswer = null;
            Password = null;
        }

        public override ContractorUser MapToEntity(ContractorUser entity)
        {
            entity.PasswordQuestion = PasswordQuestion;
            entity.PasswordAnswer =
                PasswordAnswer.SanitizeAndDowncase().Salt(entity.PasswordSalt);
            return entity;
        }

        #endregion
    }

}