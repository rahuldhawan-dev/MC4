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
    public class SapHydrantTaskTest
    {
        #region Private Members

        private Mock<IRepository<Hydrant>> _repository;
        private Mock<ISAPEquipmentRepository> _sapEquipmentRepository;
        private Mock<ILog> _log;
        private SAPHydrantTask _target;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _container.Inject((_repository = new Mock<IRepository<Hydrant>>()).Object);
            _container.Inject((_sapEquipmentRepository = new Mock<ISAPEquipmentRepository>()).Object);
            _container.Inject((_log = new Mock<ILog>()).Object);

            _target = _container.GetInstance<SAPHydrantTask>();
        }

        #endregion

        [TestMethod]
        public void TestProcessSetsSapEquipmentNumberWhenNoIssuesReturned()
        {
            var hydrant = new Hydrant { SAPErrorCode = "RETRY" };
            var hydrants = new List<Hydrant> { hydrant };
            _repository.Setup(x => x.Save(hydrant));
            _repository.Setup(x => x.Where(It.IsAny<Expression<Func<Hydrant, bool>>>())).Returns(hydrants.AsQueryable());
            var sapEquipment = new SAPEquipment { SAPEquipmentNumber = "0000001", SAPErrorCode = "Successful" };
            _sapEquipmentRepository.Setup(x => x.Save(It.IsAny<SAPEquipment>())).Returns(sapEquipment);

            _target.Run();

            _repository.Verify(x => x.Save(It.Is<Hydrant>(z => z.SAPErrorCode == sapEquipment.SAPErrorCode && z.SAPEquipmentId == long.Parse(sapEquipment.SAPEquipmentNumber))), Times.Once);
            _log.Verify(x => x.Info($"Completed Processing (1/1) SAP Hydrants. 0 Exceptions"));
        }

        [TestMethod]
        public void TestProcessSetsSapErrorCodeWhenIssueOccurs()
        {
            var exception = new InvalidOperationException("None Shall Pass!");
            _sapEquipmentRepository.Setup(x => x.Save(It.IsAny<SAPEquipment>())).Throws(exception);
            var hydrant = new Hydrant { SAPErrorCode = SAPHydrantTask.SAP_UPDATE_FAILURE };
            var hydrants = new List<Hydrant> { hydrant };
            _repository.Setup(x => x.Where(It.IsAny<Expression<Func<Hydrant, bool>>>())).Returns(hydrants.AsQueryable());

            _target.Run();

            _repository.Verify(x => x.Save(It.Is<Hydrant>(z => z.SAPErrorCode == SAPHydrantTask.SAP_UPDATE_FAILURE + exception.Message && z.SAPEquipmentId == null)), Times.Once);
            _log.Verify(x => x.Info($"Completed Processing (1/1) SAP Hydrants. 1 Exceptions"));
        }
    }
}