using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210609092839064), Tags("Production")]
    public class MC3259AddBusinessUnitFieldToFacilitySystemDeliveryEntryTypes : Migration
    {
        public override void Up()
        {
            Alter.Table("FacilitiesSystemDeliveryEntryTypes").AddColumn("BusinessUnit").AsInt32().Nullable();
            // Populate the existing business units into the table NOT all operating center / department combos have a BusinessUnit
            Execute.Sql($@"UPDATE FacilitiesSystemDeliveryEntryTypes
SET BusinessUnit = tempData.BU
FROM FacilitiesSystemDeliveryEntryTypes fsd
INNER JOIN  (SELECT DISTINCT fac.RecordId, fac.OperatingCenterID, bu.BU 
FROM tblFacilities fac
INNER JOIN BusinessUnits bu ON bu.DepartmentID = fac.DepartmentID AND bu.OperatingCenterID = fac.OperatingCenterID
WHERE bu.BU IN (SELECT TOP(1)bu FROM BusinessUnits WHERE OperatingCenterID = fac.OperatingCenterID AND DepartmentID = fac.DepartmentID)
AND fac.RecordId IN (SELECT DISTINCT FacilityId FROM FacilitiesSystemDeliveryEntryTypes)
) tempData
ON fsd.FacilityId = tempData.RecordId
WHERE fsd.FacilityId = tempData.RecordId");
        }

        public override void Down()
        {
            Delete.Column("BusinessUnit").FromTable("FacilitiesSystemDeliveryEntryTypes");
        }
    }
}

