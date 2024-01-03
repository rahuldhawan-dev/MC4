using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ConfinedSpaceFormMethodOfCommunicationMap : EntityLookupMap<ConfinedSpaceFormMethodOfCommunication>
    {
        public ConfinedSpaceFormMethodOfCommunicationMap()
        {
            Table("ConfinedSpaceFormMethodsOfCommunication");
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
