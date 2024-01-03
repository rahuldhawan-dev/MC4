using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class WorkDescriptionControllerTest
        : MapCallMvcControllerTestBase<WorkDescriptionController, WorkDescription>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => {
                var desc = GetFactory<WaterMainBleedersWorkDescriptionFactory>().Create();
                Session.Save(desc);
                Session.Flush();
                return desc;
            };
        }

        #endregion
        
        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.FieldServicesWorkManagement;
                a.RequiresRole("~/FieldOperations/WorkDescription/Search", module, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/WorkDescription/Index", module, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/WorkDescription/Show", module, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/WorkDescription/Edit", module, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/WorkDescription/Update", module, RoleActions.UserAdministrator);
                a.RequiresLoggedInUserOnly("~/FieldOperations/WorkDescription/ActiveByAssetTypeId/");
                a.RequiresLoggedInUserOnly("~/FieldOperations/WorkDescription/UsedByAssetTypeIds/");
                a.RequiresLoggedInUserOnly("~/FieldOperations/WorkDescription/ActiveByAssetTypeIdForCreate/");
                a.RequiresLoggedInUserOnly("~/FieldOperations/WorkDescription/ActiveByAssetTypeIdAndIsRevisit/");
            });
        }

        #endregion

        #region Show

        [TestMethod]
        public void TestShowJsonReturnsJson()
        {
            var entity = GetEntityFactory<WorkDescription>().Create(new { DigitalAsBuiltRequired = true });
            InitializeControllerAndRequest("~/FieldOperations/WorkDescription/Show" + entity.Id + ".json");

            var result = _target.Show(entity.Id) as JsonResult;
            var resultData = (dynamic)result.Data;

            Assert.IsNotNull(result);
            Assert.IsTrue(resultData.DigitalAsBuiltRequired);
        }

        #endregion

        #region ActiveByAssetTypeIdForCreate

        [TestMethod]
        public void Test_ByAssetTypeIdForCreate_ReturnsInitialDescriptions_WhenInitial()
        {
            var hydrantAssetType = GetFactory<HydrantAssetTypeFactory>().Create();
            var initialWorkDescription = GetFactory<HydrantInstallationWorkDescriptionFactory>().Create(new {
                AssetType = hydrantAssetType
            });
            var revisitWorkDescription = GetFactory<HydrantLandscapingWorkDescriptionFactory>().Create(new {
                AssetType = hydrantAssetType
            });
            Session.Flush();

            var results = (CascadingActionResult)_target.ActiveByAssetTypeIdForCreate(hydrantAssetType.Id, false);
            var data = results.GetSelectListItems().ToArray();

            Assert.AreEqual(2, data.Count());
            Assert.AreEqual(initialWorkDescription.Description, data.Last().Text);
            Assert.AreEqual(initialWorkDescription.Id.ToString(), data.Last().Value);
        }
        
        #endregion
        
        #region ActiveByAssetTypeIdForCreate

        [TestMethod]
        public void Test_ByAssetTypeIdForCreate_ReturnsRevisitDescriptions_WhenRevisit()
        {
            var hydrantAssetType = GetFactory<HydrantAssetTypeFactory>().Create();
            var initialWorkDescription = GetFactory<HydrantInstallationWorkDescriptionFactory>().Create(new {
                AssetType = hydrantAssetType
            });
            var revisitWorkDescription = GetFactory<HydrantLandscapingWorkDescriptionFactory>().Create(new {
                AssetType = hydrantAssetType
            });
            Session.Flush();

            var results = (CascadingActionResult)_target.ActiveByAssetTypeIdForCreate(hydrantAssetType.Id, true);
            var data = results.GetSelectListItems().ToArray();

            Assert.AreEqual(2, data.Count());
            Assert.AreEqual(revisitWorkDescription.Description, data.Last().Text);
            Assert.AreEqual(revisitWorkDescription.Id.ToString(), data.Last().Value);
        }
        
        #endregion
        
        #region ActiveByAssetTypeIdAndIsRevisit

        [TestMethod]
        public void Test_ByAssetTypeIdAndIsRevisit_ReturnsRevisitDescriptions_WhenRevisit()
        {
            var hydrantAssetType = GetFactory<HydrantAssetTypeFactory>().Create();
            var initialWorkDescription = GetFactory<HydrantInstallationWorkDescriptionFactory>().Create(new {
                AssetType = hydrantAssetType
            });
            var revisitWorkDescription = GetFactory<HydrantLandscapingWorkDescriptionFactory>().Create(new {
                AssetType = hydrantAssetType
            });
            Session.Flush();

            var results = (CascadingActionResult)_target.ActiveByAssetTypeIdAndIsRevisit(
                hydrantAssetType.Id,
                true);
            var data = results.GetSelectListItems().ToArray();

            Assert.AreEqual(2, data.Count());
            Assert.AreEqual(revisitWorkDescription.Description, data.Last().Text);
            Assert.AreEqual(revisitWorkDescription.Id.ToString(), data.Last().Value);
        }
        
        #endregion
        
        #region ActiveByAssetTypeIdAndIsRevisit

        [TestMethod]
        public void Test_ByAssetTypeIdAndIsRevisit_ReturnsInitialDescriptions_WhenNotRevisit()
        {
            var hydrantAssetType = GetFactory<HydrantAssetTypeFactory>().Create();
            var initialWorkDescription = GetFactory<HydrantInstallationWorkDescriptionFactory>().Create(new {
                AssetType = hydrantAssetType
            });
            var revisitWorkDescription = GetFactory<HydrantLandscapingWorkDescriptionFactory>().Create(new {
                AssetType = hydrantAssetType
            });
            Session.Flush();

            var results = (CascadingActionResult)_target.ActiveByAssetTypeIdAndIsRevisit(
                hydrantAssetType.Id,
                false);
            var data = results.GetSelectListItems().ToArray();

            Assert.AreEqual(2, data.Count());
            Assert.AreEqual(initialWorkDescription.Description, data.Last().Text);
            Assert.AreEqual(initialWorkDescription.Id.ToString(), data.Last().Value);
        }
        
        #endregion
        
        #region UsedByAssetTypeIds

        [TestMethod]
        public void Test_UsedByAssetTypeIds_ReturnsCorrectWorkDescriptions()
        {
            var hydrantAssetType = GetFactory<HydrantAssetTypeFactory>().Create();
            var sewerAssetType = GetFactory<SewerMainAssetTypeFactory>().Create();
            var hydrantWorkDescription = GetFactory<HydrantFlushingWorkDescriptionFactory>().Create(new {
                AssetType = hydrantAssetType
            });
            GetFactory<WorkOrderFactory>()
               .Create(new {WorkDescription = hydrantWorkDescription});
            var sewerWorkDescription = GetFactory<SewerInvestigationMainWorkDescriptionFactory>()
               .Create(new {
                    AssetType = sewerAssetType, Description = "This description is not used by any WorkOrder!"
                });
            Session.Flush();

            var results = (CascadingActionResult)_target.UsedByAssetTypeIds(new[] {
                hydrantAssetType.Id,
                sewerAssetType.Id
            });
            var data = ((IEnumerable<dynamic>)results.Data).ToList();

            Assert.AreEqual(1, data.Count());
            Assert.AreEqual(hydrantWorkDescription.Id, data.First().Id);
        }
        
        #endregion
    }
}