using System.Net;
using System.Web.Mvc;
using Contractors.Data.DesignPatterns.Mvc;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace Contractors.Controllers
{
    public class MainBreakController : ControllerBaseWithValidation<IRepository<MainBreak>, MainBreak>
    {
        #region Constants

        public const string MAINBREAK_NOT_FOUND = "Mainbreak not found.";

        #endregion

        #region Private Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.New:
                case ControllerAction.Edit:
                    this
                       .AddDropDownData<MainCondition>(
                            "MainCondition", m => m.Id, m => m.Description)
                       .AddDropDownData<MainBreakMaterial>(
                            "MainBreakMaterial", m => m.Id, m => m.Description)
                       .AddDropDownData<MainFailureType>(
                            "MainFailureType", m => m.Id, m => m.Description)
                       .AddDropDownData<MainBreakSoilCondition>(
                            "MainBreakSoilCondition", m => m.Id, m => m.Description)
                       .AddDropDownData<MainBreakDisinfectionMethod>(
                            "MainBreakDisinfectionMethod", m => m.Id, m => m.Description)
                       .AddDropDownData<MainBreakFlushMethod>(
                            "MainBreakFlushMethod", m => m.Id, m => m.Description)
                       .AddDropDownData<ServiceSize>(
                            "ServiceSize", m => m.Id, m => m.ServiceSizeDescription);
                    break;
            }
        }

        #endregion

        #region Actions

        #region Create

        [HttpPost]
        public ActionResult Create(CreateMainBreak model)
        {
            var args = new ActionHelperDoCreateArgs();
            args.OnSuccess = () => {
                return PartialView("_Show", Repository.Find(model.Id));
            };
            args.OnError = () => {
                // I have absolutely no idea why this returns null.
                return new EmptyResult();
            };
            return ActionHelper.DoCreate(model, args);
        }

        [HttpGet]
        public ActionResult New(int workOrderId)
        {
            var args = new ActionHelperDoNewArgs {
                ViewName = "_New",
                IsPartial = true
            };
            return ActionHelper.DoNew(_viewModelFactory.BuildWithOverrides<CreateMainBreak>(new { WorkOrder = workOrderId }), args);
        }

        #endregion

        #region Show/Index

        [HttpGet, NoCache]
        public ActionResult Show(int mainBreakId)
        {
            return ActionHelper.DoShow(mainBreakId, new ActionHelperDoShowArgs { IsPartial = true });
        }

        #endregion

        #region Update

        [HttpGet, NoCache]
        public ActionResult Edit(int mainBreakId)
        {
            return ActionHelper.DoEdit(mainBreakId,
                new ActionHelperDoEditArgs<MainBreak, EditMainBreak> {
                    IsPartial = true,
                    NotFound = MAINBREAK_NOT_FOUND,
                });
        }

        [HttpPost]
        public ActionResult Update(EditMainBreak model)
        {
            var args = new ActionHelperDoUpdateArgs();
            args.NotFound = MAINBREAK_NOT_FOUND;
            args.OnSuccess = () => {
                return PartialView("_Show", Repository.Find(model.Id));
            };
            args.OnError = () => {
                return PartialView("_Edit", model);
            };
            args.OnNotFound = () => {
                return HttpNotFound(MAINBREAK_NOT_FOUND);
            };
            return ActionHelper.DoUpdate(model, args);
        }

        #endregion

        #region Delete

        [HttpDelete]
        public ActionResult Destroy(int mainBreakId)
        {
            return ActionHelper.DoDestroy(mainBreakId, new ActionHelperDoDestroyArgs {
                NotFound = MAINBREAK_NOT_FOUND,
                OnSuccess = () => this.HttpStatusCode(HttpStatusCode.NoContent)
            });
        }

        #endregion

        #endregion

        public MainBreakController(ControllerBaseWithPersistenceArguments<IRepository<MainBreak>, MainBreak, ContractorUser> args) : base(args) {}
    }
}
