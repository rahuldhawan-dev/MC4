using System.Web.UI.WebControls;
using MMSINC.Utilities.Permissions;

namespace MapCall.Reports.Data.Services
{
    public partial class ServicesRetiredData : ReportPage
    {
        #region Properties
        
        protected override IModulePermissions ModulePermissions
        {
            get
            {
                return Common.Utility.Permissions.Modules.FieldServices.Services;
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
    }
}


