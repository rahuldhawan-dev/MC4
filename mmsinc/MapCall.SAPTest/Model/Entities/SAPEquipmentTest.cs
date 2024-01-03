using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using Moq;
using StructureMap;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Migrations._2016;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;

namespace MapCall.SAP.Model.Entities.Tests
{
    [TestClass]
    public class SapEquipmentTest
    {
        #region Private Members

        private Mock<IDateTimeProvider> _dateTimeProvider;
        private Valve valve;
        private Hydrant hydrant;
        private SewerOpening _opening;
        private IContainer _container;

        #endregion

        #region Private Methods

        public Hydrant GetTestHydrant()
        {
            var hydrant = new Hydrant {
                HydrantNumber = "HBH-96", // Inventory Number //HISTORICAL_ID
                //SAPEquipmentId = 20004237,//20001049,
                //SAPEquipmentId = 20170862,
                FunctionalLocation = new FunctionalLocation
                    {Description = "NJOC-BH-HYDRT"}, //"NJLK-HW-HYDRT", //Functional Location ID
                DateInstalled = Convert.ToDateTime("11/29/2016 12:00:00 AM"), //3/20/2013 Start Up date
                HydrantManufacturer = new HydrantManufacturer {Description = "AMERICAN"}, //Manufacturer
                HydrantModel = new HydrantModel {Description = ""}, //Model
                //YearManufactured = 2015,//2013,//Constr.yr
                Critical = true, //ABCIndicator = "M",
                StreetNumber = "963",
                Street = new Street {FullStName = "E FLORENCE CIR"},
                CrossStreet = new Street {FullStName = "STATE HWY 33"},
                Town = new Town {
                    ShortName = "LAKEWOOD", County = new County {Name = "US"}, State = new State {Abbreviation = "NJ"},
                    Zip = "07716"
                },
                TownSection = new TownSection {Name = "MIDDLETOWN TWP"},
                Coordinate = new Coordinate
                    {Latitude = Convert.ToDecimal(40.2239789463), Longitude = Convert.ToDecimal(-74.1746424326)},
                HydrantSize = new HydrantSize {Size = Convert.ToDecimal(5)},
                Status = new AssetStatus {Description = "PENDING"},

                //HydrantCharacteristics
                CriticalNotes = "testing", //SPECIAL_MAINT_NOTES_DETAILS
                Gradient = new Gradient {Description = "COURT HOUSE SYSTEM"}, //PRESSURE_ZONE
                MapPage = "1", //MAP_PAGE
                OpenDirection = new HydrantDirection {Description = "LEFT"}, //OPEN_DIRECTION
                //DepthBuryFeet = 1,  //EAM_HYD_BURY_DEPTH
                //DepthBuryInches = 1,
                //BranchLengthFeet = 1,//HYD_BRANCH_LENGTH
                //BranchLengthInches = 1,
                LateralSize = new LateralSize {Size = Convert.ToDecimal(4)}, //HYD_AUX_VALVE_BRANCH_SIZE
                LateralValve = new Valve {ValveNumber = "VHW-12"},
                HydrantThreadType = new HydrantThreadType {Description = "STORZ 4IN"}, //HYD_STEAMER_THREAD_TP
                IsDeadEndMain = true, //HYD_DEAD_END_MAIN
                HydrantMainSize = new HydrantMainSize {Size = 4}, //HYD_BARREL_SIZE //EAM_PIPE_SIZE
                MainType = new MainType {SAPCode = "PVC"}, //DI// PIPE_MATERIAL
                WorkOrderNumber = "WS0001", //INSTALLATION_WO
                FireDistrict = new FireDistrict
                    {DistrictName = "", PremiseNumber = ""}, //HYD_ACCOUNT //HYD_FIRE_DISTRICT
                HydrantBilling = new HydrantBilling {Description = "Company"}, //HYD_BILLING_TP
            };
            return hydrant;
        }

        public Valve GetTestValve()
        {
            Valve valve = new Valve {
                ValveNumber = "VGT-4558", // Inventory Number //HISTORICAL_ID
                SAPEquipmentId = 0,
                FunctionalLocation = new FunctionalLocation {
                    Description = "NJLKW-LK-MAINS"
                }, //"NJBC-GT-VALVE" not working for this because DB2 does not have that value
                DateInstalled = Convert.ToDateTime("01/28/2007 12:00:00 AM"), //Start Up date
                Critical = false, //ABCIndicator = "M",
                StreetNumber = "",
                ValveMake = new ValveManufacturer {Description = "IOWA"},
                Street = new Street {FullStName = "BEVERLY DR"},
                CrossStreet = new Street {FullStName = "PASADENA DR"},
                Town = new Town {
                    ShortName = "GLOUCESTER TWP", County = new County {Name = "US"},
                    State = new State {Abbreviation = "NJ"}, Zip = "07080"
                },
                TownSection = new TownSection {Name = "MIDDLETOWN TWP"},
                Coordinate = new Coordinate
                    {Latitude = Convert.ToDecimal(39.8456756831), Longitude = Convert.ToDecimal(-75.0511481377)},
                ValveSize = new ValveSize {Size = Convert.ToDecimal(2)},
                Status = new AssetStatus {Description = "PENDING"},

                //Mappcall Req Class-SVLV
                CriticalNotes = "", //SPECIAL_MAINT_NOTES_DETAILS
                ValveControls = new ValveControl {Description = "SAMPLE POINT"}, //PRESSURE_ZONE --"HYDRANT"
                MapPage = "", //MAP_PAGE
                OpenDirection = new ValveOpenDirection {Description = "LEFT"},
                Turns = Convert.ToDecimal(0.25),
                NormalPosition = new ValveNormalPosition {Description = "OPEN"},
                MainType = new MainType {SAPCode = "CU"},
                ValveType = new ValveType {SAPCode = "BALL"},
                WorkOrderNumber = "A7662-A76", //INSTALLATION_WO
            };
            return valve;
        }

        public Valve GetTestBlowOff()
        {
            Valve valve = new Valve {
                ValveNumber = "VBH-1B", // Inventory Number //HISTORICAL_ID
                SAPEquipmentId = 0,
                FunctionalLocation = new FunctionalLocation {Description = "NJMM-MT-VALVE"}, //Functional Location ID
                DateInstalled = Convert.ToDateTime("4/21/1998"), //Start Up date
                Critical = true, //ABCIndicator = "M",
                StreetNumber = "111",
                Street = new Street {FullStName = "MAIN AVE"},
                CrossStreet = new Street {FullStName = "TWILIGHT RD"},
                Town = new Town {
                    ShortName = "FOSTER LN", County = new County {Name = "BAY HEAD"},
                    State = new State {Abbreviation = "NJ"}, Zip = "07080"
                },
                TownSection = new TownSection {Name = "MIDDLETOWN TWP"},
                Coordinate = new Coordinate
                    {Latitude = Convert.ToDecimal(40.075332048099), Longitude = Convert.ToDecimal(-74.04249226614)},
                ValveSize = new ValveSize {Size = 2},
                Status = new AssetStatus {Description = "RETIRED"},

                //Mappcall Req Class-SVLV
                CriticalNotes = "", //SPECIAL_MAINT_NOTES_DETAILS
                ValveControls = new ValveControl {Description = "BLOW OFF WITH FLUSHING"}, //PRESSURE_ZONE --"HYDRANT"
                MapPage = "", //MAP_PAGE
                OpenDirection = new ValveOpenDirection {Description = "LEFT"},
                Turns = Convert.ToDecimal(0.25),
                NormalPosition = new ValveNormalPosition {Description = "CLOSED"},
                MainType = new MainType {SAPCode = "DI"},
                ValveType = new ValveType {SAPCode = "BALL"},
                WorkOrderNumber = "40313000", //INSTALLATION_WO
                SketchNumber = "123",
            };
            return valve;
        }

        public SewerOpening GetOpening()
        {
            SewerOpening opening = new SewerOpening {
                OpeningNumber = "MMH-11", // Inventory Number //HISTORICAL_ID
                SAPEquipmentId = 0,
                FunctionalLocation = new FunctionalLocation
                    {Description = "NJLKW-LK-MAINS"}, // "NJLKW-LK-MAINS", //Functional Location ID
                DateInstalled = DateTime.Now, //Start Up date
                StreetNumber = "1",
                Street = new Street {FullStName = "FOSTER LN"},

                IntersectingStreet = new Street {FullStName = "bayshore dr"},
                Town = new Town {
                    ShortName = "FOSTER LN", County = new County {Name = "MONMOUTH"},
                    State = new State {Abbreviation = "NJ"}, Zip = "07080"
                },
                TownSection = new TownSection {Name = "MIDDLETOWN TWP"},
                Coordinate = new Coordinate
                    {Latitude = Convert.ToDecimal(40.0919637687307), Longitude = Convert.ToDecimal(-74.6718920721181)},
                Status = new AssetStatus {Description = "ACTIVE"},

                //Class - MH

                SewerOpeningMaterial = new SewerOpeningMaterial {SAPCode = "BRICK"},
                MapPage = "",
                TaskNumber = "",
                DepthToInvert = null,
            };
            return opening;
        }

        public Equipment GetTestEquipmentENG()
        {
            EquipmentModel[] equipmentModels = new EquipmentModel[1] {
                new EquipmentModel {
                    Description = "abc"
                }
            };

            var equipment = new Equipment {
                Number = 221,
                FunctionalLocation = "NJAC-Z1-ABSC.-CAUS",
                EquipmentManufacturer =
                    new EquipmentManufacturer {Description = "MUELLER"}, //, EquipmentModels = equipmentModels
                DateInstalled = DateTime.Now,
                ABCIndicator = new ABCIndicator {Description = "High"},
                //Coordinate = new Coordinate { Latitude = 0, Longitude = 0},

                EquipmentStatus = new EquipmentStatus {Description = "in service"},
                EquipmentType = new EquipmentType {Id = 221}
            };
            return equipment;
        }

        public Equipment GetTestEquipmentGeneric()
        {
            ProductionPrerequisite[] permit = new ProductionPrerequisite[3] {
                new ProductionPrerequisite {Description = "Air Permit"},
                new ProductionPrerequisite {Description = "Job Safety Checklist"},
                new ProductionPrerequisite {Description = "Has Lockout Requirement"}
            };

            EquipmentModel[] equipmentModels = new EquipmentModel[1] {
                new EquipmentModel {
                    Description = "abc"
                }
            };

            var equipment = new Equipment();
            equipment.ProductionPrerequisites = permit;
            equipment.Number = 10751;
            equipment.FunctionalLocation = "NJAC-Z1-ABSC.-CAUS";
            equipment.EquipmentManufacturer =
                new EquipmentManufacturer {Description = "MUELLER"}; //, EquipmentModels = equipmentModels
            equipment.DateInstalled = DateTime.Now;
            equipment.ABCIndicator = new ABCIndicator {Description = "High"};
            //Coordinate = new Coordinate { Latitude = 0, Longitude = 0},
            equipment.CriticalNotes = "ABCD";
            equipment.EquipmentStatus = new EquipmentStatus {Description = "field installed"};
            equipment.EquipmentType = new EquipmentType {Id = 188};

            EquipmentCharacteristic[] characteristics = new EquipmentCharacteristic[3];
            var fieldType = new EquipmentCharacteristicFieldType { DataType = "data type" };
            characteristics[0] = new EquipmentCharacteristic();
            characteristics[0].Field = new EquipmentCharacteristicField {
                FieldName = "PDMTOOL_TYP",
                FieldType = fieldType,
                IsActive = true
            };
            characteristics[0].Value = "test pdm tool type";

            characteristics[1] = new EquipmentCharacteristic();
            characteristics[1].Field = new EquipmentCharacteristicField {
                FieldName = "OWNED_BY",
                FieldType = fieldType,
                IsActive = true
            };
            characteristics[1].Value = "test owned by";

            characteristics[2] = new EquipmentCharacteristic();
            characteristics[2].Field = new EquipmentCharacteristicField {
                FieldName = "SPECIAL_MAINT_NOTES_DETAILS",
                FieldType = fieldType,
                IsActive = true
            };
            characteristics[2].Value = "SPECIAL_MAINT_NOTES_DETAILS";

            equipment.Characteristics = characteristics;

            return equipment;
        }

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container(e => { _dateTimeProvider = e.For<IDateTimeProvider>().Mock(); });

            hydrant = GetTestHydrant();
            valve = GetTestValve();
            _opening = GetOpening();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // _dateTimeProvider.VerifyAll();
        }

        [TestMethod]
        public void TestConstructorSetsPropertiesForHydrant()
        {
            string DepthBuryFeet, DepthBuryInches, BranchLengthFeet, BranchLengthInches;

            var HydrantBilling = "";
            var target = new SAPEquipment(hydrant);
            var SAPEquipmentNumber = (hydrant.SAPEquipmentId != null && hydrant.SAPEquipmentId != 0)
                ? hydrant.SAPEquipmentId.ToString().PadLeft(18, '0')
                : string.Empty;

            DepthBuryFeet = hydrant.DepthBuryFeet != null ? hydrant.DepthBuryFeet.ToString() + "FT " : "";
            DepthBuryInches = hydrant.DepthBuryInches != null ? hydrant.DepthBuryInches.ToString() + "IN" : "";

            BranchLengthFeet = hydrant.BranchLengthFeet != null ? hydrant.BranchLengthFeet.ToString() + "FT " : "";
            BranchLengthInches = hydrant.BranchLengthInches != null ? hydrant.BranchLengthInches.ToString() + "IN" : "";

            Assert.AreEqual(hydrant.HydrantNumber, target.InventoryNumber);
            Assert.AreEqual(SAPEquipmentNumber, target.SAPEquipmentNumber);
            Assert.AreEqual(hydrant.FunctionalLocation?.Description?.ToUpper(), target.FunctionalLocID);
            Assert.AreEqual(hydrant.DateInstalled, target.StartUpDate);
            Assert.AreEqual(hydrant.HydrantManufacturer?.Description?.ToUpper(), target.Manufacturer);
            Assert.AreEqual(hydrant.HydrantModel?.Description?.ToUpper(), target.Model);
            Assert.AreEqual(hydrant.YearManufactured, target.YearManufactured);
            Assert.AreEqual(hydrant.Critical, target.Critical);
            Assert.AreEqual(hydrant.StreetNumber.ToUpper(), target.House);
            Assert.AreEqual(hydrant.Street?.FullStName?.ToUpper(), target.Street1);
            Assert.AreEqual(hydrant.CrossStreet?.FullStName?.ToUpper(), target.Street2);
            Assert.AreEqual(hydrant.Town?.County.Name?.ToUpper(), target.Street5);
            Assert.AreEqual(hydrant.Town?.ShortName?.ToUpper(), target.City);
            Assert.AreEqual(hydrant.Town?.State.Abbreviation?.ToUpper(), target.State);
            Assert.AreEqual(hydrant.TownSection?.Description?.ToUpper(), target.OtherCity);
            Assert.AreEqual("US", target.Country);
            Assert.AreEqual(hydrant.Town?.Zip, target.CityPostalCode);
            Assert.AreEqual(hydrant.Coordinate?.Latitude.ToString(), target.Latitude);
            Assert.AreEqual(hydrant.Coordinate?.Longitude.ToString(), target.Longitude);
            Assert.AreEqual(hydrant.HydrantSize?.Size.ToString(), target.SizeDimension);
            Assert.AreEqual(hydrant.Status?.Description.ToUpper(), target.EquipmentStatus);
            //HydrantCharacteristics		
            switch (hydrant.HydrantBilling?.Description.ToString())
            {
                case "Private":
                    HydrantBilling = "PRIVATE FIRE";
                    break;
                case "Municipal":
                case "Public":
                    HydrantBilling = "PUBLIC FIRE";
                    break;
                case "Company":
                    HydrantBilling = "UNBILLED AW HYDRANT";
                    break;
                default:
                    HydrantBilling = string.Empty;
                    break;
            }

            Assert.AreEqual(hydrant.CriticalNotes.ToUpper(), target.HydrantCharacteristics.SPECIAL_MAINT_NOTES_DETAILS);
            Assert.AreEqual(hydrant.Gradient.Description, target.HydrantCharacteristics.PRESSURE_ZONE);
            Assert.AreEqual(hydrant.MapPage, target.HydrantCharacteristics.MAP_PAGE);
            Assert.AreEqual(hydrant.OpenDirection.Description, target.HydrantCharacteristics.OPEN_DIRECTION);
            Assert.AreEqual(hydrant.HydrantSize.Size.ToString(), target.HydrantCharacteristics.HYD_BARREL_SIZE);
            Assert.AreEqual(DepthBuryFeet + DepthBuryInches, target.HydrantCharacteristics.EAM_HYD_BURY_DEPTH);
            Assert.AreEqual(BranchLengthFeet + BranchLengthInches, target.HydrantCharacteristics.HYD_BRANCH_LENGTH);
            Assert.AreEqual(hydrant.LateralSize.Size.ToString(),
                target.HydrantCharacteristics.HYD_AUX_VALVE_BRANCH_SIZE);
            Assert.AreEqual(Convert.ToString(hydrant.IsDeadEndMain == true ? "Y" : "N"),
                Convert.ToString(target.HydrantCharacteristics.HYD_DEAD_END_MAIN));
            Assert.AreEqual(hydrant.LateralValve.ValveNumber, target.HydrantCharacteristics.HYD_AUX_VALVENUM);
            Assert.AreEqual(hydrant.HydrantThreadType.Description, target.HydrantCharacteristics.HYD_STEAMER_THREAD_TP);
            Assert.AreEqual(hydrant.HydrantMainSize.Size.ToString(), target.HydrantCharacteristics.EAM_PIPE_SIZE);
            Assert.AreEqual(hydrant.MainType.SAPCode, target.HydrantCharacteristics.PIPE_MATERIAL);
            Assert.AreEqual(hydrant.WorkOrderNumber, target.HydrantCharacteristics.INSTALLATION_WO);
            Assert.AreEqual(hydrant.FireDistrict.DistrictName, target.HydrantCharacteristics.HYD_FIRE_DISTRICT);
            Assert.AreEqual(hydrant.FireDistrict.PremiseNumber, target.HydrantCharacteristics.HYD_ACCOUNT);
            Assert.AreEqual(hydrant.HydrantBilling.Description.ToUpper(), target.HydrantCharacteristics.HYD_BILLING_TP);
            Assert.AreEqual(hydrant.Id.ToString(), target.HydrantCharacteristics.HISTORICAL_ID.ToString());
        }

        [TestMethod]
        public void TestConstructorSetsPropertiesForValves()
        {
            var target = new SAPEquipment(valve);
            var SAPEquipmentNumber = (valve.SAPEquipmentId != null && valve.SAPEquipmentId != 0)
                ? valve.SAPEquipmentId.ToString().PadLeft(18, '0')
                : string.Empty;

            Assert.AreEqual(valve.ValveNumber, target.InventoryNumber);
            Assert.AreEqual(SAPEquipmentNumber, target.SAPEquipmentNumber);
            Assert.AreEqual(valve.FunctionalLocation?.Description.ToUpper(), target.FunctionalLocID);
            Assert.AreEqual(valve.ValveMake?.Description.ToUpper(), target.Manufacturer);
            Assert.AreEqual(valve.DateInstalled, target.StartUpDate);
            Assert.AreEqual(valve.Critical, target.Critical);
            Assert.AreEqual(valve.StreetNumber.ToUpper(), target.House);
            Assert.AreEqual(valve.Street?.FullStName.ToUpper(), target.Street1);
            Assert.AreEqual(valve.CrossStreet?.FullStName.ToUpper(), target.Street2);
            Assert.AreEqual(valve.Town?.County.Name.ToUpper(), target.Street5);
            Assert.AreEqual(valve.Town?.ShortName.ToUpper(), target.City);
            Assert.AreEqual(valve.Town?.State.Abbreviation.ToUpper(), target.State);
            Assert.AreEqual(valve.TownSection?.Description.ToUpper(), target.OtherCity);
            Assert.AreEqual("US", target.Country);
            Assert.AreEqual(valve.Town.Zip, target.CityPostalCode);
            Assert.AreEqual(Convert.ToString(valve.Coordinate.Latitude), target.Latitude);
            Assert.AreEqual(Convert.ToString(valve.Coordinate.Longitude), target.Longitude);
            Assert.AreEqual(valve.ValveSize.Description, target.SizeDimension);
            Assert.AreEqual(valve.Status.Description, target.EquipmentStatus);

            if (valve.ValveControls.Description == "BLOW OFF" ||
                valve.ValveControls.Description == "BLOW OFF WITH FLUSHING")
            {
                //BlowOffCharacteristics		
                Assert.AreEqual(valve.CriticalNotes,
                    target.BlowOffCharacteristics.SPECIAL_MAINT_NOTES_DETAILS); //SPECIAL_MAINT_NOTES_DETAILS
                Assert.AreEqual("BLOWOFF(M)", target.BlowOffCharacteristics.SVLV_BO_TYP); //SVLV_BO_TYP
                Assert.AreEqual("BLOW OFF", target.BlowOffCharacteristics.APPLICATION_SVLV_BO); //APPLICATION_SVLV_BO
                Assert.AreEqual(valve.MapPage, target.BlowOffCharacteristics.MAP_PAGE); //MAP_PAGE
                Assert.AreEqual(valve.OpenDirection.Description,
                    target.BlowOffCharacteristics.OPEN_DIRECTION); //OPEN_DIRECTION
                Assert.AreEqual(valve.Turns, target.BlowOffCharacteristics.NUMBER_OF_TURNS); //NUMBER_OF_TURNS
                Assert.AreEqual(valve.NormalPosition.Description.ToString(),
                    target.BlowOffCharacteristics.NORMAL_POSITION.ToString()); //NORMAL_POSITION
                Assert.AreEqual(valve.ValveSize.Size.ToString(),
                    target.BlowOffCharacteristics.VLV_VALVE_SIZE.ToString()); //VLV_VALVE_SIZE
                Assert.AreEqual("Y", target.BlowOffCharacteristics.BYPASS_VALVE); //BYPASS_VALVE
                Assert.AreEqual(valve.MainType.SAPCode, target.BlowOffCharacteristics.PIPE_MATERIAL); //PIPE_MATERIAL
                Assert.AreEqual(valve.ValveType.SAPCode, target.BlowOffCharacteristics.VLV_VALVE_TP); //VLV_VALVE_TP
                Assert.AreEqual(valve.WorkOrderNumber, target.BlowOffCharacteristics.INSTALLATION_WO);
                Assert.AreEqual(valve.Id.ToString(), target.BlowOffCharacteristics.HISTORICAL_ID.ToString());
            }
            else
            {
                //valveCharacteristics		
                Assert.AreEqual(valve.CriticalNotes,
                    target.ValveCharacteristics.SPECIAL_MAINT_NOTES_DETAILS); //SPECIAL_MAINT_NOTES_DETAILS
                Assert.AreEqual("DIST (M)", target.ValveCharacteristics.SVLV_TYP); //SVLV_TYP
                Assert.AreEqual("OTHER", target.ValveCharacteristics.APPLICATION_SVLV); //APPLICATION_SVLV
                Assert.AreEqual(valve.MapPage, target.ValveCharacteristics.MAP_PAGE); //MAP_PAGE
                Assert.AreEqual(valve.OpenDirection.Description,
                    target.ValveCharacteristics.OPEN_DIRECTION); //OPEN_DIRECTION
                Assert.AreEqual(valve.Turns, target.ValveCharacteristics.NUMBER_OF_TURNS); //NUMBER_OF_TURNS
                Assert.AreEqual(valve.NormalPosition.Description.ToString(),
                    target.ValveCharacteristics.NORMAL_POSITION); //NORMAL_POSITION
                Assert.AreEqual(valve.ValveSize.Size.ToString(),
                    target.ValveCharacteristics.VLV_VALVE_SIZE.ToString()); //VLV_VALVE_SIZE
                Assert.AreEqual("N", target.ValveCharacteristics.BYPASS_VALVE); //BYPASS_VALVE
                Assert.AreEqual(valve.MainType.SAPCode, target.ValveCharacteristics.PIPE_MATERIAL); //PIPE_MATERIAL
                Assert.AreEqual(valve.ValveType.SAPCode, target.ValveCharacteristics.VLV_VALVE_TP); //VLV_VALVE_TP
                Assert.AreEqual(valve.WorkOrderNumber, target.ValveCharacteristics.INSTALLATION_WO);
                Assert.AreEqual(valve.Id.ToString(), target.ValveCharacteristics.HISTORICAL_ID.ToString());
            }
        }

        [TestMethod]
        public void TestConstructorSetsPropertiesForOpening()
        {
            var target = new SAPEquipment(_opening);
            var SAPEquipmentNumber = (_opening.SAPEquipmentId != null && _opening.SAPEquipmentId != 0)
                ? _opening.SAPEquipmentId.ToString().PadLeft(18, '0')
                : string.Empty;

            Assert.AreEqual(_opening.OpeningNumber, target.InventoryNumber);
            Assert.AreEqual(SAPEquipmentNumber, target.SAPEquipmentNumber);
            Assert.AreEqual(_opening.FunctionalLocation?.Description, target.FunctionalLocID);
            Assert.AreEqual(_opening.DateInstalled, target.StartUpDate);
            Assert.AreEqual(_opening.StreetNumber?.ToUpper(), target.House);
            Assert.AreEqual(_opening.Street?.FullStName.ToUpper(), target.Street1);
            Assert.AreEqual(_opening.IntersectingStreet?.FullStName?.ToUpper(), target.Street2);
            Assert.AreEqual(_opening.Town?.County.Name.ToUpper(), target.Street5);
            Assert.AreEqual(_opening.Town?.State.Abbreviation.ToUpper(), target.State);
            Assert.AreEqual(_opening.TownSection?.Description.ToUpper(), target.OtherCity);
            Assert.AreEqual("US", target.Country);
            Assert.AreEqual(_opening.Town.Zip, target.CityPostalCode);
            Assert.AreEqual(Convert.ToString(_opening.Coordinate.Latitude), target.Latitude);
            Assert.AreEqual(Convert.ToString(_opening.Coordinate.Longitude), target.Longitude);
            Assert.AreEqual(_opening.Status.Description, target.EquipmentStatus);
            //OpeningCharacteristics	
            Assert.AreEqual(_opening.SewerOpeningMaterial.SAPCode.ToString(),
                target.OpeningCharacteristics.MATERIAL_OF_CONSTRUCTION_MH); //MAP_PAGE
            Assert.AreEqual(_opening.MapPage, target.OpeningCharacteristics.MAP_PAGE); //MAP_PAGE
            Assert.AreEqual(_opening.TaskNumber, target.OpeningCharacteristics.INSTALLATION_WO); //INSTALLATION_WO
            Assert.AreEqual(_opening.DepthToInvert,
                target.OpeningCharacteristics.MH_DEPTH != 0
                    ? target.OpeningCharacteristics.MH_DEPTH
                    : null); //INSTALLATION_WO
        }

        [TestMethod]
        /// <summary>
        /// when valve control is hydrant
        /// </summary>
        public void TestConstructorSetsPropertiesForValvesControlHydrant()
        {
            valve.ValveControls = new ValveControl {Description = "HYDRANT"};
            var target = new SAPEquipment(valve);

            Assert.AreEqual("HYD AUX (M)", target.ValveCharacteristics.SVLV_TYP); //SVLV_BO_TYP
            Assert.AreEqual("HYD AUX", target.ValveCharacteristics.APPLICATION_SVLV); //APPLICATION_SVLV_BO
            Assert.AreEqual("N", target.ValveCharacteristics.BYPASS_VALVE); //BYPASS_VALVE
        }

        [TestMethod]
        /// <summary>
        /// when valve control is nither hydrant nor blow off
        /// </summary>
        public void TestConstructorSetsPropertiesForValvesControlOther()
        {
            valve.ValveControls = new ValveControl {Description = "CONTROL VALVE"};
            var target = new SAPEquipment(valve);

            Assert.AreEqual("DIST (M)", target.ValveCharacteristics.SVLV_TYP); //SVLV_BO_TYP
            Assert.AreEqual("DISTRIBUTION IN GRID", target.ValveCharacteristics.APPLICATION_SVLV); //APPLICATION_SVLV_BO
            Assert.AreEqual("N", target.ValveCharacteristics.BYPASS_VALVE); //BYPASS_VALVE
        }

        [TestMethod]
        public void TestConstructorSetsPropertiesForValveControlsCrossing()
        {
            valve.ValveControls = new ValveControl {Description = "HYDRANT"};
            valve.ControlsCrossing = true;
            var target = new SAPEquipment(valve);

            Assert.AreEqual("HYD AUX", target.ValveCharacteristics.APPLICATION_SVLV);

            valve.ControlsCrossing = false;
            target = new SAPEquipment(valve);
            Assert.AreEqual("HYD AUX", target.ValveCharacteristics.APPLICATION_SVLV);
        }

        [TestMethod]
        /// <summary>
        /// when valve control is by pass
        /// </summary>
        public void TestConstructorSetsPropertiesForValvesControlByPass()
        {
            valve.ValveControls = new ValveControl {Description = "BY-PASS"};
            var target = new SAPEquipment(valve);
            Assert.AreEqual("DIST (M)", target.ValveCharacteristics.SVLV_TYP); //SVLV_BO_TYP
            Assert.AreEqual("BYPASS", target.ValveCharacteristics.APPLICATION_SVLV); //APPLICATION_SVLV_BO
            Assert.AreEqual("Y", target.ValveCharacteristics.BYPASS_VALVE); //BYPASS_VALVE
        }

        [TestMethod]
        /// <summary>
        /// when valve control is  blow off
        /// </summary>
        public void TestConstructorSetsPropertiesForValvesControlBlowOff()
        {
            valve.ValveControls = new ValveControl {Description = "BLOW OFF WITH FLUSHING"};
            var target = new SAPEquipment(valve);
            Assert.AreEqual("BLOWOFF(M)", target.BlowOffCharacteristics.SVLV_BO_TYP); //SVLV_BO_TYP
            Assert.AreEqual("BLOW-OFF", target.BlowOffCharacteristics.APPLICATION_SVLV_BO); //APPLICATION_SVLV_BO
            Assert.AreEqual("N", target.BlowOffCharacteristics.BYPASS_VALVE); //BYPASS_VALVE
        }

        [TestMethod]
        public void TestConstructorSetsThingsCorrectlyForCleanOut()
        {
            var cleanout = new Equipment {
                EquipmentType = new EquipmentType {
                    Id = EquipmentType.Indices.CO,
                    ProductionAssetType = new ProductionAssetType {
                        Description = ProductionAssetType.Descriptions.SEWER,
                        Id = 5
                    },
                    EquipmentCategory = "4",
                    ReferenceEquipmentNumber = "REFERENCE-CO"
                },
            };

            var target = new SAPEquipment(cleanout);
            
            Assert.AreEqual(cleanout.EquipmentType.ProductionAssetType.Description, target.AssetType);
            Assert.AreEqual(cleanout.EquipmentType.EquipmentCategory, target.EquipmentCategory);
            Assert.AreEqual(cleanout.EquipmentType.Id, target.EquipmentTypeId);
            Assert.AreEqual(cleanout.EquipmentType.ReferenceEquipmentNumber, target.ReferenceEquipmentNumber);
        }

        /// <summary>
        /// equipment status test for ActvieInstalledOrEtc
        /// </summary>
        [TestMethod]
        public void TestEquipmentSystemStatusReturnsINSTWhenActvieInstalledOrEtc()
        {
            var target = new SAPEquipment();
            target.EquipmentStatus = "ACTIVE";
            Assert.AreEqual("INST", target.EquipmentSystemStatus.ToString());
            Assert.AreEqual("INSV", target.EquipmentUserStatus.ToString());

            target.EquipmentStatus = "INSTALLED";
            Assert.AreEqual("INST", target.EquipmentSystemStatus.ToString());
            Assert.AreEqual("OOS TBIN", target.EquipmentUserStatus.ToString());

            target.EquipmentStatus = "PENDING";
            Assert.AreEqual("INST", target.EquipmentSystemStatus.ToString());
            Assert.AreEqual("OOS TBIN", target.EquipmentUserStatus.ToString());

            target.EquipmentStatus = "REQUEST CANCELLATION";
            Assert.AreEqual("INST", target.EquipmentSystemStatus.ToString());

            target.EquipmentStatus = "REQUEST RETIREMENT";
            Assert.AreEqual("INST", target.EquipmentSystemStatus.ToString());
            Assert.AreEqual("INSV", target.EquipmentUserStatus.ToString());

            target.EquipmentStatus = "CANCELLED";
            Assert.AreEqual("DLFL INAC INST", target.EquipmentSystemStatus.ToString());

            target.EquipmentStatus = "REMOVED";
            Assert.AreEqual("DLFL INAC INST", target.EquipmentSystemStatus.ToString());

            target.EquipmentStatus = "RETIRED";
            Assert.AreEqual("DLFL INAC INST", target.EquipmentSystemStatus.ToString());
            Assert.AreEqual("REIP", target.EquipmentUserStatus.ToString());

            target.EquipmentStatus = "INACTIVE";
            Assert.AreEqual("INAC INST", target.EquipmentSystemStatus.ToString());
            Assert.AreEqual("OOS", target.EquipmentUserStatus.ToString());
        }

        [TestMethod]
        public void TestABCIndicatorReturnsHWhenCriticalTrue()
        {
            var target = new SAPEquipment();
            target.Critical = true;
            Assert.AreEqual("H", target.ABCIndicator.ToString());

            target.Critical = false;
            Assert.AreEqual("M", target.ABCIndicator.ToString());
        }

        [TestMethod]
        public void TestAddEditIndicatorReturnsAddEditAsset()
        {
            var target = new SAPEquipment();
            target.SAPEquipmentNumber = "";
            Assert.AreEqual("A", target.AddEditIndicator);

            target.SAPEquipmentNumber = "0";
            Assert.AreEqual("A", target.AddEditIndicator);

            target.SAPEquipmentNumber = "123";
            Assert.AreEqual("E", target.AddEditIndicator);
        }

        [TestMethod]
        public void TestDescriptionReturnsNumberAndAsset()
        {
            var target = new SAPEquipment();
            target.InventoryNumber = "HY-122";
            target.AssetType = "Hydrant";
            Assert.AreEqual("HY-122/Hydrant", target.Description.ToString());
        }

        [TestMethod]
        public void TestDescriptionReturnsDescriptionForEquipmentInsteadOfNumberAndAsset()
        {
            var expected = "this is a description";
            var equipment = new Equipment {
                // Identifier = "NJLK-42-CSDC-1",
                Description = expected
            };
            var target = new SAPEquipment(equipment) {AssetType = "Equipment"};
            Assert.AreEqual(expected, target.Description.ToString());
        }

        [TestMethod()]
        public void TestToEquipmentsEquipmentsHydrant()
        {
            var SAPEquipment = new SAPEquipment(GetTestHydrant());

            var target = SAPEquipment.ToEquipmentsEquipments();

            Assert.AreEqual(target[0].Indicator, SAPEquipment.AddEditIndicator);
            Assert.AreEqual(target[0].EquipmentCategory, SAPEquipment.EquipmentCategory);
            Assert.AreEqual(target[0].InventoryNumber, SAPEquipment.InventoryNumber);
            //Assert.AreEqual(target[0].AssetType, SAPEquipment.AssetType);
            Assert.AreEqual(target[0].Description, SAPEquipment.Description);
            Assert.AreEqual(target[0].RefEquipmentNo, SAPEquipment.ReferenceEquipmentNumber);
            Assert.AreEqual(target[0].FunctionalLocID, SAPEquipment.FunctionalLocID);
            Assert.AreEqual(target[0].StartUpDate.ToString(), SAPEquipment.StartUpDate?.ToString("yyyyMMdd"));
            Assert.AreEqual(target[0].Manufacturer, SAPEquipment.Manufacturer);
            Assert.AreEqual(target[0].Model.ToString(), SAPEquipment.Model?.ToString());
            Assert.AreEqual(target[0].YearManufactured?.ToString(), SAPEquipment.YearManufactured?.ToString());
            Assert.AreEqual(target[0].MonthManufactured?.ToString(), SAPEquipment.MonthManufactured?.ToString());
            Assert.AreEqual(target[0].ABCIndicator, SAPEquipment.ABCIndicator);
            Assert.AreEqual(target[0].House, SAPEquipment.House);
            Assert.AreEqual(target[0].Street1, SAPEquipment.Street1.ToUpper());
            Assert.AreEqual(target[0].Street2, SAPEquipment.Street2);
            Assert.AreEqual(target[0].Street5, SAPEquipment.Street5);
            Assert.AreEqual(target[0].City, SAPEquipment.City.ToUpper());
            Assert.AreEqual(target[0].OtherCity, SAPEquipment.OtherCity.ToUpper());
            Assert.AreEqual(target[0].State, SAPEquipment.State.ToUpper());
            Assert.AreEqual(target[0].Country, SAPEquipment.Country);
            Assert.AreEqual(target[0].ZipCode, SAPEquipment.CityPostalCode);
            Assert.AreEqual(target[0].Latitude, SAPEquipment.Latitude);
            Assert.AreEqual(target[0].Longitude, SAPEquipment.Longitude);
            Assert.AreEqual(target[0].EquipmentUserStatus, SAPEquipment.EquipmentUserStatus);
            Assert.AreEqual(target[0].EquipmentSystemStatus, SAPEquipment.EquipmentSystemStatus);
            Assert.AreEqual(target[0].Size, SAPEquipment.SizeDimension);
            //CHARACTERISTICS
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "SPECIAL_MAINT_NOTES_DETAILS").AttribVal,
                SAPEquipment.HydrantCharacteristics.SPECIAL_MAINT_NOTES_DETAILS);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "PRESSURE_ZONE").AttribVal,
                SAPEquipment.HydrantCharacteristics.PRESSURE_ZONE);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "MAP_PAGE").AttribVal,
                SAPEquipment.HydrantCharacteristics.MAP_PAGE);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "OPEN_DIRECTION").AttribVal,
                SAPEquipment.HydrantCharacteristics.OPEN_DIRECTION);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "HYD_BARREL_SIZE").AttribVal,
                SAPEquipment.HydrantCharacteristics.HYD_BARREL_SIZE);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "EAM_HYD_BURY_DEPTH").AttribVal,
                SAPEquipment.HydrantCharacteristics.EAM_HYD_BURY_DEPTH);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "HYD_BRANCH_LENGTH").AttribVal,
                SAPEquipment.HydrantCharacteristics.HYD_BRANCH_LENGTH);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "HYD_AUX_VALVE_BRANCH_SIZE").AttribVal,
                SAPEquipment.HydrantCharacteristics.HYD_AUX_VALVE_BRANCH_SIZE);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "HYD_DEAD_END_MAIN").AttribVal,
                SAPEquipment.HydrantCharacteristics.HYD_DEAD_END_MAIN = true ? "Y" : "N");
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "HYD_AUX_VALVENUM").AttribVal,
                SAPEquipment.HydrantCharacteristics.HYD_AUX_VALVENUM);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "EAM_PIPE_SIZE").AttribVal,
                SAPEquipment.HydrantCharacteristics.EAM_PIPE_SIZE);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "PIPE_MATERIAL").AttribVal,
                SAPEquipment.HydrantCharacteristics.PIPE_MATERIAL);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "INSTALLATION_WO").AttribVal,
                SAPEquipment.HydrantCharacteristics.INSTALLATION_WO);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "HYD_FIRE_DISTRICT").AttribVal,
                SAPEquipment.HydrantCharacteristics.HYD_FIRE_DISTRICT);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "HYD_ACCOUNT").AttribVal,
                SAPEquipment.HydrantCharacteristics.HYD_ACCOUNT);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "HYD_BILLING_TP").AttribVal,
                SAPEquipment.HydrantCharacteristics.HydrantBillingType);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "HISTORICAL_ID").AttribVal,
                SAPEquipment.HydrantCharacteristics.HISTORICAL_ID);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "HYD_STEAMER_THREAD_TP").AttribVal,
                SAPEquipment.HydrantCharacteristics.SteamerThreadType);
        }

        [TestMethod()]
        public void TestToEquipmentsEquipmentsValve()
        {
            var SAPEquipment = new SAPEquipment(GetTestValve());

            var target = SAPEquipment.ToEquipmentsEquipments();

            Assert.AreEqual(target[0].Indicator, SAPEquipment.AddEditIndicator);
            Assert.AreEqual(target[0].EquipmentCategory, SAPEquipment.EquipmentCategory);
            Assert.AreEqual(target[0].InventoryNumber, SAPEquipment.InventoryNumber);
            //Assert.AreEqual(target[0].AssetType, SAPEquipment.AssetType);
            Assert.AreEqual(target[0].Description, SAPEquipment.Description);
            Assert.AreEqual(target[0].RefEquipmentNo, SAPEquipment.ReferenceEquipmentNumber);
            Assert.AreEqual(target[0].FunctionalLocID, SAPEquipment.FunctionalLocID);
            Assert.AreEqual(target[0].StartUpDate.ToString(), SAPEquipment.StartUpDate?.ToString("yyyyMMdd"));
            Assert.AreEqual(target[0].Manufacturer, SAPEquipment.Manufacturer);
            Assert.AreEqual(target[0].Model?.ToString(), SAPEquipment.Model?.ToString());
            Assert.AreEqual(target[0].ABCIndicator, SAPEquipment.ABCIndicator);
            Assert.AreEqual(target[0].House, SAPEquipment.House.ToUpper());
            Assert.AreEqual(target[0].Street1, SAPEquipment.Street1.ToUpper());
            Assert.AreEqual(target[0].Street2, SAPEquipment.Street2.ToUpper());
            Assert.AreEqual(target[0].Street5, SAPEquipment.Street5.ToUpper());
            Assert.AreEqual(target[0].City, SAPEquipment.City.ToUpper());
            Assert.AreEqual(target[0].OtherCity, SAPEquipment.OtherCity.ToUpper());
            Assert.AreEqual(target[0].State, SAPEquipment.State.ToUpper());
            Assert.AreEqual(target[0].Country, SAPEquipment.Country.ToUpper());
            Assert.AreEqual(target[0].ZipCode, SAPEquipment.CityPostalCode);
            Assert.AreEqual(target[0].Latitude, SAPEquipment.Latitude);
            Assert.AreEqual(target[0].Longitude, SAPEquipment.Longitude);
            Assert.AreEqual(target[0].EquipmentUserStatus, SAPEquipment.EquipmentUserStatus);
            Assert.AreEqual(target[0].EquipmentSystemStatus, SAPEquipment.EquipmentSystemStatus);
            Assert.AreEqual(target[0].Size, SAPEquipment.SizeDimension);
            //CHARACTERISTICS
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "SPECIAL_MAINT_NOTES_DETAILS").AttribVal,
                SAPEquipment.ValveCharacteristics.SPECIAL_MAINT_NOTES_DETAILS);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "SVLV_TYP").AttribVal,
                SAPEquipment.ValveCharacteristics.SVLV_TYP);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "APPLICATION_SVLV").AttribVal,
                SAPEquipment.ValveCharacteristics.APPLICATION_SVLV);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "MAP_PAGE").AttribVal,
                SAPEquipment.ValveCharacteristics.MAP_PAGE);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "OPEN_DIRECTION").AttribVal,
                SAPEquipment.ValveCharacteristics.OPEN_DIRECTION);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "NUMBER_OF_TURNS").AttribVal,
                SAPEquipment.ValveCharacteristics.NUMBER_OF_TURNS.ToString());
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "NORMAL_POSITION").AttribVal,
                SAPEquipment.ValveCharacteristics.NORMAL_POSITION);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "VLV_VALVE_SIZE").AttribVal,
                SAPEquipment.ValveCharacteristics.VLV_VALVE_SIZE.ToString());
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "BYPASS_VALVE").AttribVal,
                SAPEquipment.ValveCharacteristics.BYPASS_VALVE);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "EAM_PIPE_SIZE").AttribVal,
                SAPEquipment.ValveCharacteristics.EAM_PIPE_SIZE);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "PIPE_MATERIAL").AttribVal,
                SAPEquipment.ValveCharacteristics.PIPE_MATERIAL);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "VLV_VALVE_TP").AttribVal,
                SAPEquipment.ValveCharacteristics.VLV_VALVE_TP);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "INSTALLATION_WO").AttribVal,
                SAPEquipment.ValveCharacteristics.INSTALLATION_WO);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "HISTORICAL_ID").AttribVal,
                SAPEquipment.ValveCharacteristics.HISTORICAL_ID);
        }

        [TestMethod()]
        public void TestToEquipmentsEquipmentsOpening()
        {
            var SAPEquipment = new SAPEquipment(GetOpening());

            var target = SAPEquipment.ToEquipmentsEquipments();

            Assert.AreEqual(target[0].Indicator, SAPEquipment.AddEditIndicator);
            Assert.AreEqual(target[0].EquipmentCategory, SAPEquipment.EquipmentCategory);
            Assert.AreEqual(target[0].InventoryNumber, SAPEquipment.InventoryNumber);
            Assert.AreEqual(target[0].Description, SAPEquipment.Description);
            Assert.AreEqual(target[0].RefEquipmentNo, SAPEquipment.ReferenceEquipmentNumber);
            Assert.AreEqual(target[0].FunctionalLocID, SAPEquipment.FunctionalLocID);
            Assert.AreEqual(target[0].StartUpDate.ToString(), SAPEquipment.StartUpDate?.ToString("yyyyMMdd"));
            Assert.AreEqual(target[0].Manufacturer, SAPEquipment.Manufacturer);
            Assert.AreEqual(target[0].Model?.ToString(), SAPEquipment.Model?.ToString());

            Assert.AreEqual(target[0].ABCIndicator, SAPEquipment.ABCIndicator);
            Assert.AreEqual(target[0].House, SAPEquipment.House.ToUpper());
            Assert.AreEqual(target[0].Street1, SAPEquipment.Street1.ToUpper());
            Assert.AreEqual(target[0].Street2?.ToString(), SAPEquipment.Street2?.ToString());
            Assert.AreEqual(target[0].Street5, SAPEquipment.Street5.ToUpper());
            Assert.AreEqual(target[0].City, SAPEquipment.City.ToUpper());
            Assert.AreEqual(target[0].OtherCity, SAPEquipment.OtherCity.ToUpper());
            Assert.AreEqual(target[0].State, SAPEquipment.State.ToUpper());
            Assert.AreEqual(target[0].Country, SAPEquipment.Country);
            Assert.AreEqual(target[0].ZipCode, SAPEquipment.CityPostalCode);
            Assert.AreEqual(target[0].Latitude, SAPEquipment.Latitude);
            Assert.AreEqual(target[0].Longitude, SAPEquipment.Longitude);
            Assert.AreEqual(target[0].EquipmentUserStatus, SAPEquipment.EquipmentUserStatus);
            Assert.AreEqual(target[0].EquipmentSystemStatus, SAPEquipment.EquipmentSystemStatus);
            Assert.AreEqual(target[0].Size, SAPEquipment.SizeDimension);
            //CHARACTERISTICS
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "SPECIAL_MAINT_NOTES_DETAILS").AttribVal,
                SAPEquipment.OpeningCharacteristics.SPECIAL_MAINT_NOTES_DETAILS);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "MATERIAL_OF_CONSTRUCTION_MH").AttribVal,
                SAPEquipment.OpeningCharacteristics.MATERIAL_OF_CONSTRUCTION_MH);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "MAP_PAGE").AttribVal,
                SAPEquipment.OpeningCharacteristics.MAP_PAGE);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "INSTALLATION_WO").AttribVal,
                SAPEquipment.OpeningCharacteristics.INSTALLATION_WO);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "MH_DEPTH").AttribVal,
                SAPEquipment.OpeningCharacteristics.MH_DEPTH.ToString());
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "HISTORICAL_ID").AttribVal,
                SAPEquipment.OpeningCharacteristics.HISTORICAL_ID);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "MH_TYP").AttribVal,
                SAPEquipment.OpeningCharacteristics.MH_TYP);
        }

        [TestMethod()]
        public void TestToEquipmentsEquipmentsBlowOff()
        {
            var SAPEquipment = new SAPEquipment(GetTestBlowOff());

            var target = SAPEquipment.ToEquipmentsEquipments();

            Assert.AreEqual(target[0].Indicator, SAPEquipment.AddEditIndicator);
            Assert.AreEqual(target[0].EquipmentCategory, SAPEquipment.EquipmentCategory);
            Assert.AreEqual(target[0].InventoryNumber, SAPEquipment.InventoryNumber);
            //Assert.AreEqual(target[0].AssetType, SAPEquipment.AssetType);
            Assert.AreEqual(target[0].Description, SAPEquipment.Description);
            Assert.AreEqual(target[0].RefEquipmentNo, SAPEquipment.ReferenceEquipmentNumber);
            Assert.AreEqual(target[0].FunctionalLocID, SAPEquipment.FunctionalLocID);
            Assert.AreEqual(target[0].StartUpDate.ToString(), SAPEquipment.StartUpDate?.ToString("yyyyMMdd"));
            Assert.AreEqual(target[0].Manufacturer, SAPEquipment.Manufacturer);
            Assert.AreEqual(target[0].Model?.ToString(), SAPEquipment.Model?.ToString());
            Assert.AreEqual(target[0].ABCIndicator, SAPEquipment.ABCIndicator);
            Assert.AreEqual(target[0].House, SAPEquipment.House.ToUpper());
            Assert.AreEqual(target[0].Street1, SAPEquipment.Street1.ToUpper());
            Assert.AreEqual(target[0].Street2, SAPEquipment.Street2.ToUpper());
            Assert.AreEqual(target[0].Street5, SAPEquipment.Street5.ToUpper());
            Assert.AreEqual(target[0].City, SAPEquipment.City.ToUpper());
            Assert.AreEqual(target[0].OtherCity, SAPEquipment.OtherCity.ToUpper());
            Assert.AreEqual(target[0].State, SAPEquipment.State.ToUpper());
            Assert.AreEqual(target[0].Country, SAPEquipment.Country);
            Assert.AreEqual(target[0].ZipCode, SAPEquipment.CityPostalCode);
            Assert.AreEqual(target[0].Latitude, SAPEquipment.Latitude);
            Assert.AreEqual(target[0].Longitude, SAPEquipment.Longitude);
            Assert.AreEqual(target[0].EquipmentUserStatus, SAPEquipment.EquipmentUserStatus);
            Assert.AreEqual(target[0].EquipmentSystemStatus, SAPEquipment.EquipmentSystemStatus);
            Assert.AreEqual(target[0].Size, SAPEquipment.SizeDimension);
            //CHARACTERISTICS
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "SPECIAL_MAINT_NOTES_DETAILS").AttribVal,
                SAPEquipment.BlowOffCharacteristics.SPECIAL_MAINT_NOTES_DETAILS);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "SVLV_BO_TYP").AttribVal,
                SAPEquipment.BlowOffCharacteristics.SVLV_BO_TYP);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "APPLICATION_SVLV_BO").AttribVal,
                SAPEquipment.BlowOffCharacteristics.APPLICATION_SVLV_BO);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "MAP_PAGE").AttribVal,
                SAPEquipment.BlowOffCharacteristics.MAP_PAGE);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "OPEN_DIRECTION").AttribVal,
                SAPEquipment.BlowOffCharacteristics.OPEN_DIRECTION.ToUpper());
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "NUMBER_OF_TURNS").AttribVal,
                SAPEquipment.BlowOffCharacteristics.NUMBER_OF_TURNS.ToString());
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "NORMAL_POSITION").AttribVal,
                SAPEquipment.BlowOffCharacteristics.NORMAL_POSITION);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "VLV_VALVE_SIZE").AttribVal,
                SAPEquipment.BlowOffCharacteristics.VLV_VALVE_SIZE);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "BYPASS_VALVE").AttribVal,
                SAPEquipment.BlowOffCharacteristics.BYPASS_VALVE);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "EAM_PIPE_SIZE").AttribVal,
                SAPEquipment.BlowOffCharacteristics.EAM_PIPE_SIZE);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "PIPE_MATERIAL").AttribVal,
                SAPEquipment.BlowOffCharacteristics.PIPE_MATERIAL);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "VLV_VALVE_TP").AttribVal,
                SAPEquipment.BlowOffCharacteristics.VLV_VALVE_TP);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "INSTALLATION_WO").AttribVal,
                SAPEquipment.BlowOffCharacteristics.INSTALLATION_WO);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "HISTORICAL_ID").AttribVal,
                SAPEquipment.BlowOffCharacteristics.HISTORICAL_ID);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "SKETCH_NUM").AttribVal,
                SAPEquipment.BlowOffCharacteristics.SKETCH_NUM);
        }

        [TestMethod]
        public void TestHydrantCharacteristicsHydrantBillingType()
        {
            var target = new SAPEquipment(hydrant);
            target.HydrantCharacteristics.HYD_BILLING_TP = "PRIVATE";
            Assert.AreEqual("PRIVATE FIRE", target.HydrantCharacteristics.HydrantBillingType.ToString());

            target.HydrantCharacteristics.HYD_BILLING_TP = "MUNICIPAL";
            Assert.AreEqual("PUBLIC FIRE", target.HydrantCharacteristics.HydrantBillingType.ToString());

            target.HydrantCharacteristics.HYD_BILLING_TP = "PUBLIC";
            Assert.AreEqual("PUBLIC FIRE", target.HydrantCharacteristics.HydrantBillingType.ToString());

            target.HydrantCharacteristics.HYD_BILLING_TP = "COMPANY";
            Assert.AreEqual("UNBILLED AW HYDRANT", target.HydrantCharacteristics.HydrantBillingType.ToString());
        }

        [TestMethod]
        public void TestToEquipmentForProductionEquipmentHydrantsCorrectlyMapsData()
        {
            var equipment = GetTestEquipmentGeneric();
            equipment.EquipmentType = new EquipmentType {
                Id = EquipmentType.Indices.HYD,
                ProductionAssetType = new ProductionAssetType {
                    Description = ProductionAssetType.Descriptions.HYDRANT,
                    Id = 1
                },
                EquipmentCategory = "C",
                ReferenceEquipmentNumber = "theRefNumber"
            };
            
            var sapEquipment = new SAPEquipment(equipment);

            var target = sapEquipment.ToEquipmentsEquipments();

            Assert.AreEqual("test pdm tool type", target[0].Class_GENERIC.First(x => x.AttribName == "PDMTOOL_TYP").AttribVal);
            Assert.AreEqual("test owned by", target[0].Class_GENERIC.First(x => x.AttribName == "OWNED_BY").AttribVal);
            // In GetCharacteristicValue, it returns CriticalNotes instead
            Assert.AreEqual(equipment.CriticalNotes, target[0].Class_GENERIC.First(x => x.AttribName == "SPECIAL_MAINT_NOTES_DETAILS").AttribVal);
        }

        [TestMethod]
        public void TestToEquipmentEquipmentsWithNullsDoesNotErrorForHydrant()
        {
            SAPEquipment SAPEquipment = new SAPEquipment();
            SAPEquipment.AssetType = ProductionAssetType.Descriptions.HYDRANT;

            var target = SAPEquipment.ToEquipmentsEquipments();

            Assert.AreEqual(target[0].Indicator, SAPEquipment.AddEditIndicator);
            Assert.AreEqual(target[0].EquipmentCategory, SAPEquipment.EquipmentCategory);
            Assert.AreEqual(target[0].InventoryNumber, SAPEquipment.InventoryNumber);
            //Assert.AreEqual(target[0].AssetType, SAPEquipment.AssetType);
            Assert.AreEqual(target[0].Description, SAPEquipment.Description);
            Assert.AreEqual(target[0].RefEquipmentNo, SAPEquipment.ReferenceEquipmentNumber);
            Assert.AreEqual(target[0].FunctionalLocID, SAPEquipment.FunctionalLocID);
            Assert.AreEqual(target[0].StartUpDate?.ToString(), SAPEquipment.StartUpDate?.ToString("yyyyMMdd"));
            Assert.AreEqual(target[0].Manufacturer, SAPEquipment.Manufacturer);
            Assert.AreEqual(target[0].Model?.ToString(), SAPEquipment.Model?.ToString());
            Assert.AreEqual(target[0].YearManufactured?.ToString(), SAPEquipment.YearManufactured?.ToString());
            Assert.AreEqual(target[0].MonthManufactured?.ToString(), SAPEquipment.MonthManufactured?.ToString());
            Assert.AreEqual(target[0].ABCIndicator, SAPEquipment.ABCIndicator);
            Assert.AreEqual(target[0].House, SAPEquipment.House);
            Assert.AreEqual(target[0].Street1, SAPEquipment.Street1?.ToUpper());
            Assert.AreEqual(target[0].Street2, SAPEquipment.Street2);
            Assert.AreEqual(target[0].Street5, SAPEquipment.Street5);
            Assert.AreEqual(target[0].City, SAPEquipment.City?.ToUpper());
            Assert.AreEqual(target[0].OtherCity, SAPEquipment.OtherCity?.ToUpper());
            Assert.AreEqual(target[0].State, SAPEquipment.State?.ToUpper());
            Assert.AreEqual(target[0].Country, SAPEquipment.Country);
            Assert.AreEqual(target[0].ZipCode, SAPEquipment.CityPostalCode);
            Assert.AreEqual(target[0].Latitude, SAPEquipment.Latitude);
            Assert.AreEqual(target[0].Longitude, SAPEquipment.Longitude);
            Assert.AreEqual(target[0].EquipmentUserStatus, SAPEquipment.EquipmentUserStatus);
            Assert.AreEqual(target[0].EquipmentSystemStatus, SAPEquipment.EquipmentSystemStatus);
            Assert.AreEqual(target[0].Size, SAPEquipment.SizeDimension);
            //CHARACTERISTICS
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "SPECIAL_MAINT_NOTES_DETAILS").AttribVal,
                SAPEquipment.HydrantCharacteristics.SPECIAL_MAINT_NOTES_DETAILS);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "PRESSURE_ZONE").AttribVal,
                SAPEquipment.HydrantCharacteristics.PRESSURE_ZONE);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "MAP_PAGE").AttribVal,
                SAPEquipment.HydrantCharacteristics.MAP_PAGE);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "OPEN_DIRECTION").AttribVal,
                SAPEquipment.HydrantCharacteristics.OPEN_DIRECTION);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "HYD_BARREL_SIZE").AttribVal,
                SAPEquipment.HydrantCharacteristics.HYD_BARREL_SIZE);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "EAM_HYD_BURY_DEPTH").AttribVal,
                SAPEquipment.HydrantCharacteristics.EAM_HYD_BURY_DEPTH);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "HYD_BRANCH_LENGTH").AttribVal,
                SAPEquipment.HydrantCharacteristics.HYD_BRANCH_LENGTH);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "HYD_AUX_VALVE_BRANCH_SIZE").AttribVal,
                SAPEquipment.HydrantCharacteristics.HYD_AUX_VALVE_BRANCH_SIZE);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "HYD_DEAD_END_MAIN").AttribVal,
                SAPEquipment.HydrantCharacteristics.HYD_DEAD_END_MAIN);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "HYD_AUX_VALVENUM").AttribVal,
                SAPEquipment.HydrantCharacteristics.HYD_AUX_VALVENUM);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "EAM_PIPE_SIZE").AttribVal,
                SAPEquipment.HydrantCharacteristics.EAM_PIPE_SIZE);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "PIPE_MATERIAL").AttribVal,
                SAPEquipment.HydrantCharacteristics.PIPE_MATERIAL);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "INSTALLATION_WO").AttribVal,
                SAPEquipment.HydrantCharacteristics.INSTALLATION_WO);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "HYD_FIRE_DISTRICT").AttribVal,
                SAPEquipment.HydrantCharacteristics.HYD_FIRE_DISTRICT);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "HYD_ACCOUNT").AttribVal,
                SAPEquipment.HydrantCharacteristics.HYD_ACCOUNT);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "HYD_BILLING_TP").AttribVal,
                SAPEquipment.HydrantCharacteristics.HydrantBillingType);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "HISTORICAL_ID").AttribVal,
                SAPEquipment.HydrantCharacteristics.HISTORICAL_ID);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "HYD_STEAMER_THREAD_TP").AttribVal,
                SAPEquipment.HydrantCharacteristics.SteamerThreadType);
        }

        [TestMethod]
        public void TestToEquipmentEquipmentsWithNullsDoesNotErrorForValve()
        {
            var SAPEquipment = new SAPEquipment();
            SAPEquipment.AssetType = ProductionAssetType.Descriptions.VALVE;

            var target = SAPEquipment.ToEquipmentsEquipments();

            Assert.AreEqual(target[0].Indicator, SAPEquipment.AddEditIndicator);
            Assert.AreEqual(target[0].EquipmentCategory, SAPEquipment.EquipmentCategory);
            Assert.AreEqual(target[0].InventoryNumber, SAPEquipment.InventoryNumber);
            //Assert.AreEqual(target[0].AssetType, SAPEquipment.AssetType);
            Assert.AreEqual(target[0].Description, SAPEquipment.Description);
            Assert.AreEqual(target[0].RefEquipmentNo, SAPEquipment.ReferenceEquipmentNumber);
            Assert.AreEqual(target[0].FunctionalLocID, SAPEquipment.FunctionalLocID);
            Assert.AreEqual(target[0].StartUpDate?.ToString(), SAPEquipment.StartUpDate?.ToString("yyyyMMdd"));
            Assert.AreEqual(target[0].Manufacturer, SAPEquipment.Manufacturer);
            Assert.AreEqual(target[0].Model?.ToString(), SAPEquipment.Model?.ToString());
            Assert.AreEqual(target[0].ABCIndicator, SAPEquipment.ABCIndicator);
            Assert.AreEqual(target[0].House, SAPEquipment.House?.ToUpper());
            Assert.AreEqual(target[0].Street1, SAPEquipment.Street1?.ToUpper());
            Assert.AreEqual(target[0].Street2, SAPEquipment.Street2?.ToUpper());
            Assert.AreEqual(target[0].Street5, SAPEquipment.Street5?.ToUpper());
            Assert.AreEqual(target[0].City, SAPEquipment.City?.ToUpper());
            Assert.AreEqual(target[0].OtherCity, SAPEquipment.OtherCity?.ToUpper());
            Assert.AreEqual(target[0].State, SAPEquipment.State?.ToUpper());
            Assert.AreEqual(target[0].Country, SAPEquipment.Country?.ToUpper());
            Assert.AreEqual(target[0].ZipCode, SAPEquipment.CityPostalCode);
            Assert.AreEqual(target[0].Latitude, SAPEquipment.Latitude);
            Assert.AreEqual(target[0].Longitude, SAPEquipment.Longitude);
            Assert.AreEqual(target[0].EquipmentUserStatus, SAPEquipment.EquipmentUserStatus);
            Assert.AreEqual(target[0].EquipmentSystemStatus, SAPEquipment.EquipmentSystemStatus);
            Assert.AreEqual(target[0].Size, SAPEquipment.SizeDimension);
            //CHARACTERISTICS
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "SPECIAL_MAINT_NOTES_DETAILS").AttribVal,
                SAPEquipment.ValveCharacteristics.SPECIAL_MAINT_NOTES_DETAILS);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "SVLV_TYP").AttribVal,
                SAPEquipment.ValveCharacteristics.SVLV_TYP);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "APPLICATION_SVLV").AttribVal,
                SAPEquipment.ValveCharacteristics.APPLICATION_SVLV);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "MAP_PAGE").AttribVal,
                SAPEquipment.ValveCharacteristics.MAP_PAGE);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "OPEN_DIRECTION").AttribVal,
                SAPEquipment.ValveCharacteristics.OPEN_DIRECTION);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "NUMBER_OF_TURNS").AttribVal,
                SAPEquipment.ValveCharacteristics.NUMBER_OF_TURNS?.ToString());
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "NORMAL_POSITION").AttribVal,
                SAPEquipment.ValveCharacteristics.NORMAL_POSITION);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "VLV_VALVE_SIZE").AttribVal,
                SAPEquipment.ValveCharacteristics.VLV_VALVE_SIZE);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "BYPASS_VALVE").AttribVal,
                SAPEquipment.ValveCharacteristics.BYPASS_VALVE);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "EAM_PIPE_SIZE").AttribVal,
                SAPEquipment.ValveCharacteristics.EAM_PIPE_SIZE);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "PIPE_MATERIAL").AttribVal,
                SAPEquipment.ValveCharacteristics.PIPE_MATERIAL);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "VLV_VALVE_TP").AttribVal,
                SAPEquipment.ValveCharacteristics.VLV_VALVE_TP);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "INSTALLATION_WO").AttribVal,
                SAPEquipment.ValveCharacteristics.INSTALLATION_WO);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "HISTORICAL_ID").AttribVal,
                SAPEquipment.ValveCharacteristics.HISTORICAL_ID);
        }

        [TestMethod]
        public void TestToEquipmentEquipmentsWithNullsDoesNotErrorForBlowOff()
        {
            var SAPEquipment = new SAPEquipment();
            SAPEquipment.AssetType = ProductionAssetType.Descriptions.VALVE;
            SAPEquipment.ValveControl = "BLOW OFF WITH FLUSHING";

            var target = SAPEquipment.ToEquipmentsEquipments();

            Assert.AreEqual(target[0].Indicator, SAPEquipment.AddEditIndicator);
            Assert.AreEqual(target[0].EquipmentCategory, SAPEquipment.EquipmentCategory);
            Assert.AreEqual(target[0].InventoryNumber, SAPEquipment.InventoryNumber);
            //Assert.AreEqual(target[0].AssetType, SAPEquipment.AssetType);
            Assert.AreEqual(target[0].Description, SAPEquipment.Description);
            Assert.AreEqual(target[0].RefEquipmentNo, SAPEquipment.ReferenceEquipmentNumber);
            Assert.AreEqual(target[0].FunctionalLocID, SAPEquipment.FunctionalLocID);
            Assert.AreEqual(target[0].StartUpDate?.ToString(), SAPEquipment.StartUpDate?.ToString("yyyyMMdd"));
            Assert.AreEqual(target[0].Manufacturer, SAPEquipment.Manufacturer);
            Assert.AreEqual(target[0].Model?.ToString(), SAPEquipment.Model?.ToString());
            Assert.AreEqual(target[0].ABCIndicator, SAPEquipment.ABCIndicator);
            Assert.AreEqual(target[0].House, SAPEquipment.House?.ToUpper());
            Assert.AreEqual(target[0].Street1, SAPEquipment.Street1?.ToUpper());
            Assert.AreEqual(target[0].Street2, SAPEquipment.Street2?.ToUpper());
            Assert.AreEqual(target[0].Street5, SAPEquipment.Street5?.ToUpper());
            Assert.AreEqual(target[0].City, SAPEquipment.City?.ToUpper());
            Assert.AreEqual(target[0].OtherCity, SAPEquipment.OtherCity?.ToUpper());
            Assert.AreEqual(target[0].State, SAPEquipment.State?.ToUpper());
            Assert.AreEqual(target[0].Country, SAPEquipment.Country);
            Assert.AreEqual(target[0].ZipCode, SAPEquipment.CityPostalCode);
            Assert.AreEqual(target[0].Latitude, SAPEquipment.Latitude);
            Assert.AreEqual(target[0].Longitude, SAPEquipment.Longitude);
            Assert.AreEqual(target[0].EquipmentUserStatus, SAPEquipment.EquipmentUserStatus);
            Assert.AreEqual(target[0].EquipmentSystemStatus, SAPEquipment.EquipmentSystemStatus);
            Assert.AreEqual(target[0].Size, SAPEquipment.SizeDimension);
            //CHARACTERISTICS
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "SPECIAL_MAINT_NOTES_DETAILS").AttribVal,
                SAPEquipment.BlowOffCharacteristics.SPECIAL_MAINT_NOTES_DETAILS);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "SVLV_BO_TYP").AttribVal,
                SAPEquipment.BlowOffCharacteristics.SVLV_BO_TYP);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "APPLICATION_SVLV_BO").AttribVal,
                SAPEquipment.BlowOffCharacteristics.APPLICATION_SVLV_BO);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "MAP_PAGE").AttribVal,
                SAPEquipment.BlowOffCharacteristics.MAP_PAGE);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "OPEN_DIRECTION").AttribVal,
                SAPEquipment.BlowOffCharacteristics.OPEN_DIRECTION?.ToUpper());
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "NUMBER_OF_TURNS").AttribVal,
                SAPEquipment.BlowOffCharacteristics.NUMBER_OF_TURNS?.ToString());
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "NORMAL_POSITION").AttribVal,
                SAPEquipment.BlowOffCharacteristics.NORMAL_POSITION);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "VLV_VALVE_SIZE").AttribVal,
                SAPEquipment.BlowOffCharacteristics.VLV_VALVE_SIZE);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "BYPASS_VALVE").AttribVal,
                SAPEquipment.BlowOffCharacteristics.BYPASS_VALVE);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "EAM_PIPE_SIZE").AttribVal,
                SAPEquipment.BlowOffCharacteristics.EAM_PIPE_SIZE);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "PIPE_MATERIAL").AttribVal,
                SAPEquipment.BlowOffCharacteristics.PIPE_MATERIAL);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "VLV_VALVE_TP").AttribVal,
                SAPEquipment.BlowOffCharacteristics.VLV_VALVE_TP);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "INSTALLATION_WO").AttribVal,
                SAPEquipment.BlowOffCharacteristics.INSTALLATION_WO);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "HISTORICAL_ID").AttribVal,
                SAPEquipment.BlowOffCharacteristics.HISTORICAL_ID);
        }

        [TestMethod]
        public void TestToEquipmentEquipmentsWithNullsDoesNotErrorForOpening()
        {
            var SAPEquipment = new SAPEquipment();
            SAPEquipment.AssetType = ProductionAssetType.Descriptions.OPENING;
            var target = SAPEquipment.ToEquipmentsEquipments();

            Assert.AreEqual(target[0].Indicator, SAPEquipment.AddEditIndicator);
            Assert.AreEqual(target[0].EquipmentCategory, SAPEquipment.EquipmentCategory);
            Assert.AreEqual(target[0].InventoryNumber, SAPEquipment.InventoryNumber);
            //Assert.AreEqual(target[0].AssetType, SAPEquipment.AssetType);
            Assert.AreEqual(target[0].Description, SAPEquipment.Description);
            Assert.AreEqual(target[0].RefEquipmentNo, SAPEquipment.ReferenceEquipmentNumber);
            Assert.AreEqual(target[0].FunctionalLocID, SAPEquipment.FunctionalLocID);
            Assert.AreEqual(target[0].StartUpDate?.ToString(), SAPEquipment.StartUpDate?.ToString("yyyyMMdd"));
            Assert.AreEqual(target[0].Manufacturer, SAPEquipment.Manufacturer);
            Assert.AreEqual(target[0].Model?.ToString(), SAPEquipment.Model?.ToString());
            Assert.AreEqual(target[0].ABCIndicator, SAPEquipment.ABCIndicator);
            Assert.AreEqual(target[0].House, SAPEquipment.House?.ToUpper());
            Assert.AreEqual(target[0].Street1, SAPEquipment.Street1?.ToUpper());
            Assert.AreEqual(target[0].Street2?.ToString(), SAPEquipment.Street2?.ToString());
            Assert.AreEqual(target[0].Street5, SAPEquipment.Street5?.ToUpper());
            Assert.AreEqual(target[0].City, SAPEquipment.City?.ToUpper());
            Assert.AreEqual(target[0].OtherCity, SAPEquipment.OtherCity?.ToUpper());
            Assert.AreEqual(target[0].State, SAPEquipment.State?.ToUpper());
            Assert.AreEqual(target[0].Country, SAPEquipment.Country);
            Assert.AreEqual(target[0].ZipCode, SAPEquipment.CityPostalCode);
            Assert.AreEqual(target[0].Latitude, SAPEquipment.Latitude);
            Assert.AreEqual(target[0].Longitude, SAPEquipment.Longitude);
            Assert.AreEqual(target[0].EquipmentUserStatus, SAPEquipment.EquipmentUserStatus);
            Assert.AreEqual(target[0].EquipmentSystemStatus, SAPEquipment.EquipmentSystemStatus);
            Assert.AreEqual(target[0].Size, SAPEquipment.SizeDimension);

            //CHARACTERISTICS
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "MATERIAL_OF_CONSTRUCTION_MH").AttribVal,
                SAPEquipment.OpeningCharacteristics.MATERIAL_OF_CONSTRUCTION_MH);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "MAP_PAGE").AttribVal,
                SAPEquipment.OpeningCharacteristics.MAP_PAGE);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "INSTALLATION_WO").AttribVal,
                SAPEquipment.OpeningCharacteristics.INSTALLATION_WO);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "MH_DEPTH").AttribVal,
                SAPEquipment.OpeningCharacteristics.MH_DEPTH?.ToString());
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "HISTORICAL_ID").AttribVal,
                SAPEquipment.OpeningCharacteristics.HISTORICAL_ID);
            Assert.AreEqual(target[0].Class_GENERIC.First(x => x.AttribName == "MH_TYP").AttribVal,
                SAPEquipment.OpeningCharacteristics.MH_TYP);
        }

        [TestMethod]
        public void TestToEquipmentEquipmentsCleanOutMapsCharacteristicCorrectly()
        {
            var cleanout = new Equipment {
                EquipmentType = new EquipmentType {
                    Id = EquipmentType.Indices.CO,
                    ProductionAssetType = new ProductionAssetType {
                        Description = "Hydrant",
                        Id = 1
                    },
                    EquipmentCategory = "C",
                    ReferenceEquipmentNumber = "theRefNumber"
                },
                //   Identifier = "NJ-1-ABC-1",
                CriticalNotes = "this turns into a characteristic"
            };

            var target = new SAPEquipment(cleanout).ToEquipmentsEquipments();

            Assert.AreEqual(
                cleanout.CriticalNotes,
                target[0].Class_GENERIC.First(x => x.AttribName == "SPECIAL_MAINT_NOTES_DETAILS").AttribVal);
        }

        [TestMethod]
        public void TestConstructorDoesNotFailWithNullsForValves()
        {
            valve = new Valve();
            var target = new SAPEquipment(valve);
            var SAPEquipmentNumber = (valve.SAPEquipmentId != null && valve.SAPEquipmentId != 0)
                ? valve.SAPEquipmentId.ToString().PadLeft(18, '0')
                : string.Empty;

            Assert.AreEqual(valve.ValveNumber, target.InventoryNumber);
            Assert.AreEqual(SAPEquipmentNumber, target.SAPEquipmentNumber);
            Assert.AreEqual(valve.FunctionalLocation?.Description, target.FunctionalLocID);
            Assert.AreEqual(valve.ValveMake?.Description, target.Manufacturer);
            Assert.AreEqual(valve.DateInstalled, target.StartUpDate);
            Assert.AreEqual(valve.Critical, target.Critical);
            Assert.AreEqual(valve.StreetNumber, target.House);
            Assert.AreEqual(valve.Street?.FullStName, target.Street1);
            Assert.AreEqual(valve.CrossStreet?.FullStName, target.Street2);
            Assert.AreEqual(valve.Town?.County.Name, target.Street5);
            Assert.AreEqual(valve.Town?.ShortName, target.City);
            Assert.AreEqual(valve.Town?.State.Abbreviation, target.State);
            Assert.AreEqual(valve.TownSection?.Description, target.OtherCity);
            Assert.AreEqual("US", target.Country);
            Assert.AreEqual(valve.Town?.Zip, target.CityPostalCode);
            Assert.AreEqual(valve.Coordinate?.Latitude.ToString(), target.Latitude);
            Assert.AreEqual(valve.Coordinate?.Longitude.ToString(), target.Longitude);
            Assert.AreEqual(valve.ValveSize?.Description, target.SizeDimension);
            Assert.AreEqual(valve.Status?.Description, target.EquipmentStatus);

            Assert.AreEqual(valve.CriticalNotes,
                target.ValveCharacteristics.SPECIAL_MAINT_NOTES_DETAILS); //SPECIAL_MAINT_NOTES_DETAILS
            Assert.AreEqual("", target.ValveCharacteristics.SVLV_TYP); //SVLV_TYP
            Assert.AreEqual("", target.ValveCharacteristics.APPLICATION_SVLV); //APPLICATION_SVLV
            Assert.AreEqual(valve.MapPage, target.ValveCharacteristics.MAP_PAGE); //MAP_PAGE
            Assert.AreEqual(valve.OpenDirection?.Description,
                target.ValveCharacteristics.OPEN_DIRECTION); //OPEN_DIRECTION
            Assert.AreEqual(valve.Turns,
                target.ValveCharacteristics.NUMBER_OF_TURNS != 0
                    ? target.ValveCharacteristics.NUMBER_OF_TURNS
                    : null); //NUMBER_OF_TURNS
            Assert.AreEqual(valve.NormalPosition?.Description.ToString(),
                target.ValveCharacteristics.NORMAL_POSITION); //NORMAL_POSITION
            Assert.AreEqual(valve.ValveSize?.Size.ToString(),
                target.ValveCharacteristics.VLV_VALVE_SIZE?.ToString()); //VLV_VALVE_SIZE
            Assert.AreEqual("N", target.ValveCharacteristics.BYPASS_VALVE); //BYPASS_VALVE
            Assert.AreEqual(valve.MainType?.SAPCode, target.ValveCharacteristics.PIPE_MATERIAL); //PIPE_MATERIAL
            Assert.AreEqual(valve.ValveType?.SAPCode, target.ValveCharacteristics.VLV_VALVE_TP); //VLV_VALVE_TP
            Assert.AreEqual(valve.WorkOrderNumber, target.ValveCharacteristics.INSTALLATION_WO);
            Assert.AreEqual(valve.Id.ToString(), target.ValveCharacteristics.HISTORICAL_ID.ToString());
        }

        [TestMethod]
        public void TestConstructorDoesNotFailWithNullsForOpening()
        {
            _opening = new SewerOpening();
            var target = new SAPEquipment(_opening);
            var SAPEquipmentNumber = (_opening.SAPEquipmentId != null && _opening.SAPEquipmentId != 0)
                ? _opening.SAPEquipmentId.ToString().PadLeft(18, '0')
                : string.Empty;

            Assert.AreEqual(_opening.OpeningNumber, target.InventoryNumber);
            Assert.AreEqual(SAPEquipmentNumber, target.SAPEquipmentNumber);
            Assert.AreEqual(_opening.FunctionalLocation?.Description, target.FunctionalLocID);
            Assert.AreEqual(_opening.DateInstalled, target.StartUpDate);
            Assert.AreEqual(_opening.StreetNumber, target.House);
            Assert.AreEqual(_opening.Street?.FullStName, target.Street1);
            Assert.AreEqual(_opening.IntersectingStreet?.FullStName, target.Street2);
            Assert.AreEqual(_opening.Town?.County.Name, target.Street5);
            Assert.AreEqual(_opening.Town?.State.Abbreviation, target.State);
            Assert.AreEqual(_opening.TownSection?.Description, target.OtherCity);
            Assert.AreEqual("US", target.Country);
            Assert.AreEqual(_opening.Town?.Zip, target.CityPostalCode);
            Assert.AreEqual(_opening.Coordinate?.Latitude.ToString(), target.Latitude);
            Assert.AreEqual(_opening.Coordinate?.Longitude.ToString(), target.Longitude);
            Assert.AreEqual(_opening.Status?.Description, target.EquipmentStatus);
            //OpeningCharacteristics	
            Assert.AreEqual(_opening.SewerOpeningMaterial?.SAPCode.ToString(),
                target.OpeningCharacteristics.MATERIAL_OF_CONSTRUCTION_MH); //MAP_PAGE
            Assert.AreEqual(_opening.MapPage, target.OpeningCharacteristics.MAP_PAGE); //MAP_PAGE
            Assert.AreEqual(_opening.TaskNumber, target.OpeningCharacteristics.INSTALLATION_WO); //INSTALLATION_WO
            Assert.AreEqual(_opening.DepthToInvert,
                target.OpeningCharacteristics.MH_DEPTH != 0
                    ? target.OpeningCharacteristics.MH_DEPTH
                    : null); //INSTALLATION_WO
        }

        [TestMethod]
        public void TestConstructorDoesNotFailWithNullsForHydrant()
        {
            string DepthBuryFeet, DepthBuryInches, BranchLengthFeet, BranchLengthInches;
            var HydrantBilling = "";
            hydrant = new Hydrant();
            var target = new SAPEquipment(hydrant);
            var SAPEquipmentNumber = (hydrant.SAPEquipmentId != null && hydrant.SAPEquipmentId != 0)
                ? hydrant.SAPEquipmentId.ToString().PadLeft(18, '0')
                : string.Empty;

            DepthBuryFeet = hydrant.DepthBuryFeet != null ? hydrant.DepthBuryFeet.ToString() + "FT " : "";
            DepthBuryInches = hydrant.DepthBuryInches != null ? hydrant.DepthBuryInches.ToString() + "IN" : "";

            BranchLengthFeet = hydrant.BranchLengthFeet != null ? hydrant.BranchLengthFeet.ToString() + "FT " : "";
            BranchLengthInches = hydrant.BranchLengthInches != null ? hydrant.BranchLengthInches.ToString() + "IN" : "";

            Assert.AreEqual(hydrant.HydrantNumber, target.InventoryNumber);
            Assert.AreEqual(SAPEquipmentNumber, target.SAPEquipmentNumber);
            Assert.AreEqual(hydrant.FunctionalLocation?.Description, target.FunctionalLocID);
            Assert.AreEqual(hydrant.DateInstalled, target.StartUpDate);
            Assert.AreEqual(hydrant.HydrantManufacturer?.Description, target.Manufacturer);
            Assert.AreEqual(hydrant.HydrantModel?.Description, target.Model);
            Assert.AreEqual(hydrant.YearManufactured, target.YearManufactured);
            Assert.AreEqual(hydrant.Critical, target.Critical);
            Assert.AreEqual(hydrant.StreetNumber, target.House);
            Assert.AreEqual(hydrant.Street?.FullStName, target.Street1);
            Assert.AreEqual(hydrant.CrossStreet?.FullStName, target.Street2);
            Assert.AreEqual(hydrant.Town?.County.Name, target.Street5);
            Assert.AreEqual(hydrant.Town?.ShortName, target.City);
            Assert.AreEqual(hydrant.Town?.State.Abbreviation, target.State);
            Assert.AreEqual(hydrant.TownSection?.Description, target.OtherCity);
            Assert.AreEqual("US", target.Country);
            Assert.AreEqual(hydrant.Town?.Zip, target.CityPostalCode);
            Assert.AreEqual(hydrant.Coordinate?.Latitude.ToString(), target.Latitude);
            Assert.AreEqual(hydrant.Coordinate?.Longitude.ToString(), target.Longitude);
            Assert.AreEqual(hydrant.HydrantSize?.Size.ToString(), target.SizeDimension);
            Assert.AreEqual(hydrant.Status?.Description, target.EquipmentStatus);
            //HydrantCharacteristics		
            switch (hydrant.HydrantBilling?.Description.ToString())
            {
                case "Private":
                    HydrantBilling = "PRIVATE FIRE";
                    break;
                case "Municipal":
                case "Public":
                    HydrantBilling = "PUBLIC FIRE";
                    break;
                case "Company":
                    HydrantBilling = "UNBILLED AW HYDRANT";
                    break;
                default:
                    HydrantBilling = string.Empty;
                    break;
            }

            Assert.AreEqual(hydrant.CriticalNotes?.ToUpper(),
                target.HydrantCharacteristics.SPECIAL_MAINT_NOTES_DETAILS);
            Assert.AreEqual(hydrant.Gradient?.Description, target.HydrantCharacteristics.PRESSURE_ZONE);
            Assert.AreEqual(hydrant.MapPage, target.HydrantCharacteristics.MAP_PAGE);
            Assert.AreEqual(hydrant.OpenDirection?.Description, target.HydrantCharacteristics.OPEN_DIRECTION);
            Assert.AreEqual(hydrant.HydrantSize?.Size.ToString(), target.HydrantCharacteristics.HYD_BARREL_SIZE);
            Assert.AreEqual(DepthBuryFeet + DepthBuryInches, target.HydrantCharacteristics.EAM_HYD_BURY_DEPTH);
            Assert.AreEqual(BranchLengthFeet + BranchLengthInches, target.HydrantCharacteristics.HYD_BRANCH_LENGTH);
            Assert.AreEqual(hydrant.LateralSize?.Size.ToString(),
                target.HydrantCharacteristics.HYD_AUX_VALVE_BRANCH_SIZE);
            Assert.AreEqual(Convert.ToString(hydrant.IsDeadEndMain == true ? "Y" : "N"),
                Convert.ToString(target.HydrantCharacteristics.HYD_DEAD_END_MAIN));
            Assert.AreEqual(hydrant.LateralValve?.ValveNumber, target.HydrantCharacteristics.HYD_AUX_VALVENUM);
            Assert.AreEqual(hydrant.HydrantThreadType?.Description,
                target.HydrantCharacteristics.HYD_STEAMER_THREAD_TP);
            Assert.AreEqual(hydrant.HydrantMainSize?.Size.ToString(), target.HydrantCharacteristics.EAM_PIPE_SIZE);
            Assert.AreEqual(hydrant.MainType?.SAPCode, target.HydrantCharacteristics.PIPE_MATERIAL);
            Assert.AreEqual(hydrant.WorkOrderNumber, target.HydrantCharacteristics.INSTALLATION_WO);
            Assert.AreEqual(hydrant.FireDistrict?.DistrictName, target.HydrantCharacteristics.HYD_FIRE_DISTRICT);
            Assert.AreEqual(hydrant.FireDistrict?.PremiseNumber, target.HydrantCharacteristics.HYD_ACCOUNT);
            Assert.AreEqual(hydrant.HydrantBilling?.Description.ToUpper(),
                target.HydrantCharacteristics.HYD_BILLING_TP);
            Assert.AreEqual(hydrant.Id.ToString(), target.HydrantCharacteristics.HISTORICAL_ID.ToString());
        }

        [TestMethod]
        public void TestStringFormatForSize()
        {
            decimal SizeValue = Convert.ToDecimal(5.250);
            Assert.AreEqual("5.25", string.Format(CommonStringFormats.DECIMAL_WITHOUT_TRAILING_ZEROS, SizeValue));

            SizeValue = Convert.ToDecimal(0.2500);
            Assert.AreEqual("0.25", string.Format(CommonStringFormats.DECIMAL_WITHOUT_TRAILING_ZEROS, SizeValue));

            SizeValue = 0.2500m;
            Assert.AreEqual("0.25", string.Format(CommonStringFormats.DECIMAL_WITHOUT_TRAILING_ZEROS, SizeValue));
        }

        [TestMethod]
        public void TestUserStatusForEquipmentReturnsCorrectSAPString()
        {
            var target = new SAPEquipment {EquipmentStatus = "RETIRED"};

            Assert.AreEqual("REMV", target.UserStatusForEquipment);
        }

        [TestMethod]
        public void TestEquipmentUserStatusReturnsINSVFORUserStatusForEquipment()
        {
            var target = new SAPEquipment {EquipmentStatus = "IN SERVICE"};

            foreach (var assetType in SAPEquipment.USER_SYSTEM_STATUS_FOR_EQUIPMENT_ASSET_TYPES)
            {
                target.AssetType = assetType;
                Assert.AreEqual("INSV", target.ToEquipmentsEquipments()[0].EquipmentUserStatus);
            }
        }
    }
}
