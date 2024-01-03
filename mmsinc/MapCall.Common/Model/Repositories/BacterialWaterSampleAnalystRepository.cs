using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IBacterialWaterSampleAnalystRepository : IRepository<BacterialWaterSampleAnalyst>
    {
        IQueryable<BacterialWaterSampleAnalyst> GetAllActiveByOperatingCenter(int operatingCenterId);
        IQueryable<BacterialWaterSampleAnalyst> GetAllByOperatingCenter(int operatingCenterId);
    }

    public class BacterialWaterSampleAnalystRepository : RepositoryBase<BacterialWaterSampleAnalyst>,
        IBacterialWaterSampleAnalystRepository
    {
        #region Constructor

        public BacterialWaterSampleAnalystRepository(ISession session, IContainer container) :
            base(session, container) { }

        #endregion

        #region Public Methods

        public IQueryable<BacterialWaterSampleAnalyst> GetAllActiveByOperatingCenter(int operatingCenterId)
        {
            return Linq.Where(x => x.IsActive && x.OperatingCenters.Select(y => y.Id).Contains(operatingCenterId));
        }

        public IQueryable<BacterialWaterSampleAnalyst> GetAllByOperatingCenter(int operatingCenterId)
        {
            return Linq.Where(x => x.OperatingCenters.Select(y => y.Id).Contains(operatingCenterId));
        }

        #endregion
    }
}
