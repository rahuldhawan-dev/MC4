using System.Collections.Generic;
using System.Linq;
using MMSINC.Data.NHibernate;
using MapCall.Common.Model.Entities;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class MeterRepository : RepositoryBase<Meter>, IMeterRepository
    {
        public MeterRepository(ISession session, IContainer container) : base(session, container) { }

        public IEnumerable<Meter> GetByProfileId(int profileId)
        {
            return (from m in Linq where m.Profile.Id == profileId select m);
        }
    }

    public interface IMeterRepository : IRepository<Meter>
    {
        IEnumerable<Meter> GetByProfileId(int profileId);
    }
}
