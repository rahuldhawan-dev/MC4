using System;
using System.Collections.Generic;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ServiceFlush : IEntityWithCreationUserTracking<User>
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual Service Service { get; set; }
        public virtual ServiceFlushFlushType FlushType { get; set; }
        public virtual ServiceFlushSampleType SampleType { get; set; }
        public virtual ServiceFlushSampleStatus SampleStatus { get; set; }
        public virtual ServiceFlushSampleTakenByType TakenBy { get; set; }
        public virtual ServiceFlushPremiseContactMethod ContactMethod { get; set; }
        public virtual ServiceFlushReplacementType ReplacementType { get; set; }
        public virtual User CreatedBy { get; set; }

        [Multiline]
        public virtual string FlushingNotes { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime SampleDate { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? PremiseContactDate { get; set; }
        public virtual bool? NotifiedCustomerServiceCenter { get; set; }
        public virtual IList<Service> Services { get; set; }

        /// <summary>
        /// This property's a flag for the scheduler. Notifications should only be sent out once.
        /// </summary>
        public virtual bool HasSentNotification { get; set; }

        [View("Sample Result"), BoolFormat("Passed", "Failed", "n/a")]
        public virtual bool? SampleResultPassed { get; set; }

        [View("Sample Id")]
        public virtual int? SampleId { get; set; }

        #endregion
    }
}
