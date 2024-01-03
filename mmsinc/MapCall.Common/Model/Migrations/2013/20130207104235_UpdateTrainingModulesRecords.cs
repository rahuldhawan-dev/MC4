using System;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    #region Constants

    public struct Tables
    {
        public const string TRAINING_RECORDS = "tblTrainingRecords",
                            TRAINING_MODULES = "tblTrainingModules";
    }

    public struct StringLengths
    {
        public const int MAX_DEFAULT_VALUE = 255,
                         TARGET_TRAINING_MONTH = 10,
                         TRAINING_PERFORMED_BY = 25;
    }

    #endregion

    [Migration(20130207104235), Tags("Production")]
    public class UpdateTrainingModulesRecords : Migration
    {
        public override void Up()
        {
            Rename.Column("Time_To_Deliver_Hours").OnTable(Tables.TRAINING_MODULES).To("TotalHours");
            Delete.Column("TrainingPerformedBy").FromTable(Tables.TRAINING_MODULES);

            Delete.Column("Training_Location").FromTable(Tables.TRAINING_MODULES);
            Delete.Column("Target_Training_Month").FromTable(Tables.TRAINING_MODULES);
            Delete.Column("TCH_Credit_Value").FromTable(Tables.TRAINING_RECORDS);

            Alter.Table(Tables.TRAINING_MODULES).AddColumn("IsActive").AsBoolean().Nullable();

            Delete.Column("TotalHours").FromTable(Tables.TRAINING_RECORDS);
            Rename.Column("TopicDescription").OnTable(Tables.TRAINING_RECORDS).To("Title");

            Delete.ForeignKey("FK_tblTrainingRecords_tblEmployee").OnTable(Tables.TRAINING_RECORDS);
            Alter.Column("PresentedBy").OnTable(Tables.TRAINING_RECORDS).AsString(StringLengths.MAX_DEFAULT_VALUE)
                 .Nullable();
            Execute.Sql(String.Format(
                "UPDATE {0} SET [PresentedBy] = (SELECT ISNULL(First_Name, '') + ISNULL(' ' + Middle_Name, '') + ' ' + ISNULL(Last_Name,'') from tblEmployee T where T.tblEmployeeID = PresentedBy)",
                Tables.TRAINING_RECORDS));

            Rename.Column("CourseDescription").OnTable(Tables.TRAINING_RECORDS).To("CourseLocation");

            Delete.ForeignKey("FK_tblTrainingRecords_Lookup").OnTable(Tables.TRAINING_RECORDS);
            Delete.Column("SponsoredBy").FromTable(Tables.TRAINING_RECORDS);
        }

        public override void Down()
        {
            Alter.Table(Tables.TRAINING_RECORDS).AddColumn("SponsoredBy").AsInt32().Nullable();
            Create.ForeignKey("FK_tblTrainingRecords_Lookup")
                  .FromTable(Tables.TRAINING_RECORDS)
                  .ForeignColumn("SponsoredBy")
                  .ToTable("Lookup")
                  .PrimaryColumn("LookupID");

            Rename.Column("CourseLocation").OnTable(Tables.TRAINING_RECORDS).To("CourseDescription");

            Execute.Sql(String.Format("UPDATE {0} SET PresentedBy = null", Tables.TRAINING_RECORDS));
            Alter.Column("PresentedBy").OnTable(Tables.TRAINING_RECORDS).AsInt32().Nullable();
            Create.ForeignKey("FK_tblTrainingRecords_tblEmployee")
                  .FromTable(Tables.TRAINING_RECORDS)
                  .ForeignColumn("PresentedBy")
                  .ToTable("tblEmployee")
                  .PrimaryColumn("tblEmployeeID");

            Rename.Column("Title").OnTable(Tables.TRAINING_RECORDS).To("TopicDescription");
            Alter.Table(Tables.TRAINING_RECORDS).AddColumn("TotalHours").AsFloat().Nullable();

            Delete.Column("IsActive").FromTable(Tables.TRAINING_MODULES);

            Alter.Table(Tables.TRAINING_RECORDS).AddColumn("TCH_Credit_Value").AsFloat().Nullable();
            Alter.Table(Tables.TRAINING_MODULES).AddColumn("Training_Location")
                 .AsString(StringLengths.MAX_DEFAULT_VALUE).Nullable();
            Alter.Table(Tables.TRAINING_MODULES).AddColumn("Target_Training_Month")
                 .AsString(StringLengths.TARGET_TRAINING_MONTH).Nullable();

            Alter.Table(Tables.TRAINING_MODULES).AddColumn("TrainingPerformedBy")
                 .AsString(StringLengths.TRAINING_PERFORMED_BY).Nullable();
            Rename.Column("TotalHours").OnTable(Tables.TRAINING_MODULES).To("Time_To_Deliver_Hours");
        }
    }
}
