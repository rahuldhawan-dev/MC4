using System.ComponentModel.DataAnnotations;
using MapCall.Common.Authentication;

namespace MapCall.Common.Metadata
{
    public class ContractorUserPasswordValidationAttribute : ValidationAttribute
    {
        #region Fields

        // This can be static readonly because ContractorUserCredentialPolicy instances are thread safe.
        private static readonly ContractorUserCredentialPolicy _credentialPolicy = new ContractorUserCredentialPolicy();

        #endregion

        #region Private Methods

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // NOTE: Use a RequiredAttribute on the property if null values aren't allowed. Password fields are not
            //       always required when this validator is used. 
            if (value != null && !_credentialPolicy.PasswordMeetsRequirement((string)value))
            {
                return new ValidationResult("Password does not meet security requirements.",
                    new[] {validationContext.MemberName});
            }

            return ValidationResult.Success;
        }

        #endregion
    }
}
