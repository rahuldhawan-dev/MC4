using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20191008101104286), Tags("Production")]
    public class MC927AddCrossStreetToTapImages : Migration
    {
        public override void Up()
        {
            // Using 50 to match TapImage.Street
            Create.Column("CrossStreet").OnTable("TapImages").AsString(50).Nullable();

            Execute.Sql(@"update TapImages set CrossStreet = Streets.FullStName
from TapImages
inner join Services on Services.Id = TapImages.ServiceId
inner join Streets on Streets.StreetId = Services.CrossStreetId");
        }

        public override void Down()
        {
            Delete.Column("CrossStreet").FromTable("TapImages");
        }
    }
}
