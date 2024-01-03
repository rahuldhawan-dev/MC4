using System;
using System.Web.UI.WebControls;
using MMSINC.Utilities.Permissions;
using MapCall.Controls;

namespace MapCall.Modules.HR.Administrator
{
    public partial class EmailNotificationConfiguration : TemplatedDetailsViewDataPageBase
    {
        #region Properties

        protected override IModulePermissions ModulePermissions
        {
            get { return Common.Utility.Permissions.Modules.FieldServices.DataLookups; }
        }

        protected override DetailsViewDataPageTemplate Template
        {
            get { return template; }
        }

        #endregion

        private void SetFocus()
        {
            if (DetailsView != null && DetailsView.CurrentMode == DetailsViewMode.Insert)
            {
                var focus = DetailsView.FindControl<DropDownList>("ddlOperatingCenter");
                if (focus != null)
                    focus.Focus();
            }

        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            SetFocus();
            searchOperatingCenter.Focus();
        }
    }
}
