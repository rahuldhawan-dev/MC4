using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Production.ViewModels
{
    [TestClass]
    public class SkillSetViewModelTest : MapCallMvcInMemoryDatabaseTestBase<SkillSet>
    {
        #region Fields

        private ViewModelTester<SkillSetViewModel, SkillSet> _vmTester;
        private SkillSetViewModel _viewModel;
        private SkillSet _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = _viewModelFactory.Build<SkillSetViewModel>();
            _entity = new SkillSet();
            _vmTester = new ViewModelTester<SkillSetViewModel, SkillSet>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Name);
            _vmTester.CanMapBothWays(x => x.Abbreviation);
            _vmTester.CanMapBothWays(x => x.IsActive);
            _vmTester.CanMapBothWays(x => x.Description);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Name);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Abbreviation);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IsActive);
        }

        [TestMethod]
        public void TestNotRequiredFields()
        {
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.Description);
        }

        [TestMethod]
        public void TestMaxLengthOnStringProperties()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Name, SkillSet.StringLengths.NAME);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Abbreviation, SkillSet.StringLengths.ABBREVIATION);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Description, SkillSet.StringLengths.DESCRIPTION);
        }

        #endregion
    }
}
