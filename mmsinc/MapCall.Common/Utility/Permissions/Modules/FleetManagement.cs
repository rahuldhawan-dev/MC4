using MMSINC.Utilities.Permissions;

namespace MapCall.Common.Utility.Permissions.Modules
{
    public static class FleetManagement
    {
        public static readonly IModulePermissions General;

        static FleetManagement()
        {
            ModulePermissionsHelper.InitType(typeof(FleetManagement));
        }
    }
}
