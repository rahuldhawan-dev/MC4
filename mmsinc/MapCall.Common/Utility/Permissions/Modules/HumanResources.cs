using MMSINC.Utilities.Permissions;

namespace MapCall.Common.Utility.Permissions.Modules
{
    public static class HumanResources
    {
        public static readonly IModulePermissions Employee;
        public static readonly IModulePermissions Environmental;
        public static readonly IModulePermissions SampleSites;
        public static readonly IModulePermissions Admin;
        public static readonly IModulePermissions Proposals;
        public static readonly IModulePermissions Grievances;
        public static readonly IModulePermissions Contracts;
        public static readonly IModulePermissions Sections;
        public static readonly IModulePermissions PositionHistory;
        public static readonly IModulePermissions PositionPosting;
        public static readonly IModulePermissions Union;
        public static readonly IModulePermissions Facilities;
        public static readonly IModulePermissions Positions;
        public static readonly IModulePermissions StaffingHours;
        public static readonly IModulePermissions SystemDelivery;
        public static readonly IModulePermissions EmployeeLimited;

        static HumanResources()
        {
            ModulePermissionsHelper.InitType(typeof(HumanResources));
        }
    }
}
