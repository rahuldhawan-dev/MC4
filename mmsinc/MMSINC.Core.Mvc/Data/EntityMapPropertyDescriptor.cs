using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using MMSINC.ClassExtensions.ReflectionExtensions;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;
using StructureMap.Query;

namespace MMSINC.Data
{
    /// <summary>
    /// Descriptor for mapping an int/int? property on a ViewModel to a regular entity reference
    /// property on an entity object.
    /// </summary>
    public class EntityMapPropertyDescriptor : AutoPropertyDescriptor
    {
        #region Fields

        private static readonly Type _iRepositoryType = typeof(IBaseRepository);

        // This needs to be the generic IRepository type used in DependencyRegistrar.
        private static readonly Type _iRepositoryGenericType = typeof(MMSINC.Data.NHibernate.IRepository<>);
        protected readonly IContainer _container;

        #endregion

        #region Properties

        /// <summary>
        /// Gets/sets the repository type registered with ObjectFactory that can be used
        /// to retrieve an existing entity that matches the ViewModel's property. If this
        /// property is null, the mapper will attempt to figure out the repository
        /// based on the entity property's type.
        /// </summary>
        public Type RepositoryType { get; private set; }

        #endregion

        #region Constructors

        public EntityMapPropertyDescriptor(IContainer container,
            PropertyInfo primaryProp, Type secondaryType, Type repositoryType,
            MapDirections direction = MapDirections.BothWays)
            : this(container, primaryProp, secondaryType, repositoryType, null,
                direction) { }

        public EntityMapPropertyDescriptor(IContainer container, PropertyInfo primaryProp, Type secondaryType,
            Type repositoryType,
            string secondaryPropertyName,
            MapDirections direction = MapDirections.BothWays)
            : base(primaryProp, secondaryType, secondaryPropertyName, direction)
        {
            _container = container;
            if (repositoryType != null)
            {
                if (!_iRepositoryType.IsAssignableFrom(repositoryType))
                {
                    throw EntityMapperException.RepositoryMustImplementIRepository(repositoryType.FullName);
                }
            }

            RepositoryType = repositoryType;
        }

        #endregion

        #region Private Methods

        protected override IValueConverter CreateValueConverter()
        {
            return new EntityMapValueConverter(_container, EnsureGeneratedRepositoryTypeWorks,
                SecondaryAccessor.PropertyType, PrimaryAccessor.PropertyType);
        }

        private Type EnsureGeneratedRepositoryTypeWorks()
        {
            if (RepositoryType == null)
            {
                var entityType = EntityMapHelpers.IsPropertyASupportedGenericCollectionType(SecondaryAccessor.PropertyType)
                    ? SecondaryAccessor.PropertyType.GetGenericArguments().Single()
                    : SecondaryAccessor.PropertyType;
                var baseRepoType = _iRepositoryGenericType.MakeGenericType(new[] {entityType});

                var matches = _container.Model.PluginTypes.Where(x => baseRepoType.IsAssignableFrom(x.PluginType))
                                        .ToArray();

                // TODO: This needs to be tested. When there aren't any matches and GetInstance
                //       is called below, PluginTypes after that contains two registered types. It's annoying.
                if (matches.Count() == 1)
                {
                    RepositoryType = matches.First().PluginType;
                }
                else if (!matches.Any())
                {
                    try
                    {
                        // We need to know if the ObjectFactory has some other crap setup to dynamically
                        // register a repository type. So rather than just returning baseRepoType, we need to
                        // see if we can even create an instance.
                        RepositoryType = _container.GetInstance(baseRepoType).GetType();
                    }
                    catch (Exception)
                    {
                        throw EntityMapperException.CanNotFindRepositoryType(entityType.FullName);
                    }
                }
                else
                {
                    var baseMatch = matches.FirstOrDefault(x => x.PluginType == baseRepoType);
                    if (baseMatch != null)
                    {
                        RepositoryType = baseMatch.PluginType;
                    }
                    else
                    {
                        throw EntityMapperException.TooManyRepositoryTypeMatches(entityType.FullName,
                            matches.Select(x => x.PluginType));
                    }
                }
            }

            return RepositoryType;
        }

        #endregion
    }
}
