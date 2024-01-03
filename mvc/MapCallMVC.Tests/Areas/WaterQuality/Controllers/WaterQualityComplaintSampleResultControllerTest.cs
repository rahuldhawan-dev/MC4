using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.WaterQuality.Controllers;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Tests.Areas.WaterQuality.Controllers
{
    [TestClass]
    public class WaterQualityComplaintSampleResultControllerTest : MapCallMvcControllerTestBase<WaterQualityComplaintSampleResultController, WaterQualityComplaintSampleResult>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.UpdateRedirectsToRouteOnSuccessArgs = (vm) => {
                var model = (EditWaterQualityComplaintSampleResult)vm;
                var result = _container.GetInstance<IRepository<WaterQualityComplaintSampleResult>>().Find(model.Id);
                return new { action = "Show", controller = "WaterQualityComplaint", area = "WaterQuality", id = result.Complaint.Id };
            };
        }

        #endregion

        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var role = WaterQualityComplaintSampleResultController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/WaterQuality/WaterQualityComplaintSampleResult/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/WaterQuality/WaterQualityComplaintSampleResult/Update/", role, RoleActions.Edit);
			});
		}

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<WaterQualityComplaintSampleResult>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditWaterQualityComplaintSampleResult, WaterQualityComplaintSampleResult>(eq, new {
                SampleValue = expected
            }));

            Assert.AreEqual(expected, Session.Get<WaterQualityComplaintSampleResult>(eq.Id).SampleValue);
        }

        #endregion
	}
}
