using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ValveControlMap : EntityLookupMap<ValveControl>
    {
        public ValveControlMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
