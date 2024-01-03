using System;

namespace MMSINC.Utilities.ObjectMapping
{
    /// <summary>
    /// Use this attribute to indicate a property is not to be automatically mapped in either direction.
    /// This property may still be mapped manually in either direction.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DoesNotAutoMapAttribute : AutoMapAttribute
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reason">The reason this property does not auto map.</param>
        public DoesNotAutoMapAttribute(string reason) : this()
        {
            // The string param isn't currently set anywhere because it's more for forced documentation
            // than it is for actually looking at the value at runtime.
        }

        /// <summary>
        /// It is HIGHLY RECOMMENDED that you use the constructor that accepts a reason. Documentation is a good thing!
        /// </summary>
        public DoesNotAutoMapAttribute()
        {
            Direction = MapDirections.None;
        }

        #endregion
    }
}
