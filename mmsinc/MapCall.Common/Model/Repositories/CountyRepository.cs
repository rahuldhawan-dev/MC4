using System.Collections.Generic;
using System.Linq;
using MMSINC.Data.NHibernate;
using MapCall.Common.Model.Entities;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class CountyRepository : RepositoryBase<County>, ICountyRepository
    {
        public CountyRepository(ISession session, IContainer container) : base(session, container) { }

        public IEnumerable<County> GetByStateId(int stateId)
        {
            return (from c in Linq where c.State.Id == stateId select c);
        }
    }

    public interface ICountyRepository : IRepository<County>
    {
        IEnumerable<County> GetByStateId(int stateId);
    }
}
