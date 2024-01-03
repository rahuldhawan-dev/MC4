using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ContractorCompanyMap : ClassMap<ContractorCompany>
    {
        #region Constructors

        public ContractorCompanyMap()
        {
            Table("ContractorCompanies");
            Id(x => x.Id, "ContractorCompanyID");
            Map(x => x.Description, "Name");
        }

        #endregion
    }
}
