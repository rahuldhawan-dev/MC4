using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class JobSiteCheckListTest
    {
        #region Tests

        [TestMethod]
        public void TestConstructorSetsExcavationsToEmptyList()
        {
            var target = new JobSiteCheckList();
            Assert.IsNotNull(target.Excavations);
            Assert.IsFalse(target.Excavations.Any());
        }

        [TestMethod]
        public void TestExcavationsByDateReturnsExcavationsOrderedByEarliestExcavationDate()
        {
            var target = new JobSiteCheckList();
            var earliest = new JobSiteExcavation {
                ExcavationDate = DateTime.Today.AddDays(-12)
            };
            var latest = new JobSiteExcavation {
                ExcavationDate = DateTime.Today
            };

            target.Excavations.Add(latest);
            target.Excavations.Add(earliest);

            var sorted = target.ExcavationsByDate.ToArray();
            Assert.AreSame(earliest, sorted[0]);
            Assert.AreSame(latest, sorted[1]);
        }

        [TestMethod]
        public void TestMostRecentCommentReturnsNullIfThereAreNoComments()
        {
            Assert.IsNull(new JobSiteCheckList().MostRecentComment);
        }

        [TestMethod]
        public void TestMostRecentCommentReturnsMostRecentComment()
        {
            var target = new JobSiteCheckList();
            var oldest = new JobSiteCheckListComment {
                CreatedAt = DateTime.Today
            };
            var newest = new JobSiteCheckListComment {
                CreatedAt = DateTime.Today.AddDays(1)
            };

            target.Comments.Add(oldest);
            target.Comments.Add(newest);

            Assert.AreSame(newest, target.MostRecentComment);

            // Now test in reverse order to make sure we're not just 
            // passing because the last comment in the list happened
            // to be the one we were expecting.
            target.Comments.Clear();
            target.Comments.Add(newest);
            target.Comments.Add(oldest);

            Assert.AreSame(newest, target.MostRecentComment);
        }

        [TestMethod]
        public void TestMostRecentCrewMembersReturnsNullIfThereAreNoCrewMembers()
        {
            Assert.IsNull(new JobSiteCheckList().MostRecentCrewMembers);
        }

        [TestMethod]
        public void TestMostRecentCrewMembersReturnsMostRecentCrewMembers()
        {
            var target = new JobSiteCheckList();
            var oldest = new JobSiteCheckListCrewMembers {
                CreatedAt = DateTime.Today
            };
            var newest = new JobSiteCheckListCrewMembers {
                CreatedAt = DateTime.Today.AddDays(1)
            };

            target.CrewMembers.Add(oldest);
            target.CrewMembers.Add(newest);

            Assert.AreSame(newest, target.MostRecentCrewMembers);

            // Now test in reverse order to make sure we're not just 
            // passing because the last comment in the list happened
            // to be the one we were expecting.
            target.CrewMembers.Clear();
            target.CrewMembers.Add(newest);
            target.CrewMembers.Add(oldest);

            Assert.AreSame(newest, target.MostRecentCrewMembers);
        }

        #endregion
    }
}
