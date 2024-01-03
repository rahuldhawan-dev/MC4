using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201116090143678), Tags("Production")]
    public class FacilitiesColumnRenamesForMC2716 : AutoReversingMigration
    {
        private void RenameColumn(string oldName, string newName)
        {
            Rename.Column(oldName).OnTable("tblFacilities").To(newName);
        }

        public override void Up()
        {
            RenameColumn("DEPIDNumber", "EnvironmentalRegulatorIDNumber");
            RenameColumn("NJDEP_Designation_TreatmentPlant", "DesignationTreatmentPlant");
            RenameColumn("NJDEP_Designation_PumpStation", "DesignationPumpStation");
            RenameColumn("DEP_Firm_Capacity_Facility_MGD ", "FirmCapacityFacilityMGD ");
            RenameColumn("DEP_Total_Effective_Capacity_Facility_MGD", "TotalEffectiveCapacityFacilityMGD");
            RenameColumn("DEP_Production_Capacity_Facility_MGD", "ProductionCapacityFacilityMGD");
            RenameColumn("DEP_Production_Capacity_Under_Aux_Power_Facility_MGD",
                "ProductionCapacityUnderAuxPowerFacilityMGD");
        }
    }
}
