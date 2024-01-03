using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class AccountingTypeMap : EntityLookupMap<AccountingType>
    {
        protected override string IdName => "AccountingTypeID";

        public AccountingTypeMap()
        {
            Id(x => x.Id, IdName)
               .Not.Nullable()
               .GeneratedBy.Assigned();
        }
    }
}
