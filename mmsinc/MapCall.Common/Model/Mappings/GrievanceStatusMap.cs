using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class GrievanceStatusMap : ClassMap<GrievanceStatus>
    {
        public GrievanceStatusMap()
        {
            Table(MoveGrievanceCategorizationsAndStatusesToTheirOwnTable.TableNames.GRIEVANCE_STATUSES);

            Id(x => x.Id);

            Map(x => x.Description);
        }
    }
}
