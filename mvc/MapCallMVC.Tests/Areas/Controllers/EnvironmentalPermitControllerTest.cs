using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Environmental.Controllers;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalPermits;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using StructureMap;
using ControllerBase = MMSINC.Controllers.ControllerBase;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class EnvironmentalPermitControllerTest : MapCallMvcControllerTestBase<EnvironmentalPermitController, EnvironmentalPermit>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IFacilityRepository>().Use<FacilityRepository>();
            e.For<IEquipmentRepository>().Use<EquipmentRepository>();
        }

        #endregion

        #region Roles

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                var module = RoleModules.EnvironmentalGeneral;
                a.RequiresRole("~/Environmental/EnvironmentalPermit/Show/", module, RoleActions.Read);
                a.RequiresRole("~/Environmental/EnvironmentalPermit/Search/", module, RoleActions.Read);
                a.RequiresRole("~/Environmental/EnvironmentalPermit/Index/", module, RoleActions.Read);
                a.RequiresRole("~/Environmental/EnvironmentalPermit/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/Environmental/EnvironmentalPermit/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/Environmental/EnvironmentalPermit/New/", module, RoleActions.Add);
                a.RequiresRole("~/Environmental/EnvironmentalPermit/Create/", module, RoleActions.Add);
                a.RequiresRole("~/Environmental/EnvironmentalPermit/Destroy/", module, RoleActions.Delete);
                a.RequiresRole("~/Environmental/EnvironmentalPermit/AddFacility/", module, RoleActions.Edit);
                a.RequiresRole("~/Environmental/EnvironmentalPermit/RemoveFacility/", module, RoleActions.Edit);
                a.RequiresRole("~/Environmental/EnvironmentalPermit/AddEquipment/", module, RoleActions.Edit);
                a.RequiresRole("~/Environmental/EnvironmentalPermit/RemoveEquipment/", module, RoleActions.Edit);
                a.RequiresRole("~/Environmental/EnvironmentalPermit/AddEnvironmentalPermitRequirement/", module, RoleActions.Edit);
                a.RequiresRole("~/Environmental/EnvironmentalPermit/RemoveEnvironmentalPermitRequirement/", module, RoleActions.Edit);
                a.RequiresRole("~/Environmental/EnvironmentalPermit/RemoveEnvironmentalPermitFee/", module, RoleActions.Edit);
            });
        }

        #endregion

        #region Show

        [TestMethod]
        public void TestShowAddsEquipmentForState()
        {
            var nj = GetEntityFactory<State>().Create(new { Abbreviation = "NJ", Name = "New Jersey"});
            var ny = GetEntityFactory<State>().Create(new { Abbreviation = "NY", Name = "New York" });
            var nj7 = GetFactory<UniqueOperatingCenterFactory>().Create(new {State = nj, OperatingCenterCode = "NJ7" });
            var ny1 = GetFactory<UniqueOperatingCenterFactory>().Create(new {State = ny, OperatingCenterCode = "NY1" });
            var njFacility = GetEntityFactory<Facility>().Create(new { OperatingCenter = nj7 });
            var nyFacility = GetEntityFactory<Facility>().Create(new { OperatingCenter = ny1 });
            var njEquipment = GetEntityFactory<Equipment>().Create(new { OperatingCenter = nj7, Identifier = "NJ-1-T-1", Facility = njFacility });
            var nyEquipment = GetEntityFactory<Equipment>().Create(new { OperatingCenter = ny1, Identifier = "NY-1-T-1", Facility = nyFacility });
            var entity = GetEntityFactory<EnvironmentalPermit>().Create(new { State = ny });

            var result = _target.Show(entity.Id) as ViewResult;
            var data = (IEnumerable<SelectListItem>)result.ViewData["Equipment"];

            Assert.AreEqual(nyEquipment.Id.ToString(), data.First().Value);
        }

        [TestMethod]
        public void TestShowAddsEquipmentForOperatingCenters()
        {
            var nj = GetEntityFactory<State>().Create(new { Abbreviation = "NJ", Name = "New Jersey" });
            var ny = GetEntityFactory<State>().Create(new { Abbreviation = "NY", Name = "New York" });
            var nj7 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = nj, OperatingCenterCode = "NJ7" });
            var ny1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = ny, OperatingCenterCode = "NY1" });
            var njFacility = GetEntityFactory<Facility>().Create(new {OperatingCenter = nj7});
            var nyFacility = GetEntityFactory<Facility>().Create(new {OperatingCenter = ny1});
            var njEquipment = GetEntityFactory<Equipment>().Create(new { OperatingCenter = nj7, Identifier = "NJ-1-T-1", Facility = njFacility });
            var nyEquipment = GetEntityFactory<Equipment>().Create(new { OperatingCenter = ny1, Identifier = "NY-1-T-1", Facility = nyFacility });
            var entity = GetEntityFactory<EnvironmentalPermit>().Create(new { State = ny });
            //purposely setting it to an nj opcntr, so we make sure we aren't getting it based on state ny
            entity.OperatingCenters.Add(nj7);
            Session.Save(entity);
            Session.Flush();

            var result = _target.Show(entity.Id) as ViewResult;
            var data = (IEnumerable<SelectListItem>)result.ViewData["Equipment"];

            Assert.AreEqual(njEquipment.Id.ToString(), data.First().Value);
        }

        [TestMethod]
        public void TestShowAddsEquipmentForFacilities()
        {
            var nj = GetEntityFactory<State>().Create(new { Abbreviation = "NJ", Name = "New Jersey" });
            var ny = GetEntityFactory<State>().Create(new { Abbreviation = "NY", Name = "New York" });
            var nj7 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = nj, OperatingCenterCode = "NJ7" });
            var ny1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = ny, OperatingCenterCode = "NY1" });
            var njFacility = GetEntityFactory<Facility>().Create(new { OperatingCenter = nj7 });
            var nyFacility = GetEntityFactory<Facility>().Create(new { OperatingCenter = ny1 });
            var njEquipment = GetEntityFactory<Equipment>().Create(new { OperatingCenter = nj7, Identifier = "NJ-1-T-1", Facility = njFacility });
            var nyEquipment = GetEntityFactory<Equipment>().Create(new { OperatingCenter = ny1, Identifier = "NY-1-T-1", Facility = nyFacility });
            var entity = GetEntityFactory<EnvironmentalPermit>().Create(new { State = ny });
            //purposely setting it to an nj facility, so we make sure we aren't getting it based on state ny
            entity.Facilities.Add(njFacility);
            Session.Save(entity);
            Session.Flush();

            var result = _target.Show(entity.Id) as ViewResult;
            var data = (IEnumerable<SelectListItem>)result.ViewData["Equipment"];

            Assert.AreEqual(njEquipment.Id.ToString(), data.First().Value);
        }

        [TestMethod]
        public void TestShowAddsFacilitiesForState()
        {
            var nj = GetEntityFactory<State>().Create(new { Abbreviation = "NJ", Name = "New Jersey" });
            var ny = GetEntityFactory<State>().Create(new { Abbreviation = "NY", Name = "New York" });
            var nj7 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = nj, OperatingCenterCode = "NJ7" });
            var ny1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = ny, OperatingCenterCode = "NY1" });
            var njFacility = GetEntityFactory<Facility>().Create(new { OperatingCenter = nj7 });
            var nyFacility = GetEntityFactory<Facility>().Create(new { OperatingCenter = ny1 });
            var entity = GetEntityFactory<EnvironmentalPermit>().Create(new { State = nj });
            
            var result = _target.Show(entity.Id) as ViewResult;
            var data = (IEnumerable<SelectListItem>)result.ViewData["Facility"];

            Assert.AreEqual(njFacility.Id.ToString(), data.First().Value);
        }

        [TestMethod]
        public void TestShowAddsFacilitiesForOperatingCenters()
        {
            var nj = GetEntityFactory<State>().Create(new { Abbreviation = "NJ", Name = "New Jersey" });
            var ny = GetEntityFactory<State>().Create(new { Abbreviation = "NY", Name = "New York" });
            var nj7 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = nj, OperatingCenterCode = "NJ7" });
            var ny1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = ny, OperatingCenterCode = "NY1" });
            var njFacility = GetEntityFactory<Facility>().Create(new { OperatingCenter = nj7 });
            var nyFacility = GetEntityFactory<Facility>().Create(new { OperatingCenter = ny1 });
            var entity = GetEntityFactory<EnvironmentalPermit>().Create(new { State = nj });
            //manually adding a ny1 one, because state is nj,
            //so we can see if it's actually basing the results on facility and not state
            entity.OperatingCenters.Add(ny1);
            Session.Save(entity);


            var result = _target.Show(entity.Id) as ViewResult;
            var data = (IEnumerable<SelectListItem>)result.ViewData["Facility"];

            Assert.AreEqual(nyFacility.Id.ToString(), data.First().Value);
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexExportsExcel()
        {
            var facility1 = GetFactory<FacilityFactory>().Create();
            var facility2 = GetFactory<FacilityFactory>().Create();
            var ep1 = _container.GetInstance<TestDataFactory<EnvironmentalPermit>>().Create(new { Facilities = new[] {facility1, facility2} });
            var ep2 = _container.GetInstance<TestDataFactory<EnvironmentalPermit>>().Create();
            var search = new SearchEnvironmentalPermit();

            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(facility1 + ", " + facility2, "Facilities");
                helper.AreEqual(string.Empty, "Facilities", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = _container.GetInstance<TestDataFactory<EnvironmentalPermit>>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditEnvironmentalPermit, EnvironmentalPermit>(eq, new {
                Description = expected
            })) as RedirectToRouteResult;

            Assert.AreEqual(expected, Session.Get<EnvironmentalPermit>(eq.Id).Description);
        }

        #endregion

        #region Children

        #region Equipment

        [TestMethod]
        public void TestAddEquipmentAddsEquipmentToEnvironmentalPermit()
        {
            var environmentalPermit = GetEntityFactory<EnvironmentalPermit>().Create();
            var equipment = GetEntityFactory<Equipment>().Create(new { Identifier = "NJSD-1", Description = "Foo" });

            MyAssert.CausesIncrease(
                () => _target.AddEquipment(environmentalPermit.Id, equipment.Id),
                () => Session.Get<EnvironmentalPermit>(environmentalPermit.Id).Equipment.Count());
        }

        [TestMethod]
        public void TestRemoveEquipmentRemovesEquipmentFromEnvironmentalPermit()
        {
            var environmentalPermit = GetEntityFactory<EnvironmentalPermit>().Create();
            var equipment = GetEntityFactory<Equipment>().Create(new { Identifier = "NJSD-1", Description = "Foo" });
            environmentalPermit.Equipment.Add(equipment);
            Session.Save(environmentalPermit);

            MyAssert.CausesDecrease(
                () => _target.RemoveEquipment(environmentalPermit.Id, equipment.Id),
                () => Session.Get<EnvironmentalPermit>(environmentalPermit.Id).Equipment.Count());
        }

        #endregion

        #region Facilities

        [TestMethod]
        public void TestAddFacilityAddsFacilityToEnvironmentalPermit()
        {
            var environmentalPermit = GetEntityFactory<EnvironmentalPermit>().Create();
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var facility = GetEntityFactory<Facility>().Create(new { OperatingCenter = operatingCenter });

            MyAssert.CausesIncrease(
                () => _target.AddFacility(environmentalPermit.Id, facility.Id),
                () => Session.Get<EnvironmentalPermit>(environmentalPermit.Id).Facilities.Count());
        }

        [TestMethod]
        public void TestRemoveFacilityRemovesFacilityFromEnvironmentalPermit()
        {
            var environmentalPermit = GetEntityFactory<EnvironmentalPermit>().Create();
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var facility = GetEntityFactory<Facility>().Create(new { OperatingCenter = operatingCenter });

            environmentalPermit.Facilities.Add(facility);
            Session.Save(environmentalPermit);

            MyAssert.CausesDecrease(
                () => _target.RemoveFacility(environmentalPermit.Id, facility.Id),
                () => Session.Get<EnvironmentalPermit>(environmentalPermit.Id).Facilities.Count());
        }

        #endregion

        #region Requirements

        [TestMethod]
        public void TestAddEnvironmentalPermitRequirementDoesThatThing()
        {
            var permit = GetEntityFactory<EnvironmentalPermit>().Create();

            MyAssert.CausesIncrease(
                () => _target.AddEnvironmentalPermitRequirement(_viewModelFactory.BuildWithOverrides<CreateEnvironmentalPermitRequirement, EnvironmentalPermit>(permit, new {
                    Requirement = "yo",
                    RequirementType = GetEntityFactory<EnvironmentalPermitRequirementType>().Create().Id,
                    ValueUnit = GetEntityFactory<EnvironmentalPermitRequirementValueUnit>().Create().Id,
                    ValueDefinition = GetEntityFactory<EnvironmentalPermitRequirementValueDefinition>().Create().Id,
                    TrackingFrequency = GetEntityFactory<EnvironmentalPermitRequirementTrackingFrequency>().Create().Id,
                    ReportingFrequency =
                        GetEntityFactory<EnvironmentalPermitRequirementReportingFrequency>().Create().Id,
                    ReportingOwner = GetEntityFactory<Employee>().Create().Id
                })),
                () => _container.GetInstance<RepositoryBase<EnvironmentalPermitRequirement>>().GetAll().Count());
        }

        [TestMethod]
        public void TestRemoveEnvironmentalPermitRequirementDoesThatThing()
        {
            var requirement = GetEntityFactory<EnvironmentalPermitRequirement>().Create();

            MyAssert.CausesDecrease(() => _target.RemoveEnvironmentalPermitRequirement(requirement.Id),
                () => _container.GetInstance<RepositoryBase<EnvironmentalPermitRequirement>>().GetAll().Count());
        }
        
        [TestMethod]
        public void TestRemoveEnvironmentalPermitRequirementDoesNotDoThatThingIfOnlyOneRequirementAndRequired()
        {
            var permit = GetEntityFactory<EnvironmentalPermit>().Create(new {RequiresRequirements = true});
            var requirement = GetEntityFactory<EnvironmentalPermitRequirement>().Create(new { EnvironmentalPermit = permit});
            permit.Requirements.Add(requirement);
            Session.Save(permit);

            ActionResult result;
            MyAssert.DoesNotCauseDecrease(() => result = _target.RemoveEnvironmentalPermitRequirement(requirement.Id),
                () => _container.GetInstance<RepositoryBase<EnvironmentalPermitRequirement>>().GetAll().Count());

            _target.AssertTempDataContainsMessage(EnvironmentalPermitController.MUST_HAVE_REQUIREMENT_ERROR, ControllerBase.ERROR_MESSAGE_KEY);

        }

        #endregion

        #endregion

        #region RemoveEnvironmentalPermitFee
        
        [TestMethod]
        public void TestRemoveEnvironmentalPermitFeeRedirectsToEnvironmentalPermitShowPageWhenSuccessful()
        {
            var fee = GetEntityFactory<EnvironmentalPermitFee>().Create();
            var model = new RemoveEnvironmentalPermitFee(_container) 
            {
                Id = fee.EnvironmentalPermit.Id,
                EnvironmentalPermitFeeId = fee.Id
            };

            var result = _target.RemoveEnvironmentalPermitFee(model);
            MvcAssert.RedirectsToRoute(result,
                new {
                    action = "Show", controller = "EnvironmentalPermit",
                    id = fee.EnvironmentalPermit.Id
                });
        }

        [TestMethod]
        public void TestRemoveEnvironmentalPermitFeeRedirectsToEnvironmentalPermitShowPageWhenThereIsValidationError()
        {
            var fee = GetEntityFactory<EnvironmentalPermitFee>().Create();
            var model = new RemoveEnvironmentalPermitFee(_container)
            {
                Id = fee.EnvironmentalPermit.Id,
                EnvironmentalPermitFeeId = fee.Id
            };
            _target.ModelState.AddModelError("Whoopsy!", "Oops!");
            var result = _target.RemoveEnvironmentalPermitFee(model);
            MvcAssert.RedirectsToRoute(result,
                new
                {
                    action = "Show",
                    controller = "EnvironmentalPermit",
                    id = fee.EnvironmentalPermit.Id
                });
        }

        #endregion
    }
}
