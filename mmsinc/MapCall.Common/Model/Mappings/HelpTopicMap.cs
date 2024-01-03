using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class HelpTopicMap : ClassMap<HelpTopic>
    {
        public HelpTopicMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("HelpTopicID");

            References(x => x.Category).Column("Category").Nullable();
            References(x => x.SubjectMatter).Column("HelpTopicSubjectMatterId").Nullable();

            Map(x => x.Title).Column("Topic").Not.Nullable().Length(255);
            Map(x => x.Description);

            HasMany(x => x.HelpTopicDocuments).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.HelpTopicNotes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }
    }
}
