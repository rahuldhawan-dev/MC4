using System.Data;

namespace MMSINC.DataPages
{
    public interface IFilterBuilderParameter
    {
        #region Properties

        /// <summary>
        /// Gets or sets the parameter name as it will be used in the resulting select command.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        object Value { get; set; }

        /// <summary>
        /// Gets or sets the DbType of the column/value.
        /// </summary>
        DbType DbType { get; set; }

        /// <summary>
        /// Returns the Name property formatted so that it can be used properly as a parameter variable name. 
        /// Ex: "t1.field" returns "t1field" so it can be used as "@t1field". 
        /// </summary>
        string ParameterFormattedName { get; }

        /// <summary>
        /// Returns the Name property formatte so that it's in its fully qualified format.
        /// Ex: "t1.field" returns "[t1].[field]"
        /// </summary>
        string QualifiedFormattedName { get; }

        #endregion
    }
}
