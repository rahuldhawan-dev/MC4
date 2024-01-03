using MapCall.Common.Model.Entities.Users;
using MMSINC.Controllers;
using MMSINC.Metadata;
using MMSINC.Utilities;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.MainBreaks;
using MMSINC.ClassExtensions;
using System.Net;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class MainBreakController : ControllerBaseWithPersistence<MainBreak, User>
    {
        #region Constructors

        public MainBreakController(ControllerBaseWithPersistenceArguments<IRepository<MainBreak>, MainBreak, User> args) : base(args) { }

        #endregion

        #region Constants

        public const string MAIN_BREAK_NOT_FOUND = "Main Break not found.";

        #endregion

        #region Create/New

        [HttpGet]
        public ActionResult New(int workOrderId)
        {
            var model = _viewModelFactory.BuildWithOverrides<CreateMainBreak>(new {
                WorkOrder = workOrderId
            });
            return ActionHelper.DoNew(model, new ActionHelperDoNewArgs { IsPartial = true });
        }

        [HttpPost]
        public ActionResult Create(CreateMainBreak model)
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
            return ActionHelper.DoEdit(id, new ActionHelperDoEditArgs<MainBreak, EditMainBreak> {
                IsPartial = true,
                NotFound = MAIN_BREAK_NOT_FOUND
            });
        }

        [HttpPost]
        public ActionResult Update(EditMainBreak model)
        {
            var args = new ActionHelperDoUpdateArgs {
                OnSuccess = () => PartialView("_Show", Repository.Find(model.Id)),
                OnError = () => PartialView("_Edit", model),
                OnNotFound = () => HttpNotFound(MAIN_BREAK_NOT_FOUND)
            };

            return ActionHelper.DoUpdate(model, args);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id, new ActionHelperDoDestroyArgs
            {
                NotFound = MAIN_BREAK_NOT_FOUND,
                OnSuccess = () => this.HttpStatusCode(HttpStatusCode.NoContent)
            });
        }

        #endregion
    }
}
