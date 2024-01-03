using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class CommunicationTypeMap : EntityLookupMap<CommunicationType>
    {
        public CommunicationTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
