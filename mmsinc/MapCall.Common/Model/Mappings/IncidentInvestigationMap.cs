using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class IncidentInvestigationMap : ClassMap<IncidentInvestigation>
    {
        #region Constructors

        public IncidentInvestigationMap()
        {
            Id(x => x.Id);

            References(x => x.Incident).Not.Nullable();
            References(x => x.IncidentInvestigationRootCauseFindingType).Not.Nullable();
            References(x => x.IncidentInvestigationRootCauseLevel1Type).Not.Nullable();
            References(x => x.IncidentInvestigationRootCauseLevel2Type).Not.Nullable();
            References(x => x.IncidentInvestigationRootCauseLevel3Type).Nullable(); // Not all level 2's have a level 3.

            HasManyToMany(x => x.RootCauseFindingPerformedByUsers)
               .Table("IncidentInvestigationsRootCauseFindingPerformedByUsers")
               .ParentKeyColumn("IncidentInvestigationId")
               .ChildKeyColumn("UserId");
        }

        #endregion
    }
}
