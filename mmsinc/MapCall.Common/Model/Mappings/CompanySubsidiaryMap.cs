using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class CompanySubsidiaryMap : EntityLookupMap<CompanySubsidiary>
    {
        public CompanySubsidiaryMap()
        {
            Table("CompanySubsidiaries");
        }
    }
}
