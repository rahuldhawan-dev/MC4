using System;

namespace MMSINC.Validation
{
    /// <summary>
    /// Indicates that an entity model, or a property on an entity model does not have a string length requirement.
    /// If this is applied to a class, all properties on it are considered to not require a string length attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class StringLengthNotRequiredAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// Gets the reason that string length is not required on a property.
        /// </summary>
        public string Reason { get; private set; }

        #endregion

        #region Constructors

        public StringLengthNotRequiredAttribute() { }

        public StringLengthNotRequiredAttribute(string reason)
        {
            Reason = reason;
        }

        #endregion
    }
}
