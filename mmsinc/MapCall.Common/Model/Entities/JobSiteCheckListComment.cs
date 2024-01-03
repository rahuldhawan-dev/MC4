using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Migrations;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    // This is just a crappy text field that users enter comments into.
    // It's needed in order to keep a historical record of changes to this specific
    // field.
    [Serializable]
    public class JobSiteCheckListComment : IEntityWithCreationTracking<User>
    {
        #region Consts

        public struct StringLengths
        {
            public const int MAX_CREATED_BY =
                ModifyCommentsAndCrewMembersOnJobSiteCheckListsTableBug1648.COMMENTS_CREATED_BY_MAX_LENGTH;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual JobSiteCheckList JobSiteCheckList { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        public virtual string Comments { get; set; }

        public virtual User CreatedBy { get; set; }

        [View(DisplayFormat = CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE)]
        public virtual DateTime CreatedAt { get; set; }

        #endregion
    }
}
