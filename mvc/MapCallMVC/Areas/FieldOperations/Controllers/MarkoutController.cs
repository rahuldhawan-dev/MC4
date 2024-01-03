using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using System.Net;
using System.Web.Mvc;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class MarkoutController : ControllerBaseWithPersistence<Markout, User>
    {
        #region Constructors

        public MarkoutController(ControllerBaseWithPersistenceArguments<IRepository<Markout>, Markout, User> args) : base(args) { }

        #endregion

        #region Consts

        public const string MARKOUT_NOT_FOUND = "Markout not found.";

        #endregion

        #region Private Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            switch (action)
            {
                case ControllerAction.New:
                case ControllerAction.Edit:
                    this.AddDropDownData<MarkoutType>(t => t.Id, t => t.Description);
                    break;
            }
        }

        #endregion

        #region Create/New

        [HttpPost]
        public ActionResult Create(CreateMarkout model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => PartialView("_Show", Repository.Find(model.Id)),
                OnError = () => PartialView("_New", model)
            });
        }

        [HttpGet]
        public ActionResult New(int workOrderId)
        {
            var model = _viewModelFactory.BuildWithOverrides<CreateMarkout>(new {
                WorkOrder = workOrderId,
                DateOfRequest = _container.GetInstance<IDateTimeProvider>().GetCurrentDate().Date
            });
            return ActionHelper.DoNew(model, new ActionHelperDoNewArgs {
                IsPartial = true
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
            return ActionHelper.DoEdit(id, new ActionHelperDoEditArgs<Markout, EditMarkout> {
                IsPartial = true,
                NotFound = MARKOUT_NOT_FOUND
            });
        }

        [HttpPost]
        public ActionResult Update(EditMarkout model)
        {
            var args = new ActionHelperDoUpdateArgs {
                OnSuccess = () => PartialView("_Show", Repository.Find(model.Id)),
                OnError = () => PartialView("_Edit", model),
                OnNotFound = () => HttpNotFound(MARKOUT_NOT_FOUND)
            };

            return ActionHelper.DoUpdate(model, args);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id, new ActionHelperDoDestroyArgs {
                NotFound = MARKOUT_NOT_FOUND,
                OnSuccess = () => this.HttpStatusCode(HttpStatusCode.NoContent)
            });
        }

        #endregion
    }
}
