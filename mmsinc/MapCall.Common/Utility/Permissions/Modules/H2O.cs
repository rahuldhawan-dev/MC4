using MMSINC.Utilities.Permissions;

namespace MapCall.Common.Utility.Permissions.Modules
{
    // Needed because Wordify doesn't count numbers
    // as part of camel case. It'd return the auto-generated
    // name as "H2 O" instead of "H2O"
    [RoleApplicationName("H2O")]
    public static class H2O
    {
        public static readonly IModulePermissions General;

        static H2O()
        {
            ModulePermissionsHelper.InitType(typeof(H2O));
        }
    }
}
