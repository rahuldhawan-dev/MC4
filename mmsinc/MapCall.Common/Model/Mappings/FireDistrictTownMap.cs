using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class FireDistrictTownMap : ClassMap<FireDistrictTown>
    {
        public const string TABLE_NAME = "FireDistrictsTowns";

        public FireDistrictTownMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id, "FireDistrictTownID");
            Map(x => x.IsDefault);
            References(x => x.State);
            References(x => x.Town);
            References(x => x.FireDistrict);
        }
    }
}
