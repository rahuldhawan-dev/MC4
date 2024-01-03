using System;
using System.Diagnostics;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallImporter.Library.ClassExtensions;
using MapCallImporter.Models.Import.Equipment;
using MapCallImporter.SampleValues;
using MapCallImporter.Validation;
using MapCallImporter.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;

namespace MapCallImporter.Library.Testing
{
    public abstract class EquipmentExcelRecordTestBase<TExcelRecord> : ExcelRecordTestBase<Equipment, MyCreateEquipment, TExcelRecord>
        where TExcelRecord : EquipmentExcelRecordBase<TExcelRecord>, new()
    {
        #region Abstract Properties

        protected abstract string ExpectedIdentifier { get; }

        #endregion

        #region Private Methods

        protected override void ImportTestData()
        {
            var throwaway = new TExcelRecord();

            TestDataHelper.CreateStuffForEquipmentInAberdeenNJ(_container, throwaway.GetHiddenPropertyValueByName("EquipmentType").ToString());
        }

        protected override TExcelRecord CreateTarget()
        {
            var target = new TExcelRecord {
                Equipment = 12345678,
                Description = "jerkhammer or something",
                FacilityMC = Facilities.NJSB10.FUNCTIONAL_LOCATION,
                Planningplant = "P218",
                Systemstatus = "INST",
                Userstatus = "INSV",
                Createdby = "mcadmin",
                FunctionalLoc = Facilities.NJSB10.FUNCTIONAL_LOCATION
            };

            string manufacturer = null;
            WithUnitOfWork(uow => {
                target.Manufacturer = uow
                    .Where<EquipmentManufacturer>(m =>
                        m.EquipmentType.Id == (int)target.GetHiddenPropertyValueByName("EquipmentTypeId"))
                    .First().Description;
            });

            return target;
        }

        protected virtual TExcelRecord CreateTargetWithFacility(string facilityFunctionalLocation)
        {
            var target = CreateTarget();

            target.FacilityMC = facilityFunctionalLocation;

            return target;
        }

        protected override void InnerTestMappings(ExcelRecordMappingTester<Equipment, MyCreateEquipment, TExcelRecord> test)
        {
            test.RequiredString(e => e.Description, e => e.Description);

            test.Int(e => e.Equipment, e => e.SAPEquipmentId.Value);
            test.String(e => e.ManufSerialNo, e => e.SerialNumber);
            test.String(e => e.FunctionalLoc, e => e.FunctionalLocation);
            test.DateTime(e => e.DateInstalled, e => e.DateInstalled);

            test.EntityLookup(e => e.EquipmentCondition, e => e.Condition, "Poor");
            test.EntityLookup(e => e.EquipmentPerformance, e => e.Performance, "Poor");
            test.EntityLookup(e => e.EquipmentConsequenceofFailure, e => e.ConsequenceOfFailure, "Low");
            test.EntityLookup(e => e.EquipmentStaticDynamicType, e => e.StaticDynamicType, "Dynamic");

            test.TestedElsewhere(x => x.Manufacturer);
            test.TestedElsewhere(e => e.Createdby);
            test.TestedElsewhere(e => e.FacilityMC);
            test.TestedElsewhere(e => e.Planningplant);
            test.TestedElsewhere(e => e.Userstatus);
            test.TestedElsewhere(e => e.Systemstatus);
            test.TestedElsewhere(e => e.ABCindic);
            test.TestedElsewhere(e => e.Modelnumber);

            WithCharacteristicMappingTester(test, t => {
                TestCharacteristicMappings(t);

                t.String(x => x.NARUCSpecialMtnNote, "NARUC_SPECIAL_MAINT_NOTES");
                t.String(x => x.NARUCSpecialMtnNoteDet, "NARUC_SPECIAL_MAINT_NOTE_DETAILS");
            });
        }

        #endregion

        #region Exposed Methods

        [DebuggerStepThrough]
        public void WithCharacteristicMappingTester(ExcelRecordMappingTester<Equipment, MyCreateEquipment, TExcelRecord> test,
            Action<EquipmentCharacteristicMappingTester<TExcelRecord>> fn)
        {
            fn(new EquipmentCharacteristicMappingTester<TExcelRecord>(this, test, _target));
        }

        #endregion

        #region Mappings

        protected abstract void TestCharacteristicMappings(EquipmentCharacteristicMappingTester<TExcelRecord> test);

        [TestMethod]
        public override void TestMappings()
        {
            base.TestMappings();
        }

        #endregion

        #region Equipment/SAPEquipmentId

        [TestMethod]
        public void TestSAPErrorCodeIsSetToRetryWhenEquipmentIsEmpty()
        {
            _target.Equipment = null;

            WithUnitOfWork(uow => {
                var result = _target.MapToEntity(uow, 1, _mappingHelper);

                Assert.IsTrue(result.SAPErrorCode.StartsWith("RETRY"));
                Assert.IsNull(result.SAPEquipmentId);
            });
        }

        [TestMethod]
        public void TestThrowsUniqueErrorWhenSAPEquipmentIdIsNotUnique()
        {
            _target.Equipment = 2517;

            WithUnitOfWork(uow => {
                uow.GetRepository<Equipment>().Insert(_target.MapToEntity(uow, 1, MappingHelper));
                uow.Commit();
            });
            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region EquipmentType/EqiupmentType

        [TestMethod]
        public void TestEquipmentTypeAndEquipmentPurposeMatchUp()
        {
            var equipmentType = (string)_target.GetHiddenPropertyValueByName("EquipmentType");
            var equipmentTypeId = (int)_target.GetHiddenPropertyValueByName("EquipmentTypeId");
            var equipmentPurposeId = (int)_target.GetHiddenPropertyValueByName("EquipmentPurposeId");

            WithUnitOfWork(uow => {
                var type = uow.Find<EquipmentType>(equipmentTypeId);

                Assert.IsNotNull(type, $"Could not find EquipmentType with id {equipmentTypeId}");
                Assert.AreEqual(equipmentType, type.Description);

                var mapCallPurpose = uow.Find<EquipmentPurpose>(equipmentPurposeId);

                Assert.AreEqual(equipmentTypeId, mapCallPurpose.EquipmentType.Id);
            });
        }

        [TestMethod]
        public void TestEquipmentTypeIsSetFromPropertyInClass()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(_target.GetHiddenPropertyValueByName("EquipmentTypeId"),
                    _target.MapToEntity(uow, 1, MappingHelper).EquipmentType.Id);
            });
        }

        [TestMethod]
        public void TestEquipmentPurposeIsSetFromPropertyInClass()
        {
            WithUnitOfWork(uow => {
                var result = _target.MapToEntity(uow, 1, MappingHelper).EquipmentPurpose.Id;

                Assert.AreEqual(_target.GetHiddenPropertyValueByName("EquipmentPurposeId"), result);
            });
        }

        #endregion

        #region Manufacturer/SAPEquipmentManufactuer

        [TestMethod]
        public void TestSAPEqiupmentManufacturerIsSetFromManufacturer()
        {
            WithUnitOfWork(uow => {
                var manufacturer = uow
                    .Where<EquipmentManufacturer>(m =>
                        m.EquipmentType.Id == (int)_target.GetHiddenPropertyValueByName("EquipmentTypeId"))
                    .First();

                _target.Manufacturer = manufacturer.Description;

                Assert.AreEqual(manufacturer, _target.MapToEntity(uow, 1, MappingHelper).EquipmentManufacturer);
            });
        }

        [TestMethod]
        public void TestThrowsWhenSAPEqiupmentManufacturerNotSet()
        {
            _target.Manufacturer = null;

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        [TestMethod]
        public void TestThrowsWhenEquipmentManufacturerNotFound()
        {
            _target.Manufacturer = "some guy in a shed";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        [TestMethod]
        public void TestUsesMapCallDescriptionToLookUpEquipmentManufacturerToPreventDuplicateClashes()
        {
            var typeId = (int)_target.GetHiddenPropertyValueByName("EquipmentTypeId");

            WithUnitOfWork(uow => {
                var manufacturer = uow
                                      .Where<EquipmentManufacturer>(m =>
                                           m.EquipmentType.Id == typeId)
                                      .First();

                _target.Manufacturer = manufacturer.Description;

                Assert.AreEqual(manufacturer, _target.MapToEntity(uow, 1, MappingHelper).EquipmentManufacturer);
            });
        }

        [TestMethod]
        public void TestThrowsWhenEquipmentManufacturerNotFoundForEquipmentType()
        {
            WithUnitOfWork(uow => {
                var manufacturer = new EquipmentManufacturer {
                    Description = "vOv",
                    EquipmentType = uow.Where<EquipmentType>(t =>
                        t.Id != (int)_target.GetHiddenPropertyValueByName("EquipmentTypeId")).First()
                };

                uow.Insert(manufacturer);

                _target.Manufacturer = manufacturer.Description;

                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region Modelnumber/EquipmentModel

        [TestMethod]
        public void TestModelnumberMapsToEquipmentModel()
        {
            WithUnitOfWork(uow => {
                var typeId = (int)_target.GetHiddenPropertyValueByName("EquipmentTypeId");
                var model = uow.Where<EquipmentModel>(m =>
                    m.EquipmentManufacturer.EquipmentType.Id == typeId).FirstOrDefault();

                if (model == null)
                {
                    Assert.Inconclusive($"No EquipmentModels found for EquipmentTypeId {typeId}");
                    return;
                }

                _target.Manufacturer = model.EquipmentManufacturer.Description;
                _target.Modelnumber = model.Description;

                Assert.AreEqual(model, _target.MapToEntity(uow, 1, MappingHelper).EquipmentModel);
            });
        }

        [TestMethod]
        public void TestEquipmentModelNotSetWhenModelnumberNotProvided()
        {
            WithUnitOfWork(uow => {
                foreach (var value in new[] {string.Empty, null})
                {
                    _target.Modelnumber = value;

                    Assert.IsNull(_target.MapToEntity(uow, 1, MappingHelper).EquipmentModel);
                }
            });
        }

        [TestMethod]
        public void TestCreatesNewEquipmentModelWhenModelnumberNotFound()
        {
            WithUnitOfWork(uow => {
                var typeId = (int)_target.GetHiddenPropertyValueByName("EquipmentTypeId");
                var manufacturer = uow.Where<EquipmentManufacturer>(m => m.EquipmentType.Id == typeId).First();

                _target.Modelnumber = "655321";
                _target.Manufacturer = manufacturer.Description;

                var newModel = _target.MapToEntity(uow, 1, MappingHelper).EquipmentModel;

                Assert.AreEqual(_target.Modelnumber, newModel.Description);
                Assert.AreEqual(manufacturer, newModel.EquipmentManufacturer);
            });
        }

        [TestMethod]
        public void TestDoesNotDuplicateEquipmentModelLikeADumbass()
        {
            TExcelRecord[] records = null;
            Equipment[] entities = null;
            WithUnitOfWork(uow => {
                var typeId = (int)_target.GetHiddenPropertyValueByName("EquipmentTypeId");
                var manufacturer = uow.Where<EquipmentManufacturer>(m => m.EquipmentType.Id == typeId).First();

                records = new[] {
                    _target, CreateTarget(), CreateTarget(), CreateTarget()
                };

                foreach (var record in records)
                {
                    record.Modelnumber = "655321";
                    record.Manufacturer = manufacturer.Description;
                }

                entities = records.Map(x => x.MapToEntity(uow, 1, MappingHelper)).ToArray();
            });

            Assert.AreSame(entities[0].EquipmentModel, entities[1].EquipmentModel);
            Assert.AreSame(entities[1].EquipmentModel, entities[2].EquipmentModel);
            Assert.AreSame(entities[2].EquipmentModel, entities[3].EquipmentModel);
        }

        #endregion

        #region ABCIndicator

        [TestMethod]
        public void TestABCindicMapsToABCIndicator()
        {
            WithUnitOfWork(uow => {
                var abcIndicator = uow.Where<ABCIndicator>(_ => true).First();

                _target.ABCindic = abcIndicator.Description;

                Assert.AreEqual(abcIndicator, _target.MapToEntity(uow, 1, MappingHelper).ABCIndicator);
            });
        }

        [TestMethod]
        public void TestThrowsWhenABCIndicatorNotFound()
        {
            _target.ABCindic = "ZYX";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        [TestMethod]
        public void TestABCIndicatorIsNotSetWhenABCindicNotProvided()
        {
            WithUnitOfWork(uow => {
                foreach (var value in new[] {string.Empty, null})
                {
                    _target.ABCindic = value;

                    Assert.IsNull(_target.MapToEntity(uow, 1, MappingHelper).ABCIndicator);
                }
            });
        }

        #endregion

        #region Identifier

        [TestMethod]
        public void TestGeneratesIdentifier()
        {
            WithUnitOfWork(uow => {
                var result = _target.MapToEntity(uow, 1, MappingHelper);
                // if we don't do this manually it'll be a 0
                result.Id = 1;

                Assert.AreEqual(ExpectedIdentifier, result.Identifier);
            });
        }

        #endregion

        #region PlanningPlant/OperatingCenter

        [TestMethod]
        public void TestThrowsWhenPlanningPlantNotFound()
        {
            _target.Planningplant = "this is no planning plant that i've ever heard of";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        [TestMethod]
        public void TestDoesNotThrowForOperatingCenterWhenPlanningPlantNotSet()
        {
            var mappingHelper = new ExcelRecordItemValidationHelper<Equipment, MyCreateEquipment, TExcelRecord>(
                _container.GetInstance<ExcelFileValidator<Equipment, MyCreateEquipment, TExcelRecord>>());
            _target.Planningplant = "this is no planning plant that i've ever heard of";

            WithUnitOfWork(uow => {
                MyAssert.DoesNotThrow(() =>
                    _target.MapToEntity(uow, 1, mappingHelper));
            });
        }

        [TestMethod]
        public void TestThrowsWhenPlanningPlantNotSet()
        {
            _target.Planningplant = null;

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region EquipmentStatus - SystemStatus/UserStatus

        [TestMethod]
        public void TestEquipmentStatusIsSetFromUserStatusAndSystemStatus()
        {
            WithUnitOfWork(uow => {
                foreach (var map in new[] {
                    (EquipmentStatus: "In Service", SystemStatus: "INST", UserStatus: "INSV"),
                    (EquipmentStatus: "Out of Service", SystemStatus: "INST", UserStatus: "OOS"),
                    (EquipmentStatus: "Field Installed", SystemStatus: "INST", UserStatus: "OOS TBIN"),
//                    (EquipmentStatus: "Retired", SystemStatus: "INAC INST", UserStatus: "REMV"),
                    (EquipmentStatus: "Cancelled", SystemStatus: "DLFL INAC INST", UserStatus: "OOS"),
                })
                {
                    _target.Userstatus = map.UserStatus;
                    _target.Systemstatus = map.SystemStatus;

                    var result = _target.MapToEntity(uow, 1, MappingHelper);

                    Assert.AreEqual(map.EquipmentStatus,
                        uow.Find<EquipmentStatus>(result.EquipmentStatus.Id).Description);
                }
            });
        }

        [TestMethod]
        public void TestDateRetiredIsSetToCurrentDateWhenEquipmentIsRetired()
        {
            WithUnitOfWork(uow => {
                _target.Userstatus = "REMV";
                _target.Systemstatus = "INAC INST";

                var result = _target.MapToEntity(uow, 1, MappingHelper);

                Assert.AreEqual(_now, result.DateRetired);
            });
        }

        [TestMethod]
        public void TestThrowsWhenUserStatusNotSet()
        {
            _target.Userstatus = null;

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        [TestMethod]
        public void TestThrowsWhenUserStatusNotMappable()
        {
            _target.Userstatus = "this isn't right at all";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        [TestMethod]
        public void TestThrowsWhenSystemStatusNotSet()
        {
            _target.Systemstatus = null;

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        [TestMethod]
        public void TestThrowsWhenSystemStatusNotMappable()
        {
            _target.Systemstatus = "this isn't right at all";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region CreatedBy

        [TestMethod]
        public void TestCreatedByIsSetToUserByUserName()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(_target.Createdby,
                    uow.Find<User>(_target.MapToEntity(uow, 1, MappingHelper).CreatedBy.Id).UserName);
            });
        }

        [TestMethod]
        public void TestThrowsWhenCreatedByNotSet()
        {
            _target.Createdby = null;

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        [TestMethod]
        public void TestThrowsWhenCreatedByNotMappable()
        {
            _target.Createdby = "i've never met this man in my life";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region Facility MC/Facility

        [TestMethod]
        public void TestFacilityIsSetFromFacilityMC()
        {
            WithUnitOfWork(uow => {
                var result = _target.MapToEntity(uow, 1, MappingHelper);

                Assert.IsNotNull(result.Facility);
                Assert.AreEqual(_target.FacilityMC, result.Facility.FunctionalLocation);
            });
        }

        [TestMethod]
        public void TestFacilityIsSetUsing3PartsOfFacilityMCStringWhere4PartsAreProvided()
        {
            _target.FacilityMC = _target.FacilityMC + "-FOO";

            WithUnitOfWork(uow => {
                var result = _target.MapToEntity(uow, 1, MappingHelper);

                Assert.IsNotNull(result.Facility);
                MyAssert.Contains(result.Facility.FunctionalLocation, _target.FacilityMC);
            });
        }

        [TestMethod]
        public void TestThrowsWhenFacilityNotFoundByFacilityMC()
        {
            _target.FacilityMC = "this is not a real functional location";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        [TestMethod]
        public void TestThrowsWhenFacilityMCNotSet()
        {
            WithUnitOfWork(uow => {
                foreach (var value in new [] {null, string.Empty, " "})
                {
                    _target.FacilityMC = value;

                    ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
                }
            });
        }

        [TestMethod]
        public void TestCoordinateIsSetFromFacility()
        {
            WithUnitOfWork(uow => {
                var result = _target.MapToEntity(uow, 1, MappingHelper);

                Assert.AreEqual(result.Coordinate, result.Facility.Coordinate);
            });

            
        }

        #endregion

        #region Functional Loc/FunctionalLocation

        [TestMethod]
        public void TestFunctionalLocationIsSetFromFunctionalLoc()
        {
            _target.FunctionalLoc = _target.FacilityMC;

            WithUnitOfWork(uow => {
                var result = _target.MapToEntity(uow, 1, MappingHelper);

                Assert.AreEqual(_target.FunctionalLoc, result.FunctionalLocation);
            });
        }

        #endregion
    }
}
