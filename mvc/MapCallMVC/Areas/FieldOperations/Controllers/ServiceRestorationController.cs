using System;
using System.Linq;
using MapCall.Common.Helpers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using System.Web.Mvc;
using MapCallMVC.ClassExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class ServiceRestorationController : ControllerBaseWithPersistence<ServiceRestoration, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesAssets;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            switch (action)
            {
                case ControllerAction.Search:
                    this.AddOperatingCenterDropDownData();
                    break;
                case ControllerAction.Edit:
                case ControllerAction.New:
                    this.AddDropDownData<RestorationType>();
                    this.AddDropDownData<RestorationMethod>("PartialRestorationMethod");
                    this.AddDropDownData<RestorationMethod>("FinalRestorationMethod");
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchServiceRestoration search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchServiceRestoration search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search);
                    return this.Excel(results.Select(x => new {
                        x.Service.ServiceNumber,
                        x.Service.StreetAddress,
                        x.Service.OperatingCenter,
                        x.Service.Town,
                        x.Service.ServiceCategory,
                        x.Service.WorkIssuedTo,
                        x.Service.DateInstalled,
                        x.EstimatedRestorationAmount,
                        x.FinalRestorationAmount,
                        x.FinalRestorationMethod,
                        x.ApprovedOn,
                        x.Service.TaskNumber1,
                        x.EstimatedValue,
                        x.RestorationType,
                        x.InitiatedBy,
                        x.PartialRestorationDate,
                        x.PartialRestorationCompletionBy,
                        x.FinalRestorationDate,
                        x.FinalRestorationCompletionBy,
                        x.PurchaseOrderNumber
                    }));
                });
            });
        }

        #endregion

        #region New/Create

        [ActionBarVisible(false)]
        [HttpGet, NoCache, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(int? id)
        {
            var service = (id.HasValue) ? _container.GetInstance<IServiceRepository>().Find(id.Value) : null;
            if (service == null)
                return
                    DoHttpNotFound(
                        String.Format(
                            "Service with id {0} could not be found. A restoration may only be created for an existing service.",
                            id));

            this.AddDropDownData<ServiceRestorationContractor>("PartialRestorationCompletionBy",
                x => x.GetAllSorted().Where(y => y.OperatingCenter == service.OperatingCenter && y.PartialRestoration),
                x => x.Id,
                x => x.Contractor);
            this.AddDropDownData<ServiceRestorationContractor>("FinalRestorationCompletionBy",
                x => x.GetAllSorted().Where(y => y.OperatingCenter == service.OperatingCenter && y.FinalRestoration),
                x => x.Id,
                x => x.Contractor);

            var model = new CreateServiceRestoration(_container) {
                Service = service.Id,
                //DisplayService = service
            };

            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateServiceRestoration model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            var restoration = Repository.Find(id);
            if (restoration == null)
                return new HttpNotFoundResult(String.Format("A restoration with the id {0} could not be found.", id));

            if (restoration.Service?.OperatingCenter != null)
            {
                this.AddDropDownData<ServiceRestorationContractor>("PartialRestorationCompletionBy",
                    x => x.GetAllSorted().Where(y =>
                        y.OperatingCenter == restoration.Service.OperatingCenter && y.PartialRestoration),
                    x => x.Id,
                    x => x.Contractor);
                this.AddDropDownData<ServiceRestorationContractor>("FinalRestorationCompletionBy",
                    x => x.GetAllSorted().Where(y =>
                        y.OperatingCenter == restoration.Service.OperatingCenter && y.FinalRestoration),
                    x => x.Id,
                    x => x.Contractor);
            }

            return ActionHelper.DoEdit<EditServiceRestoration>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditServiceRestoration model)
        {
            return ActionHelper.DoUpdate(model);
        }
		
        #endregion

        #region Delete

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

		#region Constructors

        public ServiceRestorationController(ControllerBaseWithPersistenceArguments<IRepository<ServiceRestoration>, ServiceRestoration, User> args) : base(args) {}

		#endregion
    }
}