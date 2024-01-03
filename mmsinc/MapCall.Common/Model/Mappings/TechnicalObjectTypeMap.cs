using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class TechnicalObjectTypeMap : EntityLookupMap<TechnicalObjectType>
    {
        public TechnicalObjectTypeMap()
        {
            Map(x => x.SAPCode);
        }
    }
}
