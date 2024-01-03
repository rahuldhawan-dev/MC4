using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using MapCallImporter.Common;
using MapCallImporter.Importing;
using MapCallImporter.Library.TypeRegistration;
using MapCallImporter.Validation;
using ShellProgressBar;
using StructureMap;

namespace MapCallImporter.Console
{
    class Program
    {
        public enum ExitCodes
        {
            Success = 0,
            InvalidArgument,
            FileAlreadyOpen,
            InvalidFileType,
            InvalidFileContents,
            OtherError
        }

        private static ProgressBarWrapper _pbar;

        static void Main(string[] args)
        {
            var options = ParseArguments(args);
            var container = new DependencyRegistrar().Initialize();

            using (_pbar = new ProgressBarWrapper(new ProgressBar(2, "main", new ProgressBarOptions {ForegroundColor = ConsoleColor.Yellow, BackgroundColor = ConsoleColor.DarkYellow})))
            {
                _pbar.Tick(0, $"Validating '{options.FileName}'...");

                var validationResult = ValidateFile(_pbar, container, options.FileName);
                CheckValidationResult(options.FileName, validationResult);

                _pbar.Tick(1, $"Importing '{options.FileName}'...");

                var importResult = ImportFile(_pbar, container, validationResult);
                CheckImportResult(options.FileName, importResult);

                _pbar.Tick(2, $"File '{options.FileName} has been successfully imported!");
            }

            System.Console.WriteLine(Environment.NewLine);
        }

        private static void CheckImportResult(string fileName, TimedExcelFileMappingResult result)
        {
            switch (result.Result)
            {
                case ExcelFileProcessingResult.InvalidFileContents:
                    OnError(ExitCodes.InvalidFileContents, new [] {$"The file '{fileName}' has invalid contents."}.Union(result.Issues).ToArray());
                    break;
                case ExcelFileProcessingResult.FileValid:
                    Exit(ExitCodes.Success);
                    break;
            }
        }

        private static void CheckValidationResult(string fileName, TimedExcelFileMappingResult result)
        {
            switch (result.Result)
            {
                case ExcelFileProcessingResult.FileAlreadyOpen:
                    OnError(ExitCodes.FileAlreadyOpen,
                        $"The file '{fileName}' is already open in another process.  Please close the file, or choose another.");
                    break;
                case ExcelFileProcessingResult.InvalidFileType:
                    OnError(ExitCodes.InvalidFileType,
                        $"The file '{fileName}' does not appear to be a valid Microsoft Excel Open XML (Excel 2007+) document.  Please choose another file.");
                    break;
                case ExcelFileProcessingResult.InvalidFileContents:
                    OnError(ExitCodes.InvalidFileContents, new [] {$"The file '{fileName}' has invalid contents."}.Union(result.Issues).ToArray());
                    break;
                case ExcelFileProcessingResult.OtherError:
                    OnError(ExitCodes.OtherError, new [] {$"Error processing file '{fileName}'."}.Union(result.Issues).ToArray());
                    break;
                case ExcelFileProcessingResult.CouldNotDetermineContentType:
                    OnError(ExitCodes.InvalidFileType,
                        $"Could not determine type of file '{fileName}' by its column headers.  Please adjust the columns to match an expected file type, or choose another file.");
                    break;
            }
        }

        private static TimedExcelFileMappingResult HandleFile<TService>(IProgressBar parentBar, IContainer container, string stepName, Func<TService, TimedExcelFileMappingResult> handleFn)
            where TService : ExcelFileHandlerServiceBase
        {
            var service = container.GetInstance<TService>();

            using (var pbar = parentBar.Spawn(100, stepName, new ProgressBarOptions {ForegroundColor = ConsoleColor.Green, BackgroundColor = ConsoleColor.DarkGreen, CollapseWhenFinished = true}))
            {
                service.ProgressChanged += (_, e) => pbar.Tick((int)Math.Round(e.ProgressPercentage), e.Status);

                return handleFn(service);
            }
        }

        private static TimedExcelFileMappingResult ImportFile(IProgressBar parentBar, IContainer container, TimedExcelFileMappingResult validationResult)
        {
            return HandleFile<ExcelFileImportingService>(parentBar, container, "Importing", s => s.Handle(validationResult));
        }

        private static TimedExcelFileMappingResult ValidateFile(IProgressBar parentBar, IContainer container, string fileName)
        {
            return HandleFile<ExcelFileValidationService>(parentBar, container, "Importing", s => s.Handle(fileName));
        }

        private static Options ParseArguments(string[] args)
        {
            Options ret = null;
            Parser.Default.ParseArguments<Options>(args)
                  .WithParsed(o => ret = ValidateOptions(o))
                  .WithNotParsed(OnArgumentError);

            return ret;
        }

        private static void OnError(ExitCodes code, params string[] errors)
        {
            if (_pbar != null)
            {
                _pbar.Tick(2);
                try
                {
                    _pbar.Dispose();
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e);
                }
            }

            foreach (var error in errors)
            {
                System.Console.Error.WriteLine(error);
            }

            Exit(code);
        }

        private static void OnArgumentError(IEnumerable<Error> obj)
        {
            OnArgumentError(obj.Select(o => o.ToString()));
        }

        private static void OnArgumentError(IEnumerable<string> obj)
        {
            OnError(ExitCodes.InvalidArgument, obj.ToArray());
        }

        private static Options ValidateOptions(Options options)
        {
            if (!File.Exists(options.FileName))
            {
                OnArgumentError(new [] {$"Path '{options.FileName}' does not exist."});
            }

            return options;
        }

        public static void Exit(ExitCodes code)
        {
            Environment.Exit((int)code);
        }
    }

    public class Options
    {
        [Value(0, MetaName = "input file", HelpText = "Input file to be processed.", Required = true)]
        public string FileName { get; set; }
    }

    public class ProgressBarWrapper : IProgressBar
    {
        private ProgressBar _progressBar;

        public ProgressBarWrapper(ProgressBar progressBar)
        {
            _progressBar = progressBar;
        }

        public void Dispose()
        {
            _progressBar.Dispose();
        }

        public ChildProgressBar Spawn(int maxTicks, string message, ProgressBarOptions options = null)
        {
            return _progressBar.Spawn(maxTicks, message, options);
        }

        public void Tick(string message = "")
        {
            _progressBar.Tick(message);
        }

        public void Tick(int newTickCount, string message = "")
        {
            _progressBar.Tick(newTickCount, message);
        }

        public void WriteLine(string message)
        {
            _progressBar.WriteLine(message);
        }

        public void WriteErrorLine(string message)
        {
            _progressBar.WriteErrorLine(message);
        }

        public IProgress<T> AsProgress<T>(Func<T, string> message = null, Func<T, double?> percentage = null)
        {
            return _progressBar.AsProgress(message, percentage);
        }

        public int MaxTicks
        {
            get => _progressBar.MaxTicks;
            set => _progressBar.MaxTicks = value;
        }

        public string Message
        {
            get => _progressBar.Message;
            set => _progressBar.Message = value;
        }

        public double Percentage => _progressBar.Percentage;

        public int CurrentTick => _progressBar.CurrentTick;

        public ConsoleColor ForegroundColor
        {
            get => _progressBar.ForegroundColor;
            set => _progressBar.ForegroundColor = value;
        }
    }
}
