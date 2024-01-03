using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.ProjectManagement.Models.ViewModels
{
    [TestClass]
    public class CreateRecurringProjectEndorsementTest : MapCallMvcInMemoryDatabaseTestBase<RecurringProjectEndorsement>
    {
        #region Fields

        private ViewModelTester<CreateRecurringProjectEndorsement, RecurringProjectEndorsement> _vmTester;
        private CreateRecurringProjectEndorsement _viewModel;
        private RecurringProjectEndorsement _entity;
        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _container.Inject(_dateTimeProvider.Object);

            _viewModel = new CreateRecurringProjectEndorsement(_container);
            _entity = new RecurringProjectEndorsement();
            _vmTester = new ViewModelTester<CreateRecurringProjectEndorsement, RecurringProjectEndorsement>(_viewModel, _entity);
        }

        #endregion

        [TestMethod]
        public void TestMapToEntitySetsEndorsementDate()
        {
            var expected = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expected);

            Assert.AreNotEqual(expected, _entity.EndorsementDate);

            _vmTester.MapToEntity();

            Assert.AreEqual(expected, _entity.EndorsementDate);
        }
    }
}