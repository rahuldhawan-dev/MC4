using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class CrewTest
    {
        #region Private Members

        protected Crew _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void CrewTestInitialize()
        {
            _target = new Crew();
            // Beginning of day
            _target.CrewAssignments.Add(new CrewAssignment {AssignedFor = DateTime.Today, Priority = 1});
            // Absolute end of day
            _target.CrewAssignments.Add(new CrewAssignment
                {AssignedFor = DateTime.Today.AddDays(1).AddMilliseconds(-1), Priority = 2});
            // Tomorrow
            _target.CrewAssignments.Add(new CrewAssignment {AssignedFor = DateTime.Today.AddDays(1), Priority = 1});
        }

        #endregion

        [TestMethod]
        public void TestToStringReturnsDescription()
        {
            _target.Description = "this is the description";

            Assert.AreEqual(_target.Description, _target.ToString());
        }

        #region GetMaxPriorityByDate

        [TestMethod]
        public void TestGetMaxPriorityByDateReturnsZeroIfThereAreNoCrewAssignmentsInDateRange()
        {
            Assert.AreEqual(0, _target.GetMaxPriorityByDate(DateTime.Today.AddDays(-1)));
        }

        [TestMethod]
        public void TestGetMaxPriorityReturnsTheMaxPriorityForTheDate()
        {
            Assert.AreEqual(2, _target.GetMaxPriorityByDate(DateTime.Today));
            // Add some arbitrary hours to ensure datespan.
            Assert.AreEqual(2, _target.GetMaxPriorityByDate(DateTime.Today.AddHours(3)));
            Assert.AreEqual(1, _target.GetMaxPriorityByDate(DateTime.Today.AddDays(1)));
        }

        #endregion
    }
}
