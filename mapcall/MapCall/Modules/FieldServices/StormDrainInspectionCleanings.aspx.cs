using MMSINC.Utilities.Permissions;
using mod = MapCall.Common.Utility.Permissions.Modules;
using MapCall.Controls;

namespace MapCall.Modules.FieldServices
{
    public partial class StormDrainInspectionCleanings : TemplatedDetailsViewDataPageBase
    {
        protected override DetailsViewDataPageTemplate Template
        {
            get { return template; }
        }

        protected override IModulePermissions ModulePermissions
        {
            get { return mod.FieldServices.Assets; }
        }
    }
}