using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels
{
    [TestClass]
    public class ServiceSizeViewModelTest : MapCallMvcInMemoryDatabaseTestBase<ServiceSize>
    {
        private ViewModelTester<ServiceSizeViewModel, ServiceSize> _vmTester;
        private ServiceSizeViewModel _viewModel;
        private ServiceSize _entity;

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new ServiceSizeViewModel(_container);
            _entity = GetFactory<ServiceSizeFactory>().Create();
            _vmTester = new ViewModelTester<ServiceSizeViewModel, ServiceSize>(_viewModel, _entity);
        }
        
        #endregion

        [TestMethod]
        public void TestPropertiesCanMopBathWays()
        {
            _vmTester.CanMapBothWays(x => x.Hydrant);
            _vmTester.CanMapBothWays(x => x.Lateral);
            _vmTester.CanMapBothWays(x => x.Main);
            _vmTester.CanMapBothWays(x => x.Meter);
            _vmTester.CanMapBothWays(x => x.SortOrder);
            _vmTester.CanMapBothWays(x => x.Service);
            _vmTester.CanMapBothWays(x => x.ServiceSizeDescription);
            _vmTester.CanMapBothWays(x => x.Size);
        }
    }
}