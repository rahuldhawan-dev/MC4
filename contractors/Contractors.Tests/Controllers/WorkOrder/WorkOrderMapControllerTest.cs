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
    public class WorkOrderMapControllerTest : ContractorControllerTestBase<WorkOrderMapController, MapCall.Common.Model.Entities.WorkOrder, WorkOrderRepository>
    {
        #region Init/Cleanup

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

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/WorkOrderMap/Show");
            });
        }

        #endregion

        #region Show

        [TestMethod]
        public override void TestShowReturnsNotFoundIfRecordCanNotBeFound()
        {
            // noop override: returns view with null model for some reason.
        }

        [TestMethod]
        public void TestShowReturnsViewWithNullModelIfWorkOrderIsNotFound()
        {
            var result = (PartialViewResult)_target.Show(0);
            Assert.IsNull(result.Model);
        }

        #endregion
        
        #endregion

    }
}
