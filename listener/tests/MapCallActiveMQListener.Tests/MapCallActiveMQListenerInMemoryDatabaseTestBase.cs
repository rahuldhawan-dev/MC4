using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallActiveMQListener.Tests
{
    [DeploymentItem(@"x64\SQLite.Interop.dll", "x64")]
    [DeploymentItem(@"x86\SQLite.Interop.dll", "x86")]
    public class MapCallActiveMQListenerInMemoryDatabaseTestBase : InMemoryDatabaseTest<Town>
    {
        #region Private Members

        protected DateTime _now;
        protected TestDateTimeProvider _dateTimeProvider;

        #endregion

        #region Properties

        public virtual IContainer Container => _container;

        #endregion

        #region Constructors

        public MapCallActiveMQListenerInMemoryDatabaseTestBase()
        {
            _dateTimeProvider = new TestDateTimeProvider(_now = DateTime.Now);
        }

        #endregion

        #region Private Methods

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);

            i.AddRegistry<TestDependencyRegistry>();

            i.For<IDateTimeProvider>().Use(_dateTimeProvider);
        }

        #endregion

        #region Exposed Methods

        [DebuggerStepThrough]
        public string GetRelativePath(string filePath)
        {
            return Path.GetFullPath(Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), filePath));
        }

        [DebuggerStepThrough]
        public void ExpectCountChange<TEntity>(int increase, Action fn)
        {
            var countBefore = Session.Query<TEntity>().Count();

            fn();

            var expected = countBefore + increase;
            var actual = Session.Query<TEntity>().Count();
            var difference = expected - actual;

            if (difference == 0)
            {
                return;
            }

            Assert.AreEqual(expected, actual,
                $"{(increase > 0 ? "Increase" : "Decrease")} in count of {typeof(TEntity).Name} records was {(difference > 0 ? "less" : "greater")} than expected.");
        }

        [DebuggerStepThrough]
        public void WithUnitOfWork(Action<IUnitOfWork> fn)
        {
            using (var uow = Container.GetInstance<IUnitOfWorkFactory>().Build())
            {
                fn(uow);
            }
        }

        #endregion
    }

    public class MapCallActiveMQListenerInMemoryDatabaseTestBase<TTarget> : MapCallActiveMQListenerInMemoryDatabaseTestBase
    {
        #region Private Members

        protected TTarget _target;

        #endregion

        #region Private Methods

        [TestInitialize]
        public void MapCallActiveMQListenerInMemoryDatabaseTestBaseTestInitialize()
        {
            _target = CreateTarget();
        }

        protected virtual TTarget CreateTarget()
        {
            return Container.GetInstance<TTarget>();
        }

        #endregion
    }
}