using System;
using MMSINC.Data.V2;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MMSINC.Data
{
    public interface IUnitOfWork : IDisposable
    {
        #region Abstract Properties

        IContainer Container { get; }

        #endregion

        #region Abstract Methods

        NHibernate.IRepository<T> GetRepository<T>();
        TRepository GetRepository<T, TRepository>() where TRepository : NHibernate.IRepository<T>;

        /// <summary>
        /// USE SPARINGLY!  Remember to parameterize your queries.
        /// </summary>
        ISqlQuery SqlQuery(string query);

        void Commit();
        void Rollback();
        IObjectMapper GetMapper(Type getType, Type type);
        void Flush();

        #endregion
    }
}
