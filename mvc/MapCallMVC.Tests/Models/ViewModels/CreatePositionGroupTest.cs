using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class CreatePositionGroupTest : MapCallMvcInMemoryDatabaseTestBase<PositionGroup>
    {
        #region Fields

        private ViewModelTester<CreatePositionGroup, PositionGroup> _vmTester;
        private CreatePositionGroup _viewModel;
        private PositionGroup _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _entity = new PositionGroup();
            _viewModel = _viewModelFactory.Build<CreatePositionGroup, PositionGroup>( _entity);
            _vmTester = new ViewModelTester<CreatePositionGroup, PositionGroup>(_viewModel, _entity);
        }

        #endregion

        #region Mapping

        [TestMethod]
        public void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.BusinessUnit);
            _vmTester.CanMapBothWays(x => x.BusinessUnitDescription);
            _vmTester.CanMapBothWays(x => x.Group);
            _vmTester.CanMapBothWays(x => x.PositionDescription);
        }

        [TestMethod]
        public void TestCommonNameCanMapBothWays()
        {
            var pg = GetFactory<PositionGroupCommonNameFactory>().Create();
            _entity.CommonName = pg;
            _viewModel.CommonName = null;
            _vmTester.MapToViewModel();
            Assert.AreEqual(pg.Id, _viewModel.CommonName);

            _entity.CommonName = null;
            _vmTester.MapToEntity();
            Assert.AreSame(pg, _entity.CommonName);
        }

        [TestMethod]
        public void TestSAPCompanyCodeCanMapBothWays()
        {
            var sap = GetFactory<SAPCompanyCodeFactory>().Create();
            _entity.SAPCompanyCode = sap;
            _viewModel.SAPCompanyCode = null;
            _vmTester.MapToViewModel();
            Assert.AreEqual(sap.Id, _viewModel.SAPCompanyCode);

            _entity.SAPCompanyCode = null;
            _vmTester.MapToEntity();
            Assert.AreSame(sap, _entity.SAPCompanyCode);
        }

        [TestMethod]
        public void TestStateCanMapBothWays()
        {
            var state = GetFactory<StateFactory>().Create();
            _entity.State = state;
            _viewModel.State = null;
            _vmTester.MapToViewModel();
            Assert.AreEqual(state.Id, _viewModel.State);

            _entity.State = null;
            _vmTester.MapToEntity();
            Assert.AreSame(state, _entity.State);
        }
        

        #endregion

        #region Validation

        [TestMethod]
        public void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.BusinessUnit);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.BusinessUnitDescription);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.CommonName);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Group);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PositionDescription);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.SAPCompanyCode);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.SAPPositionGroupKey);
        }

        [TestMethod]
        public void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.State, GetFactory<StateFactory>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.SAPCompanyCode, GetFactory<SAPCompanyCodeFactory>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.CommonName, GetFactory<PositionGroupCommonNameFactory>().Create());
        }

        [TestMethod]
        public void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.BusinessUnit, PositionGroup.StringLengths.BUSINESS_UNIT);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.BusinessUnitDescription, PositionGroup.StringLengths.BUSINESS_UNIT_DESCRIPTION);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Group, PositionGroup.StringLengths.GROUP);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.PositionDescription, PositionGroup.StringLengths.POSITION_DESCRIPTION);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.SAPPositionGroupKey, PositionGroup.StringLengths.SAP_POSITION_GROUP_KEY);
        }

        [TestMethod]
        public void TestValidateSAPPositionGroupKeyFailsIfAnExistingPositionGroupHasTheSameSAPPositionGroupKey()
        {
            var existing = GetFactory<PositionGroupFactory>().Create(new { SAPPositionGroupKey = "1234" });
            _viewModel.SAPPositionGroupKey = "1234";

            ValidationAssert.ModelStateHasError(_viewModel, x => x.SAPPositionGroupKey, $"Position Group ID#{existing.Id} already has the SAPPositionGroupKey value \"1234\".");

            _viewModel.SAPPositionGroupKey = "2222";
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.SAPPositionGroupKey);
        }

        #endregion
    }
}
