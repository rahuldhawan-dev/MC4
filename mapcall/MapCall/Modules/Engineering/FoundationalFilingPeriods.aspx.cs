using MMSINC.Utilities.Permissions;
using MapCall.Controls;

namespace MapCall.Modules.Engineering
{
    public partial class FoundationalFilingPeriods : TemplatedDetailsViewDataPageBase
    {
        protected override DetailsViewDataPageTemplate Template
        {
            get { return template; }
        }

        protected override IModulePermissions ModulePermissions
        {
            get { return Common.Utility.Permissions.Modules.FieldServices.Projects; }
        }
    }
}
