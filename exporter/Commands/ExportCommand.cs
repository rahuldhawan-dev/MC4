using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Oakton;

namespace exporter.Commands
{
    public class ExportCommand : OaktonCommand<ExportInput>
    {
        #region Constants

        public const string CONNECTION_STRING =
            "Data Source=localhost;Initial Catalog=mapcalldev;Integrated Security=true;MultipleActiveResultSets=true";

        #endregion

        #region Private Methods

        static async Task QueryAndWriteFile(
            SqlConnection connection, (string FileName, string Script) script, ExportInput input)
        {
            var outputPath = Path.Join(
                input.OutputDirectoryFlag, input.GenerateOutputFileName(script.FileName));

            Console.WriteLine($"Generating file '{outputPath}'...");

            await using var outputFile = new StreamWriter(outputPath);
            await using var command = new SqlCommand(script.Script, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                await outputFile.WriteAsync(reader.GetString(0));
            }

            outputFile.Close();
            await reader.CloseAsync();
        }

        private void DoExport(ExportInput input, (string FileName, string Script) script)
        {
            using var connection = new SqlConnection(CONNECTION_STRING);
            connection.Open();

            Task.WaitAll(QueryAndWriteFile(connection, script, input));
        }

        private IEnumerable<(string FileName, string Script)> EnsureInputScripts(ExportInput input)
        {
            if (!Directory.Exists(input.InputScriptDirectory))
            {
                ConsoleWriter.Write(
                    ConsoleColor.Red,
                    $"Cannot locate input script directory '{input.InputScriptDirectory}'");
                Program.Exit(Program.ExitCodes.InputScriptDirectoryDoesNotExist);
            }

            var files = Directory
                       .EnumerateFiles(
                            input.InputScriptDirectory, "*.sql", SearchOption.TopDirectoryOnly)
                       .OrderBy(s => s);

            foreach (var file in files)
            {
                yield return (file, File.ReadAllText(file));
            }
        }

        private void EnsureOutputDirectory(ExportInput input)
        {
            if (!Directory.Exists(input.OutputDirectoryFlag))
            {
                ConsoleWriter.Write(
                    $"Output directory '{input.OutputDirectoryFlag}' does not exist");

                if (input.CreateOutputDirectoryFlag)
                {
                    ConsoleWriter.Write(
                        $"Creating output directory '{input.OutputDirectoryFlag}'...");
                    Directory.CreateDirectory(input.OutputDirectoryFlag);
                }
                else
                {
                    ConsoleWriter.Write(
                        ConsoleColor.Red,
                        "Cannot write to output directory which does not exist.");
                    Program.Exit(Program.ExitCodes.OutputDirectoryDoesNotExist);
                }
            }
        }

        #endregion

        #region Exposed Methods

        public override bool Execute(ExportInput input)
        {
            EnsureOutputDirectory(input);
            foreach (var script in EnsureInputScripts(input))
            {
                DoExport(input, script);
            }

            ConsoleWriter.Write(
                ConsoleColor.Green,
                "All tasks performed successfully!");
            return true;
        }

        #endregion
    }
}
