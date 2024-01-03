using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Facilities.Controllers;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Controllers;
using MMSINC.Data;

namespace MapCallMVC.Tests.Areas.Facilities.Controllers
{
    [TestClass]
    public class FacilitySystemDeliveryEntryTypeControllerTest : MapCallMvcControllerTestBase<FacilitySystemDeliveryEntryTypeController, FacilitySystemDeliveryEntryType>
    {
        #region Init

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            // TODO: This test is likely going to be flakey because there's a RequiredWhen
            // on PurchaseSupplier that relies on SystemDeliveryEntryType, but that factory/entity
            // is not done as readonly.
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditFacilitySystemDeliveryEntryType)vm;
                model.PurchaseSupplier = "Someone";
                model.BusinessUnit = 123456;
            };
        }

        #endregion

        #region Roles

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                var module = RoleModules.ProductionSystemDeliveryConfiguration;
                a.RequiresRole("~/FacilitySystemDeliveryEntryType/Edit", module, RoleActions.Edit);
                a.RequiresRole("~/FacilitySystemDeliveryEntryType/Update", module, RoleActions.Edit);
            });
        }

        #endregion

         #region Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            _currentUser.IsAdmin = true;
            var deliveryType = GetFactory<FacilitySystemDeliveryEntryTypeFactory>().Create();

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditFacilitySystemDeliveryEntryType, FacilitySystemDeliveryEntryType>(deliveryType, new {
                Facility = GetEntityFactory<Facility>().Create().Id
            }));

            Assert.AreEqual(1, Session.Get<FacilitySystemDeliveryEntryType>(deliveryType.Id).Facility.Id);
        }

        [TestMethod]
        public override void TestUpdateRedirectsToShowActionAfterSuccessfulSave()
        {
            var entity = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create();
            var model = _viewModelFactory.BuildWithOverrides<EditFacilitySystemDeliveryEntryType, FacilitySystemDeliveryEntryType>(entity, new {Id = entity.Id});
            var result = (RedirectResult)_target.Update(model);

            Assert.AreEqual($"/Facility/Show/{entity.Id}#SystemDeliveryTab", result.Url);

        }

        #endregion

        #region Lookups

        [TestMethod]
        public void TestOperatingCenterDropDownLookupForEditSetsActiveOC()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = true });
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = false });

            _authenticationService.Setup(x => x.CurrentUserIsAdmin).Returns(true);

            _target.SetLookupData(ControllerAction.Edit);

            var opcDropDownData = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];

            // Need to filter out operating center created during _authentication service setup
            var _authenticationServiceOc = _authenticationService.Object.CurrentUser.DefaultOperatingCenter.Id.ToString();

            var dropDownData = opcDropDownData.Where(x => x.Value != _authenticationServiceOc);

            Assert.AreEqual(1, dropDownData.Count());
            Assert.AreEqual(opc1.Id.ToString(), dropDownData.First().Value);
            Assert.AreNotEqual(opc2.Id.ToString(), dropDownData.First().Value);
        }

        #endregion
    }
}
