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
    public class CreateSampleIdMatrixTest : MapCallMvcInMemoryDatabaseTestBase<SampleIdMatrix>
    {
        #region Fields

        private ViewModelTester<CreateSampleIdMatrix, SampleIdMatrix> _vmTester;
        private CreateSampleIdMatrix _viewModel;
        private SampleIdMatrix _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new CreateSampleIdMatrix(_container);
            _entity = new SampleIdMatrix();
            _vmTester = new ViewModelTester<CreateSampleIdMatrix, SampleIdMatrix>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Parameter);
            _vmTester.CanMapBothWays(x => x.RoutineSample);
            _vmTester.CanMapBothWays(x => x.ProcessStage);
            _vmTester.CanMapBothWays(x => x.ProcessStageSequence);
            _vmTester.CanMapBothWays(x => x.ParameterSequence);
            _vmTester.CanMapBothWays(x => x.SamplePurpose);
            _vmTester.CanMapBothWays(x => x.ProcessReasonForSample);
            _vmTester.CanMapBothWays(x => x.PerformedBy);
            _vmTester.CanMapBothWays(x => x.Frequency);
            _vmTester.CanMapBothWays(x => x.DataStorageLocation);
            _vmTester.CanMapBothWays(x => x.MethodInstrumentLaboratory);
            _vmTester.CanMapBothWays(x => x.TatBellvilleLabHrs);
            _vmTester.CanMapBothWays(x => x.BellevilleSampleId);
            _vmTester.CanMapBothWays(x => x.InterferenceBy);
            _vmTester.CanMapBothWays(x => x.DataStorageLocationOnLineInstrument);
            _vmTester.CanMapBothWays(x => x.IHistorianSignalIdOnLineInstrument);
            _vmTester.CanMapBothWays(x => x.ComplianceReq);
            _vmTester.CanMapBothWays(x => x.ProcessTarget);
            _vmTester.CanMapBothWays(x => x.TriggerPhase1);
            _vmTester.CanMapBothWays(x => x.ActionPhase1);
            _vmTester.CanMapBothWays(x => x.TriggerPhase2);
            _vmTester.CanMapBothWays(x => x.ActionPhase2);
            _vmTester.CanMapBothWays(x => x.Comment);
            _vmTester.CanMapBothWays(x => x.ScadaNotes);
        }

        [TestMethod]
        public void TestSampleSiteCanMapBothWays()
        {
            var ss = GetEntityFactory<SampleSite>().Create(new { CommonSiteName = "Foo"});
            _entity.SampleSite = ss;

            _vmTester.MapToViewModel();

            Assert.AreEqual(ss.Id, _viewModel.SampleSite);

            _entity.SampleSite = null;
            _vmTester.MapToEntity();

            Assert.AreSame(ss, _entity.SampleSite);
        }

        [TestMethod]
        public void TestWaterConstituentCanMapBothWays()
        {
            var wc = GetEntityFactory<WaterConstituent>().Create(new {Description = "Foo"});
            _entity.WaterConstituent = wc;

            _vmTester.MapToViewModel();

            Assert.AreEqual(wc.Id, _viewModel.WaterConstituent);

            _entity.WaterConstituent = null;
            _vmTester.MapToEntity();

            Assert.AreSame(wc, _entity.WaterConstituent);
        }

        #endregion
    }

    [TestClass]
    public class EditSampleIdMatrixTest : MapCallMvcInMemoryDatabaseTestBase<SampleIdMatrix>
    {
        #region Fields

        private ViewModelTester<EditSampleIdMatrix, SampleIdMatrix> _vmTester;
        private EditSampleIdMatrix _viewModel;
        private SampleIdMatrix _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new EditSampleIdMatrix(_container);
            _entity = new SampleIdMatrix();
            _vmTester = new ViewModelTester<EditSampleIdMatrix, SampleIdMatrix>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Parameter);
            _vmTester.CanMapBothWays(x => x.RoutineSample);
            _vmTester.CanMapBothWays(x => x.ProcessStage);
            _vmTester.CanMapBothWays(x => x.ProcessStageSequence);
            _vmTester.CanMapBothWays(x => x.ParameterSequence);
            _vmTester.CanMapBothWays(x => x.SamplePurpose);
            _vmTester.CanMapBothWays(x => x.ProcessReasonForSample);
            _vmTester.CanMapBothWays(x => x.PerformedBy);
            _vmTester.CanMapBothWays(x => x.Frequency);
            _vmTester.CanMapBothWays(x => x.DataStorageLocation);
            _vmTester.CanMapBothWays(x => x.MethodInstrumentLaboratory);
            _vmTester.CanMapBothWays(x => x.TatBellvilleLabHrs);
            _vmTester.CanMapBothWays(x => x.BellevilleSampleId);
            _vmTester.CanMapBothWays(x => x.InterferenceBy);
            _vmTester.CanMapBothWays(x => x.DataStorageLocationOnLineInstrument);
            _vmTester.CanMapBothWays(x => x.IHistorianSignalIdOnLineInstrument);
            _vmTester.CanMapBothWays(x => x.ComplianceReq);
            _vmTester.CanMapBothWays(x => x.ProcessTarget);
            _vmTester.CanMapBothWays(x => x.TriggerPhase1);
            _vmTester.CanMapBothWays(x => x.ActionPhase1);
            _vmTester.CanMapBothWays(x => x.TriggerPhase2);
            _vmTester.CanMapBothWays(x => x.ActionPhase2);
            _vmTester.CanMapBothWays(x => x.Comment);
            _vmTester.CanMapBothWays(x => x.ScadaNotes);
        }

        [TestMethod]
        public void TestSampleSiteCanMapBothWays()
        {
            var ss = GetEntityFactory<SampleSite>().Create(new { CommonSiteName = "Foo" });
            _entity.SampleSite = ss;

            _vmTester.MapToViewModel();

            Assert.AreEqual(ss.Id, _viewModel.SampleSite);

            _entity.SampleSite = null;
            _vmTester.MapToEntity();

            Assert.AreSame(ss, _entity.SampleSite);
        }

        [TestMethod]
        public void TestWaterConstituentCanMapBothWays()
        {
            var wc = GetEntityFactory<WaterConstituent>().Create(new { Description = "Foo" });
            _entity.WaterConstituent = wc;

            _vmTester.MapToViewModel();

            Assert.AreEqual(wc.Id, _viewModel.WaterConstituent);

            _entity.WaterConstituent = null;
            _vmTester.MapToEntity();

            Assert.AreSame(wc, _entity.WaterConstituent);
        }

        #endregion
    }
}
