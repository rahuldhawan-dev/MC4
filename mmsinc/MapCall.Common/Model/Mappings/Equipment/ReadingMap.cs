using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ReadingMap : ClassMap<Reading>
    {
        public ReadingMap()
        {
            Id(x => x.Id, "ReadingID");

            Map(x => x.DateTimeStamp)
               .Not.Nullable();
            Map(x => x.RawData)
               .Nullable();
            Map(x => x.ScaledData)
               .Not.Nullable();
            Map(x => x.CheckSum)
               .Nullable();
            Map(x => x.Interpolate)
               .Nullable();

            References(x => x.Sensor);
        }
    }
}
