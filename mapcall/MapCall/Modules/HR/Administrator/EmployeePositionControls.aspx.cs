using System;
using MMSINC.DataPages;
using MMSINC.Utilities.Permissions;
using MapCall.Common.Utility.Permissions.Modules;
using MapCall.Controls;

namespace MapCall.Modules.HR.Administrator
{
    public partial class EmployeePositionControls : TemplatedDetailsViewDataPageBase
    {
        protected override DetailsViewDataPageTemplate Template
        {
            get { return template; }
        }

        protected override IModulePermissions ModulePermissions
        {
            get { return HumanResources.Employee; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (PageMode == PageModes.RecordInsert 
                || PageMode == PageModes.RecordUpdate)
            {
                bfDescription.Visible = false;
                bfDepartment.Visible = false;
                bfArea.Visible = false;
            }
        }
    }
}