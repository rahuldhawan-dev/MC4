using MMSINC.Utilities.Permissions;

namespace MapCall.Common.Utility.Permissions.Modules
{
    public static class Events
    {
        // Has to be done this way because we can't have a field called "Events". 
        public static readonly IModulePermissions EventsRole = new ModulePermissions("Events", "Events");

        //static Events()
        //{
        //    ModulePermissionsHelper.InitType(typeof(Events));
        //}
    }
}
