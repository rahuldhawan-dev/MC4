using System;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Facilities.Controllers;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Facilities.Controllers
{
    [TestClass]
    public class CommunityRightToKnowControllerTest : MapCallMvcControllerTestBase<CommunityRightToKnowController, CommunityRightToKnow, IRepository<CommunityRightToKnow>>
    {
        #region Init

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateCommunityRightToKnow)vm;
                model.SubmissionDate = DateTime.Now;
                model.ExpirationDate = DateTime.Now;
            };
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditCommunityRightToKnow)vm;
                model.SubmissionDate = DateTime.Now;
                model.ExpirationDate = DateTime.Now;
            };
        }

        #endregion

        #region Roles

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = CommunityRightToKnowController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/Facilities/CommunityRightToKnow/Show/", role);
                a.RequiresRole("~/Facilities/CommunityRightToKnow/Search/", role);
                a.RequiresRole("~/Facilities/CommunityRightToKnow/Index/", role);
                a.RequiresRole("~/Facilities/CommunityRightToKnow/New/", role, RoleActions.Add);
                a.RequiresRole("~/Facilities/CommunityRightToKnow/Create/", role, RoleActions.Add);
                a.RequiresRole("~/Facilities/CommunityRightToKnow/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/Facilities/CommunityRightToKnow/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/Facilities/CommunityRightToKnow/Destroy/", role, RoleActions.Delete);
                a.RequiresRole("~/Facilities/CommunityRightToKnow/Copy/", role, RoleActions.Add);
            });
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<CommunityRightToKnow>().Create();
            var entity1 = GetEntityFactory<CommunityRightToKnow>().Create();
            var search = new SearchCommunityRightToKnow();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<CommunityRightToKnow>().Create();
            var expected = "1234567890";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditCommunityRightToKnow, CommunityRightToKnow>(eq, new
            {
                CommunityRightToKnowFacilityId = expected
            }));

            Assert.AreEqual(expected, Session.Get<CommunityRightToKnow>(eq.Id).CommunityRightToKnowFacilityId);
        }

        #endregion

        #region New/Create

        [TestMethod]
        public void TestCreateRedirectsToShowWhenAnArcFlashStudyIsNotRequiredToBeEntered()
        {
            _currentUser.IsAdmin = true;
            var eq = GetEntityFactory<CommunityRightToKnow>().Create();
            var expected = "1234567890";
            var communityrighttoknow = _viewModelFactory.BuildWithOverrides<CreateCommunityRightToKnow, CommunityRightToKnow>(eq, new
            {
                CommunityRightToKnowFacilityId = expected
            });

            var result = _target.Create(communityrighttoknow) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Show", result.RouteValues["action"]);
            Assert.AreEqual("CommunityRightToKnow", result.RouteValues["controller"]);
            Assert.AreEqual(communityrighttoknow.Id, result.RouteValues["id"]);
        }

        #endregion

        #region Copy

        [TestMethod]
        public void TestCopyCopiesFacilityAndRedirectsToNew()
        {
            // Data setup
            var communityRightToKnow = GetEntityFactory<CommunityRightToKnow>().Create();
            var facility = GetEntityFactory<Facility>().Create();

            communityRightToKnow.Facility = facility;

            Session.SaveOrUpdate(communityRightToKnow);
            Session.Flush();

            // Act
            var result = _target.Copy(communityRightToKnow.Id);
            var model = ((ViewResult)result).Model;
            var actualModel = (CreateCommunityRightToKnow)model;

            // Assert 
            Assert.IsInstanceOfType(model, typeof(CreateCommunityRightToKnow));
            Assert.AreEqual(actualModel.Facility, facility.Id);
        }

        [TestMethod]
        public void TestCopyIsNotFoundReturns404WhenEntityDoesNotExist()
        {
            var result = _target.Copy(0);
            MvcAssert.IsNotFound(result);
            MvcAssert.IsStatusCode(404, result);
        }

        #endregion
    }
}
