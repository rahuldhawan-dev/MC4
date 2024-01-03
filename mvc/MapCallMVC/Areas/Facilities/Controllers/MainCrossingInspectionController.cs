using System;
using System.Web.Mvc;
using MapCall.Common.Helpers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Facilities.Controllers
{
    public class MainCrossingInspectionController : ControllerBaseWithPersistence<MainCrossingInspection, User>
    {
        #region Constants

        private const RoleModules ROLE = RoleModules.FieldServicesAssets;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            Action addRatings = () =>
            {
                // These need to be in the order they were inserted.
                var ratings = _container.GetInstance<IMainCrossingInspectionAssessmentRatingRepository>().GetAll();
                this.AddDropDownData("AssessmentRating", ratings, x => x.Id, x => x.Description);
            };

            switch (action)
            {
                case ControllerAction.Edit:
                    addRatings();
                    this.AddDropDownData<User>("InspectedBy");
                    break;

                case ControllerAction.New:
                    addRatings();
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        #endregion

        #region New/Create

        [ActionBarVisible(false), SkipRoleOperatingCenterCheck]
        [HttpGet, NoCache, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(int id)
        {
            var mc = _container.GetInstance<IMainCrossingRepository>().Find(id);
            if (mc == null)
            {
                return
                    DoHttpNotFound("Main Crossing " + id +
                                   " could not be found. An inspection can not be added to a non-existent main crossing.");
            }

            var model = new CreateMainCrossingInspection(_container)
            {
                MainCrossing = mc.Id,
                DisplayMainCrossing = mc,
                InspectedOn = _container.GetInstance<IDateTimeProvider>().GetCurrentDate()
            };

            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateMainCrossingInspection model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs
            {
                OnSuccess = () => RedirectToAction("Show", "MainCrossing", new { area = "Facilities", id = model.MainCrossing.Value })
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, NoCache, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditMainCrossingInspection>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditMainCrossingInspection model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs
            {
                OnSuccess = () =>
                {
                    // ViewModel doesn't include the MainCrossing so it needs to be requeried for.
                    var entity = Repository.Find(model.Id);
                    return RedirectToAction("Show", "MainCrossing", new { area = "Facilities", id = entity.MainCrossing.Id });
                }
            });
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            // TODO: This shouldn't be here and should be one of those wacky routed urls for manies.
            var mainCrossingId = Repository.Find(id)?.MainCrossing.Id;
            return ActionHelper.DoDestroy(id, new ActionHelperDoDestroyArgs
            {
                OnSuccess = () => RedirectToAction("Show", "MainCrossing", new { area = "Facilities", id = mainCrossingId })
            });
        }

        #endregion

        public MainCrossingInspectionController(ControllerBaseWithPersistenceArguments<IRepository<MainCrossingInspection>, MainCrossingInspection, User> args) : base(args) { }
    }
}
