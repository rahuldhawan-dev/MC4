using System;

namespace MMSINC.Data
{
    /// <summary>
    /// Interface for search classes that need to implement additional logic to ViewModelToSearchMapper.
    /// </summary>
    [Obsolete("Implement ISearchSet and then create a specific repository and search method to do this.")]
    public interface ISearchCriteria
    {
        /// <summary>
        /// Called on a model to remove/add property values before
        /// </summary>
        /// <param name="args"></param>
        void EnsureSearchValues(SearchMappableArgs args);
    }

    [Obsolete("Use ISearchCriteria instead.")]
    public interface ISearchMappable : ISearchCriteria { }
}
