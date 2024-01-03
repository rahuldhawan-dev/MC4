using System;

namespace SAP.Data.Model.Entities
{
    [Serializable]
    public class Equipments
    {
        public virtual string EquipmentCategory { get; set; }
        public virtual string InventoryNumber { get; set; }
        public virtual string Description { get; set; }
        public virtual string AssetType { get; set; }
        public virtual string RefEquipmentNo { get; set; }
        public virtual string Class { get; set; }
        public virtual string FunctionalLocID { get; set; }
        public virtual string AuthGroup { get; set; }
        public virtual DateTime? StartUpDate { get; set; }
        public virtual string Manufacturer { get; set; }
        public virtual int Model { get; set; }
        public virtual int? YearManufactured { get; set; }
        public virtual bool ABCIndicator { get; set; }
        public virtual string SortField { get; set; }
        public virtual string CompanyCode { get; set; }
        public virtual string CostCenter { get; set; }
        public virtual string ControllingArea { get; set; }
        public virtual string PlanningPlant { get; set; }
        public virtual string PlannerGroup { get; set; }
        public virtual string MainWorkCenter { get; set; }
        public virtual string CatalogProfile { get; set; }
        public virtual string House { get; set; }
        public virtual string Street1 { get; set; }
        public virtual string Street2 { get; set; }
        public virtual string City { get; set; }
        public virtual string OtherCity { get; set; }
        public virtual string State { get; set; }
        public virtual string Country { get; set; }

        public Equipments(object objEquipment)
        {
            InventoryNumber = "HPV-1"; // objEquipment.HydrantNumber ;
            EquipmentCategory = "H";
            if (EquipmentCategory == "H")
                AssetType = "Hydrant";

            FunctionalLocID = "NJAC-PV-HYDRT-0002"; // objEquipment.GeoEFunctionalLocation;
            StartUpDate = DateTime.Now; // objEquipment.DateInstalled;
            Manufacturer = "WATEROUS"; // objEquipment.HydrantManufacturer;
            Model = 1; // objEquipment.HydrantModel;
            YearManufactured = 0; //objEquipment.YearManufactured;
            ABCIndicator = false; // objEquipment.Critical;
            House = ""; // objEquipment.StreetNumber;
            Street1 = "ADAMS AVE"; // objEquipment.Street;
            Street2 = "FRANKLIN AVE"; // objEquipment.CrossStreet;
            City = "PLEASANTVILLE"; //objEquipment.Town
            OtherCity = ""; //objEquipment.TownSection
            State = "";
            Country = "US";
        }
    }
}
