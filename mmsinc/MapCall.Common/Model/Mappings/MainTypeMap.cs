using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class MainTypeMap : EntityLookupMap<MainType>
    {
        public MainTypeMap()
        {
            Map(x => x.SAPCode);
        }
    }
}
