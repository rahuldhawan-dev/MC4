using System;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.HydrantPaintings;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Results;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class HydrantPaintingController : ControllerBaseWithPersistence<HydrantPainting, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesAssets;

        #endregion

        #region Constructors

        public HydrantPaintingController(
            ControllerBaseWithPersistenceArguments<
                IRepository<HydrantPainting>, HydrantPainting, User> args) : base(args) { }

        #endregion

        #region Private Methods

        private Func<ActionResult> RedirectToHydrantForShowPageFn(HydrantPaintingViewModel model)
        {
            return () => RedirectToReferrerOr("Show", "Hydrant", new { Id = model.Hydrant }, "#PaintTab");
        }

        #endregion

        #region Create

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Create(CreateHydrantPainting model)
        {
            ActionResult RedirectToHydrantForMap()
                => RedirectToAction("Show", "Hydrant", new {
                    id = model.Hydrant,
                    ext = ResponseFormatter.KnownExtensions.FRAGMENT
                });

            return this.RespondTo(f => {
                f.View(() => ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                    OnSuccess = RedirectToHydrantForShowPageFn(model),
                    OnError = RedirectToHydrantForShowPageFn(model)
                }));
                f.Fragment(() => ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                    OnSuccess = RedirectToHydrantForMap,
                    OnError = RedirectToHydrantForMap
                }));
            });
        }

        #endregion

        #region Update

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditHydrantPainting model)
        {
            return this.RespondTo(f =>
                f.View(() => ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                    OnSuccess = RedirectToHydrantForShowPageFn(model),
                    OnError = RedirectToHydrantForShowPageFn(model)
                })));
        }

        #endregion

        #region Index

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchHydrantPainting search)
        {
            if (search.EditPainting.HasValue)
            {
                search.EditPaintingObj =
                    _container.GetInstance<IRepository<HydrantPainting>>().Find(search.EditPainting.Value);
            }

            return this.RespondTo(f => {
                f.Fragment(() => ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                    ViewName = "_Index",
                    IsPartial = true,
                    RedirectSingleItemToShowView = false,
                    OnNoResults = () => DoView("_Index", search, true)
                }));
            });
        }

        #endregion

        #region Delete

        [HttpDelete, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Destroy(DeleteHydrantPainting painting)
        {
            // using null coalescence because we don't care if it doesn't exist, DoDestroy will handle
            // returning 404 if that's the case
            var hydrant = Repository.Find(painting.Id)?.Hydrant?.Id;
            var model = new EditHydrantPainting(_container) {
                Hydrant = hydrant
            };

            return ActionHelper.DoDestroy(painting.Id, new ActionHelperDoDestroyArgs {
                OnSuccess = RedirectToHydrantForShowPageFn(model),
                OnError = RedirectToHydrantForShowPageFn(model)
            });
        }

        #endregion
    }
}
