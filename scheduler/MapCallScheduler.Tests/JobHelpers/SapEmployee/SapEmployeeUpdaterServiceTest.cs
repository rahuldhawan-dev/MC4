using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallScheduler.JobHelpers.SapEmployee;
using MapCallScheduler.Tests.Library.JobHelpers.Sap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;
using NHibernate.Linq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.SapEmployee
{
    [TestClass]
    public class SapEmployeeUpdaterServiceTest : SapEntityUpdaterServiceTestBase<SapEmployeeFileRecord, ISapEmployeeFileParser, Employee, IRepository<Employee>, SapEmployeeUpdaterService>
    {
        private State _nj;
        private OperatingCenter _opc;
        private PersonnelArea _pa, _pa2;
        private SapEmployeeFileRecord _targetSapRecord;

        private EmployeeStatus _activeEmployeeStatus,
                               _inactiveEmployeeStatus,
                               _withdrawnEmployeeStatus,
                               _retireeEmployeeStatus;

        private UserType _internalUserType;

        private PositionGroup _positionGroupWithState;
        private SAPCompanyCode _sapCompanyCode;

        private CommercialDriversLicenseProgramStatus _notInProgramStatus;

        private Mock<INotificationService> _notificationService;

        #region Setup/Teardown

        protected override void CreateTestData()
        {
            InitPersonnelAreas();
            InitEmployeeStatuses();
            InitUserTypes();
            InitPositionGroups();
            InitCDLRepo();
            _sapCompanyCode = GetEntityFactory<SAPCompanyCode>().Create(new { Description = "PG Company Code" });
            _targetSapRecord = CreateBasicSapEmployeeFileRecord();
        }

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);

            i.For<IEmployeeStatusRepository>().Use<EmployeeStatusRepository>();
            i.For<IPersonnelAreaRepository>().Use<PersonnelAreaRepository>();
            i.For<IUserRepository>().Use<UserRepository>();
            i.For<IUserTypeRepository>().Use<UserTypeRepository>();
            i.For<IPositionGroupRepository>().Use<PositionGroupRepository>();
            i.For<ICommercialDriversLicenseProgramStatusRepository>()
             .Use<CommercialDriversLicenseProgramStatusRepository>();
            i.For<IDateTimeProvider>().Mock();
            _notificationService = i.For<INotificationService>().Mock();
        }

        private void InitCDLRepo()
        {
            _notInProgramStatus = GetFactory<NotInProgramCommercialDriversLicenseProgramStatusFactory>().Create();
        }

        private void InitPersonnelAreas()
        {
            _nj = GetEntityFactory<State>().Create(new {Abbreviation = "NJ"});
            _opc = GetEntityFactory<OperatingCenter>().Create(new {State = _nj});
            _pa = GetEntityFactory<PersonnelArea>().Create(new {PersonnelAreaId = 1});
            _pa2 = GetEntityFactory<PersonnelArea>().Create(new {PersonnelAreaId = 2, OperatingCenter = _opc});

            _pa.OperatingCenter = null;
            Session.Save(_pa);
        }

        private void InitEmployeeStatuses()
        {
            _activeEmployeeStatus = GetFactory<ActiveEmployeeStatusFactory>().Create();
            _inactiveEmployeeStatus = GetFactory<InactiveEmployeeStatusFactory>().Create();
            _withdrawnEmployeeStatus = GetFactory<WithdrawnEmployeeStatusFactory>().Create();
            _retireeEmployeeStatus = GetFactory<RetireeEmployeeStatusFactory>().Create();
        }

        private void InitUserTypes()
        {
            _internalUserType = GetFactory<InternalUserTypeFactory>().Create();
        }

        private void InitPositionGroups()
        {
            var companyCode = GetEntityFactory<SAPCompanyCode>().Create(new {Description = "PG Company Code"});

            _positionGroupWithState = GetEntityFactory<PositionGroup>().Create(new {
                BusinessUnit = "PGBU",
                BusinessUnitDescription = "PGBU Description",
                SAPCompanyCode = companyCode,
                Group = "PG Group",
                PositionDescription = "PG Position Description",
                State = _nj,
                SAPPositionGroupKey = "Key1"
            });
        }

        private SapEmployeeFileRecord CreateBasicSapEmployeeFileRecord()
        {
            // This should be a record that will import correctly by default.
            return new SapEmployeeFileRecord {
                EmployeeId = "12345",
                PersonnelAreaId = 1,
                Status = "Active",
                PositionGroupKey = "Key1",
                PositionGroupCompanyCode = "PG Company Code"
            };
        }

        #endregion

        [TestMethod]
        public void TestProcessCreatesEmployeeIfItDoesNotExist()
        {
            var file = SetupFileAndRecords(_targetSapRecord);

            MyAssert.CausesIncrease(() => _target.Process(file), () => Repository.GetAll().Count());

            var entity = Repository.GetAll().ToList().Last();

            Assert.AreEqual("12345", entity.EmployeeId);
            Assert.AreEqual("Active", entity.Status.Description);
        }

        [TestMethod]
        public void TestProcessUpdatesEmployeeIfItDoesExist()
        {
            var file = SetupFileAndRecords(_targetSapRecord);

            var existingEmployee = GetEntityFactory<Employee>()
               .Create(new {EmployeeId = "12345", Status = _inactiveEmployeeStatus});

            MyAssert.DoesNotCauseIncrease(() => _target.Process(file), () => Repository.GetAll().Count());

            Assert.AreEqual(_activeEmployeeStatus, Repository.Find(existingEmployee.Id).Status);
        }

        [TestMethod]
        public void TestProcessThrowsIfMoreThanOneNEWEmployeeHaveTheSameEmployeeId()
        {
            var file = SetupFileAndRecords(_targetSapRecord, _targetSapRecord);

            MyAssert.ThrowsWithMessage<ArgumentException>(() => _target.Process(file), "More than one new employee has the Employee ID '12345'. Employee ID must be unique.");
        }

        [TestMethod]
        public void TestMappingOfBasicFieldsThatAreEasyToMap()
        {
            _targetSapRecord.EmployeeId = "12345";
            _targetSapRecord.PersonnelAreaId = 1;
            _targetSapRecord.Status = "Active";
            _targetSapRecord.FirstName = "Dude";
            _targetSapRecord.LastName = "McDuderson";
            _targetSapRecord.DateHired = DateTime.Today;
            _targetSapRecord.City = "Bradley Beach";
            _targetSapRecord.State = "NJ";
            _targetSapRecord.EmailAddress = "dude@mcduderson.com";
            _targetSapRecord.PhoneHome = "1234567890";
            _targetSapRecord.Address = "123 Fake St";
            _targetSapRecord.Address2 = "Apt 9";
            _targetSapRecord.PhoneCellular = "999-999-9999";
            var file = SetupFileAndRecords(_targetSapRecord);

            _target.Process(file);

            var entity = Repository.GetAll().ToList().Last();

            Assert.AreEqual(_targetSapRecord.EmployeeId, entity.EmployeeId);
            Assert.AreEqual(_targetSapRecord.FirstName, entity.FirstName);
            Assert.AreEqual(_targetSapRecord.LastName, entity.LastName);
            Assert.AreEqual(_targetSapRecord.DateHired, entity.DateHired);
            Assert.AreEqual(_targetSapRecord.City, entity.City);
            Assert.AreEqual(_targetSapRecord.State, entity.State);
            Assert.AreEqual(_targetSapRecord.EmailAddress, entity.EmailAddress);
            Assert.AreEqual(_targetSapRecord.PhoneHome, entity.PhoneHome);
            Assert.AreEqual(_targetSapRecord.PhoneCellular, entity.PhoneCellular);
            Assert.AreEqual(_targetSapRecord.Address, entity.Address);
            Assert.AreEqual(_targetSapRecord.Address2, entity.Address2);
        }

        [TestMethod]
        public void TestPhoneCellularIsNotOverWrittenWhenItIsNull()
        {
            var existingEmployee = GetEntityFactory<Employee>().Create(new {
                EmployeeId = "12345", 
                Status = _inactiveEmployeeStatus,
                PhoneCellular = "555-EXPECT-ME"
            });
            _targetSapRecord.EmployeeId = "12345";
            _targetSapRecord.PhoneCellular = null;
            var file = SetupFileAndRecords(_targetSapRecord);

            _target.Process(file);

            var entity = Repository.GetAll().ToList().Last();

           
            Assert.AreEqual("555-EXPECT-ME", entity.PhoneCellular);
        }

        [TestMethod]
        public void TestMappingZipCodesAddsPaddedZerosBackBecauseSAPLikesToRemoveTheInitialZeroOnZipCodesThatStartWithZero()
        {
            _targetSapRecord.ZipCode = "7720"; // SAP likes to send these without the beginning zero for some reason.
            var file = SetupFileAndRecords(_targetSapRecord);

            _target.Process(file);

            var entity = Repository.GetAll().ToList().Last();

            Assert.AreEqual("07720", entity.ZipCode);
        }

        [TestMethod]
        public void TestMappingZipCodeMapsNonNumericalZipCodes()
        {
            _targetSapRecord.ZipCode = "WESTINDIES"; // SAP likes to send these as if they're valid.
            var file = SetupFileAndRecords(_targetSapRecord);

            _target.Process(file);

            var entity = Repository.GetAll().ToList().Last();

            Assert.AreEqual("WESTINDIES", entity.ZipCode);
        }

        #region Mapping CDL Program Status


        [TestMethod]
        public void TestEmployeeWithoutCDLStatusGetsDefaultedToNotInProgramStatus()
        {
            GetEntityFactory<Employee>().Create(new {EmployeeId = "12345", Status = _activeEmployeeStatus});

            var file = SetupFileAndRecords(_targetSapRecord);

            _target.Process(file);

            var entity = Repository.GetAll().ToList().Last();

            Assert.AreEqual(_notInProgramStatus, entity.CommercialDriversLicenseProgramStatus);
        }

        [TestMethod]
        public void TestEmployeeWithCDLStatusDoesNotHaveStatusChanged()
        {
            var someOtherStatus = GetFactory<NotInProgramCommercialDriversLicenseProgramStatusFactory>().Create();
             GetEntityFactory<Employee>().Create(new {
                EmployeeId = "12345",
                Status = _activeEmployeeStatus,
                CommercialDriversLicenseProgramStatus = someOtherStatus
            });
            var file = SetupFileAndRecords(_targetSapRecord);

            _target.Process(file);

            var entity = Repository.GetAll().ToList().Last();

            Assert.AreEqual(someOtherStatus, entity.CommercialDriversLicenseProgramStatus);
        }

        #endregion

        #region Mapping EmployeeStatus

        [TestMethod]
        public void TestEmployeeStatusIsMappedCorrectlyForActiveStatus()
        {
            var file = SetupFileAndRecords(_targetSapRecord);
            GetEntityFactory<Employee>().Create(new {EmployeeId = "12345", Status = _inactiveEmployeeStatus});

            _target.Process(file);

            var entity = Repository.GetAll().ToList().Last();

            Assert.AreEqual(_activeEmployeeStatus, entity.Status);
        }

        [TestMethod]
        public void TestEmployeeStatusIsMappedCorrectlyForInactiveStatus()
        {
            _targetSapRecord.Status = "Inactive";
            var file = SetupFileAndRecords(_targetSapRecord);
            GetEntityFactory<Employee>().Create(new {EmployeeId = "12345", Status = _activeEmployeeStatus});

            _target.Process(file);

            var entity = Repository.GetAll().ToList().Last();

            Assert.AreEqual(_inactiveEmployeeStatus, entity.Status);
        }

        [TestMethod]
        public void TestMappingEmployeeStatusThrowsIfEmployeeStatusDoesNotExist()
        {
            _targetSapRecord.Status = "An invalid status";
            var file = SetupFileAndRecords(_targetSapRecord);

            MyAssert.ThrowsWithMessage<ArgumentException>(() => _target.Process(file), "Unable to find an employee status with the description 'An invalid status'.");
        }

        #endregion

        #region Mapping Position Groups

        [TestMethod]
        public void TestMappingPositionGroupIsMappedByPositionGroupKey()
        {
            _targetSapRecord.PositionGroupKey = _positionGroupWithState.SAPPositionGroupKey;
            var file = SetupFileAndRecords(_targetSapRecord);
            _target.Process(file);
            var entity = Repository.GetAll().ToList().Last();
            Assert.AreSame(_positionGroupWithState, entity.PositionGroup);
        }

        [TestMethod]
        public void TestMappingPositionGroupCreatesNewPositionGroupIfOneDoesNotExistForThePositionGroupKey()
        {
            _targetSapRecord.PositionGroupState = _nj.Abbreviation;
            _targetSapRecord.PositionGroupCompanyCode = _sapCompanyCode.Description;
            _targetSapRecord.PositionGroupKey = "99999";
            _targetSapRecord.PositionGroupBusinessUnit = "Some business unit";
            _targetSapRecord.PositionGroupBusinessUnitDescription = "Some business unit description";
            _targetSapRecord.PositionGroupGroup = "Some group";

            var file = SetupFileAndRecords(_targetSapRecord);
            _target.Process(file);

            // Ensure this got saved correctly.
            var resultPg = Repository.GetAll().ToList().Last().PositionGroup;
            Assert.AreSame(_nj, resultPg.State);
            Assert.AreSame(_sapCompanyCode, resultPg.SAPCompanyCode);
            Assert.AreEqual(_targetSapRecord.PositionGroupKey, resultPg.SAPPositionGroupKey);
            Assert.AreEqual(_targetSapRecord.PositionGroupBusinessUnit, resultPg.BusinessUnit);
            Assert.AreEqual(_targetSapRecord.PositionGroupBusinessUnitDescription, resultPg.BusinessUnitDescription);
            Assert.AreEqual(_targetSapRecord.PositionGroupGroup, resultPg.Group);
        }

        [TestMethod]
        public void TestMappingPositionGroupDoesNotSetEmployeePositionGroupToNullIfPositionGroupCodeIsNullInImportedRecord()
        {
            var expectedPositionGroup = GetEntityFactory<PositionGroup>().Create();
            var expectedEmployee = GetEntityFactory<Employee>().Create(new { EmployeeId = "123456", PositionGroup = expectedPositionGroup });

            // Basically, we don't want to overwrite Employee.PositionGroup when we try to create a new 
            // PositionGroup but fail to for whatever reason. In this case, we won't create a new PositionGroup
            // record when the PositionGroupGroup is null/empty.
            _targetSapRecord.EmployeeId = "123456";
            _targetSapRecord.PositionGroupState = _nj.Abbreviation;
            _targetSapRecord.PositionGroupCompanyCode = _sapCompanyCode.Description;
            _targetSapRecord.PositionGroupKey = "99999";
            _targetSapRecord.PositionGroupBusinessUnit = "Some business unit";
            _targetSapRecord.PositionGroupBusinessUnitDescription = "Some business unit description";
            _targetSapRecord.PositionGroupGroup = null;

            var file = SetupFileAndRecords(_targetSapRecord);

            _target.Process(file);

            // Ensure this got saved correctly.
            Assert.AreSame(expectedPositionGroup, expectedEmployee.PositionGroup);
        }

        [TestMethod]
        public void TestMappingPositionGroupDoesNotCreateDuplicateNewPositionGroupsBasedOnThePositionGroupKey()
        {
            _targetSapRecord.PositionGroupState = _nj.Abbreviation;
            _targetSapRecord.PositionGroupCompanyCode = _sapCompanyCode.Description;
            _targetSapRecord.PositionGroupKey = "99999";
            _targetSapRecord.PositionGroupBusinessUnit = "Some business unit";
            _targetSapRecord.PositionGroupBusinessUnitDescription = "Some business unit description";
            _targetSapRecord.PositionGroupGroup = "Some group";

            var otherRecord = CreateBasicSapEmployeeFileRecord();
            otherRecord.EmployeeId = "3413413";
            otherRecord.PositionGroupState = _nj.Abbreviation;
            otherRecord.PositionGroupCompanyCode = _sapCompanyCode.Description;
            otherRecord.PositionGroupKey = "99999";
            otherRecord.PositionGroupBusinessUnit = "Some business unit";
            otherRecord.PositionGroupBusinessUnitDescription = "Some business unit description";
            otherRecord.PositionGroupGroup = "Some group";

            var file = SetupFileAndRecords(_targetSapRecord, otherRecord);
            _target.Process(file);

            // There should only be one that is created, so this will throw if there's multiple.
            var result = Session.Query<PositionGroup>().SingleOrDefault(x => x.SAPPositionGroupKey == "99999");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestMappingPositionGroupDoesNotOverwriteEmployeePositionGroupWhenPositionGroupKeyIsNullOrEmptyOrWhiteSpace()
        {
            var entity = GetEntityFactory<Employee>().Create(new { EmployeeId = "12345", PositionGroup = _positionGroupWithState });
            Action<string> doTest = (pgKey) => {
                _target._newEmployeeIds.Clear();
                _targetSapRecord.PositionGroupKey = null;
                var file = SetupFileAndRecords(_targetSapRecord);
                _target.Process(file);
                Assert.AreSame(_positionGroupWithState, entity.PositionGroup);
            };

            doTest(null);
            doTest(string.Empty);
            doTest("   ");
        }

        [TestMethod]
        public void TestMappingPositionGroupSetsPositionGroupToNullWhenThePositionGroupKeyIs99999999()
        {
            // 99999999 is a weird special case value sent from SAP when employees are retired/withdrawn.
            // We don't hold this value in MapCall because there is no other information for it and it 
            // does not work with our schema.
            var entity = GetEntityFactory<Employee>().Create(new { EmployeeId = "12345", PositionGroup = _positionGroupWithState });
            _targetSapRecord.PositionGroupKey = "99999999";
            var file = SetupFileAndRecords(_targetSapRecord);
            _target.Process(file);
            Assert.IsNull(entity.PositionGroup);
        }

        [TestMethod]
        public void TestMappingPositionGroupThrowsErrorWhenCreatingANewPositionGroupAndOneOfTheRequiredFieldsIsNullOrEmptyOrWhiteSpace()
        {
            void DoTest(Action<SapEmployeeFileRecord> recordSetupFn)
            {
                _target._newEmployeeIds.Clear();

                // setup the target with all required values for creating a new position group 
                _targetSapRecord.PositionGroupKey = "Some key that does not exist";
                _targetSapRecord.PositionGroupBusinessUnit = "Some business unit";
                _targetSapRecord.PositionGroupBusinessUnitDescription = "Some business unit description";
                _targetSapRecord.PositionGroupGroup = "Some group";
                _targetSapRecord.PositionGroupCompanyCode = "Some company code";
                _targetSapRecord.PositionGroupState = _nj.Abbreviation;

                recordSetupFn(_targetSapRecord);

                var file = SetupFileAndRecords(_targetSapRecord);
                MyAssert.ThrowsWithMessage<ArgumentException>(() => { _target.Process(file); }, "Invalid data from SAP for employee id 12345. One of the many position group fields needed to create a new PositionGroup is missing a value.");
            }

            foreach (var badVal in new[] { null, string.Empty, "   " })
            {
                DoTest(x => x.PositionGroupCompanyCode = badVal);
                DoTest(x => x.PositionGroupBusinessUnit = badVal);
                DoTest(x => x.PositionGroupBusinessUnitDescription = badVal);
                DoTest(x => x.PositionGroupState = badVal);
            }
        }

        [TestMethod]
        public void TestMappingPositionGroupThrowsErrorWhenCreatingANewPositionGroupAndAMatchingStateCanNotBeFound()
        {
            _targetSapRecord.PositionGroupKey = "Some key that does not exist";
            _targetSapRecord.PositionGroupBusinessUnit = "Some business unit";
            _targetSapRecord.PositionGroupBusinessUnitDescription = "Some business unit description";
            _targetSapRecord.PositionGroupGroup = "Some group";
            _targetSapRecord.PositionGroupCompanyCode = _sapCompanyCode.Description;

            _targetSapRecord.PositionGroupState = "ZZ";
            var file = SetupFileAndRecords(_targetSapRecord);
            MyAssert.ThrowsWithMessage<KeyNotFoundException>(() => { _target.Process(file); }, "Unable to find state with abbreviation 'ZZ'");
        }

        [TestMethod]
        public void TestMappingPositionGroupThrowsErrorWhenCreatingANewPositionGroupAndAMatchingSAPCompanyCodeCanNotBeFound()
        {
            _targetSapRecord.PositionGroupKey = "Some key that does not exist";
            _targetSapRecord.PositionGroupBusinessUnit = "Some business unit";
            _targetSapRecord.PositionGroupBusinessUnitDescription = "Some business unit description";
            _targetSapRecord.PositionGroupGroup = "Some group";
            _targetSapRecord.PositionGroupCompanyCode = "I don't exist";
            _targetSapRecord.PositionGroupState = _nj.Abbreviation;

            var file = SetupFileAndRecords(_targetSapRecord);
            MyAssert.ThrowsWithMessage<KeyNotFoundException>(() => { _target.Process(file); }, "Unable to find SAPCompanyCode with description 'I don't exist'");
        }

        [TestMethod]
        public void TestProcessSendsNotificationForNewPositionGroups()
        {
            NotifierArgs result = null;
            _notificationService.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback((NotifierArgs args) =>
            {
                result = args;
            });
            _targetSapRecord.PositionGroupState = _nj.Abbreviation;
            _targetSapRecord.PositionGroupCompanyCode = _sapCompanyCode.Description;
            _targetSapRecord.PositionGroupKey = "99999";
            _targetSapRecord.PositionGroupBusinessUnit = "Some business unit";
            _targetSapRecord.PositionGroupBusinessUnitDescription = "Some business unit description";
            _targetSapRecord.PositionGroupGroup = "Some group";

            var file = SetupFileAndRecords(_targetSapRecord);
            _target.Process(file);

            // Ensure this got saved correctly.
            var resultPg = Repository.GetAll().ToList().Last().PositionGroup;
            Assert.AreEqual("Position Groups Created", result.Subject);
            Assert.AreEqual("Position Groups Created", result.Purpose);
            Assert.AreEqual(RoleModules.HumanResourcesPositions, result.Module);

            var data = (IEnumerable<PositionGroup>)result.Data;
            Assert.IsTrue(data.Contains(resultPg));
        }

        [TestMethod]
        public void TestMappingPositionGroupWillSetPositionGroupCommonNameOnNewlyCreatedPositionGroupIfAnExistingPositionGroupHasMatchingData()
        {
            var expectedCommonName = GetEntityFactory<PositionGroupCommonName>().Create();
            var mostlyIdenticalPositionGroup = GetEntityFactory<PositionGroup>().Create(new { CommonName = expectedCommonName, SAPCompanyCode = _sapCompanyCode });

            // Reinitialize the target as the constructor for it caches data and we just created some new data to cache.
            _target = _container.GetInstance<SapEmployeeUpdaterService>();

            _targetSapRecord.PositionGroupState = mostlyIdenticalPositionGroup.State.Abbreviation;
            _targetSapRecord.PositionGroupCompanyCode = mostlyIdenticalPositionGroup.SAPCompanyCode.Description;
            _targetSapRecord.PositionGroupBusinessUnit = mostlyIdenticalPositionGroup.BusinessUnit;
            _targetSapRecord.PositionGroupBusinessUnitDescription = mostlyIdenticalPositionGroup.BusinessUnitDescription;
            _targetSapRecord.PositionGroupGroup = mostlyIdenticalPositionGroup.Group;
            _targetSapRecord.PositionGroupPositionDescription = mostlyIdenticalPositionGroup.PositionDescription;
            _targetSapRecord.PositionGroupKey = "XXXX";

            var file = SetupFileAndRecords(_targetSapRecord);
            _target.Process(file);

            var resultPg = Repository.GetAll().ToList().Last().PositionGroup;
            Assert.AreEqual("XXXX", resultPg.SAPPositionGroupKey);
            Assert.AreSame(expectedCommonName, resultPg.CommonName);
        }

        #endregion

        #region Mapping PersonnelArea

        [TestMethod]
        public void TestProcessSetsPersonnelAreaIfMatchingPersonnelAreaExists()
        {
            var file = SetupFileAndRecords(_targetSapRecord);
            GetEntityFactory<Employee>().Create(new { EmployeeId = "12345", Status = _activeEmployeeStatus, PersonnelArea = _pa2 });

            _target.Process(file);

            var entity = Repository.GetAll().ToList().Last();

            Assert.AreSame(_pa, entity.PersonnelArea, "PersonnelArea should have been changed");
        }

        [TestMethod]
        public void TestProcessDoesNotOverwriteExistingPersonnelAreaIfThereIsNoMatchingPersonnelArea()
        {
            _targetSapRecord.PersonnelAreaId = -242;
            var file = SetupFileAndRecords(_targetSapRecord);
            GetEntityFactory<Employee>().Create(new { EmployeeId = "12345", Status = _activeEmployeeStatus, PersonnelArea = _pa2 });

            _target.Process(file);

            var entity = Repository.GetAll().ToList().Last();

            Assert.AreSame(_pa2, entity.PersonnelArea, "PersonnelArea should not have been changed");
        }

        #endregion

        #region Mapping OperatingCenter

        [TestMethod]
        public void TestProcessSetsOperatingCenterFromPersonnelArea()
        {
            var file = SetupFileAndRecords(_targetSapRecord);
            GetEntityFactory<Employee>().Create(new {EmployeeId = "12345", Status = _activeEmployeeStatus});

            _target.Process(file);

            var entity = Repository.GetAll().ToList().Last();

            Assert.AreEqual(_opc, entity.OperatingCenter);
        }

        [TestMethod]
        public void TestProcessDoesNotSetOperatingCenterOnNewEmployeeIfPersonnelAreaCanNotBeFound()
        {
            _targetSapRecord.PersonnelAreaId = 1000;
            var file = SetupFileAndRecords(_targetSapRecord);

            _target.Process(file);

            var entity = Repository.GetAll().ToList().Last();

            Assert.IsNull(entity.OperatingCenter);
        }

        [TestMethod]
        public void TestProcessDoesNotSetOperatingCenterOnExistingEmployeeIfPersonnelAreaCanNotBeFound()
        {
            _targetSapRecord.PersonnelAreaId = 1000;
            var file = SetupFileAndRecords(_targetSapRecord);
            GetEntityFactory<Employee>().Create(new {EmployeeId = "12345", Status = _activeEmployeeStatus, OperatingCenter = _opc});

            _target.Process(file);

            var entity = Repository.GetAll().ToList().Last();

            Assert.AreEqual(_opc, entity.OperatingCenter);
        }

        [TestMethod]
        public void TestProcessDoesNotSetOperatingCenterOnNewEmployeeIfPersonnelAreaDoesNotHaveOperatingCenter()
        {
            _pa.OperatingCenter = null;
            var file = SetupFileAndRecords(_targetSapRecord);

            _target.Process(file);

            var entity = Repository.GetAll().ToList().Last();

            Assert.IsNull(entity.OperatingCenter);
        }

        [TestMethod]
        public void TestProcessDoesNotSetOperatingCenterOnExistingEmployeeIfPersonnelAreaDoesNotHaveOperatingCenter()
        {
            _pa.OperatingCenter = null;
            var file = SetupFileAndRecords(_targetSapRecord);
            GetEntityFactory<Employee>().Create(new {EmployeeId = "12345", Status = _activeEmployeeStatus, OperatingCenter = _opc});

            _target.Process(file);

            var entity = Repository.GetAll().ToList().Last();

            Assert.AreEqual(_opc, entity.OperatingCenter);
        }

        #endregion

        #region Mapping LocalEmployeeRelationsBusinessPartner/HumanResourcesManager

        [TestMethod]
        public void TestProcessSetsHumanResourcesManagerToNullIfCsvRecordDoesNotHaveLocalEmployeeRelationsBusinessPartnerValue()
        {
            _targetSapRecord.LocalEmployeeRelationsBusinessPartner = null;
            var file = SetupFileAndRecords(_targetSapRecord);

            _target.Process(file);

            var entity = Repository.GetAll().ToList().Last();

            Assert.IsNull(entity.HumanResourcesManager);
        }

        [TestMethod]
        public void TestProcessSetsHumanResourcesManagerCorrectlyWhenTheHumanResourcesEmployeeIsAlsoBeingUpdated()
        {
            _targetSapRecord.LocalEmployeeRelationsBusinessPartner = "54321";
            var file = SetupFileAndRecords(_targetSapRecord);

            var supervisor = GetEntityFactory<Employee>()
               .Create(new {EmployeeId = "54321", Status = _activeEmployeeStatus});

            _target.Process(file);

            var entity = Repository.Where(e => e.EmployeeId == "12345").Single();

            Assert.AreEqual(supervisor, entity.HumanResourcesManager);
        }

        [TestMethod]
        public void TestProcessSetsHumanResourcesManagerCorrectlyWhenTheHumanResourcesManagerEmployeeIsNOTBeingUpdated()
        {
            _targetSapRecord.LocalEmployeeRelationsBusinessPartner = "54321";
            var hrmanager = CreateBasicSapEmployeeFileRecord();
            hrmanager.EmployeeId = "54321";
            hrmanager.LocalEmployeeRelationsBusinessPartner = "54321";
            var file = SetupFileAndRecords(
                _targetSapRecord, // Regular employee
                hrmanager // HRManager employee that doesn't exist yet
            );

            _target.Process(file);

            var entity = Repository.Where(e => e.EmployeeId == "12345").Single();

            Assert.AreEqual("54321", entity.HumanResourcesManager.EmployeeId);
        }

        [TestMethod]
        public void TestProcessSetsHumanResourcesManagerToNullIfMatchingHumanResourcesManagerIsNotFound()
        {
            _targetSapRecord.LocalEmployeeRelationsBusinessPartner = "54321";
            var file = SetupFileAndRecords(_targetSapRecord);

            _target.Process(file);

            var entity = Repository.Where(e => e.EmployeeId == "12345").Single();

            Assert.IsNull(entity.HumanResourcesManager);
        }

        #endregion

        #region Mapping ReportsTo/Supervisor

        [TestMethod]
        public void TestProcessSetsReportsToToNullIfCsvRecordDoesNotHaveReportsToValue()
        {
            _targetSapRecord.ReportsTo = null;
            var file = SetupFileAndRecords(_targetSapRecord);

            _target.Process(file);

            var entity = Repository.GetAll().ToList().Last();

            Assert.IsNull(entity.ReportsTo);
        }

        [TestMethod]
        public void TestProcessSetsReportsToCorrectlyWhenTheSupervisorEmployeeIsAlsoBeingUpdated()
        {
            _targetSapRecord.ReportsTo = "54321";
            var file = SetupFileAndRecords(_targetSapRecord);

            var supervisor = GetEntityFactory<Employee>()
               .Create(new { EmployeeId = "54321", Status = _activeEmployeeStatus });

            _target.Process(file);

            var entity = Repository.Where(e => e.EmployeeId == "12345").Single();

            Assert.AreEqual(supervisor, entity.ReportsTo);
        }

        [TestMethod]
        public void TestProcessSetsReportsToCorrectlyWhenTheSupervisorEmployeeIsNOTBeingUpdated()
        {
            _targetSapRecord.ReportsTo = "54321";
            var supervisor = CreateBasicSapEmployeeFileRecord();
            supervisor.EmployeeId = "54321";
            supervisor.ReportsTo = "54321";
            var file = SetupFileAndRecords(
                _targetSapRecord, // Regular employee
                supervisor // Supervisor employee that doesn't exist yet
            );

            _target.Process(file);

            var entity = Repository.Where(e => e.EmployeeId == "12345").Single();

            Assert.AreEqual("54321", entity.ReportsTo.EmployeeId);
        }

        [TestMethod]
        public void TestProcessSetsReportsToToNullIfMatchingSupervisorIsNotFound()
        {
            _targetSapRecord.ReportsTo = "54321";
            var file = SetupFileAndRecords(_targetSapRecord);

            _target.Process(file);

            var entity = Repository.Where(e => e.EmployeeId == "12345").Single();

            Assert.IsNull(entity.ReportsTo);
        }

        #endregion

        #region NotificationForNewEmployee

        [TestMethod]
        public void TestProcessSendsNotificationForNewHire()
        {
            var employeeId = "12345";
            var existingEmployee = GetEntityFactory<Employee>().Create(new { EmployeeId = employeeId, Status = _inactiveEmployeeStatus });
            var employee = new SapEmployeeFileRecord { EmployeeId = "4444", Status = "Active", City = "New Jersey", FirstName = "Dude", LastName = "McDuderson", DateHired = DateTime.Today };
            var employeeThatExists = new SapEmployeeFileRecord { EmployeeId = employeeId, Status = "Active", City = "New Jersey", FirstName = "Dude", LastName = "McDuderson", DateHired = DateTime.Today };

            NotifierArgs result = null;
            _notificationService.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback((NotifierArgs args) =>
            {
                result = args;
            });

            // Assert : Ensure Existing employee doesn't get notification/result is null.
            var file = SetupFileAndRecords(employeeThatExists);
            _target.Process(file);

            Assert.IsNull(result);

            // Assert : Employee will trigger the notification
            file = SetupFileAndRecords(employee);
            _target.Process(file);

            var resultEmp = Repository.GetAll().ToList().Last();
            Assert.AreEqual("New Hire Email Notification", result.Subject);
            Assert.AreEqual("New Hire Email Notification", result.Purpose);
            Assert.AreEqual(RoleModules.HumanResourcesEmployee, result.Module);
            Assert.AreEqual(resultEmp,result.Data);
        }

        #endregion

        #region Activate/Deactivate users

        [TestMethod]
        public void TestUserWithMatchingEmployeeIdGetsHasAccesSetToTrueWhenEmployeeStatusIsActive()
        {
            var file = SetupFileAndRecords(_targetSapRecord);

            var employee = GetEntityFactory<Employee>().Create(new { EmployeeId = "12345", Status = _activeEmployeeStatus });
            var user = GetEntityFactory<User>().Create(new { UserType = _internalUserType, IsAdmin = false, HasAccess = false, Employee = employee });
            employee.User = user;

            _target.Process(file);

            Assert.IsTrue(user.HasAccess);
        }

        [TestMethod]
        public void TestUserWithMatchingEmployeeIdGetsHasAccesSetToFalseWhenEmployeeStatusIsInactive()
        {
            _targetSapRecord.Status = "Inactive";
            var file = SetupFileAndRecords(_targetSapRecord);

            var employee = GetEntityFactory<Employee>().Create(new { EmployeeId = "12345", Status = _activeEmployeeStatus });
            var user = GetEntityFactory<User>().Create(new { UserType = _internalUserType, IsAdmin = false, Employee = employee, HasAccess = true });
            employee.User = user;

            _target.Process(file);

            Assert.IsFalse(user.HasAccess);
        }

        [TestMethod]
        public void TestUserWithMatchingEmployeeIdGetsHasAccesSetToFalseWhenEmployeeStatusIsWithdrawn()
        {
            _targetSapRecord.Status = "Withdrawn";
            var file = SetupFileAndRecords(_targetSapRecord);

            var employee = GetEntityFactory<Employee>().Create(new { EmployeeId = "12345", Status = _activeEmployeeStatus });
            var user = GetEntityFactory<User>().Create(new { UserType = _internalUserType, IsAdmin = false, Employee = employee, HasAccess = true });
            employee.User = user;

            _target.Process(file);

            Assert.IsFalse(user.HasAccess);
        }

        [TestMethod]
        public void TestUserWithMatchingEmployeeIdGetsHasAccesSetToFalseWhenEmployeeStatusIsRetiree()
        {
            _targetSapRecord.Status = "Retiree";
            var file = SetupFileAndRecords(_targetSapRecord);

            var employee = GetEntityFactory<Employee>().Create(new { EmployeeId = "12345", Status = _activeEmployeeStatus });
            var user = GetEntityFactory<User>().Create(new { UserType = _internalUserType, IsAdmin = false, Employee = employee, HasAccess = true });
            employee.User = user;

            _target.Process(file);

            Assert.IsFalse(user.HasAccess);
        }

        [TestMethod]
        public void TestActivateOrDeactivateMatchingUsersThrowsExceptionWhenUserEmployeeLinkIsInvalid()
        {
            _targetSapRecord.Status = "Retiree";
            var file = SetupFileAndRecords(_targetSapRecord);

            var employee = GetEntityFactory<Employee>().Create(new { EmployeeId = "12345", Status = _activeEmployeeStatus });
            var differentEmployee = GetEntityFactory<Employee>().Create(new { EmployeeId = "33333", Status = _activeEmployeeStatus });
            var user = GetEntityFactory<User>().Create(new { UserType = _internalUserType, IsAdmin = false, Employee = differentEmployee, HasAccess = true });
            employee.User = user;
            differentEmployee.User = user;

            var expected = $"Employee#{employee.Id} links to a User#{user.Id} that links to a different or null Employee(Id: {differentEmployee.Id}).";
            MyAssert.ThrowsWithMessage<InvalidOperationException>(() => _target.Process(file), expected);
        }

        [TestMethod]
        public void TestProcessActuallySavesChangesToUserRecords()
        {
            // Because Nhibernate is automatically doing change tracking on the User record,
            // we aren't explicitly saving the User record when a change is made. So to test
            // that this actually saves, we need to evict and requery after Process.

            _targetSapRecord.Status = "Inactive";
            var file = SetupFileAndRecords(_targetSapRecord);

            var employee = GetEntityFactory<Employee>().Create(new { EmployeeId = "12345", Status = _activeEmployeeStatus });
            var user = GetEntityFactory<User>().Create(new { UserType = _internalUserType, IsAdmin = false, Employee = employee, HasAccess = true });
            Assert.IsTrue(user.HasAccess, "Sanity check");
            employee.User = user;

            _target.Process(file);

            Session.Evict(user);
            var requeriedUser = Session.Query<User>().Single(x => x.Id == user.Id);
            Assert.AreNotSame(user, requeriedUser, "Sanity check.");

            Assert.IsFalse(requeriedUser.HasAccess);
        }

        [TestMethod]
        public void TestProcessDoesNotUpdateUsersWhoAreSiteAdmins()
        {
            _targetSapRecord.Status = "Inactive";
            var file = SetupFileAndRecords(_targetSapRecord);
            var employee = GetEntityFactory<Employee>().Create(new { EmployeeId = "12345", Status = _activeEmployeeStatus });
            var user = GetEntityFactory<User>().Create(new { UserType = _internalUserType, IsAdmin = true, Employee = employee, HasAccess = true });
            employee.User = user;
            _target.Process(file);

            Assert.IsTrue(user.HasAccess, "This user is a site admin so they should still have access");
        }

        [TestMethod]
        public void TestProcessDoesNotUpdateUsersWhoAreNotInternalUserType()
        {
            _targetSapRecord.Status = "Inactive";
            var file = SetupFileAndRecords(_targetSapRecord);

            var employee = GetEntityFactory<Employee>().Create(new { EmployeeId = "12345", Status = _activeEmployeeStatus });
            var user = GetEntityFactory<User>().Create(new { UserType = GetEntityFactory<UserType>().Create(), IsAdmin = false, Employee = employee, HasAccess = true });
            employee.User = user;

            _target.Process(file);

            Assert.IsTrue(user.HasAccess, "This user is not an internal user type so they should still have access");
        }

        #endregion
    }
}
