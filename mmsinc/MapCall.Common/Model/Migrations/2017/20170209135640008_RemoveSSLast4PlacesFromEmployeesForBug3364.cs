using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170209135640008), Tags("Production")]
    public class RemoveSSLast4PlacesFromEmployeesForBug3364 : Migration
    {
        public override void Up()
        {
            Delete.Column("SS_Last_4_Places").FromTable("tblEmployee");
        }

        public override void Down()
        {
            Create.Column("SS_Last_4_Places").OnTable("tblEmployee").AsFixedLengthString(4);
        }
    }
}
