using System;
using System.Collections.Generic;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MMSINC.Utilities.ObjectMapping
{
    /// <summary>
    /// Describes a class that can map from one type(the primary) to another type(the secondary) and back again.
    /// </summary>
    public interface IObjectMapper
    {
        void MapToPrimary(object primaryObject, object secondaryObject);
        void MapToSecondary(object primaryObject, object secondaryObject);
    }

    public abstract class ObjectMapperBase : IObjectMapper
    {
        #region Private Methods

        /// <summary>
        /// Method called retrieve a collection of ObjectPropertyDescriptors used by 
        /// this mapper instance. This isn't cached, so inheritors might wanna do that.
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<ObjectPropertyDescriptor> GetPropertyDescriptors();

        private void Map(object primaryObject, object secondaryObject,
            Action<ObjectPropertyDescriptor, object, object> mapAction)
        {
            primaryObject.ThrowIfNull("primaryObject");
            secondaryObject.ThrowIfNull("secondaryObject");
            var descriptors = GetPropertyDescriptors();
            foreach (var descriptor in descriptors)
            {
                try
                {
                    mapAction(descriptor, primaryObject, secondaryObject);
                }
                catch (Exception ex)
                {
                    throw ObjectMapperException.UnableToMapProperty(descriptor.Name, ex);
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Maps values from the secondary object to the primary object
        /// </summary>
        public void MapToPrimary(object primaryObject, object secondaryObject)
        {
            Map(primaryObject, secondaryObject, (desc, o1, o2) => desc.MapToPrimary(o1, o2));
        }

        /// <summary>
        /// Maps values from the primary object to the secondary object
        /// </summary>
        public void MapToSecondary(object primaryObject, object secondaryObject)
        {
            Map(primaryObject, secondaryObject, (desc, o1, o2) => desc.MapToSecondary(o1, o2));
        }

        #endregion
    }
}
