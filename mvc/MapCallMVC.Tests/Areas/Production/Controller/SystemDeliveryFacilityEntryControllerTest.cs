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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.BooleanExtensions;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Production.Controller
{
    [TestClass]
    public class SystemDeliveryFacilityEntryControllerTest : MapCallMvcControllerTestBase<SystemDeliveryFacilityEntryController, SystemDeliveryFacilityEntry, SystemDeliveryFacilityEntryRepository>
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
                const RoleModules role = SystemDeliveryFacilityEntryController.ROLE;
                const string path = "~/SystemDeliveryFacilityEntry/";
                a.RequiresRole(path + "Index", role, RoleActions.Read);
                a.RequiresRole(path + "Search", role, RoleActions.Read);
            });
        }
        
        // Need to override due to how the base test is setup
        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            var entity = GetEntityFactory<SystemDeliveryEntry>().Create();
            var entityWithNoEquipmentEntries = GetEntityFactory<SystemDeliveryEntry>().Create();
            var entry = GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new {SystemDeliveryEntry = entity});
            var entry1 = GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new {SystemDeliveryEntry = entityWithNoEquipmentEntries });
            entity.FacilityEntries.Add(entry);
            entityWithNoEquipmentEntries.FacilityEntries.Add(entry1);
            
            Session.Save(entity);
            Session.Save(entityWithNoEquipmentEntries);
            
            var search = new SearchSystemDeliveryFacilityEntry();
            var result = (ViewResult)_target.Index(search);
            Assert.IsInstanceOfType(result.Model, typeof(SearchSystemDeliveryFacilityEntry));
            var results = ((SearchSystemDeliveryFacilityEntry)result.Model).Results.ToList();

            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(entity.Id, results.First().SystemDeliveryEntry.Id);
            Assert.AreEqual(entry.EntryDate, results.First().EntryDate);
            Assert.AreEqual(entity.IsHyperionFileCreated, results.First().SystemDeliveryEntry.IsHyperionFileCreated);
        }

        [TestMethod]
        public void TestIndexReturnsYesWhenIsValidatedIsTrue()
        {
            var entity = GetEntityFactory<SystemDeliveryEntry>().Create();
            var entry = GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new { SystemDeliveryEntry = entity });
            entity.FacilityEntries.Add(entry);
            entity.IsValidated = true;
            Session.Save(entity);

            var search = new SearchSystemDeliveryFacilityEntry();
            var result = (ViewResult)_target.Index(search);
            var results = ((SearchSystemDeliveryFacilityEntry)result.Model).Results.ToList();

            Assert.AreEqual((entity.IsValidated ?? false), results.First().SystemDeliveryEntry.IsValidated);
        }

        [TestMethod]
        public void TestIndexReturnsNoWhenIsValidatedIsFalse()
        {
            var entity = GetEntityFactory<SystemDeliveryEntry>().Create();
            var entry = GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new { SystemDeliveryEntry = entity });
            entity.FacilityEntries.Add(entry);
            entity.IsValidated = false;
            Session.Save(entity);

            var search = new SearchSystemDeliveryFacilityEntry();
            var result = (ViewResult)_target.Index(search);
            var results = ((SearchSystemDeliveryFacilityEntry)result.Model).Results.ToList();

            Assert.AreEqual((entity.IsValidated ?? false), results.First().SystemDeliveryEntry.IsValidated);
        }

        [TestMethod]
        public void TestIndexReturnsNoWhenIsValidatedIsNull()
        {
            var entity = GetEntityFactory<SystemDeliveryEntry>().Create();
            var entry = GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new { SystemDeliveryEntry = entity });
            entity.FacilityEntries.Add(entry);
            entity.IsValidated = null;
            Session.Save(entity);

            var search = new SearchSystemDeliveryFacilityEntry();
            var result = (ViewResult)_target.Index(search);
            var results = ((SearchSystemDeliveryFacilityEntry)result.Model).Results.ToList();

            Assert.AreEqual(entity.IsValidated, results.First().SystemDeliveryEntry.IsValidated);
        }

        [TestMethod]
        public void TestIndexXlsExportsExcel()
        {
            var pws = GetEntityFactory<PublicWaterSupply>().Create();
            var facility = GetEntityFactory<Facility>().Create(new { RegionalPlanningArea = "TestFunctionalLocation", PublicWaterSupply = pws });
            facility.FacilitySystemDeliveryEntryTypes.Add(GetEntityFactory<FacilitySystemDeliveryEntryType>().Create());
            var entity = GetEntityFactory<SystemDeliveryEntry>().Create();
            var entry = GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new { Facility = facility, SystemDeliveryEntry = entity });
            entity.FacilityEntries.Add(entry);
            
            Session.Save(facility);
            Session.Save(entity);
            
            var search = new SearchSystemDeliveryFacilityEntry();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entry.EntryDate, "Date");
                // helper.AreEqual(entry.TuesdayEntryDate, "Date", 1);
                helper.AreEqual(entry.Facility.OperatingCenter, "OperatingCenter");
                helper.AreEqual(entry.SystemDeliveryType, "SystemDeliveryType");
                helper.AreEqual(entry.Facility.FacilityIdWithFacilityName, "Facility");
                helper.AreEqual(entry.SystemDeliveryEntryType.Description, "SystemDeliveryEntryType");
                helper.AreEqual((false).ToString("yn"), "Adjustment");
                helper.AreEqual(entry.EntryValue, "Value");
                helper.AreEqual(entry.EnteredBy, "Employee");
                helper.AreEqual(entry.Facility.PublicWaterSupply, "PublicWaterSupply");
                helper.AreEqual(string.Empty, "PurchaseSupplier");
                helper.AreEqual((false).ToString("yn"), "IsInjection");
                helper.AreEqual((false).ToString("yn"), "IsValidated");
                helper.AreEqual((false).ToString("yn"), "IsHyperionFileCreated");
                helper.AreEqual(entry.Facility.RegionalPlanningArea, "LegacyIdSd");
            }
        }

        [TestMethod]
        public void TestExportedExcelUsesBusinessUnitByMatchingEntryType()
        {
            var entry = SetupSystemDeliveryFacilityEntry();

            var search = new SearchSystemDeliveryFacilityEntry();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                // business unit needs to match by system delivery entry type
                helper.AreEqual(entry.Facility.FacilitySystemDeliveryEntryTypes
                                     .FirstOrDefault(f => f.SystemDeliveryEntryType.Id == entry.SystemDeliveryEntryType.Id)?
                                     .BusinessUnit, 
                    "BusinessUnit");
            }
        }

        [TestMethod]
        public void TestExportedExcelHasBusinessUnitForTransferredFromEntryTypeWhereIsEnabledIsFalse()
        {
            var entry = SetupSystemDeliveryFacilityEntry();
            entry.SystemDeliveryEntryType.Id = SystemDeliveryEntryType.Indices.TRANSFERRED_FROM;
            Session.Save(entry);

            var search = new SearchSystemDeliveryFacilityEntry();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                // business unit needs to be from Supplier Facility
                helper.AreEqual(entry.Facility.FacilitySystemDeliveryEntryTypes
                                     .FirstOrDefault(f => f.SystemDeliveryEntryType.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_FROM
                                                          && !f.IsEnabled)?.BusinessUnit,
                    "BusinessUnit");
            }
        }

        [TestMethod]
        public void TestExportedExcelHasEmptyBusinessUnitIfEntryTypeDoesNotExist()
        {
            var entry = SetupSystemDeliveryFacilityEntry();
            entry.SystemDeliveryEntryType.Id = SystemDeliveryEntryType.Indices.WASTEWATER_COLLECTED;
            Session.Save(entry);

            var search = new SearchSystemDeliveryFacilityEntry();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.IsNull("BusinessUnit");
            }
        }

        [TestMethod]
        public override void TestSearchReturnsSearchViewWithModel()
        {
            //this test is handled in SystemDeliveryEntryControllerTest.cs
            return;
        }

        #endregion
    }
}