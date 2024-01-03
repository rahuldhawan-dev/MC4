using System.Web.Mvc;
using Contractors.Controllers.WorkOrder;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;
using WorkOrderRepository = Contractors.Data.Models.Repositories.WorkOrderRepository;

namespace Contractors.Tests.Controllers.WorkOrder
{
    [TestClass]
    public class WorkOrderInitialControllerTest : ContractorControllerTestBase<WorkOrderInitialController, MapCall.Common.Model.Entities.WorkOrder, WorkOrderRepository>
    {
        #region Setup/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => GetFactory<WorkOrderFactory>().Create(new { AssignedContractor = _currentUser.Contractor });
            options.ExpectedShowViewName = "_Show";
            options.ShowReturnsPartialView = true;
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestShowReturnsPartialViewWithWorkOrderModel()
        {
            var workOrder = GetFactory<WorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor
            });

            var result = (PartialViewResult)_target.Show(workOrder.Id);

            Assert.IsNotNull(result);
            Assert.AreSame(workOrder, result.Model);
            Assert.IsInstanceOfType(result.Model, typeof(MapCall.Common.Model.Entities.WorkOrder));
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/WorkOrderInitial/Show");
            });
        }

        #endregion
    }
}
