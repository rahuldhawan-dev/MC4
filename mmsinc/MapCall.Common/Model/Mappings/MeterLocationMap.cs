using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class MeterLocationMap : EntityLookupMap<MeterLocation>
    {
        #region Constructors

        public MeterLocationMap()
        {
            Id(x => x.Id, "MeterLocationId");

            Map(x => x.SAPCode, "Code").Not.Nullable();
        }

        #endregion
    }
}
