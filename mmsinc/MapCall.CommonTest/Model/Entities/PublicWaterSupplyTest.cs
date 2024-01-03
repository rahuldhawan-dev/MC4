using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class PublicWaterSupplyTest : MapCallMvcInMemoryDatabaseTestBase<PublicWaterSupply>
    {
        #region Fields

        private Mock<IDateTimeProvider> _dateTimeProvider;
        private IContainer _container;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _container = new Container();
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _container.Inject(_dateTimeProvider.Object);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestDescriptionReturnsOperatingAreaIdentifier()
        {
            var target = new PublicWaterSupply {Identifier = "123A", OperatingArea = "Op Area"};
            Assert.AreEqual($"123A - Op Area - ", target.Description);
        }

        [TestMethod]
        public void TestDescriptionReturnsOperatingAreaIdentifierAndSystem()
        {
            var target = new PublicWaterSupply {Identifier = "123A", OperatingArea = "Op Area", System = "Foo"};
            Assert.AreEqual($"123A - Op Area - Foo", target.Description);
        }

        [TestMethod]
        public void TestDescriptionRemovesDoubleSpaces()
        {
            var target = new PublicWaterSupply {Identifier = "123A", System = "Foo"};
            Assert.AreEqual($"123A - - Foo", target.Description);
        }

        [TestMethod]
        public void TestHasWaterSampleComplianceFormForTheCurrentMonthReturnsFalseWhenThereAreNoFormsAtAll()
        {
            var target = new PublicWaterSupply();
            _container.BuildUp(target);
            Assert.AreEqual(0, target.WaterSampleComplianceForms.Count, "Sanity");
            Assert.IsFalse(target.HasWaterSampleComplianceFormForTheCurrentMonth);
        }

        [TestMethod]
        public void TestHasWaterSampleComplianceFormForTheCurrentMonth()
        {
            var now = new DateTime(2018, 2, 15);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            var form = new WaterSampleComplianceForm();
            form.CertifiedMonth = 2;
            form.CertifiedYear = 2018;
            var target = new PublicWaterSupply();
            target.WaterSampleComplianceForms.Add(form);
            _container.BuildUp(target);

            Assert.IsTrue(target.HasWaterSampleComplianceFormForTheCurrentMonth);
        }

        [TestMethod]
        public void TestWaterSampleComplianceFormForTheCurrentMonthReturnsForTheCurrentMonth()
        {
            var formFeb = new WaterSampleComplianceForm();
            formFeb.CertifiedMonth = 2;
            formFeb.CertifiedYear = 2018;
            var formMar = new WaterSampleComplianceForm();
            formMar.CertifiedMonth = 3;
            formMar.CertifiedYear = 2018;
            var target = new PublicWaterSupply();
            target.WaterSampleComplianceForms.Add(formFeb);
            target.WaterSampleComplianceForms.Add(formMar);
            _container.BuildUp(target);

            var now = new DateTime(2018, 2, 15);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            Assert.AreSame(formFeb, target.WaterSampleComplianceFormForTheCurrentMonth);

            now = new DateTime(2018, 3, 15);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            Assert.AreSame(formMar, target.WaterSampleComplianceFormForTheCurrentMonth);
        }

        [TestMethod]
        public void TestWaterSampleComplianceFormForTheCurrentMonthThrowsAnExceptionIfThereAreMultipleMatches()
        {
            // There should never be more than one certification for a given month.
            var now = new DateTime(2018, 2, 15);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            var form = new WaterSampleComplianceForm();
            form.CertifiedMonth = 2;
            form.CertifiedYear = 2018;
            var target = new PublicWaterSupply();
            target.WaterSampleComplianceForms.Add(form);
            target.WaterSampleComplianceForms.Add(form); // Add it a second time for duplication

            WaterSampleComplianceForm result = null;
            MyAssert.Throws(() => result = target.WaterSampleComplianceFormForTheCurrentMonth);
        }

        [TestMethod]
        public void TestHasAnticipatedActiveDateHasConsentOrderFields()
        {
            var target = GetEntityFactory<PublicWaterSupply>().Create(new {
                Identifier = "123A",
                AnticipatedActiveDate = DateTime.Parse(@"2/19/2020"),
                HasConsentOrder = false
            });

            Assert.AreEqual(@"2/19/2020 12:00:00 AM", target.AnticipatedActiveDate.ToString());
            Assert.AreEqual(false, target.HasConsentOrder);
        }

        #endregion
    }
}
