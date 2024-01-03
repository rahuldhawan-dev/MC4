using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170922132705547), Tags("Production")]
    public class ExtendUnitsOnScadaTagNames : Migration
    {
        public override void Up()
        {
            Alter.Column("Units").OnTable("ScadaTagNames").AsString(40).Nullable();
        }

        public override void Down() { }
    }
}
