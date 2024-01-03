using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Data.WebApi;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class ServiceInstallationController : ControllerBaseWithPersistence<MMSINC.Data.NHibernate.IRepository<ServiceInstallation>, ServiceInstallation, User>
    { 
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;
        public const string ALREADY_FINALIZED = "Unabled to add/edit a record where the work order has already been finalized.",
            ALREADY_HAS_ORDER = "Unable to add a record when one already exists.";

        #endregion

        #region Constructors

        public ServiceInstallationController(ControllerBaseWithPersistenceArguments<MMSINC.Data.NHibernate.IRepository<ServiceInstallation>, ServiceInstallation, User> args) : base(args) {}

        #endregion

        #region Private Methods

        private WorkOrder GetWorkOrder(int id)
        {
            return _container.GetInstance<IWorkOrderRepository>().Find(id);
        }

        private void UpdateWorkOrderMeterLocation(ServiceInstallation si)
        {
            // Inside and Outside MeterSupplementalLocation maps to Inside and Outside respectively in MeterLocation in WorkOrder
            // Other (SecureAccess, LS etc) MeterSupplementalLocation maps to Unknown in MeterLocation in WorkOrder
            if (si.WorkOrder != null // there is a linked WorkOrder
                && si.WorkOrder.MeterLocation?.Id != si.MeterLocation?.Id) // MeterLocation is different
            {
                int meterLocationId;
                // map ServiceInstallation's MeterSupplementalLocation.Id to WorkOrder.MeterLocation.Id
                switch (si.MeterLocation?.Id)
                {
                    case MeterSupplementalLocation.Indices.INSIDE:
                    case MeterSupplementalLocation.Indices.OUTSIDE:
                        meterLocationId = si.MeterLocation.Id;
                        break;
                    default:
                        meterLocationId = MeterLocation.Indices.UNKNOWN;
                        break;
                }
                
                si.WorkOrder.MeterLocation = new MeterLocation() { Id = meterLocationId };

                Repository.Save(si);
            }
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Search:
                    this.AddOperatingCenterDropDownData();
                    break;
            }
        }

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchServiceInstallation search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchServiceInstallation search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region Create/New
        
        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateServiceInstallation model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs()
            {
                OnSuccess = () =>
                {
                    UpdateWorkOrderMeterLocation(Repository.Find(model.Id));
                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(int? id)
        {
            var model = new CreateServiceInstallation(_container);
            if (id.HasValue)
            {
                var wo = GetWorkOrder(id.Value);
                if (wo != null)
                {
                    model.WorkOrder = wo.Id;
                    if (wo.DateCompleted.HasValue || wo.DateRejected.HasValue)
                    {
                        DisplayNotification(ALREADY_FINALIZED);
                        return DoRedirectionToAction("Search", null);
                    }
                    if (wo.ServiceInstallations != null && wo.ServiceInstallations.Any())
                    {
                        DisplayNotification(ALREADY_HAS_ORDER);
                        return DoRedirectionToAction("Search", null);
                    }
                }
            }
            return ActionHelper.DoNew(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            var entity = Repository.Find(id);
            if (entity != null && entity.WorkOrder != null && ( entity.WorkOrder.DateCompleted.HasValue || entity.WorkOrder.DateRejected.HasValue))
            {
                DisplayNotification(ALREADY_FINALIZED);
                return DoRedirectionToAction("Show", new {id = id });
            }
            return ActionHelper.DoEdit<EditServiceInstallation>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditServiceInstallation model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs()
            {
                OnSuccess = () =>
                {
                    UpdateWorkOrderMeterLocation(Repository.Find(model.Id));
                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        #endregion

        #region Delete

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #endregion
    }
}