using MapCall.Common.Model.Entities.Users;
using MMSINC.Controllers;
using MMSINC.Metadata;
using MMSINC.Utilities;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.MaterialsUsed;
using MMSINC.Data.NHibernate;
using System.Net;
using MMSINC.ClassExtensions;
using System.Linq;
using MapCall.Common.Model.Repositories;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class MaterialUsedController : ControllerBaseWithPersistence<MaterialUsed, User>
    {
        #region Constants

        public const string MATERIAL_NOT_FOUND = "Materials not found.";
        public const string VIEWDATA_MATERIALS = "Material";
        public const string VIEWDATA_STOCK_LOCATIONS = "StockLocation";

        #endregion

        #region Constructors

        public MaterialUsedController(ControllerBaseWithPersistenceArguments<IRepository<MaterialUsed>, MaterialUsed, User> args) : base(args) { }

        #endregion

        #region Private Methods

        #region Private Methods

        private void PopulateMaterialsAndStockLocations(WorkOrder workOrder)
        {
            this
               .AddDropDownData(VIEWDATA_MATERIALS, workOrder.OperatingCenter.StockedMaterials.Select(sm => sm.Material),
                    m => m.Id, m => m.FullDescription)
               .AddDropDownData(VIEWDATA_STOCK_LOCATIONS, workOrder.OperatingCenter.StockLocations,
                    l => l.Id, l => l.Description);
        }

        #endregion

        #endregion

        #region Create/New

        [HttpGet]
        public ActionResult New(int workOrderId)
        {
            var workOrder = _container.GetInstance<IWorkOrderRepository>().Find(workOrderId);
            PopulateMaterialsAndStockLocations(workOrder);
            var model = _viewModelFactory.BuildWithOverrides<CreateMaterialUsed>(new {
                WorkOrder = workOrderId,
                OperatingCenter = workOrder.OperatingCenter.Id
            });
            return ActionHelper.DoNew(model, new ActionHelperDoNewArgs {
                IsPartial = true
            });
        }

        [HttpPost]
        public ActionResult Create(CreateMaterialUsed model)
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
            return ActionHelper.DoEdit(id, new ActionHelperDoEditArgs<MaterialUsed, EditMaterialUsed> {
                IsPartial = true,
                NotFound = MATERIAL_NOT_FOUND
            }, onModelFound: materialUsed => {
                PopulateMaterialsAndStockLocations(materialUsed.WorkOrder);
            });
        }

        [HttpPost]
        public ActionResult Update(EditMaterialUsed model)
        {
            var args = new ActionHelperDoUpdateArgs {
                OnSuccess = () => PartialView("_Show", Repository.Find(model.Id)),
                OnError = () => PartialView("_Edit", model),
                OnNotFound = () => HttpNotFound(MATERIAL_NOT_FOUND)
            };

            return ActionHelper.DoUpdate(model, args);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id, new ActionHelperDoDestroyArgs {
                NotFound = MATERIAL_NOT_FOUND,
                OnSuccess = () => this.HttpStatusCode(HttpStatusCode.NoContent)
            });
        }

        #endregion
    }
}
