using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190722093918599), Tags("Production")]
    public class MC1279RenameNJDEPCaseNumberToDEPCaseNumber : Migration
    {
        public override void Up()
        {
            Rename.Column("NJDEPCaseNumber").OnTable("SewerOverflows").To("DEPCaseNumber");
        }

        public override void Down()
        {
            Rename.Column("DEPCaseNumber").OnTable("SewerOverflows").To("NJDEPCaseNumber");
        }
    }
}
