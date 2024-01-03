using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220913120030775), Tags("Production")]
    public class MC1177_RenameTownSectionColumnAddTownSectionIDToAsBuiltImage : Migration
    {
        public override void Up()
        {
            Rename.Column("TownSection")
                  .OnTable("AsBuiltImages")
                  .To("ProjectName");
            Alter.Table("AsBuiltImages")
                 .AddForeignKeyColumn("TownSectionID", "TownSections", "TownSectionID");
        }

        public override void Down()
        {
            Rename.Column("ProjectName")
                  .OnTable("AsBuiltImages")
                  .To("TownSection");
            Delete.ForeignKeyColumn("AsBuiltImages", "TownSectionID", "TownSections", "TownSectionID");
        }
    }
}

