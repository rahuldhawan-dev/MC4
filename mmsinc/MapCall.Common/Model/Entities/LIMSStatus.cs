using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    // Do not make this editable by users. It's a fragile lookup value.
    [Serializable]
    public class LIMSStatus : ReadOnlyEntityLookup
    {
        #region Structs

        public struct Indices
        {
            public const int NOT_READY = 1,
                             READY_TO_SEND = 2,
                             SENT_SUCCESSFULLY = 3,
                             SEND_FAILED = 4;
        }

        #endregion
    }
}
