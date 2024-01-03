using System;
using Contractors.Data.Models.Repositories;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Data;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace Contractors.Tests.Models.ViewModels
{
    [TestClass]
    public class EditMeterChangeOutViewModelTest : MapCallMvcInMemoryDatabaseTestBase<MeterChangeOut>
    {
        #region Fields

        private ViewModelTester<EditMeterChangeOut, MeterChangeOut> _vmTester;
        private EditMeterChangeOut _viewModel;
        private MeterChangeOut _entity;
        private Mock<IAuthenticationService<ContractorUser>> _authServ;
        private ContractorUser _user;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private MeterChangeOutStatus _scheduledStatus;
        private MeterChangeOutStatus _otherStatus;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<ContractorUser>>().Mock();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            e.For<IUserRepository>().Use<UserRepository>();
            e.For<IMeterChangeOutStatusRepository>().Use<MeterChangeOutStatusRepository>();
        }

        [TestInitialize]
        public void TestInitilize()
        {
            _user = GetFactory<ContractorUserFactory>().Create(new {
                IsAdmin = true
            });

            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _entity = GetEntityFactory<MeterChangeOut>().Create();
            _viewModel = _viewModelFactory.Build<EditMeterChangeOut, MeterChangeOut>(_entity);
            _vmTester = new ViewModelTester<EditMeterChangeOut, MeterChangeOut>(_viewModel,_entity);

            GetFactory<MeterChangeOutStatusFactory>().CreateList(MeterChangeOutStatus.Indices.SCHEDULED - 1);
            _scheduledStatus = GetFactory<MeterChangeOutStatusFactory>().Create(new {Description = "Scheduled"});
            _otherStatus = GetFactory<MeterChangeOutStatusFactory>().Create(new {Description = "Other"});
        }

        #endregion

        #region Tests


        [TestMethod]
        public void TestDisplayMeterChangeOutReturnsOriginalMeterChangeOut()
        {
            var entity = GetFactory<MeterChangeOutFactory>().Create();
            var vm = _viewModelFactory.Build<EditMeterChangeOut, MeterChangeOut>(entity);

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
            _vmTester.CanMapBothWays(x => x.NewSerialNumber);
            _vmTester.CanMapBothWays(x => x.NewRFNumber);
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
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.MeterReadComment4, MeterChangeOut.StringLengths.METER_READ_COMMENT);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.SAPOrderNumber, MeterChangeOut.StringLengths.SAP_ORDER_NUMBER);
        }


        [TestMethod]
        public void TestDateScheduledIsRequiredWhenStatusIsSetToScheduled()
        {
            _viewModel.DateScheduled = null;
            _viewModel.MeterChangeOutStatus = null;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.DateScheduled);

            _viewModel.MeterChangeOutStatus = _otherStatus.Id;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.DateScheduled);

            _viewModel.MeterChangeOutStatus = _scheduledStatus.Id;
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.DateScheduled, "The Date Scheduled field is required when the change out status is set to scheduled.");

            _viewModel.DateScheduled = DateTime.Now;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.DateScheduled);

        }

        [TestMethod]
        public void TestScheduledTimeIsRequiredWhenStatusIsSetToScheduled()
        {
            _viewModel.MeterScheduleTime = null;
            _viewModel.MeterChangeOutStatus = null;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.MeterScheduleTime);

            _viewModel.MeterChangeOutStatus = _otherStatus.Id;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.MeterScheduleTime);

            _viewModel.MeterChangeOutStatus = _scheduledStatus.Id;
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.MeterScheduleTime, "The Scheduled Time field is required when the change out status is set to scheduled.");

            _viewModel.MeterScheduleTime = GetFactory<MeterScheduleTimeFactory>().Create().Id;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.MeterScheduleTime);
        }

        [TestMethod]
        public void TestAssignedContractorMeterCrewIsRequiredWhenStatusIsSetToScheduled()
        {
            _viewModel.AssignedContractorMeterCrew = null;
            _viewModel.MeterChangeOutStatus = null;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.AssignedContractorMeterCrew);

            _viewModel.MeterChangeOutStatus = _otherStatus.Id;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.AssignedContractorMeterCrew);

            _viewModel.MeterChangeOutStatus = _scheduledStatus.Id;
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AssignedContractorMeterCrew, "The Assigned Contractor Meter Crew field is required when the change out status is set to scheduled.");

            _viewModel.AssignedContractorMeterCrew = GetFactory<ContractorMeterCrewFactory>().Create().Id;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.AssignedContractorMeterCrew);
        }

        #endregion
    }
}
