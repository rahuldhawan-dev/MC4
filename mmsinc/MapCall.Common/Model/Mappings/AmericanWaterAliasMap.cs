using System;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    [Serializable]
    public class AmericanWaterAliasMap : EntityLookupMap<AmericanWaterAlias>
    {
        public const string TABLE_NAME = "AmericanWaterAliases";

        public AmericanWaterAliasMap()
        {
            Table(TABLE_NAME);
        }
    }
}
