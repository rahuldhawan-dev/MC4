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
using StructureMap;

namespace Contractors.Controllers
{
    public class MarkoutController : ControllerBaseWithValidation<Markout>
    {
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
                    this.AddDropDownData<MarkoutType>("MarkoutType", t => t.Id, t => t.Description);
                    break;
            }
        }

        #endregion

        #region Actions

        #region Create/New

        [HttpPost]
        public ActionResult Create(CreateMarkout model)
        {
            var args = new ActionHelperDoCreateArgs();
            args.OnSuccess = () => {
                return PartialView("_Show", Repository.Find(model.Id));
            };
            args.OnError = () => {
                return PartialView("_New", model);
            };
            return ActionHelper.DoCreate(model, args);
        }

        [HttpGet]
        public ActionResult New(int workOrderId)
        {
            var model = _viewModelFactory.BuildWithOverrides<CreateMarkout>(new {
                WorkOrder = workOrderId,
                // This needs to be DateTime.Today basically.
                DateOfRequest = _container.GetInstance<IDateTimeProvider>().GetCurrentDate().Date
            });
            return ActionHelper.DoNew(model, new ActionHelperDoNewArgs {
                IsPartial = true
            });
        }

        #endregion

        #region Show/Index

        [HttpGet, NoCache]
        public ActionResult Show(int markoutId)
        {
            return ActionHelper.DoShow(markoutId, new ActionHelperDoShowArgs {  IsPartial = true });
        }

        #endregion

        #region Edit/Update

        [HttpGet, NoCache]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditMarkout>(id, new ActionHelperDoEditArgs<Markout, EditMarkout> {
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
        public ActionResult Destroy(int markoutId)
        {
            return ActionHelper.DoDestroy(markoutId, new ActionHelperDoDestroyArgs {
                NotFound = MARKOUT_NOT_FOUND,
                OnSuccess = () => this.HttpStatusCode(HttpStatusCode.NoContent)
            });
        }
        
        #endregion

        #endregion

        public MarkoutController(ControllerBaseWithPersistenceArguments<IRepository<Markout>, Markout, ContractorUser> args) : base(args) {}
    }
}