using System.Web.UI;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class FacilitySubAreaMap : ClassMap<FacilitySubArea>
    {
        public const string TABLE_NAME = "FacilitySubAreas";

        public FacilitySubAreaMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Description)
               .Length(FacilitySubArea.MAX_DESCRIPTION_LENGTH)
               .Not.Nullable();
            References(x => x.Area).Not.Nullable();
        }
    }
}
