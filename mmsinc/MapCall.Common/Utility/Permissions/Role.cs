using System.Diagnostics;
using MMSINC.Utilities.Permissions;

namespace MapCall.Common.Utility.Permissions
{
    [DebuggerDisplay("{Application} : {Module} : {OperatingCenter} : {Action}")]
    public class Role : IRole
    {
        #region Properties

        public int UserId { get; set; }

        public string OperatingCenter { get; set; }
        public int OperatingCenterId { get; set; }

        public string Application { get; set; }
        public int ApplicationId { get; set; }

        public string Module { get; set; }
        public int ModuleId { get; set; }

        public string Action { get; set; }
        public int ActionId { get; set; }

        #endregion
    }
}
