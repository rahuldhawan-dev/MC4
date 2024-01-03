using System.Web.Mvc;
using Contractors.Controllers.WorkOrder;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;
using WorkOrderRepository = Contractors.Data.Models.Repositories.WorkOrderRepository;

namespace Contractors.Tests.Controllers.WorkOrder
{
    [TestClass]
    public class WorkOrderReadOnlyControllerTest : ContractorControllerTestBase<WorkOrderReadOnlyController, MapCall.Common.Model.Entities.WorkOrder, WorkOrderRepository>
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
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/WorkOrderReadOnly/Show");
            });
        }

        [TestMethod]
        public void TestShowHasHttpGetAttribute()
        {
            MyAssert.MethodHasAttribute<HttpGetAttribute>(_target, "Show", new[] {typeof (int)});
        }

        [TestMethod]
        public override void TestShowReturnsNotFoundIfRecordCanNotBeFound()
        {
            // noop override: action returns view with null model for some reason.
        }

        [TestMethod]
        public void TestShowReturnsViewWithNullModelIfWorkOrderIsNotFound()
        {
            // No clue why we're returning a view with a null model for 404.
            var result = (ViewResult)_target.Show(1);
            Assert.IsNull(result.Model);
        }

        #endregion
    }
}
