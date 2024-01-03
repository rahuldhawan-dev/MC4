using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class BappTeamRepository : RepositoryBase<BappTeam>, IBappTeamRepository
    {
        public BappTeamRepository(ISession session, IContainer container) : base(session, container) { }

        public IEnumerable<BappTeam> GetByOperatingCenterId(int id)
        {
            return (from t in Linq where t.OperatingCenter.Id == id select t);
        }
    }

    public interface IBappTeamRepository : IRepository<BappTeam>
    {
        IEnumerable<BappTeam> GetByOperatingCenterId(int id);
    }
}
