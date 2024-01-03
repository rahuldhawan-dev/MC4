namespace MapCallImporter.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.InputFile = new System.Windows.Forms.TextBox();
            this.BrowseFiles = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Output = new System.Windows.Forms.RichTextBox();
            this.ValidateButton = new System.Windows.Forms.Button();
            this.InputFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.Cancel = new System.Windows.Forms.Button();
            this.Import = new System.Windows.Forms.Button();
            this.Progress = new System.Windows.Forms.ProgressBar();
            this.ValidationWorker = new System.ComponentModel.BackgroundWorker();
            this.ImportWorker = new System.ComponentModel.BackgroundWorker();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.TopPanel = new System.Windows.Forms.Panel();
            this.SelectFilePanel = new System.Windows.Forms.TableLayoutPanel();
            this.ImportCancelPanel = new System.Windows.Forms.TableLayoutPanel();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.MenuStrip.SuspendLayout();
            this.MainLayoutPanel.SuspendLayout();
            this.TopPanel.SuspendLayout();
            this.SelectFilePanel.SuspendLayout();
            this.ImportCancelPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // InputFile
            // 
            this.InputFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InputFile.Location = new System.Drawing.Point(38, 3);
            this.InputFile.Name = "InputFile";
            this.InputFile.Size = new System.Drawing.Size(298, 20);
            this.InputFile.TabIndex = 1;
            this.InputFile.TextChanged += new System.EventHandler(this.InputFile_TextChanged);
            // 
            // BrowseFiles
            // 
            this.BrowseFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseFiles.Location = new System.Drawing.Point(342, 3);
            this.BrowseFiles.Name = "BrowseFiles";
            this.BrowseFiles.Size = new System.Drawing.Size(69, 20);
            this.BrowseFiles.TabIndex = 2;
            this.BrowseFiles.Text = "Browse";
            this.BrowseFiles.UseVisualStyleBackColor = true;
            this.BrowseFiles.Click += new System.EventHandler(this.BrowseFiles_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "File:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Output
            // 
            this.Output.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Output.Location = new System.Drawing.Point(3, 133);
            this.Output.Name = "Output";
            this.Output.ReadOnly = true;
            this.Output.Size = new System.Drawing.Size(408, 300);
            this.Output.TabIndex = 3;
            this.Output.Text = "";
            // 
            // ValidateButton
            // 
            this.ValidateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectFilePanel.SetColumnSpan(this.ValidateButton, 3);
            this.ValidateButton.Enabled = false;
            this.ValidateButton.Location = new System.Drawing.Point(3, 29);
            this.ValidateButton.Name = "ValidateButton";
            this.ValidateButton.Size = new System.Drawing.Size(408, 53);
            this.ValidateButton.TabIndex = 4;
            this.ValidateButton.Text = "Select File...";
            this.ValidateButton.UseVisualStyleBackColor = true;
            this.ValidateButton.Click += new System.EventHandler(this.Validate_Click);
            // 
            // InputFileDialog
            // 
            this.InputFileDialog.Filter = "Microsoft Excel Open XML|*.xlsx";
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.Location = new System.Drawing.Point(210, 3);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(201, 79);
            this.Cancel.TabIndex = 1;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Import
            // 
            this.Import.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Import.Location = new System.Drawing.Point(3, 3);
            this.Import.Name = "Import";
            this.Import.Size = new System.Drawing.Size(201, 79);
            this.Import.TabIndex = 0;
            this.Import.Text = "Import";
            this.Import.UseVisualStyleBackColor = true;
            this.Import.Click += new System.EventHandler(this.Import_Click);
            // 
            // Progress
            // 
            this.Progress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Progress.Location = new System.Drawing.Point(3, 88);
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(408, 24);
            this.Progress.TabIndex = 4;
            // 
            // ValidationWorker
            // 
            this.ValidationWorker.WorkerReportsProgress = true;
            this.ValidationWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ValidationWorker_DoWork);
            this.ValidationWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ValidationWorker_RunWorkerCompleted);
            // 
            // ImportWorker
            // 
            this.ImportWorker.WorkerReportsProgress = true;
            this.ImportWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ImportWorker_DoWork);
            this.ImportWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ImportWorker_RunWorkerCompleted);
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(444, 24);
            this.MenuStrip.TabIndex = 5;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // MainLayoutPanel
            // 
            this.MainLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainLayoutPanel.ColumnCount = 1;
            this.MainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainLayoutPanel.Controls.Add(this.TopPanel, 0, 0);
            this.MainLayoutPanel.Controls.Add(this.Progress, 0, 1);
            this.MainLayoutPanel.Controls.Add(this.Output, 0, 3);
            this.MainLayoutPanel.Controls.Add(this.StatusLabel, 0, 2);
            this.MainLayoutPanel.Location = new System.Drawing.Point(15, 30);
            this.MainLayoutPanel.Margin = new System.Windows.Forms.Padding(6);
            this.MainLayoutPanel.Name = "MainLayoutPanel";
            this.MainLayoutPanel.RowCount = 4;
            this.MainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.MainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.MainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.MainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainLayoutPanel.Size = new System.Drawing.Size(414, 436);
            this.MainLayoutPanel.TabIndex = 6;
            // 
            // TopPanel
            // 
            this.TopPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TopPanel.Controls.Add(this.SelectFilePanel);
            this.TopPanel.Controls.Add(this.ImportCancelPanel);
            this.TopPanel.Location = new System.Drawing.Point(0, 0);
            this.TopPanel.Margin = new System.Windows.Forms.Padding(0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(414, 85);
            this.TopPanel.TabIndex = 8;
            // 
            // SelectFilePanel
            // 
            this.SelectFilePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectFilePanel.ColumnCount = 3;
            this.SelectFilePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.SelectFilePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SelectFilePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.SelectFilePanel.Controls.Add(this.ValidateButton, 0, 1);
            this.SelectFilePanel.Controls.Add(this.BrowseFiles, 2, 0);
            this.SelectFilePanel.Controls.Add(this.label1, 0, 0);
            this.SelectFilePanel.Controls.Add(this.InputFile, 1, 0);
            this.SelectFilePanel.Location = new System.Drawing.Point(0, 0);
            this.SelectFilePanel.Margin = new System.Windows.Forms.Padding(0);
            this.SelectFilePanel.Name = "SelectFilePanel";
            this.SelectFilePanel.RowCount = 2;
            this.SelectFilePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.SelectFilePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SelectFilePanel.Size = new System.Drawing.Size(414, 85);
            this.SelectFilePanel.TabIndex = 7;
            // 
            // ImportCancelPanel
            // 
            this.ImportCancelPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ImportCancelPanel.ColumnCount = 2;
            this.ImportCancelPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ImportCancelPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ImportCancelPanel.Controls.Add(this.Cancel, 1, 0);
            this.ImportCancelPanel.Controls.Add(this.Import, 0, 0);
            this.ImportCancelPanel.Location = new System.Drawing.Point(0, 0);
            this.ImportCancelPanel.Name = "ImportCancelPanel";
            this.ImportCancelPanel.RowCount = 1;
            this.ImportCancelPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ImportCancelPanel.Size = new System.Drawing.Size(414, 85);
            this.ImportCancelPanel.TabIndex = 7;
            // 
            // StatusLabel
            // 
            this.StatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StatusLabel.Location = new System.Drawing.Point(3, 115);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(408, 15);
            this.StatusLabel.TabIndex = 9;
            this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 481);
            this.Controls.Add(this.MainLayoutPanel);
            this.Controls.Add(this.MenuStrip);
            this.MainMenuStrip = this.MenuStrip;
            this.MinimumSize = new System.Drawing.Size(400, 500);
            this.Name = "MainForm";
            this.Text = "MapCall Importer";
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.MainLayoutPanel.ResumeLayout(false);
            this.TopPanel.ResumeLayout(false);
            this.SelectFilePanel.ResumeLayout(false);
            this.SelectFilePanel.PerformLayout();
            this.ImportCancelPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox InputFile;
        private System.Windows.Forms.Button BrowseFiles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox Output;
        private System.Windows.Forms.Button ValidateButton;
        private System.Windows.Forms.OpenFileDialog InputFileDialog;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button Import;
        private System.Windows.Forms.ProgressBar Progress;
        private System.ComponentModel.BackgroundWorker ValidationWorker;
        private System.ComponentModel.BackgroundWorker ImportWorker;
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel SelectFilePanel;
        private System.Windows.Forms.TableLayoutPanel MainLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel ImportCancelPanel;
        private System.Windows.Forms.Panel TopPanel;
        private System.Windows.Forms.Label StatusLabel;
    }
}

