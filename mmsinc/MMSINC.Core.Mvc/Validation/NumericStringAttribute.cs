using System.ComponentModel.DataAnnotations;

namespace MMSINC.Validation
{
    /// <summary>
    /// Validation attribute for strings that need to be numerical in nature even
    /// though they might not specifically represent a number. ex: Things that need
    /// padded zeros.
    /// </summary>
    /// /// <remarks>
    /// 
    /// The default DataAnnotationsModelValidatorProviderExtensions.RegisterValidationExtensions()
    /// doesn't know about anything that inherits from RegularExpressionAttribute.  
    /// So to use this, you need to call this in MvcApplication.RegisterValidatorProviders
    /// DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(NumericStringAttribute), typeof(RegularExpressionAttributeAdapter));
    ///
    /// </remarks>
    public class NumericStringAttribute : RegularExpressionAttribute
    {
        public NumericStringAttribute() : base("^[0-9]*$") { }
    }
}
