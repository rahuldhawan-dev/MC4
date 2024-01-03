using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IValveOpenDirectionRepository : IRepository<ValveOpenDirection>
    {
        ValveOpenDirection FindByDescription(string description);
    }

    public class ValveOpenDirectionRepository : RepositoryBase<ValveOpenDirection>, IValveOpenDirectionRepository
    {
        #region Constructor

        public ValveOpenDirectionRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Public Methods

        public ValveOpenDirection FindByDescription(string description)
        {
            return Linq.SingleOrDefault(x => x.Description == description);
        }

        #endregion
    }
}
