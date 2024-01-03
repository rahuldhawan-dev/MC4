using System.ComponentModel;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Models.ViewModels;
using MMSINC.Authentication;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Controllers
{
    [DisplayName("Equipment Type Measurement Points")]
    public class MeasurementPointEquipmentTypeController : ControllerBaseWithPersistence<MeasurementPointEquipmentType, User>
    {
        #region Create
        
        [HttpPost, RequiresAdmin]
        public ActionResult Create(MeasurementPointEquipmentTypeViewModel model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => RedirectToReferrerOr("Show", "EquipmentType", new { Id = model.EquipmentType }, "#MeasurementPointsTab"),
                OnError = () => RedirectToAction("Show", "EquipmentType", new { Id = model.EquipmentType })
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresAdmin]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditMeasurementPointEquipmentType>(id);
        }

        [HttpPost, RequiresAdmin]
        public ActionResult Update(EditMeasurementPointEquipmentType model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => RedirectToAction("Show", "EquipmentType", new { Id = model.EquipmentType })
            });
        }
        
        #endregion

        #region Constructors

        public MeasurementPointEquipmentTypeController(ControllerBaseWithPersistenceArguments<IRepository<MeasurementPointEquipmentType>, MeasurementPointEquipmentType, User> args) : base(args) {}

        #endregion
    }
}
