using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.WaterQuality.Models.ViewModels
{
    [TestClass]
    public class CreateWaterConstituentTest : MapCallMvcInMemoryDatabaseTestBase<WaterConstituent>
    {
        #region Fields

        private ViewModelTester<CreateWaterConstituent, WaterConstituent> _vmTester;
        private CreateWaterConstituent _viewModel;
        private WaterConstituent _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new CreateWaterConstituent(_container);
            _entity = new WaterConstituent();
            _vmTester = new ViewModelTester<CreateWaterConstituent, WaterConstituent>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Description);
            _vmTester.CanMapBothWays(x => x.Min);
            _vmTester.CanMapBothWays(x => x.Max);
            _vmTester.CanMapBothWays(x => x.Mcl);
            _vmTester.CanMapBothWays(x => x.Mclg);
            _vmTester.CanMapBothWays(x => x.Smcl);
            _vmTester.CanMapBothWays(x => x.ActionLimit);
            _vmTester.CanMapBothWays(x => x.Regulation);
            _vmTester.CanMapBothWays(x => x.SamplingFrequency);
            _vmTester.CanMapBothWays(x => x.SamplingMethod);
            _vmTester.CanMapBothWays(x => x.SampleContainerSizeMl);
            _vmTester.CanMapBothWays(x => x.HoldingTimeHrs);
            _vmTester.CanMapBothWays(x => x.PreservativeQuenchingAgent);
            _vmTester.CanMapBothWays(x => x.AnalyticalMethod);
            _vmTester.CanMapBothWays(x => x.TatBellvileDays);
            _vmTester.CanMapBothWays(x => x.DrinkingWaterContaminantCategory, GetEntityFactory<DrinkingWaterContaminantCategory>().Create());
            _vmTester.CanMapBothWays(x => x.WasteWaterContaminantCategory, GetEntityFactory<WasteWaterContaminantCategory>().Create());
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Description);
        }

        [TestMethod]
        public void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.DrinkingWaterContaminantCategory, GetEntityFactory<DrinkingWaterContaminantCategory>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.WasteWaterContaminantCategory, GetEntityFactory<WasteWaterContaminantCategory>().Create());
        }
        
        #endregion
    }

    [TestClass]
    public class EditWaterConstituentTest : MapCallMvcInMemoryDatabaseTestBase<WaterConstituent>
    {
        #region Fields

        private ViewModelTester<EditWaterConstituent, WaterConstituent> _vmTester;
        private EditWaterConstituent _viewModel;
        private WaterConstituent _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new EditWaterConstituent(_container);
            _entity = new WaterConstituent();
            _vmTester = new ViewModelTester<EditWaterConstituent, WaterConstituent>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Description);
            _vmTester.CanMapBothWays(x => x.Min);
            _vmTester.CanMapBothWays(x => x.Max);
            _vmTester.CanMapBothWays(x => x.Mcl);
            _vmTester.CanMapBothWays(x => x.Mclg);
            _vmTester.CanMapBothWays(x => x.Smcl);
            _vmTester.CanMapBothWays(x => x.ActionLimit);
            _vmTester.CanMapBothWays(x => x.Regulation);
            _vmTester.CanMapBothWays(x => x.SamplingFrequency);
            _vmTester.CanMapBothWays(x => x.SamplingMethod);
            _vmTester.CanMapBothWays(x => x.SampleContainerSizeMl);
            _vmTester.CanMapBothWays(x => x.HoldingTimeHrs);
            _vmTester.CanMapBothWays(x => x.PreservativeQuenchingAgent);
            _vmTester.CanMapBothWays(x => x.AnalyticalMethod);
            _vmTester.CanMapBothWays(x => x.TatBellvileDays);
            _vmTester.CanMapBothWays(x => x.DrinkingWaterContaminantCategory, GetEntityFactory<DrinkingWaterContaminantCategory>().Create());
            _vmTester.CanMapBothWays(x => x.WasteWaterContaminantCategory, GetEntityFactory<WasteWaterContaminantCategory>().Create());
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Description);
        }

        [TestMethod]
        public void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.DrinkingWaterContaminantCategory, GetEntityFactory<DrinkingWaterContaminantCategory>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.WasteWaterContaminantCategory, GetEntityFactory<WasteWaterContaminantCategory>().Create());
        }

        #endregion
    }
}
