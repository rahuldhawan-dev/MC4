using System;
using System.Web.Mvc;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Utilities.StructureMap;
using NHibernate;
using StructureMap;

namespace MMSINC.Data.V2
{
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        #region Properties

        public IContainer Container { get; }
        public ITransaction Transaction { get; }
        public IDisposable DisposableSession { get; }

        #endregion

        #region Constructors

        protected UnitOfWorkBase(IContainer container, ITransaction transaction, IDisposable disposableSession)
        {
            Container = container;
            Transaction = transaction;
            DisposableSession = disposableSession;
            Container.Configure(i => { i.For<ITransaction>().Use(Transaction); });
            SetResolver();
        }

        #endregion

        #region Private Methods

        protected virtual void SetResolver()
        {
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(Container));
        }

        protected virtual void DisposeSession()
        {
            DisposableSession.Dispose();
        }

        #endregion

        #region Abstract Methods

        public abstract MMSINC.Data.V2.IRepository<T> GetRepository<T>();
        public abstract TRepository GetRepository<T, TRepository>() where TRepository : MMSINC.Data.V2.IRepository<T>;
        public abstract ISqlQuery SqlQuery(string query);

        public abstract void Flush();

        public abstract void Clear();

        #endregion

        #region Exposed Methods

        public void Dispose()
        {
            Transaction.Dispose();
            Container.Dispose();
            DisposeSession();
        }

        public void Commit()
        {
            Transaction.Commit();
        }

        public void Rollback()
        {
            Transaction.Rollback();
        }

        public IObjectMapper GetMapper(Type primary, Type secondary)
        {
            return Container.GetInstance<IObjectMapperFactory>().Build(primary, secondary);
        }

        public T GetInstance<T>()
        {
            return Container.GetInstance<T>();
        }

        #endregion
    }
}
