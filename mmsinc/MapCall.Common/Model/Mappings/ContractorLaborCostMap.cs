using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class ContractorLaborCostMap : ClassMap<ContractorLaborCost>
    {
        public ContractorLaborCostMap()
        {
            Table("ContractorLaborCosts");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.StockNumber).Not.Nullable().Unique().Length(4);
            Map(x => x.Unit).Not.Nullable().Length(5);
            Map(x => x.JobDescription)
               .Not.Nullable().Length(AddContractorLaborCostPercentagesForBug2273.StringLengths.JOB_DESCRIPTION);
            Map(x => x.SubDescription).Nullable().Length(CreateContractorLaborCostsTable.StringLengths.SUB_DESCRIPTION);
            Map(x => x.Cost).Nullable().Precision(19).Scale(4);
            Map(x => x.Percentage).Nullable();

            HasMany(x => x.OverrideLaborCosts).KeyColumn("ContractorLaborCostId").Cascade.All().Not.LazyLoad();

            HasManyToMany(x => x.OperatingCenters)
               .Table("ContractorLaborCostsOperatingCenters")
               .ParentKeyColumn("ContractorLaborCostId")
               .ChildKeyColumn("OperatingCenterId")
               .Cascade.All();
        }
    }
}
