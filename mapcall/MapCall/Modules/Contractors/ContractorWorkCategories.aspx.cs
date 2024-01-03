using MMSINC.Utilities.Permissions;
using MapCall.Controls;

namespace MapCall.Modules.Contractors
{
    public partial class ContractorWorkCategories : TemplatedDetailsViewDataPageBase
    {
        protected override DetailsViewDataPageTemplate Template
        {
            get { return template; }
        }

        protected override IModulePermissions ModulePermissions
        {
            get { return Common.Utility.Permissions.Modules.Contractors.General; }
        }
    }
}
