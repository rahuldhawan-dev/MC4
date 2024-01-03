using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ChemicalMap : ClassMap<Chemical>
    {
        public ChemicalMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();
            References(x => x.ChemicalType).Nullable();
            Map(x => x.Name).Length(61).Not.Nullable();
            Map(x => x.PartNumber).Length(50).Not.Nullable();
            Map(x => x.PricePerPoundWet).Nullable();
            Map(x => x.WetPoundsPerGal).Nullable();
            References(x => x.PackagingType).Nullable();
            Map(x => x.PackagingQuantities).Nullable();
            Map(x => x.PackagingUnits).Length(50).Nullable();
            Map(x => x.ChemicalSymbol).Length(50).Nullable();
            Map(x => x.Appearance).Nullable();
            Map(x => x.ChemicalConcentrationLiquid).Nullable();
            Map(x => x.ConcentrationLbsPerGal).Column("ConcentrationLBSPerGal").Nullable();
            Map(x => x.SpecificGravityMin).Nullable();
            Map(x => x.SpecificGravityMax).Nullable();
            Map(x => x.RatioResidualProduction).Nullable();
            Map(x => x.CasNumber).Column("CASNumber").Length(50).Nullable();
            Map(x => x.SdsHyperlink).Column("SDSHyperlink").Length(2048).Nullable();

            Map(x => x.SubNumber).Nullable();
            Map(x => x.DepartmentOfTransportationNumber).Nullable();
            Map(x => x.IsPure).Nullable();
            Map(x => x.TradeSecret).Nullable();
            Map(x => x.EmergencyPlanningCommunityRightToKnowActOnly).Nullable();
            Map(x => x.ExtremelyHazardousChemical).Not.Nullable().Default("false");
            HasManyToMany(x => x.PhysicalHazards)
               .Table("ChemicalsPhysicalHazards")
               .ParentKeyColumn("ChemicalId")
               .ChildKeyColumn("PhysicalHazardId")
               .Cascade.All();
            HasManyToMany(x => x.HealthHazards)
               .Table("ChemicalsHealthHazards")
               .ParentKeyColumn("ChemicalId")
               .ChildKeyColumn("HealthHazardId")
               .Cascade.All();
            HasManyToMany(x => x.ChemicalStates)
               .Table("ChemicalsStatesOfMatter")
               .ParentKeyColumn("ChemicalId")
               .ChildKeyColumn("StateOfMatterId")
               .Cascade.All();
            HasMany(x => x.Documents)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }
    }
}
