using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201014144945545), Tags("Production")]
    public class MC2122DeleteIdentifierColumnInEquipiment : Migration
    {
        public override void Up()
        {
            Delete.Column("Identifier").FromTable("Equipment");
        }

        public override void Down()
        {
            Alter.Table("Equipment").AddColumn("Identifier").AsString(StringLengths.MAX_DEFAULT_VALUE).Nullable();
        }
    }
}
