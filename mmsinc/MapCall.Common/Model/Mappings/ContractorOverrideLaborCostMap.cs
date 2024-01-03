using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ContractorOverrideLaborCostMap : ClassMap<ContractorOverrideLaborCost>
    {
        #region Constructors

        public ContractorOverrideLaborCostMap()
        {
            Id(x => x.Id);

            Map(x => x.Cost).Nullable();
            Map(x => x.Percentage).Nullable();
            Map(x => x.EffectiveDate).Not.Nullable();

            References(x => x.OperatingCenter)
               .Not.Nullable();
            References(x => x.Contractor)
               .Not.Nullable();
            References(x => x.ContractorLaborCost)
               .Not.Nullable();
        }

        #endregion
    }
}
