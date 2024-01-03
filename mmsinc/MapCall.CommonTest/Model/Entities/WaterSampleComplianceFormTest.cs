using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class WaterSampleComplianceFormTest
    {
        [TestMethod]
        public void TestCertifiedMonthYearDisplayReturnsCorrectFormat()
        {
            var target = new WaterSampleComplianceForm();
            target.CertifiedMonth = 1;
            target.CertifiedYear = 2018;

            Assert.AreEqual("01/2018", target.CertifiedMonthYearDisplay);

            target.CertifiedMonth = 12;
            Assert.AreEqual("12/2018", target.CertifiedMonthYearDisplay);
        }

        [TestMethod]
        public void TestComplianceResultReturnsExpectedValues()
        {
            var yes = new WaterSampleComplianceFormAnswerType {Id = WaterSampleComplianceFormAnswerType.Indices.YES};
            var no = new WaterSampleComplianceFormAnswerType {Id = WaterSampleComplianceFormAnswerType.Indices.NO};
            var notAvailable = new WaterSampleComplianceFormAnswerType
                {Id = WaterSampleComplianceFormAnswerType.Indices.NOT_AVAILABLE};

            var target = new WaterSampleComplianceForm();
            target.CentralLabSamplesHaveBeenCollected = yes;
            target.CentralLabSamplesHaveBeenReported = yes;
            target.ContractedLabsSamplesHaveBeenCollected = yes;
            target.ContractedLabsSamplesHaveBeenReported = yes;
            target.InternalLabsSamplesHaveBeenCollected = yes;
            target.InternalLabsSamplesHaveBeenReported = yes;
            target.BactiSamplesHaveBeenCollected = yes;
            target.BactiSamplesHaveBeenReported = yes;
            target.LeadAndCopperSamplesHaveBeenCollected = yes;
            target.LeadAndCopperSamplesHaveBeenReported = yes;
            target.WQPSamplesHaveBeenCollected = yes;
            target.WQPSamplesHaveBeenReported = yes;
            target.SurfaceWaterPlantSamplesHaveBeenCollected = yes;
            target.SurfaceWaterPlantSamplesHaveBeenReported = yes;
            target.ChlorineResidualsHaveBeenCollected = yes;
            target.ChlorineResidualsHaveBeenReported = yes;
            Assert.AreEqual(ComplianceResult.EntirelyCompliant, target.ComplianceResult,
                "Should be EntirelyCompliant when ALL questions are YES");

            target.CentralLabSamplesHaveBeenCollected = notAvailable;
            Assert.AreEqual(ComplianceResult.EntirelyCompliant, target.ComplianceResult,
                "Should be EntirelyCompliant when ALL questions are either YES or N/A");

            target.CentralLabSamplesHaveBeenCollected = no;
            Assert.AreEqual(ComplianceResult.PartiallyCompliant, target.ComplianceResult,
                "Should be PartiallyCompliant when ANY questions is NO");

            target.CentralLabSamplesHaveBeenCollected = null;
            Assert.AreEqual(ComplianceResult.NotCompliant, target.ComplianceResult,
                "Should be NotCompliant when ANY questions is NULL");
        }
    }
}
