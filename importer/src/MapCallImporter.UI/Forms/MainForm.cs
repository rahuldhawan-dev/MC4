using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MapCallImporter.Common;
using MapCallImporter.Importing;
using MapCallImporter.Library;
using MapCallImporter.Library.TypeRegistration;
using MapCallImporter.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallImporter.Forms
{
    public partial class MainForm : Form
    {
        #region Constants

        public struct ProcessButtonLabels
        {
            #region Constants

            public const string SELECT_FILE = "Select File...",
                VALIDATE_FILE = "Validate File",
                VALIDATING_FILE = "Validating File...",
                IMPORTING_FILE = "Importing File...",
                IMPORT = "Import";

            #endregion
        }

        public struct AppSettings
        {
            #region Constants

            public const string DEVELOPER_EMAIL = "DeveloperEmail";

            #endregion
        }

        #endregion

        #region Private Members

        protected Stopwatch _stopwatch;
        protected Timer _timer;
        protected decimal _lastPercentage;
        protected TimedExcelFileMappingResult _lastResult;

        #endregion

        #region Properties

        protected Button EmailDeveloper { get; set; }

        public bool ValidateReady { get; protected set; }
        public IContainer ObjectFactory { get; }

        #endregion

        #region Constructors

        public MainForm(IContainer container)
        {
            InitializeComponent();
            ObjectFactory = container;;
            _timer = new Timer {
                Interval = 1000
            };
            _timer.Tick += ETATimer_Tick;
            Text += " " + ObjectFactory.GetInstance<IAssemblyInfoService>().Version;
        }

        #endregion

        #region Private Methods

        private void DoButtonThing()
        {
            MainLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute,
                ValidateButton.Size.Height + ValidateButton.Margin.Top + ValidateButton.Margin.Bottom));
            EmailDeveloper = new Button { 
                Anchor = ValidateButton.Anchor,
                Location = ValidateButton.Location,
                Margin = ValidateButton.Margin,
                Name = "EmailDeveloper",
                Size = ValidateButton.Size,
                Text = "Email Developer",
                UseVisualStyleBackColor = true
            };
            EmailDeveloper.Click += EmailDeveloper_Click;
            MainLayoutPanel.Controls.Add(EmailDeveloper, 0, 4);
        }

        private void UndoButtonThing()
        {
            if (EmailDeveloper != null)
            {
                MainLayoutPanel.Controls.Remove(MainLayoutPanel);
                EmailDeveloper.Click -= EmailDeveloper_Click;
                EmailDeveloper.Dispose();
                EmailDeveloper = null;
                MainLayoutPanel.RowCount -= 1;
                MainLayoutPanel.RowStyles.RemoveAt(MainLayoutPanel.RowStyles.Count - 1);
            }
        }

        private void ClearLastRun()
        {
            Progress.Value = 0;
            Output.Text = StatusLabel.Text = string.Empty;
            UndoButtonThing();
        }

        private void ReadyForFileValidation()
        {
            _timer.Stop();
            ImportCancelPanel.Visible = false;
            SelectFilePanel.Visible = ValidateReady = ValidateButton.Enabled = true;
            ValidateButton.Text = ProcessButtonLabels.VALIDATE_FILE;
        }

        private void ReadyForImport()
        {
            _timer.Stop();
            ImportCancelPanel.Visible = true;
            SelectFilePanel.Visible = false;
        }

        private void BackToSelectFile()
        {
            SelectFilePanel.Visible = true;
            ValidateButton.Enabled = ImportCancelPanel.Visible = false;
            ValidateButton.Text = ProcessButtonLabels.SELECT_FILE;
            ValidateReady = false;
        }

        #endregion

        #region Event Handlers

        private void EmailDeveloper_Click(object sender, EventArgs e)
        {
            var info = ObjectFactory.GetInstance<IAssemblyInfoService>();
            var email = ConfigurationManager.AppSettings[AppSettings.DEVELOPER_EMAIL];
            var subject = Uri.EscapeDataString("MapCallImporter Error");
            var body = Uri.EscapeDataString($"MapCallImporter {info.Version} {Environment.NewLine}{Output.Text}");
            var process = $"mailto:{email}?subject={subject}&body={body}";

            Process.Start(new ProcessStartInfo(process));
        }

        private void FileHandler_ProgressChanged(object sender, ProgressAndStatusChangedArgs e)
        {
            if (e.ProgressPercentage > 100)
            {
                throw new InvalidOperationException("ProgressPercentage cannot be greater than 100.");
            }

            Output.Invoke(new Action(() => Output.AppendText($"{e.Status}{Environment.NewLine}")));
            Progress.Invoke(new Action(() => Progress.Value = (int)Math.Round(e.ProgressPercentage)));
            _lastPercentage = e.ProgressPercentage;
        }

        private void ETATimer_Tick(object sender, EventArgs e)
        {
            string display;

            if (_lastPercentage == 0)
            {
                display = string.Empty;
            }
            else
            {
                var elapsed = _stopwatch.ElapsedMilliseconds;
                var remaining = (long)Math.Round((elapsed / _lastPercentage) * (Progress.Maximum - _lastPercentage));
                var total = elapsed + remaining;
                display = $"{TimeSpan.FromMilliseconds(elapsed):hh\\:mm\\:ss} elapsed, {TimeSpan.FromMilliseconds(remaining):hh\\:mm\\:ss} remaining, {TimeSpan.FromMilliseconds(total):hh\\:mm\\:ss} total";
            }

            StatusLabel.Invoke(new Action(() => {
                if (StatusLabel.Text != display)
                {
                    StatusLabel.Text = display;
                }
            }));
        }

        private void BrowseFiles_Click(object sender, EventArgs e)
        {
            if (InputFileDialog.ShowDialog() == DialogResult.OK)
            {
                InputFile.Text = InputFileDialog.FileName;
            }
        }

        private void InputFile_TextChanged(object sender, EventArgs e)
        {
            var file = InputFile.Text;

            if (!File.Exists(file))
            {
                if (ValidateReady)
                {
                    BackToSelectFile();
                }
            }
            else if (!ValidateReady)
            {
                ReadyForFileValidation();
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            ReadyForFileValidation();
        }

        private void Validate_Click(object sender, EventArgs e)
        {
            if (!ValidationWorker.IsBusy)
            {
                ClearLastRun();
                _lastResult = null;
                ValidateButton.Text = ProcessButtonLabels.VALIDATING_FILE;
                ValidateButton.Enabled = false;
                ValidationWorker.RunWorkerAsync();
                _timer.Start();
            }
        }

        private void Import_Click(object sender, EventArgs e)
        {
            if (!ImportWorker.IsBusy)
            {
                ClearLastRun();
                Import.Text = ProcessButtonLabels.IMPORTING_FILE;
                Import.Enabled = Cancel.Enabled = false;
                ImportWorker.RunWorkerAsync();
                _timer.Start();
            }
        }

        private void ValidationWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            _stopwatch = Stopwatch.StartNew();
            Output.Invoke(new Action(() => Output.Text = string.Empty));
            var validationService = ObjectFactory.GetInstance<ExcelFileValidationService>();
            validationService.ProgressChanged += FileHandler_ProgressChanged;
            // get a more accurate time by starting the stopwatch after the file is open
            validationService.FileOpen += (s, arg) => _stopwatch = Stopwatch.StartNew();
            _lastResult = validationService.Handle(InputFile.Text);
            string message = null;
            _stopwatch.Stop();

            FileHandler_ProgressChanged(this, new ProgressAndStatusChangedArgs(Progress.Maximum, $"Finished processing in {TimeSpan.FromMilliseconds(_lastResult.ElapsedMiliseconds)}."));

            switch (_lastResult.Result)
            {
                case ExcelFileProcessingResult.FileAlreadyOpen:
                    message =
                        $"The file '{InputFile.Text}' is already open in another process.  Please close the file, or choose another.";
                    break;
                case ExcelFileProcessingResult.InvalidFileType:
                    message =
                        $"The file '{InputFile.Text}' does not appear to be a valid Microsoft Excel Open XML (Excel 2007+) document.  Please choose another file.";
                    break;
                case ExcelFileProcessingResult.InvalidFileContents:
                    Output.Invoke(new Action(() => Output.Text += string.Join(Environment.NewLine, _lastResult.Issues.ToArray())));
                    Invoke(new Action(DoButtonThing));
                    message =
                        $"The file '{InputFile.Text}' appears to have invalid data; see the output box for more information.";
                    break;
                case ExcelFileProcessingResult.OtherError:
                    Output.Invoke(new Action(() => Output.Text += string.Join(Environment.NewLine, _lastResult.Issues.ToArray())));
                    Invoke(new Action(DoButtonThing));
                    message =
                        $"There was an error processing the file '{InputFile.Text}'; see the output box for more information.";
                    break;
                case ExcelFileProcessingResult.CouldNotDetermineContentType:
                    message =
                        $"Could not determine type of file '{InputFile.Text}' by its column headers.  Please adjust the columns to match an expected file type, or choose another file.";
                    break;
                case ExcelFileProcessingResult.FileValid:
                    message = "File validation passed, ready for import.";
                    Invoke(new Action(ReadyForImport));
                    break;
            }

            MessageBox.Show(message);
        }

        private void ValidationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ValidateButton.Text = ProcessButtonLabels.VALIDATE_FILE;
            ValidateButton.Enabled = true;
        }

        private void ImportWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            _stopwatch = Stopwatch.StartNew();
            Output.Invoke(new Action(() => Output.Text = string.Empty));
            var importingService = ObjectFactory.GetInstance<ExcelFileImportingService>();
            importingService.ProgressChanged += FileHandler_ProgressChanged;
            _lastResult = importingService.Handle(_lastResult);
            string message = null;
            _stopwatch.Stop();

            FileHandler_ProgressChanged(this, new ProgressAndStatusChangedArgs(Progress.Maximum, $"Finished processing in {TimeSpan.FromMilliseconds(_lastResult.ElapsedMiliseconds)}."));

            switch (_lastResult.Result)
            {
                case ExcelFileProcessingResult.InvalidFileContents:
                    Output.Invoke(new Action(() => Output.Text += string.Join(Environment.NewLine, _lastResult.Issues.ToArray())));
                    Invoke(new Action(DoButtonThing));
                    message =
                        $"The file '{InputFile.Text}' appears to have invalid data; see the output box for more information.";
                    break;
                case ExcelFileProcessingResult.FileValid:
                    message = "Import was success.  Yay.";
                    Invoke(new Action(BackToSelectFile));
                    break;
                default:
                    throw new InvalidOperationException($"{nameof(ExcelFileImportingService)} should only ever return FileValid or InvalidFileContents.  Not sure how to handle {_lastResult.Result.ToString()}.");
            }

            MessageBox.Show(message);
        }

        private void ImportWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Import.Text = ProcessButtonLabels.IMPORT;
            Import.Enabled = Cancel.Enabled = true;
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<AboutBox>().Show();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion
    }
}
