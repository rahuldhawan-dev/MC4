using System.Collections.Generic;
using NHibernate.Criterion;

namespace MMSINC.Data
{
    /// <summary>
    /// Interface for implementing search modifiers used by SearchMapper for SearchSets
    /// </summary>
    public interface ISearchModifier
    {
        /// <summary>
        /// Returns an ICriterion to be added to the final search criteria.
        ///
        /// There are some important things to know about how search modifiers are used:
        /// 1. Modifiers must be created and added to the ISearchMapper instance passed to an ISearchSet's ModifyValues method.
        /// 2. Modifiers are ran *before* the mapped properties. This means a modifier can stop a mapped property from being
        /// mapped if it wants to.
        /// </summary>
        /// <param name="currentCrit"></param>
        /// <param name="mappedProperties"></param>
        /// <returns></returns>
        ICriterion ToCriterion(ICriterion currentCrit, IDictionary<string, SearchMappedProperty> mappedProperties);
    }
}
