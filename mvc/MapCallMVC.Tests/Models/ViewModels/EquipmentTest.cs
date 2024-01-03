using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Migrations;
using MapCall.Common.Testing;
using MapCall.Common.Utility.Notifications;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Metadata;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class EquipmentTest : MapCallMvcInMemoryDatabaseTestBase<Equipment>
    {
        #region Fields

        private Equipment _entity;
        private EquipmentViewModel _target;
        private ViewModelTester<EquipmentViewModel, Equipment> _vmTester;
        private Mock<IAuthenticationService<User>> _authServ;
        private EquipmentType _eqType;
        private User _user;
        private Mock<INotificationService> _notifier;
        protected Mock<IDateTimeProvider> _dateTimeProvider;
        private CopyEquipment _viewModel;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IAuthenticationService<User>>().Use((_authServ = new Mock<IAuthenticationService<User>>()).Object);
            e.For<IEquipmentRepository>().Use<EquipmentRepository>();
            e.For<IDateTimeProvider>().Use((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);
            e.For<IEquipmentCharacteristicFieldRepository>().Use<EquipmentCharacteristicFieldRepository>();
            e.For<IFacilityRepository>().Use<FacilityRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IFacilityRepository>().Use<FacilityRepository>();
            e.For<INotificationService>().Use((_notifier = new Mock<INotificationService>()).Object);
            e.For<ISensorRepository>().Use<SensorRepository>();
        }

        protected override ITestDataFactoryService CreateFactoryService()
        {
            return _container.With(typeof(EquipmentPurposeFactory).Assembly).GetInstance<TestDataFactoryService>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _eqType = GetEntityFactory<EquipmentType>().Create();
            _entity = GetFactory<EquipmentFactory>().Create(new { EquipmentType = _eqType});
            _target = new EquipmentViewModel(_container);
            _target.FunctionalLocation = "test";
            _vmTester = new ViewModelTester<EquipmentViewModel, Equipment>(_target, _entity);
            GetFactory<ProductionPrerequisiteFactory>().CreateAll();

            _user = new User { UserName = "mcadmin", IsAdmin = true };
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            CreateTablesForBug1891.FIELD_TYPES.Each(o => GetEntityFactory<EquipmentCharacteristicFieldType>().Create(o));

            GetEntityFactory<EquipmentStatus>().Create(new {Description = "In Service"});
        }

        #endregion

        #region Mapping

        [TestMethod]
        public void TestMappings()
        {
            _vmTester.CanMapToViewModel(x => x.Id, 1);
            _vmTester.DoesNotMapToEntity(x => x.Id, 31);

            _vmTester.CanMapBothWays(x => x.Description);
            _vmTester.CanMapBothWays(x => x.DateInstalled);
            _vmTester.CanMapBothWays(x => x.SerialNumber);
            _vmTester.CanMapBothWays(x => x.PSMTCPA);
            _vmTester.CanMapBothWays(x => x.SafetyNotes);
            _vmTester.CanMapBothWays(x => x.MaintenanceNotes);
            _vmTester.CanMapBothWays(x => x.OperationNotes);
            _vmTester.CanMapBothWays(x => x.SAPEquipmentId);
            _vmTester.CanMapBothWays(x => x.ArcFlashHierarchy);
            _vmTester.CanMapBothWays(x => x.ArcFlashRating);
            _vmTester.CanMapBothWays(x => x.Legacy);

            _vmTester.CanMapBothWays(x => x.HasProcessSafetyManagement);
            _vmTester.CanMapBothWays(x => x.HasRegulatoryRequirement);
            _vmTester.CanMapBothWays(x => x.OtherCompliance);
            _vmTester.CanMapBothWays(x => x.HasOshaRequirement);
            _vmTester.CanMapBothWays(x => x.HasCompanyRequirement);
            _vmTester.CanMapBothWays(x => x.OtherComplianceReason);
        }

        [TestMethod]
        public void TestEquipmentViewModelMapSetsPropertiesAndIds()
        {
            var facility = GetFactory<FacilityFactory>().Create(new {FacilityId = "NJSB-99"});
            var facilityArea = GetEntityFactory<FacilityArea>().Create(new {Description = "test"});
            var facilitySubArea = GetEntityFactory<FacilitySubArea>().Create(new {
                Description = "SubareaDescription",
                Area = facilityArea
            });
            var equipmentPurpose = GetFactory<EquipmentPurposeFactory>().Create();
            var ffa = GetEntityFactory<FacilityFacilityArea>().Create(new {
                Facility = facility, FacilityArea = facilityArea, FacilitySubArea = facilitySubArea, Id = 1
            });
            var equipmentStatus = GetFactory<EquipmentStatusFactory>().Create();
            var equipmentManufacturer = GetFactory<EquipmentManufacturerFactory>().Create();
            var equipmentModel = GetFactory<EquipmentModelFactory>().Create();
            var functionalLocation = "Foo";
            var equipment = GetFactory<EquipmentFactory>().Create(new {
                EquipmentPurpose = equipmentPurpose,
                Facility = facility,
                FacilityFacilityArea = ffa,
                EquipmentStatus = equipmentStatus,
                EquipmentManufacturer = equipmentManufacturer,
                EquipmentModel = equipmentModel,
                Legacy = "My Leg",
                FunctionalLocation = functionalLocation
            });
            var target = new EquipmentViewModel(_container);
            target.Map(equipment);

            Assert.AreEqual(ffa.Id, target.FacilityFacilityArea);
            Assert.AreEqual(equipmentPurpose.Id, target.EquipmentPurpose);
            Assert.AreEqual(equipmentStatus.Id, target.EquipmentStatus);
            Assert.AreEqual(facility.Id, target.Facility);
            Assert.AreEqual(equipmentModel.Id, target.EquipmentModel);
            Assert.AreEqual(functionalLocation, target.FunctionalLocation);
            Assert.AreEqual("My Leg", target.Legacy);
        }

        [TestMethod]
        public void TestEquipmentMapToEntitySetsPreReqToTrueIfCertainSAPEqType()
        {
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var equipmentType = GetFactory<EquipmentTypeFactory>().Create();
            var prodPreq = GetFactory<HasLockoutRequirementProductionPrerequisiteFactory>().Create();
           
            _target.EquipmentType = equipmentType.Id;
            _target.OperatingCenter = operatingCenter.Id;
            Assert.IsFalse(_entity.ProductionPrerequisites.Any());
            _vmTester.MapToEntity();

            Assert.AreEqual(equipmentType.Id, _entity.EquipmentType.Id);
            Assert.IsTrue(_entity.ProductionPrerequisites.Count() == 1);
            Assert.AreEqual(1, _entity.ProductionPrerequisites[0].Id);
            Assert.AreEqual("Has Lockout Requirement", _entity.ProductionPrerequisites[0].Description);
        }

        [TestMethod]
        public void TestEquipmentMapToEntityAddsARedTagPermitPrerequisiteIfEquipmentTypeRequiresIt()
        {
            GetFactory<RedTagPermitProductionPrerequisiteFactory>().Create();

            var equipmentType = GetFactory<EquipmentTypeFireSuppressionFactory>().Create();
           
            _target.EquipmentType = equipmentType.Id;
            _vmTester.MapToEntity();

            Assert.AreEqual(1, _entity.ProductionPrerequisites.Count());
            Assert.AreEqual(equipmentType.Id, _entity.EquipmentType.Id);
            Assert.AreEqual(ProductionPrerequisite.Indices.RED_TAG_PERMIT, _entity.ProductionPrerequisites[0].Id);
            Assert.AreEqual("Red Tag Permit", _entity.ProductionPrerequisites[0].Description);
        }

        [TestMethod]
        public void TestEquipmentViewModelMapToEntitySetsProperties()
        {
            var equipmentPurpose = GetFactory<EquipmentPurposeFactory>().Create();
            var facility = GetFactory<FacilityFactory>().Create(new { FunctionalLocation = "Bar" });
            var equipmentStatus = GetFactory<EquipmentStatusFactory>().Create();
            var equipmentModel = GetFactory<EquipmentModelFactory>().Create();
            var functionalLocation = "Foo";

            var target = new EquipmentViewModel(_container) {
                EquipmentPurpose = equipmentPurpose.Id,
                Facility = facility.Id,
                EquipmentStatus = equipmentStatus.Id,
                EquipmentModel = equipmentModel.Id,
                FunctionalLocation = functionalLocation
            };
            var entity = new Equipment();

            target.MapToEntity(entity);
            
            Assert.AreEqual(equipmentPurpose.Id, entity.EquipmentPurpose.Id);
            Assert.AreEqual(facility.Id, entity.Facility.Id);
            Assert.AreEqual(equipmentStatus.Id, entity.EquipmentStatus.Id);
            Assert.AreEqual(equipmentModel.Id, entity.EquipmentModel.Id);
            Assert.AreEqual(facility.FunctionalLocation, entity.FunctionalLocation);
        }

        [TestMethod]
        public void TestEquipmentViewModelMapToEntitySetsCharacteristics()
        {
            var equipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            var equipmentPurpose = GetEntityFactory<EquipmentPurpose>().Create(new {
                EquipmentType = equipmentType
            });
            var equipment = GetEntityFactory<Equipment>().Create(new {
                EquipmentType = equipmentType,
                EquipmentPurpose = equipmentPurpose
            });
            var field = GetEntityFactory<EquipmentCharacteristicField>().Create(new {
                EquipmentType = equipmentType,
                FieldType = Session.Query<EquipmentCharacteristicFieldType>().Single(t => t.DataType == "Number"),
                FieldName = "SomeField"
            });
            
            Session.Flush();
            Session.Clear();

            equipment = Session.Load<Equipment>(equipment.Id);
            var model = _viewModelFactory.BuildWithOverrides<EditEquipment, Equipment>(equipment, new {
                Form = new FormCollection {
                    {field.FieldName, "1234"}
                }
            });
            equipment = model.MapToEntity(equipment);

            Assert.AreEqual("1234", equipment.Characteristics.Single(c => c.Field.FieldName == field.FieldName).Value);
        }

        [TestMethod]
        public void TestEquipmentViewModelMapToEntityDoesNotSetCharacteristicValueToDefaultSelectLabel()
        {
            var equipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            var equipmentPurpose = GetEntityFactory<EquipmentPurpose>().Create(new {
                EquipmentType = equipmentType
            });
            var equipment = GetEntityFactory<Equipment>().Create(new {
                EquipmentPurpose = equipmentPurpose
            });
            var field = GetEntityFactory<EquipmentCharacteristicField>().Create(new {
                EquipmentType = equipmentType,
                FieldType = Session.Query<EquipmentCharacteristicFieldType>().Single(t => t.DataType == "DropDown"),
                FieldName = "SomeField"
            });
            
            Session.Flush();
            Session.Clear();

            equipment = Session.Load<Equipment>(equipment.Id);
            var model = _viewModelFactory.BuildWithOverrides<EditEquipment, Equipment>(equipment, new {
                Form = new FormCollection {
                    {field.FieldName, SelectAttribute.DEFAULT_ITEM_LABEL}
                }
            });
            equipment = model.MapToEntity(equipment);
            
            Assert.AreEqual(0, equipment.Characteristics.Count);
        }

        [TestMethod]
        public void TestEquipmentViewModelMapToEntityDoesNotSetCharacteristicValueToEmptyString()
        {
            new[] {null, String.Empty, "\n"}.Each(s => {
                var equipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create();
                var equipmentPurpose = GetEntityFactory<EquipmentPurpose>().Create(new {
                    EquipmentType = equipmentType
                });
                var equipment = GetEntityFactory<Equipment>().Create(new {
                    EquipmentPurpose = equipmentPurpose
                });
                var field = GetEntityFactory<EquipmentCharacteristicField>().Create(new {
                    EquipmentType = equipmentType,
                    FieldType = Session.Query<EquipmentCharacteristicFieldType>().Single(t => t.DataType == "String"),
                    FieldName = "SomeField"
                });

                Session.Flush();
                Session.Clear();

                equipment = Session.Load<Equipment>(equipment.Id);
                var model = _viewModelFactory.BuildWithOverrides<EditEquipment, Equipment>(equipment, new {
                    Form = new FormCollection {
                        {field.FieldName, s}
                    }
                });
                equipment = model.MapToEntity(equipment);

                Assert.AreEqual(0, equipment.Characteristics.Count);
            });
        }

        [TestMethod]
        public void TestEquipmentViewModelMapToEntityDeletesCharacteristicForDropDownValueSetToDefaultSelectLabel()
        {
            var equipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            var equipmentPurpose = GetEntityFactory<EquipmentPurpose>().Create(new {
                EquipmentType = equipmentType
            });
            var equipment = GetEntityFactory<Equipment>().Create(new {
                EquipmentType = equipmentType,
                EquipmentPurpose = equipmentPurpose
            });
            var field = GetEntityFactory<EquipmentCharacteristicField>().Create(new {
                EquipmentType = equipmentType,
                FieldType = Session.Query<EquipmentCharacteristicFieldType>().Single(t => t.DataType == "DropDown"),
                FieldName = "SomeField"
            });
            var value = GetEntityFactory<EquipmentCharacteristicDropDownValue>().Create(new {
                Field = field,
                Value = "asdf"
            });

            Session.Flush();
            Session.Clear();

            equipment = Session.Load<Equipment>(equipment.Id);
            var model = _viewModelFactory.BuildWithOverrides<EditEquipment, Equipment>(equipment, new {
                Form = new FormCollection()
            });

            model.Characteristics.Add(GetEntityFactory<EquipmentCharacteristic>().Build(new {
                Equipment = equipment,
                Field = field,
                value.Value
            }));

            Session.SaveOrUpdate(model.MapToEntity(equipment));

            Session.Flush();
            Session.Clear();

            model = _viewModelFactory.BuildWithOverrides<EditEquipment, Equipment>(equipment, new {
                Form = new FormCollection {
                    {field.FieldName, SelectAttribute.DEFAULT_ITEM_LABEL}
                }
            });
            equipment = model.MapToEntity(equipment);

            Assert.AreEqual(0, equipment.Characteristics.Count);
        }

        [TestMethod]
        public void TestEquipmentViewModelMapToEntityDeletesCharacteristicForEmptyStringValue()
        {
            var equipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            var equipmentPurpose = GetEntityFactory<EquipmentPurpose>().Create(new {
                EquipmentType = equipmentType
            });
            var equipment = GetEntityFactory<Equipment>().Create(new {
                EquipmentType = equipmentType,
                EquipmentPurpose = equipmentPurpose
            });
            var field = GetEntityFactory<EquipmentCharacteristicField>().Create(new {
                EquipmentType = equipmentType,
                FieldType = Session.Query<EquipmentCharacteristicFieldType>().Single(t => t.DataType == "Number"),
                FieldName = "SomeField"
            });
            var value = GetEntityFactory<EquipmentCharacteristicDropDownValue>().Create(new {
                Field = field,
                Value = "1234"
            });

            Session.Flush();
            Session.Clear();

            equipment = Session.Load<Equipment>(equipment.Id);
            var model = _viewModelFactory.BuildWithOverrides<EditEquipment, Equipment>(equipment, new {
                Form = new FormCollection()
            });

            model.Characteristics.Add(GetEntityFactory<EquipmentCharacteristic>().Build(new {
                Equipment = equipment,
                Field = field,
                value.Value
            }));

            Session.SaveOrUpdate(model.MapToEntity(equipment));

            Session.Flush();
            Session.Clear();

            model = _viewModelFactory.BuildWithOverrides<EditEquipment, Equipment>(equipment, new {
                Form = new FormCollection {
                    {field.FieldName, String.Empty}
                }
            });
            equipment = model.MapToEntity(equipment);

            Assert.AreEqual(0, equipment.Characteristics.Count);
        }

        [TestMethod]
        public void TestMapToEntityNullsOutManufacturerOtherIfTheEquipmentManufacturerIsNotOTHER()
        {
            var sapEqManu = GetFactory<EquipmentManufacturerFactory>().Create(new
                { MapCallDescription = EquipmentManufacturer.MAPCALLDESCRIPTION_OTHER });

            // Test that ManufacturerOther does not get nulled out when the manufacturer is "OTHER"
            _target.EquipmentManufacturer = sapEqManu.Id;
            _target.ManufacturerOther = "Something";

            _vmTester.MapToEntity();

            Assert.AreEqual("Something", _entity.ManufacturerOther);

            // Test that it does tget nulled out when the manufacturer is not "OTHER"
            sapEqManu.MapCallDescription = "DEFINITELY NOT OTHER";
            _vmTester.MapToEntity();

            Assert.IsNull(_entity.ManufacturerOther);
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_target, x => x.Legacy, Equipment.StringLengths.LEGACY);
            ValidationAssert.PropertyHasMaxStringLength(_target, x => x.Description, Equipment.StringLengths.DESCRIPTION);
            ValidationAssert.PropertyHasMaxStringLength(_target, x => x.SerialNumber, Equipment.StringLengths.SERIAL_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(_target, x => x.CriticalNotes, Equipment.StringLengths.CRITICAL_NOTES);
            ValidationAssert.PropertyHasMaxStringLength(_target, x => x.FunctionalLocation, Equipment.StringLengths.FUNCTIONAL_LOCATION);
            ValidationAssert.PropertyHasMaxStringLength(_target, x => x.ArcFlashRating, Equipment.StringLengths.ARC_FLASH_RATING);
            ValidationAssert.PropertyHasMaxStringLength(_target, x => x.ManufacturerOther, Equipment.StringLengths.MANUFACTURER_OTHER);
            ValidationAssert.PropertyHasMaxStringLength(_target, x => x.WBSNumber, Equipment.StringLengths.WBS_NUMBER);
        }

        [TestMethod]
        public void TestDateInstalledIsNotRequiredBase()
        {
            _target.DateInstalled = null;

            ValidationAssert.ModelStateIsValid(_target, e => e.DateInstalled);
        }

        [TestMethod]
        public void TestEquipmentManufacturerIsNotRequiredBase()
        {
            _target.EquipmentManufacturer = null;

            ValidationAssert.ModelStateIsValid(_target, e => e.EquipmentManufacturer);
        }

        [TestMethod]
        public void TestDateInstalledIsRequiredForCreate()
        {
            var target = _viewModelFactory.Build<CreateEquipment>();

            ValidationAssert.PropertyIsRequired(target, e => e.DateInstalled);
        }

        [TestMethod]
        public void TestEquipmentManufacturerIsRequiredForCreate()
        {
            var target = _viewModelFactory.Build<CreateEquipment>();

            ValidationAssert.PropertyIsRequired(target, e => e.EquipmentManufacturer);
        }

        [TestMethod]
        public void TestModelIsValidWithMissingCharacteristicForFieldThatIsNotRequired()
        {
            var equipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            var equipmentPurpose = GetEntityFactory<EquipmentPurpose>().Create(new {
                EquipmentType = equipmentType
            });
            var facility = GetFactory<FacilityFactory>().Create();
            var equipment = GetEntityFactory<Equipment>().Create(new {
                EquipmentType = equipmentType,
                EquipmentPurpose = equipmentPurpose,
                RequestedBy = GetFactory<ActiveEmployeeFactory>().Create(),
                ABCIndicator = GetEntityFactory<ABCIndicator>().Create(),
                EquipmentStatus = GetEntityFactory<EquipmentStatus>().Create(),
                Facility = facility,
                facility.FunctionalLocation
            });
            GetEntityFactory<EquipmentCharacteristicField>().Create(new {
                EquipmentType = equipmentType,
                FieldType = Session.Query<EquipmentCharacteristicFieldType>().Single(t => t.DataType == "Number"),
                FieldName = "SomeField"
            });

            Session.Flush();
            Session.Clear();

            var model = _viewModelFactory.Build<EditEquipment, Equipment>(Session.Load<Equipment>(equipment.Id));

            ValidationAssert.WholeModelIsValid(model);
        }

        [TestMethod]
        public void TestModelIsNotValidWithMissingRequiredCharacteristics()
        {
            var equipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            var equipmentPurpose = GetEntityFactory<EquipmentPurpose>().Create(new {
                EquipmentType = equipmentType
            });
            var facility = GetFactory<FacilityFactory>().Create();
            var equipment = GetEntityFactory<Equipment>().Create(new {
                EquipmentType = equipmentType,
                EquipmentPurpose = equipmentPurpose,
                RequestedBy = GetFactory<ActiveEmployeeFactory>().Create(),
                ABCIndicator = GetEntityFactory<ABCIndicator>().Create(),
                EquipmentStatus = GetEntityFactory<EquipmentStatus>().Create(),
                Facility = facility,
                facility.FunctionalLocation
            });
            var field = GetEntityFactory<EquipmentCharacteristicField>().Create(new {
                EquipmentType = equipmentType,
                FieldType = Session.Query<EquipmentCharacteristicFieldType>().Single(t => t.DataType == "Number"),
                FieldName = "SomeField",
                Required = true
            });

            Session.Flush();
            Session.Clear();

            var model = _viewModelFactory.Build<EditEquipment, Equipment>(Session.Load<Equipment>(equipment.Id));

            ValidationAssert.SomethingAboutModelIsNotValid(model);

            model.Characteristics.Add(GetEntityFactory<EquipmentCharacteristic>().Build(new {
                Equipment = equipment,
                Field = field,
                Value = "123"
            }));

            ValidationAssert.WholeModelIsValid(model);
        }

        [TestMethod]
        public void TestModelIsNotValidWithInvalidCharacteristicValue()
        {
            var equipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            var equipmentPurpose = GetEntityFactory<EquipmentPurpose>().Create(new {
                EquipmentType = equipmentType
            });
            var facility = GetEntityFactory<Facility>().Create();
            var equipment = GetEntityFactory<Equipment>().Create(new {
                EquipmentType = equipmentType,
                EquipmentPurpose = equipmentPurpose,
                RequestedBy = GetFactory<ActiveEmployeeFactory>().Create(),
                ABCIndicator = GetEntityFactory<ABCIndicator>().Create(),
                EquipmentStatus = GetEntityFactory<EquipmentStatus>().Create(),
                Facility = facility,
                facility.FunctionalLocation
            });
            var field = GetEntityFactory<EquipmentCharacteristicField>().Create(new {
                EquipmentType = equipmentType,
                FieldType = Session.Query<EquipmentCharacteristicFieldType>().Single(t => t.DataType == "Number"),
                FieldName = "SomeField"
            });

            Session.Flush();
            Session.Clear();

            var model = _viewModelFactory.Build<EditEquipment, Equipment>(Session.Load<Equipment>(equipment.Id));

            model.Characteristics.Add(GetEntityFactory<EquipmentCharacteristic>().Build(new {
                Equipment = equipment,
                Field = field,
                Value = "abc"
            }));

            ValidationAssert.SomethingAboutModelIsNotValid(model);

            model.Characteristics.Last().Value = "123";

            ValidationAssert.WholeModelIsValid(model);
        }

        [TestMethod]
        public void TestCannotChangeEquipmentPurposeOnceCharacteristicsHaveBeenEntered()
        {
            var firstEquipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create(); 
            var firstEquipmentPurpose = GetEntityFactory<EquipmentPurpose>().Create(new {
                EquipmentType = firstEquipmentType
            });
            var secondEquipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            var secondEquipmentPurpose = GetEntityFactory<EquipmentPurpose>().Create(new {
                EquipmentType = secondEquipmentType
            });
            var equipment = GetEntityFactory<Equipment>().Create(new {
                EquipmentPurpose = firstEquipmentPurpose
            });
            var field = GetEntityFactory<EquipmentCharacteristicField>().Create(new {
                EquipmentType = firstEquipmentType,
                FieldType = Session.Query<EquipmentCharacteristicFieldType>().Single(t => t.DataType == "Number"),
                FieldName = "SomeField"
            });

            Session.Flush();
            Session.Clear();

            equipment = Session.Load<Equipment>(equipment.Id);
            var model = _viewModelFactory.BuildWithOverrides<EditEquipment, Equipment>(equipment, new {
                Form = new FormCollection()
            });

            model.Characteristics.Add(GetEntityFactory<EquipmentCharacteristic>().Build(new {
                Equipment = equipment,
                Field = field,
                Value = "1234"
            }));

            Session.SaveOrUpdate(model.MapToEntity(equipment));

            Session.Flush();
            Session.Clear();

            equipment = Session.Load<Equipment>(equipment.Id);
            model.Map(equipment);

            model.EquipmentPurpose = secondEquipmentPurpose.Id;

            ValidationAssert.SomethingAboutModelIsNotValid(model);
        }
        // Verify with dev team: this test is not valid anymore as Identifier is now logical field 

        [TestMethod]
        public void TestModelIsNotValidWhenActiveEquipmentExists()
        {
            var equipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            var requestedBy = GetFactory<ActiveEmployeeFactory>().Create();
            GetFactory<InServiceEquipmentStatusFactory>().Create();
            GetFactory<PendingEquipmentStatusFactory>().Create();
            var statuses = _container.GetInstance<IRepository<EquipmentStatus>>().GetAll();
            var facility = GetFactory<FacilityFactory>().Create();
            var equipment1 = GetEntityFactory<Equipment>().Create(new { Facility = facility, EquipmentStatus = statuses.FirstOrDefault(x => x.Id == EquipmentStatus.Indices.IN_SERVICE), RequestedBy = requestedBy, EquipmentType = equipmentType, facility.FunctionalLocation });
            var equipment2 = GetEntityFactory<Equipment>().Create(new { Facility = facility, EquipmentStatus = statuses.FirstOrDefault(x => x.Id == EquipmentStatus.Indices.IN_SERVICE), RequestedBy = requestedBy, EquipmentType = equipmentType, facility.FunctionalLocation });
            Session.Clear();
            Session.Flush();
            Assert.AreEqual(equipment1.EquipmentStatus.Id, EquipmentStatus.Indices.IN_SERVICE);
            Assert.AreEqual(equipment2.EquipmentStatus.Id, EquipmentStatus.Indices.IN_SERVICE);
            var model = _viewModelFactory.BuildWithOverrides<EditEquipment, Equipment>(equipment2, new {EquipmentStatus = EquipmentStatus.Indices.IN_SERVICE});

            model.EquipmentStatus = EquipmentStatus.Indices.PENDING;

            ValidationAssert.WholeModelIsValid(model);
        }

        [TestMethod]
        public void TestCreateRequiresManufacturerOtherWhenEquipmentManufacturerIsOther()
        {
            var manu = GetFactory<EquipmentManufacturerFactory>().Create(new { MapCallDescription = "UNKNOWN" });
            var otherManu = GetFactory<EquipmentManufacturerFactory>().Create(new{ MapCallDescription = "OTHER" });
            var equipmentType = GetEntityFactory<EquipmentType>().Create();

            var target = _viewModelFactory.Build<CreateEquipment, Equipment>(_entity);
            target.EquipmentManufacturer = manu.Id;
            target.EquipmentType = equipmentType.Id;
            target.ManufacturerOther = null;

            ValidationAssert.ModelStateIsValid(target, x => x.ManufacturerOther);

            target.EquipmentManufacturer = otherManu.Id;
            ValidationAssert.ModelStateHasError(target, x => x.ManufacturerOther, "Manufacturer Other is required.");

            target.ManufacturerOther = "Neat";
            ValidationAssert.ModelStateIsValid(target, x => x.ManufacturerOther);
        }

        #endregion

        #region SendToSAP

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPFalseWhenOperatingCenterSAPEnabledFalse()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = false });
            _entity.OperatingCenter = opc1;

            _vmTester.MapToEntity();

            Assert.IsFalse(_target.SendToSAP);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPTrueWhenOperatingCenterSAPEnabledTrue()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var departments = GetEntityFactory<Department>().CreateList(20);
            var facility = GetEntityFactory<Facility>().Create(new { Department = departments.FirstOrDefault(x => x.Id == Department.Indices.PRODUCTION)});
            _entity.OperatingCenter = opc1;
            _entity.Facility = facility;

            _vmTester.MapToEntity();

            Assert.IsTrue(_target.SendToSAP);
        }

        [TestMethod]
        public void TestMapToEntitySetsSAPEnabledToFalseWhenEquipmentTypeIsNotCorrect()
        {
            var equipmentTypes = GetFactory<EquipmentTypeGeneratorFactory>().CreateList(250);
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            _entity.OperatingCenter = opc1;
            var departments = GetEntityFactory<Department>().CreateList(20);
            var facility = GetEntityFactory<Facility>().Create(new { Department = departments.FirstOrDefault(x => x.Id == Department.Indices.PRODUCTION) });
            
            foreach (var equipmentType in equipmentTypes)
            {
                _entity.Facility = facility;
                _entity.EquipmentType = equipmentType;
                _vmTester.MapToEntity();

                if (EquipmentType.SyncronizedEquipmentTypes.Contains(equipmentType.Id))
                    Assert.IsTrue(_target.SendToSAP, equipmentType.Id.ToString());
                else
                    Assert.IsFalse(_target.SendToSAP, equipmentType.Id.ToString());
            }
        }

        [TestMethod]
        public void TestMapToEntitySetsSAPEnabledToFalseWhenFacilityIsNotProduction()
        {
            var equipmentTypes = GetFactory<EquipmentTypeGeneratorFactory>().CreateList(250);
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var departments = GetEntityFactory<Department>().CreateList(20);
            var facility = GetEntityFactory<Facility>().Create();
            _entity.OperatingCenter = opc1;

            foreach (var department in departments)
            {
                facility.Department = department;
                _entity.Facility = facility;
                _vmTester.MapToEntity();

                if (Department.SAP_DEPARTMENTS.Contains(department.Id))
                    Assert.IsTrue(_target.SendToSAP);
                else
                    Assert.IsFalse(_target.SendToSAP);
            }
        }
        [TestMethod]
        public void TestCopyEquipment()
        {
            var entity = GetEntityFactory<Equipment>().Create(new {
                HasProcessSafetyManagement = true,
                HasCompanyRequirement = true,
                HasRegulatoryRequirement = true,
                HasOshaRequirement = true,
                OtherCompliance = true,
                OtherComplianceReason = "test"
            });
            _viewModel = _viewModelFactory.Build<CopyEquipment, Equipment>(entity);

            _viewModel.Map(entity);

            Assert.IsFalse(_viewModel.HasProcessSafetyManagement);
            Assert.IsFalse(_viewModel.HasCompanyRequirement);
            Assert.IsFalse(_viewModel.HasRegulatoryRequirement);
            Assert.IsFalse(_viewModel.HasOshaRequirement);
            Assert.IsFalse(_viewModel.OtherCompliance);
            Assert.IsNull(_viewModel.OtherComplianceReason);
        }
        #endregion

        #region Defaults

        [TestMethod]
        public void TestCreateSetsEquipmentCriticalityToHigh()
        {
            var abcIndicators = GetEntityFactory<ABCIndicator>().CreateList(3);
            var target = new CreateEquipment(_container);

            target.SetDefaults();

            Assert.AreEqual(ABCIndicator.Indices.HIGH, target.ABCIndicator);
        }

        #endregion

        #region Risk Characteristics

        [TestMethod]
        public void TestMapToEntityLikelyhoodOfFailureStaticCalculatesCorrectly()
        {
            var facility = GetFactory<FacilityFactory>().Create(new { FacilityId = "NJSB-99" });
            var equipment = GetFactory<EquipmentFactory>().Build(new { Facility = facility});
            var target = _viewModelFactory.BuildWithOverrides<CreateEquipment, Equipment>(equipment, new {
                Facility = facility.Id,
                StaticDynamicType = EquipmentStaticDynamicType.Indices.STATIC
            });

            //Condition Poor
            target.Condition = EquipmentCondition.Indices.POOR;
            //Performance Good => High
            target.Performance = EquipmentPerformanceRating.Indices.GOOD;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentLikelyhoodOfFailureRating.Indices.HIGH, equipment.LikelyhoodOfFailure.Id);
            //Performance Average => High
            target.Performance = EquipmentPerformanceRating.Indices.AVERAGE;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentLikelyhoodOfFailureRating.Indices.HIGH, equipment.LikelyhoodOfFailure.Id);
            //Performance Poor => High
            target.Performance = EquipmentPerformanceRating.Indices.AVERAGE;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentLikelyhoodOfFailureRating.Indices.HIGH, equipment.LikelyhoodOfFailure.Id);

            //Condition Average
            target.Condition = EquipmentCondition.Indices.AVERAGE;
            //Performance Good => Low
            target.Performance = EquipmentPerformanceRating.Indices.GOOD;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentLikelyhoodOfFailureRating.Indices.MEDIUM, equipment.LikelyhoodOfFailure.Id);
            //Performance Average => Medium
            target.Performance = EquipmentPerformanceRating.Indices.AVERAGE;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentLikelyhoodOfFailureRating.Indices.MEDIUM, equipment.LikelyhoodOfFailure.Id);
            //Performance Poor => High
            target.Performance = EquipmentPerformanceRating.Indices.POOR;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentLikelyhoodOfFailureRating.Indices.HIGH, equipment.LikelyhoodOfFailure.Id);

            //Condition Good
            target.Condition = EquipmentCondition.Indices.GOOD;
            //Performance Good => Low
            target.Performance = EquipmentPerformanceRating.Indices.GOOD;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentLikelyhoodOfFailureRating.Indices.LOW, equipment.LikelyhoodOfFailure.Id);
            //Performance Average => Low
            target.Performance = EquipmentPerformanceRating.Indices.AVERAGE;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentLikelyhoodOfFailureRating.Indices.LOW, equipment.LikelyhoodOfFailure.Id);
            //Performance Poor => Medium
            target.Performance = EquipmentPerformanceRating.Indices.POOR;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentLikelyhoodOfFailureRating.Indices.MEDIUM, equipment.LikelyhoodOfFailure.Id);
        }

        [TestMethod]
        public void TestMapToEntityLikelyhoodOfFailureDynamicCalculatesCorrectly()
        {
            var facility = GetFactory<FacilityFactory>().Create(new { FacilityId = "NJSB-99" });
            var equipment = GetFactory<EquipmentFactory>().Build(new { Facility = facility });
            var target = _viewModelFactory.BuildWithOverrides<CreateEquipment, Equipment>(equipment, new {
                Facility = facility.Id,
                StaticDynamicType = EquipmentStaticDynamicType.Indices.DYNAMIC
            });

            //Condition Poor
            target.Condition = EquipmentCondition.Indices.POOR;
            //Performance Good => High
            target.Performance = EquipmentPerformanceRating.Indices.GOOD;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentLikelyhoodOfFailureRating.Indices.MEDIUM, equipment.LikelyhoodOfFailure.Id);
            //Performance Average => High
            target.Performance = EquipmentPerformanceRating.Indices.AVERAGE;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentLikelyhoodOfFailureRating.Indices.HIGH, equipment.LikelyhoodOfFailure.Id);
            //Performance Poor => High
            target.Performance = EquipmentPerformanceRating.Indices.AVERAGE;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentLikelyhoodOfFailureRating.Indices.HIGH, equipment.LikelyhoodOfFailure.Id);

            //Condition Average
            target.Condition = EquipmentCondition.Indices.AVERAGE;
            //Performance Good => Low
            target.Performance = EquipmentPerformanceRating.Indices.GOOD;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentLikelyhoodOfFailureRating.Indices.LOW, equipment.LikelyhoodOfFailure.Id);
            //Performance Average => Medium
            target.Performance = EquipmentPerformanceRating.Indices.AVERAGE;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentLikelyhoodOfFailureRating.Indices.MEDIUM, equipment.LikelyhoodOfFailure.Id);
            //Performance Poor => High
            target.Performance = EquipmentPerformanceRating.Indices.POOR;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentLikelyhoodOfFailureRating.Indices.HIGH, equipment.LikelyhoodOfFailure.Id);

            //Condition Good
            target.Condition = EquipmentCondition.Indices.GOOD;
            //Performance Good => Low
            target.Performance = EquipmentPerformanceRating.Indices.GOOD;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentLikelyhoodOfFailureRating.Indices.LOW, equipment.LikelyhoodOfFailure.Id);
            //Performance Average => Low
            target.Performance = EquipmentPerformanceRating.Indices.AVERAGE;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentLikelyhoodOfFailureRating.Indices.LOW, equipment.LikelyhoodOfFailure.Id);
            //Performance Poor => Medium
            target.Performance = EquipmentPerformanceRating.Indices.POOR;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentLikelyhoodOfFailureRating.Indices.MEDIUM, equipment.LikelyhoodOfFailure.Id);
        }

        [TestMethod]
        public void TestMapToEntityEquipmentReliabilityStaticCalculatesCorrectly()
        {
            var facility = GetFactory<FacilityFactory>().Create(new { FacilityId = "NJSB-99" });
            var equipment = GetFactory<EquipmentFactory>().Build(new { Facility = facility });
            var target = _viewModelFactory.BuildWithOverrides<CreateEquipment, Equipment>(equipment, new {
                Facility = facility.Id,
                StaticDynamicType = EquipmentStaticDynamicType.Indices.STATIC
            });

            //Condition Poor
            target.Condition = EquipmentCondition.Indices.POOR;
            //Performance Good => High
            target.Performance = EquipmentPerformanceRating.Indices.GOOD;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentReliabilityRating.Indices.LOW, equipment.Reliability.Id);
            //Performance Average => High
            target.Performance = EquipmentPerformanceRating.Indices.AVERAGE;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentReliabilityRating.Indices.LOW, equipment.Reliability.Id);
            //Performance Poor => High
            target.Performance = EquipmentPerformanceRating.Indices.AVERAGE;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentReliabilityRating.Indices.LOW, equipment.Reliability.Id);

            //Condition Average
            target.Condition = EquipmentCondition.Indices.AVERAGE;
            //Performance Good => Low
            target.Performance = EquipmentPerformanceRating.Indices.GOOD;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentReliabilityRating.Indices.MEDIUM, equipment.Reliability.Id);
            //Performance Average => Medium
            target.Performance = EquipmentPerformanceRating.Indices.AVERAGE;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentReliabilityRating.Indices.MEDIUM, equipment.Reliability.Id);
            //Performance Poor => High
            target.Performance = EquipmentPerformanceRating.Indices.POOR;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentReliabilityRating.Indices.LOW, equipment.Reliability.Id);

            //Condition Good
            target.Condition = EquipmentCondition.Indices.GOOD;
            //Performance Good => Low
            target.Performance = EquipmentPerformanceRating.Indices.GOOD;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentReliabilityRating.Indices.HIGH, equipment.Reliability.Id);
            //Performance Average => Low
            target.Performance = EquipmentPerformanceRating.Indices.AVERAGE;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentReliabilityRating.Indices.HIGH, equipment.Reliability.Id);
            //Performance Poor => Medium
            target.Performance = EquipmentPerformanceRating.Indices.POOR;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentReliabilityRating.Indices.MEDIUM, equipment.Reliability.Id);
        }

        [TestMethod]
        public void TestMapToEntityEquipmentReliabilityDynamicCalculatesCorrectly()
        {
            var facility = GetFactory<FacilityFactory>().Create(new { FacilityId = "NJSB-99" });
            var equipment = GetFactory<EquipmentFactory>().Build(new { Facility = facility });
            var target = _viewModelFactory.BuildWithOverrides<CreateEquipment, Equipment>(equipment, new {
                Facility = facility.Id,
                StaticDynamicType = EquipmentStaticDynamicType.Indices.DYNAMIC
            });

            //Condition Poor
            target.Condition = EquipmentCondition.Indices.POOR;
            //Performance Good => High
            target.Performance = EquipmentPerformanceRating.Indices.GOOD;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentReliabilityRating.Indices.MEDIUM, equipment.Reliability.Id);
            //Performance Average => High
            target.Performance = EquipmentPerformanceRating.Indices.AVERAGE;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentReliabilityRating.Indices.LOW, equipment.Reliability.Id);
            //Performance Poor => High
            target.Performance = EquipmentPerformanceRating.Indices.AVERAGE;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentReliabilityRating.Indices.LOW, equipment.Reliability.Id);

            //Condition Average
            target.Condition = EquipmentCondition.Indices.AVERAGE;
            //Performance Good => Low
            target.Performance = EquipmentPerformanceRating.Indices.GOOD;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentReliabilityRating.Indices.HIGH, equipment.Reliability.Id);
            //Performance Average => Medium
            target.Performance = EquipmentPerformanceRating.Indices.AVERAGE;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentReliabilityRating.Indices.MEDIUM, equipment.Reliability.Id);
            //Performance Poor => High
            target.Performance = EquipmentPerformanceRating.Indices.POOR;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentReliabilityRating.Indices.LOW, equipment.Reliability.Id);

            //Condition Good
            target.Condition = EquipmentCondition.Indices.GOOD;
            //Performance Good => Low
            target.Performance = EquipmentPerformanceRating.Indices.GOOD;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentReliabilityRating.Indices.HIGH, equipment.Reliability.Id);
            //Performance Average => Low
            target.Performance = EquipmentPerformanceRating.Indices.AVERAGE;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentReliabilityRating.Indices.HIGH, equipment.Reliability.Id);
            //Performance Poor => Medium
            target.Performance = EquipmentPerformanceRating.Indices.POOR;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentReliabilityRating.Indices.MEDIUM, equipment.Reliability.Id);
        }

        [TestMethod]
        public void TestMapToEntityLocalizedRiskOfFailureScoreCalculates()
        {
            var facility = GetFactory<FacilityFactory>().Create(new { FacilityId = "NJSB-99" });
            var equipment = GetFactory<EquipmentFactory>().Build(new { Facility = facility });
            var target = _viewModelFactory.BuildWithOverrides<CreateEquipment, Equipment>(equipment, new {
                Facility = facility.Id,
                StaticDynamicType = EquipmentStaticDynamicType.Indices.STATIC
            });

            //COF HIGH / LOF LOW => 3
            //Performance Average => Low
            target.Condition = EquipmentCondition.Indices.GOOD;
            target.Performance = EquipmentPerformanceRating.Indices.GOOD;
            target.ConsequenceOfFailure = EquipmentConsequencesOfFailureRating.Indices.HIGH;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentLikelyhoodOfFailureRating.Indices.LOW, equipment.LikelyhoodOfFailure.Id);
            Assert.AreEqual(3, equipment.LocalizedRiskOfFailure.Value);

            //COF HIGH / LOF MEDIUM => 6
            target.Condition = EquipmentCondition.Indices.AVERAGE;
            target.Performance = EquipmentPerformanceRating.Indices.GOOD;
            target.ConsequenceOfFailure = EquipmentConsequencesOfFailureRating.Indices.HIGH;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentLikelyhoodOfFailureRating.Indices.MEDIUM, equipment.LikelyhoodOfFailure.Id);
            Assert.AreEqual(6, equipment.LocalizedRiskOfFailure.Value);

            //COF HIGH / LOF HIGH => 9 
            target.Condition = EquipmentCondition.Indices.POOR;
            target.Performance = EquipmentPerformanceRating.Indices.GOOD;
            target.ConsequenceOfFailure = EquipmentConsequencesOfFailureRating.Indices.HIGH;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentLikelyhoodOfFailureRating.Indices.HIGH, equipment.LikelyhoodOfFailure.Id);
            Assert.AreEqual(9, equipment.LocalizedRiskOfFailure.Value);

            //COF MEDIUM / LOF LOW => 2
            target.Condition = EquipmentCondition.Indices.GOOD;
            target.Performance = EquipmentPerformanceRating.Indices.GOOD;
            target.ConsequenceOfFailure = EquipmentConsequencesOfFailureRating.Indices.MEDIUM;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentLikelyhoodOfFailureRating.Indices.LOW, equipment.LikelyhoodOfFailure.Id);
            Assert.AreEqual(2, equipment.LocalizedRiskOfFailure.Value);

            //COF MEDIUM / LOF MEDIUM => 4
            target.Condition = EquipmentCondition.Indices.AVERAGE;
            target.Performance = EquipmentPerformanceRating.Indices.GOOD;
            target.ConsequenceOfFailure = EquipmentConsequencesOfFailureRating.Indices.MEDIUM;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentLikelyhoodOfFailureRating.Indices.MEDIUM, equipment.LikelyhoodOfFailure.Id);
            Assert.AreEqual(4, equipment.LocalizedRiskOfFailure.Value);

            //COF MEDIUM / LOF HIGH => 6
            target.Condition = EquipmentCondition.Indices.POOR;
            target.Performance = EquipmentPerformanceRating.Indices.GOOD;
            target.ConsequenceOfFailure = EquipmentConsequencesOfFailureRating.Indices.MEDIUM;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentLikelyhoodOfFailureRating.Indices.HIGH, equipment.LikelyhoodOfFailure.Id);
            Assert.AreEqual(6, equipment.LocalizedRiskOfFailure.Value);

            //COF LOW / LOF LOW => 1
            target.Condition = EquipmentCondition.Indices.GOOD;
            target.Performance = EquipmentPerformanceRating.Indices.GOOD;
            target.ConsequenceOfFailure = EquipmentConsequencesOfFailureRating.Indices.LOW;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentLikelyhoodOfFailureRating.Indices.LOW, equipment.LikelyhoodOfFailure.Id);
            Assert.AreEqual(1, equipment.LocalizedRiskOfFailure.Value);

            //COF LOW / LOF MEDIUM => 2
            target.Condition = EquipmentCondition.Indices.AVERAGE;
            target.Performance = EquipmentPerformanceRating.Indices.GOOD;
            target.ConsequenceOfFailure = EquipmentConsequencesOfFailureRating.Indices.LOW;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentLikelyhoodOfFailureRating.Indices.MEDIUM, equipment.LikelyhoodOfFailure.Id);
            Assert.AreEqual(2, equipment.LocalizedRiskOfFailure.Value);
            
            //COF LOW / LOF HIGH => 3
            target.Condition = EquipmentCondition.Indices.POOR;
            target.Performance = EquipmentPerformanceRating.Indices.GOOD;
            target.ConsequenceOfFailure = EquipmentConsequencesOfFailureRating.Indices.LOW;
            equipment = target.MapToEntity(equipment);
            Assert.AreEqual(EquipmentLikelyhoodOfFailureRating.Indices.HIGH, equipment.LikelyhoodOfFailure.Id);
            Assert.AreEqual(3, equipment.LocalizedRiskOfFailure.Value);
        }

        [TestMethod]
        public void TestMapToEntityCalculatesRiskOfFailure()
        {
            var facility = GetFactory<FacilityFactory>().Create(new { FacilityId = "NJSB-99" });
            var facilityRepo = _container.GetInstance<RepositoryBase<Facility>>();
            var tier1 = GetFactory<Tier1FacilityAssetManagementMaintenanceStrategyTierFactory>().Create();
            var tier2 = GetFactory<Tier2FacilityAssetManagementMaintenanceStrategyTierFactory>().Create();
            var tier3 = GetFactory<Tier3FacilityAssetManagementMaintenanceStrategyTierFactory>().Create();
            var lowCOF = GetFactory<LowEquipmentConsequencesOfFailureRatingFactory>().Create();
            var mediumCOF = GetFactory<MediumEquipmentConsequencesOfFailureRatingFactory>().Create();
            var highCOF = GetFactory<HighEquipmentConsequencesOfFailureRatingFactory>().Create();
            var lowLOF = GetFactory<LowEquipmentLikelyhoodOfFailureRatingFactory>().Create();
            var mediumLOF = GetFactory<MediumEquipmentLikelyhoodOfFailureRatingFactory>().Create();
            var highLOF = GetFactory<HighEquipmentLikelyhoodOfFailureRatingFactory>().Create();
            var lowRisk = GetFactory<LowEquipmentFailureRiskRatingFactory>().Create();
            var mediumRisk = GetFactory<MediumEquipmentFailureRiskRatingFactory>().Create();
            var highRisk = GetFactory<HighEquipmentFailureRiskRatingFactory>().Create();

            var truthTable = new (FacilityAssetManagementMaintenanceStrategyTier Tier, EquipmentConsequencesOfFailureRating Consequence, EquipmentLikelyhoodOfFailureRating Likelihood, EquipmentFailureRiskRating ExpectedRisk)[] {
                (tier1, lowCOF, lowLOF, lowRisk),
                (tier1, lowCOF, mediumLOF, mediumRisk),
                (tier1, lowCOF, highLOF, mediumRisk),
                (tier1, mediumCOF, lowLOF, mediumRisk),
                (tier1, mediumCOF, mediumLOF, highRisk),
                (tier1, mediumCOF, highLOF, highRisk),
                (tier1, highCOF, lowLOF, mediumRisk),
                (tier1, highCOF, mediumLOF, highRisk),
                (tier1, highCOF, highLOF, highRisk),

                (tier2, lowCOF, lowLOF, lowRisk),
                (tier2, lowCOF, mediumLOF, lowRisk),
                (tier2, lowCOF, highLOF, mediumRisk),
                (tier2, mediumCOF, lowLOF, lowRisk),
                (tier2, mediumCOF, mediumLOF, mediumRisk),
                (tier2, mediumCOF, highLOF, mediumRisk),
                (tier2, highCOF, lowLOF, mediumRisk),
                (tier2, highCOF, mediumLOF, mediumRisk),
                (tier2, highCOF, highLOF, highRisk),

                (tier3, lowCOF, lowLOF, lowRisk),
                (tier3, lowCOF, mediumLOF, lowRisk),
                (tier3, lowCOF, highLOF, lowRisk),
                (tier3, mediumCOF, lowLOF, lowRisk),
                (tier3, mediumCOF, mediumLOF, lowRisk),
                (tier3, mediumCOF, highLOF, lowRisk),
                (tier3, highCOF, lowLOF, lowRisk),
                (tier3, highCOF, mediumLOF, lowRisk),
                (tier3, highCOF, highLOF, mediumRisk),
            };

            foreach (var tup in truthTable)
            {
                facility.StrategyTier = tup.Tier;
                facilityRepo.Save(facility);

                var target = _viewModelFactory.BuildWithOverrides<CreateEquipment>(new {
                    Facility = facility.Id,
                    ConsequenceOfFailure = tup.Consequence.Id,
                    LikelyhoodOfFailure = tup.Likelihood.Id
                });

                var actualRisk = target.MapToEntity(new Equipment()).RiskOfFailure;

                Assert.IsNotNull(actualRisk);
                Assert.AreEqual(tup.ExpectedRisk.Id, actualRisk.Id,
                    $"{tup.Tier.Description} - {tup.Consequence.Description} Consequence - {tup.Likelihood.Description} Likelihood should be {tup.ExpectedRisk.Description} Risk.  Got {actualRisk.Description} instead.");
            }
        }

        [TestMethod]
        public void TestSetLastUpdatedIfRiskCharacteristicsHaveBeenModified()
        {
            var now = DateTime.Now;
            _dateTimeProvider
               .Setup(dt => dt.GetCurrentDate())
               .Returns(now);
            
            var facility = GetFactory<FacilityFactory>().Create(new { FacilityId = "NJSB-99" });
            var equipment = GetFactory<EquipmentFactory>().Build(new { Facility = facility });
            var target = _viewModelFactory.BuildWithOverrides<CreateEquipment, Equipment>(equipment, new {
                Facility = facility.Id,
                StaticDynamicType = EquipmentStaticDynamicType.Indices.STATIC
            });
            
            target.Condition = EquipmentCondition.Indices.POOR;
            target.Performance = EquipmentPerformanceRating.Indices.GOOD;
            target.ConsequenceOfFailure = EquipmentConsequencesOfFailureRating.Indices.LOW;
            
            Assert.IsNull(equipment.RiskCharacteristicsLastUpdatedBy);
            
            equipment = target.MapToEntity(equipment);
            
            var riskCharacteristicsLastUpdated = 
                equipment.RiskCharacteristicsLastUpdatedOn ?? DateTime.MinValue;
            
            Assert.AreEqual(equipment.RiskCharacteristicsLastUpdatedBy, _user);
            Assert.AreEqual(riskCharacteristicsLastUpdated, now);
        }

        [TestMethod]
        public void TestMapToEntityMapsFacilityFunctionalLocationToEquipmentFunctionalLocation()
        {
            var facility = GetFactory<FacilityFactory>().Create(new { FunctionalLocation = "Bar" });
            var functionalLocation = "Foo";

            var target = new EquipmentViewModel(_container) {
                Facility = facility.Id,
                FunctionalLocation = functionalLocation
            };
            var entity = new Equipment();

            target.MapToEntity(entity);

            Assert.AreEqual(facility.FunctionalLocation, entity.FunctionalLocation);
        }

        #endregion
    }
}
