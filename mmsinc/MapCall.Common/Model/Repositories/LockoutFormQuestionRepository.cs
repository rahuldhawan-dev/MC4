using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public static class LockoutFormQuestionRepositoryExtensions
    {
        public static IQueryable<LockoutFormQuestion> GetActiveQuestions(this IRepository<LockoutFormQuestion> that)
        {
            return that.Where(q => q.IsActive);
        }

        public static IQueryable<LockoutFormQuestion> GetActiveQuestionsForCreate(
            this IRepository<LockoutFormQuestion> that)
        {
            return that.GetActiveQuestions()
                       .Where(q => LockoutFormQuestionCategory.NEW_CATEGORIES.Contains(q.Category.Id));
        }
    }
}
