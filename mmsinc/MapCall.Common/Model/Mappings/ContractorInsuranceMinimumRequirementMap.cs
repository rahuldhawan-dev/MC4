using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ContractorInsuranceMinimumRequirementMap : ClassMap<ContractorInsuranceMinimumRequirement>
    {
        #region Constructors

        public ContractorInsuranceMinimumRequirementMap()
        {
            Id(x => x.Id, "ContractorInsuranceMinimumRequirementID");

            Map(x => x.Description);
        }

        #endregion
    }
}
