using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Production.Controllers;
using MapCallMVC.Areas.Production.Models.ViewModels.PreJobSafetyBriefs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Production.Controller
{
    [TestClass]
    public class ProductionPreJobSafetyBriefControllerTest
        : MapCallMvcControllerTestBase<ProductionPreJobSafetyBriefController, ProductionPreJobSafetyBrief>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>()
             .Add(new TestDateTimeProvider(_now = DateTime.Now));
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (model) => {
                var actual = (CreateProductionPreJobSafetyBriefNoWorkOrder)model;
                actual.HaveAllHazardsAndPrecautionsBeenReviewed = true;
                actual.Contractors = new[] { "Some contractor" };
                actual.DescriptionOfWork = "k";
            };
            options.InitializeUpdateViewModel = (model) => {
                var actual = (EditProductionPreJobSafetyBriefNoWorkOrder)model;
                actual.HaveAllHazardsAndPrecautionsBeenReviewed = true;
                actual.Contractors = new[] { "Some contractor" };
                actual.DescriptionOfWork = "k";
            };
            options.UpdateViewModelType = typeof(EditProductionPreJobSafetyBriefNoWorkOrder);
            options.CreateValidEntity = () => GetEntityFactory<ProductionPreJobSafetyBrief>().Create(new {
                ProductionWorkOrder = (ProductionWorkOrder)null
            });
        }

        #endregion

        #region Tests
        
        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const RoleModules role = RoleModules.OperationsHealthAndSafety;
                const string path = "~/Production/ProductionPreJobSafetyBrief/";
                a.RequiresRole(path + "Search", role);
                a.RequiresRole(path + "Index", role);
                a.RequiresRole(path + "Show", role);
                a.RequiresRole(path + "Create", role, RoleActions.Add);
                a.RequiresRole(path + "CreateFromOrder", role, RoleActions.Add);
                a.RequiresRole(path + "New", role, RoleActions.Add);
                a.RequiresRole(path + "NewForOrder", role, RoleActions.Add);
                a.RequiresRole(path + "Edit", role, RoleActions.Edit);
                a.RequiresRole(path + "Update", role, RoleActions.Edit);
                a.RequiresRole(path + "UpdateWithOrder", role, RoleActions.Edit);
            });
        }
        
        #endregion
        
        #region NewFromOrder

        [TestMethod]
        public void Test_NewFromOrder_ReturnsNewViewWithNewViewModel()
        {
            var pwo = GetEntityFactory<ProductionWorkOrder>().Create();
            
            var result = (ViewResult)_target.NewForOrder(pwo.Id);
            var resultModel = (CreateProductionPreJobSafetyBriefFromWorkOrder)result.Model;
            
            Assert.AreEqual(pwo.Id, resultModel.ProductionWorkOrder);
            Assert.AreSame(pwo, resultModel.GetProductionWorkOrderForDisplay());
        }

        [TestMethod]
        public void Test_NewFromOrder_Returns404_IfProductionWorkOrderIsNotFound()
        {
            MvcAssert.IsNotFound(_target.NewForOrder(0));
        }

        [TestMethod]
        public void Test_NewFromOrder_SetsSpecificEmployeeDropDownData()
        {
            // And also do this for Create on fail, Edit, and Update on fail.
            var badEmployee = GetEntityFactory<Employee>().Create();
            var goodEmployee = GetEntityFactory<Employee>().Create();
            var pwo = GetEntityFactory<ProductionWorkOrder>().Create();
            var pwoAssignment = GetEntityFactory<EmployeeAssignment>().Create(new {
                ProductionWorkOrder = pwo,
                AssignedTo = goodEmployee
            });
            pwo.EmployeeAssignments.Add(pwoAssignment);
            Assert.IsTrue(pwo.EmployeeAssignments.Contains(pwoAssignment), "Sanity");

            _target.NewForOrder(pwo.Id); // Result doesn't matter for this test.

            var employeeResult = (IEnumerable<SelectListItem>)_target.ViewData["Employees"];
            Assert.AreEqual(goodEmployee.Id.ToString(), employeeResult.Single().Value);
        }

        #endregion
        
        #region UpdateWithOrder

        [TestMethod]
        public void Test_UpdateWithOrder_RedirectsToShowActionAfterSuccessfulSave()
        {
            var entity = GetEntityFactory<ProductionPreJobSafetyBrief>().Create();
            var viewModel = _viewModelFactory
               .BuildWithOverrides<
                    EditProductionPreJobSafetyBriefWithWorkOrder,
                    ProductionPreJobSafetyBrief>(
                    entity,
                    new {
                        HaveAllHazardsAndPrecautionsBeenReviewed = true,
                        Contractors = new[] { "Some contractor" }
                    });

            var result = _target.UpdateWithOrder(viewModel);
            
            MvcAssert.RedirectsToRoute(result, "Show", new {id = entity.Id});
        }

        [TestMethod]
        public void Test_UpdateWithOrder_ReturnsEditViewWithModel_WhenThereAreModelStateErrors()
        {
            var entity = GetEntityFactory<ProductionPreJobSafetyBrief>().Create();
            var viewModel = _viewModelFactory
               .BuildWithOverrides<
                    EditProductionPreJobSafetyBriefWithWorkOrder,
                    ProductionPreJobSafetyBrief>(
                    entity,
                    new {
                        HaveAllHazardsAndPrecautionsBeenReviewed = true,
                        Contractors = new[] { "Some contractor" }
                    });

            _target.ModelState.AddModelError("oh", "no!");

            var result = _target.UpdateWithOrder(viewModel);
            
            MvcAssert.IsViewWithNameAndModel(result, "Edit", viewModel);
        }

        [TestMethod]
        public void Test_UpdateWithOrder_ReturnsNotFound_IfRecordBeingUpdatedDoesNotExist()
        {
            var entity = GetEntityFactory<ProductionPreJobSafetyBrief>().Create();
            var viewModel = _viewModelFactory
               .BuildWithOverrides<
                    EditProductionPreJobSafetyBriefWithWorkOrder,
                    ProductionPreJobSafetyBrief>(
                    entity,
                    new {
                        HaveAllHazardsAndPrecautionsBeenReviewed = true,
                        Contractors = new[] { "Some contractor" }
                    });

            viewModel.Id = -1;

            var result = _target.UpdateWithOrder(viewModel);
            
            MvcAssert.IsNotFound(result);
        }
        
        #endregion
        
        #endregion
    }
}
