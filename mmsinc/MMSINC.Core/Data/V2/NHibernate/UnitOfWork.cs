using NHibernate;
using StructureMap;

namespace MMSINC.Data.V2.NHibernate
{
    public class UnitOfWork : UnitOfWorkBase
    {
        #region Properties

        public ISession Session { get; }

        #endregion

        #region Constructors

        public UnitOfWork(IContainer container, ISession session) : base(container, session.BeginTransaction(), session)
        {
            Session = session;
        }

        #endregion

        #region Exposed Methods

        public override IRepository<T> GetRepository<T>()
        {
            return Container.GetInstance<IRepository<T>>();
        }

        public override ISqlQuery SqlQuery(string query)
        {
            return new SqlQueryWrapper(Session.CreateSQLQuery(query));
        }

        public override void Flush()
        {
            Session.Flush();
        }

        public override TRepository GetRepository<T, TRepository>()
        {
            return Container.GetInstance<TRepository>();
        }

        public override void Clear()
        {
            Session.Clear();
        }

        #endregion
    }
}
