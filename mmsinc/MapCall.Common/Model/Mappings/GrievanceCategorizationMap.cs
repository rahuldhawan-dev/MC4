using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class GrievanceCategorizationMap : ClassMap<GrievanceCategorization>
    {
        public GrievanceCategorizationMap()
        {
            Id(x => x.Id);

            Map(x => x.Description);
            References(x => x.GrievanceCategory, "GrievanceCategoryId");
        }
    }
}
