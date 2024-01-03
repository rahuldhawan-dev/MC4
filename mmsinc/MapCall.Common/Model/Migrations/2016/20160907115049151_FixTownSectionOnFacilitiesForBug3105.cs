using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160907115049151), Tags("Production")]
    public class FixTownSectionOnFacilitiesForBug3105 : Migration
    {
        private const string FACILITIES = "tblFacilities";

        public override void Up()
        {
            // Cleanup some garbage data:
            Update.Table(FACILITIES).Set(new {TownSection = (string)null}).Where(new {TownSection = string.Empty});

            // There's a lot of town sections filled in where the name is just the town name. These are being ignored.

            // A few Frenchtown rows don't have TownID set for some reason.
            Execute.Sql(@"
declare @frenchTownId int
set @frenchTownId = (select top 1 TownID from Towns where Town = 'FRENCHTOWN BOROUGH')

declare @hempsteadId int
set @hempsteadId = (select top 1 TownID from Towns where Town = 'HEMPSTEAD');

update tblFacilities set TownID = @frenchTownID where Town = 'Frenchtown Boro';
update tblFacilities set TownID = @hempsteadId where TownSection in ('Baldwin', 'Hewlett', 'Inwood', 'Lakeview', 'Oceanside', 'Roosevelt', 'South Hempstead');

update tblFacilities set TownSection = ts.TownSectionId from TownSections ts where ts.TownId = tblFacilities.TownId and tblFacilities.TownSection = ts.Name;

update tblFacilities set TownSection = null where TownSection = Town;

update tblFacilities set TownSection = null where isnumeric(TownSection) <> 1;
");

            Rename.Column("TownSection").OnTable("tblFacilities").To("TownSectionId");

            Alter.Column("TownSectionId")
                 .OnTable("tblFacilities")
                 .AsForeignKey("TownSectionId", "TownSections", "TownSectionId");
        }

        public override void Down()
        {
            Alter.Column("TownSectionId")
                 .OnTable("tblFacilities")
                 .AsString(25);

            Execute.Sql(
                "update tblFacilities set TownSectionId = ts.Name from TownSections ts where tblFacilities.TownSectionId = ts.TownSectionId");

            Rename.Column("TownSectionId").OnTable("tblFacilities").To("TownSection");
        }
    }
}
