using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class BappTeamIdeaMap : ClassMap<BappTeamIdea>
    {
        #region Constructors

        public BappTeamIdeaMap()
        {
            Table(CreateBAPPTeamIdeasTableForBug1999.TableNames.BAPP_TEAM_IDEAS);
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.BappTeam).Not.Nullable();
            References(x => x.Contact).Not.Nullable();
            References(x => x.SafetyImplementationCategory).Not.Nullable();

            Map(x => x.Description).Not.Nullable();
            Map(x => x.CreatedAt).Not.Nullable();

            HasMany(x => x.BappTeamIdeaNotes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.BappTeamIdeaDocuments).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }

        #endregion
    }
}
