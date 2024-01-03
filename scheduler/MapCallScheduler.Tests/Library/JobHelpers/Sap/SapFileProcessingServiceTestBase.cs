using log4net;
using MapCallScheduler.Library.Common;
using MapCallScheduler.Library.JobHelpers.Sap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.Library.JobHelpers.Sap
{
    public abstract class SapFileProcessingServiceTestBase<TTarget, TFileService, TUpdaterService>
        where TTarget : SapFileProcessingServiceBase<TFileService, TUpdaterService>
        where TFileService : class, ISapFileService
        where TUpdaterService : class, ISapEntityUpdaterService
    {
        #region Private Members

        protected Mock<TFileService> _fileService;
        protected Mock<TUpdaterService> _updaterService;
        protected IContainer _container;
        protected TTarget _target;

        #endregion

        #region Exposed Methods

        #region Setup/Teardown

        [TestInitialize]
        public virtual void TestInitialize()
        {
            _container = new Container();
            _container.Inject(new Mock<ILog>().Object);
            _container.Inject((_fileService = new Mock<TFileService>(MockBehavior.Strict)).Object);
            _container.Inject((_updaterService = new Mock<TUpdaterService>(MockBehavior.Strict)).Object);

            _target = _container.GetInstance<TTarget>();
        }

        #endregion

        [TestMethod]
        public virtual void TestProcessPassesAndDeletesEachFileFromFileServiceIntoUpdaterService()
        {
            var files = new[] {
                new FileData("foo", null),
                new FileData("bar", null),
            };
            var seq = new MockSequence();

            _fileService.Setup(f => f.GetAllFiles()).Returns(files);
            _updaterService.InSequence(seq).Setup(u => u.Process(files[0]));
            _fileService.InSequence(seq).Setup(f => f.DeleteFile("foo"));
            _updaterService.InSequence(seq).Setup(u => u.Process(files[1]));
            _fileService.InSequence(seq).Setup(f => f.DeleteFile("bar"));

            _target.Process();
        }

        #endregion
    }
}