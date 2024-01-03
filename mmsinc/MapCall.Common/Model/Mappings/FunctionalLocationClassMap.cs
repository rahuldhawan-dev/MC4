using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class FunctionalLocationClassMap : EntityLookupMap<FunctionalLocationClass>
    {
        public const string TABLE_NAME = "FunctionalLocationClasses";

        public FunctionalLocationClassMap()
        {
            Table(TABLE_NAME);
        }
    }
}
