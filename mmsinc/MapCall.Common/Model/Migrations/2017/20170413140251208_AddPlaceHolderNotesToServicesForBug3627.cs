using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170413140251208), Tags("Production")]
    public class AddPlaceHolderNotesToServicesForBug3627 : Migration
    {
        public override void Up()
        {
            Alter.Table("Services").AddColumn("PlaceHolderNotes").AsCustom("text").Nullable();
        }

        public override void Down()
        {
            Delete.Column("PlaceholderNotes").FromTable("Services");
        }
    }
}
