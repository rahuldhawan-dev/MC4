using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels.OperatorLicenses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class OperatorLicenseControllerTest
        : MapCallMvcControllerTestBase<OperatorLicenseController, OperatorLicense, OperatorLicenseRepository>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateOperatorLicense)vm;
                model.Status = GetEntityFactory<EmployeeStatus>().Create().Id;
            };
        }

        #endregion

        #region Exposed Methods

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var role = OperatorLicenseController.ROLE;
                a.RequiresRole("~/OperatorLicense/Search/", role);
                a.RequiresRole("~/OperatorLicense/Index/", role);
                a.RequiresRole("~/OperatorLicense/Show/", role);
                a.RequiresRole("~/OperatorLicense/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/OperatorLicense/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/OperatorLicense/New/", role, RoleActions.Add);
                a.RequiresRole("~/OperatorLicense/Create/", role, RoleActions.Add);
                a.RequiresRole("~/OperatorLicense/Destroy/", role, RoleActions.Delete);
                a.RequiresRole("~/OperatorLicense/AddWasteWaterSystem/", role, RoleActions.Edit);
                a.RequiresRole("~/OperatorLicense/RemoveWasteWaterSystem/", role, RoleActions.Edit);
            });
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            DateTime date0 = DateTime.Now, date1 = DateTime.Now;
            var oType = GetEntityFactory<OperatorLicenseType>().Create(new {Description = "Yes"});
            var oType1 = GetEntityFactory<OperatorLicenseType>().Create(new {Description = "No"});
            var entity0 = GetEntityFactory<OperatorLicense>().Create(new {ValidationDate = date0, OperatorLicenseType = oType});
            var entity1 = GetEntityFactory<OperatorLicense>().Create(new {ValidationDate = date1, OperatorLicenseType = oType1});
            var search = new SearchOperatorLicense();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = (ExcelResult)_target.Index(search);

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.ValidationDate, "ValidationDate");
                helper.AreEqual(entity1.ValidationDate, "ValidationDate", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<OperatorLicense>().Create();
            var date = DateTime.Now;

            _target.Update(_viewModelFactory.BuildWithOverrides<OperatorLicenseViewModel, OperatorLicense>(eq, new {
                ValidationDate = date
            }));
            eq = Session.Get<OperatorLicense>(eq.Id);

            Assert.AreEqual(date, eq.ValidationDate);
        }

        #endregion
        
        #region Add/Remove WasteWaterSystems

        [TestMethod]
        public void TestAddWasteWaterSystemAddsWasteWaterSystemToOperatorLicense()
        {
            var license = GetEntityFactory<OperatorLicense>().Create();
            var system = GetEntityFactory<WasteWaterSystem>().Create();

            var model = new AddOperatorLicenseWasteWaterSystem(_container) {
                Id = license.Id,
                WasteWaterSystem = system.Id
            };

            _target.AddWasteWaterSystem(model);

            var reloaded = Session.Load<OperatorLicense>(license.Id);
            
            Assert.AreEqual(system, reloaded.WasteWaterSystems.Single());
        }

        [TestMethod]
        public void TestAddWasteWaterSystemRedirectsToOperatorLicenseShowPageWhenSuccessful()
        {
            var license = GetEntityFactory<OperatorLicense>().Create();
            var system = GetEntityFactory<WasteWaterSystem>().Create();

            var model = new AddOperatorLicenseWasteWaterSystem(_container) {
                Id = license.Id,
                WasteWaterSystem = system.Id
            };

            var result = _target.AddWasteWaterSystem(model);
            
            MvcAssert.RedirectsToRoute(result, new {
                action = "Show",
                controller = nameof(OperatorLicense),
                id = license.Id
            });
        }

        [TestMethod]
        public void TestRemoveWasteWaterSystemRemovesWasteWaterSystemFromOperatorLicense()
        {
            var license = GetEntityFactory<OperatorLicense>().Create();
            var system = GetEntityFactory<WasteWaterSystem>().Create();
            license.WasteWaterSystems.Add(system);
            Session.Save(license);
            Session.Flush();

            var model = new RemoveOperatorLicenseWasteWaterSystem(_container) {
                Id = license.Id,
                WasteWaterSystem = system.Id
            };

            _target.RemoveWasteWaterSystem(model);

            var reloaded = Session.Load<OperatorLicense>(license.Id);
            
            Assert.AreEqual(0, reloaded.WasteWaterSystems.Count);
        }

        [TestMethod]
        public void TestRemoveWasteWaterSystemRedirectsToOperatorLicenseShowPageWhenSuccessful()
        {
            var license = GetEntityFactory<OperatorLicense>().Create();
            var system = GetEntityFactory<WasteWaterSystem>().Create();
            license.WasteWaterSystems.Add(system);
            Session.Save(license);
            Session.Flush();

            var model = new RemoveOperatorLicenseWasteWaterSystem(_container) {
                Id = license.Id,
                WasteWaterSystem = system.Id
            };

            var result = _target.RemoveWasteWaterSystem(model);
            
            MvcAssert.RedirectsToRoute(result, new {
                action = "Show",
                controller = nameof(OperatorLicense),
                id = license.Id
            });
        }

        [TestMethod]
        public void TestWasteWaterSystemsAreSorted()
        {
            var oc1 = GetEntityFactory<OperatingCenter>().Create(new {
                OperatingCenterCode = "XYZ"
            });
            var oc2 = GetEntityFactory<OperatingCenter>().Create(new {
                OperatingCenterCode = "DEF"
            });
            var oc3 = GetEntityFactory<OperatingCenter>().Create(new {
                OperatingCenterCode = "ABC"
            });
            var wws1 = GetEntityFactory<WasteWaterSystem>().Create(new {
                Id=111,
                WasteWaterSystemName="City WasteWater",
                OperatingCenter = oc1
            });
            var wws2 = GetEntityFactory<WasteWaterSystem>().Create(new {
                Id=222,
                WasteWaterSystemName="Village WasteWater",
                OperatingCenter = oc2
            });
            var wws3 = GetEntityFactory<WasteWaterSystem>().Create(new {
                Id=333,
                WasteWaterSystemName="Town WasteWater",
                OperatingCenter = oc3
            });
            var descriptions = new [] { wws1.Description, wws2.Description, wws3.Description };
            Array.Sort(descriptions);
            
            _target.SetLookupData(ControllerAction.Show);
            var vd = (IEnumerable<SelectListItem>)_target.ViewData["WasteWaterSystem"];
            
            Assert.AreEqual(vd.ElementAt(0).Text, descriptions[0]);
            Assert.AreEqual(vd.ElementAt(1).Text, descriptions[1]);
            Assert.AreEqual(vd.ElementAt(2).Text, descriptions[2]);
        }
        
        #endregion
    }
}
