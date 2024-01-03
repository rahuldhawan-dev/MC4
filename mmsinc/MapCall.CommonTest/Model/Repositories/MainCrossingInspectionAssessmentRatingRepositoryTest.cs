using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class MainCrossingInspectionAssessmentRatingRepositoryTest : InMemoryDatabaseTest<
        MainCrossingInspectionAssessmentRating, MainCrossingInspectionAssessmentRatingRepository>
    {
        #region Tests

        [TestMethod]
        public void TestGetAllReturnsItemsInOrderById()
        {
            var itemOne = GetFactory<MainCrossingInspectionAssessmentRatingFactory>().Create(new {Description = "Z"});
            var itemTwo = GetFactory<MainCrossingInspectionAssessmentRatingFactory>().Create(new {Description = "Q"});
            var itemThree = GetFactory<MainCrossingInspectionAssessmentRatingFactory>().Create(new {Description = "A"});

            Session.Clear();

            var results = Repository.GetAll().ToArray();

            Assert.AreEqual(itemOne.Id, results[0].Id);
            Assert.AreEqual(itemTwo.Id, results[1].Id);
            Assert.AreEqual(itemThree.Id, results[2].Id);
        }

        #endregion
    }
}
