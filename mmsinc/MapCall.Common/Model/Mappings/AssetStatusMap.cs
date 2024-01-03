using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class AssetStatusMap : EntityLookupMap<AssetStatus>
    {
        protected override string IdName
        {
            get { return "AssetStatusID"; }
        }

        public AssetStatusMap()
        {
            Table("AssetStatuses");

            Id(x => x.Id, "AssetStatusID").GeneratedBy.Assigned();

            Map(x => x.IsUserAdminOnly)
               .Not.Nullable();
        }
    }
}
