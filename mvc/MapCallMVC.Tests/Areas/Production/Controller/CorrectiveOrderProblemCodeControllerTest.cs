using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Production.Controllers;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Production.Controller
{
    [TestClass]
    public class CorrectiveOrderProblemCodeControllerTest : MapCallMvcControllerTestBase<CorrectiveOrderProblemCodeController, CorrectiveOrderProblemCode>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const string path = "~/Production/CorrectiveOrderProblemCode/";
                a.RequiresSiteAdminUser(path + "Search");
                a.RequiresSiteAdminUser(path + "Show");
                a.RequiresSiteAdminUser(path + "Index");
                a.RequiresSiteAdminUser(path + "New");
                a.RequiresSiteAdminUser(path + "Create");
                a.RequiresSiteAdminUser(path + "Edit");
                a.RequiresSiteAdminUser(path + "Update");
                a.RequiresLoggedInUserOnly(path + "ByEquipmentTypeId");
            });
        }

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<CorrectiveOrderProblemCode>().Create();
            var expectedCode = "Suddenly the animator had a fatal heart attack";
            var expectedDescription = "And the quest for the Holy Grail could continue";

            _target.Update(_viewModelFactory.BuildWithOverrides<CorrectiveOrderProblemCodeViewModel, CorrectiveOrderProblemCode>(eq, new {
                Code = expectedCode,
                Description = expectedDescription
            }));

            Assert.AreEqual(expectedCode, Session.Get<CorrectiveOrderProblemCode>(eq.Id).Code);
            Assert.AreEqual(expectedDescription, Session.Get<CorrectiveOrderProblemCode>(eq.Id).Description);
        }
    }
}