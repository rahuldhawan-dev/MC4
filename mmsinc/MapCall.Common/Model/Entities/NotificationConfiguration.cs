using System;
using System.Collections.Generic;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class NotificationConfiguration : IEntity, IThingWithContact
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual Contact Contact { get; set; }

        /// <summary>
        /// If the OperatingCenter value is null, then this configuration applies to all operating centers.
        /// </summary>
        public virtual OperatingCenter OperatingCenter { get; set; }
        [View("Notification Purposes")]
        public virtual IList<NotificationPurpose> NotificationPurposes { get; set; } = new List<NotificationPurpose>();

        #region Logical

        public virtual bool AppliesToAllOperatingCenters => OperatingCenter == null;

        #endregion

        #endregion
    }
}
