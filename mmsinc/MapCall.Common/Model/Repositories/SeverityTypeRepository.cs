using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface ISeverityTypeRepository : IRepository<SeverityType>
    {
        SeverityType GetSeverityTypeById(int id);
    }

    public class SeverityTypeRepository : RepositoryBase<SeverityType>, ISeverityTypeRepository
    {
        #region Constructor

        public SeverityTypeRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Public Methods

        public SeverityType GetSeverityTypeById(int id)
        {
            return Linq.Single(x => x.Id == id);
        }

        #endregion
    }
}
