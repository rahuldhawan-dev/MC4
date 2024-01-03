using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.WaterQuality.Controllers;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Testing.MSTest.TestExtensions;
using OperatingCenter = MapCall.Common.Model.Entities.OperatingCenter;

namespace MapCallMVC.Tests.Areas.WaterQuality.Controllers
{
    [TestClass]
    public class BacterialWaterSampleAnalystControllerTest : MapCallMvcControllerTestBase<BacterialWaterSampleAnalystController, BacterialWaterSampleAnalyst>
    {
        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.WaterQualityGeneral;

            Authorization.Assert(a => {
                a.RequiresRole("~/WaterQuality/BacterialWaterSampleAnalyst/Show/", role, RoleActions.UserAdministrator);
                a.RequiresRole("~/WaterQuality/BacterialWaterSampleAnalyst/Index/", role, RoleActions.UserAdministrator);
                a.RequiresRole("~/WaterQuality/BacterialWaterSampleAnalyst/New/", role, RoleActions.UserAdministrator);
                a.RequiresRole("~/WaterQuality/BacterialWaterSampleAnalyst/Create/", role, RoleActions.UserAdministrator);
                a.RequiresRole("~/WaterQuality/BacterialWaterSampleAnalyst/Edit/", role, RoleActions.UserAdministrator);
                a.RequiresRole("~/WaterQuality/BacterialWaterSampleAnalyst/Update/", role, RoleActions.UserAdministrator);
                a.RequiresRole("~/WaterQuality/BacterialWaterSampleAnalyst/Destroy/", role, RoleActions.UserAdministrator);
                a.RequiresRole("~/WaterQuality/BacterialWaterSampleAnalyst/AddOperatingCenter", role, RoleActions.UserAdministrator);
                a.RequiresRole("~/WaterQuality/BacterialWaterSampleAnalyst/RemoveOperatingCenter", role, RoleActions.UserAdministrator);
                a.RequiresLoggedInUserOnly("~/WaterQuality/BacterialWaterSampleAnalyst/GetActiveByOperatingCenter/");
                a.RequiresLoggedInUserOnly("~/WaterQuality/BacterialWaterSampleAnalyst/GetByOperatingCenter/");
            });
		}

        #endregion

        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateBacterialWaterSampleAnalyst)vm;
                model.OperatingCenter = GetEntityFactory<OperatingCenter>().Create().Id;
            };
        }

        #endregion

        #region Cascades

        [TestMethod]
        public void TestGetActiveByOperatingCenterReturnsActiveAnalystsByOperatingCenter()
        {
            var thisOpCntr = GetFactory<UniqueOperatingCenterFactory>().Create();
            var thatOpCntr = GetFactory<UniqueOperatingCenterFactory>().Create();
            var expected = GetEntityFactory<BacterialWaterSampleAnalyst>().CreateList(2, new {
                IsActive = true
            });
            var inactive = GetEntityFactory<BacterialWaterSampleAnalyst>().CreateList(2, new {
                IsActive = false
            });
            var wrongOpCenter = GetEntityFactory<BacterialWaterSampleAnalyst>().CreateList(2, new {
                IsActive = true
            });
            foreach (var e in expected.Union(inactive))
            {
                e.OperatingCenters.Add(thisOpCntr);
                Session.Save(e);
            }
            foreach (var e in wrongOpCenter)
            {
                e.OperatingCenters.Add(thatOpCntr);
                Session.Save(e);
            }

            Session.Flush();
            Session.Clear();

            var result = (CascadingActionResult)_target.GetActiveByOperatingCenter(thisOpCntr.Id);
            var actual = result.GetSelectListItems();
            var values = actual.Select(i => i.Value).ToArray();

            Assert.AreEqual(expected.Count + 1, values.Length);
            MyAssert.Contains(values, expected.First().Id.ToString());
            MyAssert.Contains(values, expected.Last().Id.ToString());
        }

        [TestMethod]
        public void TestGetByOperatingCenterReturnsAllAnalystsByOperatingCenter()
        {
            var thisOpCntr = GetFactory<UniqueOperatingCenterFactory>().Create();
            var thatOpCntr = GetFactory<UniqueOperatingCenterFactory>().Create();
            var expected = GetEntityFactory<BacterialWaterSampleAnalyst>().CreateList(2, new {
                IsActive = true
            });
            var alsoExpected = GetEntityFactory<BacterialWaterSampleAnalyst>().CreateList(2, new {
                IsActive = false
            });
            var wrongOpCenter = GetEntityFactory<BacterialWaterSampleAnalyst>().CreateList(2, new {
                IsActive = true
            });
            foreach (var e in expected.Union(alsoExpected))
            {
                e.OperatingCenters.Add(thisOpCntr);
                Session.Save(e);
            }
            foreach (var e in wrongOpCenter)
            {
                e.OperatingCenters.Add(thatOpCntr);
                Session.Save(e);
            }

            Session.Flush();
            Session.Clear();

            var result = (CascadingActionResult)_target.GetByOperatingCenter(thisOpCntr.Id);
            var actual = result.GetSelectListItems();
            var values = actual.Select(i => i.Value).ToArray();

            Assert.AreEqual(expected.Count + alsoExpected.Count + 1, values.Length);
            MyAssert.Contains(values, expected.First().Id.ToString());
            MyAssert.Contains(values, expected.Last().Id.ToString());
            MyAssert.Contains(values, alsoExpected.First().Id.ToString());
            MyAssert.Contains(values, alsoExpected.Last().Id.ToString());
        }

        #endregion
    }
}
