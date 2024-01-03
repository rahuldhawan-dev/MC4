using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    // This is just a crappy text field that users enter multiple crew members into.
    // It's needed in order to keep a historical record of changes to this specific
    // field. The class name is left as plural because it's not for an individual
    // crew member.
    [Serializable]
    public class JobSiteCheckListCrewMembers : IEntityWithCreationTracking<User>
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual JobSiteCheckList JobSiteCheckList { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        public virtual string CrewMembers { get; set; }

        public virtual User CreatedBy { get; set; }
        
        [View(DisplayFormat = CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE)]
        public virtual DateTime CreatedAt { get; set; }

        #endregion
    }
}
