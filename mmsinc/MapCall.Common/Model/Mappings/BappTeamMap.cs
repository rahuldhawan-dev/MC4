using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class BappTeamMap : ClassMap<BappTeam>
    {
        public BappTeamMap()
        {
            Table("BAPPTeams");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Description).Not.Nullable().Length(50);

            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.CreatedBy).Not.Nullable();
        }
    }
}
