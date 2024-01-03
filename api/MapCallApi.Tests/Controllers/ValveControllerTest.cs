using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallApi.Controllers;
using MapCallApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MapCallApi.Tests.Controllers
{
    [TestClass]
    public class ValveControllerTest : MapCallApiControllerTestBase<ValveController, Valve, ValveRepository>
    {
        #region Init/Cleanup
        
        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                GetFactory<NsiPendingAssetStatusFactory>().Create();
                var model = (CreateValve)vm;
                var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
                var town = GetEntityFactory<Town>().Create();
                town.OperatingCentersTowns.Add(new OperatingCenterTown
                    { OperatingCenter = operatingCenter, Town = town, Abbreviation = "XX" });
                model.OperatingCenter = operatingCenter.Id;
                model.Town = town.Id;
                model.CrossStreet = GetEntityFactory<Street>().Create().Id;
            };
        }

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.FieldServicesAssets;
            Authorization.Assert(a => {
                SetupHttpAuth(a);
                a.RequiresRole("~/Valve/Create", role);
            });
        }

        [TestMethod]
        public override void TestCreateRedirectsToTheRecordsShowPageAfterSuccessfullySaving()
        {
            //no-op since we don't redirect to the show page.
        }

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            //no-op since we don't redirect to the show page.
        }

        [TestMethod]
        public void TestCreateReturnsBadRequestWhenRequiredFieldsAreNotProvided()
        {
            _target.ModelState.AddModelError("Town", "Required");
            var result = (JsonResult)_target.Create(new CreateValve(_container));

            Request.Response.VerifySet(x => x.StatusCode = 400);
            var error = (result.Data.GetPropertyValueByName("Errors") as Dictionary<string, List<string>>).FirstOrDefault();
            Assert.AreEqual("Town", error.Key);
            Assert.AreEqual("Required", error.Value.FirstOrDefault());
        }

        [TestMethod]
        public void TestCreateReturnsResultsForValidInput()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = operatingCenter, Town = town, Abbreviation = "XX" });
            var valveSize = GetEntityFactory<ValveSize>().Create();
            var street = GetEntityFactory<Street>().Create();
            GetEntityFactory<ValveBilling>().Create();
            GetFactory<NsiPendingAssetStatusFactory>().Create();
            GetFactory<HydrantValveControlFactory>().Create();

            var createValve = new CreateValve(_container) {
                OperatingCenter = operatingCenter.Id,
                Town = town.Id,
                ValveSize = valveSize.Id,
                Street = street.Id,
                CrossStreet = street.Id,
                WbsNumber = "D25-1401-P-0323",
                Longitude = -87.0289681M,
                Latitude = 41.6170238M
            };

            var result = _target.Create(createValve) as JsonResult;
            Assert.IsNotNull(result);
            var id = Convert.ToInt32(result.Data.GetPropertyValueByName("Id"));
            Assert.AreEqual(1, id);
            Valve valve = Repository.Find(id);
            Assert.IsNotNull(valve);
            Assert.AreEqual(operatingCenter.Id, valve.OperatingCenter.Id);
            Assert.AreEqual(town.Id, valve.Town.Id);
            Assert.AreEqual(valveSize.Id, valve.ValveSize.Id);
            Assert.AreEqual(street.Id, valve.Street.Id);
            Assert.AreEqual(street.Id, valve.CrossStreet.Id);
            Assert.AreEqual("D25-1401-P-0323", valve.WorkOrderNumber);
            Assert.AreEqual(ValveBilling.Indices.PUBLIC, valve.ValveBilling.Id);
            Assert.AreEqual(AssetStatus.Indices.NSI_PENDING, valve.Status.Id);
            Assert.AreEqual(AuthenticationService.Object.CurrentUser.Id, valve.Initiator.Id);
            Assert.IsNotNull(valve.ValveNumber);
            Assert.IsNotNull(valve.ValveSuffix);
            StringAssert.Contains(valve.SAPErrorCode, "RETRY::INITIAL RECORD CREATED");
            Assert.AreEqual(ValveControl.Indices.HYDRANT, valve.ValveControls.Id);
            Assert.IsNotNull(valve.Coordinate);
        }

        #endregion
    }
}
