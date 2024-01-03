using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MMSINC.Data.V2.NHibernate
{
    public class MemoizedUnitOfWork : UnitOfWork
    {
        #region Constructors

        public MemoizedUnitOfWork(IContainer container, ISession session) : base(container,
            new MemoizedSessionWrapper(session)) { }

        #endregion
    }
}
