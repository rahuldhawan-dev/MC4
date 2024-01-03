using System.Collections.Generic;
using System.Linq;
using MMSINC.Data.NHibernate;
using MapCall.Common.Model.Entities;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class FireDistrictRepository : RepositoryBase<FireDistrict>, IFireDistrictRepository
    {
        public FireDistrictRepository(ISession session, IContainer container) : base(session, container) { }

        public IEnumerable<FireDistrict> GetByStateId(int stateId)
        {
            return (from fd in Linq where fd.State.Id == stateId select fd);
        }

        public IEnumerable<FireDistrict> GetByTownId(int townId)
        {
            return (from fd in Linq
                    from towns in fd.TownFireDistricts
                    where towns.Town.Id == townId
                    select fd);
        }
    }

    public interface IFireDistrictRepository : IRepository<FireDistrict>
    {
        IEnumerable<FireDistrict> GetByStateId(int stateId);
        IEnumerable<FireDistrict> GetByTownId(int townId);
    }
}
