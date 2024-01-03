using System.Linq;
using System.Net;
using System.Web.Mvc;
using Contractors.Data.DesignPatterns.Mvc;
using Contractors.Data.Models.Repositories;
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
    public class MaterialUsedController : ControllerBaseWithValidation<MaterialUsed>
    {
        #region Constants

        public const string NO_SUCH_RECORD = "Material Used record not found.";

        public const string VIEWDATA_MATERIALS = "Material";
        public const string VIEWDATA_STOCK_LOCATIONS = "StockLocation";

        #endregion

        #region Actions

        #region Create/New

        [HttpPost]
        public ActionResult Create(CreateMaterialUsed model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => PartialView("_Show", Repository.Find(model.Id)),
                // No clue why this was returning null in the first place, but keeping it that way for now by returning EmptyResult.
                OnError = () => new EmptyResult() 
            });
        }

        [HttpGet, NoCache]
        public ActionResult New(int workOrderId)
        {
            var workOrder = _container.GetInstance<IWorkOrderRepository>().Find(workOrderId);
            PopulateMaterialsAndStockLocations(workOrder);
            return ActionHelper.DoNew(
                _viewModelFactory.BuildWithOverrides<CreateMaterialUsed>(new {WorkOrder = workOrderId}),
                new ActionHelperDoNewArgs {
                    IsPartial = true
                });
        }

        #endregion
        
        #region Edit/Update

        [HttpGet, NoCache]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit(id, new ActionHelperDoEditArgs<MaterialUsed, EditMaterialUsed> {
                IsPartial = true
            }, onModelFound: materialUsed => {
                PopulateMaterialsAndStockLocations(materialUsed.WorkOrder);
            });
        }

        [HttpPost]
        public ActionResult Update(EditMaterialUsed model)
        {
            return ActionHelper.DoUpdate(model,
                new ActionHelperDoUpdateArgs {
                    OnSuccess = () => PartialView("_Show", Repository.Find(model.Id)),
                    OnError = () => PartialView("_Edit", model),
                    OnNotFound = () => HttpNotFound()
                });
        }

        #endregion

        #region Delete

        [HttpDelete]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id, new ActionHelperDoDestroyArgs {
                NotFound = NO_SUCH_RECORD,
                OnSuccess = () => this.HttpStatusCode(HttpStatusCode.NoContent)
            });
        }

        #endregion

        #endregion

        #region Private Methods

        private void PopulateMaterialsAndStockLocations(MapCall.Common.Model.Entities.WorkOrder workOrder)
        {
            this
                .AddDropDownData(VIEWDATA_MATERIALS, workOrder.OperatingCenter.StockedMaterials.Select(sm => sm.Material),
                    m => m.Id, m => m.FullDescription)
                .AddDropDownData(VIEWDATA_STOCK_LOCATIONS, workOrder.OperatingCenter.StockLocations,
                    l => l.Id, l => l.Description);
        }

        #endregion
 
        public MaterialUsedController(ControllerBaseWithPersistenceArguments<IRepository<MaterialUsed>, MaterialUsed, ContractorUser> args) : base(args) {}
    }
}