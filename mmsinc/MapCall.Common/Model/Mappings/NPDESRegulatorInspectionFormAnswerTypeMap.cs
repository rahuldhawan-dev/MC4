using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class GateStatusAnswerTypeMap : EntityLookupMap<GateStatusAnswerType>
    {
        public GateStatusAnswerTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            ReadOnly();
        }
    }
}
