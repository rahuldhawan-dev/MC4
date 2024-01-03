using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using NHibernate;
using NHibernate.Criterion;

namespace MMSINC.Data
{
    /// <summary>
    /// Represents an object that maps an ISearchSet instance to a where clause in NHibernate.
    /// </summary>
    public interface ISearchMapper
    {
        /// <summary>
        /// Clears all of the Value properties in the MappedProperties dictionary.
        /// </summary>
        void ClearValues();

        /// <summary>
        /// Accessible mapped properties that can have values added/removed/modified before
        /// creating criterions.
        /// </summary>
        IDictionary<string, SearchMappedProperty> MappedProperties { get; }

        IList<ISearchModifier> SearchModifiers { get; }

        IDictionary<string, string> GetAliases();
        ICriterion Map();
    }

    public static class SearchMapperSpecialValues
    {
        /// <summary>
        /// Use this to indicate that a mapped value needs to be anything but null.
        /// </summary>
        public static readonly object IsNotNull = new object();

        /// <summary>
        /// Use this to indicate that a mapped value must equal null. Using null itself
        /// will cause the property to be excluded from searching.
        /// </summary>
        public static readonly object IsNull = new object();

        /// <summary>
        /// Use this to indicate that a mapped value must must be an empty string
        /// </summary>
        public static readonly object IsNotNullOrEmpty = new object();

        /// <summary>
        /// Use this to indicate that A mapped value must be null or an empty string
        /// </summary>
        public static readonly object IsNullOrEmpty = new object();
    }

    public class SearchMapper : ISearchMapper
    {
        #region Fields

        private readonly ISearchSet _model;
        private readonly IDictionary<string, SearchMappedProperty> _mappedProperties;
        private readonly ISession _session;

        /// <summary>
        /// List of known mapped entity types in the current ISession.
        /// </summary>
        private readonly IEnumerable<Type> _knownEntityTypes;

        private readonly IDictionary<string, Type> _primaryEntityPropertyTypesByName;

        private static readonly Type _intType = typeof(int),
                                     _nullableIntType = typeof(int?);

        /// <summary>
        /// Properties from ISearchSet/ISearchSet(Of T) that can't be mapped.
        /// </summary>
        private static readonly HashSet<string> _ignoredProperties;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the dictionary of all the mapped properties, including those that do not have a value. Key
        /// is the property name as it appears in the search model.
        /// </summary>
        public IDictionary<string, SearchMappedProperty> MappedProperties
        {
            get { return _mappedProperties; }
        }

        /// <summary>
        /// Gets a collection of search modifiers that run after all of the model properties
        /// have been mapped and after the search model's ModifyValues function has been called.
        /// </summary>
        public IList<ISearchModifier> SearchModifiers { get; } = new List<ISearchModifier>();

        #endregion

        #region Constructor

        static SearchMapper()
        {
            var iSearchSetProps = typeof(ISearchSet).GetProperties().Select(x => x.Name);
            var iSearchSetTProps = typeof(ISearchSet<>).GetProperties().Select(x => x.Name);
            // "Mock" property gets added by Moq objects.
            _ignoredProperties = new HashSet<string>(iSearchSetProps.Union(iSearchSetTProps).Union(new[] {"Mock"}));
        }

        // TODO: Make this internal. The goal is to make this so only the Repository actually has to create
        // an instance of it.
        /// <summary>
        /// Creates a new SearchMapper instance.
        /// </summary>
        /// <param name="model">The ISearchSet instance that includes any search parameters.</param>
        /// <param name="primaryEntityType">The Type of the primary entity that is being searched for. The TEntity basically.</param>
        /// <param name="session">The current session to use.</param>
        public SearchMapper(ISearchSet model, Type primaryEntityType, ISession session)
        {
            model.ThrowIfNull("Null models are bad.");
            _model = model;
            _session = session;
            _knownEntityTypes = _session.GetSessionImplementation().Factory.GetAllClassMetadata().Values
                                        .Select(x => x.MappedClass)
                                        .ToList();
            _primaryEntityPropertyTypesByName = primaryEntityType
                                               .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                               .ToDictionary(x => x.Name, x => x.PropertyType);

            // Must come after everything else is set since GetPropertyValue is called in here
            // and relies on all of the above.
            _mappedProperties = CreateSearchPropertiesDictionary();

            model.ModifyValues(this);
        }

        #endregion

        #region Private Methods

        private IDictionary<string, SearchMappedProperty> CreateSearchPropertiesDictionary()
        {
            var props = new Dictionary<string, SearchMappedProperty>();
            // NOTE: The GetCustomAttributes extension being used in the linq below automatically includes
            //       inherited attributes.
            var modelType = _model.GetType();
            var interfaceProps = modelType.GetInterfaces()
                                          .SelectMany(x => x.GetProperties())
                                          .GroupBy(x => x.Name)
                                          .ToDictionary(x => x.Key,
                                               x => x.ToList().SelectMany(y => y.GetCustomAttributes()));

            foreach (var prop in modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!_ignoredProperties.Contains(prop.Name))
                {
                    var attributes = prop.GetCustomAttributes();
                    if (interfaceProps.ContainsKey(prop.Name))
                    {
                        // Make sure interface attributes come *after* the concrete type's.
                        // Concrete type can override what's on the interface.
                        attributes = attributes.Concat(interfaceProps[prop.Name]);
                    }

                    var mapped = new SearchMappedProperty(prop, attributes.ToArray());
                    if (mapped.Name == "EntityId")
                    {
                        mapped.ActualName = "Id";
                    }

                    mapped.Value = GetPropertyValue(mapped);

                    if (props.ContainsKey(mapped.PropertyInfo.Name))
                    {
                        // This happens when a search model inherits from another type and the inheriting model has a property with the same name.
                        // Make sure the property is overriding the child property rather than hiding it.
                        throw new InvalidOperationException(
                            $"SearchMapper can not map the current property({mapped.PropertyInfo.Name}) because it has already been mapped. See code comments for this error.");
                    }

                    props.Add(mapped.PropertyInfo.Name, mapped);
                }
            }

            return props;
        }

        private object GetPropertyValue(SearchMappedProperty prop)
        {
            var rawValue = prop.PropertyInfo.GetValue(_model);
            if (rawValue != null)
            {
                var propType = prop.PropertyInfo.PropertyType;

                // We can only attempt to load an entity reference on int/int? types.
                if (propType == _intType || propType == _nullableIntType)
                {
                    // Do not convert int properties to their entity version if they have a SearchAlias
                    // on them. The search aliases always point at an Id property of the entity which
                    // causes the search to fail with a type mismatch.
                    if (!prop.IsAliased && _primaryEntityPropertyTypesByName.ContainsKey(prop.ActualName))
                    {
                        // It's possible a property is mapped to an object that isn't mapped as an entity type.
                        // That would be a bad thing, of course.
                        var possibleEntityType = _primaryEntityPropertyTypesByName[prop.ActualName];
                        if (_knownEntityTypes.Contains(possibleEntityType))
                        {
                            return _session.Load(possibleEntityType, rawValue);
                        }
                    }
                }
            }

            return rawValue;
        }

        #endregion

        #region Public Methods
        
        /// <inheritdoc />
        public void ClearValues()
        {
            foreach (var prop in MappedProperties)
            {
                prop.Value.Value = null;
            }
        }

        public IDictionary<string, string> GetAliases()
        {
            var ret = MappedProperties.Values
                                      .Where(x => x.SearchAlias != null)
                                      .Select(x => x.SearchAlias)
                                      .DistinctBy(p => new {p.Alias, p.AssociationPath})
                                      .ToDictionary(prop => prop.Alias, prop => prop.AssociationPath);

            return ret;
        }

        public ICriterion Map()
        {
            var cur = Restrictions.Conjunction();
            IDictionary<string, SearchMappedProperty> toRemove = new Dictionary<string, SearchMappedProperty>();

            // Run modifiers *before* the MappedProperties as the modifiers
            // can change things about the mapped properties.
            foreach (var modifier in SearchModifiers)
            {
                cur.Add(modifier.ToCriterion(cur, MappedProperties));
            }

            foreach (var prop in MappedProperties.Values)
            {
                // CreateCriterion will return the same criterion instance back if it's
                // unable to create a criterion for a property.
                var next = prop.CanMap ? prop.ToCriterion(cur) : null;
                // TODO: Range<T> returns the same criterion back rather than null.
                //       This should be fixed and then the ReferenceEquals check removed.
                if (next != null && !ReferenceEquals(cur, next))
                {
                    // Using Restrictions.And with an empty Restrictions.Conjunction is
                    // what adds that (1=1) query that adds nothing of value. 
                    cur.Add(next);
                    //cur = Restrictions.And(cur, next);
                }
                else
                {
                    if (prop.SearchAlias == null || !prop.SearchAlias.Required)
                    {
                        // we aren't searching property, so lets mark it for removal from the mapped properties
                        toRemove.Add(prop.Name, prop);
                    }
                }
            }

            // If there are any properties we will remove them from the list of MappedProperties
            // This will keep any unnecessary joins out of the query. This keeps duplicates out
            // of the results when many to many joins exist but aren't queried.
            foreach (var item in toRemove)
            {
                MappedProperties.Remove(item);
            }

            return cur;
        }

        #endregion
    }
}
