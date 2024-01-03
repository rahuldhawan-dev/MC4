using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class JobSiteCheckListCommentMap : ClassMap<JobSiteCheckListComment>
    {
        public JobSiteCheckListCommentMap()
        {
            Id(x => x.Id);

            Map(x => x.Comments)
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
