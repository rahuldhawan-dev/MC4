using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SAPWorkOrderPurposeMap : EntityLookupMap<SAPWorkOrderPurpose>
    {
        public SAPWorkOrderPurposeMap()
        {
            Map(x => x.Code).Length(SAPWorkOrderPurpose.StringLengths.CODE).Not.Nullable();
            Map(x => x.CodeGroup).Length(SAPWorkOrderPurpose.StringLengths.CODE_GROUP).Not.Nullable();
        }
    }
}
