using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class RedTagPermitProtectionTypeRepository : RepositoryBase<RedTagPermitProtectionType>, IRedTagPermitProtectionTypeRepository
    {
        #region Constructors

        public RedTagPermitProtectionTypeRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Public Methods

        public override IQueryable<RedTagPermitProtectionType> GetAllSorted() => base.GetAllSorted().OrderBy(x => x.Id);

        #endregion
    }

    public interface IRedTagPermitProtectionTypeRepository : IRepository<RedTagPermitProtectionType> { }
}