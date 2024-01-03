using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.SewerMainCleanings;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using Moq;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class SewerMainCleaningControllerTest : MapCallMvcControllerTestBase<SewerMainCleaningController, SewerMainCleaning>
    {
        #region Fields

        private User _user;

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
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateSewerMainCleaning)vm;
                model.Town = GetEntityFactory<Town>().Create().Id;
                model.Date = DateTime.Now;
                model.FootageOfMainInspected = 1;
                model.InspectedDate = DateTime.Now;
                model.Opening1 = GetEntityFactory<SewerOpening>().Create().Id;
                model.Opening2 = GetEntityFactory<SewerOpening>().Create().Id;
                model.InspectionType = GetEntityFactory<SewerMainInspectionType>().Create().Id;
            };
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditSewerMainCleaning)vm;
                model.Town = GetEntityFactory<Town>().Create().Id;
                model.Date = DateTime.Now;
                model.FootageOfMainInspected = 1;
                model.InspectedDate = DateTime.Now;
                model.Opening1 = GetEntityFactory<SewerOpening>().Create().Id;
                model.Opening2 = GetEntityFactory<SewerOpening>().Create().Id;
                model.InspectionType = GetEntityFactory<SewerMainInspectionType>().Create().Id;
            };
        }
		
        #endregion

        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var role = SewerMainCleaningController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/SewerMainCleaning/Search/", role);
                a.RequiresRole("~/FieldOperations/SewerMainCleaning/Show/", role);
                a.RequiresRole("~/FieldOperations/SewerMainCleaning/Index/", role);
                a.RequiresRole("~/FieldOperations/SewerMainCleaning/New/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/SewerMainCleaning/NewFromSewerOpening/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/SewerMainCleaning/Create/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/SewerMainCleaning/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/SewerMainCleaning/Update/", role, RoleActions.Edit);
                a.RequiresSiteAdminUser("~/FieldOperations/SewerMainCleaning/Destroy/");
			});
		}				

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<SewerMainCleaning>().Create(new {TableNotes = "description 0"});
            var entity1 = GetEntityFactory<SewerMainCleaning>().Create(new {TableNotes = "description 1"});
            var search = new SearchSewerMainCleaning();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.TableNotes, "Notes");
                helper.AreEqual(entity1.TableNotes, "Notes", 1);
            }
        }

        #endregion

        #region New/Create

        [TestMethod]
        public void TestNewFromSewerOpeningCallsActionHelperAndGetsResult()
        {
            var sewerOpening = GetEntityFactory<SewerOpening>().Create();

            var result = (ViewResult)_target.NewFromSewerOpening(sewerOpening.Id);

            MyAssert.IsInstanceOfType<ActionResult>(result);
            MyAssert.IsInstanceOfType<CreateSewerMainCleaning>(result.Model);
        }

        [TestMethod]
        public void TestNewFromSewerOpeningShows404WhenSewerOpeningIsNull()
        {
            var result = (HttpNotFoundResult)_target.NewFromSewerOpening(123);
            Assert.IsNotNull(result);
        }

        #region SAP

        [TestMethod]
        public void TestCreateCallsSAPRepositoryCreateAndRecordsErrorCodeUponFailure()
        {
            _user.IsAdmin = true;
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var opening = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opCntr, Town = town });
            var cleaning = GetEntityFactory<SewerMainCleaning>().BuildWithConcreteDependencies(new { Opening1 = opening });
            var model = _viewModelFactory.Build<CreateSewerMainCleaning, SewerMainCleaning>( cleaning);

            _target.Create(model);

            var hydrantInspection = Repository.Find(model.Id);
            Assert.IsTrue(hydrantInspection.SAPErrorCode.StartsWith(SewerMainCleaningController.SAP_UPDATE_FAILURE));
        }

        [TestMethod]
        public void TestCreateCallsSAPRepositoryCreateAndRecordsSAPEquipmentIdAndNoErrorMessage()
        {
            _user.IsAdmin = true;
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var opening = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opCntr, Town = town });
            var cleaning = GetEntityFactory<SewerMainCleaning>().BuildWithConcreteDependencies(new { Opening1 = opening });
            var model = _viewModelFactory.Build<CreateSewerMainCleaning, SewerMainCleaning>( cleaning);
            var repository = new Mock<ISAPInspectionRepository>();
            var sapInspection = new SAPInspection { SAPErrorCode = string.Empty, SAPNotificationNumber = "2345" };
            repository.Setup(x => x.Save(It.IsAny<SAPInspection>())).Returns(sapInspection);
            _container.Inject(repository.Object);

            _target.Create(model);
            var actual = Repository.Find(model.Id);

            Assert.AreEqual(string.Empty, actual.SAPErrorCode);
            Assert.AreEqual(sapInspection.SAPNotificationNumber, actual.SAPNotificationNumber);
        }

        #endregion

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<SewerMainCleaning>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditSewerMainCleaning, SewerMainCleaning>(eq, new {
                TableNotes = expected
            }));

            Assert.AreEqual(expected, Session.Get<SewerMainCleaning>(eq.Id).TableNotes);
        }
        
        #region SAP

        [TestMethod]
        public void TestUpdateEffectivelyCreatesInSAPWhenSAPNotificationNumberIsNullish()
        {
            _user.IsAdmin = true;
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var opening = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opCntr, Town = town });
            var cleaning = GetEntityFactory<SewerMainCleaning>().Create(new { Opening1 = opening });
            var model = _viewModelFactory.Build<EditSewerMainCleaning, SewerMainCleaning>( cleaning);
            var repository = new Mock<ISAPInspectionRepository>();
            var sapInspection = new SAPInspection { SAPErrorCode = string.Empty, SAPNotificationNumber = "2345" };
            repository.Setup(x => x.Save(It.IsAny<SAPInspection>())).Returns(sapInspection);
            _container.Inject(repository.Object);

            _target.Update(model);
            var actual = Repository.Find(model.Id);

            Assert.AreEqual(string.Empty, actual.SAPErrorCode);
            Assert.AreEqual(sapInspection.SAPNotificationNumber, actual.SAPNotificationNumber);
        }

        [TestMethod]
        public void TestUpdateDoesNotAttemptToCreatesInSAPWhenSAPNotificationNumberIsNotNull()
        {
            _user.IsAdmin = true;
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var opening = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opCntr, Town = town });
            var cleaning = GetEntityFactory<SewerMainCleaning>().Create(new { Opening1 = opening, SAPNotificationNumber = "1234" });
            var model = _viewModelFactory.Build<EditSewerMainCleaning, SewerMainCleaning>( cleaning);
            var repository = new Mock<ISAPInspectionRepository>();
            _container.Inject(repository.Object);

            _target.Update(model);

            repository.Verify(x => x.Save(It.IsAny<SAPInspection>()), Times.Never);
        }

        #endregion

        #endregion

        #region Lookups

        [TestMethod]
        public void TestDropDownLookupForNewSetsActiveOC()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = true });
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = false });

            _authenticationService.Setup(x => x.CurrentUserIsAdmin).Returns(true);

            _target.SetLookupData(ControllerAction.New);

            var authenticationServiceOC = _authenticationService.Object.CurrentUser.DefaultOperatingCenter.Id.ToString();

            var opcDropDownData = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];

            // Need to filter out _user default OC since it created a active OC when creating the _user object

            var DropDownData = opcDropDownData.Where(x => x.Value != authenticationServiceOC);

            Assert.AreEqual(1, DropDownData.Count());
            Assert.AreEqual(opc1.Id.ToString(), DropDownData.First().Value);
            Assert.AreNotEqual(opc2.Id.ToString(), DropDownData.First().Value);

        }

        #endregion
    }
}
