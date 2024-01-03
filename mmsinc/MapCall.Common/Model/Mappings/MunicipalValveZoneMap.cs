using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class MunicipalValveZoneMap : ClassMap<MunicipalValveZone>
    {
        public MunicipalValveZoneMap()
        {
            Table("TownValveZones");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("TownValveZoneId");
            References(x => x.OperatingCenter);
            References(x => x.Town);
            References(x => x.SmallValveZone).Column("SmallValveZone");
            References(x => x.LargeValveZone).Column("LargeValveZone");
        }
    }
}
