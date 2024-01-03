using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200918101755471), Tags("Production")]
    public class MC2618UpdateOutOfServiceLockoutQuestion : Migration
    {
        private const string OLD_QUESTION =
            "'Relieve, disconnect, restrain, or otherwise render safe and stored residual energy. If re-accumulation of any stored energy is possible, continue to verify that it has been rendered “safe” until the job is complete.'";

        private const string NEW_QUESTION =
            "'Relieve, disconnect, restrain, or otherwise render safe and stored residual energy (such as that in capacitors, springs, elevated machine members, rotating flywheels, hydraulic, air, gas, or water pressure systems, etc.). If re-accumulation of any stored energy is possible, continue to verify that it has been rendered “safe” until the job is complete.'";

        public override void Up()
        {
            Execute.Sql("UPDATE LockoutFormQuestions SET Question = " + NEW_QUESTION + " WHERE Id = 5");
        }

        public override void Down()
        {
            Execute.Sql("UPDATE LockoutFormQuestions SET Question = " + OLD_QUESTION + " WHERE Id = 5");
        }
    }
}
