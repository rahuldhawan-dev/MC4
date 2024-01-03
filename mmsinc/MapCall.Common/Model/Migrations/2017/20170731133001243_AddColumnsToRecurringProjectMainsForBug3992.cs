using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170731133001243), Tags("Production")]
    public class AddColumnsToRecurringProjectMainsForBug3992 : Migration
    {
        public override void Up()
        {
            Alter.Table("RecurringProjectMains")
                 .AddColumn("Diameter").AsInt32().Nullable()
                 .AddColumn("DateInstalled").AsDateTime().Nullable()
                 .AddColumn("Material").AsAnsiString(50).Nullable();
            Alter.Table("RecurringProjects")
                 .AddColumn("ExistingDiameterOverride").AsInt32().Nullable()
                 .AddColumn("DecadeInstalledOverride").AsInt32().Nullable()
                 .AddColumn("ExistingPipeMaterialOverride").AsAnsiString(50).Nullable();
        }

        public override void Down()
        {
            Delete.Column("Diameter").FromTable("RecurringProjectMains");
            Delete.Column("Material").FromTable("RecurringProjectMains");
            Delete.Column("DateInstalled").FromTable("RecurringProjectMains");

            Delete.Column("ExistingDiameterOverride").FromTable("RecurringProjects");
            Delete.Column("ExistingPipeMaterialOverride").FromTable("RecurringProjects");
            Delete.Column("DecadeInstalledOverride").FromTable("RecurringProjects");
        }
    }
}
