using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using System.Web.Mvc;
using MMSINC.Testing.ClassExtensions;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class CutoffSawQuestionnaireControllerTest : MapCallMvcControllerTestBase<CutoffSawQuestionnaireController, CutoffSawQuestionnaire>
    {
        #region Private Members

        private User _user;
        protected Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion
        
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            e.For<ICutoffSawQuestionRepository>().Use<CutoffSawQuestionRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = new User { UserName = "mcadmin" };
            _authenticationService.Setup(x => x.CurrentUser).Returns(_user);
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateCutoffSawQuestionnaire)vm;
                model.Agree = true;
            };
        }

        #endregion

        #region Roles

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.FieldServicesWorkManagement;
                a.RequiresRole("~/CutoffSawQuestionnaire/Search", module, RoleActions.Read);
                a.RequiresRole("~/CutoffSawQuestionnaire/Show", module, RoleActions.Read);
                a.RequiresRole("~/CutoffSawQuestionnaire/Index", module, RoleActions.Read);
                a.RequiresRole("~/CutoffSawQuestionnaire/New", module, RoleActions.Read);
                a.RequiresRole("~/CutoffSawQuestionnaire/Create", module, RoleActions.Read);

                a.RequiresSiteAdminUser("~/CutoffSawQuestionnaire/Destroy/");
            });
        }

        #endregion

        #region Show

        [TestMethod]
        public void TestShowRespondsToPdf()
        {
            var entity = GetFactory<CutoffSawQuestionnaireFactory>().Create();
            InitializeControllerAndRequest("~/CutoffSawQuestionnaire/Show/" + entity.Id + ".pdf");
            var result = _target.Show(entity.Id);
            Assert.IsInstanceOfType(result, typeof(PdfResult));
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetFactory<CutoffSawQuestionnaireFactory>().Create(new { Comments = "description 0" });
            var entity1 = GetFactory<CutoffSawQuestionnaireFactory>().Create(new { Comments = "description 1" });
            var search = new SearchCutoffSawQuestionnaire();
            _target.ControllerContext = new ControllerContext();
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

        #region New/Create

        [TestMethod]
        public void TestNewCallsActionHelperAndGetsResultWithActiveQuestions()
        {
            var questions = GetFactory<CutoffSawQuestionFactory>().CreateList(3);
            var invalid = GetFactory<CutoffSawQuestionFactory>().Create(new {IsActive = false});
            var result = (ViewResult)_target.New();

            MyAssert.IsInstanceOfType<ActionResult>(result);
            MyAssert.IsInstanceOfType<CreateCutoffSawQuestionnaire>(result.Model);
            Assert.AreEqual(3, ((CreateCutoffSawQuestionnaire)result.Model).CutoffSawQuestions.Count);
        }

        [TestMethod]
        public void TestNewSetOperatedOnToCurrentDate()
        {
            var questions = GetFactory<CutoffSawQuestionFactory>().CreateList(3);
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            var result = (ViewResult)_target.New();

            MyAssert.AreClose(now, ((CreateCutoffSawQuestionnaire)result.Model).OperatedOn.Value);
        }

        #endregion
    }
}
