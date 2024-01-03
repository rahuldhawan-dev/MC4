using System;
using System.Collections.Generic;
using System.Linq;

namespace MMSINC.Utilities.ObjectMapping
{
    /// <summary>
    /// An IPropertyAccessor that can get/set properties That.Are.Deeply.Nested.
    /// </summary>
    public class DeepPropertyAccessor : IPropertyAccessor
    {
        #region Fields

        private readonly List<IPropertyAccessor> _orderedPropertyAccessors;
        private readonly IPropertyAccessor _lastPropertyAccessor;

        #endregion

        #region Properties

        public Type PropertyType { get; private set; }
        public bool IsSettable { get; private set; }

        #endregion

        #region Constructor

        public DeepPropertyAccessor(Type classType, string propertyPathFromClass)
        {
            _orderedPropertyAccessors = GetOrderedPropertyAccessors(classType, propertyPathFromClass);
            _lastPropertyAccessor = _orderedPropertyAccessors.Last();
            PropertyType = _lastPropertyAccessor.PropertyType;
            IsSettable = _lastPropertyAccessor.IsSettable;
        }

        #endregion

        #region Private Methods

        private static List<IPropertyAccessor> GetOrderedPropertyAccessors(Type rootType,
            string propertyPathFromRoot)
        {
            // TODO: Probably move this to ObjectExtensions or PropertyInfoExtensions or something.

            var orderedProps = new List<IPropertyAccessor>();
            var propNames = propertyPathFromRoot.Split('.');

            var curPropInfo = rootType.GetProperty(propNames.First());
            orderedProps.Add(new DefaultPropertyAccessor(curPropInfo));

            foreach (var pName in propNames.Skip(1))
            {
                var nextProp = curPropInfo.PropertyType.GetProperty(pName);
                if (nextProp != null)
                {
                    orderedProps.Add(new DefaultPropertyAccessor(nextProp));
                    curPropInfo = nextProp;
                }
                else
                {
                    throw ExceptionHelper.Format<InvalidOperationException>(
                        "Unable to find property '{0}' on type '{1}'. Full property path: {2}", pName,
                        curPropInfo.PropertyType.FullName, propertyPathFromRoot);
                }
            }

            return orderedProps;
        }

        #endregion

        public object GetValue(object propertyOwner)
        {
            var curVal = propertyOwner;
            foreach (var accessor in _orderedPropertyAccessors)
            {
                curVal = accessor.GetValue(curVal);
                if (curVal == null)
                {
                    return null;
                }
            }

            return curVal;
        }

        public void SetValue(object propertyOwner, object value)
        {
            foreach (var accessor in _orderedPropertyAccessors)
            {
                if (accessor != _lastPropertyAccessor)
                {
                    propertyOwner = accessor.GetValue(propertyOwner);
                }
            }

            _lastPropertyAccessor.SetValue(propertyOwner, value);
        }

        public int GetUniqueSetterIdentifier()
        {
            return _orderedPropertyAccessors.Last().GetUniqueSetterIdentifier();
        }
    }
}
