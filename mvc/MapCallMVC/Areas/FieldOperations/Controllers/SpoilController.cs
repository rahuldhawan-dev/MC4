using MMSINC.Controllers;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.ClassExtensions;
using System.Net;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.Spoils;
using MapCall.Common.Model.Repositories;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class SpoilController : ControllerBaseWithPersistence<Spoil, User>
    {
        #region Constructors

        public SpoilController(ControllerBaseWithPersistenceArguments<IRepository<Spoil>, Spoil, User> args) : base(args) { }

        #endregion

        #region Constants

        public const string SPOIL_NOT_FOUND = "Spoil not found.";

        #endregion

        #region Create/New

        [HttpGet]
        public ActionResult New(int workOrderId)
        {
            var workOrder = _container.GetInstance<IWorkOrderRepository>().Find(workOrderId);
            var model = _viewModelFactory.BuildWithOverrides<CreateSpoil>(new {
                WorkOrder = workOrderId,
                OperatingCenter = workOrder.OperatingCenter.Id
            });
            return ActionHelper.DoNew(model, new ActionHelperDoNewArgs { IsPartial = true });
        }

        [HttpPost]
        public ActionResult Create(CreateSpoil model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => PartialView("_Show", Repository.Find(model.Id)),
                OnError = () => PartialView("_New", model)
            });
        }

        #endregion

        #region Show/Index

        [HttpGet, NoCache]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id, new ActionHelperDoShowArgs { IsPartial = true });
        }

        #endregion

        #region Edit/Update

        [HttpGet, NoCache]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit(id, new ActionHelperDoEditArgs<Spoil, EditSpoil> {
                IsPartial = true,
                NotFound = SPOIL_NOT_FOUND
            });
        }

        [HttpPost]
        public ActionResult Update(EditSpoil model)
        {
            var args = new ActionHelperDoUpdateArgs {
                OnSuccess = () => PartialView("_Show", Repository.Find(model.Id)),
                OnError = () => PartialView("_Edit", model),
                OnNotFound = () => HttpNotFound(SPOIL_NOT_FOUND)
            };

            return ActionHelper.DoUpdate(model, args);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id, new ActionHelperDoDestroyArgs {
                NotFound = SPOIL_NOT_FOUND,
                OnSuccess = () => this.HttpStatusCode(HttpStatusCode.NoContent)
            });
        }

        #endregion
    }
}
