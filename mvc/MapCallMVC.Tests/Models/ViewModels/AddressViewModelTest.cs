using MMSINC.ClassExtensions;
using MMSINC.Testing;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class AddressViewModelTest : MapCallMvcInMemoryDatabaseTestBase<Address>
    {
        private ViewModelTester<AddressViewModel, Address> _vmTester;
        private AddressViewModel _viewModel;
        private Address _entity;

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ITownRepository>().Use<TownRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            var someStates = GetFactory<StateFactory>().CreateList(3);
            _viewModel = new AddressViewModel(_container);
            _entity = GetFactory<AddressFactory>().Create();
            _vmTester = new ViewModelTester<AddressViewModel, Address>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        #region Mapping

        [TestMethod]
        public void TestCanMapStateFromEntityToViewModel()
        {
            _viewModel.State = 0;
            _viewModel.Map(_entity);
            Assert.AreNotEqual(0, _entity.State.Id, "Sanity.");
            Assert.AreEqual(_entity.State.Id, _viewModel.State);
        }

        [TestMethod]
        public void TestCanMapCountyFromEntityToViewModel()
        {
            _viewModel.County = 0;
            _viewModel.Map(_entity);
            Assert.AreNotEqual(0, _entity.County.Id, "Sanity.");
            Assert.AreEqual(_entity.County.Id, _viewModel.County);
        }

        [TestMethod]
        public void TestCanMapTownFromEntityToViewModel()
        {
            _viewModel.Town = 0;
            _viewModel.Map(_entity);
            Assert.AreNotEqual(0, _entity.Town.Id, "Sanity.");
            Assert.AreEqual(_entity.Town.Id, _viewModel.Town);
        }

        [TestMethod]
        public void TestCanMapTownToEntity()
        {
            var town = GetFactory<TownFactory>().Create();
            _viewModel.Town = town.Id;
            _entity.Town = null;
            _viewModel.MapToEntity(_entity);
            Assert.AreSame(town, _entity.Town);
        }
        
        [TestMethod]
        public void TestCanMapAddress1BothWays()
        {
            _vmTester.CanMapBothWays(x => x.Address1);
        }

        [TestMethod]
        public void TestCanMapAddress2BothWays()
        {
            _vmTester.CanMapBothWays(x => x.Address2);
        }

        [TestMethod]
        public void TestCanMapZipCodeBothWays()
        {
            _vmTester.CanMapBothWays(x => x.ZipCode);
        }

        [TestMethod]
        public void TestHasRequiredValuesReturnsTrueForSpecificSituations()
        {
            var target = new AddressViewModel(_container);
            Assert.IsFalse(target.HasRequiredValues());

            target.ZipCode = "00000";
            Assert.IsFalse(target.HasRequiredValues());

            target.Address1 = "blah";
            Assert.IsFalse(target.HasRequiredValues());

            target.Town = 3;
            Assert.IsTrue(target.HasRequiredValues());

            target.ZipCode = null;
            Assert.IsFalse(target.HasRequiredValues());

            target.ZipCode = "00000";
            target.Address1 = null;
            Assert.IsFalse(target.HasRequiredValues());
        }
        
        #endregion

        #region Validation

        [TestMethod]
        public void TestAddress1IsRequired()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Address1);
        }

        [TestMethod]
        public void TestAddress1HasMaxLength()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Address1, Address.StringLengths.ADDRESS_1);
        }

        [TestMethod]
        public void TestAddress2HasMaxLength()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Address2, Address.StringLengths.ADDRESS_2);
        }

        [TestMethod]
        public void TestCountyIdIsRequired()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.County);
        }

        [TestMethod]
        public void TestStateIdIsRequired()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.State);
        }

        [TestMethod]
        public void TestTownIdIsRequired()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Town);
        }

        [TestMethod]
        public void TestTownIdMustBeExistingEntity()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.Town, GetFactory<TownFactory>().Create());
        }

        [TestMethod]
        public void TestZipCodeIsRequired()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ZipCode);
        }

        [TestMethod]
        public void TestZipCodeHasMaxLength()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ZipCode, Address.StringLengths.ZIP_CODE);
        }
        
        #endregion

        #endregion
    }
}
