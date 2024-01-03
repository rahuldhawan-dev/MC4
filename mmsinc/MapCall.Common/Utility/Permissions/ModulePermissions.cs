using MMSINC.Utilities.Permissions;

namespace MapCall.Common.Utility.Permissions
{
    public class ModulePermissions : IModulePermissions
    {
        public string Application { get; protected set; }
        public string Module { get; protected set; }

        public ModulePermissions(string application, string module)
        {
            Application = application;
            Module = module;
        }
    }
}
