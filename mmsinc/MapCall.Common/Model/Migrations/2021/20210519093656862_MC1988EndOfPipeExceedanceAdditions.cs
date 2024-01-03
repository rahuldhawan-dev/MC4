using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210519093656862), Tags("Production")]
    public class MC1988EndOfPipeExceedanceAdditions : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("EndOfPipeExceedanceTypes").Row(new {Description = "Chloride"});
            Insert.IntoTable("EndOfPipeExceedanceTypes").Row(new {Description = "Chlorine"});
            Insert.IntoTable("EndOfPipeExceedanceTypes").Row(new {Description = "Copper"});
            Insert.IntoTable("EndOfPipeExceedanceTypes").Row(new {Description = "Sodium"});
            Insert.IntoTable("EndOfPipeExceedanceTypes").Row(new {Description = "Turbidity"});
            Execute.Sql("UPDATE EndOfPipeExceedanceTypes SET Description = 'Plant Bypass/Flow' WHERE Description = 'Plant Flow';");

            this.CreateLookupTableWithValues("LimitationTypes", "Daily", "Weekly", "Monthly", "Load");
            Alter.Table("EndOfPipeExceedances")
                 .AddForeignKeyColumn("LimitationTypeId", "LimitationTypes", nullable: false);
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("EndOfPipeExceedances", "LimitationTypeId", "LimitationTypes");
            Delete.Table("LimitationTypes");
        }
    }
}

