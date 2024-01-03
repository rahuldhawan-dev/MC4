using MMSINC.Utilities.Permissions;
using MapCall.Controls;

namespace MapCall.Modules.Contractors
{
    public partial class ContractorInsuranceMinimumRequirements : LookUpDataPageBase
    {
        protected override LookupControl LookupControl
        {
            get { return lookup; }
        }

        protected override IModulePermissions ModulePermissions
        {
            get { return Common.Utility.Permissions.Modules.Contractors.General; }
        }
    }
}
