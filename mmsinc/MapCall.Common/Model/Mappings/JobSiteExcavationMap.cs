using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class JobSiteExcavationMap : ClassMap<JobSiteExcavation>
    {
        public JobSiteExcavationMap()
        {
            Id(x => x.Id);

            Map(x => x.CreatedBy)
               .Length(JobSiteExcavation.StringLengths.CREATED_BY)
               .Not.Nullable();
            Map(x => x.DepthInInches)
               .Not.Nullable();
            Map(x => x.ExcavationDate)
               .Not.Nullable();
            Map(x => x.LengthInFeet)
               .Not.Nullable();
            Map(x => x.WidthInFeet)
               .Not.Nullable();

            References(x => x.JobSiteCheckList)
               .Not.Nullable();
            References(x => x.LocationType, "JobSiteExcavationLocationTypeId")
               .Not.Nullable();
            References(x => x.SoilType, "JobSiteExcavationSoilTypeId")
               .Not.Nullable();
        }
    }
}
