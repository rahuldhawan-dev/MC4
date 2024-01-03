using System.Linq;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class EchoshoreLeakAlertControllerTest : MapCallMvcControllerTestBase<EchoshoreLeakAlertController, EchoshoreLeakAlert>
    {
        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.FieldServicesWorkManagement;
            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/EchoshoreLeakAlert/Show", role);
                a.RequiresRole("~/FieldOperations/EchoshoreLeakAlert/Search", role);
                a.RequiresRole("~/FieldOperations/EchoshoreLeakAlert/Index", role);
            });
        }

        [TestMethod]
        public void TestShowRespondsToFragment()
        {
            var entity = GetEntityFactory<EchoshoreLeakAlert>().Create();
            InitializeControllerAndRequest("~/FieldOperations/EchoshoreLeakAlert/Show/" + entity.Id + ".frag");

            var result = _target.Show(entity.Id);

            MvcAssert.IsViewNamed(result, "_ShowPopup");
            MvcAssert.IsViewWithModel(result, entity);
        }

        [TestMethod]
        public void TestIndexRespondsToMapWithExpectedModels()
        {
            InitializeControllerAndRequest("~/FieldOperations/EchoshoreLeakAlert/Index.map");
            var good = GetEntityFactory<EchoshoreLeakAlert>().Create();
            var bad = GetEntityFactory<EchoshoreLeakAlert>().Create();
            var model = new SearchEchoshoreLeakAlert {EchoshoreSite = good.EchoshoreSite.Id};

            var result = (MapResult)_target.Index(model);
            var resultModel = result.CoordinateSets.Single().Coordinates.ToArray();

            Assert.AreEqual(1, resultModel.Count());
            Assert.IsTrue(resultModel.Contains(good));
            Assert.IsFalse(resultModel.Contains(bad));
        }
    }
}
