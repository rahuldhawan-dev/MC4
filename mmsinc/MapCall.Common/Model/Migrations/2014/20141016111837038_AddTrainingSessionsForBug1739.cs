using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20141016111837038), Tags("Production")]
    public class AddTrainingSessionsForBug1739 : Migration
    {
        public struct TableNames
        {
            public const string TRAINING_SESSIONS = "TrainingSessions", TRAINING_RECORDS = "tblTrainingRecords";
        }

        public struct ColumnNames
        {
            public const string DATE_START = "StartDateTime",
                                DATE_END = "EndDateTime",
                                TRAINING_RECORD_ID = "TrainingRecordId";
        }

        public override void Up()
        {
            Create.Table(TableNames.TRAINING_SESSIONS)
                  .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn(ColumnNames.DATE_START).AsDateTime().NotNullable()
                  .WithColumn(ColumnNames.DATE_END).AsDateTime().NotNullable();
            Alter.Table(TableNames.TRAINING_SESSIONS)
                 .AddForeignKeyColumn(
                      ColumnNames.TRAINING_RECORD_ID,
                      TableNames.TRAINING_RECORDS,
                      ColumnNames.TRAINING_RECORD_ID);
        }

        public override void Down()
        {
            Delete.Table(TableNames.TRAINING_SESSIONS);
        }
    }
}
