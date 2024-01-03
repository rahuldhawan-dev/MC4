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
    public class SapValveInspectionTaskTest
    {
        #region Private Members

        private Mock<IRepository<ValveInspection>> _repository;
        private Mock<ISAPInspectionRepository> _sapInspectionRepository;
        private Mock<ILog> _log;
        private SAPValveInspectionTask _target;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _container.Inject((_repository = new Mock<IRepository<ValveInspection>>()).Object);
            _container.Inject((_sapInspectionRepository = new Mock<ISAPInspectionRepository>()).Object);
            _container.Inject((_log = new Mock<ILog>()).Object);

            _target = _container.GetInstance<SAPValveInspectionTask>();
        }

        #endregion

        [TestMethod]
        public void TestProcessSetsSapInspectionNumberWhenNoIssuesReturned()
        {
            var valveInspection = new ValveInspection { SAPErrorCode = "RETRY" };
            var valveInspections = new List<ValveInspection> { valveInspection };
            _repository.Setup(x => x.Save(valveInspection));
            _repository.Setup(x => x.Where(It.IsAny<Expression<Func<ValveInspection, bool>>>())).Returns(valveInspections.AsQueryable());

            var sapInspection = new SAPInspection { SAPNotificationNumber = "0000001", SAPErrorCode = "Successful" };
            _sapInspectionRepository.Setup(x => x.Save(It.IsAny<SAPInspection>())).Returns(sapInspection);

            _target.Run();

            _repository.Verify(
                x =>
                    x.Save(
                        It.Is<ValveInspection>(
                            z =>
                                z.SAPErrorCode == sapInspection.SAPErrorCode &&
                                z.SAPNotificationNumber == sapInspection.SAPNotificationNumber)), Times.Once);
            _log.Verify(x => x.Info($"Completed Processing (1/1) SAP ValveInspections. 0 Exceptions"));
        }

        [TestMethod]
        public void TestProcessSetsSapErrorCodeWhenIssueOccurs()
        {
            var exception = new InvalidOperationException("None Shall Pass!");
            _sapInspectionRepository.Setup(x => x.Save(It.IsAny<SAPInspection>())).Throws(exception);
            var valve = new ValveInspection { SAPErrorCode = SAPValveInspectionTask.SAP_UPDATE_FAILURE };
            var valves = new List<ValveInspection> { valve };
            _repository.Setup(x => x.Where(It.IsAny<Expression<Func<ValveInspection, bool>>>())).Returns(valves.AsQueryable());

            _target.Run();

            _repository.Verify(
                x =>
                    x.Save(
                        It.Is<ValveInspection>(
                            z =>
                                z.SAPErrorCode == SAPValveInspectionTask.SAP_UPDATE_FAILURE + exception.Message &&
                                z.SAPNotificationNumber == null)), Times.Once);
            _log.Verify(x => x.Info($"Completed Processing (1/1) SAP ValveInspections. 1 Exceptions"));
        }
    }
}