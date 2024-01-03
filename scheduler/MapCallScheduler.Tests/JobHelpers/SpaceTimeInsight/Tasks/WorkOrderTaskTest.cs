using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using log4net;
using MapCall.Common.Model.Entities;
using MapCallScheduler.JobHelpers.SpaceTimeInsight;
using MapCallScheduler.JobHelpers.SpaceTimeInsight.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Moq;
using Newtonsoft.Json;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.SpaceTimeInsight.Tasks
{
    [TestClass]
    public class WorkOrderTaskTest : MapCallSchedulerInMemoryDatabaseTest<WorkOrder>
    {
        #region Private Members

        private Mock<IRepository<WorkOrder>> _repository;
        private Mock<ISpaceTimeInsightJsonFileSerializer> _Serializer;
        private Mock<ISpaceTimeInsightFileUploadService> _uploadService;
        private WorkOrderTask _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _target = _container.GetInstance<WorkOrderTask>();
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);

            _repository = e.For<IRepository<WorkOrder>>().Mock();
            _Serializer = e.For<ISpaceTimeInsightJsonFileSerializer>().Mock();
            _uploadService = e.For<ISpaceTimeInsightFileUploadService>().Mock();
            e.For<IDateTimeProvider>().Use(new TestDateTimeProvider());
            e.For<ILog>().Mock();
        }

        #endregion

        [TestMethod]
        public void TestProcessDoesNotCreateEmptyFile()
        {
            var coll = new List<WorkOrder>();
            var str = "foo";
            _repository.Setup(r => r.Where(It.IsAny<Expression<Func<WorkOrder, bool>>>())).Returns(coll.AsQueryable());

            _target.Run();

            _repository.VerifyAll();
            _Serializer.VerifyAll();
            _uploadService.VerifyAll();
        }

        [TestMethod]
        public void TestProcessPassesWorkOrdersWithLostWaterFromRepositoryToJsonSerializerAndSendsResultsToUploadService()
        {
            var coll = new List<WorkOrder> {new WorkOrder()};
            var str = "foo";
            _repository.Setup(r => r.Where(It.IsAny<Expression<Func<WorkOrder, bool>>>())).Returns(coll.AsQueryable());
            _Serializer.Setup(u => u.SerializeWorkOrders(coll, Formatting.None)).Returns(str);
            _uploadService.Setup(u => u.UploadWorkOrders(str));

            _target.Run();

            _repository.VerifyAll();
            _Serializer.VerifyAll();
            _uploadService.VerifyAll();
        }
    }
}
