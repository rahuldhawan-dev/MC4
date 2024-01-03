using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class WaterSystemRepository : RepositoryBase<WaterSystem>, IWaterSystemRepository
    {
        public IQueryable<WaterSystem> GetByOperatingCenterId(int operatingCenterId)
        {
            return (from t in Linq where t.OperatingCenters.Any(x => x.Id == operatingCenterId) select t);
        }

        public WaterSystemRepository(ISession session, IContainer container) : base(session, container) { }
    }

    public interface IWaterSystemRepository : IRepository<WaterSystem>
    {
        IQueryable<WaterSystem> GetByOperatingCenterId(int operatingCenterId);
    }
}
