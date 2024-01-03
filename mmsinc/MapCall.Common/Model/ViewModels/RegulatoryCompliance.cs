using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCall.Common.Model.ViewModels
{
    public class RegulatoryCompliance
    {
        public string State { get; set; }
        public int StateId { get; set; }
        public string OperatingCenterCode { get; set; }
        public string OperatingCenterName { get; set; }
        public int OperatingCenterId { get; set; }
        public string PlanningPlantCode { get; set; }
        public string PlanningPlantDescription { get; set; }
        public int PlanningPlantId { get; set; }
        public PublicWaterSupply PublicWaterSupply { get; set; }
        public string Facility { get; set; }
        public int FacilityId { get; set; }
        public int EquipmentId { get; set; }
        public EquipmentType EquipmentType { get; set; }
        public EquipmentPurpose EquipmentPurpose { get; set; }
        public string Description { get; set; }
        public bool HasProcessSafetyManagement { get; set; }
        public bool HasCompanyRequirement { get; set; }
        public bool HasRegulatoryRequirement { get; set; }
        public bool HasOshaRequirement { get; set; }
        public bool OtherCompliance { get; set; }
        public string OtherComplianceReason { get; set; }
        [View("# of WO's Incomplete")]
        public int NumberIncomplete { get; set; }
        [View("# of WO's Cancelled")]
        public int NumberCancelled { get; set; }
        [View("# of WO's Completed")]
        public int NumberCompleted { get; set; }
        public DateTime? DateCompleted { get; set; }
        public int? LastCompletedWorkOrderId { get; set; }
    }

    public interface ISearchRegulatoryCompliance : ISearchSet<RegulatoryCompliance>
    {
        int[] State { get; set; }
        int[] OperatingCenter { get; set; }
        int[] PlanningPlant { get; set; }
        int[] Facility { get; set; }
        int[] EquipmentType { get; set; }
        int[] EquipmentPurpose { get; set; }
        string Description { get; set; }
        RequiredDateRange DateReceived { get; set; }
        string[] SelectedEquipmentTypes { get; set; }
        string[] SelectedEquipmentPurposes { get; set; }
        string[] SelectedFacilities { get; set; }
        string[] SelectedOperatingCenters { get; set; }
        string[] SelectedPlanningPlants { get; set; }
        string[] SelectedStates { get; set; }
        bool? HasProcessSafetyManagement { get; set; }
        bool? HasCompanyRequirement { get; set; }
        bool? HasRegulatoryRequirement { get; set; }
        bool? HasOshaRequirement { get; set; }
        bool? OtherCompliance { get; set; }
    }
}
