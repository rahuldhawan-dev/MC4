using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class TimeZoneMap : ClassMap<TimeZone>
    {
        #region Constructors

        public TimeZoneMap()
        {
            Id(x => x.Id);

            Map(x => x.Zone);
            Map(x => x.Description);
            Map(x => x.UTCOffSet);
        }

        #endregion
    }
}
