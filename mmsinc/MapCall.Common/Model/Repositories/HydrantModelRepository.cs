using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IHydrantModelRepository : IRepository<HydrantModel>
    {
        IEnumerable<HydrantModel> GetByManufacturerId(int id);
    }

    public class HydrantModelRepository : RepositoryBase<HydrantModel>, IHydrantModelRepository
    {
        #region Constructor

        public HydrantModelRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Public Methods

        public IEnumerable<HydrantModel> GetByManufacturerId(int id)
        {
            return Linq.Where(x => x.HydrantManufacturer.Id == id).ToList();
        }

        #endregion
    }
}
