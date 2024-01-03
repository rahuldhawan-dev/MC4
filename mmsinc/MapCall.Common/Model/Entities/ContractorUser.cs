using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Authentication;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ContractorUser : IEntity, IValidatableObject, IRetrievablePasswordUser
    {
        #region Consts

        public const int EMAIL_MAX_LENGTH = 254,
                         PASSWORD = 128, // 128 because it's hashed
                         PASSWORD_ANSWER = 128, // 128 because it's hashed
                         QUESTION_MAX_LENGTH = 256;

        public const string INVALID_EMAIL_ERROR = "The Email field must have a valid email address.";

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        [Required, StringLength(EMAIL_MAX_LENGTH, MinimumLength = 5)]
        public virtual string Email { get; set; }

        public virtual string UniqueName => Email;

        [DataType(System.ComponentModel.DataAnnotations.DataType.Password), Required]
        public virtual string Password { get; set; }

        public virtual Guid PasswordSalt { get; set; }

        [StringLength(QUESTION_MAX_LENGTH, MinimumLength = 5)]
        public virtual string PasswordQuestion { get; set; }

        public virtual string PasswordAnswer { get; set; }
        public virtual bool IsAdmin { get; set; }
        public virtual Contractor Contractor { get; set; }

        public virtual bool IsActive { get; set; }
        public virtual int FailedLoginAttemptCount { get; set; }

        public virtual bool IsLockedOutDueToFailedLoginAttempts => !IsActive && FailedLoginAttemptCount >=
            ContractorUserCredentialPolicy.MAXIMUM_FAILED_LOGIN_ATTEMPT_COUNT;

        public virtual int[] OperatingCenterIds =>
            Contractor.OperatingCenters.Map(oc => oc.Id).ToArray();
        public virtual DateTime? LastLoggedInAt { get; set; }

        #endregion

        #region Logical Properties

        [View(Description = "User has access if they are active and their assigned contractor also has access.")]
        public virtual bool HasAccess => (IsActive && Contractor.ContractorsAccess);

        #endregion

        #region Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
