using System.Collections.Generic;
using System.Linq;

namespace WorkOrders.Model
{
    class SampleSiteRepository : WorkOrdersRepository<SampleSite>
    {
        public static bool IsPremisedLinkedToSampleSite(string premiseNumber)
        {
            return DataTable.Any(x => x.Premise != null && 
                                      x.Premise.PremiseNumber == premiseNumber);
        }

        public static SampleSite LinkedToSampleSite(string premiseNumber)
        {
            return DataTable.FirstOrDefault(e => e.Premise != null && 
                                                 e.Premise.PremiseNumber == premiseNumber);
        }
    }
}
