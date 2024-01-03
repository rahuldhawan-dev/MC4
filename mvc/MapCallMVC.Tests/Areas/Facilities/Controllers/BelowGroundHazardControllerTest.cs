using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.Facilities.Controllers;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using Moq;

namespace MapCallMVC.Tests.Areas.Facilities.Controllers
{
    [TestClass]
    public class BelowGroundHazardControllerTest : MapCallMvcControllerTestBase<BelowGroundHazardController, BelowGroundHazard, BelowGroundHazardRepository>
    {
        #region Fields

        private User _user;
        private Mock<INotificationService> _noteServ;

        #endregion

        #region Init/Cleanup

        protected override User CreateUser()
        {
            _user = GetFactory<AdminUserFactory>().Create();
            return _user;
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeSearchTester = (tester) => {
                tester.IgnoredPropertyNames.Add("WorkOrderRequired"); // not a mappable property, must test separately
            };
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _noteServ = new Mock<INotificationService>();
            _container.Inject(_noteServ.Object);
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = BelowGroundHazardController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/Facilities/BelowGroundHazard/Search/", role);
                a.RequiresRole("~/Facilities/BelowGroundHazard/Show/", role);
                a.RequiresRole("~/Facilities/BelowGroundHazard/Index/", role);
                a.RequiresRole("~/Facilities/BelowGroundHazard/New/", role, RoleActions.Add);
                a.RequiresRole("~/Facilities/BelowGroundHazard/Create/", role, RoleActions.Add);
                a.RequiresRole("~/Facilities/BelowGroundHazard/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/Facilities/BelowGroundHazard/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/Facilities/BelowGroundHazard/Destroy/", role, RoleActions.UserAdministrator);
            });
        }

        #region New

        [TestMethod]
        public void TestNewLoadsNewFormWithValuesFromOrder()
        {
            var coord = GetEntityFactory<Coordinate>().Create();
            var ts = GetEntityFactory<TownSection>().Create();
            var order = GetEntityFactory<WorkOrder>().Create(new { TownSection = ts, Coordinate = coord });

            var result = (ViewResult)_target.New(order.Id, 1);

            MyAssert.IsInstanceOfType<ActionResult>(result);
            MyAssert.IsInstanceOfType<CreateBelowGroundHazard>(result.Model);
            var foo = (CreateBelowGroundHazard)result.Model;

            Assert.AreEqual(order.Id, foo.WorkOrder);
            Assert.AreEqual(order.OperatingCenter.Id, foo.OperatingCenter);
            Assert.AreEqual(order.Street.Id, foo.Street);
            Assert.AreEqual(order.NearestCrossStreet.Id, foo.CrossStreet);
            Assert.AreEqual(int.Parse(order.StreetNumber), foo.StreetNumber);
            Assert.AreEqual(order.Town.Id, foo.Town);
            Assert.AreEqual(order.TownSection?.Id, foo.TownSection);
            Assert.AreEqual("/Coordinate/Create", foo.CoordinateCreateUrl);
        }

        [TestMethod]
        public void TestNewOnlyIncludesActiveOperatingCentersInLookupData()
        {
            var activeOpc = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = true });
            var inactiveOpc = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = false });

            _target.New();

            var opcData = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];
            Assert.IsTrue(opcData.Any(x => x.Value == activeOpc.Id.ToString()));
            Assert.IsFalse(opcData.Any(x => x.Value == inactiveOpc.Id.ToString()));
        }

        #endregion

        #region Create

        [TestMethod]
        public void TestCreateSendsNotification()
        {
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var model = _viewModelFactory.Build<CreateBelowGroundHazard, BelowGroundHazard>(GetEntityFactory<BelowGroundHazard>().BuildWithConcreteDependencies(new { OperatingCenter = opCntr, Town = town }));
            
            _target.Create(model);

            _noteServ.Verify(x => x.Notify(It.Is<NotifierArgs>(args => args.Data.GetType().GetProperty("RecordUrl").GetValue(args.Data, null).ToString() == "http://localhost/Facilities/BelowGroundHazard/Show/1")));
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<BelowGroundHazard>().Create(new { HazardDescription = "HazardDescription 0" });
            var entity1 = GetEntityFactory<BelowGroundHazard>().Create(new { HazardDescription = "HazardDescription 1" });
            var search = new SearchBelowGroundHazard();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.HazardDescription, "HazardDescription");
                helper.AreEqual(entity1.HazardDescription, "HazardDescription", 1);
            }
        }

        [TestMethod]
        public void TestIndexRespondsToMapWithExpectedModels()
        {
            InitializeControllerAndRequest("~/Facilities/BelowGroundHazard/Index.map");
            var bgh1 = GetEntityFactory<BelowGroundHazard>().Create();
            var bgh2 = GetEntityFactory<BelowGroundHazard>().Create();
            var model = new SearchBelowGroundHazard();

            var result = (MapResult)_target.Index(model);

            var resultModel = result.CoordinateSets.Single().Coordinates.ToArray();
            Assert.AreEqual(2, resultModel.Count());
        }

        #endregion

        #region Show

        [TestMethod]
        public void TestShowRespondsToMap()
        {
            var coordinate1 = GetEntityFactory<Coordinate>().Create(new { Latitude = 40m, Longitude = -70m });
            var coordinate2 = GetEntityFactory<Coordinate>().Create(new { Latitude = 20m, Longitude = -50m });
            var good = GetEntityFactory<BelowGroundHazard>().Create(new { Coordinate = coordinate1 });
            var bad = GetEntityFactory<BelowGroundHazard>().Create(new { Coordinate = coordinate2 });
            InitializeControllerAndRequest("~/Facilities/BelowGroundHazard/Show/" + good.Id + ".map");

            var result = (MapResult)_target.Show(good.Id);

            var resultModel = result.CoordinateSets.Single().Coordinates.ToArray();
            Assert.AreEqual(1, resultModel.Count());
            Assert.IsNotNull(resultModel.First().Coordinate);
            Assert.AreEqual(good.Coordinate.Latitude, resultModel.First().Coordinate.Latitude);
            Assert.AreEqual(good.Coordinate.Longitude, resultModel.First().Coordinate.Longitude);
            Assert.AreNotEqual(bad.Coordinate.Latitude, resultModel.First().Coordinate.Latitude);
            Assert.AreNotEqual(bad.Coordinate.Longitude, resultModel.First().Coordinate.Longitude);
        }

        #endregion
    }
}