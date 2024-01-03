using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Production.Controllers;
using MapCallMVC.Areas.Production.Models.ViewModels;
using MapCallMVC.Areas.Production.Models.ViewModels.SystemDeliveryEntries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Production.Controller
{
    [TestClass]
    public class SystemDeliveryEntryControllerTest : MapCallMvcControllerTestBase<SystemDeliveryEntryController, SystemDeliveryEntry, SystemDeliveryEntryRepository>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create(new {Employee = GetEntityFactory<Employee>().Create()});
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            var mondayDate = new DateTime(2020, 12, 7);
            
            options.CreateValidEntity = () => {
                var facility = GetEntityFactory<Facility>().Create();
                var entity = GetEntityFactory<SystemDeliveryEntry>().Create();
                entity.Facilities.Add(facility);
                return entity;
            };
            options.InitializeCreateViewModel = (model) => {
                ((CreateSystemDeliveryEntryViewModel)model).WeekOf = mondayDate;
            };
            options.CreateRedirectsToRouteOnSuccessArgs = (model) => {
                var viewModel = (CreateSystemDeliveryEntryViewModel)model;
                viewModel.WeekOf = mondayDate;
                Assert.AreNotEqual(0, viewModel.Id);
                return new {action = "Edit", controller = "SystemDeliveryEntry", id = viewModel.Id};
            };
        }

        #endregion

        #region Helpers

        private SystemDeliveryFacilityEntry SetupSystemDeliveryFacilityEntry()
        {
            var facility = GetEntityFactory<Facility>().Create(new { RegionalPlanningArea = "TestFunctionalLocation" });
            var entity = GetEntityFactory<SystemDeliveryEntry>().Create();
            var entry = GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new { Facility = facility, SystemDeliveryEntry = entity });
            
            // setting a business unit with entry type same as entry
            var entrySystemDeliveryEntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new {
                SystemDeliveryEntryType = GetEntityFactory<SystemDeliveryEntryType>().Create(),
                BusinessUnit = 22222
            });
            entrySystemDeliveryEntryType.SystemDeliveryEntryType.Id = entry.SystemDeliveryEntryType.Id; // same as entry's entry type

            // setting business unit with entry type different from entry
            var facilitySystemDeliveryEntryTypeWithDifferentEntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new {
                SystemDeliveryEntryType = GetEntityFactory<SystemDeliveryEntryType>().Create(),
                BusinessUnit = 33333,
                IsEnabled = false
            });
            facilitySystemDeliveryEntryTypeWithDifferentEntryType.SystemDeliveryEntryType.Id =
                SystemDeliveryEntryType.Indices.TRANSFERRED_FROM;

            facility.FacilitySystemDeliveryEntryTypes = new List<FacilitySystemDeliveryEntryType> {
                entrySystemDeliveryEntryType,
                facilitySystemDeliveryEntryTypeWithDifferentEntryType
            };

            entity.Facilities.Add(facility);
            entity.FacilityEntries.Add(entry);
            Session.Save(facility);
            Session.Save(entity);
            return entry;
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const RoleModules role = SystemDeliveryEntryController.ROLE;
                const string path = "~/SystemDeliveryEntry/";
                a.RequiresRole(path + "Search", role, RoleActions.Read);
                a.RequiresRole(path + "Show", role, RoleActions.Read);
                a.RequiresRole(path + "Create", role, RoleActions.Add);
                a.RequiresRole(path + "New", role, RoleActions.Add);
                a.RequiresRole(path + "Copy", role, RoleActions.Add);
                a.RequiresRole(path + "Edit", role, RoleActions.Edit);
                a.RequiresRole(path + "Update", role, RoleActions.Edit);
                a.RequiresRole(path + "ValidateAndSubmit", SystemDeliveryEntryController.VALIDATOR_ROLE, RoleActions.Add);
                a.RequiresRole(path + "AddSystemDeliveryEquipmentEntryReversal", role, RoleActions.Edit);
                a.RequiresSiteAdminUser(path + "Destroy");
            });
        }

        [TestMethod]
        public void TestValidateAndSubmitUpdatesEntityAndRedirectsToShow()
        {
            var entity = GetEntityFactory<SystemDeliveryEntry>().Create();
            var viewModel = _viewModelFactory.Build<ValidateSystemDeliveryEntryViewModel, SystemDeliveryEntry>(entity);

            var result = _target.ValidateAndSubmit(viewModel);

            Assert.AreEqual(true, Session.Get<SystemDeliveryEntry>(entity.Id).IsValidated);
            MvcAssert.RedirectsToRoute(result, "SystemDeliveryEntry", "Show", new { id = entity.Id });
        }

        [TestMethod]
        public void TestAddSystemDeliveryFacilityEntryAdjustmentAddsAdjustmentAndRedirectsToShow()
        {
            var systemDeliveryEntryType = GetEntityFactory<SystemDeliveryEntryType>().Create();
            var facility = GetEntityFactory<Facility>().Create();
            var facilitySystemDelivery = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new {SystemDeliveryEntryType = systemDeliveryEntryType, IsEnabled = true, MaximumValue = 2.32m, MinimumValue = 1.25m });
            facility.FacilitySystemDeliveryEntryTypes.Add(facilitySystemDelivery);
            var entity = GetEntityFactory<SystemDeliveryEntry>().Create();
            var entry = GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new {Facility = facility, SystemDeliveryEntryType = systemDeliveryEntryType});
            entity.FacilityEntries.Add(entry);
            var viewModel = _viewModelFactory.Build<AddSystemDeliveryEquipmentEntryReversal, SystemDeliveryEntry>(entity);
            var viewModelEntry = entry;
            viewModelEntry.IsBeingAdjusted = true;
            viewModelEntry.AdjustedEntryValue = 2.00m;
            viewModelEntry.AdjustmentComment = string.Empty;
            viewModel.FacilityEntries.Add(_viewModelFactory.Build<EditSystemDeliveryFacilityEntry, SystemDeliveryFacilityEntry>(viewModelEntry));

            var result = _target.AddSystemDeliveryEquipmentEntryReversal(viewModel);

            Assert.AreEqual(1, Session.Get<SystemDeliveryEntry>(entity.Id).FacilityEntries.First().Adjustments.Count);
            Assert.AreEqual(entry.Id, Session.Get<SystemDeliveryEntry>(entity.Id).FacilityEntries.First().Adjustments.First().SystemDeliveryFacilityEntry.Id);
            MvcAssert.RedirectsToRoute(result, "SystemDeliveryEntry", "Show", new { id = entity.Id });
        }

        [TestMethod]
        public void TestCopyCopiesOperatingCenterAndFacilitiesAndRedirectsToNew()
        {
            // Data setup
            var systemDeliveryEntry = GetEntityFactory<SystemDeliveryEntry>().Create();
            var operatingCenters = GetFactory<UniqueOperatingCenterFactory>().CreateList(2);
            var facilities = GetEntityFactory<Facility>().CreateList(2);
            var publicWaterSupplies = GetEntityFactory<PublicWaterSupply>().CreateList(2);
            var wasteWaterSystems = GetEntityFactory<WasteWaterSystem>().CreateList(2);

            systemDeliveryEntry.OperatingCenters.Add(operatingCenters.First());
            systemDeliveryEntry.OperatingCenters.Add(operatingCenters.Last());

            systemDeliveryEntry.Facilities.Add(facilities.First());
            systemDeliveryEntry.Facilities.Add(facilities.Last());

            systemDeliveryEntry.PublicWaterSupplies.Add(publicWaterSupplies.First());
            systemDeliveryEntry.PublicWaterSupplies.Add(publicWaterSupplies.Last());

            systemDeliveryEntry.WasteWaterSystems.Add(wasteWaterSystems.First());
            systemDeliveryEntry.WasteWaterSystems.Add(wasteWaterSystems.Last());

            Session.SaveOrUpdate(systemDeliveryEntry);
            Session.Flush();

            // Act
            var result = _target.Copy(systemDeliveryEntry.Id);
            var model = ((ViewResult)result).Model;
            var actualModel = (CreateSystemDeliveryEntryViewModel)model;

            // Assert 
            MvcAssert.IsViewNamed(result, "New");
            Assert.IsInstanceOfType(model, typeof(CreateSystemDeliveryEntryViewModel));
            CollectionAssert.AreEqual(actualModel.OperatingCenters, operatingCenters.Select(x => x.Id).ToArray());
            CollectionAssert.AreEqual(actualModel.Facilities, facilities.Select(x => x.Id).ToArray());
            CollectionAssert.AreEqual(actualModel.PublicWaterSupplies, publicWaterSupplies.Select(x => x.Id).ToArray());
            CollectionAssert.AreEqual(actualModel.WasteWaterSystems, wasteWaterSystems.Select(x => x.Id).ToArray());
        }

        [TestMethod]
        public void TestCopyIsNotFoundReturns404WhenEntityDoesNotExist()
        {
            var result = _target.Copy(0);
            MvcAssert.IsNotFound(result);
            MvcAssert.IsStatusCode(404, result);
        }

        [TestMethod]
        public void TestUpdateWillRedirectBackToEditIfFacilityListChanged()
        {
            var entity = GetEntityFactory<SystemDeliveryEntry>().Create();
            var facility = GetEntityFactory<Facility>().Create();
            var facility2 = GetEntityFactory<Facility>().Create();
            entity.Facilities.Add(facility);

            var viewModel = _viewModelFactory.Build<EditSystemDeliveryEntryViewModel, SystemDeliveryEntry>(entity);
            viewModel.Facilities = new[] {facility.Id, facility2.Id};

            var result = _target.Update(viewModel);

            MvcAssert.RedirectsToRoute(result, "SystemDeliveryEntry", "Edit", new { id = entity.Id });
        }

        [TestMethod]
        public override void TestSearchReturnsSearchViewWithModel()
        {
            var expected = _container.GetInstance<SearchSystemDeliveryFacilityEntry>();

            var result = (ViewResult)_target.Search(expected);

            MvcAssert.IsViewWithNameAndModel(result, "Search", expected, "SystemDeliveryEntryController.Search did not return view with expected view name or model.");
        }

        #endregion
    }
}