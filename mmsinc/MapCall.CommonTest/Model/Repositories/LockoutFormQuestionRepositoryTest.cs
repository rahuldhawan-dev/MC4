using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class
        LockoutFormQuestionRepositoryTest : InMemoryDatabaseTest<LockoutFormQuestion, IRepository<LockoutFormQuestion>>
    {
        #region Tests

        [TestMethod]
        public void TestGetActiveQuestionsReturnsAllActiveQuestions()
        {
            var categories = GetFactory<LockoutFormQuestionCategoryFactory>().CreateAll();
            foreach (var category in categories)
            {
                GetEntityFactory<LockoutFormQuestion>()
                   .Create(new {Question = "huh?", IsActive = true, Category = category});
                GetEntityFactory<LockoutFormQuestion>()
                   .Create(new {Question = "what?", IsActive = false, Category = category});
            }

            var result = Repository.GetActiveQuestions();

            Assert.AreEqual(4, result.Count());
        }

        [TestMethod]
        public void TestGetActiveQuestionsForCreateOReturnTheCorrectActiveCategories()
        {
            var categories = GetFactory<LockoutFormQuestionCategoryFactory>().CreateAll();
            foreach (var category in categories)
            {
                GetEntityFactory<LockoutFormQuestion>()
                   .Create(new {Question = "huh?", IsActive = true, Category = category});
                GetEntityFactory<LockoutFormQuestion>()
                   .Create(new {Question = "what?", IsActive = false, Category = category});
            }

            var result = Repository.GetActiveQuestionsForCreate();

            foreach (var category in categories)
            {
                if (LockoutFormQuestionCategory.NEW_CATEGORIES.Contains(category.Id))
                {
                    Assert.IsTrue(result.Any(x => x.Category.Id == category.Id));
                }
                else
                {
                    Assert.IsFalse(result.Any(x => x.Category.Id == category.Id));
                }
            }
        }

        #endregion
    }
}
