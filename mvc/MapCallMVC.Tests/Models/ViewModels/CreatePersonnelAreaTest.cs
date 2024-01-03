using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class CreatePersonnelAreaTest : MapCallMvcInMemoryDatabaseTestBase<PersonnelArea>
    {
        #region Fields

        private ViewModelTester<CreatePersonnelArea, PersonnelArea> _vmTester;
        private CreatePersonnelArea _viewModel;
        private PersonnelArea _entity;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
        }

        [TestInitialize]
        public void InitializeTest()
        {
            _entity = new PersonnelArea();
            _viewModel = _viewModelFactory.Build<CreatePersonnelArea, PersonnelArea>( _entity);
            _vmTester = new ViewModelTester<CreatePersonnelArea, PersonnelArea>(_viewModel, _entity);
        }

        #endregion

        #region Mapping

        [TestMethod]
        public void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Description);
            _vmTester.CanMapBothWays(x => x.PersonnelAreaId);
        }

        [TestMethod]
        public void TestOperatingCenterCanMapBothWays()
        {
            var sap = GetFactory<OperatingCenterFactory>().Create();
            _entity.OperatingCenter = sap;
            _viewModel.OperatingCenter = null;
            _vmTester.MapToViewModel();
            Assert.AreEqual(sap.Id, _viewModel.OperatingCenter);

            _entity.OperatingCenter = null;
            _vmTester.MapToEntity();
            Assert.AreSame(sap, _entity.OperatingCenter);
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Description);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PersonnelAreaId, "The Personnel Area Id field is required.");
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
        }

        [TestMethod]
        public void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.OperatingCenter, GetFactory<OperatingCenterFactory>().Create());
        }

        [TestMethod]
        public void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Description, PersonnelArea.MAX_DESCRIPTION_LENGTH);
        }

        [TestMethod]
        public void TestValidationFailsIfPersonnelAreaIdIsNotUnique()
        {
            var otherPersonnelArea = GetEntityFactory<PersonnelArea>().Create(new { PersonnelAreaId = 2154 });
            _viewModel.PersonnelAreaId = 2154;

            ValidationAssert.ModelStateHasError(_viewModel, x => x.PersonnelAreaId, "The given Personnel Area ID is already being used by another Personnel Area.");

            _viewModel.PersonnelAreaId = 9156;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.PersonnelAreaId);
        }

        #endregion
    }
}
