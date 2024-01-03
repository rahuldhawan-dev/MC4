using System;
using System.Collections.Generic;
using NHibernate.Criterion;

namespace MMSINC.Data
{
    /// <summary>
    /// Search modifier that allows for a query do "where PropertyOne = Whatever OR PropertyTwo = Something".
    /// This is only meant to be used with different properties, not multiple different values on the same property.
    ///
    /// NOTE: This will remove the default mapping for both properties.
    /// ie it won't do "where PropertyOne == Whatever AND PropertyTwo == Something" along with the OR clause.
    /// </summary>
    public class OrConstraintSearchModifier : ISearchModifier
    {
        #region Fields

        private readonly string _propertyName1,
                                _propertyName2;

        #endregion

        #region Constructor

        public OrConstraintSearchModifier(string propertyName1, string propertyName2)
        {
            if (propertyName1 == propertyName2)
            {
                throw new InvalidOperationException(
                    "OrConstraintSearchModifier must be used with two different properties.");
            }

            _propertyName1 = propertyName1;
            _propertyName2 = propertyName2;
        }

        #endregion

        #region Public Methods

        public ICriterion ToCriterion(ICriterion currentCrit,
            IDictionary<string, SearchMappedProperty> mappedProperties)
        {
            ICriterion PreparePropertyAndGetCriterion(string propertyName)
            {
                SearchMappedProperty prop;
                if (!mappedProperties.TryGetValue(propertyName, out prop))
                {
                    throw new KeyNotFoundException(
                        $"The property '{propertyName}' was not found in the dictionary of mapped properties.");
                }

                // Need to disable normal mapping on both properties, otherwise we'll end up with a
                // query that does "where Prop1 = ? AND Prop2 = ? AND (Prop1 = ? OR Prop2 = ?)"
                prop.CanMap = false;
                return prop.ToCriterion(currentCrit);
            }

            return Restrictions.Or(PreparePropertyAndGetCriterion(_propertyName1),
                PreparePropertyAndGetCriterion(_propertyName2));
        }

        #endregion
    }
}
