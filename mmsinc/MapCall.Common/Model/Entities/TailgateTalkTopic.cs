using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class TailgateTalkTopic
        : IEntityLookup,
            IThingWithDocuments,
            IThingWithNotes,
            IThingWithVideos,
            IEntityWithCreationUserTracking<User>
    {
        #region Private Members

        private TailgateTalkTopicDisplayItem _display;

        #endregion

        #region Properties

        #region Table Columns

        public virtual int Id { get; set; }
        public virtual string Topic { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual DateTime? TargetDeliveryDate { get; set; }
        public virtual string OrmReferenceNumber { get; set; }

        #endregion

        #region References

        public virtual TopicLevels? TopicLevel { get; set; }
        public virtual TailgateTopicCategory Category { get; set; }
        public virtual TailgateTopicMonth Month { get; set; }

        public virtual User CreatedBy { get; set; }
        public virtual IList<TailgateTalkTopicDocument> TailgateTalkTopicDocuments { get; set; }
        public virtual IList<TailgateTalkTopicNote> TailgateTalkTopicNotes { get; set; }

        #endregion

        #region Logical Columns

        [DoesNotExport]
        public virtual string TableName => TailgateTalkTopicMap.TABLE_NAME;

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return TailgateTalkTopicDocuments.Map(x => (IDocumentLink)x); }
        }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return TailgateTalkTopicNotes.Map(x => (INoteLink)x); }
        }

        public virtual string Description => (_display ?? (_display = new TailgateTalkTopicDisplayItem {
            Id = Id,
            Topic = Topic
        })).Display;

        public virtual IList<TailgateTalkTopicVideo> Videos { get; set; }

        public virtual IEnumerable<IVideoLink> LinkedVideos => Videos;

        #endregion

        #endregion

        #region Constructors

        public TailgateTalkTopic()
        {
            TailgateTalkTopicDocuments = new List<TailgateTalkTopicDocument>();
            TailgateTalkTopicNotes = new List<TailgateTalkTopicNote>();
        }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Description;
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }

    [Serializable]
    public class TailgateTalkTopicDisplayItem : DisplayItem<TailgateTalkTopic>
    {
        public string Topic { get; set; }
        public override string Display => $"{Id} - {Topic}";
    }

    public enum TopicLevels
    {
        State,
        Local
    }
}
