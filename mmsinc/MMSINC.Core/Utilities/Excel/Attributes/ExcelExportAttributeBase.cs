using System;

// ReSharper disable CheckNamespace
namespace MMSINC.Utilities.Excel
    // ReSharper restore CheckNamespace
{
    /// <summary>
    /// Base class for attributes used by ExcelExport
    /// </summary>
    public abstract class ExcelExportAttributeBase : Attribute { }

    public class ExcelExportColumnAttribute : ExcelExportAttributeBase
    {
        #region Properties

        /// <summary>
        /// Tells the Excel Exporter to use the property name for the column header rather than the display name.
        /// </summary>
        public bool UsePropertyName { get; set; }

        #endregion
    }
}
