using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class StreetPrefixMap : EntityLookupMap<StreetPrefix>
    {
        #region Constants

        public const string TABLE_NAME = "StreetPrefixes";

        #endregion

        #region Constructors

        public StreetPrefixMap()
        {
            Table(TABLE_NAME);
        }

        #endregion
    }
}
