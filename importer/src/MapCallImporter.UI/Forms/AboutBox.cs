using System;
using System.Windows.Forms;
using MapCallImporter.Library;

namespace MapCallImporter.Forms
{
    partial class AboutBox : Form
    {
        #region Constructors

        public AboutBox(IAssemblyInfoService assemblyInfo)
        {
            InitializeComponent();
            this.Text = $"About {assemblyInfo.Title}";
            this.labelProductName.Text = assemblyInfo.Product;
            this.labelVersion.Text = $"Version {assemblyInfo.Version}";
            this.labelCopyright.Text = assemblyInfo.Copyright;
            this.labelCompanyName.Text = assemblyInfo.Company;
            this.textBoxDescription.Text = assemblyInfo.Description;
        }

        #endregion

        #region Event Handlers

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}
