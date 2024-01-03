using MMSINC.Utilities.Permissions;
using MapCall.Common.Utility.Permissions.Modules;
using MapCall.Controls;

namespace MapCall.Modules.HR.Employee
{
    public partial class ScheduleType : TemplatedDetailsViewDataPageBase
    {
        protected override IModulePermissions ModulePermissions
        {
            get
            {
                return HumanResources.Admin;
            }
        }

        protected override DetailsViewDataPageTemplate Template
        {
            get { return template; }
        }
    }
}