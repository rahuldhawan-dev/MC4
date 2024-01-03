using MMSINC.Utilities.Permissions;

namespace MapCall.Common.Utility.Permissions
{
    public class RoleModule : IRoleModule
    {
        #region Properties

        public int ApplicationId { get; set; }
        public int ModuleId { get; set; }
        public string Name { get; set; }

        #endregion
    }
}
