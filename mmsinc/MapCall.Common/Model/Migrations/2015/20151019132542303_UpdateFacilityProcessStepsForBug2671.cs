using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151019132542303), Tags("Production")]
    public class UpdateFacilityProcessStepsForBug2671 : Migration
    {
        public override void Up()
        {
            Alter.Table("FacilityProcessSteps")
                 .AlterColumn("StepNumber").AsDecimal().NotNullable();
            Alter.Table("FacilityProcessSteps")
                 .AddColumn("ContingencyOperation").AsCustom("text").Nullable()
                 .AddColumn("LossOfCommunicationPowerImpact").AsCustom("text").Nullable();
            Alter.Table("FacilityProcesses")
                 .AddColumn("FacilityProcessDescription").AsCustom("text").Nullable();
        }

        public override void Down()
        {
            Delete.Column("FacilityProcessDescription").FromTable("FacilityProcesses");
            Delete.Column("LossOfCommunicationPowerImpact").FromTable("FacilityProcessSteps");
            Delete.Column("ContingencyOperation").FromTable("FacilityProcessSteps");
            Alter.Table("FacilityProcessSteps")
                 .AlterColumn("StepNumber").AsInt32().NotNullable();
        }
    }
}
