using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class StreetSuffixMap : EntityLookupMap<StreetSuffix>
    {
        #region Constants

        public const string TABLE_NAME = "StreetSuffixes";

        #endregion

        #region Constructors

        public StreetSuffixMap()
        {
            Table(TABLE_NAME);
        }

        #endregion
    }
}
