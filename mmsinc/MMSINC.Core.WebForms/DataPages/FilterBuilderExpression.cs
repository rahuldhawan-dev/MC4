using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;

namespace MMSINC.DataPages
{
    public class FilterBuilderExpression : IFilterBuilderExpression
    {
        #region Properties

        public string CustomFilterExpression { get; set; }

        protected string FilterExpression
        {
            get
            {
                if (!CanBuild)
                {
                    return string.Empty;
                }

                // Return Custom if not null/empty so that this property can be
                // used consistantly instead of having to check for one or the other.
                else if (!String.IsNullOrWhiteSpace(CustomFilterExpression))
                {
                    return CustomFilterExpression;
                }

                // After this point this we're only concerned with the auto-generated filter expression.

                else if (Parameters.Count != 1)
                {
                    throw new InvalidOperationException(
                        "A CustomFilterExpression must be set when using two or more parameters, or if no parameters are involved.");
                }
                else
                {
                    return GetDefaultFilterExpression(GetDefaultParameter());
                }
            }
        }

        public bool IgnoreIfThereAreNullParameters { get; set; }
        public IList<IFilterBuilderParameter> Parameters { get; internal set; }

        /// <summary>
        /// Gets whether this instance has the correct setup to create a valid filter expression.
        /// </summary>
        /// <returns></returns>
        internal bool CanBuild
        {
            get
            {
                var hasNullParameters = (from f in Parameters
                                         where f.Value == null
                                         select f).Any();

                if (hasNullParameters && IgnoreIfThereAreNullParameters)
                {
                    return false;
                }

                // CustomFilterExpressions are  here in case something
                // needs to be added that requires more than one parameter,
                // something other than "A = B", 
                // or no parameters at all(*cough* sql injection *cough*). 
                // Assuming this is set, it's up to the user to make sure
                // any parameters that are required get set. 
                if (!String.IsNullOrWhiteSpace(CustomFilterExpression))
                {
                    return true;
                }

                return (Parameters.Any());
            }
        }

        #endregion

        private IFilterBuilderParameter GetDefaultParameter()
        {
            // Using Single instead of First just to really hit home that there can only be one parameter at this point.
            return Parameters.Single();
        }

        private static string GetDefaultFilterExpression(IFilterBuilderParameter parameter)
        {
            return string.Format("{0} = @{1}", parameter.QualifiedFormattedName, parameter.ParameterFormattedName);
        }

        #region Constructor

        public FilterBuilderExpression()
        {
            Parameters = new List<IFilterBuilderParameter>();
        }

        public FilterBuilderExpression(string customFilterExpression)
            : this()
        {
            if (String.IsNullOrWhiteSpace(customFilterExpression))
            {
                throw new ArgumentNullException("customFilterExpression");
            }

            CustomFilterExpression = customFilterExpression;
        }

        /// <summary>
        /// Constructs a new FilterBuilderExpression and creates an initial IFilterBuilderParameter with
        /// the given values. 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="dbType"></param>
        /// <param name="value"></param>
        public FilterBuilderExpression(string paramName, DbType dbType, object value)
            : this()
        {
            if (String.IsNullOrWhiteSpace(paramName))
            {
                throw new ArgumentNullException("paramName");
            }

            AddParameter(paramName, dbType, value);
        }

        #endregion

        #region Exposed Methods

        public void AddParameter(IFilterBuilderParameter filterParameter)
        {
            if (filterParameter == null)
            {
                throw new ArgumentNullException("filterParameter");
            }

            // Don't want duplicates. 
            if (!Parameters.Contains(filterParameter))
            {
                Parameters.Add(filterParameter);
            }
        }

        public IFilterBuilderParameter AddParameter(string name, DbType dbType, object value)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            var p = new FilterBuilderParameter {
                Name = name,
                DbType = dbType,
                Value = value
            };

            AddParameter(p);
            return p;
        }

        public void BuildFilterExpression(IList<string> expressions)
        {
            if (CanBuild)
            {
                var filter = FilterExpression;
                if (!string.IsNullOrEmpty(filter))
                {
                    expressions.Add(filter);
                }
            }
        }

        // TODO: Add testing to ensure that BuildParameters never returns the same instances of a parameter.

        public IEnumerable<Parameter> BuildParameters()
        {
            // Never return null. ALways return an empty list if
            // there are no parameters. Make a test for this. 
            var parms = new List<Parameter>();

            if (CanBuild)
            {
                foreach (var p in Parameters)
                {
                    // I believe this needed to be string, and not just var. So tell
                    // ReSharper to shut up. 
                    string value = (p.Value != null ? p.Value.ToString() : null);
                    parms.Add(new Parameter(p.ParameterFormattedName, p.DbType, value));
                }
            }

            return parms;
        }

        public IEnumerable<SqlParameter> BuildSqlParameters()
        {
            var parms = new List<SqlParameter>();

            if (CanBuild)
            {
                foreach (var p in Parameters)
                {
                    parms.Add(new SqlParameter(p.ParameterFormattedName, p.Value));
                }
            }

            return parms;
        }

        #endregion
    }
}
