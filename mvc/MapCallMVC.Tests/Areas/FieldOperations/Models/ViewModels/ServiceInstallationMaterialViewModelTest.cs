using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels
{
    [TestClass]
    public class ServiceInstallationMaterialViewModelTest : MapCallMvcInMemoryDatabaseTestBase<ServiceInstallationMaterial>
    {
        private ViewModelTester<ServiceInstallationMaterialViewModel, ServiceInstallationMaterial> _vmTester;
        private ServiceInstallationMaterialViewModel _viewModel;
        private ServiceInstallationMaterial _entity;

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new ServiceInstallationMaterialViewModel(_container);
            _entity = GetFactory<ServiceInstallationMaterialFactory>().Create();
            _vmTester = new ViewModelTester<ServiceInstallationMaterialViewModel, ServiceInstallationMaterial>(_viewModel, _entity);
        }

        #endregion

        [TestMethod]
        public void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.SortOrder);
            _vmTester.CanMapBothWays(x => x.Description);
            _vmTester.CanMapBothWays(x => x.PartSize);
            _vmTester.CanMapBothWays(x => x.PartQuantity);
        }


        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ServiceSize);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ServiceCategory);
        }

        [TestMethod]
        public void TestOperatingCenterCanMapBothWays()
        {
            var opcntr = GetEntityFactory<OperatingCenter>().Create();
            _entity.OperatingCenter = opcntr;

            _vmTester.MapToViewModel();

            Assert.AreEqual(opcntr.Id, _viewModel.OperatingCenter);

            _entity.OperatingCenter = null;
            _vmTester.MapToEntity();

            Assert.AreSame(opcntr, _entity.OperatingCenter);
        }

        [TestMethod]
        public void TestServiceCategoryCanMapBothWays()
        {
            var sc = GetEntityFactory<ServiceCategory>().Create(new {Description = "Foo"});
            _entity.ServiceCategory = sc;

            _vmTester.MapToViewModel();

            Assert.AreEqual(sc.Id, _viewModel.ServiceCategory);

            _entity.ServiceCategory = null;
            _vmTester.MapToEntity();

            Assert.AreSame(sc, _entity.ServiceCategory);
        }

        [TestMethod]
        public void TestServiceSizeCanMapBothWays()
        {
            var ss = GetEntityFactory<ServiceSize>().Create(new {Description = "Foo"});
            _entity.ServiceSize = ss;

            _vmTester.MapToViewModel();

            Assert.AreEqual(ss.Id, _viewModel.ServiceSize);

            _entity.ServiceSize = null;
            _vmTester.MapToEntity();

            Assert.AreSame(ss, _entity.ServiceSize);
        }
    }
}