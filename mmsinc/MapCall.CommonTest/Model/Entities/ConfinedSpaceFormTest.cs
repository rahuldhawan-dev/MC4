using System;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class ConfinedSpaceFormTest
    {
        #region Fields

        private ConfinedSpaceForm _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new ConfinedSpaceForm();
        }

        #endregion

        #region Tests

        #region Address

        #endregion

        [TestMethod]
        public void TestIsBeginEntrySectionSignedReturnsTrueIfHazardSignedByIsNotNull()
        {
            _target.BeginEntryAuthorizedBy = null;
            Assert.IsFalse(_target.IsBeginEntrySectionSigned);

            _target.BeginEntryAuthorizedBy = new Employee();
            Assert.IsTrue(_target.IsBeginEntrySectionSigned);
        }

        [TestMethod]
        public void TestIsBumpTestConfrmedReturnsTrueIfHazardSignedByIsNotNull()
        {
            _target.BumpTestConfirmedBy = null;
            Assert.IsFalse(_target.IsBumpTestConfirmed);

            _target.BumpTestConfirmedBy = new Employee();
            Assert.IsTrue(_target.IsBumpTestConfirmed);
        }

        [TestMethod]
        public void TestIsHazardSectionSignedReturnsTrueIfHazardSignedByIsNotNull()
        {
            _target.HazardSignedBy = null;
            Assert.IsFalse(_target.IsHazardSectionSigned);

            _target.HazardSignedBy = new Employee();
            Assert.IsTrue(_target.IsHazardSectionSigned);
        }

        [TestMethod]
        public void TestIsPermitCancelledSectionSignedReturnsTrueIfHazardSignedByIsNotNull()
        {
            _target.PermitCancelledBy = null;
            Assert.IsFalse(_target.IsPermitCancelledSectionSigned);

            _target.PermitCancelledBy = new Employee();
            Assert.IsTrue(_target.IsPermitCancelledSectionSigned);
        }

        [TestMethod]
        public void TestIsReclassificationSectionSignedReturnsTrueIfReclassificationSignedByIsNotNull()
        {
            _target.ReclassificationSignedBy = null;
            Assert.IsFalse(_target.IsReclassificationSectionSigned);

            _target.ReclassificationSignedBy = new Employee();
            Assert.IsTrue(_target.IsReclassificationSectionSigned);
        }

        [TestMethod]
        public void
            TestAtleastOneTestHasAReadingOutsideOfAcceptableConcentrationsReturnsFalseWhenThereAreZeroAtmosphericTests()
        {
            _target.AtmosphericTests.Clear();
            Assert.IsFalse(_target.AtleastOneTestHasAReadingOutsideOfAcceptableConcentrations);
        }

        [TestMethod]
        public void
            TestAtleastOneTestHasAReadingOutsideOfAcceptableConcentrationsReturnsTrueWhenAtleastOneAtmosphericTestHasAReadingOutsideOfAcceptableConcentrations()
        {
            var validTest = new ConfinedSpaceFormAtmosphericTest {
                OxygenPercentageBottom = 20m,
                OxygenPercentageMiddle = 20m,
                OxygenPercentageTop = 20m,
            };
            var invalidTest = new ConfinedSpaceFormAtmosphericTest(); // Defaulting all to zero will be invalid.
            _target.AtmosphericTests.Add(validTest);
            _target.AtmosphericTests.Add(invalidTest);

            Assert.IsTrue(_target.AtleastOneTestHasAReadingOutsideOfAcceptableConcentrations);
        }

        [TestMethod]
        public void TestIsSection5EnabledReturnsTrueOnlyWhenCanBeControlledByVentilationAloneIsFalse()
        {
            _target.CanBeControlledByVentilationAlone = false;
            Assert.IsTrue(_target.IsSection5Enabled);
            _target.CanBeControlledByVentilationAlone = null;
            Assert.IsFalse(_target.IsSection5Enabled);
            _target.CanBeControlledByVentilationAlone = true;
            Assert.IsFalse(_target.IsSection5Enabled);
        }

        #region HasAtLeastOneValidPreEntryAtmosphericTest

        [TestMethod]
        public void TestHasAtLeastOneValidPreEntryAtmosphericTestReturnsFalseIfThereAreNotAnyAtmosphericTests()
        {
            _target.AtmosphericTests.Clear();
            Assert.IsFalse(_target.HasAtLeastOneValidPreEntryAtmosphericTest);
        }

        [TestMethod]
        public void
            TestHasAtLeastOneValidPreEntryAtmosphericTestReturnsTrueWhenThereIsAtLeastOneValidPreEntryAtmosphericTest()
        {
            var validTest = new ConfinedSpaceFormAtmosphericTest {
                OxygenPercentageBottom = 20m,
                OxygenPercentageMiddle = 20m,
                OxygenPercentageTop = 20m,
            };

            var invalidTest = new ConfinedSpaceFormAtmosphericTest(); // Defaulting all to zero will be invalid.
            _target.AtmosphericTests.Add(validTest);
            _target.AtmosphericTests.Add(invalidTest);

            Assert.IsTrue(_target.HasAtLeastOneValidPreEntryAtmosphericTest,
                "This should be true because there is one valid test.");

            _target.AtmosphericTests.Remove(validTest);
            Assert.IsFalse(_target.HasAtLeastOneValidPreEntryAtmosphericTest,
                "This should be false when there are only invalid tests.");
        }

        #endregion

        #endregion
    }
}
