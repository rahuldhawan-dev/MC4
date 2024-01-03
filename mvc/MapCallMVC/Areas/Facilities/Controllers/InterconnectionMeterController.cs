using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Facilities.Controllers
{
    public class InterconnectionMeterController : ControllerBaseWithPersistence<Interconnection, User>
    {
        #region Constants

        public const string FRAGMENT_IDENTIFIER = "#Meters";

        #endregion

        #region Exposed Methods

        [NonAction]
        public void SetLookupData(ControllerAction action, CreateInterconnectionMeter model)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.New:
                    if (!model.Interconnection.HasValue)
                    {
                        this.AddDynamicDropDownData<Interconnection, InterconnectionDisplayItem>();
                    }
                    if (!model.Meter.HasValue)
                    {
                        this.AddDropDownData<MeterProfile>( m => m.Id, m => m.ProfileName);
                    }
                    break;
            }
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(RoleModules.ProductionFacilities,RoleActions.Add)]
        public ActionResult New(CreateInterconnectionMeter model)
        {
            ModelState.Clear();
            SetLookupData(ControllerAction.New, model);
            return View(model);
        }

        [HttpPost, RequiresRole(RoleModules.ProductionFacilities, RoleActions.Add)]
        public ActionResult Create(CreateInterconnectionMeter model)
        {
            var ic = Repository.Find(model.Interconnection.Value);
            var meter = _container.GetInstance<IRepository<Meter>>().Find(model.Meter.Value);

            ic.Meters.Add(meter);

            Repository.Save(ic);

            return RedirectToReferrerOr("Show", "Interconnection", new {ic.Id}, FRAGMENT_IDENTIFIER);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(RoleModules.ProductionFacilities, RoleActions.Delete)]
        public ActionResult Destroy(DestroyInterconnectionMeter model)
        {
            var ic = Repository.Find(model.InterconnectionId);
            if (ic == null)
            {
                return HttpNotFound();
            }
            var meter = (from m in ic.Meters where m.Id == model.MeterId select m).FirstOrDefault();

            ic.Meters.Remove(meter);

            Repository.Save(ic);

            return RedirectToReferrerOr("Show", "Interconnection", new {id = model.InterconnectionId},
                FRAGMENT_IDENTIFIER);
        }

        #endregion

        public InterconnectionMeterController(ControllerBaseWithPersistenceArguments<IRepository<Interconnection>, Interconnection, User> args) : base(args) {}
    }
}