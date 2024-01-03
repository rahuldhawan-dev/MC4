using System.Net;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.StreetOpeningPermits;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    /// <summary>
    /// Controller for working with <see cref="StreetOpeningPermit"/> entities in terms of the
    /// <see cref="WorkOrder"/> entities they're associated with.
    /// </summary>
    public class StreetOpeningPermitController : ControllerBaseWithPersistence<StreetOpeningPermit, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;
        public const string NOT_FOUND = "Street Opening Permit not found.";

        #endregion

        #region Constructors

        public StreetOpeningPermitController(
            ControllerBaseWithPersistenceArguments<
                IRepository<StreetOpeningPermit>,
                StreetOpeningPermit,
                User> args)
            : base(args) { }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            if (action == ControllerAction.Search)
            {
                this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE);
            }
        }

        #region Delete

        [HttpDelete, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Destroy(int streetOpeningPermitId)
        {
            return ActionHelper.DoDestroy(streetOpeningPermitId, new ActionHelperDoDestroyArgs {
                NotFound = NOT_FOUND,
                OnSuccess = () => this.HttpStatusCode(HttpStatusCode.NoContent)
            });
        }

        #endregion

        #endregion

        #region Show/Index

        [HttpGet, NoCache, RequiresRole(ROLE)]
        public ActionResult Show(int streetOpeningPermitId)
        {
            return ActionHelper.DoShow(
                streetOpeningPermitId,
                new ActionHelperDoShowArgs { IsPartial = true });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult New(int workOrderId)
        {
            return ActionHelper.DoNew(ViewModelFactory.BuildWithOverrides<CreateStreetOpeningPermit>(new {
                WorkOrder = workOrderId
            }), new ActionHelperDoNewArgs {
                IsPartial = true
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Create(CreateStreetOpeningPermit model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => PartialView("_Show", Repository.Find(model.Id)),
                OnError = () => PartialView("_New", model)
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit), NoCache]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit(
                id,
                new ActionHelperDoEditArgs<StreetOpeningPermit, EditStreetOpeningPermit> {
                    IsPartial = true,
                    NotFound = NOT_FOUND
                });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditStreetOpeningPermit model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => PartialView("_Show", Repository.Find(model.Id)),
                OnError = () => PartialView("_Edit", model),
                OnNotFound = () => HttpNotFound(NOT_FOUND)
            });
        }

        #endregion
    }
}
