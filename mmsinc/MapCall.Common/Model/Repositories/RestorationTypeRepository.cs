using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IRestorationTypeRepository : IRepository<RestorationType>
    {
        IEnumerable<RestorationType> GetCurbTypes();
        IEnumerable<RestorationType> GetNonCurbTypes();
    }

    public class RestorationTypeRepository : RepositoryBase<RestorationType>, IRestorationTypeRepository
    {
        #region Constructor

        public RestorationTypeRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Public Methods

        public IEnumerable<RestorationType> GetCurbTypes()
        {
            return Linq.Where(x => x.Description.StartsWith("CURB"));
        }

        public IEnumerable<RestorationType> GetNonCurbTypes()
        {
            return Linq.Where(x => !x.Description.StartsWith("CURB"));
        }

        #endregion
    }
}
