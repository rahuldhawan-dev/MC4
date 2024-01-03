using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.WebControls;

namespace MMSINC.DataPages
{
    // TODO: Add an optional "Importance" property to FilterBuilderExpressions. This would be used for ordering the
    //       expressions in most-selective to least-selective to maybe speed up performance. 

    // TODO: Add an OrderBy property. Needed due to the LIKE %% stuff not always returning in the same order.

    public class FilterBuilder : IFilterBuilder
    {
        #region Properties

        public string OriginatingPageUrl { get; set; }

        // These two properties are here as a quick fix. They don't really belong to FilterBuilder.
        // Though I guess I'm essentially remaking SqlCommand at this point.  
        public string ConnectionString { get; set; }
        public string SelectCommand { get; set; }

        public IList<IFilterBuilderExpression> Expressions { get; internal set; }

        #endregion

        #region Constructors

        public FilterBuilder()
        {
            Expressions = new List<IFilterBuilderExpression>();
        }

        #endregion

        #region Exposed Methods

        public void AddExpression(IFilterBuilderExpression filterExpression)
        {
            if (filterExpression == null)
            {
                throw new ArgumentNullException("filterExpression");
            }

            // Don't want duplicates. 
            if (!Expressions.Contains(filterExpression))
            {
                Expressions.Add(filterExpression);
            }
        }

        /// <summary>
        /// Combines the SelectCommand and Filter
        /// </summary>
        /// <returns></returns>
        public string BuildCompleteCommand()
        {
            if (string.IsNullOrWhiteSpace(SelectCommand))
            {
                throw new NullReferenceException("BuildCompleteCommand requires a non-null or empty SelectCommand.");
            }

            return (SelectCommand + BuildWhereClause());
        }

        public string BuildWhereClause()
        {
            var filt = BuildFilter();

            if (!string.IsNullOrWhiteSpace(filt))
            {
                // Add " WHERE " here and not in BuildFilter because it'll screw up the
                // Maps page otherwise. 
                // NewLine is needed to prevent commenting out the entire WHERE statement
                // and parameters on the off chance that the last line of the sql statement
                // is commented out. 
                return Environment.NewLine + " WHERE " + filt;
            }

            return string.Empty;
        }

        public string BuildFilter()
        {
            var preparedExpressions = new List<string>();

            foreach (var exp in Expressions)
            {
                exp.BuildFilterExpression(preparedExpressions);
            }

            if (!preparedExpressions.Any())
            {
                return string.Empty;
            }

            var result = string.Join(" AND ", preparedExpressions.ToArray());
            Debug.Print("Filter: " + result);
            return result;
        }

        public IEnumerable<Parameter> BuildParameters()
        {
            var parms = new List<Parameter>();

            // It's possible that parameters could be duplicated here. SelectParameters will throw
            // in that instance, but might wanna throw here just to specifically say "Yo dawg, I hear
            // you like parameters in your parameters".

            foreach (var exp in Expressions)
            {
                parms.AddRange(exp.BuildParameters());
            }

            return parms;
        }

        public IEnumerable<SqlParameter> BuildSqlParameters()
        {
            var parms = new List<SqlParameter>();

            foreach (var exp in Expressions)
            {
                parms.AddRange(exp.BuildSqlParameters());
            }

            return parms;
        }

        #endregion
    }
}
