using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using NHibernate.Criterion;
using MMSINC.ClassExtensions.IEnumerableExtensions;

namespace MMSINC.Data
{
    [DebuggerDisplay("{Name}")]
    public class SearchMappedProperty
    {
        #region Properties

        /// <summary>
        /// Gets the name of the property as it appears on the entity. Set this if the 
        /// property on the search model has a different name than the property on the
        /// entity. Defaults to the same value as Name.
        /// </summary>
        public string ActualName { get; set; }

        /// <summary>
        /// Gets/sets whether this property can be directly mapped from a search model 
        /// to an entity. ex: A logical property. Defaults to true. This should really
        /// only be set via the SearchAttribute or an ISearchModifier.
        /// </summary>
        public bool CanMap { get; set; }

        /// <summary>
        /// Returns true if the property has a search alias.
        /// </summary>
        public bool IsAliased
        {
            get { return SearchAlias != null; }
        }

        /// <summary>
        /// Gets the property's name as it appears on the search model.
        /// </summary>
        public string Name { get; private set; }

        public PropertyInfo PropertyInfo { get; private set; }
        public SearchAliasAttribute SearchAlias { get; private set; }

        /// <summary>
        /// Set to true if the search for this property is a child collection and
        /// that the search is only checking if the collection has(or doesn't have)
        /// any items.
        /// </summary>
        public bool ChecksExistenceOfChildCollection { get; set; }

        /// <summary>
        /// Returns true if the mapped property has a searchable value.
        /// </summary>
        public bool HasValue
        {
            get
            {
                if (Value == null)
                {
                    return false;
                }

                if (Value is IEnumerable && !((IEnumerable)Value).Any())
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Gets/sets the property value. If this is null, the property is not going to be searched for.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets the name of the property as it should be used when mapping. 
        /// Uses ActualName, then search alias, then property name until it finds a value.
        /// </summary>
        public string MappedName
        {
            get
            {
                // ActualName defaults to using PropertyInfo.Name. If that's set then
                // we need to ignore ActualName and skip to the aliased name.
                if (ActualName != Name)
                {
                    return ActualName;
                }

                if (IsAliased)
                {
                    return String.Format("{0}.{1}", SearchAlias.Alias, SearchAlias.Property);
                }

                return Name;
            }
        }

        #endregion

        #region Constructor

        // No one should be creating instances of this except for SearchMapper.
        internal SearchMappedProperty(PropertyInfo prop, IEnumerable<Attribute> attrs)
        {
            PropertyInfo = prop;
            // ReSharper disable PossibleMultipleEnumeration
            // Use FirstOrDefault so inheritors can override interface attributes.
            SearchAlias = attrs.OfType<SearchAliasAttribute>().FirstOrDefault();
            ActualName = PropertyInfo.Name;
            Name = ActualName;

            var searchAttr = attrs.OfType<SearchAttribute>().FirstOrDefault() ?? new SearchAttribute();
            CanMap = searchAttr.CanMap;
            ChecksExistenceOfChildCollection = searchAttr.ChecksExistenceOfChildCollection;
            // ReSharper restore PossibleMultipleEnumeration
        }

        #endregion

        #region Private Methods

        private ICriterion GetWildcardMatch(string value, string propName)
        {
            if (value.Contains('*'))
            {
                // MatchMode.Exact adheres to the wildcards exactly.
                value = value.Replace('*', '%');
                return Restrictions.Like(propName, value, MatchMode.Exact);
            }

            // Using MatchMode.Anywhere completely ignores wildcard characters and essentially
            // does a search of: LIKE '%prop.Value%'
            return Restrictions.Like(propName, value, MatchMode.Anywhere);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns an ICriterion instance for this property. If HasValue is false, null is returned.
        /// </summary>
        /// <param name="currentCrit">This parameter should die. The only thing that needs it is ISearchCriteria stuff that doesn't actually need the previous criterion.</param>
        /// <returns></returns>
        public ICriterion ToCriterion(ICriterion currentCrit)
        {
            if (!HasValue)
            {
                return null;
            }

            var mappedName = MappedName;
            var propVal = Value;

            // NOTE: Do any equality checks before doing type checks as they're faster.
            if (propVal == SearchMapperSpecialValues.IsNull)
            {
                return Restrictions.IsNull(mappedName);
            }
            // ISearchMappable may add null values because they're needed in the search.
            else if (propVal == SearchMapperSpecialValues.IsNotNull)
            {
                return Restrictions.IsNotNull(mappedName);
            }
            // Ensure the value is not empty and is not null
            else if (propVal == SearchMapperSpecialValues.IsNotNullOrEmpty)
            {
                return Restrictions.Conjunction().Add(Restrictions.IsNotNull(mappedName))
                                   .Add(Restrictions.Not(Restrictions.Eq(mappedName, String.Empty)));
            }
            else if (propVal == SearchMapperSpecialValues.IsNullOrEmpty)
            {
                return Restrictions.Disjunction().Add(Restrictions.IsNull(mappedName))
                                   .Add(Restrictions.Eq(mappedName, String.Empty));
            }
            else if (propVal is ISearchCriterion)
            {
                // NOTE: This is where criterions come from that return the same criteria
                //       that was passed in. Specifically Range<T> does this. Leaving this
                //       for compatability with ViewModelToSearchMapper but it should probably
                //       do something more clear(like return null).
                return ((ISearchCriterion)Value).GetCriterion(currentCrit, mappedName);
            }
            else if (propVal is SearchString)
            {
                var search = (SearchString)Value;
                var strValue = search.Value;

                // search.Value can end up null if the user posts back the match type but doesn't 
                // enter a value, which is the default view if they aren't searching on this specific field.
                if (string.IsNullOrWhiteSpace(strValue))
                {
                    return null;
                }
                else
                {
                    switch (search.MatchType)
                    {
                        case SearchStringMatchType.Exact:
                            return Restrictions.Eq(mappedName, strValue);

                        case SearchStringMatchType.Wildcard:
                            return GetWildcardMatch(strValue, mappedName);

                        default:
                            throw new NotSupportedException();
                    }
                }
            }
            else if (propVal is string[])
            {
                ICriterion result = null;

                // For normal single string fields, the model binder converts empty strings
                // to null. The model binder doesn't do this with string arrays, so we want
                // to ignore empty strings here like we would ignore null strings otherwise.

                var strings = ((string[])propVal).Where(x => !string.IsNullOrWhiteSpace(x));

                foreach (var str in strings)
                {
                    if (result == null)
                    {
                        result = GetWildcardMatch(str, mappedName);
                    }
                    else
                    {
                        result = Restrictions.Or(result, GetWildcardMatch(str, mappedName));
                    }
                }

                return result;
            }
            else if (propVal is IList)
            {
                return Restrictions.In(mappedName, (IList)propVal);
            }
            else if (propVal is string)
            {
                return GetWildcardMatch((string)propVal, mappedName);
            }
            else if (propVal is bool && ChecksExistenceOfChildCollection)
            {
                // This mapping is working on the assumption that if it is passed true then we want
                // to find records where the child collection has at least one item.
                return (bool)propVal ? Restrictions.IsNotEmpty(mappedName) : Restrictions.IsEmpty(mappedName);
            }
            else
            {
                return Restrictions.Eq(mappedName, propVal);
            }
        }

        #endregion
    }
}
