using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SewerAuthorityCodeMap : EntityLookupMap<SewerAuthorityCode>
    {
        #region Constants

        public const string TABLE_NAME = "SewerAuthorityCodes";

        #endregion

        #region Constructors

        public SewerAuthorityCodeMap()
        {
            Table(TABLE_NAME);
            Map(x => x.SAPCode).Not.Nullable().Length(10);
        }

        #endregion
    }
}
