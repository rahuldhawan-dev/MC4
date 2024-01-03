using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20191127122127578), Tags("Production")]
    public class MC1796AddSDSHyperlinkToChemicals : Migration
    {
        public override void Up()
        {
            Alter.Table("Chemicals")
                 .AddColumn("SDSHyperlink").AsAnsiString(2048).Nullable();
        }

        public override void Down()
        {
            Delete.Column("SDSHyperlink").FromTable("Chemicals");
        }
    }
}
