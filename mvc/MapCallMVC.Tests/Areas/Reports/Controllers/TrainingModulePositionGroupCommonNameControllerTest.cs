using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Areas.Reports.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class TrainingModulePositionGroupCommonNameControllerTest : MapCallMvcControllerTestBase<TrainingModulePositionGroupCommonNameController, PositionGroupCommonName,  PositionGroupCommonNameRepository>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ITrainingModuleRepository>().Use<TrainingModuleRepository>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.OperationsTrainingModules;
                a.RequiresRole("~/FieldOperations/TrainingModulePositionGroupCommonName/Search", module);
                a.RequiresRole("~/FieldOperations/TrainingModulePositionGroupCommonName/Index", module);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            var cat1 = GetFactory<TrainingModuleCategoryFactory>().Create(new { Description = "Some category" });

            var expectedTrainingModule = GetEntityFactory<TrainingModule>().Create(new
            {
                TrainingModuleCategory = cat1,
                Title = "Neat"
            });
            var someOtherTrainingModule = GetEntityFactory<TrainingModule>().Create();

            var pgcn = GetFactory<PositionGroupCommonNameFactory>().Create(new { Description = "This one" });
            var req = GetFactory<TrainingRequirementFactory>().Create();
            req.TrainingModules.Add(expectedTrainingModule);
            pgcn.TrainingRequirements.Add(req);
            expectedTrainingModule.TrainingRequirement = req;
            Session.Flush();
            var search = new SearchTrainingModulePositionGroupCommonName { TrainingModule = expectedTrainingModule.Id };
            var result = _target.Index(search);
            MvcAssert.IsViewNamed(result, "Index");

            Assert.AreEqual(1, search.Count);
        }

        [TestMethod]
        public void TestIndexDisablesPaging()
        {
            var search = new SearchTrainingModulePositionGroupCommonName();
            search.EnablePaging = true;
            _target.Index(search);
            Assert.IsFalse(search.EnablePaging);
        }

        [TestMethod]
        public void TestIndexRespondsToExcel()
        {
            var cat1 = GetFactory<TrainingModuleCategoryFactory>().Create(new { Description = "Some category" });

            var expectedTrainingModule = GetEntityFactory<TrainingModule>().Create(new
            {
                TrainingModuleCategory = cat1,
                Title = "Neat"
            });
            var someOtherTrainingModule = GetEntityFactory<TrainingModule>().Create();

            var pgcn = GetFactory<PositionGroupCommonNameFactory>().Create(new { Description = "This one" });
            var req = GetFactory<TrainingRequirementFactory>().Create();
            req.TrainingModules.Add(expectedTrainingModule);
            pgcn.TrainingRequirements.Add(req);
            expectedTrainingModule.TrainingRequirement = req;

            var search = new SearchTrainingModulePositionGroupCommonName { TrainingModule = expectedTrainingModule.Id };

            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.EXCEL_2003;
            Session.Flush();
            var result = _target.Index(search) as ExcelResult;
            Assert.IsFalse(search.EnablePaging, "EnablePaging should be disabled always.");

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                var viewModel = search.Results.Single();
                helper.AreEqual(viewModel.ModuleCategory, "ModuleCategory");
                helper.AreEqual(viewModel.ModuleTitle, "ModuleTitle");
                helper.AreEqual(viewModel.PositionGroupCommonName, "PositionGroupCommonName");
            }
        }
        

        #endregion

    }
}
