using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190712130651478), Tags("Production")]
    public class AdjustRecurringProjectsForMC1294 : Migration
    {
        private string TABLE_NAME = "RecurringProjects";

        public override void Up()
        {
            Insert.IntoTable("OverrideInfoMasterReasons").Row(new {Description = "Paving Opportunity/Moratorium"});

            Create.LookupTable("GISDataInaccuracyTypes", 35);

            Insert.IntoTable("GISDataInaccuracyTypes")
                  .Rows(new {Description = "Material Incorrect or Missing"},
                       new {Description = "Install Date Incorrect or Missing"},
                       new {Description = "Diameter Incorrect or Missing"},
                       new {Description = "Main Break History Incorrect"});
            Alter.Table(TABLE_NAME)
                 .AddForeignKeyColumn("CorrectDiameterId", "PipeDiameters")
                 .AddForeignKeyColumn("CorrectMaterialId", "PipeMaterials")
                 .AddColumn("CorrectInstallationDate").AsDateTime().Nullable();
            Create.Table("RecurringProjectsMainBreakOrders")
                  .WithForeignKeyColumn("RecurringProjectId", TABLE_NAME)
                  .WithForeignKeyColumn("WorkOrderId", "WorkOrders", "WorkOrderID");
            Create.Table("RecurringProjectsGISDataInaccuracies")
                  .WithForeignKeyColumn("RecurringProjectId", TABLE_NAME)
                  .WithForeignKeyColumn("GISDataInaccuracyTypeId", "GISDataInaccuracyTypes");
            //DocTypeForGISEmail
        }

        public override void Down()
        {
            Delete.Table("RecurringProjectsGISDataInaccuracies");
            Delete.Table("RecurringProjectsMainBreakOrders");
            Delete.ForeignKeyColumn(TABLE_NAME, "CorrectMaterialId", "PipeMaterials");
            Delete.ForeignKeyColumn(TABLE_NAME, "CorrectDiameterId", "PipeDiameters");
            Delete.Column("CorrectInstallationDate").FromTable(TABLE_NAME);
            Delete.Table("GISDataInaccuracyTypes");
            Delete.FromTable("OverrideInfoMasterReasons").Row(new {Description = "Paving Opportunity/Moratorium"});
        }
    }
}
