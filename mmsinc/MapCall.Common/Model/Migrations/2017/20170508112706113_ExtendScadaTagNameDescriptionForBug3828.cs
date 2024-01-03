using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170508112706113), Tags("Production")]
    public class ExtendScadaTagNameDescriptionForBug3828 : Migration
    {
        public override void Up()
        {
            Alter.Column("Description").OnTable("ScadaTagNames").AsString(200).Nullable();
        }

        public override void Down() { }
    }
}
