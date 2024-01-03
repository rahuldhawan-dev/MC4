using System;
using log4net;
using MapCallScheduler.Library.JobHelpers.FileDumps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.Library.JobHelpers.FileDumps
{
    public abstract class FileDumpTaskTestBase<TSerializer, TUploadService, TEntity, TRepository, TTask>
        where TTask : FileDumpTaskBase<TSerializer, TUploadService, TEntity, TRepository>
        where TRepository : class
        where TSerializer : class
        where TUploadService : class
    {
        #region Private Members

        protected TTask _target;
        protected Mock<TRepository> _repository;
        protected Mock<TSerializer> _serializer;
        protected Mock<TUploadService> _uploadService;
        protected TestDateTimeProvider _dateTimeProvider;
        protected DateTime _now;
        protected IContainer _container;

        #endregion

        #region Private Methods

        protected virtual TTask InitializeTarget()
        {
            return _container.GetInstance<TTask>();
        }

        protected virtual void ConfigureContainer(ConfigurationExpression c)
        {
            _repository = c.For<TRepository>().Mock();
            _serializer = c.For<TSerializer>().Mock();
            _uploadService = c.For<TUploadService>().Mock();
            c.For<IDateTimeProvider>().Use((_dateTimeProvider = new TestDateTimeProvider(_now = DateTime.Now)));
            c.For<ILog>().Mock();
        }

        #endregion

        #region Exposed Methods

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container(ConfigureContainer);

            _target = InitializeTarget();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _repository.VerifyAll();
            _serializer.VerifyAll();
            _uploadService.VerifyAll();
        }

        #endregion
    }
}
