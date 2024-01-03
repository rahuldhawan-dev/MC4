using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170920102848247), Tags("Production")]
    public class Bug4077ContractorUsersLockOut : Migration
    {
        public override void Up()
        {
            Create.Column("FailedLoginAttemptCount").OnTable("ContractorUsers")
                  .AsInt32().NotNullable().WithDefaultValue(0);
        }

        public override void Down()
        {
            Delete.Column("FailedLoginAttemptCount")
                  .FromTable("ContractorUsers");
        }
    }
}
