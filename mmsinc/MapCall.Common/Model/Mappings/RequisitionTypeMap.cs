using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class RequisitionTypeMap : EntityLookupMap<RequisitionType>
    {
        #region Properties

        protected override string IdName => "RequisitionTypeID";

        #endregion
    }
}
