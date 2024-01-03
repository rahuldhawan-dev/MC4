using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    public class PositionGroupCommonNameTrainingRequirementController : ControllerBaseWithPersistence<TrainingRequirement, User>
    {
        #region Constants

        public const RoleModules ROLE_MODULE = RoleModules.OperationsTrainingModules;
        public const string FRAGMENT_IDENTIFIER = "#PositionGroupCommonNamesTab", ALREADY_LINKED = "This position group common name is already linked.";

        #endregion

        #region Exposed Methods

        [NonAction]
        public void SetLookupData(ControllerAction action, CreatePositionGroupCommonNameTrainingRequirement model)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.New:
                    if (!model.TrainingRequirement.HasValue)
                    {
                        this.AddDropDownData<TrainingRequirement>();
                    }
                    if (!model.PositionGroupCommonName.HasValue)
                    {
                        this.AddDropDownData<PositionGroupCommonName>();
                    }
                    break;
            }
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE_MODULE , RoleActions.Add)]
        public ActionResult New(CreatePositionGroupCommonNameTrainingRequirement model)
        {
            ModelState.Clear();
            SetLookupData(ControllerAction.New, model);
            return View(model);
        }

        [HttpPost, RequiresRole(ROLE_MODULE , RoleActions.Add)]
        public ActionResult Create(CreatePositionGroupCommonNameTrainingRequirement model)
        {
            var tm = Repository.Find(model.TrainingRequirement.Value);
            var jt = _container.GetInstance<IRepository<PositionGroupCommonName>>().Find(model.PositionGroupCommonName.Value);

            if (!tm.PositionGroupCommonNames.Contains(jt))
                tm.PositionGroupCommonNames.Add(jt);
            else
            {
                ModelState.AddModelError("Duplicate", ALREADY_LINKED);
                DisplayModelStateErrors();
            }

            Repository.Save(tm);

            return RedirectToReferrerOr("Show", "TrainingRequirement", new {tm.Id}, FRAGMENT_IDENTIFIER);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(ROLE_MODULE , RoleActions.Delete)]
        public ActionResult Destroy(DestroyPositionGroupCommonNameTrainingRequirement model)
        {
            var tm = Repository.Find(model.TrainingRequirementId);
            if (tm == null)
            {
                return HttpNotFound();
            }

            var job =
                (from m in tm.PositionGroupCommonNames where m.Id == model.PositionGroupCommonNameId select m).FirstOrDefault();

            tm.PositionGroupCommonNames.Remove(job);
            
            Repository.Save(tm);

            return RedirectToReferrerOr("Show", "TrainingRequirement", new {id = model.TrainingRequirementId}, FRAGMENT_IDENTIFIER);
        }

        #endregion

        public PositionGroupCommonNameTrainingRequirementController(ControllerBaseWithPersistenceArguments<IRepository<TrainingRequirement>, TrainingRequirement, User> args) : base(args) {}
    }
}
