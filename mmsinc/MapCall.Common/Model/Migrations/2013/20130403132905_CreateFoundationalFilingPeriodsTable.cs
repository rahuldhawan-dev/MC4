using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130403132905), Tags("Production")]
    public class CreateFoundationalFilingPeriodsTable : Migration
    {
        #region Constants

        public struct Sql
        {
            public const string PERMISSIONS = "GRANT ALL ON [FoundationalFilingPeriods] TO MCUser",
                                NULL_FOUNDATIONAL_FILING_PERIOD =
                                    "UPDATE [RPProjects] SET FoundationalFilingPeriod = NULL";
        }

        public struct Tables
        {
            public const string FOUNDATIONAL_FILING_PERIODS = "FoundationalFilingPeriods",
                                RP_PROJECTS = "RPProjects";
        }

        public struct Columns
        {
            public const string FOUNDATIONAL_FILING_PERIOD = "FoundationalFilingPeriod",
                                FOUNDATIONAL_FILING_PERIOD_ID = "FoundationalFilingPeriodID",
                                DESCRIPTION = "Description";
        }

        public struct ForeignKeys
        {
            public const string FK_RPPROJECTS_FOUNDATIONAL_FILING_PERIODS =
                "FK_RPProjects_FoundationalFilingPeriods_FoundationalFilingPeriodID";
        }

        public struct StringLengths
        {
            public const int DESCRIPTION_LENGTH = 50,
                             FOUNDATIONAL_FILING_PERIOD = 50;
        }

        #endregion

        public override void Up()
        {
            Execute.Sql(Sql.NULL_FOUNDATIONAL_FILING_PERIOD); // we don't care about the existing data.
            Rename.Column(Columns.FOUNDATIONAL_FILING_PERIOD)
                  .OnTable(Tables.RP_PROJECTS)
                  .To(Columns.FOUNDATIONAL_FILING_PERIOD_ID);
            Alter.Column(Columns.FOUNDATIONAL_FILING_PERIOD_ID)
                 .OnTable(Tables.RP_PROJECTS)
                 .AsInt32().Nullable();

            Create.Table(Tables.FOUNDATIONAL_FILING_PERIODS)
                  .WithColumn(Columns.FOUNDATIONAL_FILING_PERIOD_ID).AsInt32().Identity().PrimaryKey().NotNullable()
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION_LENGTH);

            Create.ForeignKey(ForeignKeys.FK_RPPROJECTS_FOUNDATIONAL_FILING_PERIODS)
                  .FromTable(Tables.RP_PROJECTS).ForeignColumn(Columns.FOUNDATIONAL_FILING_PERIOD_ID)
                  .ToTable(Tables.FOUNDATIONAL_FILING_PERIODS).PrimaryColumn(Columns.FOUNDATIONAL_FILING_PERIOD_ID);

            Execute.Sql(Sql.PERMISSIONS);
        }

        public override void Down()
        {
            Delete.ForeignKey(ForeignKeys.FK_RPPROJECTS_FOUNDATIONAL_FILING_PERIODS).OnTable(Tables.RP_PROJECTS);
            Delete.Table(Tables.FOUNDATIONAL_FILING_PERIODS);

            Rename.Column(Columns.FOUNDATIONAL_FILING_PERIOD_ID)
                  .OnTable(Tables.RP_PROJECTS)
                  .To(Columns.FOUNDATIONAL_FILING_PERIOD);
            Alter.Column(Columns.FOUNDATIONAL_FILING_PERIOD)
                 .OnTable(Tables.RP_PROJECTS)
                 .AsAnsiString(StringLengths.FOUNDATIONAL_FILING_PERIOD).Nullable();
        }
    }
}
