using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using NHibernate.Criterion;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels
{
    [TestClass]
    public class CreateSewerOpeningConnectionTest : MapCallMvcInMemoryDatabaseTestBase<SewerOpeningConnection>
    {
        #region Fields

        private ViewModelTester<CreateSewerOpeningConnection, SewerOpeningConnection> _vmTester;
        private CreateSewerOpeningConnection _viewModel;
        private SewerOpeningConnection _entity;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            e.For<ISewerOpeningConnectionRepository>().Use<SewerOpeningConnectionRepository>();
            e.For<ISewerOpeningRepository>().Use<SewerOpeningRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<ITownRepository>().Use<TownRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _viewModel = new CreateSewerOpeningConnection(_container);
            _entity = new SewerOpeningConnection();
            _vmTester = new ViewModelTester<CreateSewerOpeningConnection, SewerOpeningConnection>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.IsInlet);
            _vmTester.CanMapBothWays(x => x.Size);
            _vmTester.CanMapBothWays(x => x.Invert);
            _vmTester.CanMapBothWays(x => x.Route);
            _vmTester.CanMapBothWays(x => x.Stop);
            _vmTester.CanMapBothWays(x => x.InspectionFrequency);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.UpstreamOpening);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.DownstreamOpening);
        }

        #region CMBW

        [TestMethod]
        public void TestDownstreamSewerOpeningCanMapBothWays()
        {
            _user.IsAdmin = true;
            var sm = GetFactory<SewerOpeningFactory>().Create(new { OpeningNumber = "MAD-1"});
            _entity.DownstreamOpening = sm;

            _vmTester.MapToViewModel();

            Assert.AreEqual(sm.Id, _viewModel.DownstreamOpening);

            _entity.DownstreamOpening = null;
            _vmTester.MapToEntity();

            Assert.AreSame(sm, _entity.DownstreamOpening);
        }

        [TestMethod]
        public void TestUpstreamSewerOpeningCanMapBothWays()
        {
            _user.IsAdmin = true;
            var sm = GetEntityFactory<SewerOpening>().Create();
            _entity.UpstreamOpening = sm;

            _vmTester.MapToViewModel();

            Assert.AreEqual(sm.Id, _viewModel.UpstreamOpening);

            _entity.UpstreamOpening = null;
            _vmTester.MapToEntity();

            Assert.AreSame(sm, _entity.UpstreamOpening);
        }

        [TestMethod]
        public void TestPipeMaterialCanMapBothWays()
        {
            var pipeMaterial = GetEntityFactory<PipeMaterial>().Create(new { Description = "Foo" });
            _entity.SewerPipeMaterial = pipeMaterial;

            _vmTester.MapToViewModel();

            Assert.AreEqual(pipeMaterial.Id, _viewModel.SewerPipeMaterial);

            _entity.SewerPipeMaterial = null;
            _vmTester.MapToEntity();

            Assert.AreSame(pipeMaterial, _entity.SewerPipeMaterial);
        }

        [TestMethod]
        public void TestSewerTerminationTypeCanMapBothWays()
        {
            var stt = GetEntityFactory<SewerTerminationType>().Create(new { Description = "Foo" });
            _entity.SewerTerminationType = stt;

            _vmTester.MapToViewModel();

            Assert.AreEqual(stt.Id, _viewModel.SewerTerminationType);

            _entity.SewerTerminationType = null;
            _vmTester.MapToEntity();

            Assert.AreSame(stt, _entity.SewerTerminationType);
        }

        [TestMethod]
        public void TestRecurringFrequencyUnitCanMapBothWays()
        {
            var rfu = GetFactory<YearlyRecurringFrequencyUnitFactory>().Create();
            _entity.InspectionFrequencyUnit = rfu;

            _vmTester.MapToViewModel();

            Assert.AreEqual(rfu.Id, _viewModel.InspectionFrequencyUnit);

            _entity.InspectionFrequencyUnit = null;
            _vmTester.MapToEntity();

            Assert.AreSame(rfu, _entity.InspectionFrequencyUnit);
        }

        #endregion

        #endregion
    }

    [TestClass]
    public class EditSewerOpeningConnectionTest : MapCallMvcInMemoryDatabaseTestBase<SewerOpeningConnection>
    {
        #region Fields

        private ViewModelTester<EditSewerOpeningConnection, SewerOpeningConnection> _vmTester;
        private EditSewerOpeningConnection _viewModel;
        private SewerOpeningConnection _entity;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            e.For<ISewerOpeningConnectionRepository>().Use<SewerOpeningConnectionRepository>();
            e.For<ISewerOpeningRepository>().Use<SewerOpeningRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<ITownRepository>().Use<TownRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _viewModel = new EditSewerOpeningConnection(_container);
            _entity = new SewerOpeningConnection();
            _vmTester = new ViewModelTester<EditSewerOpeningConnection, SewerOpeningConnection>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.IsInlet);
            _vmTester.CanMapBothWays(x => x.Size);
            _vmTester.CanMapBothWays(x => x.Invert);
            _vmTester.CanMapBothWays(x => x.Route);
            _vmTester.CanMapBothWays(x => x.Stop);
            _vmTester.CanMapBothWays(x => x.InspectionFrequency);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.UpstreamOpening);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.DownstreamOpening);
        }

        [TestMethod]
        public void TestUpstreamSewerOpeningCanMapBothWays()
        {
            _user.IsAdmin = true;
            var sm = GetEntityFactory<SewerOpening>().Create();
            _entity.UpstreamOpening = sm;

            _vmTester.MapToViewModel();

            Assert.AreEqual(sm.Id, _viewModel.UpstreamOpening);

            _entity.UpstreamOpening = null;
            _vmTester.MapToEntity();

            Assert.AreSame(sm, _entity.UpstreamOpening);
        }

        [TestMethod]
        public void TestDownstreamSewerOpeningCanMapBothWays()
        {
            _user.IsAdmin = true;
            var sm = GetEntityFactory<SewerOpening>().Create();
            _entity.DownstreamOpening = sm;

            _vmTester.MapToViewModel();

            Assert.AreEqual(sm.Id, _viewModel.DownstreamOpening);

            _entity.DownstreamOpening = null;
            _vmTester.MapToEntity();

            Assert.AreSame(sm, _entity.DownstreamOpening);
        }

        [TestMethod]
        public void TestPipeMaterialCanMapBothWays()
        {
            var pipeMaterial = GetEntityFactory<PipeMaterial>().Create(new {Description = "Foo"});
            _entity.SewerPipeMaterial = pipeMaterial;

            _vmTester.MapToViewModel();

            Assert.AreEqual(pipeMaterial.Id, _viewModel.SewerPipeMaterial);

            _entity.SewerPipeMaterial = null;
            _vmTester.MapToEntity();

            Assert.AreSame(pipeMaterial, _entity.SewerPipeMaterial);
        }

        [TestMethod]
        public void TestSewerTerminationTypeCanMapBothWays()
        {
            var stt = GetEntityFactory<SewerTerminationType>().Create(new {Description = "Foo"});
            _entity.SewerTerminationType = stt;

            _vmTester.MapToViewModel();

            Assert.AreEqual(stt.Id, _viewModel.SewerTerminationType);

            _entity.SewerTerminationType = null;
            _vmTester.MapToEntity();

            Assert.AreSame(stt, _entity.SewerTerminationType);
        }

        [TestMethod]
        public void TestRecurringFrequencyUnitCanMapBothWays()
        {
            var rfu = GetFactory<YearlyRecurringFrequencyUnitFactory>().Create();
            _entity.InspectionFrequencyUnit = rfu;

            _vmTester.MapToViewModel();

            Assert.AreEqual(rfu.Id, _viewModel.InspectionFrequencyUnit);

            _entity.InspectionFrequencyUnit = null;
            _vmTester.MapToEntity();

            Assert.AreSame(rfu, _entity.InspectionFrequencyUnit);
        }

        #endregion
    }
}