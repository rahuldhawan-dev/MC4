using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.ProjectManagement.Models.ViewModels
{
    [TestClass]
    public class CreatePipeDataLookupValueTest : MapCallMvcInMemoryDatabaseTestBase<PipeDataLookupValue>
    {
        #region Fields

        private ViewModelTester<CreatePipeDataLookupValue, PipeDataLookupValue> _vmTester;
        private CreatePipeDataLookupValue _viewModel;
        private PipeDataLookupValue _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new CreatePipeDataLookupValue(_container);
            _entity = new PipeDataLookupValue();
            _vmTester = new ViewModelTester<CreatePipeDataLookupValue, PipeDataLookupValue>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.IsDefault);
            _vmTester.CanMapBothWays(x => x.IsEnabled);
            _vmTester.CanMapBothWays(x => x.PriorityWeightedScore);
            _vmTester.CanMapBothWays(x => x.VariableScore);
            _vmTester.CanMapBothWays(x => x.Description);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Description);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IsEnabled);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IsDefault);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PriorityWeightedScore);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.VariableScore);
        }
        
        [TestMethod]
        public void TestPipeDataLookupTypeCanMapBothWays()
        {
            var pdlt = GetEntityFactory<PipeDataLookupType>().Create(new { Description = "Foo" });
            _entity.PipeDataLookupType = pdlt;

            _vmTester.MapToViewModel();

            Assert.AreEqual(pdlt.Id, _viewModel.PipeDataLookupType);

            _entity.PipeDataLookupType = null;
            _vmTester.MapToEntity();

            Assert.AreSame(pdlt, _entity.PipeDataLookupType);
        }

        [TestMethod]
        public void TestMapToEntitySetsAllOtherItemsToDefaultFalseIfDefaultTrue()
        {
            var pdlt = GetEntityFactory<PipeDataLookupType>().Create();
            var pdlv1 = GetEntityFactory<PipeDataLookupValue>().Create(new { PipeDataLookupType = pdlt, IsDefault = true });
            var pdlv2 = GetEntityFactory<PipeDataLookupValue>().Create(new { PipeDataLookupType = pdlt });
            var pdlv3 = GetEntityFactory<PipeDataLookupValue>().Create(new { PipeDataLookupType = pdlt });

            var viewModel = _viewModelFactory.BuildWithOverrides<EditPipeDataLookupValue, PipeDataLookupValue>(pdlv3, new { IsDefault = true });

            viewModel.MapToEntity(pdlv3);

            var repo = _container.GetInstance<IRepository<PipeDataLookupValue>>();
            Assert.IsFalse(repo.Find(pdlv1.Id).IsDefault);
            Assert.IsFalse(repo.Find(pdlv2.Id).IsDefault);
            Assert.IsTrue(repo.Find(pdlv3.Id).IsDefault);
        }

        #endregion
    }

    [TestClass]
    public class EditPipeDataLookupValueTest : MapCallMvcInMemoryDatabaseTestBase<PipeDataLookupValue>
    {
        #region Fields

        private ViewModelTester<EditPipeDataLookupValue, PipeDataLookupValue> _vmTester;
        private EditPipeDataLookupValue _viewModel;
        private PipeDataLookupValue _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new EditPipeDataLookupValue(_container);
            _entity = new PipeDataLookupValue();
            _vmTester = new ViewModelTester<EditPipeDataLookupValue, PipeDataLookupValue>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.IsDefault);
            _vmTester.CanMapBothWays(x => x.IsEnabled);
            _vmTester.CanMapBothWays(x => x.PriorityWeightedScore);
            _vmTester.CanMapBothWays(x => x.VariableScore);
            _vmTester.CanMapBothWays(x => x.Description);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Description);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IsEnabled);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IsDefault);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PriorityWeightedScore);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.VariableScore);
        }

        [TestMethod]
        public void TestMapToEntitySetsAllOtherItemsToDefaultFalseIfDefaultTrue()
        {
            var pdlt = GetEntityFactory<PipeDataLookupType>().Create();
            var pdlv1 = GetEntityFactory<PipeDataLookupValue>().Create(new { PipeDataLookupType = pdlt, IsDefault = true });
            var pdlv2 = GetEntityFactory<PipeDataLookupValue>().Create(new { PipeDataLookupType = pdlt });
            var pdlv3 = GetEntityFactory<PipeDataLookupValue>().Create(new { PipeDataLookupType = pdlt });

            var viewModel = _viewModelFactory.BuildWithOverrides<EditPipeDataLookupValue, PipeDataLookupValue>(pdlv3, new { IsDefault = true });

            viewModel.MapToEntity(pdlv3);

            var repo = _container.GetInstance<IRepository<PipeDataLookupValue>>();
            Assert.IsFalse(repo.Find(pdlv1.Id).IsDefault);
            Assert.IsFalse(repo.Find(pdlv2.Id).IsDefault);
            Assert.IsTrue(repo.Find(pdlv3.Id).IsDefault);
        }

        #endregion
    }
}
