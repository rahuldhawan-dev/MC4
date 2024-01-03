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
    public class SapSewerOpeningTaskTest
    {
        #region Private Members

        private Mock<IRepository<SewerOpening>> _repository;
        private Mock<ISAPEquipmentRepository> _sapEquipmentRepository;
        private Mock<ILog> _log;
        private SAPSewerOpeningTask _target;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _container.Inject((_repository = new Mock<IRepository<SewerOpening>>()).Object);
            _container.Inject((_sapEquipmentRepository = new Mock<ISAPEquipmentRepository>()).Object);
            _container.Inject((_log = new Mock<ILog>()).Object);

            _target = _container.GetInstance<SAPSewerOpeningTask>();
        }

        #endregion

        [TestMethod]
        public void TestProcessSetsSapEquipmentNumberWhenNoIssuesReturned()
        {
            var sewerOpening = new SewerOpening { SAPErrorCode = "RETRY" };
            var sewerOpenings = new List<SewerOpening> { sewerOpening };
            _repository.Setup(x => x.Save(sewerOpening));
            _repository.Setup(x => x.Where(It.IsAny<Expression<Func<SewerOpening, bool>>>())).Returns(sewerOpenings.AsQueryable());
            var sapEquipment = new SAPEquipment { SAPEquipmentNumber = "0000001", SAPErrorCode = "Successful" };
            _sapEquipmentRepository.Setup(x => x.Save(It.IsAny<SAPEquipment>())).Returns(sapEquipment);

            _target.Run();

            _repository.Verify(x => x.Save(It.Is<SewerOpening>(z => z.SAPErrorCode == sapEquipment.SAPErrorCode && z.SAPEquipmentId == long.Parse(sapEquipment.SAPEquipmentNumber))), Times.Once);
            _log.Verify(x => x.Info($"Completed Processing (1/1) SAP SewerOpenings. 0 Exceptions"));
        }

        [TestMethod]
        public void TestProcessSetsSapErrorCodeWhenIssueOccurs()
        {
            var exception = new InvalidOperationException("None Shall Pass!");
            _sapEquipmentRepository.Setup(x => x.Save(It.IsAny<SAPEquipment>())).Throws(exception);
            var sewerOpening = new SewerOpening { SAPErrorCode = SAPSewerOpeningTask.SAP_UPDATE_FAILURE };
            var sewerOpenings = new List<SewerOpening> { sewerOpening };
            _repository.Setup(x => x.Where(It.IsAny<Expression<Func<SewerOpening, bool>>>())).Returns(sewerOpenings.AsQueryable());

            _target.Run();

            _repository.Verify(x => x.Save(It.Is<SewerOpening>(z => z.SAPErrorCode == SAPSewerOpeningTask.SAP_UPDATE_FAILURE + exception.Message && z.SAPEquipmentId == null)), Times.Once);
            _log.Verify(x => x.Info($"Completed Processing (1/1) SAP SewerOpenings. 1 Exceptions"));
        }
    }
}