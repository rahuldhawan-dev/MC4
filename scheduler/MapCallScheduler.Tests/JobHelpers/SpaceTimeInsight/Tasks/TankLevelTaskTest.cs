using System;
using System.Collections.Generic;
using Historian.Data.Client.Entities;
using Historian.Data.Client.Repositories;
using log4net;
using MapCallScheduler.JobHelpers.SpaceTimeInsight;
using MapCallScheduler.JobHelpers.SpaceTimeInsight.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Moq;
using Newtonsoft.Json;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.SpaceTimeInsight.Tasks
{
    [TestClass]
    public class TankLevelTaskTest
    {
        #region Private Members

        private TankLevelTask _target;
        private Mock<IRawDataRepository> _repository;
        private Mock<ISpaceTimeInsightJsonFileSerializer> _serializer;
        private Mock<ISpaceTimeInsightFileUploadService> _uploadService;
        private TestDateTimeProvider _dateTimeProvider;
        private IContainer _container;
        private DateTime _now;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container(InitializeContainer);
            _now = DateTime.Now;

            _target = _container.GetInstance<TankLevelTask>();
        }

        private void InitializeContainer(ConfigurationExpression e)
        {
            _repository = e.For<IRawDataRepository>().Mock();
            _serializer = e.For<ISpaceTimeInsightJsonFileSerializer>().Mock();
            _uploadService = e.For<ISpaceTimeInsightFileUploadService>().Mock();
            e.For<IDateTimeProvider>().Use(_dateTimeProvider = new TestDateTimeProvider(_now = DateTime.Now));
            e.For<ILog>().Mock();
        }

        #endregion

        [TestMethod]
        public void TestProcessDoesNotCreateEmptyFile()
        {
            var coll = new List<RawData>();
            var str = "foo";
            var yesterday = _now.AddDays(-1).Date;
            _repository
                .Setup(r => r.FindByTagName(TankLevelTask.TAG_NAME, false, yesterday, yesterday.EndOfDay()))
                .Returns(coll);

            _target.Run();

            _repository.VerifyAll();
            _serializer.VerifyAll();
            _uploadService.VerifyAll();
        }

        [TestMethod]
        public void TestProcessPassesRawDatumsFromRepositoryToJsonSerializerAndSendsResultsToUploadService()
        {
            var coll = new List<RawData> { new RawData() };
            var str = "foo";
            var yesterday = _now.AddDays(-1).Date;
            _target.ExpectedCount = 1;
            _repository
                .Setup(r => r.FindByTagName(TankLevelTask.TAG_NAME, false, yesterday, yesterday.EndOfDay()))
                .Returns(coll);
            _serializer.Setup(u => u.SerializeTankLevelData(coll, Formatting.None)).Returns(str);
            _uploadService.Setup(u => u.UploadTankLevelData(str));

            _target.Run();

            _repository.VerifyAll();
            _serializer.VerifyAll();
            _uploadService.VerifyAll();
        }

        [TestMethod]
        public void TestProcessPassesRawDatumsFromRepositoryToJsonSerializerAndDoesNotSendResultsToUploadServiceIfCountIsOff()
        {
            var coll = new List<RawData> { new RawData() };
            var str = "foo";
            var yesterday = _now.AddDays(-1).Date;
            _repository
                .Setup(r => r.FindByTagName(TankLevelTask.TAG_NAME, false, yesterday, yesterday.EndOfDay()))
                .Returns(coll);

            _target.Run();

            _serializer.Verify(u => u.SerializeTankLevelData(coll, Formatting.None), Times.Never);
            _uploadService.Verify(u => u.UploadTankLevelData(str), Times.Never);
            _repository.VerifyAll();
            _serializer.VerifyAll();
            _uploadService.VerifyAll();
        }
    }
}