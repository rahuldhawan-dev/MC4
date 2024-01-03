using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20191126085408519), Tags("Production")]
    public class MC1801AddNewColumnsToChemicals : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("MatterStates", "Gas", "Liquid", "Solid");
            this.CreateLookupTableWithValues("PhysicalHazards",
                "Explosive",
                "Flammable (gases, aerosols, liquids, or solids)",
                "Oxidizer (liguid, solid, or gas)",
                "Self-active",
                "Pyrophoric (liguid or solid)",
                "Pyrophoric gas",
                "Self-heating",
                "Organic peroxide",
                "Corrosive to metal",
                "Gas under pressure (compressed gas)",
                "In contact with water emits flammable gas",
                "Physical hazard not otherwise classified",
                "No physical hazards per SDS");
            this.CreateLookupTableWithValues("HealthHazards", 100,
                "Acute toxicity (any route of exposure)",
                "Skin corrosion or irritation",
                "Serious eye damage or eye irritation",
                "Respiratory or skin sensitization",
                "Germ cell mutagenicity",
                "Carcinogenicity",
                "Reproductive toxicity",
                "Specific target organ toxicity(single or repeated exposure)",
                "Aspiration hazard",
                "Simple asphyxiant",
                "Heath hazard not otherwise classified",
                "No heath hazards per SDS");
            Alter.Table("Chemicals")
                 .AddColumn("SubNumber").AsInt32().Nullable()
                 .AddColumn("DepartmentOfTransportationNumber").AsInt32().Nullable()
                 .AddColumn("IsPure").AsBoolean().Nullable()
                 .AddColumn("TradeSecret").AsBoolean().Nullable()
                 .AddColumn("EmergencyPlanningCommunityRightToKnowActOnly").AsBoolean().Nullable()
                 .AddForeignKeyColumn("MatterStateId", "MatterStates").Nullable();
            // Physical Hazards
            Create.Table("ChemicalsPhysicalHazards")
                  .WithForeignKeyColumn("ChemicalId", "Chemicals", nullable: false)
                  .WithForeignKeyColumn("PhysicalHazardId", "PhysicalHazards", nullable: false);
            // Health Hazards
            Create.Table("ChemicalsHealthHazards")
                  .WithForeignKeyColumn("ChemicalId", "Chemicals", nullable: false)
                  .WithForeignKeyColumn("HealthHazardId", "HealthHazards", nullable: false);
        }

        public override void Down()
        {
            Delete.Table("ChemicalsHealthHazards");
            Delete.Table("ChemicalsPhysicalHazards");
            Delete.ForeignKeyColumn("Chemicals", "MatterStateId", "MatterStates");
            Delete.Column("SubNumber").FromTable("Chemicals");
            Delete.Column("DepartmentOfTransportationNumber").FromTable("Chemicals");
            Delete.Column("IsPure").FromTable("Chemicals");
            Delete.Column("TradeSecret").FromTable("Chemicals");
            Delete.Column("EmergencyPlanningCommunityRightToKnowActOnly").FromTable("Chemicals");
            Delete.Table("HealthHazards");
            Delete.Table("PhysicalHazards");
            Delete.Table("MatterStates");
        }
    }
}
