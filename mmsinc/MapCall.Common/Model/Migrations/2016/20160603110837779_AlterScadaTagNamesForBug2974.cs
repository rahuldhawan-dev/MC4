using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160603110837779), Tags("Production")]
    public class AlterScadaTagNamesForBug2974 : Migration
    {
        public override void Up()
        {
            Alter.Table("ScadaTagNames").AlterColumn("TagName").AsString(100);

            Alter.Table("ScadaTagNames").AddColumn("Description").AsString(100).Nullable();
        }

        public override void Down()
        {
            Delete.Column("Description").FromTable("ScadaTagNames");
        }
    }
}
