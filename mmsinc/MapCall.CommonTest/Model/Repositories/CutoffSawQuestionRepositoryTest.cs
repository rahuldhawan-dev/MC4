using System.Linq;
using MMSINC.Testing.NHibernate;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class CutoffSawQuestionRepositoryTest : InMemoryDatabaseTest<CutoffSawQuestion, CutoffSawQuestionRepository>
    {
        [TestMethod]
        public void TestGetActiveQuestionsReturnsActiveQuestions()
        {
            var factory = GetFactory<CutoffSawQuestionFactory>();
            var questions = factory.CreateList(4);
            var invalid = factory.Create(new {IsActive = false});

            var result = Repository.GetActiveQuestions();

            Assert.AreEqual(questions.Count, result.Count());
            Assert.IsFalse(questions.Contains(invalid));
        }

        [TestMethod]
        public void TestGetActiveQuestionsReturnsQuestionsInOrder()
        {
            var q1 = GetFactory<CutoffSawQuestionFactory>().Create(new {SortOrder = 2});
            var q2 = GetFactory<CutoffSawQuestionFactory>().Create(new {SortOrder = 1});

            var result = Repository.GetActiveQuestions();

            Assert.AreSame(q1, result.Last());
            Assert.AreSame(q2, result.First());
        }
    }
}
