using System;
using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class GasMonitorTest : InMemoryDatabaseTest<GasMonitor>
    {
        #region Fields

        private GasMonitor _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _container.Inject<IDateTimeProvider>(new DateTimeProvider());
            _target = GetFactory<GasMonitorFactory>().Create();
        }

        #endregion

        #region Tests

        #region Logical properties

        [TestMethod]
        public void TestOperatingCenterReturnsEquipmentOperatingCenter()
        {
            var expected = new OperatingCenter();
            var equipment = new Equipment {OperatingCenter = expected};
            _target.Equipment = equipment;

            Assert.AreSame(expected, _target.OperatingCenter);
        }

        [TestMethod]
        public void TestDepartmentReturnsEquipmentFacilityDepartment()
        {
            var expected = new Department();
            var equipment = new Equipment {
                Facility = new Facility {
                    Department = expected
                }
            };
            _target.Equipment = equipment;

            Assert.AreSame(expected, _target.Department);
        }

        [TestMethod]
        public void TestEquipmentDescriptionReturnsEquipmentDescription()
        {
            var expected = "neato";
            var equipment = new Equipment {Description = expected};
            _target.Equipment = equipment;

            Assert.AreEqual(expected, _target.EquipmentDescription);
        }

        [TestMethod]
        public void TestEquipmentModelReturnsEquipmentModel()
        {
            var expected = new EquipmentModel();
            var equipment = new Equipment {EquipmentModel = expected};
            _target.Equipment = equipment;

            Assert.AreSame(expected, _target.EquipmentModel);
        }

        [TestMethod]
        public void TestManufacturerReturnsEquipmentsEquipmentManufacturer()
        {
            var expected = new EquipmentManufacturer();
            var equipment = new Equipment {
                EquipmentManufacturer = expected
            };
            _target.Equipment = equipment;

            Assert.AreSame(expected, _target.Manufacturer);
        }

        [TestMethod]
        public void TestManufacturerReturnsNullIfEquipmentDoesNotHaveAnEquipmentModel()
        {
            var equipment = new Equipment();
            equipment.EquipmentModel = null;
            _target.Equipment = equipment;
            Assert.IsNull(_target.Manufacturer);
        }

        [TestMethod]
        public void TestEquipmentPurposeReturnsEquipmentsEquipmentPurpose()
        {
            var expected = new EquipmentPurpose();
            var equipment = new Equipment {EquipmentPurpose = expected};
            _target.Equipment = equipment;

            Assert.AreSame(expected, _target.EquipmentPurpose);
        }

        [TestMethod]
        public void TestSerialNumberReturnsEquipmentSerialNumber()
        {
            var expected = "serial";
            var equipment = new Equipment {SerialNumber = expected};
            _target.Equipment = equipment;

            Assert.AreEqual(expected, _target.SerialNumber);
        }

        #endregion

        #region NextCalibrationDueDate

        [TestMethod]
        public void TestMostRecentPassingGasMonitorCalibrationReturnsNullIfThereAreNoCalibrations()
        {
            Assert.IsNull(_target.MostRecentPassingGasMonitorCalibration);
        }

        [TestMethod]
        public void NextCalibrationDueDateReturnsMostRecentCalibrationDateWithCalibrationFrequencyDaysAddedToItsValue()
        {
            var expected = new DateTime(1984, 4, 27);
            var _goodGoodGood_goodCalibration_aaaah = GetFactory<GasMonitorCalibrationFactory>()
               .Create(new {CalibrationDate = new DateTime(1984, 4, 24), GasMonitor = _target});
            var _badCalibration = GetEntityFactory<GasMonitorCalibration>()
               .Create(new {CalibrationDate = new DateTime(1983, 4, 23), GasMonitor = _target});
            Session.Evict(_target);

            _target = Session.Load<GasMonitor>(_target.Id);

            Assert.AreEqual(expected, _target.MostRecentPassingGasMonitorCalibration.NextDueDate);
        }

        #endregion

        #region OwnedBy

        [TestMethod]
        public void TestOwnedByReturnsUnknownWhenNoCharacteristicsExist()
        {
            var equipment = new Equipment {Characteristics = new List<EquipmentCharacteristic>(0)};
            var target = new GasMonitor {Equipment = equipment};

            Assert.AreEqual("Unknown", target.OwnedBy);
        }

        [TestMethod]
        public void TestOwnedByReturnsUnknownWhenNoCharacteristicOfOwnedByExist()
        {
            var equipment = new Equipment();
            var field = new EquipmentCharacteristicField {
                IsActive = true,
                FieldName = "SOMETHING",
                FieldType = new EquipmentCharacteristicFieldType()
            };
            equipment.Characteristics.Add(new EquipmentCharacteristic
                {Equipment = equipment, Field = field, Value = "some thing"});
            var target = new GasMonitor {Equipment = equipment};

            Assert.AreEqual("Unknown", target.OwnedBy);
        }

        [TestMethod]
        public void TestOwnedByReturnsOwnedByFromCharacteristics()
        {
            //Equipment?.ActiveCharacteristics?.FirstOrDefault(x => x.Field.FieldName == "OWNED_BY");
            var ownedBy = "Hawthorne Wipes";
            var equipment = new Equipment();
            var field = new EquipmentCharacteristicField {
                IsActive = true,
                FieldName = "OWNED_BY",
                FieldType = new EquipmentCharacteristicFieldType()
            };
            equipment.Characteristics.Add(new EquipmentCharacteristic
                {Equipment = equipment, Field = field, Value = ownedBy});
            var target = new GasMonitor {Equipment = equipment};

            Assert.AreEqual(ownedBy, target.OwnedBy);
        }

        #endregion

        #region ToString

        [TestMethod]
        public void TestDisplayReturnsSerialNumberOperatingCenterEquipmentDescriptionAndEquipmentStatus()
        {
            var expected = "123456789 - QQ1 - Q Town - Desc - Stat";
            var OperatingCenter = new OperatingCenter {
                OperatingCenterCode = "QQ1",
                OperatingCenterName = "Q Town"
            };
            var eq = new Equipment {
                OperatingCenter = OperatingCenter,
                SerialNumber = "123456789",
                Description = "Desc",
                EquipmentStatus = new EquipmentStatus {
                    Description = "Stat"
                }
            };
            _target.Equipment = eq;

            Assert.AreEqual(expected, _target.ToString());
        }

        [TestMethod]
        public void TestToStringReturnsDisplay()
        {
            Assert.AreEqual(_target.Display, _target.ToString());
        }

        [TestMethod]
        public void TestToStringReturnsNullIfEquipmentIsNull()
        {
            // This probably won't happen in practice since Equipment is required,
            // but anything is possible before the Equipment property gets set.
            _target.Equipment = null;
            Assert.IsNull(_target.ToString());
        }

        #endregion

        #endregion
    }
}
