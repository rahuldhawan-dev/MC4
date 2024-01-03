using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using Moq;
using Permits.Data.Client.Repositories;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class BondControllerTest : MapCallMvcControllerTestBase<BondController, Bond>
    {
        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = BondController.ROLE;

            Authorization.Assert(a =>
            {
                a.RequiresRole("~/FieldOperations/Bond/Search/", role);
                a.RequiresRole("~/FieldOperations/Bond/Show/", role);
                a.RequiresRole("~/FieldOperations/Bond/Index/", role);
                a.RequiresRole("~/FieldOperations/Bond/New/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/Bond/Create/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/Bond/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/Bond/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/Bond/Destroy/", role, RoleActions.Delete);
            });
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<Bond>().Create(new { BondNumber = "description 0" });
            var entity1 = GetEntityFactory<Bond>().Create(new { BondNumber = "description 1" });
            var search = new SearchBond();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.BondNumber, "BondNumber");
                helper.AreEqual(entity1.BondNumber, "BondNumber", 1);
            }
        }

        #endregion

        #region New/Create

        [TestMethod]
        public void TestCreateCreatesNewBondOnPermitsIfRoadOpeningPurpose()
        {
            var state = GetEntityFactory<State>().Create(new { Name = "New Jersey", Abbreviation = "NJ" });
            var county = GetEntityFactory<County>().Create(new { Name = "Monmouth", State = state });
            var town = GetEntityFactory<Town>().Create(new { ShortName = "Allenhurst" });
            var bondPurpose = GetFactory<RoadOpeningPermitBondPurposeFactory>().Create();
            var entity = _viewModelFactory.Build<CreateBond, Bond>(GetEntityFactory<Bond>().Build(new {
                BondPurpose = bondPurpose,
                State = state,
                County = county,
                Town = town
            }));
            var permitBond = new Permits.Data.Client.Entities.Bond { Id = 4 };
            var permitsDataClientRepo = new Mock<IPermitsDataClientRepository<Permits.Data.Client.Entities.Bond>>();
            permitsDataClientRepo.Setup(x => x.Save(It.IsAny<Permits.Data.Client.Entities.Bond>())).Returns(permitBond);
            _target.SetHiddenFieldValueByName("_permitsDataClientRepository", permitsDataClientRepo.Object);

            var result = _target.Create(entity);
            var resultModel = Repository.Find(entity.Id);

            Assert.AreEqual(permitBond.Id, resultModel.PermitsBondId);
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

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<Bond>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditBond, Bond>(eq, new
            {
                BondNumber = expected
            }));

            Assert.AreEqual(expected, Session.Get<Bond>(eq.Id).BondNumber);
        }

        [TestMethod]
        public void TestUpdateDeletesBondAndRecreatesIfRoadOpeningPermit()
        {
            var bondPurpose = GetFactory<RoadOpeningPermitBondPurposeFactory>().Create();
            var entity = GetEntityFactory<Bond>().Create(new { PermitsBondId = 666, BondPurpose = bondPurpose });
            var permitsDataClientRepo = new Mock<IPermitsDataClientRepository<Permits.Data.Client.Entities.Bond>>();
            var permitBond = new Permits.Data.Client.Entities.Bond { Id = 4 };
            permitsDataClientRepo.Setup(x => x.Delete(entity.PermitsBondId));
            permitsDataClientRepo.Setup(x => x.Save(It.IsAny<Permits.Data.Client.Entities.Bond>())).Returns(permitBond);
            _target.SetHiddenFieldValueByName("_permitsDataClientRepository", permitsDataClientRepo.Object);

            var result = _target.Update(_viewModelFactory.Build<EditBond, Bond>(entity));

            permitsDataClientRepo.Verify(x => x.Delete(entity.PermitsBondId));
            permitsDataClientRepo.Verify(x => x.Save(It.IsAny<Permits.Data.Client.Entities.Bond>()));
            entity = Repository.Find(entity.Id);
            Assert.AreEqual(permitBond.Id, entity.PermitsBondId);
        }

        [TestMethod]
        public void TestUpdateDeletesBondAndDoesNotRecreatesIfNotRoadOpeningPermit()
        {
            var bondPurpose = GetFactory<RoadOpeningPermitBondPurposeFactory>().Create();
            var otherBondPurpose = GetFactory<PerformanceBondPurposeFactory>().Create();
            var originalBondId = 666;
            var entity = GetEntityFactory<Bond>().Create(new { PermitsBondId = originalBondId, BondPurpose = bondPurpose });
            var permitsDataClientRepo = new Mock<IPermitsDataClientRepository<Permits.Data.Client.Entities.Bond>>();
            var permitBond = new Permits.Data.Client.Entities.Bond { Id = 4 };
            permitsDataClientRepo.Setup(x => x.Delete(entity.PermitsBondId));
            _target.SetHiddenFieldValueByName("_permitsDataClientRepository", permitsDataClientRepo.Object);

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditBond, Bond>(entity, new { BondPurpose = otherBondPurpose.Id }));

            permitsDataClientRepo.Verify(x => x.Delete(originalBondId));
            permitsDataClientRepo.Verify(x => x.Save(It.IsAny<Permits.Data.Client.Entities.Bond>()), Times.Never);
            entity = Repository.Find(entity.Id);
            Assert.AreEqual(0, entity.PermitsBondId);
        }

        #endregion

        #region Delete

        [TestMethod]
        public void TestDestroyDeletesTheBondOnPermitsIfItExists()
        {
            var entity = GetEntityFactory<Bond>().Create(new { PermitsBondId = 666 });
            var permitsDataClientRepo = new Mock<IPermitsDataClientRepository<Permits.Data.Client.Entities.Bond>>();
            permitsDataClientRepo.Setup(x => x.Delete(entity.PermitsBondId));
            _target.SetHiddenFieldValueByName("_permitsDataClientRepository", permitsDataClientRepo.Object);

            var result = _target.Destroy(entity.Id);

            permitsDataClientRepo.Verify(x => x.Delete(entity.PermitsBondId));
        }

        #endregion
    }
}
