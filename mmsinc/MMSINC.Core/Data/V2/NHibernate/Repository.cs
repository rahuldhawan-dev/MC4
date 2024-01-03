using NHibernate;

namespace MMSINC.Data.V2.NHibernate
{
    public sealed class Repository<T> : RepositoryBase<T>
    {
        #region Constructors

        public Repository(ISession session) : base(session) { }

        #endregion
    }
}
