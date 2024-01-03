using System.Collections.Generic;

namespace MMSINC.Data
{
    // NOTE: You generally do not want to implement the interface. Just inherit from SearchSet<T> instead.

    /// <summary>
    /// Represents a set of entities, and the properties used to search/query for them.
    /// </summary>
    public interface ISearchSet
    {
        #region Abstract Properties
        
        /// <summary>
        /// Current page number, when paging is enabled.
        /// </summary>
        int PageNumber { get; set; }
        /// <summary>
        /// Size of pages being generated, when paging is enabled.
        /// </summary>
        int PageSize { get; set; }
        /// <summary>
        /// Count of pages, when paging is enabled.  This value is <see cref="Count"/> divided by
        /// <see cref="PageSize"/> (rounded to a whole number).
        /// </summary>
        int PageCount { get; set; }
        /// <summary>
        /// Count of records returned in the <see cref="ISearchSet{TResult}.Results"/>.
        /// </summary>
        int Count { get; set; }

        /// <summary>
        /// Gets/sets whether or not paging should occur during a search. True by default.
        /// </summary>
        bool EnablePaging { get; set; }

        /// <summary>
        /// String representing the property by which to sort the <see cref="ISearchSet{TResult}.Results"/>,
        /// when set.
        /// </summary>
        string SortBy { get; set; }
        /// <summary>
        /// Boolean indicating whether to sort the results in ascending order by the property represented by
        /// <see cref="SortBy"/>.  Sort is descending when false.
        /// </summary>
        bool SortAscending { get; set; }

        /// <summary>
        /// Gets the default sort string to use if the SortBy property is null/empty.
        /// If this is also null then there won't be any sorting.
        /// </summary>
        string DefaultSortBy { get; }

        /// <summary>
        /// Gets the default sorting direction to be used when DefaultSortBy is being
        /// used for the sorting.
        /// </summary>
        bool DefaultSortAscending { get; }

        /// <summary>
        /// Gets/sets the list of properties that can be exported. If null, all properties
        /// are exported aside from those with DoesNotExport.
        /// </summary>
        List<string> ExportableProperties { get; set; }
        
        #endregion
        
        #region Abstract Methods

        /// <summary>
        /// Called by a search <paramref name="mapper"/> to modify any properties.
        /// </summary>
        void ModifyValues(ISearchMapper mapper);
        
        #endregion
    }

    /// <inheritdoc />
    public interface ISearchSet<TResult> : ISearchSet
    {
        /// <summary>
        /// The results returned by the query.
        /// </summary>
        IEnumerable<TResult> Results { get; set; }
    }

    /// <inheritdoc />
    /// <remarks>
    /// Default implementation of ISearchSet.
    /// </remarks>
    public abstract class SearchSet<TResult> : ISearchSet<TResult>
    {
        #region Fields

        private readonly List<TResult> _results = new List<TResult>();

        #endregion

        #region Properties

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int Count { get; set; }

        public virtual bool EnablePaging { get; set; } = true;

        public string SortBy { get; set; }
        public bool SortAscending { get; set; }

        public virtual string DefaultSortBy { get; private set; }

        public virtual bool DefaultSortAscending { get; private set; }

        public List<string> ExportableProperties { get; set; }

        public IEnumerable<TResult> Results
        {
            get => _results;
            set
            {
                _results.Clear();
                _results.AddRange(value);
            }
        }

        #endregion

        #region Public Methods

        public virtual void ModifyValues(ISearchMapper mapper) { } /* noop for default */

        #endregion
    }

    /// <inheritdoc />
    /// <remarks>
    /// Implementation of a SearchSet without any searchable properties. Mostly useful for testing.
    /// </remarks>
    public sealed class EmptySearchSet<TResult> : SearchSet<TResult> { }
}
