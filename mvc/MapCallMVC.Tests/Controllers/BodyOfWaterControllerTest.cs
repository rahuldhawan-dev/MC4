using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Testing.NHibernate;
using StructureMap;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class BodyOfWaterControllerTest : MapCallMvcControllerTestBase<BodyOfWaterController, BodyOfWater>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IBodyOfWaterRepository>().Use<BodyOfWaterRepository>();
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresRole("~/BodyOfWater/Search/", BodyOfWaterController.ROLE);
                a.RequiresRole("~/BodyOfWater/Index/", BodyOfWaterController.ROLE);
                a.RequiresRole("~/BodyOfWater/Create/", BodyOfWaterController.ROLE, RoleActions.Add);
                a.RequiresRole("~/BodyOfWater/New/", BodyOfWaterController.ROLE, RoleActions.Add);
                a.RequiresRole("~/BodyOfWater/Edit/", BodyOfWaterController.ROLE, RoleActions.Edit);
                a.RequiresRole("~/BodyOfWater/Update/", BodyOfWaterController.ROLE, RoleActions.Edit);
                a.RequiresLoggedInUserOnly("~/BodyOfWater/Show/");
                a.RequiresLoggedInUserOnly("~/BodyOfWater/ByOperatingCenterId/");
            });
        }

        [TestMethod]
        public void TestByOperatingCenterIdReturnsBodiesOfWaterForOperatingCenter()
        {
            var op1 = GetFactory<OperatingCenterFactory>().Create();
            var valid = _container.GetInstance<TestDataFactory<BodyOfWater>>().CreateList(2, new {
                OperatingCenter = op1
            });
            var invalid = _container.GetInstance<TestDataFactory<BodyOfWater>>().Create();

            var result = (CascadingActionResult)_target.ByOperatingCenterId(op1.Id);
            var actual = result.GetSelectListItems();

            Assert.AreEqual(valid.Count(), actual.Count() - 1);
            foreach (var selectListItem in actual)
            {
                Assert.AreNotEqual(invalid.Id.ToString(), selectListItem.Value);
            }
        }

        #region Show

        [TestMethod]
        public override void TestShowReturnsShowViewWhenRecordIsFound()
        {
            // override needed due to json-only response.
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var entity = GetEntityFactory<BodyOfWater>().Create(new {
                OperatingCenter = operatingCenter,
                Name = "Deal Lake",
                Description = "A lake",
                CriticalNotes = "Do not swim in this lake"
            });
            InitializeControllerAndRequest("~/BodyOfWater/Show" + entity.Id + ".json");
            
            var result = _target.Show(entity.Id) as JsonResult;
            var resultData = (dynamic)result.Data;

            Assert.IsNotNull(result);
            Assert.AreEqual(entity.CriticalNotes, resultData.CriticalNotes);
            //Assert.AreEqual(entity.Name, resultData.Name);
            //Assert.AreEqual(entity.Description, resultData.Description);
            //Assert.AreEqual(operatingCenter.ToString(), resultData.OperatingCenter.ToString());
        }

        #endregion
    }
}
