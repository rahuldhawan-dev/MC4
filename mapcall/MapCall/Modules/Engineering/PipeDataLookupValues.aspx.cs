using MapCall.Controls;
using MMSINC.Utilities.Permissions;

namespace MapCall.Modules.Engineering
{
    public partial class PipeDataLookupValues : TemplatedDetailsViewDataPageBase
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
