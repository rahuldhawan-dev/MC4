using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.ClassExtensions.TypeExtensions;

namespace MMSINC.Utilities.ObjectMapping
{
    /// <summary>
    /// Default implementation of property descriptor used by AutoObjectMapper.
    /// </summary>
    public class AutoPropertyDescriptor : ObjectPropertyDescriptor
    {
        #region Fields

        private IValueConverter _valueConverter;

        // DO NOT call these fields directly. Call their property accessors.
        private Lazy<bool> _canConvertToPrimary,
                           _canConvertToSecondary;

        private readonly string _primaryPropertyName;

        // This field exists for testing purposes.
        private readonly Type _secondaryType;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the direction this property supports during mapping.
        /// </summary>
        public MapDirections Direction { get; protected set; }

        // These two props most likely don't need to be public.
        // protected PropertyInfo PrimaryProperty { get; private set; }
        // protected PropertyInfo SecondaryProperty { get; private set; }

        /// <summary>
        /// Gets/sets the name of the property on the secondary type that
        /// the primary property should map to. If this value is null, then
        /// the primary property's name is used.
        /// </summary>
        public string SecondaryPropertyName { get; private set; }

        public override string Name
        {
            get { return _primaryPropertyName; }
        }

        public override bool CanMapToPrimary
        {
            get
            {
                // True if direction matches
                //      and primary has setter
                //      and secondary has getter
                return (CanMapToDirection(MapDirections.ToPrimary) && _canConvertToPrimary.Value);
            }
        }

        public override bool CanMapToSecondary
        {
            get { return (CanMapToDirection(MapDirections.ToSecondary) && _canConvertToSecondary.Value); }
        }

        public override IValueConverter ValueConverter
        {
            get
            {
                // This is lazy-created because the CanMap properties
                // rely on the converter being initialized. Inheritors
                // might wanna change the default value converter.
                if (_valueConverter == null)
                {
                    _valueConverter = CreateValueConverter();
                }

                return _valueConverter;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new AutoPropertyDescriptor with the direction set to BothWays if the parameter isn't specified.
        /// </summary>
        public AutoPropertyDescriptor(PropertyInfo primaryProp, Type secondaryType,
            MapDirections direction = MapDirections.BothWays)
            : this(primaryProp, secondaryType, null, direction) { }

        /// <summary>
        /// Creates a new AutoPropertyDescriptor with a specific property name to be mapped on the secondary type.
        /// </summary>
        /// <param name="primaryProp"></param>
        /// <param name="secondaryType"></param>
        /// <param name="secondaryPropertyName">If null, the primary property's name is used instead.</param>
        /// <param name="direction"></param>
        public AutoPropertyDescriptor(PropertyInfo primaryProp, Type secondaryType, string secondaryPropertyName,
            MapDirections direction = MapDirections.BothWays)
        {
            primaryProp.ThrowIfNull("primaryProp");
            secondaryType.ThrowIfNull("secondaryType");
            _primaryPropertyName = primaryProp.Name;
            _secondaryType = secondaryType;

            SecondaryPropertyName = secondaryPropertyName ?? _primaryPropertyName;

            Direction = direction;
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            PrimaryAccessor = new DefaultPropertyAccessor(primaryProp);
            SecondaryAccessor = TryCreateSecondaryPropertyAccessor(secondaryType);
            // ReSharper restore DoNotCallOverridableMethodsInConstructor

            // TODO: See about moving most of this check to the base descriptor class.
            _canConvertToPrimary =
                new Lazy<bool>(GetCanConvertToPrimary);

            _canConvertToSecondary =
                new Lazy<bool>(GetCanConvertToSecondary);
        }

        private bool GetCanConvertToPrimary()
        {
            return SecondaryAccessor != null && PrimaryAccessor.IsSettable &&
                   ValueConverter.CanConvert(PrimaryAccessor.PropertyType, SecondaryAccessor.PropertyType);
        }

        private bool GetCanConvertToSecondary()
        {
            return SecondaryAccessor != null && SecondaryAccessor.IsSettable &&
                   ValueConverter.CanConvert(SecondaryAccessor.PropertyType, PrimaryAccessor.PropertyType);
        }

        #endregion

        #region Private Methods

        protected virtual IValueConverter CreateValueConverter()
        {
            return new DefaultValueConverter();
        }

        private bool CanMapToDirection(MapDirections dir)
        {
            return Direction == MapDirections.BothWays || Direction == dir;
        }

        private IPropertyAccessor TryCreateSecondaryPropertyAccessor(Type secondaryType)
        {
            if (SecondaryPropertyName.Contains("."))
            {
                return new DeepPropertyAccessor(secondaryType, SecondaryPropertyName);
            }

            return TryCreateSimpleSecondaryPropertyAccessor(secondaryType);
        }

        private IPropertyAccessor TryCreateSimpleSecondaryPropertyAccessor(Type secondaryType)
        {
            var secondaryProp = !secondaryType.IsInterface
                ? secondaryType.GetProperty(SecondaryPropertyName)
                : secondaryType.GetPropertyFromInterface(SecondaryPropertyName);

            return secondaryProp == null ? null : new DefaultPropertyAccessor(secondaryProp);
        }

        #endregion
    }
}
