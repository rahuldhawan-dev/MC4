using MMSINC.Utilities.Permissions;

namespace MapCall.Common.Utility.Permissions.Modules
{
    public static class FieldServices
    {
#pragma warning disable 649
        public static readonly IModulePermissions Assets;
        public static readonly IModulePermissions DataLookups;
        public static readonly IModulePermissions Images;
        public static readonly IModulePermissions MeterChangeOuts;
        public static readonly IModulePermissions Meters;

        public static readonly IModulePermissions WorkManagement;

        //public static readonly IModulePermissions Hydrants;
        public static readonly IModulePermissions Services;

        //public static readonly IModulePermissions Valves;
        public static readonly IModulePermissions Projects;
        public static readonly IModulePermissions LocalApproval;
        public static readonly IModulePermissions AssetPlanningApproval;
        public static readonly IModulePermissions AssetPlanningEndorsement;
        public static readonly IModulePermissions CapitalPlanning;
#pragma warning restore 649

        static FieldServices()
        {
            ModulePermissionsHelper.InitType(typeof(FieldServices));
        }
    }
}
