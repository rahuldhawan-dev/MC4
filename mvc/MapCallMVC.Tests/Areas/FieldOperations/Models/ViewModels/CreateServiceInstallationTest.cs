using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels
{
    [TestClass]
    public class CreateServiceInstallationTest : MapCallMvcInMemoryDatabaseTestBase<ServiceInstallation>
    {
        #region Fields

        private ViewModelTester<CreateServiceInstallation, ServiceInstallation> _vmTester;
        private CreateServiceInstallation _viewModel;
        private ServiceInstallation _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create(new { IsAdmin = true });
            _authServ = new Mock<IAuthenticationService<User>>();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _container.Inject(_authServ.Object);
            _entity = new ServiceInstallation { };
            _viewModel = _viewModelFactory.Build<CreateServiceInstallation, ServiceInstallation>(_entity);
            _vmTester = new ViewModelTester<CreateServiceInstallation, ServiceInstallation>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.MeterManufacturerSerialNumber);
            _vmTester.CanMapBothWays(x => x.MaterialNumber);
            _vmTester.CanMapBothWays(x => x.Manufacturer);
            _vmTester.CanMapBothWays(x => x.ServiceType);
            _vmTester.CanMapBothWays(x => x.MeterSize);
            _vmTester.CanMapBothWays(x => x.Register1Dials);
            _vmTester.CanMapBothWays(x => x.MeterLocationInformation);
            _vmTester.CanMapBothWays(x => x.Register1CurrentRead);
            _vmTester.CanMapBothWays(x => x.MeterDeviceCategory);
            _vmTester.CanMapBothWays(x => x.Register1DeviceCategory);
            _vmTester.CanMapBothWays(x => x.Register2DeviceCategory);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.MeterManufacturerSerialNumber);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.MeterPositionalLocation);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.MeterLocation);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.MeterDirectionalLocation);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ReadingDeviceSupplemental);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ReadingDevicePosition);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Register1ReadType);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Register1RFMIU);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Register1CurrentRead);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Activity1);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ServiceFound);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ServiceLeft);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatedPointOfControl);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ServiceInstallationReason);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.MeterLocationInformation);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.MiuInstallReason);
        }

        [TestMethod]
        public void TestSetDefaultsSetsMaterialNumber()
        { 
          _viewModel.SetDefaults();

           Assert.AreEqual(ServiceInstallationViewModel.MATERIAL_NUMBER, _viewModel.MaterialNumber);
        }
        #endregion
    }

}
