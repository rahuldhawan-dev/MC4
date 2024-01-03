using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Environmental.Models.ViewModels
{
    [TestClass]
    public class CreateChemicalVendorTest : MapCallMvcInMemoryDatabaseTestBase<ChemicalVendor>
    {
        #region Fields

        private ViewModelTester<CreateChemicalVendor, ChemicalVendor> _vmTester;
        private CreateChemicalVendor _viewModel;
        private ChemicalVendor _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = _viewModelFactory.Build<CreateChemicalVendor>();
            _entity = new ChemicalVendor();
            _vmTester = new ViewModelTester<CreateChemicalVendor, ChemicalVendor>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.JdeVendorId);
            _vmTester.CanMapBothWays(x => x.Vendor);
            _vmTester.CanMapBothWays(x => x.OrderContact);
            _vmTester.CanMapBothWays(x => x.PhoneOffice);
            _vmTester.CanMapBothWays(x => x.PhoneCell);
            _vmTester.CanMapBothWays(x => x.Fax);
            _vmTester.CanMapBothWays(x => x.Email);
            _vmTester.CanMapBothWays(x => x.Address);
            _vmTester.CanMapBothWays(x => x.City);
            _vmTester.CanMapBothWays(x => x.State);
            _vmTester.CanMapBothWays(x => x.Zip);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Vendor);
        }

        [TestMethod]
        public void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.Coordinate, GetEntityFactory<Coordinate>().Create());
        }

        #endregion
    }

    [TestClass]
    public class EditChemicalVendorTest : MapCallMvcInMemoryDatabaseTestBase<ChemicalVendor>
    {
        #region Fields

        private ViewModelTester<EditChemicalVendor, ChemicalVendor> _vmTester;
        private EditChemicalVendor _viewModel;
        private ChemicalVendor _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = _viewModelFactory.Build<EditChemicalVendor>();
            _entity = new ChemicalVendor();
            _vmTester = new ViewModelTester<EditChemicalVendor, ChemicalVendor>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.JdeVendorId);
            _vmTester.CanMapBothWays(x => x.Vendor);
            _vmTester.CanMapBothWays(x => x.OrderContact);
            _vmTester.CanMapBothWays(x => x.PhoneOffice);
            _vmTester.CanMapBothWays(x => x.PhoneCell);
            _vmTester.CanMapBothWays(x => x.Fax);
            _vmTester.CanMapBothWays(x => x.Email);
            _vmTester.CanMapBothWays(x => x.Address);
            _vmTester.CanMapBothWays(x => x.City);
            _vmTester.CanMapBothWays(x => x.State);
            _vmTester.CanMapBothWays(x => x.Zip);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Vendor);
        }

        [TestMethod]
        public void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.Coordinate, GetEntityFactory<Coordinate>().Create());
        }

        #endregion
    }
}
