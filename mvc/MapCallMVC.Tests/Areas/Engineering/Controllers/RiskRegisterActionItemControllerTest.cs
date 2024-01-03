using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Engineering.Controllers;
using MapCallMVC.Areas.Engineering.Models.ViewModels.RiskRegisterActionItems;
using MapCallMVC.Areas.HealthAndSafety.Controllers;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class RiskRegisterActionItemControllerTest : MapCallMvcControllerTestBase<RiskRegisterActionItemController, ActionItem<RiskRegisterAsset>>
    {
        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => {
                var dataType = GetEntityFactory<DataType>().Create(new { TableName = "RiskRegisterAssets" });
                var riskRegisterAssetCategory = GetEntityFactory<RiskRegisterAssetCategory>().Create();
                var riskRegisterAsset = GetEntityFactory<RiskRegisterAsset>().Create(new {
                    RiskRegisterAssetCategory = riskRegisterAssetCategory
                });
                var entity = GetEntityFactory<ActionItem>().Create(new { DataType = dataType, LinkedId = riskRegisterAsset.Id });
                return Repository.Find(entity.Id);
            };
        }

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresRole("~/Engineering/RiskRegisterActionItem/Search/", RoleModules.EngineeringRiskRegister, RoleActions.Read);
                a.RequiresRole("~/Engineering/RiskRegisterActionItem/Index/", RoleModules.EngineeringRiskRegister, RoleActions.Read);
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var dataType = GetEntityFactory<DataType>().Create(new { TableName = "RiskRegisterAssets" });

            var riskRegisterAsset1 = GetEntityFactory<RiskRegisterAsset>().Create();
            var riskRegisterAsset2 = GetEntityFactory<RiskRegisterAsset>().Create();

            var entity0 = GetEntityFactory<ActionItem>().Create(new { DataType = dataType, LinkedId = riskRegisterAsset1.Id });
            var entity1 = GetEntityFactory<ActionItem>().Create(new { DataType = dataType, LinkedId = riskRegisterAsset2.Id });

            var search = new SearchRiskRegisterActionItem();
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
