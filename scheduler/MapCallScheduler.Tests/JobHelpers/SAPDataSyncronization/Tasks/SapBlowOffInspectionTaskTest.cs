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
    public class SapBlowOffInspectionTaskTest
    {
        #region Private Members

        private Mock<IRepository<BlowOffInspection>> _repository;
        private Mock<ISAPInspectionRepository> _sapInspectionRepository;
        private Mock<ILog> _log;
        private SAPBlowOffInspectionTask _target;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _container.Inject((_repository = new Mock<IRepository<BlowOffInspection>>()).Object);
            _container.Inject((_sapInspectionRepository = new Mock<ISAPInspectionRepository>()).Object);
            _container.Inject((_log = new Mock<ILog>()).Object);

            _target = _container.GetInstance<SAPBlowOffInspectionTask>();
        }

        #endregion

        [TestMethod]
        public void TestProcessSetsSapInspectionNumberWhenNoIssuesReturned()
        {
            var blowOffInspection = new BlowOffInspection { SAPErrorCode = "RETRY" };
            var blowOffInspections = new List<BlowOffInspection> { blowOffInspection };
            _repository.Setup(x => x.Save(blowOffInspection));
            _repository.Setup(x => x.Where(It.IsAny<Expression<Func<BlowOffInspection, bool>>>())).Returns(blowOffInspections.AsQueryable());

            var sapInspection = new SAPInspection { SAPNotificationNumber = "0000001", SAPErrorCode = "Successful" };
            _sapInspectionRepository.Setup(x => x.Save(It.IsAny<SAPInspection>())).Returns(sapInspection);

            _target.Run();

            _repository.Verify(
                x =>
                    x.Save(
                        It.Is<BlowOffInspection>(
                            z =>
                                z.SAPErrorCode == sapInspection.SAPErrorCode &&
                                z.SAPNotificationNumber == sapInspection.SAPNotificationNumber)), Times.Once);
            _log.Verify(x => x.Info($"Completed Processing (1/1) SAP BlowOffInspections. 0 Exceptions"));
        }

        [TestMethod]
        public void TestProcessSetsSapErrorCodeWhenIssueOccurs()
        {
            var exception = new InvalidOperationException("None Shall Pass!");
            _sapInspectionRepository.Setup(x => x.Save(It.IsAny<SAPInspection>())).Throws(exception);
            var blowOff = new BlowOffInspection { SAPErrorCode = SAPBlowOffInspectionTask.SAP_UPDATE_FAILURE };
            var blowOffs = new List<BlowOffInspection> { blowOff };
            _repository.Setup(x => x.Where(It.IsAny<Expression<Func<BlowOffInspection, bool>>>())).Returns(blowOffs.AsQueryable());

            _target.Run();

            _repository.Verify(
                x =>
                    x.Save(
                        It.Is<BlowOffInspection>(
                            z =>
                                z.SAPErrorCode == SAPBlowOffInspectionTask.SAP_UPDATE_FAILURE + exception.Message &&
                                z.SAPNotificationNumber == null)), Times.Once);
            _log.Verify(x => x.Info($"Completed Processing (1/1) SAP BlowOffInspections. 1 Exceptions"));
        }
    }
}