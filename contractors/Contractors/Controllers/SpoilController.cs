using System.Net;
using System.Web.Mvc;
using Contractors.Data.DesignPatterns.Mvc;
using Contractors.Data.Models.Repositories;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Exceptions;
using MMSINC.Metadata;

namespace Contractors.Controllers
{
    public class SpoilController : ControllerBaseWithValidation<ISpoilRepository, Spoil>
    {
        #region Constants

        public const string NO_SUCH_SPOIL = "No such spoil.";

        public const string VIEWDATA_SPOIL_STORAGE_LOCATION = "SpoilStorageLocation";

        #endregion

        #region Private methods

        //private HttpNotFoundResult NoSuchSpoil()
        //{
        //    return HttpNotFound(NO_SUCH_SPOIL);
        //}

        private void SetSpoilLocationsForWorkOrder  (MapCall.Common.Model.Entities.WorkOrder wo)
        {
            this
                .AddDropDownData<ISpoilStorageLocationRepository, SpoilStorageLocation>(
                    "SpoilStorageLocation",
                    r => r.GetAllInOperatingCenter(wo.OperatingCenter.Id),
                    l => l.Id, l => l.Name);
        }

        #endregion

        //
        // GET: /Spoil/
        [HttpGet, NoCache]
        public ActionResult Index(int id)
        {
            var model = GetWorkOrder(id);
            if (model == null)
            {
                return NoSuchWorkOrder();
            }
            return PartialView("_Index", model);
        }

        #region New/Create

        [HttpGet, NoCache]
        public ActionResult New(int workOrderId)
        {
            var wo = GetWorkOrder(workOrderId);
            if (wo == null)
            {
                return NoSuchWorkOrder();
            }
            SetSpoilLocationsForWorkOrder(wo);
            var model = _viewModelFactory.BuildWithOverrides<SpoilNew>(new { WorkOrder = workOrderId });
            return ActionHelper.DoNew(model,
                new MMSINC.Utilities.ActionHelperDoNewArgs {
                    IsPartial = true
                });
        }

        [HttpPost]
        public ActionResult Create(SpoilNew model)
        {
            var wo = GetWorkOrder(model.WorkOrder);
            if (wo == null)
            {
                throw new DomainLogicException($"WorkOrderID '{model.WorkOrder}' does not exist, so a spoil could not be created.");
            }

            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => PartialView("_Show", Repository.Find(model.Id)),
                OnError = () => {
                    SetSpoilLocationsForWorkOrder(wo);
                    return PartialView("_New", model);
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, NoCache]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit(id, new MMSINC.Utilities.ActionHelperDoEditArgs<Spoil, EditSpoil> {
                NotFound = NO_SUCH_SPOIL,
                IsPartial = true
            }, onModelFound: (entity) => {
                SetSpoilLocationsForWorkOrder(entity.WorkOrder);
            });
        }

        [HttpPost]
        public ActionResult Update(EditSpoil model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => PartialView("_Show", Repository.Find(model.Id)),
                OnError = () => {
                    SetSpoilLocationsForWorkOrder(Repository.Find(model.Id).WorkOrder);
                    return PartialView("_Edit", model);
                },
                NotFound = NO_SUCH_SPOIL
            });
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id, new MMSINC.Utilities.ActionHelperDoDestroyArgs {
                NotFound = NO_SUCH_SPOIL,
                OnSuccess = () => this.HttpStatusCode(HttpStatusCode.NoContent)
            });
        }

        #endregion

        public SpoilController(ControllerBaseWithPersistenceArguments<ISpoilRepository, Spoil, ContractorUser> args) : base(args) {}
    }
}
