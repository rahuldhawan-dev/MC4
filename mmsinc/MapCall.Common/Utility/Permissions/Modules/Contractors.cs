using MMSINC.Utilities.Permissions;

namespace MapCall.Common.Utility.Permissions.Modules
{
    public static class Contractors
    {
        public static readonly IModulePermissions General;
        public static readonly IModulePermissions Agreements;

        static Contractors()
        {
            ModulePermissionsHelper.InitType(typeof(Contractors));
        }
    }
}
