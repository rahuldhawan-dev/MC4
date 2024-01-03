using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.ProjectManagement.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing.MSTest.TestExtensions;
using StructureMap;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class ContractorLaborCostControllerTest : MapCallMvcControllerTestBase<ContractorLaborCostController, ContractorLaborCost>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IContractorLaborCostRepository>().Use<ContractorLaborCostRepository>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            // Needed because controller inherits from EntityLookupControllerBase
            options.ExpectedEditViewName = "~/Areas/ProjectManagement/Views/ContractorLaborCost/Edit.cshtml";
            options.ExpectedNewViewName = "~/Areas/ProjectManagement/Views/ContractorLaborCost/New.cshtml";
            options.ExpectedShowViewName = "~/Areas/ProjectManagement/Views/ContractorLaborCost/Show.cshtml";
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresRoles("~/ProjectManagement/ContractorLaborCost/Index/",
                    new Dictionary<RoleModules, RoleActions> {
                        {RoleModules.FieldServicesEstimatingProjects, RoleActions.Read},
                        {RoleModules.FieldServicesDataLookups, RoleActions.Read}
                    });
                a.RequiresRoles("~/ProjectManagement/ContractorLaborCost/Show/",
                    new Dictionary<RoleModules, RoleActions> {
                        {RoleModules.FieldServicesEstimatingProjects, RoleActions.Read},
                        {RoleModules.FieldServicesDataLookups, RoleActions.Read}
                    });
                a.RequiresRoles("~/ProjectManagement/ContractorLaborCost/New/",
                    new Dictionary<RoleModules, RoleActions> {
                        {RoleModules.FieldServicesEstimatingProjects, RoleActions.Read},
                        {RoleModules.FieldServicesDataLookups, RoleActions.Add}
                    });
                a.RequiresRoles("~/ProjectManagement/ContractorLaborCost/Create/",
                    new Dictionary<RoleModules, RoleActions> {
                        {RoleModules.FieldServicesEstimatingProjects, RoleActions.Read},
                        {RoleModules.FieldServicesDataLookups, RoleActions.Add}
                    });
                a.RequiresRoles("~/ProjectManagement/ContractorLaborCost/Edit/",
                    new Dictionary<RoleModules, RoleActions> {
                        {RoleModules.FieldServicesEstimatingProjects, RoleActions.Read},
                        {RoleModules.FieldServicesDataLookups, RoleActions.Edit}
                    });
                a.RequiresRoles("~/ProjectManagement/ContractorLaborCost/Update/",
                    new Dictionary<RoleModules, RoleActions> {
                        {RoleModules.FieldServicesEstimatingProjects, RoleActions.Read},
                        {RoleModules.FieldServicesDataLookups, RoleActions.Edit}
                    });

                a.RequiresRole(
                    "~/ProjectManagement/ContractorLaborCost/FindByStockNumberUnitOrDescription",
                    RoleModules.FieldServicesEstimatingProjects);
                a.RequiresRole(
                    "~/ProjectManagement/ContractorLaborCost/FindByOperatingCenterId",
                    RoleModules.FieldServicesEstimatingProjects);

                a.RequiresRole("~/ProjectManagement/ContractorLaborCost/AddOperatingCenter/",
                    ContractorLaborCostController.ROLE, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/ContractorLaborCost/RemoveOperatingCenter/",
                    ContractorLaborCostController.ROLE, RoleActions.Edit);
            });
        }

        #endregion

        #region FindByStockNumberUnitOrDescription

        [TestMethod]
        public void TestFindByStockNumberUnitOrDescriptionFindsByStockNumber()
        {
            // not the contractor labor costs we're looking for
            GetEntityFactory<ContractorLaborCost>().CreateList(3);
            var search = "asd";

            var expected = GetEntityFactory<ContractorLaborCost>().Create(new {
                StockNumber = search
            });

            MyAssert.Contains(((AutoCompleteResult)_target.FindByStockNumberUnitOrDescription(search)).Data, expected);
        }

        [TestMethod]
        public void TestFindByStockNumberUnitOrDescriptionFindsByUnit()
        {
            // not the contractor labor costs we're looking for
            GetEntityFactory<ContractorLaborCost>().CreateList(3);
            var search = "asd";

            var expected = GetEntityFactory<ContractorLaborCost>().Create(new {
                Unit = search
            });

            MyAssert.Contains(((AutoCompleteResult)_target.FindByStockNumberUnitOrDescription(search)).Data, expected);
        }

        [TestMethod]
        public void TestFindByStockNumberUnitOrDescriptionFindsByDescription()
        {
            // not the contractor labor costs we're looking for
            GetEntityFactory<ContractorLaborCost>().CreateList(3);
            var search = "asd";

            var expected = GetEntityFactory<ContractorLaborCost>().Create(new {
                JobDescription = search
            });

            MyAssert.Contains(((AutoCompleteResult)_target.FindByStockNumberUnitOrDescription(search)).Data, expected);
        }

        #endregion
    }
}
