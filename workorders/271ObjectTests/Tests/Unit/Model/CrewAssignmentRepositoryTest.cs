using System;
using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for CrewAssignmentRepositoryTestTest
    /// </summary>
    [TestClass]
    public class CrewAssignmentRepositoryTest
    {
        #region Private Members

        private TestCrewAssignmentRepository _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void CrewAssignmentRepositoryTestInitialize()
        {
            _target = new TestCrewAssignmentRepositoryBuilder();
        }

        #endregion

        [TestMethod]
        public void TestGetPendingAssignmentsByCrewAndDateReturnsNullWhenCrewIDOrDateIsNull()
        {
            Assert.IsNull(CrewAssignmentRepository.GetPendingAssignmentsByCrewAndDate(null, DateTime.Today));
            Assert.IsNull(CrewAssignmentRepository.GetPendingAssignmentsByCrewAndDate(1, null));
        }
    }

    internal class TestCrewAssignmentRepositoryBuilder : TestDataBuilder<TestCrewAssignmentRepository>
    {
        #region Exposed Methods

        public override TestCrewAssignmentRepository Build()
        {
            var obj = new TestCrewAssignmentRepository();
            return obj;
        }

        #endregion
    }

    internal class TestCrewAssignmentRepository : CrewAssignmentRepository
    {
    }
}