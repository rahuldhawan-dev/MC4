using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class StockLocationRepository : RepositoryBase<StockLocation>, IStockLocationRepository
    {
        public IEnumerable<StockLocation> FindActiveByOperatingCenter(OperatingCenter operatingCenter)
        {
            return from sl in Linq
                   where sl.IsActive && sl.OperatingCenter == operatingCenter
                   orderby sl.Description
                   select sl;
        }

        public StockLocationRepository(ISession session, IContainer container) : base(session, container) { }
    }

    public interface IStockLocationRepository : IRepository<StockLocation>
    {
        IEnumerable<StockLocation> FindActiveByOperatingCenter(OperatingCenter operatingCenter);
    }
}
