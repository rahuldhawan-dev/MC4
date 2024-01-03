using System;
using System.Web.Mvc;
using MMSINC.Data.V2;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Utilities.StructureMap;
using NHibernate;
using StructureMap;

namespace MMSINC.Data.NHibernate
{
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        #region Private Members

        private readonly IDependencyResolver _oldResolver;

        #endregion

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
            Container.Configure(i => {
                i.For<ITransaction>().Use(Transaction);
                ConfigureContainer(i);
            });
            _oldResolver = DependencyResolver.Current;
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(Container));
        }

        #endregion

        #region Private Methods

        protected virtual void DisposeSession()
        {
            DisposableSession.Dispose();
        }

        #endregion

        #region Abstract Methods

        protected abstract void ConfigureContainer(ConfigurationExpression i);

        public abstract IRepository<T> GetRepository<T>();
        public abstract TRepository GetRepository<T, TRepository>() where TRepository : IRepository<T>;
        public abstract ISqlQuery SqlQuery(string query);

        public abstract void Flush();

        #endregion

        #region Exposed Methods

        public void Dispose()
        {
            Transaction.Dispose();
            DependencyResolver.SetResolver(_oldResolver);
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

        #endregion
    }
}
