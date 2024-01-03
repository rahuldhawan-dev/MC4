using System;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MMSINC.Data.V2
{
    public interface IUnitOfWork : IDisposable
    {
        #region Abstract Properties

        IContainer Container { get; }

        #endregion

        #region Abstract Methods

        IRepository<T> GetRepository<T>();
        TRepository GetRepository<T, TRepository>() where TRepository : Data.V2.IRepository<T>;

        /// <summary>
        /// USE SPARINGLY!  Remember to parameterize your queries.
        /// </summary>
        ISqlQuery SqlQuery(string query);

        void Commit();
        void Rollback();
        IObjectMapper GetMapper(Type getType, Type type);
        void Flush();

        void Clear();

        T GetInstance<T>();

        #endregion
    }
}
