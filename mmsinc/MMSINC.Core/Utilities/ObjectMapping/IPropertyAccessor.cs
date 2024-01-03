using System;

namespace MMSINC.Utilities.ObjectMapping
{
    /// <summary>
    /// Represents an object that can access a single property on another object.
    /// </summary>
    public interface IPropertyAccessor
    {
        /// <summary>
        /// Gets the type this property returns and requires for being set.
        /// </summary>
        Type PropertyType { get; }

        /// <summary>
        /// Gets whether this property accessor is able to set property values.
        /// </summary>
        bool IsSettable { get; }

        /// <summary>
        /// Returns the property value on an object.
        /// </summary>
        object GetValue(object propertyOwner);

        /// <summary>
        /// Sets the property value on an object.
        /// </summary>
        void SetValue(object propertyOwner, object value);

        /// <summary>
        /// Returns an identifier that uniquely represents the setter
        /// used by a property accessor. This is for checking that two
        /// property accessors are not trying to set the same exact property.
        /// </summary>
        /// <returns></returns>
        int GetUniqueSetterIdentifier();
    }
}
