using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class PremiseAreaCodeMap : EntityLookupMap<PremiseAreaCode>
    {
        #region Constructors

        public PremiseAreaCodeMap()
        {
            Map(x => x.SAPCode).Not.Nullable();
        }

        #endregion
    }
}
