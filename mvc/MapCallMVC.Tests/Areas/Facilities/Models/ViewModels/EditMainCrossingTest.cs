using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Facilities.Models.ViewModels
{
    [TestClass]
    public class EditMainCrossingTest : MapCallMvcInMemoryDatabaseTestBase<MainCrossing>
    {
        private ViewModelTester<EditMainCrossing, MainCrossing> _vmTester;
        private EditMainCrossing _viewModel;
        private MainCrossing _entity;

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetEntityFactory<MainCrossing>().Create();
            _viewModel = _viewModelFactory.Build<EditMainCrossing, MainCrossing>( _entity);
            _vmTester = new ViewModelTester<EditMainCrossing, MainCrossing>(_viewModel, _entity);
        }

        #endregion

        [TestMethod]
        public void TestMapToEntityCancelsAnyAttachedWorkOrders()
        {
            var now = DateTime.Now;
            var reason = GetEntityFactory<WorkOrderCancellationReason>()
               .Create(new {Status = "ARET", Description = "Asset Retired"});
            _entity.WorkOrders = GetFactory<WorkOrderFactory>().CreateList(2);
            _viewModel.DateRetired = now;

            _vmTester.MapToEntity();

            foreach (var workOrder in _entity.WorkOrders)
            {
                Assert.AreEqual(now, workOrder.CancelledAt);
                Assert.AreEqual(reason, workOrder.WorkOrderCancellationReason);
            }
        }
    }
}
