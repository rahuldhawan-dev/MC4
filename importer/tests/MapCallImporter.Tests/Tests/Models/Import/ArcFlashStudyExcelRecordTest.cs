using System;
using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import;
using MapCallImporter.SampleValues;
using MapCallImporter.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallImporter.Tests.Models.Import
{
    [TestClass]
    public class ArcFlashStudyExcelRecordTest : ExcelRecordTestBase<ArcFlashStudy, MyCreateArcFlashStudy, ArcFlashStudyExcelRecord>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            TestDataHelper.CreateStuffForArcFlashSudiesInAberdeenNJ(_container);
        }

        #endregion

        protected override ArcFlashStudyExcelRecord CreateTarget()
        {
            return new ArcFlashStudyExcelRecord {
                FacilityId = 14,
                UtilityCompany = "Jersey Central Power & Light",
                ArcFlashStatus = "Pending",
                FacilitySize = "Large",
                TransformerKVARating = "25",
                SecondaryVoltage = "120/240",
                PowerPhase = "Single",
                TypeOfArcFlashAnalysis = "Utility Data",
                FacilityTransformerWiringConfiguration = "Wye",
                UtilityCompanyDataReceivedDate = DateTime.Parse("10/1/2016"),
            };
        }

        protected override void InnerTestMappings(ExcelRecordMappingTester<ArcFlashStudy, MyCreateArcFlashStudy, ArcFlashStudyExcelRecord> test)
        {
            test.RequiredEntityRef(x => x.FacilityId, x => x.Facility);

            test.TestedElsewhere(x => x.UtilityCompany);
            test.TestedElsewhere(x => x.ArcFlashStatus);
            test.TestedElsewhere(x => x.FacilitySize);
            test.TestedElsewhere(x => x.TypeOfArcFlashAnalysis);
            test.TestedElsewhere(x => x.LabelType);
            test.TestedElsewhere(x => x.TransformerKVARating);
            test.TestedElsewhere(x => x.SecondaryVoltage);
            test.TestedElsewhere(x => x.PowerPhase);
            test.TestedElsewhere(x => x.FacilityTransformerWiringConfiguration);
            
            test.NotMapped(x => x.FacilityName);

            test.String(x => x.Priority, x => x.Priority);
            test.Boolean(x => x.UtilityCompanyDataReceived, x => x.PowerCompanyDataReceived);
            test.DateTime(x => x.UtilityCompanyDataReceivedDate, x => x.UtilityCompanyDataReceivedDate);
            test.Boolean(x => x.AFHAAnalysisPerformed, x => x.AFHAAnalysisPerformed);
            test.String(x => x.UtilityCompanyOther, x => x.UtilityCompanyOther);
            test.String(x => x.UtilityAccountNumber, x => x.UtilityAccountNumber);
            test.String(x => x.UtilityMeterNumber, x => x.UtilityMeterNumber);
            test.String(x => x.UtilityPoleNumber, x => x.UtilityPoleNumber);
            test.Decimal(x => x.PrimaryVoltageKV, x => x.PrimaryVoltageKV);
            test.Boolean(x => x.TransformerKVAFieldConfirmed, x => x.TransformerKVAFieldConfirmed);
            test.Decimal(x => x.TransformerResistancePercentage, x => x.TransformerResistancePercentage);
            test.Decimal(x => x.TransformerImpedancePercentage, x => x.TransformerReactancePercentage);
            test.Decimal(x => x.PrimaryFuseSize, x => x.PrimaryFuseSize);
            test.String(x => x.PrimaryFuseType, x => x.PrimaryFuseType);
            test.String(x => x.PrimaryFuseManufacturer, x => x.PrimaryFuseManufacturer);
            test.Decimal(x => x.LineToLineFaultAmps, x => x.LineToLineFaultAmps);
            test.Decimal(x => x.LineToLineNeutralFaultAmps, x => x.LineToLineNeutralFaultAmps);
            test.String(x => x.ArcFlashNotes, x => x.ArcFlashNotes);
            test.DateTime(x => x.DateLabelIsApplied, x => x.DateLabelsApplied);
            test.String(x => x.ArcFlashSiteDataCollectionParty, x => x.ArcFlashContractor);
            test.String(x => x.ArcFlashHazardAnalysisStudyParty, x => x.ArcFlashHazardAnalysisStudyParty);
            test.Decimal(x => x.CostToComplete, x => x.CostToComplete);
        }

        #region UtilityCompany

        [TestMethod]
        public void TestUtilityCompanyIsMappedFromUtilityCompany()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(_target.UtilityCompany, _target.MapToEntity(uow, 1, MappingHelper).UtilityCompany.Description);
            });
        }

        [TestMethod]
        public void TestThrowsWhenUtilityCompanyNotFound()
        {
            _target.UtilityCompany = "This is not a valid UtilityCompany!";

            WithUnitOfWork(uow => ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)));
        }

        #endregion

        #region ArcFlashStatus

        [TestMethod]
        public void TestArcFlashStatusIsMappedFromArcFlashStatus()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(_target.ArcFlashStatus, _target.MapToEntity(uow, 1, MappingHelper).ArcFlashStatus.Description);
            });
        }

        [TestMethod]
        public void TestThrowsWhenArcFlashStatusNotFound()
        {
            _target.ArcFlashStatus = "This is not a valid ArcFlashStatus!";

            WithUnitOfWork(uow => ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)));
        }

        #endregion

        #region PowerPhase

        [TestMethod]
        public void TestPowerPhaseIsMappedFromPowerPhase()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(_target.PowerPhase, _target.MapToEntity(uow, 1, MappingHelper).PowerPhase.Description);
            });
        }

        [TestMethod]
        public void TestThrowsWhenPowerPhaseNotFound()
        {
            _target.PowerPhase = "This is not a valid PowerPhase!";

            WithUnitOfWork(uow => ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)));
        }

        [TestMethod]
        public void TestThrowsWhenPowerPhaseNotProvided()
        {
            foreach (var value in new[] { null, " ", string.Empty })
            {
                _target.PowerPhase = value;

                WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
            }
        }

        #endregion

        #region Voltage

        [TestMethod]
        public void TestVoltageIsMappedFromVoltage()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(_target.SecondaryVoltage, _target.MapToEntity(uow, 1, MappingHelper).Voltage.Description);
            });
        }

        [TestMethod]
        public void TestThrowsWhenVoltageNotFound()
        {
            _target.SecondaryVoltage = "This is not a valid Voltage!";

            WithUnitOfWork(uow => ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)));
        }

        [TestMethod]
        public void TestThrowsWhenVoltageNotProvided()
        {
            foreach (var value in new[] { null, " ", string.Empty })
            {
                _target.SecondaryVoltage = value;

                WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
            }
        }

        #endregion

        #region TransformerKVARating

        [TestMethod]
        public void TestTransformerKVARatingIsMappedFromTransformerKVARating()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(_target.TransformerKVARating, _target.MapToEntity(uow, 1, MappingHelper).TransformerKVARating.Description);
            });
        }

        [TestMethod]
        public void TestThrowsWhenTransformerKVARatingNotFound()
        {
            _target.TransformerKVARating = "This is not a valid TransformerKVARating!";

            WithUnitOfWork(uow => ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)));
        }

        [TestMethod]
        public void TestThrowsWhenTransformerKVARatingNotProvided()
        {
            foreach (var value in new[] { null, " ", string.Empty })
            {
                _target.TransformerKVARating = value;

                WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
            }
        }

        #endregion

        #region FacilitySize

        [TestMethod]
        public void TestFacilitySizeIsMappedFromFacilitySize()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(_target.FacilitySize, _target.MapToEntity(uow, 1, MappingHelper).FacilitySize.Description);
            });
        }

        [TestMethod]
        public void TestThrowsWhenFacilitySizeNotFound()
        {
            _target.FacilitySize = "This is not a valid FacilitySize!";

            WithUnitOfWork(uow => ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)));
        }

        #endregion

        #region TypeOfArcFlashAnalysis

        [TestMethod]
        public void TestTypeOfArcFlashAnalysisIsMappedFromTypeOfArcFlashAnalysis()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(_target.TypeOfArcFlashAnalysis, _target.MapToEntity(uow, 1, MappingHelper).TypeOfArcFlashAnalysis.Description);
            });
        }

        [TestMethod]
        public void TestThrowsWhenTypeOfArcFlashAnalysisNotFound()
        {
            _target.TypeOfArcFlashAnalysis = "This is not a valid TypeOfArcFlashAnalysis!";

            WithUnitOfWork(uow => ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)));
        }

        #endregion

        #region LabelType

        [TestMethod]
        public void TestLabelTypeIsMappedFromLabelType()
        {
            _target.LabelType = "Standard Label";

            WithUnitOfWork(uow => {
                Assert.AreEqual(_target.LabelType, _target.MapToEntity(uow, 1, MappingHelper).ArcFlashLabelType.Description);
            });
        }

        [TestMethod]
        public void TestThrowsWhenLabelTypeNotFound()
        {
            _target.LabelType = "This is not a valid LabelType!";

            WithUnitOfWork(uow => ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)));
        }

        [TestMethod]
        public void TestThrowsWhenLabelTypeIsNotSetToStandardLabelAndSecondaryVoltageIsNotProvided()
        {
            _target.LabelType = "Custom Label";
            _target.SecondaryVoltage = string.Empty;

            WithUnitOfWork(uow => ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)));
        }

        [TestMethod]
        public void TestThrowsWhenLabelTypeIsNotSetToStandardLabelAndPowerPhaseIsNotProvided()
        {
            _target.LabelType = "Custom Label";
            _target.PowerPhase = string.Empty;

            WithUnitOfWork(uow => ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)));
        }

        [TestMethod]
        public void TestThrowsWhenLabelTypeIsNotSetToStandardLabelAndTransformerKVARatingIsNotProvided()
        {
            _target.LabelType = "Custom Label";
            _target.TransformerKVARating = string.Empty;

            WithUnitOfWork(uow => ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)));
        }

        #endregion

        #region FacilityTransformerWiringType

        [TestMethod]
        public void TestFacilityTransformerWiringTypeIsMappedFromFacilityTransformerWiringType()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(_target.FacilityTransformerWiringConfiguration, _target.MapToEntity(uow, 1, MappingHelper).FacilityTransformerWiringType.Description);
            });
        }

        [TestMethod]
        public void TestThrowsWhenFacilityTransformerWiringTypeNotFound()
        {
            _target.FacilityTransformerWiringConfiguration = "This is not a valid FacilityTransformerWiringType!";

            WithUnitOfWork(uow => ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)));
        }

        #endregion

        #region UtilityCompanyDataRecieved

        [TestMethod]
        public void TestThrowsWhenUtilityCompanyDataReceivedIsTrueAndUtilityCompanyDataReceivedDataIsNull()
        {
            _target.UtilityCompanyDataReceived = true;
            _target.UtilityCompanyDataReceivedDate = null;

            WithUnitOfWork(uow => ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)));
        }

        #endregion
    }
}
