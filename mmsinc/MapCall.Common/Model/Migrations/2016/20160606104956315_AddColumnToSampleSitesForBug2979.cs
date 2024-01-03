using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160606104956315), Tags("Production")]
    public class AddColumnToSampleSitesForBug2979 : Migration
    {
        public override void Up()
        {
            Alter.Table("tblWQSample_Sites").AddColumn("NJDEPID").AsAnsiString(40).Nullable();
        }

        public override void Down()
        {
            Delete.Column("NJDEPID").FromTable("tblWQSample_Sites");
        }
    }
}
