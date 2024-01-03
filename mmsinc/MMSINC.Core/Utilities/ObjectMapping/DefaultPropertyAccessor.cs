using System;
using System.Reflection;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MMSINC.Utilities.ObjectMapping
{
    // TODO: This needs some refactoring if anyone needs to inherit it.
    public class DefaultPropertyAccessor : IPropertyAccessor
    {
        #region Fields

        private readonly MethodInfo _getMethod,
                                    _setMethod;

        #endregion

        #region Properties

        public Type PropertyType { get; protected set; }
        public bool IsSettable { get; private set; }

        #endregion

        #region Constructor

        public DefaultPropertyAccessor(PropertyInfo prop)
        {
            prop.ThrowIfNull("prop");
            PropertyType = prop.PropertyType;
            _getMethod = prop.GetGetMethod();
            _setMethod = prop.GetSetMethod();
            IsSettable = (_setMethod != null);
        }

        #endregion

        #region Public Methods

        public object GetValue(object propertyOwner)
        {
            return _getMethod.Invoke(propertyOwner, null);
        }

        public void SetValue(object propertyOwner, object value)
        {
            _setMethod.Invoke(propertyOwner, new[] {value});
        }

        public int GetUniqueSetterIdentifier()
        {
            if (_setMethod != null)
            {
                return _setMethod.GetHashCode();
            }

            return GetHashCode();
        }

        #endregion
    }
}
