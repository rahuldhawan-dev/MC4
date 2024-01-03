using System;
using System.Web.UI.WebControls;
using MMSINC.Controls.GridViewHelper;
using MMSINC.Utilities.Permissions;
using MapCall.Reports;

namespace MapCall.Modules.BusinessPerformance
{
    public partial class InitiativeStepsSummaryReport : ReportPage
    {
        #region Properties

        protected override IModulePermissions ModulePermissions
        {
            get
            {
                return Common.Utility.Permissions.Modules.BusinessPerformance.General;
            }
        }

        public override Label PermissionLabel
        {
            get { return lblPermissionErrors; }
        }

        public override GridView GridView
        {
            get { return GridView1; }
        }

        public override SqlDataSource DataSource
        {
            get { return SqlDataSource1; }
        }

        public override Panel SearchPanel
        {
            get { return pnlSearch; }
        }

        public override Panel ResultsPanel
        {
            get { return pnlResults; }
        }

        public override Label InformationLabel
        {
            get { return lblInformation; }
        }

        #endregion

        #region Private Methods

        private void ApplyGrouping()
        {
            var helper = new GridViewHelper(GridView1, false, SortDirection.Ascending);
            var groups = new[] { "Initiative_Grouping", "Initiative_Focus", "Implementation_Horizon", "Initiative_Summary" };
            helper.RegisterGroup(groups, true, true);
            helper.ApplyGroupSort();
        }

        #endregion

        #region Event Handlers

        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
            GridView1.DataSource = SqlDataSource1;
            ApplyGrouping();
        }
        
        protected void GridView1_Sorting(object sender, EventArgs e)
        {

        }

        #endregion
    }
}
