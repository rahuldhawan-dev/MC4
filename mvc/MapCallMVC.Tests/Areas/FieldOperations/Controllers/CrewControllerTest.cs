using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Results;
using MMSINC.Testing;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class CrewControllerTest : MapCallMvcControllerTestBase<CrewController, Crew, CrewRepository>
    {
        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.FieldServicesWorkManagement;
                a.RequiresRole("~/Crew/Show/", module, RoleActions.Read);
                a.RequiresRole("~/Crew/Search/", module, RoleActions.Read);
                a.RequiresRole("~/Crew/Index/", module, RoleActions.Read);
                a.RequiresRole("~/Crew/Edit/", module, RoleActions.UserAdministrator);
                a.RequiresRole("~/Crew/Update/", module, RoleActions.UserAdministrator);
                a.RequiresRole("~/Crew/New/", module, RoleActions.UserAdministrator);
                a.RequiresRole("~/Crew/Create/", module, RoleActions.UserAdministrator);
                a.RequiresLoggedInUserOnly("~/Crew/ByOperatingCenterOrAll/");
                a.RequiresLoggedInUserOnly("~/Crew/ByOperatingCenterId/");
            });
        }

        [TestMethod]
        public void TestByOperatingCenterOrAll()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var crew1 = GetEntityFactory<Crew>().Create(new { OperatingCenter = opc1 });
            var crew2 = GetEntityFactory<Crew>().Create(new { OperatingCenter = opc2 });
            var workOrder = GetEntityFactory<WorkOrder>().Create(new { OperatingCenter = opc2 });

            var result = (CascadingActionResult)_target.ByOperatingCenterOrAll(null);
            var data = (IEnumerable<dynamic>)result.Data;

            Assert.AreEqual(2, data.Count());

            result = (CascadingActionResult)_target.ByOperatingCenterOrAll(opc1.Id);
            data = (IEnumerable<dynamic>)result.Data;

            Assert.AreEqual(1, data.Count());
            Assert.AreEqual(crew1.Id, data.Single().Id);
        }

        #region Index/Search

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            _currentUser.IsAdmin = true;
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var workOrderOne = GetEntityFactory<WorkOrder>().Create();
            var workOrderTwo = GetEntityFactory<WorkOrder>().Create();
            var crewAssignmentOne = GetEntityFactory<CrewAssignment>().Create(new { WorkOrder = workOrderOne });
            var crewAssignmentTwo = GetEntityFactory<CrewAssignment>().Create(new { WorkOrder = workOrderTwo });
            var crewAssignments = new List<CrewAssignment>{ crewAssignmentOne, crewAssignmentTwo };

            var entity0 = GetEntityFactory<Crew>().Create(new {
                CrewAssignments = crewAssignments, 
                Active = true,
                OperatingCenter = operatingCenter
            });

            var search = new SearchCrew { OperatingCenter = entity0.OperatingCenter.Id };
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;
            Assert.AreEqual(1, result?.Exporter.Sheets.Count);
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            _currentUser.IsAdmin = true;
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var workOrderOne = GetEntityFactory<WorkOrder>().Create();
            var workOrderTwo = GetEntityFactory<WorkOrder>().Create();
            var crewAssignmentOne = GetEntityFactory<CrewAssignment>().Create(new { WorkOrder = workOrderOne });
            var crewAssignmentTwo = GetEntityFactory<CrewAssignment>().Create(new { WorkOrder = workOrderTwo });
            var crewAssignments = new List<CrewAssignment> { crewAssignmentOne, crewAssignmentTwo };

            var entity0 = GetEntityFactory<Crew>().Create(new {
                CrewAssignments = crewAssignments,
                Active = true,
                OperatingCenter = operatingCenter
            });

            var search = new SearchCrew { OperatingCenter = entity0.OperatingCenter.Id, Active = true, Description = entity0.Description };

            var result = _target.Index(search) as ViewResult;
            var resultModel = ((SearchCrew)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(1, resultModel.Count);
            Assert.AreSame(entity0, resultModel[0]);
        }

        #endregion

        #region Create/New

        [TestMethod]
        public void TestCreatesCrew()
        {
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();

            var model = new CreateCrew(_container) {
                Description = "Demo",
                Availability = (decimal?)6.08,
                OperatingCenter = operatingCenter.Id,
                Active = true
            };

            var result = _target.Create(model);
            MvcAssert.RedirectsToRoute(result, "Crew", "Show", new { id = model.Id });

            var entity = Repository.Find(model.Id);

            Assert.AreEqual(entity.Description, model.Description);
            Assert.AreEqual(entity.Active, model.Active);
            Assert.AreEqual(entity.OperatingCenter.Id, model.OperatingCenter);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public void TestUpdateCrew()
        {
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var crew = GetFactory<CrewFactory>().Create(new { OperatingCenter = operatingCenter });
            var result = _target.Edit(crew.Id);
            MvcAssert.IsViewNamed(result, "Edit");
        }

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var crew = GetFactory<CrewFactory>().Create(new { OperatingCenter = operatingCenter });
            var description = "Demo";
            var availability = 2.50;

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditCrew, Crew>(crew, x => {
                x.Description = description;
                x.Availability = (decimal?)availability;
            }));

            MvcAssert.RedirectsToRoute(result, "Crew", "Show", new { id = crew.Id });
            Assert.AreEqual(crew.Description, Session.Get<Crew>(crew.Id).Description);
            Assert.AreEqual(crew.Availability, Session.Get<Crew>(crew.Id).Availability);
        }

        #endregion

        #endregion
    }
}
