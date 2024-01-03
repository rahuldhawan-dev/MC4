using System;
using System.Data;

namespace MMSINC.DataPages
{
    public class FilterBuilderParameter : IFilterBuilderParameter
    {
        #region Fields

        private string _name;

        #endregion

        #region Properties

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                ParameterFormattedName = (_name == null ? null : GetParameterizedFormattedName(_name));
                QualifiedFormattedName = (_name == null ? null : GetFormattedQualifiedFieldName(_name));
            }
        }

        public object Value { get; set; }

        public DbType DbType { get; set; }

        /// <summary>
        /// Returns the Name property formatted so that it can be used properly as a parameter variable name. 
        /// Ex: "t1.field" returns "t1field" so it can be used as "@t1field". 
        /// </summary>
        public string ParameterFormattedName { get; private set; }

        /// <summary>
        /// Returns the Name property formatte so that it's in its fully qualified format.
        /// Ex: "t1.field" returns "[t1].[field]"
        /// </summary>
        public string QualifiedFormattedName { get; private set; }

        #endregion

        #region Constructors

        public FilterBuilderParameter() { }

        public FilterBuilderParameter(string name, DbType dbType, object value) : this()
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            Name = name;
            DbType = dbType;
            Value = value;
        }

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Returns a parameter name formatted so that it can be used properly as a parameter variable name. 
        /// Ex: "t1.field" returns "t1field" so it can be used as "@t1field". 
        /// </summary>
        public static string GetParameterizedFormattedName(string paramName)
        {
            if (paramName == null)
            {
                return null;
            }

            paramName = paramName
                       .Replace(".", string.Empty)
                       .Replace(" ", string.Empty);

            return paramName;
        }

        /// <summary>
        /// Formats a parameter name so that it's in its fully qualified format.
        /// Ex: "t1.field" returns "[t1].[field]".
        /// </summary>
        public static string GetFormattedQualifiedFieldName(string paramName)
        {
            if (paramName == null)
            {
                throw new ArgumentNullException("paramName");
            }

            if (paramName.Contains("[") || paramName.Contains("]"))
            {
                throw new InvalidOperationException("Do not use brackets.");
            }

            var left = !paramName.Contains(".") ? new[] {paramName} : paramName.Split('.');

            const string leftFormat = "[{0}]";
            for (var i = 0; i < left.Length; i++)
            {
                var cur = left[i];
                left[i] = string.Format(leftFormat, cur);
            }

            return string.Join(".", left);
        }

        #endregion
    }
}
