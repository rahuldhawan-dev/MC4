using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140925112032488), Tags("Production")]
    public class AddTrainingFrequencyToTrainingModulesForBug2066 : Migration
    {
        public override void Up()
        {
            Alter.Table("tblTrainingModules").AddColumn("TrainingFrequency").AsInt32().Nullable();
            Alter.Table("tblTrainingModules").AddColumn("TrainingFrequencyUnit").AsString(10).Nullable();
        }

        public override void Down()
        {
            Delete.Column("TrainingFrequency").FromTable("tblTrainingModules");
            Delete.Column("TrainingFrequencyUnit").FromTable("tblTrainingModules");
        }
    }
}
