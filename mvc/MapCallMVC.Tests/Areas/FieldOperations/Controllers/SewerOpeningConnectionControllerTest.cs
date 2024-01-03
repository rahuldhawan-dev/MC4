using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class SewerOpeningConnectionControllerTest : MapCallMvcControllerTestBase<SewerOpeningConnectionController, SewerOpeningConnection>
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

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ISewerOpeningRepository>().Use<SewerOpeningRepository>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.UpdateRedirectsToRouteOnSuccessArgs = (vm) => {
                var model = (EditSewerOpeningConnection)vm;
                return new { action = "Show", controller = "SewerOpening", area = "FieldOperations", id = model.OriginalOpeningId };
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = SewerOpeningConnectionController.ROLE;
            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/SewerOpeningConnection/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/SewerOpeningConnection/Update/", role, RoleActions.Edit);
            });
        }

        #region Edit/Update

        [TestMethod]
        public override void TestEditReturnsEditViewWithEditViewModel()
        {
            var town = GetEntityFactory<Town>().Create();
            var upMan = GetEntityFactory<SewerOpening>().Create(new { Town = town });
            var downMan = GetEntityFactory<SewerOpening>().Create(new { Town = town });

            var eq = GetEntityFactory<SewerOpeningConnection>().Create(new { UpstreamOpening = upMan, DownstreamOpening = downMan });

            var result = (ViewResult)_target.Edit(eq.Id, 1);

            MvcAssert.IsViewNamed(result, "Edit");
            Assert.AreEqual(eq.Id, ((EditSewerOpeningConnection)result.Model).Id);
        }

        [TestMethod]
        public override void TestEditReturns404IfMatchingRecordCanNotBeFound()
        {
            // override needed due to multiple parameters
            MvcAssert.IsNotFound(_target.Edit(666, 1));
        }

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<SewerOpeningConnection>().Create();
            var expected = true;

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditSewerOpeningConnection, SewerOpeningConnection>(eq, new {
                IsInlet = expected
            }));

            Assert.AreEqual(expected, Session.Get<SewerOpeningConnection>(eq.Id).IsInlet);
        }

        #endregion
    }
}