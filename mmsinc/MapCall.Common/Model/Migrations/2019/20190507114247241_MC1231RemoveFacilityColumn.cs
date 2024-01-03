using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190507114247241), Tags("Production")]
    public class MC1231RemoveFacilityColumn : Migration
    {
        public override void Up()
        {
            Alter.Table("tblPosition_History")
                 .AddForeignKeyColumn("ReportingFacilityId", "tblFacilities", "RecordId");
            Execute.Sql(
                "UPDATE tblPosition_History set ReportingFacilityId = " +
                "(select recordID from tblFacilities " +
                "where FacilityName not like '%duplicate%' " +
                "AND FacilityId = Reporting_FacilityID " +
                "AND e.OperatingCenterId = tblFacilities.OperatingCenterID) " +
                "FROM tblPosition_History ph LEFT JOIN tblEmployee E on E.tblEmployeeID = ph.tblEmployeeID");
            Delete.Column("Reporting_FacilityID").FromTable("tblPosition_History");
        }

        public override void Down()
        {
            Alter.Table("tblPosition_History").AddColumn("Reporting_FacilityID").AsAnsiString(50).Nullable();
            Execute.Sql(
                "UPDATE tblPosition_History SET Reporting_FacilityID = (SELECT OperatingCenterCode from OperatingCenters where OperatingCenters.OperatingCenterID = (select OperatingCenterID from tblFacilities where recordId = ReportingFacilityId)) + '-' + cast(ReportingFacilityId as varchar(55))");
            Delete.ForeignKeyColumn("tblPosition_History", "ReportingFacilityId", "tblFacilities", "RecordId");
        }
    }
}
