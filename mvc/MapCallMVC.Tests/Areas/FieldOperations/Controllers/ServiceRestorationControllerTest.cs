using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.Pdf;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class ServiceRestorationControllerTest : MapCallMvcControllerTestBase<ServiceRestorationController, ServiceRestoration>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IImageToPdfConverter>().Mock();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateServiceRestoration)vm;
                model.Service = GetEntityFactory<Service>().Create().Id;
            };
        }

        #endregion

        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var role = ServiceRestorationController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/ServiceRestoration/Search/", role);
                a.RequiresRole("~/FieldOperations/ServiceRestoration/Show/", role);
                a.RequiresRole("~/FieldOperations/ServiceRestoration/Index/", role);
                a.RequiresRole("~/FieldOperations/ServiceRestoration/New/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/ServiceRestoration/Create/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/ServiceRestoration/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/ServiceRestoration/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/ServiceRestoration/Destroy/", role, RoleActions.Delete);
			});
		}

        #endregion

        #region New/Create

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // override due to New parameters.
            _currentUser.IsAdmin = true;
            var service = GetEntityFactory<Service>().Create();
            var result = (ViewResult)_target.New(service.Id);

            MyAssert.IsInstanceOfType<CreateServiceRestoration>(result.Model);
        }

        [TestMethod]
        public void TestNew404sIfServiceNotFound()
        {
            Assert.IsNotNull(_target.New(666) as HttpNotFoundResult);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<ServiceRestoration>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditServiceRestoration, ServiceRestoration>(eq, new {
                PurchaseOrderNumber = expected
            }));

            Assert.AreEqual(expected, Session.Get<ServiceRestoration>(eq.Id).PurchaseOrderNumber);
        }

        #endregion
	}
}
