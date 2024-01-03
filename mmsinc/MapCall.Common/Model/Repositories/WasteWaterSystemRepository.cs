using MapCall.Common.Model.Entities;
using System.Linq;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class WasteWaterSystemRepository : RepositoryBase<WasteWaterSystem>, IWasteWaterSystemRepository
    {
        public WasteWaterSystemRepository(ISession session, IContainer container) : base(session, container) { }

        public override IQueryable<WasteWaterSystem> GetAllSorted()
        {
            return GetAll().OrderBy(x => x.OperatingCenter.OperatingCenterCode).ThenBy(x => x.Id).ThenBy(x => x.WasteWaterSystemName);
        }
    }

    public interface IWasteWaterSystemRepository : IRepository<WasteWaterSystem> { }
}
