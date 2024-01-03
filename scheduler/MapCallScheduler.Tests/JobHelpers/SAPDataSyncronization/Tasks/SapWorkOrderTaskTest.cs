using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallScheduler.JobHelpers.SAPDataSyncronization.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Data.NHibernate;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.SAPDataSyncronization.Tasks
{
    [TestClass]
    public class SapWorkOrderTaskTest
    {
        #region Private Members

        private Mock<IRepository<WorkOrder>> _repository;
        private Mock<ISAPWorkOrderRepository> _sapWorkOrderRepository;
        private Mock<ISAPNewServiceInstallationRepository> _sapNewServiceInstallationRepository;
        private Mock<ILog> _log;
        private IContainer _container;
        private SAPWorkOrderTask _target;

        #endregion

        #region Setup/Teardown

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();

            _container.Inject((_repository = new Mock<IRepository<WorkOrder>>()).Object);
            _container.Inject((_sapWorkOrderRepository = new Mock<ISAPWorkOrderRepository>()).Object);
            _container.Inject((_sapNewServiceInstallationRepository = new Mock<ISAPNewServiceInstallationRepository>()).Object);
            _container.Inject((_log = new Mock<ILog>()).Object);

            _target = _container.GetInstance<SAPWorkOrderTask>();
        }

        #endregion

        #endregion

        [TestMethod]
        public void TestProcessSetsSapErrorCodeWhenErrorOcurrsWhileUpdating()
        {
            var sapWorkOrderStep = new SAPWorkOrderStep { Id = SAPWorkOrderStep.Indices.UPDATE };
            var workOrder = new WorkOrder { SAPErrorCode = "RETRY", SAPWorkOrderStep = sapWorkOrderStep };
            var workOrders = new List<WorkOrder> { workOrder };
            _repository.Setup(x => x.Save(workOrder));
            _repository.Setup(x => x.Where(It.IsAny<Expression<Func<WorkOrder, bool>>>())).Returns(workOrders.AsQueryable());
            var sapWorkOrder = new SAPProgressWorkOrder { Status = "something went wrong", SAPNotificationNo = "1234", OrderNumber = "1231" };
            _sapWorkOrderRepository.Setup(x => x.Update(It.IsAny<SAPProgressWorkOrder>())).Throws<NullReferenceException>();

            _target.Run();

            _repository.Verify(x => x.Save(It.Is<WorkOrder>(wo => wo.SAPErrorCode == "RETRY::UPDATE FAILURE: Object reference not set to an instance of an object.")), Times.Once);
        }
        
        [TestMethod]
        public void TestProcessSetsSapErrorCodeWhenErrorOcurrsWhileCreating()
        {
            var sapWorkOrderStep = new SAPWorkOrderStep { Id = SAPWorkOrderStep.Indices.CREATE };
            var workOrder = new WorkOrder {SAPErrorCode = "RETRY", SAPWorkOrderStep = sapWorkOrderStep };
            var workOrders = new List<WorkOrder> {workOrder};
            _repository.Setup(x => x.Save(workOrder));
            _repository.Setup(x => x.Where(It.IsAny<Expression<Func<WorkOrder, bool>>>())).Returns(workOrders.AsQueryable());
            var sapWorkOrder = new SAPWorkOrder { SAPErrorCode = "error", SAPNotificationNo = "1234", OrderNumber = "1231"};
            _sapWorkOrderRepository.Setup(x => x.Save(It.IsAny<SAPWorkOrder>())).Returns(sapWorkOrder);

            _target.Run();

            _repository.Verify(x => x.Save(It.Is<WorkOrder>(wo =>
                wo.SAPErrorCode == sapWorkOrder.SAPErrorCode &&
                wo.SAPWorkOrderStep.Id == SAPWorkOrderStep.Indices.CREATE)), Times.Once);

            _log.Verify(x => x.Info($"Completed Processing (1/1) SAP WorkOrders. 0 Exceptions"));
        }
        
        [TestMethod]
        public void TestProcessSetsSAPNumbersWhenNoErrorsHaveOccurredWhileCompleting()
        {
            var sapWorkOrderStep = new SAPWorkOrderStep { Id = SAPWorkOrderStep.Indices.COMPLETE };
            var workOrder = new WorkOrder { SAPErrorCode = "RETRY", SAPWorkOrderStep = sapWorkOrderStep };
            var workOrders = new List<WorkOrder> { workOrder };
            _repository.Setup(x => x.Save(workOrder));
            _repository.Setup(x => x.Where(It.IsAny<Expression<Func<WorkOrder, bool>>>())).Returns(workOrders.AsQueryable());
            var sapWorkOrder = new SAPCompleteWorkOrder { Status = "everything's fine" };
            _sapWorkOrderRepository.Setup(x => x.Complete(It.IsAny<SAPCompleteWorkOrder>())).Returns(sapWorkOrder);

            _target.Run();

            _repository.Verify(x => x.Save(It.Is<WorkOrder>(wo =>
                wo.SAPErrorCode == sapWorkOrder.Status)), Times.Once);

            _log.Verify(x => x.Info($"Completed Processing (1/1) SAP WorkOrders. 0 Exceptions"));
        }

        [TestMethod]
        public void TestProcessSetsSAPNumbersWhenNoErrorsHaveOccurredWhileCreating()
        {
            var sapWorkOrderStep = new SAPWorkOrderStep { Id = SAPWorkOrderStep.Indices.CREATE };
            var workOrder = new WorkOrder {SAPErrorCode = "RETRY", SAPWorkOrderStep = sapWorkOrderStep };
            var workOrders = new List<WorkOrder> {workOrder};
            _repository.Setup(x => x.Save(workOrder));
            _repository.Setup(x => x.Where(It.IsAny<Expression<Func<WorkOrder, bool>>>())).Returns(workOrders.AsQueryable());
            var sapWorkOrder = new SAPWorkOrder { SAPErrorCode = "everything's fine successfully", SAPNotificationNo = "1234", OrderNumber = "1231"};
            _sapWorkOrderRepository.Setup(x => x.Save(It.IsAny<SAPWorkOrder>())).Returns(sapWorkOrder);

            _target.Run();

            _repository.Verify(x => x.Save(It.Is<WorkOrder>(wo => 
                wo.SAPErrorCode == sapWorkOrder.SAPErrorCode &&
                wo.SAPWorkOrderStep.Id == SAPWorkOrderStep.Indices.UPDATE)), Times.Once);

            _log.Verify(x => x.Info($"Completed Processing (1/1) SAP WorkOrders. 0 Exceptions"));
        }

        [TestMethod]
        public void TestProcessSetsSAPNumbersWhenNoErrorsHaveOccurredWhileStocking()
        {
            var sapWorkOrderStep = new SAPWorkOrderStep { Id = SAPWorkOrderStep.Indices.APPROVE_GOODS };
            var workOrder = new WorkOrder { SAPErrorCode = "RETRY", SAPWorkOrderStep = sapWorkOrderStep };
            workOrder.MaterialsUsed.Add(new MaterialUsed { Material = new Material { PartNumber = "213" } });
            var workOrders = new List<WorkOrder> { workOrder };
            _repository.Setup(x => x.Save(workOrder));
            _repository.Setup(x => x.Where(It.IsAny<Expression<Func<WorkOrder, bool>>>())).Returns(workOrders.AsQueryable());
            var sapWorkOrder = new SAPGoodsIssue { Status = "everything's fine", MaterialDocument = "1231" };
            
            var goodsCollection = new SAPGoodsIssueCollection();
            goodsCollection.Items.Add(sapWorkOrder);
            _sapWorkOrderRepository.Setup(x => x.Approve(It.IsAny<SAPGoodsIssue>())).Returns(goodsCollection);

            _target.Run();

            _repository.Verify(x => x.Save(It.Is<WorkOrder>(wo =>
                wo.SAPErrorCode == sapWorkOrder.Status &&
                wo.MaterialsDocID == sapWorkOrder.MaterialDocument)), Times.Once);

            _log.Verify(x => x.Info($"Completed Processing (1/1) SAP WorkOrders. 0 Exceptions"));
        }

        [TestMethod]
        public void TestProcessSetsSAPNumbersWhenNoErrorsHaveOccurredWhileUpdating()
        {
            var sapWorkOrderStep = new SAPWorkOrderStep { Id = SAPWorkOrderStep.Indices.UPDATE };
            var workOrder = new WorkOrder { SAPErrorCode = "RETRY", SAPWorkOrderStep = sapWorkOrderStep, WorkDescription = new WorkDescription { Id = (int)WorkDescription.Indices.MISC_REPAIR }};
            var workOrders = new List<WorkOrder> { workOrder };
            _repository.Setup(x => x.Save(workOrder));
            _repository.Setup(x => x.Where(It.IsAny<Expression<Func<WorkOrder, bool>>>())).Returns(workOrders.AsQueryable());
            var sapWorkOrder = new SAPProgressWorkOrder { Status = "everything's fine", SAPNotificationNo = "1234", OrderNumber = "1231" };
            _sapWorkOrderRepository.Setup(x => x.Update(It.IsAny<SAPProgressWorkOrder>())).Returns(sapWorkOrder);

            _target.Run();

            _repository.Verify(x => x.Save(It.Is<WorkOrder>(wo =>
                wo.SAPErrorCode == sapWorkOrder.Status)), Times.Once);

            _log.Verify(x => x.Info($"Completed Processing (1/1) SAP WorkOrders. 0 Exceptions"));
        }

        [TestMethod]
        public void TestProcessSetsSAPNumbersWhenNoErrorsHaveOccurredWhileUpdatingWithNMI()
        {
            var workDescription = new WorkDescription { Description = "SERVICE LINE INSTALLATION" };
            workDescription.SetPropertyValueByName("Id", WorkDescriptionRepository.NEW_SERVICE_INSTALLATIONS[0]);
            var sapWorkOrderStep = new SAPWorkOrderStep { Id = SAPWorkOrderStep.Indices.UPDATE_WITH_NMI };
            var workOrder = new WorkOrder { SAPErrorCode = "RETRY", SAPWorkOrderStep = sapWorkOrderStep, WorkDescription = workDescription, DateCompleted = DateTime.Now };
            var si = new ServiceInstallation {WorkOrder = workOrder};
            workOrder.ServiceInstallations.Add(si);
            var workOrders = new List<WorkOrder> { workOrder };
            _repository.Setup(x => x.Save(workOrder));
            _repository.Setup(x => x.Where(It.IsAny<Expression<Func<WorkOrder, bool>>>())).Returns(workOrders.AsQueryable());
            var sapWorkOrder = new SAPProgressWorkOrder { Status = "everything's fine", SAPNotificationNo = "1234", OrderNumber = "1231" };
            _sapWorkOrderRepository.Setup(x => x.Update(It.IsAny<SAPProgressWorkOrder>())).Returns(sapWorkOrder);
            var sapNewServiceInstallation = new SAPNewServiceInstallation { SAPStatus = "should be this status"};
            _sapNewServiceInstallationRepository.Setup(x => x.Save(It.IsAny<SAPNewServiceInstallation>()))
                                                .Returns(sapNewServiceInstallation);
            _target.Run();

            _repository.Verify(x => x.Save(It.Is<WorkOrder>(wo =>
                wo.SAPErrorCode == sapNewServiceInstallation.SAPStatus)), Times.Once);

            _log.Verify(x => x.Info($"Completed Processing (1/1) SAP WorkOrders. 0 Exceptions"));
        }

        [TestMethod]
        public void TestProcessSetsSAPNumbersWhenNoErrorsHaveOccurredWhileNMI()
        {
            var workDescription = new WorkDescription();
            workDescription.SetPropertyValueByName("Id", WorkDescriptionRepository.NEW_SERVICE_INSTALLATIONS[1]);
            var sapWorkOrderStep = new SAPWorkOrderStep { Id = SAPWorkOrderStep.Indices.NMI };
            var workOrder = new WorkOrder { SAPErrorCode = "RETRY", SAPWorkOrderStep = sapWorkOrderStep, WorkDescription = workDescription, DateCompleted = DateTime.Now };
            var si = new ServiceInstallation { WorkOrder = workOrder };
            workOrder.ServiceInstallations.Add(si);
            var workOrders = new List<WorkOrder> { workOrder };
            _repository.Setup(x => x.Save(workOrder));
            _repository.Setup(x => x.Where(It.IsAny<Expression<Func<WorkOrder, bool>>>())).Returns(workOrders.AsQueryable());
            var sapWorkOrder = new SAPProgressWorkOrder { Status = "everything's fine", SAPNotificationNo = "1234", OrderNumber = "1231" };
            _sapWorkOrderRepository.Setup(x => x.Update(It.IsAny<SAPProgressWorkOrder>())).Returns(sapWorkOrder);
            var sapNewServiceInstallation = new SAPNewServiceInstallation { SAPStatus = "should be this status" };
            _sapNewServiceInstallationRepository.Setup(x => x.Save(It.IsAny<SAPNewServiceInstallation>()))
                .Returns(sapNewServiceInstallation);

            _target.Run();

            _repository.Verify(x => x.Save(It.Is<WorkOrder>(wo =>
                wo.SAPErrorCode == sapNewServiceInstallation.SAPStatus)), Times.Once);

            _log.Verify(x => x.Info($"Completed Processing (1/1) SAP WorkOrders. 0 Exceptions"));
        }
    }
}