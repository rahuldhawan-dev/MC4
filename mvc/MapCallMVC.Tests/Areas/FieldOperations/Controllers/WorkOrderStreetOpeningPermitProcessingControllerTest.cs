using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using System.Web.Mvc;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderStreetOpeningPermitProcessing;
using System.Linq;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class WorkOrderStreetOpeningPermitProcessingControllerTest : MapCallMvcControllerTestBase<WorkOrderStreetOpeningPermitProcessingController, WorkOrder>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);

            options.CreateValidEntity = () => {
                var wo = GetFactory<WorkOrderFactory>().Create(new {
                    SAPWorkOrderNumber = (long?)11251,
                    OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new
                        { SAPWorkOrdersEnabled = true, IsContractedOperations = false }),
                    Priority = typeof(EmergencyWorkOrderPriorityFactory),
                    StreetOpeningPermitRequired = true
                });

                GetFactory<RoleFactory>().Create(RoleModules.FieldServicesWorkManagement, wo.OperatingCenter,
                    _currentUser, RoleActions.Read);
                return wo;
            };
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.FieldServicesWorkManagement;
                a.RequiresRole("~/WorkOrderStreetOpeningPermitProcessing/Search", module, RoleActions.Read);
                a.RequiresRole("~/WorkOrderStreetOpeningPermitProcessing/Index", module, RoleActions.Read);
                a.RequiresRole("~/WorkOrderStreetOpeningPermitProcessing/Show", module, RoleActions.Read);
                a.RequiresRole("~/WorkOrderStreetOpeningPermitProcessing/Edit", module, RoleActions.UserAdministrator);
            });
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // The finalization work orders criteria needs one  ofthe following for SAP filtering:
            //   - The operating center has SAPWorkOrdersEnabled = false
            //   - OR the operating center's IsContractedOperations = true
            //   - OR the work order has an SAPWorkOrderNumber value

            // All of these need to be valid work orders
            var validWorkOrderBecauseItHasSAPWorkOrderNumber = GetFactory<WorkOrderFactory>().Create(new {
                SAPWorkOrderNumber = (long?)11251,
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = false }),
                Priority = typeof(EmergencyWorkOrderPriorityFactory),
                StreetOpeningPermitRequired = true
            });
            var validWorkOrderBecauseIsNotSAPEnabled = GetFactory<WorkOrderFactory>().Create(new {
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = false, IsContractedOperations = false }),
                Priority = typeof(EmergencyWorkOrderPriorityFactory),
                StreetOpeningPermitRequired = true
            });
            var validWorkOrderBecauseIsContractedOperations = GetFactory<WorkOrderFactory>().Create(new {
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = true }),
                Priority = typeof(EmergencyWorkOrderPriorityFactory),
                StreetOpeningPermitRequired = true
            });

            var invalidBecauseItIsSAPEnabledWithoutAnSAPWorkOrderNumber = GetFactory<WorkOrderFactory>().Create(new {
                SAPWorkOrderNumber = (long?)null,
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = false }),
                Priority = typeof(EmergencyWorkOrderPriorityFactory),
                StreetOpeningPermitRequired = true
            });
            var invalidBecauseItIsSAPEnabledButNotContractedOperations = GetFactory<WorkOrderFactory>().Create(new {
                SAPWorkOrderNumber = (long?)null,
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = false }),
                Priority = typeof(EmergencyWorkOrderPriorityFactory),
                StreetOpeningPermitRequired = true
            });
            var invalidBecauseItDoesNotHaveSAPWorkOrderNumber = GetFactory<CompletedWorkOrderFactory>().Create(new {
                SAPWorkOrderNumber = (long?)null,
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = false }),
                Priority = typeof(EmergencyWorkOrderPriorityFactory),
                StreetOpeningPermitRequired = true
            });
            var invalidWorkOrderBecauseStreetOpeningPermitNotRequired = GetFactory<WorkOrderFactory>().Create(new {
                SAPWorkOrderNumber = (long?)11251,
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = false, IsContractedOperations = true }),
                Priority = typeof(EmergencyWorkOrderPriorityFactory)
            });

            GetFactory<WildcardOpCenterRoleFactory>().Create(RoleModules.FieldServicesWorkManagement, null,
                _currentUser, RoleActions.Read);

            var result = (ViewResult)_target.Index(new SearchWorkOrderStreetOpeningPermitProcessing());
            var results = ((SearchWorkOrderStreetOpeningPermitProcessing)result.Model).Results;

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(3, results.Count());
            Assert.IsTrue(results.Contains(validWorkOrderBecauseItHasSAPWorkOrderNumber));
            Assert.IsTrue(results.Contains(validWorkOrderBecauseIsNotSAPEnabled));
            Assert.IsTrue(results.Contains(validWorkOrderBecauseIsContractedOperations));

            Assert.IsFalse(results.Contains(invalidBecauseItIsSAPEnabledWithoutAnSAPWorkOrderNumber));
            Assert.IsFalse(results.Contains(invalidBecauseItIsSAPEnabledButNotContractedOperations));
            Assert.IsFalse(results.Contains(invalidBecauseItDoesNotHaveSAPWorkOrderNumber));
            Assert.IsFalse(results.Contains(invalidWorkOrderBecauseStreetOpeningPermitNotRequired));
        }

        #region Edit

        [TestMethod]
        public override void TestEditReturns404IfMatchingRecordCanNotBeFound()
        {
            // noop, Edit action only needed so users with edit permissions can add/edit documents
        }

        [TestMethod]
        public override void TestEditReturnsEditViewWithEditViewModel()
        {
            // noop, Edit action only needed so users with edit permissions can add/edit documents
        }

        #endregion

        #endregion
    }
}
