using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class CountyMap : ClassMap<County>
    {
        public const string TABLE_NAME = "Counties";

        public CountyMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id, "CountyId");
            Map(x => x.Name);
            Map(x => x.Enabled, "CountyEnabled");
            References(x => x.State);
            HasMany(x => x.Towns).KeyColumn("CountyID");
        }
    }
}
