using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class MeterReadingRouteMap : EntityLookupMap<MeterReadingRoute>
    {
        #region Constructors

        public MeterReadingRouteMap()
        {
            Map(x => x.SAPCode).Not.Nullable();

            HasMany(x => x.MeterReadingDates).LazyLoad().Cascade.None().Inverse();
        }

        #endregion
    }
}
