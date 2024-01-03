using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels;
using MMSINC.Controllers;
using System.Web.Mvc;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.WaterQuality.Controllers
{
    public class WaterQualityComplaintSampleResultController : ControllerBaseWithPersistence<WaterQualityComplaintSampleResult, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.WaterQualityGeneral;

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditWaterQualityComplaintSampleResult>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditWaterQualityComplaintSampleResult model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs
            {
                OnSuccess = () =>
                {
                    var result = _container.GetInstance<IRepository<WaterQualityComplaintSampleResult>>().Find(model.Id);
                    return RedirectToAction("Show", "WaterQualityComplaint", new { area = "WaterQuality", id = result.Complaint.Id });
                }
            });
        }
		
        #endregion

		#region Constructors

        public WaterQualityComplaintSampleResultController(ControllerBaseWithPersistenceArguments<IRepository<WaterQualityComplaintSampleResult>, WaterQualityComplaintSampleResult, User> args) : base(args) {}

		#endregion
    }
}