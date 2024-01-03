using MMSINC.Utilities.Permissions;

namespace MapCall.Common.Utility.Permissions
{
    public class RoleApplication : IRoleApplication
    {
        #region Properties

        public int ApplicationId { get; set; }
        public string Name { get; set; }

        #endregion
    }
}
