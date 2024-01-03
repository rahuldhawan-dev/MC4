using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels
{
    [TestClass]
    public class CreateTrafficControlTicketCheckTest : MapCallMvcInMemoryDatabaseTestBase<TrafficControlTicketCheck>
    {
        #region Fields

        private ViewModelTester<CreateTrafficControlTicketCheck, TrafficControlTicketCheck> _vmTester;
        private CreateTrafficControlTicketCheck _viewModel;
        private TrafficControlTicketCheck _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new CreateTrafficControlTicketCheck(_container);
            _entity = new TrafficControlTicketCheck();
            _vmTester = new ViewModelTester<CreateTrafficControlTicketCheck, TrafficControlTicketCheck>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Amount);
            _vmTester.CanMapBothWays(x => x.CheckNumber);
            _vmTester.CanMapBothWays(x => x.Memo);
            //_vmTester.CanMapBothWays(x => x.Reconciled);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.CheckNumber);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Amount);
        }

        [TestMethod]
        public void TestTrafficControlTicketCanMapBothWays()
        {
            var ticket = GetEntityFactory<TrafficControlTicket>().Create();
            _entity.TrafficControlTicket = ticket;

            _vmTester.MapToViewModel();

            Assert.AreEqual(ticket.Id, _viewModel.TrafficControlTicket);

            _entity.TrafficControlTicket = null;
            _vmTester.MapToEntity();

            Assert.AreSame(ticket, _entity.TrafficControlTicket);
        }

        #endregion
    }

    [TestClass]
    public class EditTrafficControlTicketCheckTest : MapCallMvcInMemoryDatabaseTestBase<TrafficControlTicketCheck>
    {
        #region Fields

        private ViewModelTester<EditTrafficControlTicketCheck, TrafficControlTicketCheck> _vmTester;
        private EditTrafficControlTicketCheck _viewModel;
        private TrafficControlTicketCheck _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new EditTrafficControlTicketCheck(_container);
            _entity = new TrafficControlTicketCheck();
            _vmTester = new ViewModelTester<EditTrafficControlTicketCheck, TrafficControlTicketCheck>(_viewModel, _entity);
        }

        #endregion

        #region Tests
        
        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Amount);
            _vmTester.CanMapBothWays(x => x.CheckNumber);
            _vmTester.CanMapBothWays(x => x.Memo);
            _vmTester.CanMapBothWays(x => x.Reconciled);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.CheckNumber);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Amount);
        }

        #endregion
    }
}
