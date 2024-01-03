using System;
using System.Reflection;
using StructureMap;

namespace MMSINC.Utilities.ObjectMapping
{
    /// <summary>
    /// Base attribute that acts as an adaptor for creating auto mapping property descriptors.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AutoMapAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// Gets/sets the direction this property can map for. 
        /// </summary>
        public MapDirections Direction { get; set; }

        /// <summary>
        /// Gets/sets the name of the property on the secondary type
        /// that this property should map to. If this is null, the
        /// name of the primary property is used instead.
        /// </summary>
        public string SecondaryPropertyName { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new AutoMapAttribute for the given map direction. Defaults to BothWays if not set.
        /// </summary>
        /// <param name="direction"></param>
        public AutoMapAttribute(MapDirections direction = MapDirections.BothWays) : this(null, direction) { }

        /// <summary>
        /// Creates a new AutoMapAttribute for a specific secondary property name and optional map direction.
        /// </summary>
        public AutoMapAttribute(string secondaryPropertyName, MapDirections direction = MapDirections.BothWays)
        {
            SecondaryPropertyName = secondaryPropertyName;
            Direction = direction;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new propert descriptor for a given property based on this attribute's values.
        /// </summary>
        public virtual ObjectPropertyDescriptor CreatePropertyDescriptor(IContainer container,
            PropertyInfo primaryPropertyInfo, Type secondaryType)
        {
            return new AutoPropertyDescriptor(primaryPropertyInfo, secondaryType, SecondaryPropertyName, Direction);
        }

        #endregion
    }
}
