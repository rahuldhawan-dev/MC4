using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class TailgateTalkTopicMap : ClassMap<TailgateTalkTopic>
    {
        public const string TABLE_NAME = "tblTailgateTopics";

        public TailgateTalkTopicMap()
        {
            Table(TABLE_NAME);

            Id(x => x.Id, "TailgateTopicId");

            Map(x => x.IsActive);
            Map(x => x.Topic);
            Map(x => x.OrmReferenceNumber);
            Map(x => x.TargetDeliveryDate);
            Map(x => x.TopicLevel, "TopicLevelId").CustomType<TopicLevels>();

            References(x => x.CreatedBy).Nullable();

            References(x => x.Category, "TailgateCategory").Not.LazyLoad();
            References(x => x.Month, "Target_Month").Not.LazyLoad();

            HasMany(x => x.TailgateTalkTopicDocuments)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.TailgateTalkTopicNotes)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Videos)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }
    }
}
