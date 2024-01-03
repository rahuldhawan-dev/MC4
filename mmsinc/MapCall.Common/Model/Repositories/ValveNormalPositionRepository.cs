using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IValveNormalPositionRepository : IRepository<ValveNormalPosition>
    {
        ValveNormalPosition FindByDescription(string description);
    }

    public class ValveNormalPositionRepository : RepositoryBase<ValveNormalPosition>, IValveNormalPositionRepository
    {
        #region Constructor

        public ValveNormalPositionRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Public Methods

        public ValveNormalPosition FindByDescription(string description)
        {
            return Linq.SingleOrDefault(x => x.Description == description);
        }

        #endregion
    }
}
