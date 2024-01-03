using MMSINC.Utilities.Permissions;
using MapCall.Controls;

namespace MapCall.Reports.Contractors
{
    public partial class CurrentInventory : ReportPageBase
    {
        protected override IModulePermissions ModulePermissions
        {
            get { return Common.Utility.Permissions.Modules.FieldServices.WorkManagement; }
        }

        protected override DetailsViewDataPageTemplate Template
        {
            get { return template; }
        }
    }
}