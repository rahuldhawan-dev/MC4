using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels
{
    [TestClass]
    public class CreateBondTest : MapCallMvcInMemoryDatabaseTestBase<Bond>
    {
        #region Fields

        private ViewModelTester<CreateBond, Bond> _vmTester;
        private CreateBond _viewModel;
        private Bond _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new CreateBond(_container);
            _entity = new Bond();
            _vmTester = new ViewModelTester<CreateBond, Bond>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.BondNumber);
            _vmTester.CanMapBothWays(x => x.Principal);
            _vmTester.CanMapBothWays(x => x.Obligee);
            _vmTester.CanMapBothWays(x => x.RecurringBond);
            _vmTester.CanMapBothWays(x => x.BondingAgency);
            _vmTester.CanMapBothWays(x => x.BondValue);
            _vmTester.CanMapBothWays(x => x.AnnualPremium);
            _vmTester.CanMapBothWays(x => x.StartDate);
            _vmTester.CanMapBothWays(x => x.EndDate);
            _vmTester.CanMapBothWays(x => x.PermitsBondId);
            _vmTester.CanMapBothWays(x => x.BondOpen);
        }

        [TestMethod]
        public void TestStateCanMapBothWays()
        {
            var state = GetEntityFactory<State>().Create(new {Name = "Foo"});
            _entity.State = state;

            _vmTester.MapToViewModel();

            Assert.AreEqual(state.Id, _viewModel.State);

            _entity.State = null;
            _vmTester.MapToEntity();

            Assert.AreSame(state, _entity.State);
        }

        [TestMethod]
        public void TestCountyCanMapBothWays()
        {
            var county = GetEntityFactory<County>().Create(new { Name = "Foo" });
            _entity.County = county;

            _vmTester.MapToViewModel();

            Assert.AreEqual(county.Id, _viewModel.County);

            _entity.County = null;
            _vmTester.MapToEntity();

            Assert.AreSame(county, _entity.County);
        }

        [TestMethod]
        public void TestTownCanMapBothWays()
        {
            var town = GetEntityFactory<Town>().Create(new {ShortName = "Foo"});
            _entity.Town = town;

            _vmTester.MapToViewModel();

            Assert.AreEqual(town.Id, _viewModel.Town);

            _entity.Town = null;
            _vmTester.MapToEntity();

            Assert.AreSame(town, _entity.Town);
        }

        [TestMethod]
        public void TestBondTypeCanMapBothWays()
        {
            var bt = GetEntityFactory<BondType>().Create(new {Description = "Foo"});
            _entity.BondType = bt;

            _vmTester.MapToViewModel();

            Assert.AreEqual(bt.Id, _viewModel.BondType);

            _entity.BondType = null;
            _vmTester.MapToEntity();

            Assert.AreSame(bt, _entity.BondType);
        }

        [TestMethod]
        public void TestBondPurposeCanMapBothWays()
        {
            var bp = GetFactory<PerformanceBondPurposeFactory>().Create(new {Description = "Foo"});
            _entity.BondPurpose = bp;

            _vmTester.MapToViewModel();

            Assert.AreEqual(bp.Id, _viewModel.BondPurpose);

            _entity.BondPurpose = null;
            _vmTester.MapToEntity();

            Assert.AreSame(bp, _entity.BondPurpose);
        }

        [TestMethod]
        public void TestOperatingCenterCanMapBothWays()
        {
            var oc = GetEntityFactory<OperatingCenter>().Create();
            _entity.OperatingCenter = oc;

            _vmTester.MapToViewModel();

            Assert.AreEqual(oc.Id, _viewModel.OperatingCenter);

            _entity.OperatingCenter = null;
            _vmTester.MapToEntity();

            Assert.AreSame(oc, _entity.OperatingCenter);
        }

        #endregion
    }

    [TestClass]
    public class EditBondTest : MapCallMvcInMemoryDatabaseTestBase<Bond>
    {
        #region Fields

        private ViewModelTester<EditBond, Bond> _vmTester;
        private EditBond _viewModel;
        private Bond _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new EditBond(_container);
            _entity = new Bond();
            _vmTester = new ViewModelTester<EditBond, Bond>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.BondNumber);
            _vmTester.CanMapBothWays(x => x.Principal);
            _vmTester.CanMapBothWays(x => x.Obligee);
            _vmTester.CanMapBothWays(x => x.RecurringBond);
            _vmTester.CanMapBothWays(x => x.BondingAgency);
            _vmTester.CanMapBothWays(x => x.BondValue);
            _vmTester.CanMapBothWays(x => x.AnnualPremium);
            _vmTester.CanMapBothWays(x => x.StartDate);
            _vmTester.CanMapBothWays(x => x.EndDate);
            _vmTester.CanMapBothWays(x => x.PermitsBondId);
            _vmTester.CanMapBothWays(x => x.BondOpen);
        }
        
        [TestMethod]
        public void TestStateCanMapBothWays()
        {
            var state = GetEntityFactory<State>().Create(new { Name = "Foo" });
            _entity.State = state;

            _vmTester.MapToViewModel();

            Assert.AreEqual(state.Id, _viewModel.State);

            _entity.State = null;
            _vmTester.MapToEntity();

            Assert.AreSame(state, _entity.State);
        }

        [TestMethod]
        public void TestCountyCanMapBothWays()
        {
            var county = GetEntityFactory<County>().Create(new { Name = "Foo" });
            _entity.County = county;

            _vmTester.MapToViewModel();

            Assert.AreEqual(county.Id, _viewModel.County);

            _entity.County = null;
            _vmTester.MapToEntity();

            Assert.AreSame(county, _entity.County);
        }

        [TestMethod]
        public void TestTownCanMapBothWays()
        {
            var town = GetEntityFactory<Town>().Create(new { ShortName = "Foo" });
            _entity.Town = town;

            _vmTester.MapToViewModel();

            Assert.AreEqual(town.Id, _viewModel.Town);

            _entity.Town = null;
            _vmTester.MapToEntity();

            Assert.AreSame(town, _entity.Town);
        }

        [TestMethod]
        public void TestBondTypeCanMapBothWays()
        {
            var bt = GetEntityFactory<BondType>().Create(new { Description = "Foo" });
            _entity.BondType = bt;

            _vmTester.MapToViewModel();

            Assert.AreEqual(bt.Id, _viewModel.BondType);

            _entity.BondType = null;
            _vmTester.MapToEntity();

            Assert.AreSame(bt, _entity.BondType);
        }

        [TestMethod]
        public void TestBondPurposeCanMapBothWays()
        {
            var bp = GetFactory<PerformanceBondPurposeFactory>().Create(new { Description = "Foo" });
            _entity.BondPurpose = bp;

            _vmTester.MapToViewModel();

            Assert.AreEqual(bp.Id, _viewModel.BondPurpose);

            _entity.BondPurpose = null;
            _vmTester.MapToEntity();

            Assert.AreSame(bp, _entity.BondPurpose);
        }

        [TestMethod]
        public void TestOperatingCenterCanMapBothWays()
        {
            var oc = GetEntityFactory<OperatingCenter>().Create();
            _entity.OperatingCenter = oc;

            _vmTester.MapToViewModel();

            Assert.AreEqual(oc.Id, _viewModel.OperatingCenter);

            _entity.OperatingCenter = null;
            _vmTester.MapToEntity();

            Assert.AreSame(oc, _entity.OperatingCenter);
        }

        #endregion
    }
}
