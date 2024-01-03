using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.Engineering.Controllers;
using MapCallMVC.Areas.Engineering.Models.ViewModels.RiskRegisterAssets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using Moq;

// ReSharper disable once CheckNamespace
namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class RiskRegisterAssetControllerTest : MapCallMvcControllerTestBase<RiskRegisterAssetController, RiskRegisterAsset>
    {
        #region Fields

        private Mock<INotificationService> _noteServ;

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const string urlPathPart = "~/Engineering/RiskRegisterAsset";

                a.RequiresRole($"{urlPathPart}/Search/", RoleModules.EngineeringRiskRegister);
                a.RequiresRole($"{urlPathPart}/Show/", RoleModules.EngineeringRiskRegister);
                a.RequiresRole($"{urlPathPart}/Index/", RoleModules.EngineeringRiskRegister);
                a.RequiresRole($"{urlPathPart}/New/", RoleModules.EngineeringRiskRegister, RoleActions.Add);
                a.RequiresRole($"{urlPathPart}/Create/", RoleModules.EngineeringRiskRegister, RoleActions.Add);
                a.RequiresRole($"{urlPathPart}/Edit/", RoleModules.EngineeringRiskRegister, RoleActions.Edit);
                a.RequiresRole($"{urlPathPart}/Update/", RoleModules.EngineeringRiskRegister, RoleActions.Edit);
                a.RequiresRole($"{urlPathPart}/Destroy/", RoleModules.EngineeringRiskRegister, RoleActions.Delete);
            });
        }

        #endregion

        #region Create

        [TestMethod]
        public void TestCreateSendsNotification()
        {
            _noteServ = new Mock<INotificationService>();
            _container.Inject(_noteServ.Object);
            var entity = GetEntityFactory<RiskRegisterAsset>().Create();
            var model = _viewModelFactory.Build<CreateRiskRegisterAsset, RiskRegisterAsset>(entity);

            _target.Create(model);

            _noteServ.Verify(x => x.Notify(It.IsAny<NotifierArgs>()));
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            const string riskDescription = "Description of what the risk may have.";

            var entity = GetEntityFactory<RiskRegisterAsset>().Create();

            _target.Update(_viewModelFactory.BuildWithOverrides<EditRiskRegisterAsset, RiskRegisterAsset>(entity, new {
                RiskDescription = riskDescription
            }));

            var updatedEntity = Session.Get<RiskRegisterAsset>(entity.Id);

            Assert.AreEqual(riskDescription, updatedEntity.RiskDescription);
        }

        [TestMethod]
        public void TestUpdateSendsNotification()
        {
            _noteServ = new Mock<INotificationService>();
            _container.Inject(_noteServ.Object);
            var entity = GetEntityFactory<RiskRegisterAsset>().Create();
            var model = _viewModelFactory.Build<EditRiskRegisterAsset, RiskRegisterAsset>(entity);

            _target.Update(model);

            _noteServ.Verify(x => x.Notify(It.IsAny<NotifierArgs>()));
        }

        #endregion

        #region Search

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            var riskRegisterAssetCategory = GetEntityFactory<RiskRegisterAssetCategory>().Create();
            var riskRegisterAsset = GetEntityFactory<RiskRegisterAsset>().Create(new {
                RiskRegisterAssetCategory = riskRegisterAssetCategory
            });

            var searchViewModel = new SearchRiskRegisterAssetViewModel
                { RiskRegisterAssetCategory = riskRegisterAssetCategory.Id };

            var result = _target.Index(searchViewModel);

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(1, searchViewModel.Count);
        }

        [TestMethod]
        public void TestIndexXlsExportsExcel()
        {
            const string sheetName1 = "Risk Register Asset";
            const string sheetName2 = "Disclaimer";
            string Disclaimer = "This is disclaimer";
            string DescriptionGenerator(int uniqueValue) => $"This is a description for test item {uniqueValue}";
            DateTime DateTimeGenerator(int daysAgo) => DateTime.Now.AddDays(daysAgo * -1);

            const int expectedAssetsResultsCount = 6;
            const int expectedDisclaimerResultsCount = 1;

            var state = GetEntityFactory<State>().Create();
            var employee = GetEntityFactory<Employee>().Create();
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var riskRegisterAssetFactory = GetEntityFactory<RiskRegisterAsset>();
            var riskRegisterAssetGroup = GetEntityFactory<RiskRegisterAssetGroup>().Create();
            var riskRegisterAssetCategory = GetEntityFactory<RiskRegisterAssetCategory>().Create();

            var assets = new List<RiskRegisterAsset>(expectedAssetsResultsCount);
            var disclaimerList = new List<RiskRegisterAssetExcelDisclaimerViewModel>();
            disclaimerList.Add(new RiskRegisterAssetExcelDisclaimerViewModel { Disclaimer = Disclaimer });

            for (var a = 0; a < expectedAssetsResultsCount; a++)
            {
                assets.Add(riskRegisterAssetFactory.Create(new {
                    State = state,
                    OperatingCenter = operatingCenter,
                    Employee = employee,
                    RiskRegisterAssetGroup = riskRegisterAssetGroup,
                    RiskRegisterAssetCategory = riskRegisterAssetCategory,
                    ImpactDescription = DescriptionGenerator(a),
                    RiskDescription = DescriptionGenerator(a),
                    IdentifiedAt = DateTimeGenerator(a),
                    IsProjectInComprehensivePlanningStudy = true,
                    IsProjectInCapitalPlan = false
                }));
            }

            var searchViewModel = new SearchRiskRegisterAssetViewModel { State = state.Id };

            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var searchViewResult = _target.Index(searchViewModel) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, searchViewResult, true))
            {
                for (var rowIndex = 0; rowIndex < assets.Count(); rowIndex++)
                {
                    var asset = assets[rowIndex];
                   
                    helper.AreEqual(asset.Id, sheetName1, "Id", rowIndex);
                    helper.AreEqual(asset.ImpactDescription, sheetName1, "COF Weighted", rowIndex);
                    helper.AreEqual(asset.RiskDescription, sheetName1, "Description of Risk", rowIndex);
                }

                Assert.AreEqual(expectedAssetsResultsCount, helper.GetRowCount(sheetName1));

                for (var rowIndex = 0; rowIndex < disclaimerList.Count(); rowIndex++)
                {
                    var disclaimer = disclaimerList[rowIndex];

                    helper.AreEqual(disclaimer.Disclaimer, sheetName2, "Disclaimer", rowIndex);
                }

                Assert.AreEqual(expectedDisclaimerResultsCount, helper.GetRowCount(sheetName2));
            }
        }

        #endregion

        #region Show

        [TestMethod]
        public void TestShowRespondsToMap()
        {
            var riskRegister = GetEntityFactory<RiskRegisterAsset>().Create();
            InitializeControllerAndRequest("~/Engineering/RiskRegisterAsset/Show/" + riskRegister.Id + ".map");

            var result = (MapResult)_target.Show(riskRegister.Id);
            var resultModel = result.CoordinateSets.Single().Coordinates.ToArray();

            Assert.AreEqual(1, resultModel.Count());
            Assert.IsTrue(resultModel.Contains(riskRegister));
        }

        [TestMethod]
        public void TestShowRespondsToFrag()
        {
            var riskRegister = GetEntityFactory<RiskRegisterAsset>().Create();
            InitializeControllerAndRequest("~/Engineering/RiskRegisterAsset/Show/" + riskRegister.Id + ".frag");

            var result = _target.Show(riskRegister.Id) as PartialViewResult;

            MvcAssert.IsViewNamed(result, "_ShowPopup");
            MvcAssert.IsViewWithModel(result, riskRegister);
        }

        #endregion
    }
}
