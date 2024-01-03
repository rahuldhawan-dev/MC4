using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class PremiseDistrictMap : EntityLookupMap<PremiseDistrict>
    {
        #region Constructors

        public PremiseDistrictMap()
        {
            Map(x => x.SAPCode).Not.Nullable();
        }

        #endregion
    }
}
