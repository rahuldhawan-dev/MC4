using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.ServiceLineProtection.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.ServiceLineProtection.Models.ViewModels
{
    [TestClass]
    public class CreateServiceLineProtectionInvestigationTest : MapCallMvcInMemoryDatabaseTestBase<ServiceLineProtectionInvestigation>
    {
        #region Fields

        private ViewModelTester<CreateServiceLineProtectionInvestigation, ServiceLineProtectionInvestigation> _vmTester;
        private CreateServiceLineProtectionInvestigation _viewModel;
        private ServiceLineProtectionInvestigation _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new CreateServiceLineProtectionInvestigation(_container);
            _entity = new ServiceLineProtectionInvestigation();
            _vmTester = new ViewModelTester<CreateServiceLineProtectionInvestigation, ServiceLineProtectionInvestigation>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.CustomerName);
            _vmTester.CanMapBothWays(x => x.StreetNumber);
            _vmTester.CanMapBothWays(x => x.CustomerAddress2);
            _vmTester.CanMapBothWays(x => x.CustomerZip);
            _vmTester.CanMapBothWays(x => x.PremiseNumber);
            _vmTester.CanMapBothWays(x => x.AccountNumber);
            _vmTester.CanMapBothWays(x => x.CustomerPhone);
            _vmTester.CanMapBothWays(x => x.TheNotes);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.CustomerName);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Street);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.StreetNumber);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.CustomerCity);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.CustomerZip);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PremiseNumber);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.CustomerServiceMaterial);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.CustomerServiceSize);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.WorkType);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Contractor);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Coordinate);
        }

        [TestMethod]
        public void TestCustomerCityCanMapBothWays()
        {
            var town = GetEntityFactory<Town>().Create(new {ShortName = "Foo"});
            _entity.CustomerCity = town;

            _vmTester.MapToViewModel();

            Assert.AreEqual(town.Id, _viewModel.CustomerCity);

            _entity.CustomerCity = null;
            _vmTester.MapToEntity();

            Assert.AreSame(town, _entity.CustomerCity);
        }

        [TestMethod]
        public void TestOperatingCenterCanMapBothWays()
        {
            var opc = GetEntityFactory<OperatingCenter>().Create();
            _entity.OperatingCenter = opc;

            _vmTester.MapToViewModel();

            Assert.AreEqual(opc.Id, _viewModel.OperatingCenter);

            _entity.OperatingCenter = null;
            _vmTester.MapToEntity();

            Assert.AreSame(opc, _entity.OperatingCenter);
        }

        [TestMethod]
        public void TestServiceMaterialCanMapBothWays()
        {
            var sm = GetEntityFactory<ServiceMaterial>().Create(new {Description = "Foo"});
            _entity.CustomerServiceMaterial = sm;

            _vmTester.MapToViewModel();

            Assert.AreEqual(sm.Id, _viewModel.CustomerServiceMaterial);

            _entity.CustomerServiceMaterial = null;
            _vmTester.MapToEntity();

            Assert.AreSame(sm, _entity.CustomerServiceMaterial);
        }

        [TestMethod]
        public void TestServiceSizeCanMapBothWays()
        {
            var sm = GetEntityFactory<ServiceSize>().Create(new { Description = "Foo" });
            _entity.CustomerServiceSize = sm;

            _vmTester.MapToViewModel();

            Assert.AreEqual(sm.Id, _viewModel.CustomerServiceSize);

            _entity.CustomerServiceSize = null;
            _vmTester.MapToEntity();

            Assert.AreSame(sm, _entity.CustomerServiceSize);
        }

        [TestMethod]
        public void TestWorkTypeCanMapBothWays()
        {
            var wt = GetEntityFactory<ServiceLineProtectionWorkType>().Create(new {Description = "Foo"});
            _entity.WorkType = wt;

            _vmTester.MapToViewModel();

            Assert.AreEqual(wt.Id, _viewModel.WorkType);

            _entity.WorkType = null;
            _vmTester.MapToEntity();

            Assert.AreSame(wt, _entity.WorkType);
        }

        [TestMethod]
        public void TestCoordinateCanMapBothWays()
        {
            var coord = GetEntityFactory<Coordinate>().Create();
            _entity.Coordinate = coord;

            _vmTester.MapToViewModel();

            Assert.AreEqual(coord.Id, _viewModel.Coordinate);

            _entity.Coordinate = null;
            _vmTester.MapToEntity();

            Assert.AreSame(coord, _entity.Coordinate);
        }

        [TestMethod]
        public void TestContractorCanMapBothWays()
        {
            var contractor = GetEntityFactory<Contractor>().Create();
            _entity.Contractor = contractor;

            _vmTester.MapToViewModel();

            Assert.AreEqual(contractor.Id, _viewModel.Contractor);

            _entity.Contractor = null;
            _vmTester.MapToEntity();

            Assert.AreSame(contractor, _entity.Contractor);
        }
        
        [TestMethod]
        public void TestStreetCanMapBothWays()
        {
            var street = GetEntityFactory<Street>().Create(new { FullStName = "Foo" });
            _entity.Street = street;

            _vmTester.MapToViewModel();

            Assert.AreEqual(street.Id, _viewModel.Street);

            _entity.Street = null;
            _vmTester.MapToEntity();

            Assert.AreSame(street, _entity.Street);
        }

        #endregion
    }

    [TestClass]
    public class EditServiceLineProtectionInvestigationTest : MapCallMvcInMemoryDatabaseTestBase<ServiceLineProtectionInvestigation>
    {
        #region Fields

        private ViewModelTester<EditServiceLineProtectionInvestigation, ServiceLineProtectionInvestigation> _vmTester;
        private EditServiceLineProtectionInvestigation _viewModel;
        private ServiceLineProtectionInvestigation _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new EditServiceLineProtectionInvestigation(_container);
            _entity = new ServiceLineProtectionInvestigation();
            _vmTester = new ViewModelTester<EditServiceLineProtectionInvestigation, ServiceLineProtectionInvestigation>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.CustomerName);
            _vmTester.CanMapBothWays(x => x.StreetNumber);
            _vmTester.CanMapBothWays(x => x.CustomerAddress2);
            _vmTester.CanMapBothWays(x => x.CustomerZip);
            _vmTester.CanMapBothWays(x => x.PremiseNumber);
            _vmTester.CanMapBothWays(x => x.AccountNumber);
            _vmTester.CanMapBothWays(x => x.CustomerPhone);
            _vmTester.CanMapBothWays(x => x.TheNotes);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.CustomerName);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.StreetNumber);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Street);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.CustomerCity);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.CustomerZip);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PremiseNumber);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.CustomerServiceMaterial);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.CustomerServiceSize);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.WorkType);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Contractor);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Coordinate);
        }

        [TestMethod]
        public void TestStreetCanMapBothWays()
        {
            var street = GetEntityFactory<Street>().Create(new {FullStName= "Foo"});
            _entity.Street = street;

            _vmTester.MapToViewModel();

            Assert.AreEqual(street.Id, _viewModel.Street);

            _entity.Street = null;
            _vmTester.MapToEntity();

            Assert.AreSame(street, _entity.Street);
        }

        [TestMethod]
        public void TestCustomerCityCanMapBothWays()
        {
            var town = GetEntityFactory<Town>().Create(new { ShortName = "Foo" });
            _entity.CustomerCity = town;

            _vmTester.MapToViewModel();

            Assert.AreEqual(town.Id, _viewModel.CustomerCity);

            _entity.CustomerCity = null;
            _vmTester.MapToEntity();

            Assert.AreSame(town, _entity.CustomerCity);
        }

        [TestMethod]
        public void TestOperatingCenterCanMapBothWays()
        {
            var opc = GetEntityFactory<OperatingCenter>().Create();
            _entity.OperatingCenter = opc;

            _vmTester.MapToViewModel();

            Assert.AreEqual(opc.Id, _viewModel.OperatingCenter);

            _entity.OperatingCenter = null;
            _vmTester.MapToEntity();

            Assert.AreSame(opc, _entity.OperatingCenter);
        }

        [TestMethod]
        public void TestServiceMaterialCanMapBothWays()
        {
            var sm = GetEntityFactory<ServiceMaterial>().Create(new { Description = "Foo" });
            _entity.CustomerServiceMaterial = sm;

            _vmTester.MapToViewModel();

            Assert.AreEqual(sm.Id, _viewModel.CustomerServiceMaterial);

            _entity.CustomerServiceMaterial = null;
            _vmTester.MapToEntity();

            Assert.AreSame(sm, _entity.CustomerServiceMaterial);
        }

        [TestMethod]
        public void TestServiceSizeCanMapBothWays()
        {
            var sm = GetEntityFactory<ServiceSize>().Create(new { Description = "Foo" });
            _entity.CustomerServiceSize = sm;

            _vmTester.MapToViewModel();

            Assert.AreEqual(sm.Id, _viewModel.CustomerServiceSize);

            _entity.CustomerServiceSize = null;
            _vmTester.MapToEntity();

            Assert.AreSame(sm, _entity.CustomerServiceSize);
        }

        [TestMethod]
        public void TestWorkTypeCanMapBothWays()
        {
            var wt = GetEntityFactory<ServiceLineProtectionWorkType>().Create(new { Description = "Foo" });
            _entity.WorkType = wt;

            _vmTester.MapToViewModel();

            Assert.AreEqual(wt.Id, _viewModel.WorkType);

            _entity.WorkType = null;
            _vmTester.MapToEntity();

            Assert.AreSame(wt, _entity.WorkType);
        }

        [TestMethod]
        public void TestCoordinateCanMapBothWays()
        {
            var coord = GetEntityFactory<Coordinate>().Create();
            _entity.Coordinate = coord;

            _vmTester.MapToViewModel();

            Assert.AreEqual(coord.Id, _viewModel.Coordinate);

            _entity.Coordinate = null;
            _vmTester.MapToEntity();

            Assert.AreSame(coord, _entity.Coordinate);
        }

        [TestMethod]
        public void TestContractorCanMapBothWays()
        {
            var contractor = GetEntityFactory<Contractor>().Create();
            _entity.Contractor = contractor;

            _vmTester.MapToViewModel();

            Assert.AreEqual(contractor.Id, _viewModel.Contractor);

            _entity.Contractor = null;
            _vmTester.MapToEntity();

            Assert.AreSame(contractor, _entity.Contractor);
        }

        #endregion
    }
}
