using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;

namespace MMSINC.Utilities.Excel
{
    ///<summary>A format for a database file.</summary>
    public enum ExcelFormat
    {
        ///<summary>An Excel 97-2003 .xls file.</summary>
        Excel2003,

        ///<summary>An Excel 2007 .xlsx file.</summary>
        Excel2007,
    }

    public static class ExcelUtility
    {
        private static readonly Dictionary<ExcelFormat, string> FormatExtensions = new Dictionary<ExcelFormat, string> {
            {ExcelFormat.Excel2003, ".xls"},
            {ExcelFormat.Excel2007, ".xlsx"}
        };

        ///<summary>Gets the database format that uses the given extension.</summary>
        public static ExcelFormat GetDBType(string extension)
        {
            var pair = FormatExtensions.FirstOrDefault(kvp =>
                kvp.Value.Equals(extension, StringComparison.OrdinalIgnoreCase));

            if (pair.Value == null)
            {
                throw new ArgumentException("Unrecognized extension: " + extension, "extension");
            }

            return pair.Key;
        }

        ///<summary>Gets the file extension for a database format.</summary>
        public static string GetExtension(ExcelFormat format)
        {
            return FormatExtensions.First(kvp => kvp.Key == format).Value;
        }

        public static string GetConnectionString(string filePath, ExcelFormat format)
        {
            if (String.IsNullOrEmpty(filePath)) throw new ArgumentNullException("filePath");

            var csBuilder = new OleDbConnectionStringBuilder {DataSource = filePath, PersistSecurityInfo = false};

            switch (format)
            {
                case ExcelFormat.Excel2003:
                    csBuilder.Provider = "Microsoft.Jet.OLEDB.4.0";
                    csBuilder["Extended Properties"] = "Excel 8.0;IMEX=0;HDR=YES;";
                    break;
                case ExcelFormat.Excel2007:
                    csBuilder.Provider = "Microsoft.ACE.OLEDB.12.0";
                    // IMEX=1 forces all rows to be read as text instead of as whatever stupid datatype the Excel
                    // file thinks it should be. This needs to be 1 in order for the ValveSAPRequirement upload
                    // to work on .info and the live site. It doesn't locally. No one knows why. Probably because
                    // of Office 2013 or something. Also this is only needed on Excel 2007 and up files. 
                    // If we're reading, then IMEX should be 1, but if we're creating and the file doesn't exist
                    // then it should be 0.
                    csBuilder["Extended Properties"] = String.Format("Excel 12.0 Xml;HDR=YES;IMEX={0};",
                        File.Exists(filePath) ? 1 : 0);
                    break;
            }

            return csBuilder.ToString();
        }
    }
}
