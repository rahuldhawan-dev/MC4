using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.ProjectManagement.Models.ViewModels
{
    [TestClass]
    public class CreateLargeServiceProjectTest : MapCallMvcInMemoryDatabaseTestBase<LargeServiceProject>
    {
        #region Fields

        private ViewModelTester<CreateLargeServiceProject, LargeServiceProject> _vmTester;
        private CreateLargeServiceProject _viewModel;
        private LargeServiceProject _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new CreateLargeServiceProject(_container);
            _entity = new LargeServiceProject();
            _vmTester = new ViewModelTester<CreateLargeServiceProject, LargeServiceProject>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.WBSNumber);
            _vmTester.CanMapBothWays(x => x.ProjectTitle);
            _vmTester.CanMapBothWays(x => x.ProjectAddress);
            _vmTester.CanMapBothWays(x => x.ContactName);
            _vmTester.CanMapBothWays(x => x.ContactEmail);
            _vmTester.CanMapBothWays(x => x.ContactPhone);
            _vmTester.CanMapBothWays(x => x.InitialContactDate);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Town);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ProjectTitle);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ProjectAddress);
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
        public void TestTownCanMapBothWays()
        {
            var town = GetEntityFactory<Town>().Create();
            _entity.Town = town;

            _vmTester.MapToViewModel();

            Assert.AreEqual(town.Id, _viewModel.Town);

            _entity.Town = null;
            _vmTester.MapToEntity();

            Assert.AreSame(town, _entity.Town);
        }

        [TestMethod]
        public void TestAssetCategoryCanMapBothWays()
        {
            var ac = GetEntityFactory<AssetCategory>().Create(new {Description = "Foo"});
            _entity.AssetCategory = ac;

            _vmTester.MapToViewModel();

            Assert.AreEqual(ac.Id, _viewModel.AssetCategory);

            _entity.AssetCategory = null;
            _vmTester.MapToEntity();

            Assert.AreSame(ac, _entity.AssetCategory);
        }

        [TestMethod]
        public void TestAssetTypeCanMapBothWays()
        {
            var at = GetFactory<ValveAssetTypeFactory>().Create();
            _entity.AssetType = at;

            _vmTester.MapToViewModel();

            Assert.AreEqual(at.Id, _viewModel.AssetType);

            _entity.AssetType = null;
            _vmTester.MapToEntity();

            Assert.AreSame(at, _entity.AssetType);
        }

        [TestMethod]
        public void TestProposedPipeDiameterCanMapBothWays()
        {
            var pd = GetEntityFactory<PipeDiameter>().Create();
            _entity.ProposedPipeDiameter = pd;

            _vmTester.MapToViewModel();

            Assert.AreEqual(pd.Id, _viewModel.ProposedPipeDiameter);

            _entity.ProposedPipeDiameter = null;
            _vmTester.MapToEntity();

            Assert.AreSame(pd, _entity.ProposedPipeDiameter);
        }

        #endregion
    }

    [TestClass]
    public class EditLargeServiceProjectTest : MapCallMvcInMemoryDatabaseTestBase<LargeServiceProject>
    {
        #region Fields

        private ViewModelTester<EditLargeServiceProject, LargeServiceProject> _vmTester;
        private EditLargeServiceProject _viewModel;
        private LargeServiceProject _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new EditLargeServiceProject(_container);
            _entity = new LargeServiceProject();
            _vmTester = new ViewModelTester<EditLargeServiceProject, LargeServiceProject>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.WBSNumber);
            _vmTester.CanMapBothWays(x => x.ProjectTitle);
            _vmTester.CanMapBothWays(x => x.ProjectAddress);
            _vmTester.CanMapBothWays(x => x.ContactName);
            _vmTester.CanMapBothWays(x => x.ContactEmail);
            _vmTester.CanMapBothWays(x => x.ContactPhone);
            _vmTester.CanMapBothWays(x => x.InitialContactDate);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Town);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ProjectTitle);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ProjectAddress);
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
        public void TestTownCanMapBothWays()
        {
            var town = GetEntityFactory<Town>().Create();
            _entity.Town = town;

            _vmTester.MapToViewModel();

            Assert.AreEqual(town.Id, _viewModel.Town);

            _entity.Town = null;
            _vmTester.MapToEntity();

            Assert.AreSame(town, _entity.Town);
        }

        [TestMethod]
        public void TestAssetCategoryCanMapBothWays()
        {
            var ac = GetEntityFactory<AssetCategory>().Create(new { Description = "Foo" });
            _entity.AssetCategory = ac;

            _vmTester.MapToViewModel();

            Assert.AreEqual(ac.Id, _viewModel.AssetCategory);

            _entity.AssetCategory = null;
            _vmTester.MapToEntity();

            Assert.AreSame(ac, _entity.AssetCategory);
        }

        [TestMethod]
        public void TestAssetTypeCanMapBothWays()
        {
            var at = GetFactory<ValveAssetTypeFactory>().Create();
            _entity.AssetType = at;

            _vmTester.MapToViewModel();

            Assert.AreEqual(at.Id, _viewModel.AssetType);

            _entity.AssetType = null;
            _vmTester.MapToEntity();

            Assert.AreSame(at, _entity.AssetType);
        }

        [TestMethod]
        public void TestProposedPipeDiameterCanMapBothWays()
        {
            var pd = GetEntityFactory<PipeDiameter>().Create();
            _entity.ProposedPipeDiameter = pd;

            _vmTester.MapToViewModel();

            Assert.AreEqual(pd.Id, _viewModel.ProposedPipeDiameter);

            _entity.ProposedPipeDiameter = null;
            _vmTester.MapToEntity();

            Assert.AreSame(pd, _entity.ProposedPipeDiameter);
        }

        #endregion
    }
}
