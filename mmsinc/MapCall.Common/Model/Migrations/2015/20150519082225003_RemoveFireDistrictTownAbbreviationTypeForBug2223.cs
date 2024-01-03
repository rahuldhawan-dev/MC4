using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150519082225003), Tags("Production")]
    public class RemoveFireDistrictTownAbbreviationTypeForBug2223 : Migration
    {
        public override void Up()
        {
            // Change Hempstead(I've been calling them Nassau! Why didn't anyone tell me?(I can't find a YouTube clip of the Simpsons reference I'm making))
            // to use Town abbreviation type instead. Nassau no longer uses MapCall to handle assets
            // so the fire district abbreviation type can die.

            Execute.Sql(@"
declare @townType int; set @townType = (select top 1 AbbreviationTypeID from AbbreviationTypes where Description = 'Town')
declare @fireDistrict int; set @fireDistrict = (select top 1 AbbreviationTypeID from AbbreviationTypes where Description = 'Fire District')

update Towns set AbbreviationTypeID = @townType where AbbreviationTypeID = @fireDistrict 
");
        }

        public override void Down()
        {
            Execute.Sql(@"
declare @townType int; set @townType = (select top 1 AbbreviationTypeID from AbbreviationTypes where Description = 'Town')
declare @fireDistrict int; set @fireDistrict = (select top 1 AbbreviationTypeID from AbbreviationTypes where Description = 'Fire District')

update Towns set AbbreviationTypeID = @fireDistrict where Town = 'HEMPSTEAD'
");
        }
    }
}
