using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Migrations;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;
using MapCall.SAP.Model.Entities;
using StructureMap;


namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels
{
    [TestClass]
    public class CreateSapNotificationWorkOrderTest : MapCallMvcInMemoryDatabaseTestBase<WorkOrder>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            //_container.Inject<IRepository<WorkOrderPriority>>(_container.GetInstance<RepositoryBase<WorkOrderPriority>>());
        }
        
        #endregion
 
        [TestMethod]
        public void TestToCreateWorkOrderSetsPriorityAndOtherFields()
        {
            var sapNotificationNumber = 432;
            var priority = GetFactory<EmergencyWorkOrderPriorityFactory>().Create();
            var purpose = GetFactory<CustomerWorkOrderPurposeFactory>().Create();
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create(new {OperatingCenterCode = "NJ7", Name = "Shrewsbury"});
            var planningPlant = GetFactory<PlanningPlantFactory>().Create(new { OperatingCenter = operatingCenter, Code = "D205" });
            var town = GetFactory<TownFactory>().Create(new { ShortName = "Long Branch"});
            var street = GetFactory<StreetFactory>().Create(new { Town = town, FullStName = "rename this " });
            var xstreet = GetFactory<StreetFactory>().Create(new { Town = town, FullStName = "property already" });
            var townSection = GetFactory<TownSectionFactory>().Create(new {
                Town = town, Name = "town section"
            });
            var assetType = GetFactory<ValveAssetTypeFactory>().Create();

            town.TownSections.Add(townSection);
            town.Streets.Add(street);
            town.Streets.Add(xstreet);
            operatingCenter.OperatingCenterTowns.Add(new OperatingCenterTown { OperatingCenter = operatingCenter, Town = town, Abbreviation = "XX" });
            Session.Flush();
            
            var sapNotification = new SAPNotification {
                SAPNotificationNumber = sapNotificationNumber.ToString(),
                Priority = "1: Emergency 1-2 Hrs",
                PurposeCodingCode = "I01", // Customer
                CustomerName = "Chidi Anagonye",
                Telephone = "7328675309",
                NotificationLongText = "notification long text", 
                PlanningPlant = planningPlant.Code, 
                City = town.ShortName, 
                Street1 = street.FullStName,
                Street2 = xstreet.FullStName, 
                House = "227", 
                Street5 = townSection.Description, 
                CityPostalCode = "12345",
                Premise = "987654321",
                AssetType = assetType.Description,
                Installation = "12345678"
            };

            var target = _viewModelFactory.Build<CreateSapNotificationWorkOrder, SAPNotification>( sapNotification).ToCreateWorkOrder();

            Assert.AreEqual(sapNotificationNumber, target.SAPNotificationNumber);
            Assert.AreEqual(priority.Id, target.Priority);
            Assert.AreEqual(purpose.Id, target.Purpose);
            Assert.AreEqual(1, target.RequestedBy);
            Assert.AreEqual(sapNotification.CustomerName, target.CustomerName);
            Assert.AreEqual(sapNotification.Telephone, target.PhoneNumber);
            //Assert.AreEqual(sapNotification.NotificationLongText, target.Notes);

            Assert.AreEqual(town.Id, target.Town);
            Assert.AreEqual(street.Id, target.Street);
            Assert.AreEqual(xstreet.Id, target.NearestCrossStreet);
            Assert.AreEqual(townSection.Id, target.TownSection);
            Assert.AreEqual(sapNotification.House, target.StreetNumber);
            Assert.AreEqual(sapNotification.CityPostalCode, target.ZipCode);
            Assert.AreEqual(1, target.RequestedBy);
            Assert.AreEqual(sapNotification.Premise, target.PremiseNumber);
            Assert.AreEqual(sapNotification.Installation, target.Installation.ToString());
        }

        [TestMethod]
        public void TestToCreateWorkOrderSetsTownFromMainEquipmentId()
        {
            var sapNotificationNumber = 432;
            var equipmentId = "000012345";
            var priority = GetFactory<EmergencyWorkOrderPriorityFactory>().Create();
            var purpose = GetFactory<CustomerWorkOrderPurposeFactory>().Create();
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "NJ7", Name = "Shrewsbury" });
            var planningPlant = GetFactory<PlanningPlantFactory>().Create(new { OperatingCenter = operatingCenter, Code = "D205" });
            var town = GetFactory<TownFactory>().Create(new { ShortName = "Long Branch" });
            var street = GetFactory<StreetFactory>().Create(new { Town = town, FullStName = "rename this " });
            var xstreet = GetFactory<StreetFactory>().Create(new { Town = town, FullStName = "property already" });
            var townSection = GetFactory<TownSectionFactory>().Create(new {
                Town = town, Name = "town section"
            });
            var assetType = GetFactory<MainAssetTypeFactory>().Create();
            town.TownSections.Add(townSection);
            town.Streets.Add(street);
            town.Streets.Add(xstreet);
            var operatingCenterTown = new OperatingCenterTown {
                OperatingCenter = operatingCenter,
                Town = town,
                Abbreviation = "XX",
                MainSAPEquipmentId = 12345
            };
            operatingCenter.OperatingCenterTowns.Add(operatingCenterTown);
            town.OperatingCentersTowns.Add(operatingCenterTown);
            Session.Flush();

            var sapNotification = new SAPNotification
            {
                SAPNotificationNumber = sapNotificationNumber.ToString(),
                Equipment = equipmentId,
                Priority = "1: Emergency 1-2 Hrs",
                PurposeCodingCode = "I01", // Customer
                CustomerName = "Chidi Anagonye",
                Telephone = "7328675309",
                NotificationLongText = "notification long text",
                PlanningPlant = planningPlant.Code,
                City = "mispelled",
                Street1 = street.FullStName,
                Street2 = xstreet.FullStName,
                House = "227",
                Street5 = townSection.Description,
                CityPostalCode = "12345",
                Premise = "987654321",
                AssetType = assetType.Description
            };

            var target = _viewModelFactory.Build<CreateSapNotificationWorkOrder, SAPNotification>( sapNotification).ToCreateWorkOrder();
            Assert.AreEqual(town.Id, target.Town);
        }

        [TestMethod]
        public void TestToCreateWorkOrderSetsTownFromSewerMainEquipmentId()
        {
            var sapNotificationNumber = 432;
            var equipmentId = "000012345";
            var priority = GetFactory<EmergencyWorkOrderPriorityFactory>().Create();
            var purpose = GetFactory<CustomerWorkOrderPurposeFactory>().Create();
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "NJ7", Name = "Shrewsbury" });
            var planningPlant = GetFactory<PlanningPlantFactory>().Create(new { OperatingCenter = operatingCenter, Code = "D205" });
            var town = GetFactory<TownFactory>().Create(new { ShortName = "Long Branch" });
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = operatingCenter, Town = town, SewerMainSAPEquipmentId = 12345 });
            var street = GetFactory<StreetFactory>().Create(new { Town = town, FullStName = "rename this " });
            var xstreet = GetFactory<StreetFactory>().Create(new { Town = town, FullStName = "property already" });
            var townSection = GetFactory<TownSectionFactory>().Create(new {
                Town = town, Name = "town section"
            });
            var assetType = GetFactory<SewerMainAssetTypeFactory>().Create();
            town.TownSections.Add(townSection);
            town.Streets.Add(street);
            town.Streets.Add(xstreet);
            operatingCenter.OperatingCenterTowns.Add(new OperatingCenterTown { OperatingCenter = operatingCenter, Town = town, Abbreviation = "XX", SewerMainSAPEquipmentId = 12345 });
            Session.Flush();

            var sapNotification = new SAPNotification
            {
                SAPNotificationNumber = sapNotificationNumber.ToString(),
                Equipment = equipmentId,
                Priority = "1: Emergency 1-2 Hrs",
                PurposeCodingCode = "I01", // Customer
                CustomerName = "Chidi Anagonye",
                Telephone = "7328675309",
                NotificationLongText = "notification long text",
                PlanningPlant = planningPlant.Code,
                City = "mispelled",
                Street1 = street.FullStName,
                Street2 = xstreet.FullStName,
                House = "227",
                Street5 = townSection.Description,
                CityPostalCode = "12345",
                Premise = "987654321",
                AssetType = assetType.Description, 
                Latitude = "38", 
                Longitude = "-74"
            };

            var target = _viewModelFactory.Build<CreateSapNotificationWorkOrder, SAPNotification>( sapNotification).ToCreateWorkOrder();
            Assert.AreEqual(town.Id, target.Town);
            Assert.AreEqual(sapNotification.Latitude, target.Latitude.ToString());
            Assert.AreEqual(sapNotification.Longitude, target.Longitude.ToString());
        }

        [TestMethod]
        public void TestTrySetAssetSetsHydrantProperties()
        {
            var priority = GetFactory<EmergencyWorkOrderPriorityFactory>().Create();
            var purpose = GetFactory<CustomerWorkOrderPurposeFactory>().Create();
            Session.Flush();

            var equipmentId = "000012345";
            var sapNotificationNumber = 432;
            var hydrant = GetEntityFactory<Hydrant>().Create(new { SAPEquipmentId = 12345 });
            var sapNotification = new SAPNotification
            {
                Equipment = equipmentId,
                AssetType = "HYDRANT",
                SAPNotificationNumber = sapNotificationNumber.ToString(),
                Priority = "1: Emergency 1-2 Hrs", 
                PurposeCodingCode = "I01", // Customer
                CustomerName = "Chidi Anagonye",
                Telephone = "7328675309",
                NotificationLongText = "notification long text"
            };

            var target = _viewModelFactory.Build<CreateSapNotificationWorkOrder, SAPNotification>( sapNotification).ToCreateWorkOrder();

            Assert.AreEqual(AssetType.Indices.HYDRANT, target.AssetType);
            Assert.AreEqual(sapNotificationNumber, target.SAPNotificationNumber);
            Assert.AreEqual(priority.Id, target.Priority);
            Assert.AreEqual(purpose.Id, target.Purpose);
            Assert.AreEqual(1, target.RequestedBy);
            Assert.AreEqual(sapNotification.CustomerName, target.CustomerName);
            Assert.AreEqual(sapNotification.Telephone, target.PhoneNumber);
            //Assert.AreEqual(sapNotification.NotificationLongText, target.Notes);
            Assert.AreEqual(hydrant.Id, target.Hydrant);
        }

        [TestMethod]
        public void TestTrySetAssetSetsValveProperties()
        {
            var priority = GetFactory<EmergencyWorkOrderPriorityFactory>().Create();
            var purpose = GetFactory<CustomerWorkOrderPurposeFactory>().Create();
            Session.Flush();

            var equipmentId = "000012345";
            var sapNotificationNumber = 432;
            var valve = GetEntityFactory<Valve>().Create(new { SAPEquipmentId = 12345 });
            var sapNotification = new SAPNotification
            {
                Equipment = equipmentId,
                AssetType = "VALVE",
                SAPNotificationNumber = sapNotificationNumber.ToString(),
                Priority = "1: Emergency 1-2 Hrs",
                PurposeCodingCode = "I01", // Customer
                CustomerName = "Chidi Anagonye",
                Telephone = "7328675309",
                NotificationLongText = "notification long text"
            };

            var target = _viewModelFactory.Build<CreateSapNotificationWorkOrder, SAPNotification>( sapNotification).ToCreateWorkOrder();

            Assert.AreEqual(AssetType.Indices.VALVE, target.AssetType);
            Assert.AreEqual(sapNotificationNumber, target.SAPNotificationNumber);
            Assert.AreEqual(priority.Id, target.Priority);
            Assert.AreEqual(purpose.Id, target.Purpose);
            Assert.AreEqual(1, target.RequestedBy);
            Assert.AreEqual(sapNotification.CustomerName, target.CustomerName);
            Assert.AreEqual(sapNotification.Telephone, target.PhoneNumber);
            //Assert.AreEqual(sapNotification.NotificationLongText, target.Notes);
            Assert.AreEqual(valve.Id, target.Valve);
        }

        [TestMethod]
        public void TestTrySetAssetSetsSewerOpeningProperties()
        {
            var priority = GetFactory<EmergencyWorkOrderPriorityFactory>().Create();
            var purpose = GetFactory<CustomerWorkOrderPurposeFactory>().Create();
            Session.Flush();

            var equipmentId = "000012345";
            var sapNotificationNumber = 432;
            var sewerOpening = GetEntityFactory<SewerOpening>().Create(new { SAPEquipmentId = 12345 });
            var sapNotification = new SAPNotification
            {
                Equipment = equipmentId,
                AssetType = "SEWER OPENING",
                SAPNotificationNumber = sapNotificationNumber.ToString(),
                Priority = "1: Emergency 1-2 Hrs",
                PurposeCodingCode = "I01", // Customer
                CustomerName = "Chidi Anagonye",
                Telephone = "7328675309",
                NotificationLongText = "notification long text"
            };

            var target = _viewModelFactory.Build<CreateSapNotificationWorkOrder, SAPNotification>(sapNotification).ToCreateWorkOrder();

            Assert.AreEqual(AssetType.Indices.SEWER_OPENING, target.AssetType);
            Assert.AreEqual(sapNotificationNumber, target.SAPNotificationNumber);
            Assert.AreEqual(priority.Id, target.Priority);
            Assert.AreEqual(purpose.Id, target.Purpose);
            Assert.AreEqual(1, target.RequestedBy);
            Assert.AreEqual(sapNotification.CustomerName, target.CustomerName);
            Assert.AreEqual(sapNotification.Telephone, target.PhoneNumber);
            //Assert.AreEqual(sapNotification.NotificationLongText, target.Notes);
            Assert.AreEqual(sewerOpening.Id, target.SewerOpening);
        }

        // TODO: Move this to a property in the database
        [TestMethod]
        public void TestPurposeReturnsCorrectPurposeForCode()
        {
            var target = _viewModelFactory.BuildWithOverrides<CreateSapNotificationWorkOrder>(new {PurposeCodingCode = "I01"});

            Assert.AreEqual("Customer", target.Purpose);

            target.PurposeCodingCode = "I02";
            Assert.AreEqual("Equip Reliability", target.Purpose);
            target.PurposeCodingCode = "I03";
            Assert.AreEqual("Safety", target.Purpose);
            target.PurposeCodingCode = "I04";
            Assert.AreEqual("Compliance", target.Purpose);
            target.PurposeCodingCode = "I05";
            Assert.AreEqual("Regulatory", target.Purpose);
            target.PurposeCodingCode = "I06";
            Assert.AreEqual("Seasonal", target.Purpose);
            target.PurposeCodingCode = "I07";
            Assert.AreEqual("Leak Detection", target.Purpose);
            target.PurposeCodingCode = "I08";
            Assert.AreEqual("Revenue 150-500", target.Purpose);
            target.PurposeCodingCode = "I09";
            Assert.AreEqual("Revenue 500-1000", target.Purpose);
            target.PurposeCodingCode = "I10";
            Assert.AreEqual("Revenue >1000", target.Purpose);
            target.PurposeCodingCode = "I11";
            Assert.AreEqual("Damaged/Billable", target.Purpose);
            target.PurposeCodingCode = "I12";
            Assert.AreEqual("Estimates", target.Purpose);
            target.PurposeCodingCode = "I13";
            Assert.AreEqual("Water Quality", target.Purpose);
            target.PurposeCodingCode = "I14";
            Assert.AreEqual("Asset Record Control", target.Purpose);
            target.PurposeCodingCode = "I15";
            Assert.AreEqual("Demolition", target.Purpose);
            target.PurposeCodingCode = "I16";
            Assert.AreEqual("Locate", target.Purpose);
            target.PurposeCodingCode = "I17";
            Assert.AreEqual("Clean Out", target.Purpose);
            target.PurposeCodingCode = "";
            Assert.AreEqual(string.Empty, target.Purpose);
            target.PurposeCodingCode = null;
            Assert.AreEqual(string.Empty, target.Purpose);
        }
    }
}