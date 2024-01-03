using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.DesignPatterns;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    [TestClass]
    public class EquipmentTest : WorkOrdersTestClass<Equipment>
    {
        #region Constants

        public const int REFERENCE_EQUIPMENT_ID = 1112;

        #endregion

        #region Private Members

        protected override Equipment GetValidObjectFromDatabase()
        {
            return EquipmentRepository.GetEntity(REFERENCE_EQUIPMENT_ID);
        }

        protected override void DeleteObject(Equipment entity)
        {
            throw new NotImplementedException();
        }

        #endregion

        [TestMethod]
        public void TestToStringReturnsIdentifier()
        {
            const int equipmentId = 32;
            var target = new Equipment() { EquipmentID = equipmentId };
            target.EquipmentPurpose = new EquipmentPurpose() { EquipmentPurposeId= 1, Description= "test", Abbreviation = "FSAW"};
            target.Facility = new Facility() { RecordID = 1, OperatingCenter = new OperatingCenter() { OperatingCenterID = 1, OpCntr = "NJ7" } };
            Assert.AreEqual("NJ7-1-FSAW-32", target.ToString());          
        }

        [TestMethod]
        public void TestIAssetAssetIDReflecteEquipmentIdentifier()
        {
            const int equipmentId = 32;
            var target = new Equipment() { EquipmentID = equipmentId };
            target.EquipmentPurpose = new EquipmentPurpose() { EquipmentPurposeId = 1, Description = "test", Abbreviation = "FSAW" };
            target.Facility = new Facility() { RecordID = 1, OperatingCenter = new OperatingCenter() { OperatingCenterID = 1, OpCntr = "NJ7" } }; 
            Assert.AreEqual("NJ7-1-FSAW-32", target.AssetID);
        }

        [TestMethod]
        public void TestIAssetAssetKeyReflectedsEquipmentID()
        {
            const int equipmentId = 32;
            var target = new Equipment() { EquipmentID = equipmentId };

            Assert.AreEqual(equipmentId, target.AssetKey);
        }
    }

    internal class TestEquipmentBuilder : TestDataBuilder<Equipment>
    {
        #region Constants

        private struct DefaultValues
        {
            public const int EQUIPMENT_ID = 1;
        }

        #endregion

        #region Private Members

        private int? _equipmentID = DefaultValues.EQUIPMENT_ID;
        private string _assetID, _identifier;

        #endregion

        #region Exposed Methods

        public override Equipment Build()
        {
            var equipment = new Equipment();
            if (_equipmentID != null)
                equipment.EquipmentID = _equipmentID.Value;
            if (!String.IsNullOrWhiteSpace(_identifier))
                equipment.Identifier = _identifier;
            equipment.Facility = new Facility() { RecordID = 1, Coordinate = new Coordinate() { CoordinateID = 1, Latitude = 40, Longitude = -74 }};
            return equipment;
        }

        public TestEquipmentBuilder WithEquipmentID(int? equipmentID)
        {
            _equipmentID = equipmentID;
            return this;
        }

        public TestEquipmentBuilder WithIdentifier(string identifier)
        {
            _identifier = identifier;
            return this;
        }


        #endregion
    }

}