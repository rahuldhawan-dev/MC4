using MMSINC.Utilities.Permissions;

namespace MapCall.Common.Utility.Permissions.Modules
{
    public static class Customer
    {
#pragma warning disable 649
        public static readonly IModulePermissions General;
#pragma warning restore 649

        static Customer()
        {
            ModulePermissionsHelper.InitType(typeof(Customer));
        }
    }
}
