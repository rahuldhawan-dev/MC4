using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;

namespace MapCallMVC.Tests.Areas.Production.ViewModels
{
    [TestClass]
    public class TaskGroupCategoryViewModelTest : MapCallMvcInMemoryDatabaseTestBase<TaskGroupCategory>
    {
        private ViewModelTester<TaskGroupCategoryViewModel, TaskGroupCategory> _vmTester;
        private TaskGroupCategoryViewModel _viewModel;
        protected TaskGroupCategory _entity;

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = _viewModelFactory.Build<TaskGroupCategoryViewModel>();
            _entity = new TaskGroupCategory();
            _vmTester = new ViewModelTester<TaskGroupCategoryViewModel, TaskGroupCategory>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Description);
            _vmTester.CanMapBothWays(x => x.Type);
            _vmTester.CanMapBothWays(x => x.Abbreviation);
            _vmTester.CanMapBothWays(x => x.IsActive);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Description);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Type);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Abbreviation);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IsActive);
        }

        [TestMethod]
        public void TestMaxLengthOnStringProperties()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Description, TaskGroupCategory.StringLengths.DESCRIPTION);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Type, TaskGroupCategory.StringLengths.TYPE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Abbreviation, TaskGroupCategory.StringLengths.ABBREVIATION);
        }

        #endregion
    }
}