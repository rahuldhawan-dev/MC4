using System;
using System.IO;
using Oakton;

namespace exporter.Commands
{
    public class ExportInput
    {
        #region Constants

        public static readonly string DEFAULT_OUTPUT_PATH = Path.Combine(
            "\\", "Solutions", "mapcall-monorepo", "exporter", "output");

        #endregion

        #region Private Members

        private string? _outputFileName;

        #endregion

        #region Properties

        [Description("Directory to output exported files to.  " +
                     "Default is \"output\" in the root of this project's source")]
        public string OutputDirectoryFlag { get; set; } = DEFAULT_OUTPUT_PATH;

        [Description("Create the output directory if it doesn't already exist.  Default is true")]
        public bool CreateOutputDirectoryFlag { get; set; } = true;

        [Description(
            "Directory containing input scripts to execute, the output from which are dumped to files.")]
        public string? InputScriptDirectory { get; set; }

        [Description(
            "Extension to use for the output file.  Defaults to the same as the input file (likely .sql)")]
        public string? OutputExtensionFlag { get; set; }

        [Description("When true (the default), the current date will be added to the output file name")]
        public bool AppendDateToOutputFlag { get; set; } = true;

        #endregion

        #region Private Methods

        public string GenerateOutputFileName(string inputScript)
        {
            var inputFile = Path.GetFileName(inputScript);
            var outputExtension = OutputExtensionFlag ?? Path.GetExtension(inputFile);
            var outputFile =
                Path.GetFileNameWithoutExtension(inputFile) +
                (AppendDateToOutputFlag ? $" {DateTime.Now:yyyy-MM-dd}" : string.Empty);

            return outputFile + outputExtension;
        }

        #endregion
    }
}
