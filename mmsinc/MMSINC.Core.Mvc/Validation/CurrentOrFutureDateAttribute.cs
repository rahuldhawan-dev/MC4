using System;

namespace MMSINC.Validation
{
    /// <summary>
    /// Used to set a minimum date in the DateTimeModelValidatorProvider.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CurrentOrFutureDateAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// Set the minimum future date by adding number of days added to current date.
        /// </summary>
        public int AddDays { get; private set; }

        #endregion

        #region Constructors

        public CurrentOrFutureDateAttribute() { }

        public CurrentOrFutureDateAttribute(int addDays)
        {
            AddDays = addDays;
        }

        #endregion
    }
}
