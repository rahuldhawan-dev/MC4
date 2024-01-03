using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMSINC.Data.NHibernate;
using MapCall.Common.Model.Entities;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class CutoffSawQuestionRepository : RepositoryBase<CutoffSawQuestion>, ICutoffSawQuestionRepository
    {
        #region Constructors

        public CutoffSawQuestionRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Exposed Methods

        #endregion

        public IEnumerable<CutoffSawQuestion> GetActiveQuestions()
        {
            return (from q in Linq
                    where q.IsActive
                    orderby q.SortOrder
                    select q);
        }
    }

    public interface ICutoffSawQuestionRepository : IRepository<CutoffSawQuestion>
    {
        IEnumerable<CutoffSawQuestion> GetActiveQuestions();
    }
}
