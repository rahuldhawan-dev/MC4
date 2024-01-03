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
    public class HydrantControllerTest : MapCallApiControllerTestBase<HydrantController, Hydrant, HydrantRepository>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            // override CreateValidEntity to set Hydrant.LateralValve as CreateHydrant.MapToEntity reads CreateHydrant.Valve property
            options.CreateValidEntity = () => {
                var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
                var town = GetEntityFactory<Town>().Create();
                town.OperatingCentersTowns.Add(new OperatingCenterTown
                    { OperatingCenter = operatingCenter, Town = town, Abbreviation = "XX" });
                var street = GetEntityFactory<Street>().Create();
                GetEntityFactory<HydrantBilling>().Create();
                GetFactory<NsiPendingAssetStatusFactory>().Create();
                var valve = GetFactory<ValveFactory>().Create(new {
                    Town = town,
                    Street = street,
                    OperatingCenter = operatingCenter,
                    CrossStreet = street,
                });
                return GetEntityFactory<Hydrant>().BuildWithConcreteDependencies(new { OperatingCenter = operatingCenter, Town = town, Street = street, LateralValve = valve });
            };
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateHydrant)vm;
                model.Latitude = 1.234m;
                model.Longitude = 2.234m;
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
                a.RequiresRole("~/Hydrant/Create", role);
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
            _target.ModelState.AddModelError("Valve", "Required");
            var result = (JsonResult)_target.Create(new CreateHydrant(_container));

            Request.Response.VerifySet(x => x.StatusCode = 400);
            var error = (result.Data.GetPropertyValueByName("Errors") as Dictionary<string, List<string>>).FirstOrDefault();
            Assert.AreEqual("Valve", error.Key);
            Assert.AreEqual("Required", error.Value.FirstOrDefault());
        }

        [TestMethod]
        public void TestCreateReturnsResultsForValidInput()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = operatingCenter, Town = town, Abbreviation = "XX" });
            var street = GetEntityFactory<Street>().Create();
            GetEntityFactory<HydrantBilling>().Create();
            GetFactory<NsiPendingAssetStatusFactory>().Create();
            var valve = GetFactory<ValveFactory>().Create(new {
                Town = town,
                Street = street,
                OperatingCenter = operatingCenter,
                CrossStreet = street,
            });

            var createHydrant = new CreateHydrant(_container) {
                Valve = valve.Id,
                WbsNumber = "D25-1401-P-0323",
                Longitude = -87.0289681M,
                Latitude = 41.6170238M
            };

            var result = _target.Create(createHydrant) as JsonResult;
            Assert.IsNotNull(result);
            var id = Convert.ToInt32(result.Data.GetPropertyValueByName("Id"));
            Assert.AreEqual(1, id);
            Hydrant hydrant = Repository.Find(id);
            Assert.IsNotNull(hydrant);
            Assert.AreEqual(operatingCenter.Id, hydrant.OperatingCenter.Id);
            Assert.AreEqual(town.Id, hydrant.Town.Id);
            Assert.AreEqual(street.Id, hydrant.Street.Id);
            Assert.AreEqual(street.Id, hydrant.CrossStreet.Id);
            Assert.AreEqual("D25-1401-P-0323", hydrant.WorkOrderNumber);
            Assert.AreEqual(HydrantBilling.Indices.PUBLIC, hydrant.HydrantBilling.Id);
            Assert.AreEqual(AssetStatus.Indices.NSI_PENDING, hydrant.Status.Id);
            Assert.AreEqual(AuthenticationService.Object.CurrentUser.Id, hydrant.Initiator.Id);
            Assert.IsNotNull(hydrant.HydrantNumber);
            Assert.IsNotNull(hydrant.HydrantSuffix);
            StringAssert.Contains(hydrant.SAPErrorCode, "RETRY::INITIAL RECORD CREATED");
            Assert.IsNotNull(hydrant.Coordinate);
        }

        #endregion
    }
}
