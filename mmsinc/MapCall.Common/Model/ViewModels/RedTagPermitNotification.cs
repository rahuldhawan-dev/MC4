using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.ViewModels
{
    public class RedTagPermitNotification
    {
        #region Constants

        public const string IOC_CONTACT_NUMBER = "1-866-801-1123";

        #endregion

        #region Properties

        public RedTagPermit RedTagPermit { get; set; }
        public string RecordUrl { get; set; }
        public string ProductionWorkOrderRecordUrl { get; set; }

        #endregion
    }
}
