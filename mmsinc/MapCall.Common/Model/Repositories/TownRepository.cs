using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MapCall.Common.Model.Entities;
using NHibernate;
using NHibernate.Transform;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class TownRepository : RepositoryBase<Town>, ITownRepository
    {
        public TownRepository(ISession session, IContainer container) : base(session, container) { }

        public IEnumerable<Town> GetByCountyId(int countyId)
        {
            return (from t in Linq where t.County.Id == countyId select t);
        }

        public IQueryable<Town> GetByOperatingCenterId(int operatingCenterId)
        {
            return Linq.Where(t => t.OperatingCentersTowns.Any(x => x.OperatingCenter.Id == operatingCenterId));
        }

        public IEnumerable<Town> GetByStateId(int stateId)
        {
            return from t in Linq where t.State.Id == stateId select t;
        }

        public IEnumerable<Town> GetWithFacilitiesByStateId(int stateId)
        {
            return (from t in Linq where t.Facilities.Any() && t.State.Id == stateId select t);
        }

        public IEnumerable GetByOperatingCenterIds(int[] ids)
        {
            return (from t in Linq where t.OperatingCentersTowns.Any(x => ids.Contains(x.OperatingCenter.Id)) select t);
        }

        public override IQueryable<Town> GetAllSorted()
        {
            return Linq.OrderBy(x => x.ShortName).ToList().AsQueryable();
        }
    }

    public interface ITownRepository : IRepository<Town>
    {
        IEnumerable<Town> GetByCountyId(int countyId);
        IQueryable<Town> GetByOperatingCenterId(int operatingCenterId);
        IEnumerable<Town> GetByStateId(int stateId);
        IEnumerable<Town> GetWithFacilitiesByStateId(int stateId);
        IEnumerable GetByOperatingCenterIds(int[] ids);
    }
}
