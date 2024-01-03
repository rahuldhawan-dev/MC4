using MMSINC.Utilities.Permissions;

namespace MapCall.Common.Utility.Permissions.Modules
{
    public static class BPU
    {
#pragma warning disable 649
        public static readonly IModulePermissions General;
#pragma warning restore 649

        static BPU()
        {
            ModulePermissionsHelper.InitType(typeof(BPU));
        }
    }
}
