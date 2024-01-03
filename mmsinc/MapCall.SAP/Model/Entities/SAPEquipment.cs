using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.SAP.SAPEquipmentWS;
using MMSINC.Utilities;

namespace MapCall.SAP.Model.Entities
{
    /// <summary>
    /// This class represents and Equipment object that's saved in SAP
    /// In the mapcall world this is many different classes.
    /// This class has constructors to convert each MapCall class over into an SAPEquipment object
    /// Hydrants, Valves, SewerOpenings, etc.
    /// </summary>
    [Serializable]
    public class SAPEquipment : SAPEntity, ISAPServiceEntity
    {
        #region Constants

        public static readonly string[] USER_SYSTEM_STATUS_FOR_EQUIPMENT_ASSET_TYPES = {
            ProductionAssetType.Descriptions.MECHANICAL, 
            ProductionAssetType.Descriptions.ELECTRICAL, 
            ProductionAssetType.Descriptions.TANKS, 
            ProductionAssetType.Descriptions.SEWER
        };

        #endregion

        #region Properties

        #region Logical properties

        public virtual string ABCIndicator => Critical ? "H" : "M";

        //this has been added for equipments
        public virtual string ABCIndicatorForEquipment
        {
            get
            {
                switch (ABCIndicatorMAPCALL)
                {
                    case "High":
                        return "H";
                    case "Low":
                        return "L";
                    case "Medium":
                        return "M";
                    default:
                        return string.Empty;
                }
            }
        }

        public virtual string AddEditIndicator =>
            SAPEquipmentNumber == string.Empty || SAPEquipmentNumber == "0" ? "A" : "E";

        public virtual string EquipmentSystemStatus
        {
            get
            {
                //ACTIVE = 1, CANCELLED= 2, PENDING = 3, RETIRED = 4, INSTALLED = 5, REQUEST_RETIREMENT = 6, REQUEST_CANCELLATION = 7 
                switch (EquipmentStatus)
                {
                    case "ACTIVE":
                    case "INSTALLED":
                    case "PENDING":
                    case "REQUEST RETIREMENT":
                    case "REQUEST CANCELLATION":
                    case "IN SERVICE":
                        return "INST";
                    case "CANCELLED":
                    case "REMOVED":
                    case "RETIRED":
                        return "DLFL INAC INST";
                    case "INACTIVE":
                    case "OUT OF SERVICE":
                        return "INAC INST";
                    default:
                        return string.Empty;
                }
            }
        }

        // from map call
        // for NOT mechanical/electrical/tank
        public virtual string EquipmentUserStatus
        {
            get
            {
                switch (EquipmentStatus)
                {
                    case "PENDING":
                    case "INSTALLED":
                    case "IN SERVICE":
                        return "OOS TBIN";
                    case "INACTIVE":
                    case "CANCELLED":
                    case "REQUEST CANCELLATION":
                    case "OUT OF SERVICE":
                        return "OOS";
                    case "RETIRED":
                        return "REIP";
                    case "REQUEST RETIREMENT":
                    case "ACTIVE":
                        return "INSV";
                    case "REMOVED":
                        return "REMV";
                    default:
                        return string.Empty;
                }
            }
        }

        public virtual string SystemStatusForEquipment
        {
            get
            {
                //ACTIVE = 1, CANCELLED= 2, PENDING = 3, RETIRED = 4, INSTALLED = 5, REQUEST_RETIREMENT = 6, REQUEST_CANCELLATION = 7 
                switch (EquipmentStatus)
                {
                    case "IN SERVICE":
                    case "OUT OF SERVICE":
                        return "INST";
                    case "PENDING":
                    case "FIELD INSTALLED":
                        return "INST";
                    case "PENDING RETIREMENT":
                        return "INST";
                    case "RETIRED":
                        return "INAC INST";
                    case "CANCELLED": //added new status -11 Aug 2017
                        return "DLFL INAC INST";

                    default:
                        return string.Empty;
                }
            }
        }

        // from map call
        // for mechanical/electrical/tank
        public virtual string UserStatusForEquipment
        {
            get
            {
                switch (EquipmentStatus)
                {
                    case "IN SERVICE":
                        return "INSV";
                    case "OUT OF SERVICE":
                    case "CANCELLED": //changed on 1st sep
                        return "OOS";
                    case "PENDING":
                    case "FIELD INSTALLED":
                        return "OOS TBIN";
                    case "PENDING RETIREMENT":
                        return "INSV";
                    case "RETIRED":
                        return "REMV";
                    default:
                        return string.Empty;
                }
            }
        }

        private string _description;

        public virtual string Description
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_description))
                    _description = (InventoryNumber != null) ? InventoryNumber + "/" + AssetType : string.Empty;
                return _description;
            }
        }

        //bug-3689
        public virtual string ApplicationValue
        {
            get
            {
                switch (ValveControl)
                {
                    case "AIR RELEASE":
                        return "PRESSURE REGULATION";
                    case "BLOW OFF":
                        return "BLOW-OFF";
                    case "BLOW OFF WITH FLUSHING":
                        return "BLOW-OFF";
                    case "BY-PASS":
                        return "BYPASS";
                    case "CONTROL VALVE":
                        return "DISTRIBUTION IN GRID";
                    case "DOMESTIC SERVICE":
                        return "SERVICE LINE";
                    case "FIRE SERVICE":
                        return "FIRE SERVICE";
                    case "GRADIENT SEPARATION":
                        return "ZONE SEPARATION";
                    case "HYDRANT":
                        return "HYD AUX";
                    case "INSERTION":
                        return "INSERTION";
                    case "INTERCONNECTION":
                        return "ZONE SEPARATION";
                    case "IRRIGATION":
                        return "SERVICE LINE";
                    case "LATERAL":
                        return "DISTRIBUTION IN GRID";
                    case "MAIN":
                        return "DISTRIBUTION IN GRID";
                    case "SAMPLE POINT":
                        return "OTHER";
                    case "STUB":
                        return "DIST DEAD END";
                    case "TRANSMISSION":
                        return "TRANSMISSION";
                    default:
                        return string.Empty;
                }
            }
        }

        //bug-3689
        public virtual string StreetValveType
        {
            get
            {
                switch (ValveControl)
                {
                    case "AIR RELEASE":
                        return "DIST (M)";
                    case "BLOW OFF":
                        return "BLOWOFF(M)";
                    case "BLOW OFF WITH FLUSHING":
                        return "BLOWOFF(M)";
                    case "BY-PASS":
                        return "DIST (M)";
                    case "CONTROL VALVE":
                        return "DIST (M)";
                    case "DOMESTIC SERVICE":
                        return "SERV (M)";
                    case "FIRE SERVICE":
                        return "SERV (M)";
                    case "GRADIENT SEPARATION":
                        return "DIST (M)";
                    case "HYDRANT":
                        return "HYD AUX (M)";
                    case "INSERTION":
                        return "DIST (M)";
                    case "INTERCONNECTION":
                        return "DIST (M)";
                    case "IRRIGATION":
                        return "SERV (M)";
                    case "LATERAL":
                        return "DIST (M)";
                    case "MAIN":
                        return "DIST (M)";
                    case "SAMPLE POINT":
                        return "DIST (M)";
                    case "STUB":
                        return "DIST (M)";
                    case "TRANSMISSION":
                        return "TRANS (M)";
                    default:
                        return string.Empty;
                }
            }
        }

        //Added to fix Production Equipment issue
        public virtual string ParentEquipment { get; set; }

        #endregion

        #region WebService Response Properties

        public virtual string SAPEquipmentNumber { get; set; }
        public virtual string SAPErrorCode { get; set; }
        public virtual string Status { get; set; } // is this one needed?

        #endregion

        #region WebService Request Properties

        public virtual string EquipmentCategory { get; set; } //H- Hydrant , V-Valve, 4-Opening
        public virtual string InventoryNumber { get; set; } //As is- Asset ID from Mapcall for Hydrant / Valve/ Opening
        public virtual string AssetType { get; set; }
        public virtual string ValveControl { get; set; }
        public virtual string ReferenceEquipmentNumber { get; set; }
        public virtual string Class { get; set; }
        public virtual string FunctionalLocID { get; set; }
        public virtual string AuthGroup { get; set; }
        public virtual DateTime? StartUpDate { get; set; }
        public virtual string Manufacturer { get; set; }
        public virtual string Model { get; set; }
        public virtual int? YearManufactured { get; set; } //Constr.yr
        public virtual string MonthManufactured { get; set; } //Month Manufactured
        public virtual bool Critical { get; set; } //Month Manufactured
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
        public virtual string Street5 { get; set; } //added new 
        public virtual string City { get; set; }
        public virtual string OtherCity { get; set; }
        public virtual string State { get; set; }
        public virtual string Country { get; set; }
        public virtual string CityPostalCode { get; set; } //from map call
        public virtual string Latitude { get; set; } //from map call
        public virtual string Longitude { get; set; } //from map call

        public virtual DateTime? ValidFrom { get; set; }
        public virtual DateTime? ValidTo { get; set; }
        public virtual DateTime DateOnWhichRecordWasCreated { get; set; }
        public virtual string SizeDimension { get; set; } //from map call
        public virtual Double AcquisitionValue { get; set; } //from map call
        public virtual string CurrencyKey { get; set; }
        public virtual DateTime AcquisitionDate { get; set; } //from map call
        public virtual string ManufacturerSerialNumber { get; set; } //from map call
        public virtual string ManufacturerPartNumber { get; set; } //from map call
        public virtual string ManufacturerDrawingNumber { get; set; } //from map call
        public virtual DateTime WarrantyStartDate { get; set; } //from map call
        public virtual DateTime WarrantyEndDate { get; set; } //from map call
        public virtual string Location { get; set; } //from map call
        public virtual string Room { get; set; } //from map call
        public virtual string PlantSection { get; set; } //from map call
        public virtual string TechnicalIdentificationNumber { get; set; } //from map call

        public virtual string MapcallRecordURL { get; set; } //Mapcall URL
        public virtual string EquipmentStatus { get; set; } //Mapcall URL
        public virtual short EquipmentTypeId { get; set; }
        public virtual string ABCIndicatorMAPCALL { get; set; }

        public virtual string Permits { get; set; }

        #region Characteristics

        public virtual HydrantCharacteristics HydrantCharacteristics { get; set; }
        public virtual ValveCharacteristics ValveCharacteristics { get; set; }
        public virtual BlowOffCharacteristics BlowOffCharacteristics { get; set; }
        public virtual OpeningCharacteristics OpeningCharacteristics { get; set; }
        public virtual SAPEquipmentsClassENG SapEquipmentsClassENG { get; set; }
        public virtual SAPEquipmentsClassGEN SapEquipmentsClassGEN { get; set; }
        public virtual SAPEquipmentsClassMOT SapEquipmentsClassMOT { get; set; }
        public virtual SAPEquipmentsClassPMP_CENT SapEquipmentsClassPMP_CENT { get; set; }
        public virtual SAPEquipmentsClassPMP_GRND SapEquipmentsClassPMP_GRND { get; set; }
        public virtual SAPEquipmentsClassPMP_PD SapEquipmentsClassPMP_PD { get; set; }
        public virtual SAPEquipmentsClassRTU_PLC SapEquipmentsClassRTU_PLC { get; set; }
        public virtual SAPEquipmentsClassTNK_CHEM SapEquipmentsClassTNK_CHEM { get; set; }
        public virtual SAPEquipmentsClassTNK_FUEL SapEquipmentsClassTNK_FUEL { get; set; }
        public virtual SAPEquipmentsClassTNK_PVAC SapEquipmentsClassTNK_PVAC { get; set; }
        public virtual SAPEquipmentsClassTNK_WNON SapEquipmentsClassTNK_WNON { get; set; }
        public virtual SAPEquipmentsClassTNK_WPOT SapEquipmentsClassTNK_WPOT { get; set; }
        public virtual SAPEquipmentsClassTNK_WSTE SapEquipmentsClassTNK_WSTE { get; set; }
        public virtual IList<SAPEquipmentClassGeneric> SapEquipmentClassGeneric { get; set; }

        #endregion

        #endregion

        #endregion

        #region Constructors

        // TODO: Unit test these constructors
        public SAPEquipment(Hydrant hydrant)
        {
            string DepthBuryFeet, DepthBuryInches, BranchLengthFeet, BranchLengthInches;

            DepthBuryFeet = hydrant.DepthBuryFeet != null ? hydrant.DepthBuryFeet.ToString() + "FT " : "";
            DepthBuryInches = hydrant.DepthBuryInches != null ? hydrant.DepthBuryInches.ToString() + "IN" : "";

            BranchLengthFeet = hydrant.BranchLengthFeet != null ? hydrant.BranchLengthFeet.ToString() + "FT " : "";
            BranchLengthInches = hydrant.BranchLengthInches != null ? hydrant.BranchLengthInches.ToString() + "IN" : "";

            InventoryNumber = hydrant.HydrantNumber;
            EquipmentCategory = "H";
            AssetType = ProductionAssetType.Descriptions.HYDRANT.ToUpper();
            SAPEquipmentNumber = (hydrant.SAPEquipmentId != null && hydrant.SAPEquipmentId != 0)
                ? hydrant.SAPEquipmentNumber.ToString()
                : string.Empty;
            ReferenceEquipmentNumber =
                "REFERENCE-HYD"; //required for Reference Equipment No -Hydrant (REFERENCE-HYD); Street Valve (REFERENCE-SVLV); Blow Off valve (REFERENCE-SVLV-BO) and Openings (REFERENCE-MH)
            FunctionalLocID = hydrant.FunctionalLocation?.Description;
            StartUpDate = hydrant.DateInstalled;
            Manufacturer = hydrant.HydrantManufacturer?.Description;
            Model = hydrant.HydrantModel?.Description?.ToUpper();
            YearManufactured = hydrant.YearManufactured;
            MonthManufactured =
                hydrant.YearManufactured != null
                    ? "01"
                    : null; //defualt to - 01 if "Year Manufactured" is provided by Mapcall
            Critical = hydrant
               .Critical; // required for ABC Indicator (Default ‘M’ or based on the criticality from Mapcall)
            House = hydrant.StreetNumber;
            Street1 = hydrant.Street?.FullStName.ToUpper();
            Street2 = hydrant.CrossStreet?.FullStName.ToUpper();
            Street5 = hydrant.Town?.County.Name.ToUpper();
            City = hydrant.Town?.ShortName.ToUpper();
            OtherCity = hydrant.TownSection?.Description;
            State = hydrant.Town?.State.Abbreviation.ToUpper();
            Country = "US";
            CityPostalCode = hydrant.Town?.Zip;
            Latitude = hydrant.Coordinate?.Latitude.ToString();
            Longitude = hydrant.Coordinate?.Longitude.ToString();
            EquipmentStatus = hydrant.Status?.Description.ToUpper();
            MapcallRecordURL = GetShowUrl("Hydrant", hydrant.Id);
            SizeDimension = hydrant.HydrantSize != null
                ? string.Format(CommonStringFormats.DECIMAL_WITHOUT_TRAILING_ZEROS, hydrant.HydrantSize.Size)
                : null;
            HydrantCharacteristics = new HydrantCharacteristics {
                SPECIAL_MAINT_NOTES_DETAILS = hydrant.CriticalNotes?.ToUpper(),
                PRESSURE_ZONE = hydrant.Gradient?.Description.ToUpper(),
                MAP_PAGE = hydrant.MapPage?.ToUpper(),
                OPEN_DIRECTION = hydrant.OpenDirection?.Description.ToUpper(),
                HYD_BARREL_SIZE = hydrant.HydrantSize != null
                    ? string.Format(CommonStringFormats.DECIMAL_WITHOUT_TRAILING_ZEROS, hydrant.HydrantSize.Size)
                    : null,
                EAM_HYD_BURY_DEPTH = DepthBuryFeet + DepthBuryInches,
                HYD_BRANCH_LENGTH = BranchLengthFeet + BranchLengthInches,
                HYD_AUX_VALVE_BRANCH_SIZE = hydrant.LateralSize != null
                    ? string.Format(CommonStringFormats.DECIMAL_WITHOUT_TRAILING_ZEROS, hydrant.LateralSize.Size)
                    : null,
                HYD_DEAD_END_MAIN = hydrant.IsDeadEndMain == true ? "Y" : "N",
                HYD_AUX_VALVENUM = hydrant.LateralValve?.ValveNumber,
                HYD_STEAMER_THREAD_TP = hydrant.HydrantThreadType?.Description.ToUpper(),
                EAM_PIPE_SIZE = hydrant.HydrantMainSize != null
                    ? string.Format(CommonStringFormats.DECIMAL_WITHOUT_TRAILING_ZEROS, hydrant.HydrantMainSize.Size)
                    : null,
                PIPE_MATERIAL = hydrant.MainType?.SAPCode.ToUpper() == "GV"
                    ? "GALV"
                    : hydrant.MainType?.SAPCode.ToUpper(), //- prod issue fixed on 13th July
                INSTALLATION_WO = hydrant.WorkOrderNumber,
                HYD_FIRE_DISTRICT = hydrant.FireDistrict?.DistrictName.ToUpper(),
                HYD_ACCOUNT = hydrant.FireDistrict?.PremiseNumber,
                HYD_BILLING_TP = hydrant.HydrantBilling?.Description.ToUpper(),
                HISTORICAL_ID = hydrant.Id.ToString(),
                HYD_TYP = hydrant.HydrantType?.SAPCode,
                HYD_OUTLET_CONFIG = hydrant.HydrantOutletConfiguration?.SAPCode
            };
        }

        public SAPEquipment(Valve valve)
        {
            InventoryNumber = valve.ValveNumber;
            EquipmentCategory = "V";
            SAPEquipmentNumber = (valve.SAPEquipmentId != null && valve.SAPEquipmentId != 0)
                ? valve.SAPEquipmentNumber.ToString()
                : string.Empty;
            AssetType = ProductionAssetType.Descriptions.VALVE.ToUpper();
            FunctionalLocID = valve.FunctionalLocation?.Description;
            StartUpDate = valve.DateInstalled;
            Manufacturer = valve.ValveMake?.Description.ToUpper();
            //MonthManufactured = "01"; //defualt to - 01 if "Year Manufactured" is provided by Mapcall
            Critical = valve
               .Critical; // required for ABC Indicator (Default ‘M’ or based on the criticality from Mapcall)
            House = valve.StreetNumber;
            Street1 = valve.Street?.FullStName.ToUpper();
            Street2 = valve.CrossStreet?.FullStName.ToUpper();
            Street5 = valve.Town?.County.Name.ToUpper();
            City = valve.Town?.ShortName.ToUpper();
            OtherCity = valve.TownSection?.Description.ToUpper();
            State = valve.Town?.State.Abbreviation.ToUpper();
            Country = "US";
            CityPostalCode = valve.Town?.Zip;
            Latitude = valve.Coordinate?.Latitude.ToString();
            Longitude = valve.Coordinate?.Longitude.ToString();
            EquipmentStatus = valve.Status?.Description.ToUpper();
            MapcallRecordURL = GetShowUrl("Valve", valve.Id);
            SizeDimension = valve.ValveSize != null
                ? string.Format(CommonStringFormats.DECIMAL_WITHOUT_TRAILING_ZEROS, valve.ValveSize.Size)
                : null;
            ValveControl = valve.ValveControls?.Description?.ToUpper();

            var topNutDepth = string.Join(" ", new[] {
                valve.DepthFeet != null ? valve.DepthFeet + "FT" : "",
                valve.DepthInches != null ? valve.DepthInches + "IN" : ""
            });

            if (valve.ValveControls?.Description.ToUpper() == "BLOW OFF" ||
                valve.ValveControls?.Description.ToUpper() == "BLOW OFF WITH FLUSHING")
            {
                ReferenceEquipmentNumber = "REFERENCE-SVLV-BO";
                BlowOffCharacteristics = new BlowOffCharacteristics();

                BlowOffCharacteristics.SPECIAL_MAINT_NOTES_DETAILS =
                    valve.CriticalNotes?.ToUpper(); //	Criticality Notes
                BlowOffCharacteristics.SVLV_BO_TYP =
                    StreetValveType; // "BLOWOFF(M)";  //changed as per production issue on 31st mar
                BlowOffCharacteristics.APPLICATION_SVLV_BO =
                    ApplicationValue; //"BLOW-OFF";   changed as per production issue on 31st mar
                BlowOffCharacteristics.MAP_PAGE = valve.MapPage?.ToUpper();
                BlowOffCharacteristics.OPEN_DIRECTION = valve.OpenDirection?.Description.ToUpper();
                BlowOffCharacteristics.NUMBER_OF_TURNS = valve.Turns != null ? Convert.ToDecimal(valve.Turns) : 0;
                BlowOffCharacteristics.NORMAL_POSITION = valve.NormalPosition?.Description.ToUpper();
                BlowOffCharacteristics.VLV_VALVE_SIZE = valve.ValveSize != null
                    ? string.Format(CommonStringFormats.DECIMAL_WITHOUT_TRAILING_ZEROS, valve.ValveSize.Size)
                    : null;
                BlowOffCharacteristics.BYPASS_VALVE = "N";
                BlowOffCharacteristics.EAM_PIPE_SIZE = valve.ValveSize != null
                    ? string.Format(CommonStringFormats.DECIMAL_WITHOUT_TRAILING_ZEROS, valve.ValveSize.Size)
                    : null; //Main Size(inches)
                BlowOffCharacteristics.PIPE_MATERIAL = valve.MainType?.SAPCode.ToUpper() == "GV"
                    ? "GALV"
                    : valve.MainType?.SAPCode.ToUpper(); //Main Type - prod issue fixed on 13th July
                BlowOffCharacteristics.VLV_VALVE_TP = valve.ValveType?.SAPCode.ToUpper(); //	Valve Type
                BlowOffCharacteristics.INSTALLATION_WO = valve.WorkOrderNumber?.ToUpper(); //	WO/WBS #
                BlowOffCharacteristics.HISTORICAL_ID = valve.Id.ToString();
                BlowOffCharacteristics.SKETCH_NUM = valve.SketchNumber?.ToUpper();
                BlowOffCharacteristics.VLV_TOP_NUT_DEPTH = topNutDepth;
            }
            else
            {
                ReferenceEquipmentNumber = "REFERENCE-SVLV";
                ValveCharacteristics = new ValveCharacteristics {
                    SPECIAL_MAINT_NOTES_DETAILS = valve.CriticalNotes?.ToUpper(),
                    SVLV_TYP =
                        StreetValveType, //  valve.ValveControls?.Description.ToUpper() == "HYDRANT"?  "HYD AUX (M)" : "DIST (M)" , - changed as per production bug on 31st Mar
                    APPLICATION_SVLV =
                        ApplicationValue, // (valve.ControlsCrossing ? "CROSSOVER" : (valve.ValveControls ?.Description.ToUpper() == "HYDRANT" ? "HYD AUX" : "DISTRIBUTION IN GRID")),- changed as per production bug on 31st Mar
                    MAP_PAGE = valve.MapPage,
                    OPEN_DIRECTION = valve.OpenDirection?.Description,
                    NUMBER_OF_TURNS = valve.Turns != null ? Convert.ToDecimal(valve.Turns) : 0,
                    NORMAL_POSITION = valve.NormalPosition?.Description,
                    VLV_VALVE_SIZE = valve.ValveSize != null
                        ? string.Format(CommonStringFormats.DECIMAL_WITHOUT_TRAILING_ZEROS, valve.ValveSize.Size)
                        : null,
                    BYPASS_VALVE = valve.ValveControls?.Description.ToUpper() == "BY-PASS" ? "Y" : "N",
                    PIPE_MATERIAL = valve.MainType?.SAPCode.ToUpper() == "GV"
                        ? "GALV"
                        : valve.MainType?.SAPCode.ToUpper(), //Main Type - prod issue fixed on 13th July
                    VLV_VALVE_TP = valve.ValveType?.SAPCode,
                    INSTALLATION_WO = valve.WorkOrderNumber,
                    HISTORICAL_ID = valve.Id.ToString(),
                    SKETCH_NUM = valve.SketchNumber,
                    VLV_TOP_NUT_DEPTH = topNutDepth
                };
            }
        }

        public SAPEquipment(SewerOpening opening)
        {
            InventoryNumber = opening.OpeningNumber;
            EquipmentCategory = "4";
            AssetType = ProductionAssetType.Descriptions.OPENING.ToUpper();
            ReferenceEquipmentNumber = "REFERENCE-MH";
            SAPEquipmentNumber = (opening.SAPEquipmentId != null && opening.SAPEquipmentId != 0)
                ? opening.SAPEquipmentNumber.ToString()
                : string.Empty;
            FunctionalLocID = opening.FunctionalLocation?.Description;
            StartUpDate = opening.DateInstalled;
            //MonthManufactured = "01"; //defualt to - 01 if "Year Manufactured" is provided by Mapcall
            House = opening.StreetNumber;
            Street1 = opening.Street?.FullStName.ToUpper();
            Street2 = opening.IntersectingStreet?.FullStName?.ToUpper();
            Street5 = opening.Town?.County.Name.ToUpper();
            City = opening.Town?.ShortName.ToUpper();
            OtherCity = opening.TownSection?.Description.ToUpper();
            State = opening.Town?.State.Abbreviation.ToUpper();
            Country = "US";
            CityPostalCode = opening.Town?.Zip;
            Latitude = opening.Coordinate?.Latitude.ToString();
            Longitude = opening.Coordinate?.Longitude.ToString();
            EquipmentStatus = opening.Status?.Description.ToUpper();
            MapcallRecordURL = GetShowUrl("SewerOpening", opening.Id);
            Critical = opening.Critical != null ? (bool)opening.Critical : false;
            OpeningCharacteristics = new OpeningCharacteristics {
                SPECIAL_MAINT_NOTES_DETAILS = opening.CriticalNotes?.ToUpper(),
                MATERIAL_OF_CONSTRUCTION_MH = opening.SewerOpeningMaterial?.SAPCode.ToUpper(),
                MAP_PAGE = opening.MapPage?.ToUpper(),
                INSTALLATION_WO = opening.TaskNumber?.ToUpper(),
                MH_DEPTH = opening.DepthToInvert != null ? Convert.ToDecimal(opening.DepthToInvert) : 0,
                HISTORICAL_ID = opening.Id.ToString(),
                MH_TYP = "STANDARD"
            };
        }

        public SAPEquipment(Equipment equipment)
        {
            // type specific fields
            _description = equipment.Description;
            InventoryNumber = equipment.Identifier.ToString();
            SAPEquipmentNumber = (equipment.SAPEquipmentId != null && equipment.SAPEquipmentId != 0)
                ? equipment.SAPEquipmentId.ToString()
                : string.Empty;
            FunctionalLocID = equipment.FunctionalLocation?.ToString();
            Latitude = equipment.Coordinate?.Latitude.ToString();
            Longitude = equipment.Coordinate?.Longitude.ToString();
            EquipmentStatus = equipment.EquipmentStatus?.Description.ToUpper();
            MapcallRecordURL = GetShowUrl("Equipment", equipment.Id);
            Manufacturer = equipment.EquipmentManufacturer?.Description;
            Model = equipment.EquipmentModel?.Description;
            ManufacturerSerialNumber = equipment.SerialNumber;
            //Added to fix Production Equipment issue
            ParentEquipment = equipment.ParentEquipment?.SAPEquipmentId?.ToString();

            StartUpDate = equipment.DateInstalled;
            YearManufactured = equipment.DateInstalled?.Year;
            MonthManufactured =
                equipment.DateInstalled != null
                    ? "01"
                    : null; //defualt to - 01 if "Year Manufactured" is provided by Mapcall
            ABCIndicatorMAPCALL = equipment.ABCIndicator?.Description;
            Permits = GetPermits(equipment);
            if (equipment.EquipmentType != null)
            {
                AssetType = equipment.EquipmentType.ProductionAssetType.Description;
                EquipmentTypeId = (short)equipment.EquipmentType.Id;
                EquipmentCategory = equipment.EquipmentType.EquipmentCategory;
                ReferenceEquipmentNumber = equipment.EquipmentType.ReferenceEquipmentNumber;
                GetCharacteristicValue(equipment);
            }
        }

        public SAPEquipment()
        {
            HydrantCharacteristics = new HydrantCharacteristics();
            OpeningCharacteristics = new OpeningCharacteristics();
            ValveCharacteristics = new ValveCharacteristics();
            BlowOffCharacteristics = new BlowOffCharacteristics();
            SapEquipmentsClassENG = new SAPEquipmentsClassENG();
            SapEquipmentsClassGEN = new SAPEquipmentsClassGEN();
            SapEquipmentsClassMOT = new SAPEquipmentsClassMOT();
            SapEquipmentsClassPMP_CENT = new SAPEquipmentsClassPMP_CENT();
            SapEquipmentsClassPMP_GRND = new SAPEquipmentsClassPMP_GRND();
            SapEquipmentsClassPMP_PD = new SAPEquipmentsClassPMP_PD();
            SapEquipmentsClassRTU_PLC = new SAPEquipmentsClassRTU_PLC();
            SapEquipmentsClassTNK_CHEM = new SAPEquipmentsClassTNK_CHEM();
            SapEquipmentsClassTNK_FUEL = new SAPEquipmentsClassTNK_FUEL();
            SapEquipmentsClassTNK_PVAC = new SAPEquipmentsClassTNK_PVAC();
            SapEquipmentsClassTNK_WNON = new SAPEquipmentsClassTNK_WNON();
            SapEquipmentsClassTNK_WPOT = new SAPEquipmentsClassTNK_WPOT();
            SapEquipmentsClassTNK_WSTE = new SAPEquipmentsClassTNK_WSTE();
        }

        #endregion

        #region Private Methods

        private void AddCharacteristicToCollection(List<EquipmentsEquipmentsAttributeList> list, string property,
            string value)
        {
            list.Add(new EquipmentsEquipmentsAttributeList {
                AttribName = property,
                AttribVal = value
            });
        }

        private void OpeningCharacteristic(EquipmentsEquipments[] equipmentRequest)
        {
            var gc = new List<EquipmentsEquipmentsAttributeList>();
            AddCharacteristicToCollection(gc, nameof(OpeningCharacteristics.SPECIAL_MAINT_NOTES_DETAILS),
                OpeningCharacteristics.SPECIAL_MAINT_NOTES_DETAILS?.ToUpper());
            AddCharacteristicToCollection(gc, nameof(OpeningCharacteristics.MATERIAL_OF_CONSTRUCTION_MH),
                OpeningCharacteristics.MATERIAL_OF_CONSTRUCTION_MH?.ToUpper());
            AddCharacteristicToCollection(gc, nameof(OpeningCharacteristics.MAP_PAGE), OpeningCharacteristics.MAP_PAGE);
            AddCharacteristicToCollection(gc, nameof(OpeningCharacteristics.INSTALLATION_WO),
                OpeningCharacteristics.INSTALLATION_WO?.ToUpper());
            AddCharacteristicToCollection(gc, nameof(OpeningCharacteristics.MH_DEPTH),
                OpeningCharacteristics.MH_DEPTH?.ToString().ToUpper());
            AddCharacteristicToCollection(gc, nameof(OpeningCharacteristics.HISTORICAL_ID),
                OpeningCharacteristics.HISTORICAL_ID);
            AddCharacteristicToCollection(gc, nameof(OpeningCharacteristics.MH_TYP), OpeningCharacteristics.MH_TYP);
            equipmentRequest[0].Class_GENERIC = gc.ToArray();
        }

        private void BlowOffCharacteristic(EquipmentsEquipments[] equipmentRequest)
        {
            var gc = new List<EquipmentsEquipmentsAttributeList>();
            AddCharacteristicToCollection(gc, nameof(BlowOffCharacteristics.SPECIAL_MAINT_NOTES_DETAILS),
                BlowOffCharacteristics
                   .SPECIAL_MAINT_NOTES_DETAILS); //valve.StringLengths.CRITICAL_NOTES	Criticality Notes
            AddCharacteristicToCollection(gc, nameof(BlowOffCharacteristics.SVLV_BO_TYP),
                BlowOffCharacteristics.SVLV_BO_TYP); //valve.ValveControls	Valve Controls
            AddCharacteristicToCollection(gc, nameof(BlowOffCharacteristics.APPLICATION_SVLV_BO),
                BlowOffCharacteristics.APPLICATION_SVLV_BO); //valve.ValveControls	Valve Controls
            AddCharacteristicToCollection(gc, nameof(BlowOffCharacteristics.MAP_PAGE),
                BlowOffCharacteristics.MAP_PAGE); //valve.MapPage	Map Page
            AddCharacteristicToCollection(gc, nameof(BlowOffCharacteristics.OPEN_DIRECTION),
                BlowOffCharacteristics.OPEN_DIRECTION?.ToUpper()); //valve.Opens	Open Direction
            AddCharacteristicToCollection(gc, nameof(BlowOffCharacteristics.NUMBER_OF_TURNS),
                BlowOffCharacteristics.NUMBER_OF_TURNS?.ToString()); //valve.Turns	Number of Turns
            AddCharacteristicToCollection(gc, nameof(BlowOffCharacteristics.NORMAL_POSITION),
                BlowOffCharacteristics.NORMAL_POSITION?.ToUpper()); //valve.InNormalPosition	Normal Position
            AddCharacteristicToCollection(gc, nameof(BlowOffCharacteristics.VLV_VALVE_SIZE),
                BlowOffCharacteristics.VLV_VALVE_SIZE); //valve.ValveSize.size	Valve Size (in)
            AddCharacteristicToCollection(gc, nameof(BlowOffCharacteristics.BYPASS_VALVE),
                BlowOffCharacteristics.BYPASS_VALVE); //valve.ValveControls	Valve Controls
            AddCharacteristicToCollection(gc, nameof(BlowOffCharacteristics.EAM_PIPE_SIZE),
                BlowOffCharacteristics.EAM_PIPE_SIZE); //Main Size(inches)
            AddCharacteristicToCollection(gc, nameof(BlowOffCharacteristics.PIPE_MATERIAL),
                BlowOffCharacteristics.PIPE_MATERIAL); //valve.MainType	Main Type
            AddCharacteristicToCollection(gc, nameof(BlowOffCharacteristics.VLV_VALVE_TP),
                BlowOffCharacteristics.VLV_VALVE_TP); //valve.ValveType	Valve Type
            AddCharacteristicToCollection(gc, nameof(BlowOffCharacteristics.INSTALLATION_WO),
                BlowOffCharacteristics.INSTALLATION_WO); //valve.WorkOrderNumber	WO/WBS #
            AddCharacteristicToCollection(gc, nameof(BlowOffCharacteristics.SKETCH_NUM),
                BlowOffCharacteristics.SKETCH_NUM); //SKETCH_NUM
            AddCharacteristicToCollection(gc, nameof(BlowOffCharacteristics.HISTORICAL_ID),
                BlowOffCharacteristics.HISTORICAL_ID);
            AddCharacteristicToCollection(gc, nameof(BlowOffCharacteristics.VLV_TOP_NUT_DEPTH),
                BlowOffCharacteristics.VLV_TOP_NUT_DEPTH);
            equipmentRequest[0].Class_GENERIC = gc.ToArray();
        }

        private void ValveCharacteristic(EquipmentsEquipments[] equipmentRequest)
        {
            var gc = new List<EquipmentsEquipmentsAttributeList>();
            AddCharacteristicToCollection(gc, nameof(ValveCharacteristics.SPECIAL_MAINT_NOTES_DETAILS),
                ValveCharacteristics.SPECIAL_MAINT_NOTES_DETAILS); //valve.StringLengths.CRITICAL_NOTES
            AddCharacteristicToCollection(gc, nameof(ValveCharacteristics.SVLV_TYP),
                ValveCharacteristics.SVLV_TYP); //valve.ValveControls
            AddCharacteristicToCollection(gc, nameof(ValveCharacteristics.APPLICATION_SVLV),
                ValveCharacteristics.APPLICATION_SVLV); //valve.ValveControls
            AddCharacteristicToCollection(gc, nameof(ValveCharacteristics.MAP_PAGE),
                ValveCharacteristics.MAP_PAGE); //valve.MapPage
            AddCharacteristicToCollection(gc, nameof(ValveCharacteristics.OPEN_DIRECTION),
                ValveCharacteristics.OPEN_DIRECTION?.ToUpper()); //valve.Opens
            AddCharacteristicToCollection(gc, nameof(ValveCharacteristics.NUMBER_OF_TURNS),
                ValveCharacteristics.NUMBER_OF_TURNS?.ToString()); //valve.Turns
            AddCharacteristicToCollection(gc, nameof(ValveCharacteristics.NORMAL_POSITION),
                ValveCharacteristics.NORMAL_POSITION?.ToUpper()); //valve.InNormalPosition
            AddCharacteristicToCollection(gc, nameof(ValveCharacteristics.VLV_VALVE_SIZE),
                ValveCharacteristics.VLV_VALVE_SIZE); //valve.ValveSize.size
            AddCharacteristicToCollection(gc, nameof(ValveCharacteristics.BYPASS_VALVE),
                ValveCharacteristics.BYPASS_VALVE); //valve.ValveControls
            AddCharacteristicToCollection(gc, nameof(ValveCharacteristics.EAM_PIPE_SIZE),
                ValveCharacteristics.EAM_PIPE_SIZE); //
            AddCharacteristicToCollection(gc, nameof(ValveCharacteristics.PIPE_MATERIAL),
                ValveCharacteristics.PIPE_MATERIAL); //valve.MainType
            AddCharacteristicToCollection(gc, nameof(ValveCharacteristics.VLV_VALVE_TP),
                ValveCharacteristics.VLV_VALVE_TP); //valve.ValveType
            AddCharacteristicToCollection(gc, nameof(ValveCharacteristics.INSTALLATION_WO),
                ValveCharacteristics.INSTALLATION_WO); //valve.WorkOrderNumber
            AddCharacteristicToCollection(gc, nameof(ValveCharacteristics.HISTORICAL_ID),
                ValveCharacteristics.HISTORICAL_ID); // valve.ValveNumber
            AddCharacteristicToCollection(gc, nameof(ValveCharacteristics.VLV_TOP_NUT_DEPTH),
                ValveCharacteristics.VLV_TOP_NUT_DEPTH); // valve.ValveNumber
            equipmentRequest[0].Class_GENERIC = gc.ToArray();
        }

        private void HydrantCharacteristic(EquipmentsEquipments[] equipmentRequest)
        {
            // If we have Production Equipment then it doesn't have HydrantCharacteristics,
            // so we pull them from Generic instead
            if (HydrantCharacteristics == null)
            {
                EquipmentsClassGeneric(equipmentRequest);
            }
            else
            {
                var gc = new List<EquipmentsEquipmentsAttributeList>();
                AddCharacteristicToCollection(gc, nameof(HydrantCharacteristics.SPECIAL_MAINT_NOTES_DETAILS),
                    HydrantCharacteristics.SPECIAL_MAINT_NOTES_DETAILS);
                AddCharacteristicToCollection(gc, nameof(HydrantCharacteristics.PRESSURE_ZONE),
                    HydrantCharacteristics?.PRESSURE_ZONE); //hydrant.Gradient
                AddCharacteristicToCollection(gc, nameof(HydrantCharacteristics.MAP_PAGE),
                    HydrantCharacteristics.MAP_PAGE); //hydrant.StringLengths.MAP_PAGE
                AddCharacteristicToCollection(gc, nameof(HydrantCharacteristics.OPEN_DIRECTION),
                    HydrantCharacteristics.OPEN_DIRECTION?.ToUpper()); //hydrant.OpensDirection
                AddCharacteristicToCollection(gc, nameof(HydrantCharacteristics.HYD_BARREL_SIZE),
                    HydrantCharacteristics.HYD_BARREL_SIZE); //hydrant.HydrantSize
                AddCharacteristicToCollection(gc, nameof(HydrantCharacteristics.EAM_HYD_BURY_DEPTH),
                    HydrantCharacteristics.EAM_HYD_BURY_DEPTH); //hydrant.DepthBuryFeet + hydrant.DepthBuryInches
                AddCharacteristicToCollection(gc, nameof(HydrantCharacteristics.HYD_BRANCH_LENGTH),
                    HydrantCharacteristics.HYD_BRANCH_LENGTH); //hydrant.BranchLengthFeet + hydrant.BranchLengthInches
                AddCharacteristicToCollection(gc, nameof(HydrantCharacteristics.HYD_AUX_VALVE_BRANCH_SIZE),
                    HydrantCharacteristics.HYD_AUX_VALVE_BRANCH_SIZE); //hydrant.LateralSize
                AddCharacteristicToCollection(gc, nameof(HydrantCharacteristics.HYD_DEAD_END_MAIN),
                    HydrantCharacteristics.HYD_DEAD_END_MAIN); //hydrant.IsDeadEndMain
                AddCharacteristicToCollection(gc, nameof(HydrantCharacteristics.HYD_AUX_VALVENUM),
                    HydrantCharacteristics.HYD_AUX_VALVENUM?.ToUpper()); //hydrant.LateralSize
                AddCharacteristicToCollection(gc, nameof(HydrantCharacteristics.HYD_STEAMER_THREAD_TP),
                    HydrantCharacteristics.SteamerThreadType?.ToUpper()); //HYD_STEAMER_THREAD_TP
                AddCharacteristicToCollection(gc, nameof(HydrantCharacteristics.EAM_PIPE_SIZE),
                    HydrantCharacteristics.EAM_PIPE_SIZE); //hydrant.HydrantMainSize
                AddCharacteristicToCollection(gc, nameof(HydrantCharacteristics.PIPE_MATERIAL),
                    HydrantCharacteristics.PIPE_MATERIAL?.ToUpper()); //hydrant.MainType
                AddCharacteristicToCollection(gc, nameof(HydrantCharacteristics.INSTALLATION_WO),
                    HydrantCharacteristics.INSTALLATION_WO?.ToUpper()); //hydrant.WorkOrderNumber
                AddCharacteristicToCollection(gc, nameof(HydrantCharacteristics.HYD_FIRE_DISTRICT),
                    HydrantCharacteristics.HYD_FIRE_DISTRICT?.ToUpper()); //hydrant.FireDistrict
                AddCharacteristicToCollection(gc, nameof(HydrantCharacteristics.HYD_ACCOUNT),
                    HydrantCharacteristics.HYD_ACCOUNT); //hydrant.StringLengths.PREMISE_NUMBER
                AddCharacteristicToCollection(gc, nameof(HydrantCharacteristics.HYD_BILLING_TP),
                    HydrantCharacteristics.HydrantBillingType?.ToUpper()); //hydrant.HydrantBilling
                AddCharacteristicToCollection(gc, nameof(HydrantCharacteristics.HISTORICAL_ID),
                    HydrantCharacteristics.HISTORICAL_ID); //hydrant.HydrantNumber
                AddCharacteristicToCollection(gc, nameof(HydrantCharacteristics.HYD_AUXILLARY_VALVE),
                    HydrantCharacteristics.HYD_AUXILLARY_VALVE);
                AddCharacteristicToCollection(gc, nameof(HydrantCharacteristics.HYD_TYP),
                    HydrantCharacteristics.HYD_TYP);
                AddCharacteristicToCollection(gc, nameof(HydrantCharacteristics.HYD_OUTLET_CONFIG),
                    HydrantCharacteristics.HYD_OUTLET_CONFIG);
                equipmentRequest[0].Class_GENERIC = gc.ToArray();
            }
        }

        private void EquipmentsClassGeneric(EquipmentsEquipments[] equipmentRequest)
        {
            int count = SapEquipmentClassGeneric.ToList().Count;
            //equipmentRequest[0].Class_GENERIC = new EquipmentsEquipmentsAttributeList[count];
            EquipmentsEquipmentsAttributeList[] genericAttribute = new EquipmentsEquipmentsAttributeList[count];
            for (int i = 0; i < count; i++)
            {
                genericAttribute[i] = new EquipmentsEquipmentsAttributeList();
                genericAttribute[i].AttribName = SapEquipmentClassGeneric.ToList()[i].FieldName;
                genericAttribute[i].AttribVal = SapEquipmentClassGeneric.ToList()[i].Value;
            }

            equipmentRequest[0].Class_GENERIC = genericAttribute;
        }

        private void GetCharacteristicValue(Equipment equipment)
        {
            //Critical notes in MapCall has to be mapped with Special maintenance note Details charactristic in SAP.
            var sapEquipmentClassGeneric = new SAPEquipmentClassGeneric {
                FieldName = "SPECIAL_MAINT_NOTES_DETAILS",
                Value = equipment.CriticalNotes
            };

            var equipmentClassGeneric = from c in equipment.ActiveCharacteristics
                                        where c.Field?.FieldName != "SPECIAL_MAINT_NOTES_DETAILS"
                                        select new SAPEquipmentClassGeneric {
                                            Value = c.DisplayValue,
                                            FieldName = c.Field?.FieldName
                                        };
            // if we don't have this number then we're creating new equipment, don't 
            // send risk characteristics unless we do.
            if (!string.IsNullOrWhiteSpace(SAPEquipmentNumber))
            {
                equipmentClassGeneric = AddRiskCharacteristics(equipmentClassGeneric, equipment);
            }

            SapEquipmentClassGeneric = equipmentClassGeneric.ToList();
            SapEquipmentClassGeneric.Add(sapEquipmentClassGeneric);
        }

        private IEnumerable<SAPEquipmentClassGeneric> AddRiskCharacteristics(
            IEnumerable<SAPEquipmentClassGeneric> equipmentClassGeneric, Equipment equipment)
        {
            var ret = equipmentClassGeneric.ToList();
            ret.Add(new SAPEquipmentClassGeneric
                {FieldName = "EQUIP_CONDITION", Value = equipment.Condition?.ToString()});
            ret.Add(new SAPEquipmentClassGeneric
                {FieldName = "EQUIPMENT_TYPE", Value = equipment.StaticDynamicType?.ToString()});
            ret.Add(new SAPEquipmentClassGeneric
                {FieldName = "EQUIPMENT_RISK_FAILURE_RATING", Value = equipment.RiskOfFailure?.ToString()});
            ret.Add(new SAPEquipmentClassGeneric
                {FieldName = "EQUIP_RISK_OF_FAIL_SCORE", Value = equipment.LocalizedRiskOfFailureText});
            ret.Add(new SAPEquipmentClassGeneric
                {FieldName = "EQUIPMENT_RELIABILITY", Value = equipment.Reliability?.ToString()});
            ret.Add(new SAPEquipmentClassGeneric
                {FieldName = "EQUIP_PERFORMANCE", Value = equipment.Performance?.ToString()});
            ret.Add(new SAPEquipmentClassGeneric
                {FieldName = "EQUIPMENT_LIKELIHOOD_FAILURE", Value = equipment.LikelyhoodOfFailure?.ToString()});
            ret.Add(new SAPEquipmentClassGeneric
                {FieldName = "EQUIPMENT_CONSEQUENCE_FAILURE", Value = equipment.ConsequenceOfFailure?.ToString()});
            return ret;
        }

        #endregion

        #region Exposed Methods

        public EquipmentsEquipments[] ToEquipmentsEquipments()
        {
            EquipmentsEquipments[] EquipmentRequest = new EquipmentsEquipments[1];

            EquipmentRequest[0] = new EquipmentsEquipments();
            //{
            EquipmentRequest[0].Indicator = AddEditIndicator?.ToUpper();
            EquipmentRequest[0].EquipmentCategory = EquipmentCategory?.ToUpper();
            EquipmentRequest[0].InventoryNumber = InventoryNumber;
            EquipmentRequest[0].SAPEquipmentNumber = SAPEquipmentNumber;
            EquipmentRequest[0].Description = Description?.ToUpper();
            EquipmentRequest[0].RefEquipmentNo = ReferenceEquipmentNumber?.ToUpper();
            EquipmentRequest[0].FunctionalLocID = FunctionalLocID?.ToUpper();
            EquipmentRequest[0].StartUpDate = StartUpDate?.Date.ToString(SAP_DATE_FORMAT);
            EquipmentRequest[0].Manufacturer = Manufacturer?.ToUpper();
            EquipmentRequest[0].Model = Model?.ToString();
            EquipmentRequest[0].YearManufactured = YearManufactured?.ToString();
            EquipmentRequest[0].MonthManufactured = MonthManufactured?.ToString();
            EquipmentRequest[0].ABCIndicator =
                AssetType == ProductionAssetType.Descriptions.MECHANICAL || AssetType == ProductionAssetType.Descriptions.ELECTRICAL || AssetType == ProductionAssetType.Descriptions.TANKS
                    ? ABCIndicatorForEquipment
                    : ABCIndicator;
            EquipmentRequest[0].House = House?.ToUpper();
            EquipmentRequest[0].Street1 = Street1?.ToUpper();
            EquipmentRequest[0].Street2 = Street2?.ToUpper();
            EquipmentRequest[0].Street5 = Street5?.ToUpper();
            EquipmentRequest[0].City = City?.ToUpper();
            EquipmentRequest[0].OtherCity = OtherCity?.ToUpper();
            EquipmentRequest[0].State = State?.ToUpper();
            EquipmentRequest[0].Country = Country?.ToUpper();
            EquipmentRequest[0].ZipCode = CityPostalCode;
            EquipmentRequest[0].Latitude = Latitude;
            EquipmentRequest[0].Longitude = Longitude;
            EquipmentRequest[0].EquipmentUserStatus = USER_SYSTEM_STATUS_FOR_EQUIPMENT_ASSET_TYPES.Contains(AssetType)
                ? UserStatusForEquipment
                : EquipmentUserStatus;
            EquipmentRequest[0].EquipmentSystemStatus = USER_SYSTEM_STATUS_FOR_EQUIPMENT_ASSET_TYPES.Contains(AssetType)
                ? SystemStatusForEquipment
                : EquipmentSystemStatus;
            if (EquipmentRequest[0].EquipmentUserStatus == "INSV" && !string.IsNullOrWhiteSpace(ParentEquipment) &&
                EquipmentStatus == "IN SERVICE")
                EquipmentRequest[0].EquipmentSystemStatus = "ASEQ";

            EquipmentRequest[0].Size = SizeDimension?.ToString();
            EquipmentRequest[0].Location = Location?.ToString();
            EquipmentRequest[0].MapCallRecordURL = MapcallRecordURL; //Not avaliable in SAP web service// };
            EquipmentRequest[0].ManufacturerSerialNo = ManufacturerSerialNumber;
            EquipmentRequest[0].Permit = Permits;
            //Added to fix Production Equipment issue
            EquipmentRequest[0].SuperOrdEquip = ParentEquipment;

            //if condition added as part of prod defect on 11th Aug 2017

            if (AssetType != null)
            {
                switch (AssetType.ToUpper())
                {
                    case ProductionAssetType.DescriptionsUppercase.HYDRANT:
                        HydrantCharacteristic(EquipmentRequest);
                        break;
                    case ProductionAssetType.DescriptionsUppercase.OPENING:
                        OpeningCharacteristic(EquipmentRequest);
                        break;
                    case ProductionAssetType.DescriptionsUppercase.VALVE:
                        if (ValveControl?.ToUpper() == "BLOW OFF" ||
                            ValveControl?.ToUpper() == "BLOW OFF WITH FLUSHING")
                            BlowOffCharacteristic(EquipmentRequest);
                        else
                            ValveCharacteristic(EquipmentRequest);
                        break;
                    case ProductionAssetType.DescriptionsUppercase.MECHANICAL:
                    case ProductionAssetType.DescriptionsUppercase.ELECTRICAL:
                    case ProductionAssetType.DescriptionsUppercase.TANKS:
                    case ProductionAssetType.DescriptionsUppercase.SEWER:
                    case ProductionAssetType.DescriptionsUppercase.VEHICLE:
                        if ((EquipmentTypeId == EquipmentType.Indices.ENG)
                            || (EquipmentTypeId == EquipmentType.Indices.GEN)
                            || (EquipmentTypeId == EquipmentType.Indices.MOT)
                            || (EquipmentTypeId == EquipmentType.Indices.PMP_CENT)
                            || (EquipmentTypeId == EquipmentType.Indices.PMP_GRND)
                            || (EquipmentTypeId == EquipmentType.Indices.PMP_PD)
                            || (EquipmentTypeId == EquipmentType.Indices.RTU_PLC)
                            || (EquipmentTypeId == EquipmentType.Indices.TNK_CHEM)
                            || (EquipmentTypeId == EquipmentType.Indices.TNK_FUEL)
                            || (EquipmentTypeId == EquipmentType.Indices.TNK_PVAC)
                            || (EquipmentTypeId == EquipmentType.Indices.TNK_WNON)
                            || (EquipmentTypeId == EquipmentType.Indices.TNK_WPOT)
                            || (EquipmentTypeId == EquipmentType.Indices.TNK_WSTE)
                            || (EquipmentTypeId == EquipmentType.Indices.PDMTOOL)
                            || (EquipmentTypeId == EquipmentType.Indices.SCRBBR)
                            || (EquipmentTypeId == EquipmentType.Indices.SCALE)
                            || (EquipmentTypeId == EquipmentType.Indices.PPE_ARC)
                            || (EquipmentTypeId == EquipmentType.Indices.TRT_UV)
                            || (EquipmentTypeId == EquipmentType.Indices.PWRFEEDR)
                            || (EquipmentTypeId == EquipmentType.Indices.HVAC_VNT)
                            || (EquipmentTypeId == EquipmentType.Indices.PRNTR)
                            || (EquipmentTypeId == EquipmentType.Indices.RECORDER)
                            || (EquipmentTypeId == EquipmentType.Indices.XFMR)
                            || (EquipmentTypeId == EquipmentType.Indices.PWRSURG)
                            || (EquipmentTypeId == EquipmentType.Indices.COMM_TEL)
                            || (EquipmentTypeId == EquipmentType.Indices.MIXR)
                            || (EquipmentTypeId == EquipmentType.Indices.KIT)
                            || (EquipmentTypeId == EquipmentType.Indices.INST_SW)
                            || (EquipmentTypeId == EquipmentType.Indices.TRT_CLAR)
                            || (EquipmentTypeId == EquipmentType.Indices.HVAC_HTR)
                            || (EquipmentTypeId == EquipmentType.Indices.FIRE_SUP)
                            || (EquipmentTypeId == EquipmentType.Indices.BATT)
                            || (EquipmentTypeId == EquipmentType.Indices.DAM)
                            || (EquipmentTypeId == EquipmentType.Indices.CALIB)
                            || (EquipmentTypeId == EquipmentType.Indices.BURNER)
                            || (EquipmentTypeId == EquipmentType.Indices.BOILER)
                            || (EquipmentTypeId == EquipmentType.Indices.HVAC_DHM)
                            || (EquipmentTypeId == EquipmentType.Indices.TRT_STRP)
                            || (EquipmentTypeId == EquipmentType.Indices.CHMF_LIQ)
                            || (EquipmentTypeId == EquipmentType.Indices.COMM_FWL)
                            || (EquipmentTypeId == EquipmentType.Indices.CHMF_DRY)
                            || (EquipmentTypeId == EquipmentType.Indices.CHEM_PIP)
                            || (EquipmentTypeId == EquipmentType.Indices.CHEM_GEN)
                            || (EquipmentTypeId == EquipmentType.Indices.HVAC_CMB)
                            || (EquipmentTypeId == EquipmentType.Indices.TRT_SOFT)
                            || (EquipmentTypeId == EquipmentType.Indices.HVAC_CHL)
                            || (EquipmentTypeId == EquipmentType.Indices.TRT_FILT)
                            || (EquipmentTypeId == EquipmentType.Indices.HOIST)
                            || (EquipmentTypeId == EquipmentType.Indices.CATHODIC)
                            || (EquipmentTypeId == EquipmentType.Indices.TRT_AER)
                            || (EquipmentTypeId == EquipmentType.Indices.TRAN_SW)
                            || (EquipmentTypeId == EquipmentType.Indices.LK_MON)
                            || (EquipmentTypeId == EquipmentType.Indices.LABEQ)
                            || (EquipmentTypeId == EquipmentType.Indices.SCADASYS)
                            || (EquipmentTypeId == EquipmentType.Indices.SAF_SHWR)
                            || (EquipmentTypeId == EquipmentType.Indices.SAFGASDT)
                            || (EquipmentTypeId == EquipmentType.Indices.TRT_CONT)
                            || (EquipmentTypeId == EquipmentType.Indices.GRINDER)
                            || (EquipmentTypeId == EquipmentType.Indices.PWRRELAY)
                            || (EquipmentTypeId == EquipmentType.Indices.PWRPNL)
                            || (EquipmentTypeId == EquipmentType.Indices.PWRMON)
                            || (EquipmentTypeId == EquipmentType.Indices.HVAC_TWR)
                            || (EquipmentTypeId == EquipmentType.Indices.PHASECON)
                            || (EquipmentTypeId == EquipmentType.Indices.TOOL)
                            || (EquipmentTypeId == EquipmentType.Indices.FIRE_EX)
                            || (EquipmentTypeId == EquipmentType.Indices.AED)
                            || (EquipmentTypeId == EquipmentType.Indices.WQANLZR)
                            || (EquipmentTypeId == EquipmentType.Indices.WELL)
                            || (EquipmentTypeId == EquipmentType.Indices.XMTR)
                            || (EquipmentTypeId == EquipmentType.Indices.PWRDISC)
                            || (EquipmentTypeId == EquipmentType.Indices.PWRCOND)
                            || (EquipmentTypeId == EquipmentType.Indices.ADJSPD)
                            || (EquipmentTypeId == EquipmentType.Indices.PC)
                            || (EquipmentTypeId == EquipmentType.Indices.TOOL)
                            || (EquipmentTypeId == EquipmentType.Indices.OIT)
                            || (EquipmentTypeId == EquipmentType.Indices.INDICATR)
                            || (EquipmentTypeId == EquipmentType.Indices.HVAC_WH)
                            || (EquipmentTypeId == EquipmentType.Indices.PRESDMP)
                            || (EquipmentTypeId == EquipmentType.Indices.SERVR)
                            || (EquipmentTypeId == EquipmentType.Indices.UPS)
                            || (EquipmentTypeId == EquipmentType.Indices.PWRBRKR)
                            || (EquipmentTypeId == EquipmentType.Indices.PVLV)
                            || (EquipmentTypeId == EquipmentType.Indices.ELIGHT)
                            || (EquipmentTypeId == EquipmentType.Indices.MOTSTR)
                            || (EquipmentTypeId == EquipmentType.Indices.PPE_RESP)
                            || (EquipmentTypeId == EquipmentType.Indices.SECSYS)
                            || (EquipmentTypeId == EquipmentType.Indices.PPE_FLOT)
                            || (EquipmentTypeId == EquipmentType.Indices.SCREEN)
                            || (EquipmentTypeId == EquipmentType.Indices.PPE_FALL)
                            || (EquipmentTypeId == EquipmentType.Indices.FIRE_AL)
                            || (EquipmentTypeId == EquipmentType.Indices.FACILITY)
                            || (EquipmentTypeId == EquipmentType.Indices.EYEWASH)
                            || (EquipmentTypeId == EquipmentType.Indices.BLWR)
                            || (EquipmentTypeId == EquipmentType.Indices.BATTCHGR)
                            || (EquipmentTypeId == EquipmentType.Indices.GEARBOX)
                            || (EquipmentTypeId == EquipmentType.Indices.CONVEYOR)
                            || (EquipmentTypeId == EquipmentType.Indices.FLO_WEIR)
                            || (EquipmentTypeId == EquipmentType.Indices.ELEVATOR)
                            || (EquipmentTypeId == EquipmentType.Indices.CONTAIN)
                            || (EquipmentTypeId == EquipmentType.Indices.FLO_MET)
                            || (EquipmentTypeId == EquipmentType.Indices.CONTACTR)
                            || (EquipmentTypeId == EquipmentType.Indices.COMM_SW)
                            || (EquipmentTypeId == EquipmentType.Indices.COMM_RTR)
                            || (EquipmentTypeId == EquipmentType.Indices.COMM_RAD)
                            || (EquipmentTypeId == EquipmentType.Indices.CHMF_GAS)
                            || (EquipmentTypeId == EquipmentType.Indices.HVAC_EXC)
                            || (EquipmentTypeId == EquipmentType.Indices.COMP)
                            || (EquipmentTypeId == EquipmentType.Indices.COMM_MOD)
                            || (EquipmentTypeId == EquipmentType.Indices.CNTRLR)
                            || (EquipmentTypeId == EquipmentType.Indices.CNTRLPNL)
                            || (EquipmentTypeId == EquipmentType.Indices.UV_SOUND)
                            || (EquipmentTypeId == EquipmentType.Indices.CO)
                            || (EquipmentTypeId == EquipmentType.Indices.VEH))
                            EquipmentsClassGeneric(EquipmentRequest);

                        break;
                    default:
                        throw new InvalidOperationException("Unknown Asset Type was used");
                }
            }

            return EquipmentRequest;
        }

        #endregion
    }
}
