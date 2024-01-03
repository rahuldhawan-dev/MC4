using System.Web.UI.WebControls;
using MMSINC.Utilities.Permissions;
using MapCall.Controls;

namespace MapCall.Modules.Contractors
{
    public partial class ContractorInventory : TemplatedDetailsViewDataPageBase
    {
        #region Properties

        protected override IModulePermissions ModulePermissions
        {
            get { return Common.Utility.Permissions.Modules.FieldServices.WorkManagement; }
        }

        protected override DetailsViewDataPageTemplate Template
        {
            get { return template; }
        }

        #endregion

        #region Private Methods

        protected override void OnLoad(System.EventArgs e)
        {
            if (!IsPostBack && DetailsView.DataItemCount > 0 && DetailsView.CurrentMode!=DetailsViewMode.Insert)
                materials.OperatingCenterID =
                    ((System.Data.DataRowView)(((MMSINC.Controls.MvpDetailsView)(DetailsView)).DataItem))
                        .Row["OperatingCenterID"].ToString();
        }

        #endregion
    }
}