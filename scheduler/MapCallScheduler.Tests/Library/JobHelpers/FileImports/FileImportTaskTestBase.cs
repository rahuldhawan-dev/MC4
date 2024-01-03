using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using log4net;
using MapCallScheduler.Library.Common;
using MapCallScheduler.Library.JobHelpers.Common;
using MapCallScheduler.Library.JobHelpers.FileImports;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.Library.JobHelpers.FileImports
{
    public abstract class FileImportTaskTestBase<
        TParser,
        TDownloadService,
        TEntity,
        TRepository,
        TTask>
        where TDownloadService : class, IFileDownloadService
        where TParser : class
        where TEntity : class, IEntity, new()
        where TRepository : class, IRepository<TEntity>
        where TTask : IFileImportTask
    {
        #region Private Members

        protected IContainer _container;
        protected TTask _target;
        protected Mock<TDownloadService> _downloadService;
        protected Mock<TParser> _parser;
        protected Mock<TRepository> _repository;

        #endregion

        #region Properties

        protected virtual Expression<Func<TDownloadService, IEnumerable<FileData>>> DownloadMethod
            => x => x.GetAllFiles();

        #endregion

        #region Private Methods

        protected virtual void InitializeContainer(ConfigurationExpression e)
        {
            _downloadService = e.For<TDownloadService>().Mock();
            _parser = e.For<TParser>().Mock();
            _repository = e.For<TRepository>().Mock();
            e.For<ILog>().Mock();
        }

        #endregion

        #region Exposed Methods

        [TestInitialize]
        public virtual void TestInitialize()
        {
            _container = new Container(InitializeContainer);
            _target = _container.GetInstance<TTask>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _downloadService.VerifyAll();
            _parser.VerifyAll();
            _repository.VerifyAll();
        }

        #endregion
    }

    public abstract class FileImportTaskTestBase<
            TParser,
            TDownloadService,
            TEntity,
            TTask>
        : FileImportTaskTestBase<
            TParser,
            TDownloadService,
            TEntity,
            IRepository<TEntity>,
            TTask>
        where TDownloadService : class, IFileDownloadService
        where TParser : class
        where TEntity : class, IEntity, new()
        where TTask : IFileImportTask {}
}
