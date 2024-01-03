using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160512144504679), Tags("Production")]
    public class AlterPDESSystemsRequiredFieldsForBug2944 : Migration
    {
        public override void Up()
        {
            Alter.Column("GravityLength").OnTable("PDESSystems").AsInt32().Nullable();
            Alter.Column("ForceLength").OnTable("PDESSystems").AsInt32().Nullable();
            Alter.Column("NumberOfLiftStations").OnTable("PDESSystems").AsInt32().Nullable();
            Alter.Column("TreatmentDescription").OnTable("PDESSystems").AsString(255).Nullable();
            Alter.Column("NumberOfCustomers").OnTable("PDESSystems").AsInt32().Nullable();
            Alter.Column("PeakFlowMGD").OnTable("PDESSystems").AsInt32().Nullable();
            Alter.Column("IsCombinedSewerSystem").OnTable("PDESSystems").AsBoolean().Nullable();
        }

        public override void Down()
        {
            //Alter.Column("GravityLength").OnTable("PDESSystems").AsInt32().NotNullable();
            //Alter.Column("ForceLength").OnTable("PDESSystems").AsInt32().NotNullable();
            //Alter.Column("NumberOfLiftStations").OnTable("PDESSystems").AsInt32().NotNullable();
            //Alter.Column("TreatmentDescription").OnTable("PDESSystems").AsString(255).NotNullable();
            //Alter.Column("NumberOfCustomers").OnTable("PDESSystems").AsInt32().NotNullable();
            //Alter.Column("PeakFlowMGD").OnTable("PDESSystems").AsInt32().NotNullable();
        }
    }
}
