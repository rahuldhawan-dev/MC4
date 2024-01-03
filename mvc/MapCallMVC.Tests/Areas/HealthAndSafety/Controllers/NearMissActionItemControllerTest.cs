using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.HealthAndSafety.Controllers;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.HealthAndSafety.Controllers
{
    [TestClass]
    public class NearMissActionItemControllerTest : MapCallMvcControllerTestBase<NearMissActionItemController, ActionItem<NearMiss>>
    {
        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => {
                var dataType = GetEntityFactory<DataType>().Create(new { TableName = "NearMisses" });
                var nearMiss = GetEntityFactory<NearMiss>().Create();
                var entity = GetEntityFactory<ActionItem>().Create(new { DataType = dataType, LinkedId = nearMiss.Id });
                return Repository.Find(entity.Id);
            };
        }

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresRole("~/HealthAndSafety/NearMissActionItem/Search/", RoleModules.OperationsHealthAndSafety, RoleActions.Read);
                a.RequiresRole("~/HealthAndSafety/NearMissActionItem/Index/", RoleModules.OperationsHealthAndSafety, RoleActions.Read);
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var dataType = GetEntityFactory<DataType>().Create(new { TableName = "NearMisses" });

            var nearMiss0 = GetEntityFactory<NearMiss>().Create();
            var nearMiss1 = GetEntityFactory<NearMiss>().Create();

            var entity0 = GetEntityFactory<ActionItem>().Create(new { DataType = dataType, LinkedId = nearMiss0.Id });
            var entity1 = GetEntityFactory<ActionItem>().Create(new { DataType = dataType, LinkedId = nearMiss1.Id });

            var search = new SearchNearMissActionItem();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
            }
        }

        #endregion
    }
}
