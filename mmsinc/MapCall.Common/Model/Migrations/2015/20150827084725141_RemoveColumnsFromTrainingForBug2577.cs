using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150827084725141), Tags("Production")]
    public class RemoveColumnsFromTrainingForBug2577 : Migration
    {
        public struct TableNames
        {
            public const string TRAINING_MODULES = "tblTrainingModules";
        }

        public override void Up()
        {
            Delete.Column("PositionRequirement").FromTable(TableNames.TRAINING_MODULES);
            Delete.Column("WebTrainingBLR").FromTable(TableNames.TRAINING_MODULES);
        }

        public override void Down()
        {
            Alter.Table(TableNames.TRAINING_MODULES).AddColumn("WebTrainingBLR").AsBoolean().Nullable();
            Alter.Table(TableNames.TRAINING_MODULES).AddColumn("PositionRequirement").AsBoolean().Nullable();
        }
    }
}
