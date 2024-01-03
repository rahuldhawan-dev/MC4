using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.MSTest.TestExtensions;
using Moq;
using NHibernate;
using StructureMap;

namespace MMSINC.CoreTest.Data.NHibernate
{
    [TestClass]
    public class UnitOfWorkTest
    {
        #region Private Members

        private IContainer _container;
        private Mock<ISession> _session;
        private Mock<ITransaction> _transaction;
        private UnitOfWork _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _session = new Mock<ISession>();
            _transaction = new Mock<ITransaction>();
            _session.Setup(x => x.BeginTransaction()).Returns(_transaction.Object);

            _target = new UnitOfWork(_container, _session.Object);
        }

        #endregion

        [TestMethod]
        public void TestBeginsTransaction()
        {
            _session.Verify(x => x.BeginTransaction());
        }

        [TestMethod]
        public void TestConfiguresContainerToUseProvidedSessionForISession()
        {
            Assert.AreSame(_session.Object, _target.Container.GetInstance<ISession>());
        }

        [TestMethod]
        public void TestPreventsCreationOfNewUnitOfWorkFactoriesFromContainer()
        {
            var factory = new Mock<IUnitOfWorkFactory>();
            _container = new Container(i => i.For<IUnitOfWorkFactory>().Use(factory.Object));

            _target = new UnitOfWork(_container, _session.Object);

            MyAssert.Throws(() => _target.Container.GetInstance<IUnitOfWorkFactory>().Build());
        }

        [TestMethod]
        public void TestPreventsCreationOfNewUnitsOfWorkFromContainer()
        {
            _target = new UnitOfWork(_container, _session.Object);

            MyAssert.Throws(() => _target.Container.GetInstance<IUnitOfWork>());
        }
    }
}
