using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class HydrantTagStatusMap : EntityLookupMap<HydrantTagStatus>
    {
        protected override string IdName => "HydrantTagStatusID";

        public HydrantTagStatusMap()
        {
            Table(UpdateHydrantsForBug2223.TableNames.HYDRANT_TAG_STATUSES);
        }
    }
}
