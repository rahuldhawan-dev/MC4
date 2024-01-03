using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170418102511014), Tags("Production")]
    public class Bug3762 : Migration
    {
        public override void Up()
        {
            Alter.Column("EmergencyPhoneNumber").OnTable("MainCrossings").AsString(15).Nullable();
        }

        public override void Down()
        {
            // No rollback as that would potentially error when trying to trim values.
        }
    }
}
