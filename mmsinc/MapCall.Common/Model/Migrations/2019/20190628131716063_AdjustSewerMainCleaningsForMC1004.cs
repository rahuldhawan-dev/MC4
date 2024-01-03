using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190628131716063), Tags("Production")]
    public class AdjustSewerMainCleaningsForMC1004 : Migration
    {
        public override void Up()
        {
            Create.LookupTable("TypesOfBlockagesFound");

            Insert.IntoTable("TypesOfBlockagesFound")
                  .Rows(new {Description = "Full"},
                       new {Description = "Partial"});

            Create.LookupTable("CausesOfBlockages");

            Insert.IntoTable("CausesOfBlockages")
                  .Rows(new {Description = "Grease"},
                       new {Description = "Rages"},
                       new {Description = "Roots"});

            Alter.Table("SewerMainCleanings")
                 .AddForeignKeyColumn("MainConditionId", "MainConditions", "MainConditionId")
                 .AddForeignKeyColumn("BlockageFoundId", "TypesOfBlockagesFound")
                 .AddForeignKeyColumn("CauseOfBlockageId", "CausesOfBlockages");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("SewerMainCleanings", "BlockageFoundId", "TypesOfBlockagesFound");
            Delete.ForeignKeyColumn("SewerMainCleanings", "CauseOfBlockageId", "CausesOfBlockages");
            Delete.ForeignKeyColumn("SewerMainCleanings", "MainConditionId", "MainConditions",
                "MainConditionId");

            Delete.Table("TypesOfBlockagesFound");
            Delete.Table("CausesOfBlockages");
        }
    }
}
