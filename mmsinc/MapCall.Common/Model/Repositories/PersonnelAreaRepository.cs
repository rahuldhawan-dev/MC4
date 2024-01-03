using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IPersonnelAreaRepository : IRepository<PersonnelArea> { }

    public class PersonnelAreaRepository : RepositoryBase<PersonnelArea>, IPersonnelAreaRepository
    {
        #region Constructor

        public PersonnelAreaRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion
    }
}
