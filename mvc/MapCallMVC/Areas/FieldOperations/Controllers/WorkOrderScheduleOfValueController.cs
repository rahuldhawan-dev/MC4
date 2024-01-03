using MMSINC.Controllers;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.ClassExtensions;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Data.NHibernate;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderScheduleOfValues;
using System.Net;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class WorkOrderScheduleOfValueController : ControllerBaseWithPersistence<WorkOrderScheduleOfValue, User>
    {
        #region Constructors

        public WorkOrderScheduleOfValueController(ControllerBaseWithPersistenceArguments<IRepository<WorkOrderScheduleOfValue>, WorkOrderScheduleOfValue, User> args) : base(args) { }

        #endregion

        #region Constants

        public const string SCHEDULE_OF_VALUE_NOT_FOUND = "Schedule Of Value not found.";

        #endregion

        #region Create/New

        [HttpGet]
        public ActionResult New(int workOrderId)
        {
            var model = _viewModelFactory.BuildWithOverrides<CreateWorkOrderScheduleOfValue>(new {
                WorkOrder = workOrderId
            });
            return ActionHelper.DoNew(model, new ActionHelperDoNewArgs { IsPartial = true });
        }

        [HttpPost]
        public ActionResult Create(CreateWorkOrderScheduleOfValue model)
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
            return ActionHelper.DoEdit(id, new ActionHelperDoEditArgs<WorkOrderScheduleOfValue, EditWorkOrderScheduleOfValue> {
                IsPartial = true,
                NotFound = SCHEDULE_OF_VALUE_NOT_FOUND
            });
        }

        [HttpPost]
        public ActionResult Update(EditWorkOrderScheduleOfValue model)
        {
            var args = new ActionHelperDoUpdateArgs {
                OnSuccess = () => PartialView("_Show", Repository.Find(model.Id)),
                OnError = () => PartialView("_Edit", model),
                OnNotFound = () => HttpNotFound(SCHEDULE_OF_VALUE_NOT_FOUND)
            };

            return ActionHelper.DoUpdate(model, args);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id, new ActionHelperDoDestroyArgs {
                NotFound = SCHEDULE_OF_VALUE_NOT_FOUND,
                OnSuccess = () => this.HttpStatusCode(HttpStatusCode.NoContent)
            });
        }

        #endregion
    }
}
