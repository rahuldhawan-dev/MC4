using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using System;
using System.Collections.Generic;
using MMSINC.Data.ChangeTracking;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ActionItem : IEntityWithCreationTimeTracking
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual ActionItemType Type { get; set; }
        public virtual string NotListedType { get; set; }
        public virtual User ResponsibleOwner { get; set; }
        public virtual string Note { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime TargetedCompletionDate { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? DateCompleted { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime CreatedAt { get; set; }

        public virtual int LinkedId { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DataType DataType { get; set; }

        #endregion
    }

    public interface IThingWithActionItems : IThingWithOperatingCenter
    {
        #region Abstract Properties

        IList<IActionItemLink> LinkedActionItems { get; }
        string TableName { get; }

        #endregion
    }

    public interface IActionItemLink
    {
        #region Abstract Properties

        int Id { get; }
        ActionItem ActionItem { get; set; }
        DataType DataType { get; set; }
        int LinkedId { get; }

        #endregion
    }
}
