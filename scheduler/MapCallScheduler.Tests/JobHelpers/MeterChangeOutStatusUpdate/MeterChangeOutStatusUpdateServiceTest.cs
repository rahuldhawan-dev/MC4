using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallScheduler.JobHelpers.MeterChangeOutStatusUpdate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using NHibernate;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.MeterChangeOutStatusUpdate
{
    [TestClass]
    public class MeterChangeOutStatusUpdateServiceTest : InMemoryDatabaseTest<MeterChangeOut>
    {
        #region Private Members

        private MeterChangeOutStatusUpdateService _target;
        private Mock<ILog> _log;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private Mock<IMeterChangeOutRepository> _mockMCORepo;
        private Mock<IRepository<MeterChangeOutStatus>> _mockMCOStatusRepo;
        private Mock<IDataTypeRepository> _mockDataTypeRepo;
        private Mock<IRepository<Note>> _mockNoteRepo;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _container.Inject((_log = new Mock<ILog>()).Object);
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(DateTime.Now);
            _container.Inject(_dateTimeProvider.Object);
            _mockMCORepo = new Mock<IMeterChangeOutRepository>();
            _container.Inject(_mockMCORepo.Object);
            _mockMCOStatusRepo = new Mock<IRepository<MeterChangeOutStatus>>();
            _container.Inject(_mockMCOStatusRepo.Object);
            _mockDataTypeRepo = new Mock<IDataTypeRepository>();
            _container.Inject(_mockDataTypeRepo.Object);
            _mockNoteRepo = new Mock<IRepository<Note>>();
            _container.Inject(_mockNoteRepo.Object);

            _target = _container.GetInstance<MeterChangeOutStatusUpdateService>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestProcessUpdatesStatusAndDateStatusChanged()
        {
            // Setup all the mocked data.
            var alreadyChangedStatus = new MeterChangeOutStatus();
            _mockMCOStatusRepo.Setup(x => x.Find(MeterChangeOutStatus.Indices.ALREADY_CHANGED)).Returns(alreadyChangedStatus);
            var expectedDataType = new DataType();
            _mockDataTypeRepo.Setup(x => x.GetByTableName(nameof(MeterChangeOut) + "s")).Returns(new [] { expectedDataType });
            var mco1 = new MeterChangeOut() { Id = 1};
            var mco2 = new MeterChangeOut() { Id = 2};
            _mockMCORepo.Setup(x => x.GetActiveMeterChangeOutsWithOutOfDateNewSerialNumber()).Returns(new[] {mco1, mco2});

            IEnumerable<MeterChangeOut> savedMCOs = null;
            _mockMCORepo.Setup(x => x.Save(It.IsAny<IEnumerable<MeterChangeOut>>())).Callback((IEnumerable<MeterChangeOut> x) => {
                savedMCOs = x;
            });

            IEnumerable<Note> savedNotes = null;
            _mockNoteRepo.Setup(x => x.Save(It.IsAny<IEnumerable<Note>>())).Callback((IEnumerable<Note> x) => {
                savedNotes = x;
            });

            // Act
            _target.Process();

            // Assert that each individual MCO was updated correctly.
            Assert.AreSame(alreadyChangedStatus, mco1.MeterChangeOutStatus);
            Assert.AreSame(alreadyChangedStatus, mco2.MeterChangeOutStatus);

            // Assert that each test MCO was actually saved
            Assert.IsNotNull(savedMCOs);
            Assert.IsTrue(savedMCOs.Contains(mco1));
            Assert.IsTrue(savedMCOs.Contains(mco2));

            // Assert that each MCO had a note added and saved
            Assert.IsNotNull(savedNotes);
            Action<MeterChangeOut> testNote = (mco) => {
                var note = savedNotes.Single(x => x.LinkedId == mco.Id);
                Assert.AreSame(expectedDataType, note.DataType);
                Assert.AreEqual("Scheduler - Meter Change Out Status Update", note.CreatedBy);
                Assert.AreEqual("Already confirmed in MapCall", note.Text);
            };
            testNote(mco1);
            testNote(mco2);
        }

        #endregion
    }
}
