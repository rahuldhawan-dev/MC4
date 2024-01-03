using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class CreateMaterialTest : MapCallMvcInMemoryDatabaseTestBase<Material>
    {
        #region Fields

        private ViewModelTester<CreateMaterial, Material> _vmTester;
        private CreateMaterial _viewModel;
        private Material _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new CreateMaterial(_container);
            _entity = new Material();
            _vmTester = new ViewModelTester<CreateMaterial, Material>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Description);
            _vmTester.CanMapBothWays(x => x.Size);
            _vmTester.CanMapBothWays(x => x.PartNumber);
            _vmTester.CanMapBothWays(x => x.IsActive);
            _vmTester.CanMapBothWays(x => x.DoNotOrder);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Description);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PartNumber);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IsActive);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.DoNotOrder);
        }

        #endregion
    }

    [TestClass]
    public class EditMaterialTest : InMemoryDatabaseTest<Material>
    {
        #region Fields

        private ViewModelTester<EditMaterial, Material> _vmTester;
        private EditMaterial _viewModel;
        private Material _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new EditMaterial(_container);
            _entity = new Material();
            _vmTester = new ViewModelTester<EditMaterial, Material>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Description);
            _vmTester.CanMapBothWays(x => x.Size);
            _vmTester.CanMapBothWays(x => x.PartNumber);
            _vmTester.CanMapBothWays(x => x.IsActive);
            _vmTester.CanMapBothWays(x => x.DoNotOrder);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Description);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PartNumber);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IsActive);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.DoNotOrder);
        }

        [TestMethod]
        public void TestMapToEntityRemovesOperatingCenterStockedMaterialsWhenIsActiveSetToFalse()
        {
            var operatingCenters = GetFactory<UniqueOperatingCenterFactory>().CreateList(5);
            var material = GetFactory<MaterialFactory>().Create(new {IsActive = true});
            var ocsm1 = GetEntityFactory<OperatingCenterStockedMaterial>().Create(new { Material = material, OperatingCenter = operatingCenters[1]});
            var ocsm2 = GetEntityFactory<OperatingCenterStockedMaterial>().Create(new { Material = material, OperatingCenter = operatingCenters[3]});
            Session.Evict(material);
            Session.Flush();
            Session.Clear();
            material = Session.Load<Material>(material.Id);
            _viewModel.IsActive = true;

            _viewModel.MapToEntity(material);

            Assert.AreEqual(2, material.OperatingCenterStockedMaterials.Count);

            _viewModel.IsActive = false;

            _viewModel.MapToEntity(material);

            Assert.AreEqual(0, material.OperatingCenterStockedMaterials.Count);
        }

        #endregion
    }
}
