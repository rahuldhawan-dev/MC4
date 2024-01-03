using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FluentNHibernate.Utils;
using MMSINC.Data.ChangeTracking;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PlanReview : IEntityWithCreationTracking<User>
    {
        #region Constants

        public struct StringLengths
        {
            public const int REVIEW_CHANGE_NOTES = 255;
        }

        public struct Display
        {
            public const string REVIEW_CHANGE_NOTES_HINT = "Notes are limited to 255 characters";
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual EmergencyResponsePlan Plan { get; set; }
        [View(MMSINC.Utilities.FormatStyle.Date)]
        public virtual DateTime ReviewDate { get; set; }
        public virtual Employee ReviewedBy { get; set; }
        [Multiline, Description(Display.REVIEW_CHANGE_NOTES_HINT)]
        public virtual string ReviewChangeNotes { get; set; }
        [View(MMSINC.Utilities.FormatStyle.Date)]
        public virtual DateTime NextReviewDate { get; set; }
        public virtual User CreatedBy { get; set; }
        [View(MMSINC.Utilities.FormatStyle.Date)]
        public virtual DateTime CreatedAt { get; set; }

        #endregion
    }
}
