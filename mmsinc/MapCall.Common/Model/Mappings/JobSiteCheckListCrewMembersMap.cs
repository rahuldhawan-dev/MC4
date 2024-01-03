using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class JobSiteCheckListCrewMembersMap : ClassMap<JobSiteCheckListCrewMembers>
    {
        public JobSiteCheckListCrewMembersMap()
        {
            Id(x => x.Id);

            Map(x => x.CrewMembers)
               .Not.Nullable();
            Map(x => x.CreatedAt)
               .Not.Nullable();

            References(x => x.JobSiteCheckList)
               .Not.Nullable();
            References(x => x.CreatedBy, "CreatedBy")
               .Not.Nullable();
        }
    }
}
