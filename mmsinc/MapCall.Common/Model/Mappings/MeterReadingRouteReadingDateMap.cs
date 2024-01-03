using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class MeterReadingRouteReadingDateMap : ClassMap<MeterReadingRouteReadingDate>
    {
        #region Constructors

        public MeterReadingRouteReadingDateMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Not.Nullable();

            Map(x => x.ReadingDate).Not.Nullable();

            References(x => x.MeterReadingRoute).Not.Nullable();
        }

        #endregion
    }
}
