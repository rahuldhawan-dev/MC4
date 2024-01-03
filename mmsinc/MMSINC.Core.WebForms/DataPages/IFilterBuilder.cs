using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace MMSINC.DataPages
{
    public interface IFilterBuilder
    {
        #region Properties

        string OriginatingPageUrl { get; set; }

        string ConnectionString { get; set; }

        /// <summary>
        /// The base select statement that the filter will be appended to.
        /// </summary>
        string SelectCommand { get; set; }

        IList<IFilterBuilderExpression> Expressions { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a new IFilterBuilderExpression to the current IFilterBuilder instance.
        /// </summary>
        /// <param name="filterExpression"></param>
        void AddExpression(IFilterBuilderExpression filterExpression);

        /// <summary>
        /// Builds the parameterized filter.
        /// </summary>
        /// <returns></returns>
        string BuildFilter();

        /// <summary>
        /// Returns SqlDataSource Parameters.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Parameter> BuildParameters();

        /// <summary>
        /// Returns SqlParameters that can be used in an SqlCommand. This is not the same as BuildParameters.
        /// </summary>
        /// <returns></returns>
        IEnumerable<SqlParameter> BuildSqlParameters();

        /// <summary>
        /// Combines the SelectCommand and Filter
        /// </summary>
        /// <returns></returns>
        string BuildCompleteCommand();

        #endregion
    }
}
