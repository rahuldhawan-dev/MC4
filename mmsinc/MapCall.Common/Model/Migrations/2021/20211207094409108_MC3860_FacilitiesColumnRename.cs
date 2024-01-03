using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20211207094409108), Tags("Production")]
    public class MC3860FacilitiesColumnRename : AutoReversingMigration
    {
        public override void Up()
        {
            Rename.Column("Production_Capacity_Maximum_MGD").OnTable("tblFacilities").To("FacilityTotalCapacityMGD");
            Rename.Column("FirmCapacityFacilityMGD").OnTable("tblFacilities").To("FacilityReliableCapacityMGD");
            Rename.Column("TotalEffectiveCapacityFacilityMGD").OnTable("tblFacilities").To("FacilityOperatingCapacityMGD");
            Rename.Column("ProductionCapacityFacilityMGD").OnTable("tblFacilities").To("FacilityRatedCapacityMGD");
            Rename.Column("ProductionCapacityUnderAuxPowerFacilityMGD").OnTable("tblFacilities").To("FacilityAuxPowerCapacityMGD");
        }
    }
}

