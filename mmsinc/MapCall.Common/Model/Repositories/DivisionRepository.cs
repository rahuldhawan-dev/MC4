using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class DivisionRepository : RepositoryBase<Division>, IDivisionRepository
    {
        public DivisionRepository(ISession session, IContainer container) : base(session, container) { }

        public IEnumerable<Division> GetByStateId(int stateId)
        {
            return (from d in Linq where d.State.Id == stateId select d);
        }
    }

    public interface IDivisionRepository : IRepository<Division>
    {
        IEnumerable<Division> GetByStateId(int stateId);
    }
}
