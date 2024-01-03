using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SAPProjectTypeMap : EntityLookupMap<SAPProjectType>
    {
        public SAPProjectTypeMap()
        {
            Map(x => x.SAPCode);
        }
    }
}
