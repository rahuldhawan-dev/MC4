using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class TailgateTalkTopicRepository : RepositoryBase<TailgateTalkTopic>, ITailgateTalkTopicRepository
    {
        public TailgateTalkTopicRepository(ISession session, IContainer container) : base(session, container) { }

        public IEnumerable<TailgateTalkTopic> FindByCategoryId(int categoryId)
        {
            return from t in Linq where t.Category.Id == categoryId select t;
        }
    }

    public interface ITailgateTalkTopicRepository : IRepository<TailgateTalkTopic>
    {
        IEnumerable<TailgateTalkTopic> FindByCategoryId(int categoryId);
    }
}
