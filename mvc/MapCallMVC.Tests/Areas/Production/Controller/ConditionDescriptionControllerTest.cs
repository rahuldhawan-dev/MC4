using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Production.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;

namespace MapCallMVC.Tests.Areas.Production.Controller
{
    [TestClass]
    public class ConditionDescriptionControllerTest : MapCallMvcControllerTestBase<ConditionDescriptionController, ConditionDescription>
    {
        #region Private Methods

        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.ExpectedEditViewName = "~/Areas/Production/Views/ConditionDescription/Edit.cshtml";
            options.ExpectedNewViewName = "~/Areas/Production/Views/ConditionDescription/New.cshtml";
            options.ExpectedShowViewName = "~/Areas/Production/Views/ConditionDescription/Show.cshtml";
        }

        #endregion

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.ProductionDataAdministration;
            Authorization.Assert(auth => {
                auth.RequiresRole("~/Production/ConditionDescription/GetByConditionTypeId", role);
                auth.RequiresRole("~/Production/ConditionDescription/Show", role);
                auth.RequiresRole("~/Production/ConditionDescription/Index", role);
                auth.RequiresRole("~/Production/ConditionDescription/New", role, RoleActions.Add);
                auth.RequiresRole("~/Production/ConditionDescription/Create", role, RoleActions.Add);
                auth.RequiresRole("~/Production/ConditionDescription/Edit", role, RoleActions.Edit);
                auth.RequiresRole("~/Production/ConditionDescription/Update", role, RoleActions.Edit);
            });
        }

        [TestMethod]
        public void TestGetByConditionTypeIdReturnsConditionDescription()
        {
            var conditionType = GetFactory<ConditionTypeFactory>().Create(new {
                Description = "As Found"
            });
            var conditionType2 = GetFactory<ConditionTypeFactory>().Create(new {
                Description = "As Left"
            });
            var conditionDescription = GetFactory<ConditionDescriptionFactory>().Create(new {
                Description = "Unable to Inspect",
                ConditionType = conditionType
            });
            var conditionDescription2 = GetFactory<ConditionDescriptionFactory>().Create(new {
                Description = "Unable to Inspect",
                ConditionType = conditionType2
            });
            Session.Save(conditionType);
            Session.Save(conditionType2);
            Session.Save(conditionDescription);
            Session.Save(conditionDescription2);
            Session.Flush();

            var results = _target.GetByConditionTypeId(conditionType.Id) as CascadingActionResult;
            var data = results.GetSelectListItems().ToArray();

            Assert.AreEqual(2, data.Count()); // 2 categories are returned, the first item is -- Select --
            Assert.AreEqual(conditionDescription.Description, data[1].Text);
            Assert.AreEqual(conditionDescription.Id.ToString(), data[1].Value);
        }

        #endregion
    }
}
