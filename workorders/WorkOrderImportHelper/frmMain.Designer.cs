namespace WorkOrderImportHelper
{
    public partial class frmMain
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
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.lblCurrentWorkOrderID = new System.Windows.Forms.Label();
            this.txtCurrentWorkOrderID = new System.Windows.Forms.TextBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(12, 89);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(361, 216);
            this.txtOutput.TabIndex = 2;
            // 
            // lblCurrentWorkOrderID
            // 
            this.lblCurrentWorkOrderID.AutoSize = true;
            this.lblCurrentWorkOrderID.Location = new System.Drawing.Point(12, 55);
            this.lblCurrentWorkOrderID.Name = "lblCurrentWorkOrderID";
            this.lblCurrentWorkOrderID.Size = new System.Drawing.Size(116, 13);
            this.lblCurrentWorkOrderID.TabIndex = 3;
            this.lblCurrentWorkOrderID.Text = "Current Work Order ID:";
            // 
            // txtCurrentWorkOrderID
            // 
            this.txtCurrentWorkOrderID.Location = new System.Drawing.Point(134, 52);
            this.txtCurrentWorkOrderID.Name = "txtCurrentWorkOrderID";
            this.txtCurrentWorkOrderID.Size = new System.Drawing.Size(66, 20);
            this.txtCurrentWorkOrderID.TabIndex = 4;
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(12, 12);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(108, 27);
            this.btnImport.TabIndex = 5;
            this.btnImport.Text = "Run Data Task";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 315);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.txtCurrentWorkOrderID);
            this.Controls.Add(this.lblCurrentWorkOrderID);
            this.Controls.Add(this.txtOutput);
            this.Name = "frmMain";
            this.Text = "Work Order Import Helper";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Label lblCurrentWorkOrderID;
        private System.Windows.Forms.TextBox txtCurrentWorkOrderID;
        private System.Windows.Forms.Button btnImport;
    }
}

