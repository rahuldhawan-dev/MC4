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
    public class CreatePipeDiameterTest : MapCallMvcInMemoryDatabaseTestBase<PipeDiameter>
    {
        #region Fields

        private ViewModelTester<CreatePipeDiameter, PipeDiameter> _vmTester;
        private CreatePipeDiameter _viewModel;
        private PipeDiameter _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new CreatePipeDiameter(_container);
            _entity = new PipeDiameter();
            _vmTester = new ViewModelTester<CreatePipeDiameter, PipeDiameter>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Diameter);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Diameter);
        }

        #endregion
    }

    [TestClass]
    public class EditPipeDiameterTest : MapCallMvcInMemoryDatabaseTestBase<PipeDiameter>
    {
        #region Fields

        private ViewModelTester<EditPipeDiameter, PipeDiameter> _vmTester;
        private EditPipeDiameter _viewModel;
        private PipeDiameter _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new EditPipeDiameter(_container);
            _entity = new PipeDiameter();
            _vmTester = new ViewModelTester<EditPipeDiameter, PipeDiameter>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Diameter);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Diameter);
        }

        #endregion
    }
}
