using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Automapping;
using MMSINC.ClassExtensions.ObjectExtensions;
using StructureMap;

namespace MMSINC.Utilities.ObjectMapping
{
    /// <summary>
    /// Implementation of ObjectMapper that maps things... automatically. 
    /// </summary>
    public class AutoObjectMapper : ObjectMapperBase
    {
        #region Fields

        private ObjectPropertyDescriptor[] _cachedDescriptors;
        protected readonly IContainer _container;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the primary type that controls this AutoObjectMapper instance.
        /// The primary type is the type that has all Map attributes and stuff
        /// on it. ie: This is the ViewModelT instance for ViewModel mapping.
        /// </summary>
        public Type PrimaryType { get; private set; }

        /// <summary>
        /// Gets the secondary type that the primary type controls.
        /// ie: This is the TEntity in ViewModelOfTEntity.
        /// </summary>
        public Type SecondaryType { get; private set; }

        #endregion

        #region Constructors

        public AutoObjectMapper(IContainer container, Type primaryType, Type secondaryType)
        {
            _container = container;
            primaryType.ThrowIfNull("primaryType");
            secondaryType.ThrowIfNull("secondaryType");

            PrimaryType = primaryType;
            SecondaryType = secondaryType;
        }

        #endregion

        #region Private Methods

        protected override IEnumerable<ObjectPropertyDescriptor> GetPropertyDescriptors()
        {
            // TODO: See about caching this per-type in a static ConcurrentDictionary.
            if (_cachedDescriptors == null)
            {
                var descriptors = GetPrimaryProperties(PrimaryType).Select(CreatePropertyDescriptor).ToArray();
                EnsureUniqueSetters(descriptors);
                _cachedDescriptors = descriptors;
            }

            return _cachedDescriptors;
        }

        private static void EnsureUniqueSetters(ObjectPropertyDescriptor[] descriptors)
        {
            var settable = descriptors.Where(x => x.CanMapToSecondary).ToArray();
            foreach (var s in settable)
            {
                var others =
                    settable.Where(
                        x =>
                            x.SecondaryAccessor.GetUniqueSetterIdentifier() ==
                            s.SecondaryAccessor.GetUniqueSetterIdentifier()).ToArray();

                if (others.Count() > 1)
                {
                    var primaryNames = others.Select(x => x.Name);
                    throw ObjectMapperException.MultipleSettersFound(primaryNames);
                }
            }
        }

        private static IEnumerable<PropertyInfo> GetPrimaryProperties(Type primaryObjectType)
        {
            return primaryObjectType.GetProperties();
        }

        protected virtual ObjectPropertyDescriptor CreatePropertyDescriptor(PropertyInfo prop)
        {
            var attr = GetMapperAttribute(prop);
            return attr.CreatePropertyDescriptor(_container, prop, SecondaryType);
        }

        protected virtual AutoMapAttribute GetMapperAttribute(PropertyInfo propInfo)
        {
            return
                // first try this instance
                propInfo.GetCustomAttributes<AutoMapAttribute>(false).SingleOrDefault() ??
                // then try parent class(es)
                propInfo.GetCustomAttributes<AutoMapAttribute>(true).SingleOrDefault() ??
                new AutoMapAttribute();
        }

        #endregion
    }
}
