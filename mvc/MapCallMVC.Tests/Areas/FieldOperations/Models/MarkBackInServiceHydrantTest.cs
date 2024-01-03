using System;
using System.Linq;
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
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    [TestClass]
    public class MarkBackInServiceHydrantTest : MapCallMvcInMemoryDatabaseTestBase<Hydrant>
    {
        #region Fields

        private ViewModelTester<MarkBackInServiceHydrant, Hydrant> _vmTester;
        private MarkBackInServiceHydrant _viewModel;
        private Hydrant _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            e.For<IUserRepository>().Use<UserRepository>();
            e.For<IHydrantRepository>().Use<HydrantRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IValveRepository>().Use<ValveRepository>();
            e.For<IStreetRepository>().Use<StreetRepository>();
            e.For<IFireDistrictRepository>().Use<FireDistrictRepository>();
            e.For<IAssetStatusRepository>().Use<AssetStatusRepository>();
            e.For<IHydrantBillingRepository>().Use<HydrantBillingRepository>();
            e.For<IAbbreviationTypeRepository>().Use<AbbreviationTypeRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create(new {
                IsAdmin = true
            });
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _entity = GetEntityFactory<Hydrant>().Create();
            _viewModel = _viewModelFactory.Build<MarkBackInServiceHydrant, Hydrant>(_entity);
            _vmTester = new ViewModelTester<MarkBackInServiceHydrant, Hydrant>(_viewModel, _entity);

            // These need to exist
            GetFactory<PublicHydrantBillingFactory>().Create();
            GetFactory<RetiredAssetStatusFactory>().Create();
            GetFactory<TownSectionAbbreviationTypeFactory>().Create();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestMapToEntityDoesNotModifyMostRecentOutOfServiceRecordWhenHydrantIsAlreadyBackInService()
        {
            var expectedDate = DateTime.Today.AddHours(3);
            var expectedUser = GetFactory<UserFactory>().Create();

            var oos = GetFactory<HydrantOutOfServiceFactory>().Create(new
            {
                Hydrant = _entity,
                BackInServiceDate = expectedDate,
                BackInServiceByUser = expectedUser
            });
            Assert.IsTrue(_entity.OutOfServiceRecords.Contains(oos), "Sanity");
            Assert.IsFalse(_entity.OutOfService, "https://open.spotify.com/track/4OEieI4gzNImiQQqCrLALO");

            _viewModel.BackInServiceDate = expectedDate.AddDays(2);
            _vmTester.MapToEntity();
            Assert.AreSame(oos, _entity.OutOfServiceRecords.Single(), "No new record should be created.");
            Assert.AreEqual(expectedDate, oos.BackInServiceDate, "BackInServiceDate should not be modified when it has already been set.");
            Assert.AreSame(expectedUser, oos.BackInServiceByUser, "BackInServiceByUser should not be modified when it has already been set.");
        }

        [TestMethod]
        public void TestMapToEntitySetsBackInServiceDateAndUserWhenClosingAnExistingOutOfServiceRecord()
        {
            var expectedDate = DateTime.Today.AddHours(3);
            var expectedUser = GetFactory<UserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(expectedUser);

            var oos = GetFactory<HydrantOutOfServiceFactory>().Create(new { Hydrant = _entity });
            _viewModel.BackInServiceDate = expectedDate;
            _entity.SetPropertyValueByName("OutOfService", true);

            _vmTester.MapToEntity();
            Assert.AreEqual(expectedDate, oos.BackInServiceDate);
            Assert.AreSame(expectedUser, oos.BackInServiceByUser);
        }



        [TestMethod]
        public void TestMapToEntityDoesNotSetOufOfServiceDateWhenClosingAHydrantOutOfServiceRecord()
        {
            var expectedDate = DateTime.Today.AddHours(3);
            var expectedUser = GetFactory<UserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(expectedUser);

            var oos = GetFactory<HydrantOutOfServiceFactory>().Create(new
            {
                Hydrant = _entity,
                OutOfServiceDate = expectedDate
            });
            _viewModel.BackInServiceDate = expectedDate.AddDays(2);
           // _viewModel.OutOfServiceDate = expectedDate.AddDays(1);
           // _viewModel.OutOfService = false;
            _entity.SetPropertyValueByName("OutOfService", true);

            _vmTester.MapToEntity();
            Assert.AreEqual(expectedDate, oos.OutOfServiceDate, "Out of service date should not have changed.");
        }

        [TestMethod]
        public void TestBackInServiceDateIsRequired()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.BackInServiceDate);
        }
        #endregion
    }
}
