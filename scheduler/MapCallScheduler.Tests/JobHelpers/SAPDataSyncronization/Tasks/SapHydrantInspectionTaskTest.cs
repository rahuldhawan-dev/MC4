using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallScheduler.JobHelpers.SAPDataSyncronization.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.SAPDataSyncronization.Tasks
{
    [TestClass]
    public class SapHydrantInspectionTaskTest
    {
        #region Private Members

        private Mock<IRepository<HydrantInspection>> _repository;
        private Mock<ISAPInspectionRepository> _sapInspectionRepository;
        private Mock<ILog> _log;
        private SAPHydrantInspectionTask _target;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _container.Inject((_repository = new Mock<IRepository<HydrantInspection>>()).Object);
            _container.Inject((_sapInspectionRepository = new Mock<ISAPInspectionRepository>()).Object);
            _container.Inject((_log = new Mock<ILog>()).Object);

            _target = _container.GetInstance<SAPHydrantInspectionTask>();
        }

        #endregion

        [TestMethod]
        public void TestProcessSetsSapInspectionNumberWhenNoIssuesReturned()
        {
            var hydrantInspection = new HydrantInspection {SAPErrorCode = "RETRY"};
            var hydrantInspections = new List<HydrantInspection> {hydrantInspection};
            _repository.Setup(x => x.Save(hydrantInspection));
            _repository.Setup(x => x.Where(It.IsAny<Expression<Func<HydrantInspection, bool>>>())).Returns(hydrantInspections.AsQueryable());

            var sapInspection = new SAPInspection { SAPNotificationNumber= "0000001", SAPErrorCode = "Successful"};
            _sapInspectionRepository.Setup(x => x.Save(It.IsAny<SAPInspection>())).Returns(sapInspection);

            _target.Run();

            _repository.Verify(
                x =>
                    x.Save(
                        It.Is<HydrantInspection>(
                            z =>
                                z.SAPErrorCode == sapInspection.SAPErrorCode &&
                                z.SAPNotificationNumber == sapInspection.SAPNotificationNumber)), Times.Once);
            _log.Verify(x => x.Info($"Completed Processing (1/1) SAP HydrantInspections. 0 Exceptions"));
        }

        [TestMethod]
        public void TestProcessSetsSapErrorCodeWhenIssueOccurs()
        {
            var exception = new InvalidOperationException("None Shall Pass!");
            _sapInspectionRepository.Setup(x => x.Save(It.IsAny<SAPInspection>())).Throws(exception);
            var hydrant = new HydrantInspection {SAPErrorCode = SAPHydrantInspectionTask.SAP_UPDATE_FAILURE};
            var hydrants = new List<HydrantInspection> {hydrant};
            _repository.Setup(x => x.Where(It.IsAny<Expression<Func<HydrantInspection, bool>>>())).Returns(hydrants.AsQueryable());

            _target.Run();

            _repository.Verify(
                x =>
                    x.Save(
                        It.Is<HydrantInspection>(
                            z =>
                                z.SAPErrorCode == SAPHydrantInspectionTask.SAP_UPDATE_FAILURE + exception.Message &&
                                z.SAPNotificationNumber == null)), Times.Once);
            _log.Verify(x => x.Info($"Completed Processing (1/1) SAP HydrantInspections. 1 Exceptions"));
        }
    }
}