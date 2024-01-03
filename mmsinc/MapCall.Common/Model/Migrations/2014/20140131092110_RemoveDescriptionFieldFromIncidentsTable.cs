using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140131092110), Tags("Production")]
    public class RemoveDescriptionFieldFromIncidentsTable : Migration
    {
        public override void Up()
        {
            Delete.Column("Description").FromTable("Incidents");
        }

        public override void Down()
        {
            Alter.Table("Incidents").AddColumn("Description").AsCustom("ntext").Nullable();
        }
    }
}
