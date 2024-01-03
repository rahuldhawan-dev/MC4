using Contractors.App_Start;
using DataAnnotationsExtensions.ClientValidation;
using WebActivator;

[assembly: PreApplicationStartMethod(typeof(RegisterClientValidationExtensions), "Start")]
 
namespace Contractors.App_Start {
    public static class RegisterClientValidationExtensions {
        public static void Start() {
            DataAnnotationsModelValidatorProviderExtensions.RegisterValidationExtensions();            
        }
    }
}