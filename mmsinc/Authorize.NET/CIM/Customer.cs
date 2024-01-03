using System;
using System.Collections.Generic;

// ReSharper disable CheckNamespace
namespace AuthorizeNet
{
// ReSharper restore CheckNamespace

    public enum ValidationMode {
        None,
        TestMode,
        LiveMode,
    }

    /// <summary>
    /// This is an abstraction for use with the CIM API. It's a partial class so you can combine it with your class as needed.
    /// </summary>
    public partial class Customer {
        public Customer() {
            //default it to something
            ID = Guid.NewGuid().ToString();

            ShippingAddresses = new List<Address>();
            PaymentProfiles = new List<PaymentProfile>();
        }

        public virtual string ID { get; set; }
        public virtual string ProfileID { get; set; }
        public virtual string Description { get; set; }
        public virtual string Email { get; set; }
        public virtual Address BillingAddress { get; set; }
        public virtual IList<Address> ShippingAddresses { get; set; }
        public virtual IList<PaymentProfile> PaymentProfiles { get; set; }

        internal static APICore.validationModeEnum ToValidationMode(ValidationMode mode) {
            switch (mode) {
                case ValidationMode.None: return APICore.validationModeEnum.none;
                case ValidationMode.TestMode: return APICore.validationModeEnum.testMode;
                case ValidationMode.LiveMode: return APICore.validationModeEnum.liveMode;
                default: return (APICore.validationModeEnum)mode;
            }
        }
    }
}
