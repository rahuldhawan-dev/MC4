using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200615095654850), Tags("Production")]
    public class MC2421AddLegacyColumnToEquipment : Migration
    {
        public override void Up()
        {
            Alter.Table("Equipment")
                 .AddColumn("Legacy").AsString(50).Nullable();
        }

        public override void Down()
        {
            Delete.Column("Legacy").FromTable("Equipment");
        }
    }
}
