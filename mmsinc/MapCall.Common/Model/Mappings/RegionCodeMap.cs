using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class RegionCodeMap : EntityLookupMap<RegionCode>
    {
        #region Constructors

        public RegionCodeMap()
        {
            Map(x => x.SAPCode).Not.Nullable();

            References(x => x.State).Nullable();
        }

        #endregion
    }
}
