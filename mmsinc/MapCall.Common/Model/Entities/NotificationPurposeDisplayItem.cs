using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class NotificationPurposeDisplayItem : DisplayItem<NotificationPurpose>
    {
        #region Properties

        public string Purpose { get; set; }

        #endregion

        #region Public Methods

        public override string Display => Purpose;

        #endregion
    }
}
