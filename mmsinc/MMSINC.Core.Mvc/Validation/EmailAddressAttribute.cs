using System.ComponentModel.DataAnnotations;
using MMSINC.ClassExtensions.StringExtensions;

namespace MMSINC.Validation
{
    /// <summary>
    /// Validates that a value is a validly formatted email address.
    /// </summary>
    /// <remarks>
    /// 
    /// The default DataAnnotationsModelValidatorProviderExtensions.RegisterValidationExtensions()
    /// doesn't know about anything that inherits from RegularExpressionAttribute.  
    /// So to use this, you need to call this in MvcApplication.RegisterValidatorProviders
    /// DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(EmailAddressAttribute), typeof(RegularExpressionAttributeAdapter));
    ///
    /// </remarks>
    public class EmailAddressAttribute : RegularExpressionAttribute
    {
        public EmailAddressAttribute() : base(StringExtensions.EMAIL_REGEX)
        {
            ErrorMessage = "Invalid email address.";
        }
    }
}
