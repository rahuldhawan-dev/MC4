using Contractors.Data.DesignPatterns.Mvc;
using Contractors.Data.Models.Repositories;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MMSINC.Controllers;
using MMSINC.Metadata;
using MMSINC.Utilities;
using System.Web.Mvc;

namespace Contractors.Controllers
{
    public class ServiceInstallationController : ControllerBaseWithValidation<IServiceInstallationRepository, ServiceInstallation>
    {
        #region Private Method

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

                si.WorkOrder.MeterLocation = new MeterLocation { Id = meterLocationId };

                Repository.Save(si);
            }
        }

        #endregion

        #region Constants

        public const string 
            NO_SUCH_SERVICE_INSTALLATION = "No such service installation", 
            ALREADY_FINALIZED = "Unabled to add/edit a record where the work order has already been finalized.";

        #endregion

        #region Constructors

        public ServiceInstallationController(ControllerBaseWithPersistenceArguments<IServiceInstallationRepository, ServiceInstallation, ContractorUser> args) : base(args) { }

        #endregion

        #region Actions

        #region Show/Index

        [HttpGet, NoCache]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchServiceInstallation>();
        }

        [HttpGet]
        public ActionResult Index(SearchServiceInstallation model)
        {
            return ActionHelper.DoIndex(model);
        }

        #endregion

        #region Create/New

        [HttpPost]
        public ActionResult Create(CreateServiceInstallation model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    UpdateWorkOrderMeterLocation(Repository.Find(model.Id));
                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        [HttpGet]
        public ActionResult New(int id)
        {
            var model = _viewModelFactory.Build<CreateServiceInstallation>();
            var wo = GetWorkOrder(id);

            if (wo == null)
            {
                return DoHttpNotFound($"Work order for '#{id}' could not be found.");
            }

            model.WorkOrder = wo.Id;

            return ActionHelper.DoNew(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, NoCache]
        public ActionResult Edit(int id)
        {
            var entity = Repository.Find(id);
            if (entity != null && entity.WorkOrder != null && (entity.WorkOrder.DateCompleted.HasValue || entity.WorkOrder.DateRejected.HasValue))
            {
                DisplayNotification(ALREADY_FINALIZED);
                return DoRedirectionToAction("Show", new { id = id });
            }

            return ActionHelper.DoEdit<EditServiceInstallation>(id, new ActionHelperDoEditArgs<ServiceInstallation, EditServiceInstallation> {
                NotFound = NO_SUCH_SERVICE_INSTALLATION
            });
        }

        [HttpPost]
        public ActionResult Update(EditServiceInstallation model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    UpdateWorkOrderMeterLocation(Repository.Find(model.Id));
                    return null;
                },
                NotFound = NO_SUCH_SERVICE_INSTALLATION
            });
        }

        #endregion        

        #endregion
    }
}