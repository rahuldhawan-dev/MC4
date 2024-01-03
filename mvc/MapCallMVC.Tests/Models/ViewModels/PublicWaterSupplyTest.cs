using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class CreatePublicWaterSupplyTest : MapCallMvcInMemoryDatabaseTestBase<PublicWaterSupply>
    {
        #region Fields

        private ViewModelTester<CreatePublicWaterSupply, PublicWaterSupply> _vmTester;
        private CreatePublicWaterSupply _viewModel;
        private PublicWaterSupply _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new CreatePublicWaterSupply(_container);
            _entity = new PublicWaterSupply();
            _vmTester = new ViewModelTester<CreatePublicWaterSupply, PublicWaterSupply>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.OperatingArea);
            _vmTester.CanMapBothWays(x => x.System);
            _vmTester.CanMapBothWays(x => x.Identifier);
            _vmTester.CanMapBothWays(x => x.LIMSProfileNumber);
            _vmTester.CanMapBothWays(x => x.LocalCertifiedStateId);
            _vmTester.CanMapBothWays(x => x.UpdatedAt);
            _vmTester.CanMapBothWays(x => x.JanuaryRequiredBacterialWaterSamples);
            _vmTester.CanMapBothWays(x => x.FebruaryRequiredBacterialWaterSamples);
            _vmTester.CanMapBothWays(x => x.MarchRequiredBacterialWaterSamples);
            _vmTester.CanMapBothWays(x => x.AprilRequiredBacterialWaterSamples);
            _vmTester.CanMapBothWays(x => x.MayRequiredBacterialWaterSamples);
            _vmTester.CanMapBothWays(x => x.JuneRequiredBacterialWaterSamples);
            _vmTester.CanMapBothWays(x => x.JulyRequiredBacterialWaterSamples);
            _vmTester.CanMapBothWays(x => x.AugustRequiredBacterialWaterSamples);
            _vmTester.CanMapBothWays(x => x.SeptemberRequiredBacterialWaterSamples);
            _vmTester.CanMapBothWays(x => x.OctoberRequiredBacterialWaterSamples);
            _vmTester.CanMapBothWays(x => x.NovemberRequiredBacterialWaterSamples);
            _vmTester.CanMapBothWays(x => x.DecemberRequiredBacterialWaterSamples);
            _vmTester.CanMapBothWays(x => x.AnticipatedActiveDate);
            _vmTester.CanMapBothWays(x => x.HasConsentOrder);
            _vmTester.CanMapBothWays(x => x.DateOfOwnership);
            _vmTester.CanMapBothWays(x => x.ConsentOrderStartDate);
            _vmTester.CanMapBothWays(x => x.ConsentOrderEndDate);
            _vmTester.CanMapBothWays(x => x.NewSystemInitialSafetyAssessmentCompleted);
            _vmTester.CanMapBothWays(x => x.DateSafetyAssessmentActionItemsCompleted);
            _vmTester.CanMapBothWays(x => x.NewSystemInitialWQEnvAssessmentCompleted);
            _vmTester.CanMapBothWays(x => x.DateWQEnvAssessmentActionItemsCompleted);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Status);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.LocalCertifiedStateId);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AnticipatedActiveDate, Convert.ToDateTime("2/10/2020"), x => x.Status, PublicWaterSupplyStatus.Indices.PENDING);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Ownership);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Type);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.ConsentOrderStartDate, new DateTime(2022, 7, 20), x => x.HasConsentOrder, true);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.ConsentOrderEndDate, new DateTime(2022, 7, 20), x => x.ConsentOrderStartDate, new DateTime(2022, 7, 20));
        }

        [TestMethod]
        public void TestStatusCanMapBothWays()
        {
            var status = GetEntityFactory<PublicWaterSupplyStatus>().Create(new {Description = "Foo"});
            _entity.Status = status;

            _vmTester.MapToViewModel();

            Assert.AreEqual(status.Id, _viewModel.Status);

            _entity.Status = null;
            _vmTester.MapToEntity();

            Assert.AreSame(status, _entity.Status);
        }

        [TestMethod]
        public void TestOwnershipAndTypeCanMapBothWays()
        {
            var ownership = GetEntityFactory<PublicWaterSupplyOwnership>().Create(new { Description = "Foo" });
            var type = GetEntityFactory<PublicWaterSupplyType>().Create(new { Description = "Foo" });
            _entity.Ownership = ownership;
            _entity.Type = type;

            _vmTester.MapToViewModel();

            Assert.AreEqual(ownership.Id, _viewModel.Ownership);
            Assert.AreEqual(type.Id, _viewModel.Type);

            _entity.Ownership = null;
            _entity.Type = null;
            _vmTester.MapToEntity();

            Assert.AreSame(ownership, _entity.Ownership);
            Assert.AreSame(type, _entity.Type);
        }

        [TestMethod]
        public void TestSetDefaultsSetsFreeChlorineReportedToTrue()
        {
            _viewModel.FreeChlorineReported = false;

            _viewModel.SetDefaults();

            Assert.IsTrue(_viewModel.FreeChlorineReported);
        }

        #endregion
    }

    [TestClass]
    public class EditPublicWaterSupplyTest : MapCallMvcInMemoryDatabaseTestBase<PublicWaterSupply>
    {
        #region Fields

        private ViewModelTester<EditPublicWaterSupply, PublicWaterSupply> _vmTester;
        private EditPublicWaterSupply _viewModel;
        private PublicWaterSupply _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new EditPublicWaterSupply(_container);
            _entity = new PublicWaterSupply();
            _vmTester = new ViewModelTester<EditPublicWaterSupply, PublicWaterSupply>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.OperatingArea);
            _vmTester.CanMapBothWays(x => x.System);
            _vmTester.CanMapBothWays(x => x.UpdatedAt);
            _vmTester.CanMapBothWays(x => x.Identifier);
            _vmTester.CanMapBothWays(x => x.JanuaryRequiredBacterialWaterSamples);
            _vmTester.CanMapBothWays(x => x.FebruaryRequiredBacterialWaterSamples);
            _vmTester.CanMapBothWays(x => x.MarchRequiredBacterialWaterSamples);
            _vmTester.CanMapBothWays(x => x.AprilRequiredBacterialWaterSamples);
            _vmTester.CanMapBothWays(x => x.MayRequiredBacterialWaterSamples);
            _vmTester.CanMapBothWays(x => x.JuneRequiredBacterialWaterSamples);
            _vmTester.CanMapBothWays(x => x.JulyRequiredBacterialWaterSamples);
            _vmTester.CanMapBothWays(x => x.AugustRequiredBacterialWaterSamples);
            _vmTester.CanMapBothWays(x => x.SeptemberRequiredBacterialWaterSamples);
            _vmTester.CanMapBothWays(x => x.OctoberRequiredBacterialWaterSamples);
            _vmTester.CanMapBothWays(x => x.NovemberRequiredBacterialWaterSamples);
            _vmTester.CanMapBothWays(x => x.DecemberRequiredBacterialWaterSamples);
            _vmTester.CanMapBothWays(x => x.AnticipatedActiveDate);
            _vmTester.CanMapBothWays(x => x.HasConsentOrder);
            _vmTester.CanMapBothWays(x => x.AnticipatedMergerDate);
            _vmTester.CanMapBothWays(x => x.ValidTo);
            _vmTester.CanMapBothWays(x => x.ValidFrom);
            _vmTester.CanMapBothWays(x => x.DateOfOwnership);
            _vmTester.CanMapBothWays(x => x.ConsentOrderStartDate);
            _vmTester.CanMapBothWays(x => x.ConsentOrderEndDate);
            _vmTester.CanMapBothWays(x => x.NewSystemInitialSafetyAssessmentCompleted);
            _vmTester.CanMapBothWays(x => x.DateSafetyAssessmentActionItemsCompleted);
            _vmTester.CanMapBothWays(x => x.NewSystemInitialWQEnvAssessmentCompleted);
            _vmTester.CanMapBothWays(x => x.DateWQEnvAssessmentActionItemsCompleted);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            var pubWater = GetEntityFactory<PublicWaterSupply>().Create();
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Status);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AnticipatedActiveDate, Convert.ToDateTime("2/10/2020"), x => x.Status, PublicWaterSupplyStatus.Indices.PENDING);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AnticipatedMergerDate, Convert.ToDateTime("2/10/2020"), x => x.Status, PublicWaterSupplyStatus.Indices.PENDING_MERGER);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AnticipatedMergePublicWaterSupply, pubWater.Id, x => x.Status, PublicWaterSupplyStatus.Indices.PENDING_MERGER);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Ownership);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Type);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.ConsentOrderStartDate, new DateTime(2022, 7, 20), x => x.HasConsentOrder, true);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.ConsentOrderEndDate, new DateTime(2022, 7, 20), x => x.ConsentOrderStartDate, new DateTime(2022, 7, 20));
        }

        [TestMethod]
        public void TestStatusCanMapBothWays()
        {
            var status = GetEntityFactory<PublicWaterSupplyStatus>().Create(new { Description = "Foo" });
            _entity.Status = status;
            _vmTester.MapToViewModel();

            Assert.AreEqual(status.Id, _viewModel.Status);

            _entity.Status = null;
            _vmTester.MapToEntity();

            Assert.AreSame(status, _entity.Status);
        }

        [TestMethod]
        public void TestOwnershipAndTypeCanMapBothWays()
        {
            var ownership = GetEntityFactory<PublicWaterSupplyOwnership>().Create(new { Description = "Foo" });
            var type = GetEntityFactory<PublicWaterSupplyType>().Create(new { Description = "Foo" });
            _entity.Ownership = ownership;
            _entity.Type = type;

            _vmTester.MapToViewModel();

            Assert.AreEqual(ownership.Id, _viewModel.Ownership);
            Assert.AreEqual(type.Id, _viewModel.Type);

            _entity.Ownership = null;
            _entity.Type = null;
            _vmTester.MapToEntity();

            Assert.AreSame(ownership, _entity.Ownership);
            Assert.AreSame(type, _entity.Type);
        }

        [TestMethod]
        public void TestMapSetsOperatingCenter()
        {
            var expectedOperatingCenter = GetEntityFactory<OperatingCenter>().Create();
            _entity.OperatingCenterPublicWaterSupplies.Add(new OperatingCenterPublicWaterSupply {
                OperatingCenter = expectedOperatingCenter,
                PublicWaterSupply = _entity
            });

            _viewModel.OperatingCenter = null;
            _vmTester.MapToViewModel();

            Assert.AreEqual(expectedOperatingCenter.Id, _viewModel.OperatingCenter.Single());
        }

        #endregion
    }

    [TestClass]
    public class SearchPublicWaterSupplyTest : InMemoryDatabaseTest<PublicWaterSupply>
    {
        #region Fields

        private SearchPublicWaterSupply _target;
        private PublicWaterSupply _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = new PublicWaterSupply();
            _target = new SearchPublicWaterSupply();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestSearchDoesNotRequireHasConsentOrder()
        {
            ValidationAssert.PropertyIsNotRequired(_target, x => x.HasConsentOrder);
        }

        #endregion
    }
}