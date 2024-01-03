using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using System;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    [TestClass]
    public class EditMeterChangeOutTest : MapCallMvcInMemoryDatabaseTestBase<MeterChangeOut>
    {
        #region Fields

        private ViewModelTester<EditMeterChangeOut, MeterChangeOut> _vmTester;
        private EditMeterChangeOut _viewModel;
        private MeterChangeOut _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private MeterChangeOutStatus _scheduledStatus;
        private MeterChangeOutStatus _otherStatus;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            e.For<IUserRepository>().Use<UserRepository>();
            e.For<IMeterChangeOutStatusRepository>().Use<MeterChangeOutStatusRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create(new
            {
                IsAdmin = true
            });
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _entity = GetEntityFactory<MeterChangeOut>().Create();
            _viewModel = _viewModelFactory.Build<EditMeterChangeOut, MeterChangeOut>( _entity);
            _vmTester = new ViewModelTester<EditMeterChangeOut, MeterChangeOut>(_viewModel, _entity);

            // "scheduled" needs to be id 10
            GetFactory<MeterChangeOutStatusFactory>().CreateList(MeterChangeOutStatus.Indices.SCHEDULED - 1);
            _scheduledStatus = GetFactory<MeterChangeOutStatusFactory>().Create(new {Description = "Scheduled"});
            _otherStatus = GetFactory<MeterChangeOutStatusFactory>().Create(new {Description = "Other"});
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestDisplayMeterChangeOutReturnsOriginalMeterChangeOut()
        {
            // Using the Original property.
            var entity = GetFactory<MeterChangeOutFactory>().Create();
            var vm = _viewModelFactory.Build<EditMeterChangeOut, MeterChangeOut>(entity);
            Assert.AreSame(entity, vm.Display);

            // Using the Id value.
            vm = _viewModelFactory.BuildWithOverrides<EditMeterChangeOut>(new {Id = entity.Id});
            Assert.AreSame(entity, vm.Display);
        }

        [TestMethod]
        public void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.ServicePhone);
            _vmTester.CanMapBothWays(x => x.ServicePhoneExtension);
            _vmTester.CanMapBothWays(x => x.ServicePhone2);
            _vmTester.CanMapBothWays(x => x.ServicePhone2Extension);
            _vmTester.CanMapBothWays(x => x.FieldNotes);
            _vmTester.CanMapBothWays(x => x.DateScheduled);
           // _vmTester.CanMapBothWays(x => x.OutRead);
            _vmTester.CanMapBothWays(x => x.NewSerialNumber);
            _vmTester.CanMapBothWays(x => x.NewRFNumber);
           // _vmTester.CanMapBothWays(x => x.StartRead);
            _vmTester.CanMapBothWays(x => x.MeterReadComment4);
            _vmTester.CanMapBothWays(x => x.SAPOrderNumber);
        }

        [TestMethod]
        public void TestPropertiesHaveStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ServicePhone, MeterChangeOut.StringLengths.SERVICE_PHONE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ServicePhone2, MeterChangeOut.StringLengths.SERVICE_PHONE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ServicePhoneExtension, MeterChangeOut.StringLengths.SERVICE_PHONE_EXTENSION);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ServicePhone2Extension, MeterChangeOut.StringLengths.SERVICE_PHONE_EXTENSION);
           // ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.NewSerialNumber, MeterChangeOut.StringLengths.NEW_SERIAL_NUMBER,error: "The field NewSerialNumber must be a string with a minimum length of 8 and a maximum length of 8.");
         //   ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.NewRFNumber, MeterChangeOut.StringLengths.NEW_RF_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.MeterReadComment4, MeterChangeOut.StringLengths.METER_READ_COMMENT);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.SAPOrderNumber, MeterChangeOut.StringLengths.SAP_ORDER_NUMBER);
        }

        [TestMethod]
        public void TestDateScheduledIsRequiredWhenStatusIsSetToScheduled()
        {
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.DateScheduled, DateTime.Now, x => x.MeterChangeOutStatus, _scheduledStatus.Id, _otherStatus.Id,"The Date Scheduled field is required when the change out status is set to scheduled.");
        }

        [TestMethod]
        public void TestScheduledTimeIsRequiredWhenStatusIsSetToScheduled()
        {
            var mst = GetEntityFactory<MeterScheduleTime>().Create();
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.MeterScheduleTime, mst.Id, x => x.MeterChangeOutStatus, _scheduledStatus.Id, _otherStatus.Id, "The Scheduled Time field is required when the change out status is set to scheduled.");
        }

        [TestMethod]
        public void TestAssignedContractorMeterCrewIsRequiredWhenStatusIsSetToScheduled()
        {
            var crew = GetEntityFactory<ContractorMeterCrew>().Create();
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AssignedContractorMeterCrew, crew.Id, x => x.MeterChangeOutStatus, _scheduledStatus.Id, _otherStatus.Id,"The Assigned Contractor Meter Crew field is required when the change out status is set to scheduled.");
        }

        #endregion
    }
}
