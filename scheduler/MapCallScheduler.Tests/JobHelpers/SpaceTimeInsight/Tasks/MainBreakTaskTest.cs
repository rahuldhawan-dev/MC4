using System.Collections.Generic;
using System.Linq;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallScheduler.JobHelpers.SpaceTimeInsight;
using MapCallScheduler.JobHelpers.SpaceTimeInsight.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.ClassExtensions;
using Moq;
using Newtonsoft.Json;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.SpaceTimeInsight.Tasks
{
    [TestClass]
    public class MainBreakTaskTest
    {
        #region Private Members

        private Mock<IMainBreakRepository> _repository;
        private Mock<ISpaceTimeInsightJsonFileSerializer> _serializer;
        private Mock<ISpaceTimeInsightFileUploadService> _uploadService;
        private IContainer _container;
        private MainBreakTask _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container(InitializeContainer);

            _target = _container.GetInstance<MainBreakTask>();
        }

        private void InitializeContainer(ConfigurationExpression e)
        {
            _repository = e.For<IMainBreakRepository>().Mock();
            _serializer = e.For<ISpaceTimeInsightJsonFileSerializer>().Mock();
            _uploadService = e.For<ISpaceTimeInsightFileUploadService>().Mock();
            e.For<ILog>().Mock();
        }

        #endregion

        [TestMethod]
        public void TestProcessDoesNotCreateEmptyFile()
        {
            var coll = new List<MainBreak>();
            var str = "foo";
            _repository.Setup(r => r.GetFromPastDay()).Returns(coll.AsQueryable());

            _target.Run();

            _repository.VerifyAll();
            _serializer.VerifyAll();
            _uploadService.VerifyAll();
        }

        [TestMethod]
        public void TestProcessPassesMainBreaksFromRepositoryToJsonSerializerAndSendsResultsToUploadService()
        {
            var coll = new List<MainBreak> {new MainBreak()};
            var str = "foo";
            _repository.Setup(r => r.GetFromPastDay()).Returns(coll.AsQueryable());
            _serializer.Setup(u => u.SerializeMainBreaks(coll, Formatting.None)).Returns(str);
            _uploadService.Setup(u => u.UploadMainBreaks(str));

            _target.Run();

            _repository.VerifyAll();
            _serializer.VerifyAll();
            _uploadService.VerifyAll();
        }
    }
}