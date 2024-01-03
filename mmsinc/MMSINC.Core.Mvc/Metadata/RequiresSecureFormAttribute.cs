using System;

namespace MMSINC.Metadata
{
    /// <summary>
    /// Overrides the default behavior for handling SecureForms.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class RequiresSecureFormAttribute : Attribute
    {
        #region Properties

        public bool IsRequired { get; set; }

        #endregion

        #region Constructor

        public RequiresSecureFormAttribute(bool isRequired = true)
        {
            IsRequired = isRequired;
        }

        #endregion
    }
}
