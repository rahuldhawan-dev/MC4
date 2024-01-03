using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151117131359673), Tags("Production")]
    public class AddReplacedWithToMainBreaksForBug2710 : Migration
    {
        public override void Up()
        {
            Alter.Table("MainBreaks")
                 .AddForeignKeyColumn("ReplacedWithId", "PipeMaterials", "PipeMaterialId");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("MainBreaks", "ReplacedWithId", "PipeMaterials", "PipeMaterialId");
        }
    }
}
