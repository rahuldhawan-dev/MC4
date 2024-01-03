using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class AddIncidentEmployeeAvailabilityTest : ViewModelTestBase<Incident, AddIncidentEmployeeAvailability>
    {
        #region Private Members
        
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        
        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);
            e.For<IAuthenticationService<User>>().Use((_authServ = new Mock<IAuthenticationService<User>>()).Object);
            e.For<IIncidentRepository>().Use<IncidentRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel.Id = _entity.Id;
            _authServ.Setup(x => x.CurrentUser)
                     .Returns(_user = GetFactory<AdminUserFactory>().Create());
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            // noop, no properties on this view model map directly
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(
                _viewModel,
                x => x.EmployeeAvailabilityType);
            ValidationAssert.PropertyIsRequired(
                _viewModel,
                x => x.StartDate);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            // TODO: this fails because it goes to save an IncidentEmployeeAvailability without an Incident value
            // ValidationAssert.EntityMustExist(
            //     _viewModel,
            //     x => x.ExistingEmployeeAvailability,
            //     GetEntityFactory<IncidentEmployeeAvailability>().Create());
            // ValidationAssert.EntityMustExist(
            //     _viewModel,
            //     x => x.EmployeeAvailabilityType,
            //     GetEntityFactory<IncidentEmployeeAvailabilityType>().Create());
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // noop, no string properties
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestMapToEntityAddsNewIncidentLostTimeRecordToIncident()
        {
            var expectedStart = DateTime.Today;
            var expectedEnd = DateTime.Today.AddDays(1);
            _viewModel.StartDate = expectedStart;
            _viewModel.EndDate = expectedEnd;
            _viewModel.EmployeeAvailabilityType = GetFactory<LostTimeIncidentEmployeeAvailabilityTypeFactory>().Create().Id;

            _vmTester.MapToEntity();

            var result = _entity.EmployeeAvailabilities.Single();
            Assert.AreEqual(expectedStart, result.StartDate);
            Assert.AreEqual(expectedEnd, result.EndDate.Value);
            Assert.AreSame(_entity, result.Incident);
        }

        [TestMethod]
        public void TestMapToEntitySetsIncidentClassificationOnIncidentWhenLessSevereThanLostTimeOrRestrictedDuty()
        {
            // need these to prevent null ref
            var expectedStart = DateTime.Today;
            var expectedEnd = DateTime.Today.AddDays(1);
            _viewModel.StartDate = expectedStart;
            _viewModel.EndDate = expectedEnd;

            // this item is index 14, the highest, so this should cover all of them
            GetEntityFactory<IncidentClassification>().CreateList(IncidentClassification.Indices.RESTRICTED_DUTY + 1);

            // less severe to restricted duty
            _entity.IncidentClassification = Session.Load<IncidentClassification>(2);
            _viewModel.EmployeeAvailabilityType =
                GetFactory<RestrictiveDutyIncidentEmployeeAvailabilityTypeFactory>().Create().Id;

            _vmTester.MapToEntity();

            Assert.AreEqual(IncidentClassification.Indices.RESTRICTED_DUTY, _entity.IncidentClassification.Id);

            // restricted duty to lost time
            _entity.IncidentClassification = Session.Load<IncidentClassification>(IncidentClassification.Indices.RESTRICTED_DUTY);
            _viewModel.EmployeeAvailabilityType =
                GetFactory<LostTimeIncidentEmployeeAvailabilityTypeFactory>().Create().Id;

            _vmTester.MapToEntity();

            Assert.AreEqual(IncidentClassification.Indices.LOST_TIME, _entity.IncidentClassification.Id);

            // lost time does not change
            _entity.IncidentClassification = Session.Load<IncidentClassification>(IncidentClassification.Indices.LOST_TIME);
            _viewModel.EmployeeAvailabilityType =
                GetFactory<RestrictiveDutyIncidentEmployeeAvailabilityTypeFactory>().Create().Id;

            _vmTester.MapToEntity();

            Assert.AreEqual(IncidentClassification.Indices.LOST_TIME, _entity.IncidentClassification.Id);

            // fatality does not change
            _entity.IncidentClassification = Session.Load<IncidentClassification>(IncidentClassification.Indices.FATALITY);
            _viewModel.EmployeeAvailabilityType =
                GetFactory<RestrictiveDutyIncidentEmployeeAvailabilityTypeFactory>().Create().Id;

            _vmTester.MapToEntity();

            Assert.AreEqual(IncidentClassification.Indices.FATALITY, _entity.IncidentClassification.Id);

            _viewModel.EmployeeAvailabilityType =
                GetFactory<LostTimeIncidentEmployeeAvailabilityTypeFactory>().Create().Id;

            _vmTester.MapToEntity();

            Assert.AreEqual(IncidentClassification.Indices.FATALITY, _entity.IncidentClassification.Id);
        }

        [TestMethod]
        public void TestValidationOfDateRanges()
        {
            var type = GetEntityFactory<IncidentEmployeeAvailabilityType>().Create();
            _viewModel.EmployeeAvailabilityType = type.Id;
            var existingLostTime = new IncidentEmployeeAvailability();
            existingLostTime.StartDate = new DateTime(1984, 4, 24);
            _entity.EmployeeAvailabilities.Add(existingLostTime);

            // Test validation fails if there's a record without an EndDate set.
            existingLostTime.EndDate = null;
            ValidationAssert.ModelStateHasNonPropertySpecificError(_viewModel, "An employee availability record exists that does not have an end date specified. This record must be closed before continuing.");

            // Test start date falls within range fails
            existingLostTime.EndDate = new DateTime(1984, 4, 27);
            _viewModel.StartDate = new DateTime(1984, 4, 24);
            _viewModel.EndDate = new DateTime(1985, 5, 25);

            ValidationAssert.ModelStateHasNonPropertySpecificError(_viewModel, "The given employee availability start date overlaps with an existing record.");

            // Test end date falls within range fails
            _viewModel.StartDate = new DateTime(1984, 4, 23);
            _viewModel.EndDate = new DateTime(1984, 4, 27);

            ValidationAssert.ModelStateHasNonPropertySpecificError(_viewModel, "The given employee availability end date overlaps with an existing record.");

            // Test start date and end date cover entire existing range fails
            _viewModel.StartDate = new DateTime(1984, 4, 23);
            _viewModel.EndDate = new DateTime(1984, 4, 28);

            ValidationAssert.ModelStateHasNonPropertySpecificError(_viewModel, "The given employee availability date range is already encompassed by another record.");

            // Test validation passes if there isn't overlap anywhere.
            _viewModel.StartDate = new DateTime(1984, 4, 28);
            _viewModel.EndDate = new DateTime(1984, 4, 30);
            ValidationAssert.ModelStateIsValid(_viewModel);

            // Test validation passes if the StartDate is equal to the EndDate of the previous record.
            _viewModel.StartDate = existingLostTime.EndDate.Value;
            ValidationAssert.ModelStateIsValid(_viewModel);

            // Test validation passes(and doesn't crash) if the EndDate isn't set because EndDate isn't required.
            _viewModel.EndDate = null;
            ValidationAssert.ModelStateIsValid(_viewModel);
        }

        [TestMethod]
        public void TestValidationOfDateRangesWhenUpdatingAnExistingRecord()
        {
            var type = GetEntityFactory<IncidentEmployeeAvailabilityType>().Create();
            _viewModel.EmployeeAvailabilityType = type.Id;
            var existingLostTime = new IncidentEmployeeAvailability();
            existingLostTime.StartDate = new DateTime(1984, 4, 24);
            existingLostTime.Incident = _entity;
            existingLostTime.EmployeeAvailabilityType = type;
            _entity.EmployeeAvailabilities.Add(existingLostTime);

            var updatingAvail = new IncidentEmployeeAvailability();
            updatingAvail.Incident = _entity;
            updatingAvail.EmployeeAvailabilityType = type;
            updatingAvail.StartDate = existingLostTime.StartDate;
            _entity.EmployeeAvailabilities.Add(updatingAvail);
            Session.Save(_entity);
            Session.Flush();
            _viewModel.ExistingEmployeeAvailability = updatingAvail.Id;

            // Test validation fails if there's a record without an EndDate set.
            existingLostTime.EndDate = null;
            ValidationAssert.ModelStateHasNonPropertySpecificError(_viewModel, "An employee availability record exists that does not have an end date specified. This record must be closed before continuing.");

            // Test start date falls within range fails
            existingLostTime.EndDate = new DateTime(1984, 4, 27);
            _viewModel.StartDate = new DateTime(1984, 4, 24);
            _viewModel.EndDate = new DateTime(1985, 5, 25);

            ValidationAssert.ModelStateHasNonPropertySpecificError(_viewModel, "The given employee availability start date overlaps with an existing record.");

            // Test end date falls within range fails
            _viewModel.StartDate = new DateTime(1984, 4, 23);
            _viewModel.EndDate = new DateTime(1984, 4, 27);

            ValidationAssert.ModelStateHasNonPropertySpecificError(_viewModel, "The given employee availability end date overlaps with an existing record.");

            // Test start date and end date cover entire existing range fails
            _viewModel.StartDate = new DateTime(1984, 4, 23);
            _viewModel.EndDate = new DateTime(1984, 4, 28);

            ValidationAssert.ModelStateHasNonPropertySpecificError(_viewModel, "The given employee availability date range is already encompassed by another record.");

            // Test validation passes if there isn't overlap anywhere.
            _viewModel.StartDate = new DateTime(1984, 4, 28);
            _viewModel.EndDate = new DateTime(1984, 4, 30);
            ValidationAssert.ModelStateIsValid(_viewModel);

            // Test validation passes if the StartDate is equal to the EndDate of the previous record.
            _viewModel.StartDate = existingLostTime.EndDate.Value;
            ValidationAssert.ModelStateIsValid(_viewModel);

            // Test validation passes(and doesn't crash) if the EndDate isn't set because EndDate isn't required.
            _viewModel.EndDate = null;
            ValidationAssert.ModelStateIsValid(_viewModel);
        }

        #endregion
    }
}
