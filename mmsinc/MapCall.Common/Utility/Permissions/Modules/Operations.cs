using MMSINC.Utilities.Permissions;

namespace MapCall.Common.Utility.Permissions.Modules
{
    public static class Operations
    {
#pragma warning disable 649
        public static readonly IModulePermissions DistributionOnly,
                                                  HealthAndSafety,
                                                  Training,
                                                  Management;
#pragma warning restore 649

        static Operations()
        {
            ModulePermissionsHelper.InitType(typeof(Operations));
        }
    }
}
