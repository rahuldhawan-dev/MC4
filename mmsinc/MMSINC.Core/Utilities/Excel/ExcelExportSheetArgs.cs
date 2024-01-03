using System.Collections.Generic;

namespace MMSINC.Utilities.Excel
{
    public class ExcelExportSheetArgs
    {
        #region Properties

        /// <summary>
        /// Gets/sets the specific properties that can be exported for this sheet. 
        /// If this is null, all properties are exported. This will not override the
        /// DoesNotExport attribute on a property.
        /// </summary>
        public IEnumerable<string> ExportableProperties { get; set; }

        /// <summary>
        /// Gets/sets the optional name for the worksheet. If this is null, the export will generate one automatically.
        /// </summary>
        public string SheetName { get; set; }

        /// <summary>
        /// Gets/sets the optional text to be placed above the header row of the worksheet.
        /// </summary>
        public string Header { get; set; }

        #endregion
    }
}
