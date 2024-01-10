using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class WorkDescriptionController
        : ControllerBaseWithPersistence<IWorkDescriptionRepository, WorkDescription, User>
    {
        #region Consts

        private const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;

        #endregion

        #region Constructors

        public WorkDescriptionController(
            ControllerBaseWithPersistenceArguments<IWorkDescriptionRepository, WorkDescription, User> args)
            : base(args) { }

        #endregion

        #region Public Methods

        #region Show/Index/Search

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchWorkDescription>();
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Index(SearchWorkDescription search)
        {
            return ActionHelper.DoIndex(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(x => {
                x.View(() => ActionHelper.DoShow(id));
                x.Json(() => {
                    var model = Repository.Find(id);
                    if (model == null)
                        return HttpNotFound();
                    return new JsonResult {
                        Data = new { model.DigitalAsBuiltRequired },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                });
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditWorkDescription>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Update(EditWorkDescription model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region ByAssetTypeId

        [HttpGet]
        public ActionResult ActiveByAssetTypeId(int assetTypeId)
        {
            return new CascadingActionResult(Repository.GetActiveByAssetTypeId(assetTypeId), "Description", "Id");
        }
        
        // Get all Work Descriptions by asset ids which are already used for work orders 
        // irrespective of active/inactive status Ex: Lead Work Descriptions
        [HttpGet]
        public ActionResult UsedByAssetTypeIds(int[] assetTypeIds)
        {
            return new CascadingActionResult(
                Repository.GetUsedByAssetTypeIds(assetTypeIds),
                "Description",
                "Id");
        }

        [HttpGet]
        public ActionResult ActiveByAssetTypeIdAndIsRevisit(int assetTypeId, bool isRevisit)
        {
            return new CascadingActionResult(
                Repository.GetActiveByAssetTypeId(assetTypeId).Where(x => x.Revisit == isRevisit),
                "Description",
                "Id");
        }

        #endregion

        #region ActiveByAssetTypeIdForCreate

        [HttpGet]
        public ActionResult ActiveByAssetTypeIdForCreate(int assetTypeId, bool isRevisit)
        {
            return new CascadingActionResult(
                Repository.GetActiveByAssetTypeIdForCreate(assetTypeId, isRevisit),
                "Description",
                "Id");
        }

        #endregion

        #endregion
    }
}