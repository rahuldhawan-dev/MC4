using log4net;
using MapCallScheduler.Library.Common;
using MapCallScheduler.Library.JobHelpers.Sap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.Library.JobHelpers.Sap
{
    public abstract class SapEntityUpdaterServiceTestBase<TFileRecord, TParser, TEntity, TRepository, TTarget> : MapCallSchedulerInMemoryDatabaseTest<TEntity, TRepository>
        where TTarget : SapEntityUpdaterServiceBase<TFileRecord, TParser, TEntity, TRepository>
        where TFileRecord : new()
        where TParser : class, IFileParser<TFileRecord>
        where TRepository : class, IRepository<TEntity>
        where TEntity : class
    {
        #region Private Members

        protected Mock<TParser> _parser;
        protected TTarget _target;

        #endregion

        #region Private Methods

        protected virtual FileData SetupFileAndRecords(params TFileRecord[] args)
        {
            var file = new FileData(null, null);
            _parser.Setup(p => p.Parse(file)).Returns(args);
            return file;
        }

        protected virtual void CreateTestData()
        {
            // noop
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ILog>().Mock();
            _parser = e.For<TParser>().Mock();
        }

        #endregion

        #region Exposed Methods

        [TestInitialize]
        public void SapEntityUpdaterServiceTestBaseTestInitialize()
        {
            CreateTestData();
            Session.Flush();

            _target = _container.GetInstance<TTarget>();
        }

        #endregion
    }
}
