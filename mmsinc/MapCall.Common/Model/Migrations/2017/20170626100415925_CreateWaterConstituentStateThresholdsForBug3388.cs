using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170626100415925), Tags("Production")]
    public class CreateWaterConstituentStateLimitsForBug3388 : Migration
    {
        public override void Up()
        {
            Create.Table("WaterConstituentStateLimits")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("WaterConstituentId", "WaterConstituents").NotNullable()
                  .WithForeignKeyColumn("StateId", "States", "StateId").NotNullable()
                  .WithColumn("Description").AsString(255).Nullable()
                  .WithColumn("Agency").AsString(255).Nullable()
                  .WithColumn("Min").AsFloat().Nullable()
                  .WithColumn("Max").AsFloat().Nullable()
                  .WithColumn("MCL").AsFloat().Nullable()
                  .WithColumn("MCLG").AsFloat().Nullable()
                  .WithColumn("SMCL").AsFloat().Nullable()
                  .WithColumn("ActionLimit").AsString(255).Nullable()
                  .WithForeignKeyColumn("UnitOfMeasureId", "UnitsOfMeasure", "UnitOfMeasureId")
                  .WithColumn("Regulation").AsString(255).Nullable()
                  .WithColumn("StateDEPAnalyteCode").AsInt32().Nullable();
        }

        public override void Down()
        {
            Delete.Table("WaterConstituentStateLimits");
        }
    }
}
