using System;
using System.Diagnostics;

namespace MMSINC.Utilities.ObjectMapping
{
    [DebuggerDisplay("{Name}")]
    public abstract class ObjectPropertyDescriptor
    {
        #region Properties

        /// <summary>
        /// Gets the name of the property this descriptor is describing. 
        /// Value's only used for throwing descriptive exceptions.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the property accessor responsible for getting/setting values on the primary object.
        /// </summary>
        public virtual IPropertyAccessor PrimaryAccessor { get; protected set; }

        /// <summary>
        /// Gets the property accessor responsible for getting/setting values on the secondary object.
        /// </summary>
        public virtual IPropertyAccessor SecondaryAccessor { get; protected set; }

        /// <summary>
        /// Returns true if this descriptor is capable of mapping to the primary object. If false,
        /// MapToPrimary will not do anything.
        /// </summary>
        public abstract bool CanMapToPrimary { get; }

        /// <summary>
        /// Returns true if this descriptor is capable of mapping to the primary object. If false,
        /// MapToSecondary will not do anything.
        /// </summary>
        public abstract bool CanMapToSecondary { get; }

        /// <summary>
        /// Gets the value converter used to convert values between primary and secondary objects during mapping.
        /// </summary>
        public abstract IValueConverter ValueConverter { get; }

        #endregion

        #region Private Methods

        /// <summary>
        /// Maps a secondary object's value to the primary object.
        /// This method is only called when CanMapToPrimary is true.     
        /// NOTE: Do not call this method directly. Call the MapToPrimary
        /// method which does proper state checking before calling this method.
        /// </summary>
        internal protected virtual void MapToPrimaryCore(object primaryObject, object secondaryObject)
        {
            var secondaryValue = SecondaryAccessor.GetValue(secondaryObject);
            var convertedValue = ValueConverter.ToPrimary(secondaryValue);
            PrimaryAccessor.SetValue(primaryObject, convertedValue);
        }

        /// <summary>
        /// Maps a primary object's value to the secondary object.
        /// This method is only called when CanMapToPrimary is true.
        /// NOTE: Do not call this method directly. Call the MapToSecondary
        /// method which does proper state checking before calling this method.
        /// </summary>
        internal protected virtual void MapToSecondaryCore(object primaryObject, object secondaryObject)
        {
            var primaryValue = PrimaryAccessor.GetValue(primaryObject);
            var convertedValue = ValueConverter.ToSecondary(primaryValue);
            SecondaryAccessor.SetValue(secondaryObject, convertedValue);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Maps a secondary object's value to the primary object.
        /// This is marked virtual for testing, not recommended to override.
        /// </summary>
        public void MapToPrimary(object primaryObject, object secondaryObject)
        {
            if (CanMapToPrimary)
            {
                try
                {
                    MapToPrimaryCore(primaryObject, secondaryObject);
                }
                catch (Exception ex)
                {
                    throw ObjectMapperException.UnableToMapProperty(Name, ex);
                }
            }
        }

        /// <summary>
        /// Maps a primary object's value to the secondary object.
        /// This is marked virtual for testing, not recommended to override.
        /// </summary>
        public void MapToSecondary(object primaryObject, object secondaryObject)
        {
            if (CanMapToSecondary)
            {
                try
                {
                    MapToSecondaryCore(primaryObject, secondaryObject);
                }
                catch (Exception ex)
                {
                    throw ObjectMapperException.UnableToMapProperty(Name, ex);
                }
            }
        }

        #endregion
    }
}
