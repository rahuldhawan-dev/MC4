using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;
using StructureMap;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class TrainingRequirementControllerTest : MapCallMvcControllerTestBase<TrainingRequirementController, TrainingRequirement>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ITrainingModuleRepository>().Use<TrainingModuleRepository>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.DestroyRedirectsToRouteOnSuccessArgs = (id) => new { action = "Index" };
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                var module = RoleModules.OperationsTrainingModules;
                a.RequiresRole("~/TrainingRequirement/Index", module);
                a.RequiresRole("~/TrainingRequirement/Show", module);
                a.RequiresRole("~/TrainingRequirement/Search", module);
                a.RequiresRole("~/TrainingRequirement/New", module, RoleActions.Add);
                a.RequiresRole("~/TrainingRequirement/Create", module, RoleActions.Add);
                a.RequiresRole("~/TrainingRequirement/Edit", module, RoleActions.Edit);
                a.RequiresRole("~/TrainingRequirement/Update", module, RoleActions.Edit);
                a.RequiresRole("~/TrainingRequirement/Destroy", module, RoleActions.Delete);
            });
        }

        #region New

        [TestMethod]
        public void TestNewSetsRegulationDropDownData()
        {
            var expected = GetFactory<RegulationFactory>().Create();
            _target.New();
            var vd = (IEnumerable<SelectListItem>)_target.ViewData["Regulation"];
            Assert.AreEqual(1, vd.Count());
            Assert.AreEqual(expected.ToString(), vd.Single().Text);
            Assert.AreEqual(expected.Id.ToString(), vd.Single().Value);
        }

        #endregion

        #region Edit

        [TestMethod]
        public void TestEditSetsRegulationDropDownData()
        {
            // A requirement must exist or else ActionHelper will never call SetLookupData.
            var entity = GetFactory<TrainingRequirementFactory>().Create();
            var expected = entity.Regulation;
            Assert.IsNotNull(expected, "There should be one regulation created by the requirement factory.");
            _target.Edit(entity.Id);
            var vd = (IEnumerable<SelectListItem>)_target.ViewData["Regulation"];
            Assert.AreEqual(1, vd.Count());
            Assert.AreEqual(expected.ToString(), vd.Single().Text);
            Assert.AreEqual(expected.Id.ToString(), vd.Single().Value);
        }

        [TestMethod]
        public void TestEditSetsActiveInitialTrainingModules()
        {
            var initialRecurrantType = GetFactory<InitialTrainingModuleRecurrantTypeFactory>().Create();
            var recurringRecurrantType = GetFactory<RecurringTrainingModuleRecurrantTypeFactory>().Create();
            var initialAndRecurringRecurrantType = GetFactory<InitialAndRecurringTrainingModuleRecurrantTypeFactory>().Create();
            var trainingRequirement = GetEntityFactory<TrainingRequirement>().Create();
            var validTrainingModule = GetEntityFactory<TrainingModule>().Create(new { IsActive = true, TrainingRequirement = trainingRequirement, TrainingModuleRecurrantType = initialRecurrantType});
            var invalidTrainingModule1 = GetEntityFactory<TrainingModule>().Create(new { IsActive = false, TrainingRequirement = trainingRequirement, TrainingModuleRecurrantType = initialRecurrantType});
            var invalidTrainingModule2 = GetEntityFactory<TrainingModule>().Create(new { IsActive = true, TrainingRequirement = trainingRequirement, TrainingModuleRecurrantType = recurringRecurrantType });
            var invalidTrainingModule3 = GetEntityFactory<TrainingModule>().Create(new { IsActive = true, TrainingRequirement = trainingRequirement, TrainingModuleRecurrantType = initialAndRecurringRecurrantType});
            Session.Clear(); // updates the training requirement to have the module

            _target.Edit(trainingRequirement.Id);

            var dropdownItems = (IEnumerable<SelectListItem>)_target.ViewData["ActiveInitialTrainingModule"];

            Assert.AreEqual(1, dropdownItems.Count());
            Assert.AreEqual(validTrainingModule.Display, dropdownItems.Single().Text);
            Assert.AreEqual(validTrainingModule.Id.ToString(), dropdownItems.Single().Value);
        }

        [TestMethod]
        public void TestEditSetsActiveRecurringTrainingModules()
        {
            var initialRecurrantType = GetFactory<InitialTrainingModuleRecurrantTypeFactory>().Create();
            var recurringRecurrantType = GetFactory<RecurringTrainingModuleRecurrantTypeFactory>().Create();
            var initialAndRecurringRecurrantType = GetFactory<InitialAndRecurringTrainingModuleRecurrantTypeFactory>().Create();
            var trainingRequirement = GetEntityFactory<TrainingRequirement>().Create();
            var validTrainingModule = GetEntityFactory<TrainingModule>().Create(new { IsActive = true, TrainingRequirement = trainingRequirement, TrainingModuleRecurrantType = recurringRecurrantType });
            var invalidTrainingModule1 = GetEntityFactory<TrainingModule>().Create(new { IsActive = false, TrainingRequirement = trainingRequirement, TrainingModuleRecurrantType = recurringRecurrantType });
            var invalidTrainingModule2 = GetEntityFactory<TrainingModule>().Create(new { IsActive = true, TrainingRequirement = trainingRequirement,  TrainingModuleRecurrantType = initialRecurrantType });
            var invalidTrainingModule3 = GetEntityFactory<TrainingModule>().Create(new { IsActive = true, TrainingRequirement = trainingRequirement,  TrainingModuleRecurrantType = initialAndRecurringRecurrantType });
            Session.Clear(); // updates the training requirement to have the module

            _target.Edit(trainingRequirement.Id);

            var dropdownItems = (IEnumerable<SelectListItem>)_target.ViewData["ActiveRecurringTrainingModule"];

            Assert.AreEqual(1, dropdownItems.Count());
            Assert.AreEqual(validTrainingModule.Display, dropdownItems.Single().Text);
            Assert.AreEqual(validTrainingModule.Id.ToString(), dropdownItems.Single().Value);
        }

        [TestMethod]
        public void TestEditSetsActiveInitialAndRecurringTrainingModules()
        {
            var initialRecurrantType = GetFactory<InitialTrainingModuleRecurrantTypeFactory>().Create();
            var recurringRecurrantType = GetFactory<RecurringTrainingModuleRecurrantTypeFactory>().Create();
            var initialAndRecurringRecurrantType = GetFactory<InitialAndRecurringTrainingModuleRecurrantTypeFactory>().Create();
            var trainingRequirement = GetEntityFactory<TrainingRequirement>().Create();
            var validTrainingModule = GetEntityFactory<TrainingModule>().Create(new { IsActive = true, TrainingRequirement = trainingRequirement, TrainingModuleRecurrantType = initialAndRecurringRecurrantType }); 
            var invalidTrainingModule1 = GetEntityFactory<TrainingModule>().Create(new { IsActive = false, TrainingRequirement = trainingRequirement, TrainingModuleRecurrantType = initialAndRecurringRecurrantType });
            var invalidTrainingModule2 = GetEntityFactory<TrainingModule>().Create(new { IsActive = true, TrainingRequirement = trainingRequirement, TrainingModuleRecurrantType = recurringRecurrantType });
            var invalidTrainingModule3 = GetEntityFactory<TrainingModule>().Create(new { IsActive = true, TrainingRequirement = trainingRequirement, TrainingModuleRecurrantType = initialRecurrantType });
            Session.Clear(); // updates the training requirement to have the module

            _target.Edit(trainingRequirement.Id);

            var dropdownItems = (IEnumerable<SelectListItem>)_target.ViewData["ActiveInitialAndRecurringTrainingModule"];

            Assert.AreEqual(1, dropdownItems.Count());
            Assert.AreEqual(validTrainingModule.Display, dropdownItems.Single().Text);
            Assert.AreEqual(validTrainingModule.Id.ToString(), dropdownItems.Single().Value);
        }
        #endregion

        #endregion

        #region Index
        
        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<TrainingRequirement>().Create(new {Description = "description 0"});
            var entity1 = GetEntityFactory<TrainingRequirement>().Create(new {Description = "description 1"});
            var search = new SearchTrainingRequirement();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.Description, "Description");
                helper.AreEqual(entity1.Description, "Description", 1);
            }
        }

        #endregion
    }
}
