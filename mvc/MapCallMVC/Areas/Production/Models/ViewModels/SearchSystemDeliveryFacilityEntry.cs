using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class SearchSystemDeliveryFacilityEntry : SearchSet<SystemDeliveryFacilityEntry>
    {
        [SearchAlias("SystemDeliveryEntry", "sysDelEntry", "Id", Required = true)]
        public int? EntryId { get; set; }

        private int[] _publicWaterSupplies;
        private int[] _wasteWaterSystems;

        [MultiSelect("", nameof(OperatingCenter), "ByStateIds", DependsOn = nameof(State)),
         EntityMustExist(typeof(OperatingCenter)), SearchAlias("Facility", "fac", "OperatingCenter.Id", Required = true)]
        public int[] OperatingCenter { get; set; }

        [MultiSelect,
         SearchAlias("fac.OperatingCenter", "State.Id")]
        public int[] State { get; set; }

        [DropDown,
         EntityMap,
         EntityMustExist(typeof(SystemDeliveryType)),
         SearchAlias("SystemDeliveryType", "Id")]
        public int? SystemDeliveryType { get; set; }

        [View("PWSIDs"), MultiSelect("", "PublicWaterSupply", "ByOperatingCenterId", DependsOn = "OperatingCenter"),
         EntityMap, EntityMustExist(typeof(PublicWaterSupply)),
         SearchAlias("fac.PublicWaterSupply", "Id")]
        public int[] PublicWaterSupplies
        {
            get => SystemDeliveryType == MapCall.Common.Model.Entities.SystemDeliveryType.Indices.WATER ? _publicWaterSupplies : null;
            set => _publicWaterSupplies = value;
        }

        [View("WWSIDs"), MultiSelect("Environmental", "WasteWaterSystem", "ByOperatingCenters", DependsOn = "OperatingCenter"),
         EntityMap, EntityMustExist(typeof(WasteWaterSystem)),
         SearchAlias("fac.WasteWaterSystem", "Id")]
        public int[] WasteWaterSystems
        {
            get => SystemDeliveryType == MapCall.Common.Model.Entities.SystemDeliveryType.Indices.WASTE_WATER ? _wasteWaterSystems : null;
            set => _wasteWaterSystems = value;
        }

        [MultiSelect("", "Facility", "GetActiveByOperatingCentersWithSystemDeliveryTypeAndPublicWaterSupplyIdAndWasteWaterSystemId", DependentsRequired = DependentRequirement.One,
             DependsOn = (nameof(OperatingCenter) + "," + nameof(SystemDeliveryType) + "," + nameof(PublicWaterSupplies) + "," + nameof(WasteWaterSystems))),
         EntityMustExist(typeof(Facility))]
        public int[] Facility { get; set; }

        [MultiSelect("Production", "SystemDeliveryEntryType", "ByFacilitiesSystemDeliveryTypeId",
             DependsOn = nameof(Facility)), EntityMap, EntityMustExist(typeof(SystemDeliveryEntryType)),
         SearchAlias("SystemDeliveryEntryType", "Id")]
        public int[] SystemDeliveryEntryType { get; set; }

        public DateRange EntryDate { get; set; }

        [DropDown("", "Employee", "GetEmployeeByOperatingCentersWhoHaveEnteredSystemDeliveryEntries",
             DependsOn = nameof(OperatingCenter)), EntityMap, EntityMustExist(typeof(Employee)),
         SearchAlias("EnteredBy", "Id")]
        public int? EnteredBy { get; set; }

        [SearchAlias("SystemDeliveryEntry", "sysDelEntry", "IsHyperionFileCreated")]
        public bool? IsHyperionFileCreated { get; set; }
    }
}
