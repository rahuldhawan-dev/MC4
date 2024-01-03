using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using System.Web.Mvc;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class BappTeamIdeaControllerTest : MapCallMvcControllerTestBase<BappTeamIdeaController, BappTeamIdea>
    {
        #region Fields

        private Mock<INotificationService> _notifier;
        
        #endregion
        
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            _notifier = i.For<INotificationService>().Mock();
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresRole("~/BappTeamIdea/Index/", BappTeamIdeaController.ROLE);
                a.RequiresRole("~/BappTeamIdea/Search/", BappTeamIdeaController.ROLE);
                a.RequiresRole("~/BappTeamIdea/Show/", BappTeamIdeaController.ROLE);
                a.RequiresRole("~/BappTeamIdea/New/", BappTeamIdeaController.ROLE, RoleActions.Add);
                a.RequiresRole("~/BappTeamIdea/Create/", BappTeamIdeaController.ROLE, RoleActions.Add);
                a.RequiresRole("~/BappTeamIdea/Edit/", BappTeamIdeaController.ROLE, RoleActions.Edit);
                a.RequiresRole("~/BappTeamIdea/Update/", BappTeamIdeaController.ROLE, RoleActions.Edit);
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<BappTeamIdea>().Create(new {Description = "description 0"});
            var entity1 = GetEntityFactory<BappTeamIdea>().Create(new {Description = "description 1"});
            var search = new SearchBappTeamIdea();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.Description, "Description");
                helper.AreEqual(entity1.Description, "Description", 1);
            }
        }

        #endregion

        #region New/Create

        [TestMethod]
        public void TestCreateSendsNotificationEmail()
        {
            var ent = GetEntityFactory<BappTeamIdea>().Create();
            var model = _viewModelFactory.Build<CreateBappTeamIdea, BappTeamIdea>( ent);
            model.Id = 0;
            NotifierArgs resultArgs = null;

            ValidationAssert.ModelStateIsValid(model);

            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Create(model);
            var entity = Repository.Find(model.Id);

            Assert.AreSame(entity, resultArgs.Data);
            Assert.AreEqual(entity.BappTeam.OperatingCenter.Id, resultArgs.OperatingCenterId);
            Assert.AreEqual(BappTeamIdeaController.ROLE, resultArgs.Module);
            Assert.AreEqual(BappTeamIdeaController.NOTIFICATION_PURPOSE, resultArgs.Purpose);
            Assert.IsNull(resultArgs.Subject);
        }

        #endregion

        #region Edit/Update
       
        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<BappTeamIdea>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditBappTeamIdea, BappTeamIdea>(eq, new {
                Description = expected
            }));

            Assert.AreEqual(expected, Session.Get<BappTeamIdea>(eq.Id).Description);
        }

        #endregion
    }
}
