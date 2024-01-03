using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class FacilityProcessStepSubProcessMap : EntityLookupMap<FacilityProcessStepSubProcess>
    {
        public FacilityProcessStepSubProcessMap()
        {
            Table("FacilityProcessStepSubProcesses");
        }
    }
}
