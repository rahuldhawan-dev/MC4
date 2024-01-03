using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace MMSINC.DataPages
{
    public interface IFilterBuilderExpression
    {
        #region Properties

        /// <summary>
        /// Overrides the auto-generated expression when set.
        /// </summary>
        string CustomFilterExpression { get; set; }

        IList<IFilterBuilderParameter> Parameters { get; }

        /// <summary>
        /// Gets or sets whether this expression should be built when any of the parameters include null values. 
        /// </summary>
        bool IgnoreIfThereAreNullParameters { get; set; }

        #endregion

        #region Methods

        void AddParameter(IFilterBuilderParameter filterBuilder);
        IFilterBuilderParameter AddParameter(string name, DbType dbType, object value);

        void BuildFilterExpression(IList<string> expressions);

        IEnumerable<Parameter> BuildParameters();

        IEnumerable<SqlParameter> BuildSqlParameters();

        #endregion
    }
}
