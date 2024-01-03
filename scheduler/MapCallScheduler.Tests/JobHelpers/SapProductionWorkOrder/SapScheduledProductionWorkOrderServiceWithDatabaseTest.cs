using System;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallScheduler.JobHelpers.SapProductionWorkOrder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.SapProductionWorkOrder 
{
    [TestClass]
    public class SapScheduledProductionWorkOrderServiceWithDatabaseTest : MapCallSchedulerInMemoryDatabaseTest<ProductionWorkOrder, IRepository<ProductionWorkOrder>>
    {
        #region Private Members 

        private Mock<ISAPCreatePreventiveWorkOrderRepository> _remoteRepo;
        private Mock<ILog> _log;
        private SapScheduledProductionWorkOrderService _target;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _target = Container.GetInstance<SapScheduledProductionWorkOrderService>();
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ILog>().Use((_log = new Mock<ILog>()).Object);
            e.For<ISAPCreatePreventiveWorkOrderRepository>().Use((_remoteRepo = new Mock<ISAPCreatePreventiveWorkOrderRepository>()).Object);
            e.For<IDateTimeProvider>().Use((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);
        }

        #endregion

        [TestMethod]
        public void TestNHibernateDoesNotErrorWithTheWeirdHasOneRelationship()
        {
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            var order = GetEntityFactory<ProductionWorkOrder>().Create(new { SAPWorkOrder = "000100923374" });
            var expected = new SAPCreatePreventiveWorkOrder { OrderNumber = "000100923374", PlanningPlant = "", Equipment = "1" };
            var results = new SAPCreatePreventiveWorkOrderCollection();
            results.Items.Add(expected);
            
            _remoteRepo.Setup(x => x.Search(It.IsAny<SAPCreatePreventiveWorkOrder>())).Returns(results);

            _target.Process();

            _log.Verify(x => x.Info($"Order#: {expected.OrderNumber} Already exists"), Times.Once);
        }
    }
}
