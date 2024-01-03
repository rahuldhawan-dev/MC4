using System.Web.Mvc;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using System.Linq;
using MapCall.Common.Configuration;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class RestorationController : ControllerBaseWithPersistence<Restoration, User>
    {
        #region Constants

        private const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;    

        #endregion

        #region Constructors

        public RestorationController(ControllerBaseWithPersistenceArguments<MMSINC.Data.NHibernate.IRepository<Restoration>, Restoration, User> args) : base(args) { }

        #endregion

        #region Private Methods

        private WorkOrder GetWorkOrder(int id)
        {
            return _container.GetInstance<IWorkOrderRepository>().Find(id);
        }

        #endregion

        #region Actions

        #region Search/index

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchRestoration>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchRestoration model)
        {
            return this.RespondTo((formatter) =>
            {
                //search.EnablePaging = true;
                formatter.View(() => ActionHelper.DoIndex(model));
                formatter.Map(() => _container.GetInstance<IMapResultFactory>().Build(ModelState, Repository, model));
                formatter.Excel(() =>
                {
                    // NOTE: The excel export is unit tested! If you add fields, make sure to update the test.
                    // DOUBLE NOTE: This is annoying because they want every field from restorations as well as the linked work order.
                    // TRIPLE NOTE: This is really slow if they export too much due to all the lookups related to workorder.
                    model.EnablePaging = false;
                    var results = Repository.Search(model).Select(x => new {
                        x.Id,
                        x.AcknowledgedByContractor,
                        x.RestorationNotes,
                        x.RestorationType,
                        x.WorkOrder,
                        x.WorkOrder?.DateCompleted,
                        x.AssignedContractor,
                        x.AssignedContractorAt,
                        x.Town,
                        x.WorkOrder?.StreetNumber,
                        x.WorkOrder?.Street,
                        x.WorkOrder?.NearestCrossStreet,
                        x.WorkOrder?.WorkDescription,
                        x.WorkOrder?.CurrentCrew,
                        x.OperatingCenter,
                        x.WBSNumber,
                        x.ResponsePriority,
                        x.EightInchStabilizeBaseByCompanyForces,
                        x.TotalAccruedCost,
                        x.CompletedByOthers,
                        x.CompletedByOthersNotes,
                        x.TrafficControlRequired,
                        x.InitialPurchaseOrderNumber,
                        x.PavingSquareFootage,
                        x.LinearFeetOfCurb,
                        x.EstimatedRestorationFootage,
                        x.DateReopened,
                        x.DateRescheduled,
                        x.DateRecompleted,
                        x.PartialRestorationInvoiceNumber,
                        x.PartialRestorationDate,
                        x.PartialRestorationCompletedBy,
                        x.PartialRestorationTrafficControlCost,
                        x.PartialRestorationTrafficControlInvoiceNumber,
                        x.PartialRestorationActualCost,
                        x.PartialRestorationPurchaseOrderNumber,
                        x.PartialRestorationNotes,
                        x.PartialRestorationPriorityUpchargeType,
                        x.PartialRestorationPriorityUpcharge,
                        x.PartialPavingSquareFootage,
                        x.PartialRestorationDueDate,
                        x.PartialRestorationBreakoutBilling,
                        x.FinalRestorationInvoiceNumber,
                        x.FinalRestorationDate,
                        x.FinalRestorationCompletedBy,
                        x.FinalRestorationTrafficControlCost,
                        x.FinalRestorationTrafficControlInvoiceNumber,
                        x.FinalRestorationActualCost,
                        x.FinalRestorationNotes,
                        x.FinalRestorationPurchaseOrderNumber,
                        x.FinalRestorationPriorityUpchargeType,
                        x.FinalRestorationPriorityUpcharge,
                        x.FinalPavingSquareFootage,
                        x.FinalRestorationDueDate,
                        x.FinalRestorationApprovedAt,
                        x.MeasurementType,
                        x.PartialRestorationStatus,
                        x.HasBeenAssignedToContractor
                    });
                    return this.Excel(results);
                });
            });
        }

        #endregion

        #region Create/New

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateRestoration model)
        {
            return ActionHelper.DoCreate(model);
        }

        [SkipRoleOperatingCenterCheck] // RoleAuthorizer does not know the id param is for a workorder and not a restoration.
        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(int? id)
        {
            var model = new CreateRestoration(_container);

            if (id.HasValue)
            {
                var wo = GetWorkOrder(id.Value);

                if (wo != null)
                {
                    model.WorkOrder = wo.Id;
                    model.OperatingCenter = wo.OperatingCenter.Id;
                    model.Town = wo.Town.Id;
                    model.SetDefaultsFromLastRestoration(wo);
                }
            }

            return ActionHelper.DoNew(model);
        }
        
        #endregion

        #region Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(x => {
                x.View(() => ActionHelper.DoShow(id));
                x.Fragment(() => ActionHelper.DoShow(id, new MMSINC.Utilities.ActionHelperDoShowArgs {
                    ViewName = "_ShowPopup",
                    IsPartial = true
                }));
            });
        }

        #endregion

        #region Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditRestoration>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditRestoration model)
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

        #endregion

    }
}