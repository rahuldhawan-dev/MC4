using System.Web.Mvc;
using Contractors.Controllers;
using Contractors.Data.Models.Repositories;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class StreetOpeningPermitControllerTest : ContractorControllerTestBase<StreetOpeningPermitController, StreetOpeningPermit, StreetOpeningPermitRepository>
    {
        #region Private Members

        private MapCall.Common.Model.Entities.WorkOrder _workOrder;
        private StreetOpeningPermit _permit;

        #endregion

        #region Setup

        [TestInitialize]
        public void TestInitialize()
        {
            _workOrder = GetFactory<WorkOrderFactory>().Create(new { AssignedContractor = _currentUser.Contractor });
            _permit = GetFactory<StreetOpeningPermitFactory>().Create(new { WorkOrder = _workOrder });
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => {
                var workOrder = GetFactory<WorkOrderFactory>().Create(new { AssignedContractor = _currentUser.Contractor });
                return GetFactory<StreetOpeningPermitFactory>().Create(new { WorkOrder = workOrder });
            };
            options.ExpectedShowViewName = "_Show";
            options.ShowReturnsPartialView = true;
        }

        #endregion

        #region Private Methods

        #endregion

        #region Tests

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/StreetOpeningPermit/Index");
                a.RequiresLoggedInUserOnly("~/StreetOpeningPermit/Show");
            });
        }

        #endregion

        #region Index

        [TestMethod]
        public override void TestIndexCanPerformSearchForAllSearchModelProperties()
        {
            // noop override: Index does not act like typical Index action.
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // noop override: Index does not act like typical Index action.
        }

        [TestMethod]
        public void TestIndexReturnsPartialViewWithWorkOrderModelForWorkOrderID()
        {
            var result = (PartialViewResult)_target.Index(_workOrder.Id);
            MvcAssert.IsPartialView(result);
            Assert.AreEqual("_Index", result.ViewName);
            Assert.AreEqual(_workOrder.Id, ((MapCall.Common.Model.Entities.WorkOrder)result.Model).Id);
        }

        [TestMethod]
        public void TestIndexReturns404NotFoundIfWorkOrderDoesNotExist()
        {
            var result = (HttpNotFoundResult)_target.Index(int.MaxValue);
            Assert.AreEqual(404, result.StatusCode);
        }

        #endregion

        #endregion

    }
}