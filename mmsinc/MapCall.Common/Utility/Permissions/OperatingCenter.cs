using MMSINC.Utilities.Permissions;

namespace MapCall.Common.Utility.Permissions
{
    public class OperatingCenter : IOperatingCenter
    {
        #region Properties

        public int OperatingCenterId { get; set; }
        public string OperatingCenterCode { get; set; }

        #endregion
    }
}
