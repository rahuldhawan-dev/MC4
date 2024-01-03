using System;
using System.Linq;
using System.Web.Mvc;
using Contractors.Data.DesignPatterns.Mvc;
using Contractors.Data.Models.Repositories;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Results;
using MMSINC.Utilities.Pdf;

namespace Contractors.Controllers
{
    public class ServiceController : ControllerBaseWithValidation<IServiceRepository, Service>
    {
        #region Constants

        public const string SAMPLE_SITE_WARNING = "This Service Record is Linked to a Sample Site. Contact WQ before making any changes.";

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Search:
                    this.AddDropDownData<ServiceSize>("ServiceSize", x => x.GetAllSorted(y => y.SortOrder), x => x.Id, x => x.ServiceSizeDescription);
                    this.AddDropDownData<ServiceCategory>();
                    this.AddDropDownData<ServiceMaterial>();
                    this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>();
                    break;                    
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet]
        public ActionResult Search(SearchService search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet]
        public ActionResult Show(int id, bool nopdf = false)
        {
            return this.RespondTo(x => {
                x.View(() => {
                    Action<Service> onModelFound = (entity) => {
                        if (entity.StatusMessage != string.Empty)
                        {
                            DisplayNotification(entity.StatusMessage);
                        }
                        if (entity.SAPErrorCodeType == ServiceSAPErrorCodeType.InvalidDeviceLocation)
                        {
                            DisplayErrorMessage("SAP Error: " + entity.SAPErrorCode);
                        }
                        if ((entity.Premise != null && 
                             entity.Premise.SampleSite != null && 
                             entity.Premise.SampleSites.Any()) || 
                            Repository.AnyWithInstallationNumberAndOperatingCenterAndSampleSites(entity.Installation, entity.OperatingCenter.Id))
                        {
                            DisplayErrorMessage(SAMPLE_SITE_WARNING);
                        }

                        this.AddDropDownData<IWorkOrderRepository, MapCall.Common.Model.Entities.WorkOrder>("WorkOrder",
                            r => r.GetByTownIdForServices(entity.Town.Id), wo => wo.Id, wo => wo.Id);
                    };

                    return ActionHelper.DoShow(id, onModelFound: onModelFound);
                });
                x.Json(() =>
                {
                    var model = Repository.Find(id);
                    if (model == null)
                        return HttpNotFound();
                    return new JsonResult
                    {
                        Data = new
                        {
                            DateInstalled = $"{model.DateInstalled:d}",
                            ServiceMaterial = model.ServiceMaterial?.Id,
                            ServiceSize = model.ServiceSize?.Id,
                            ServiceMaterialDescription = model.ServiceMaterial?.Description,
                            CustomerSideMaterialDescription = model.CustomerSideMaterial?.Description
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                });
                x.Pdf(() =>
                {
                    var model = Repository.Find(id);
                    if (model == null)
                        return HttpNotFound();
                    return nopdf
                        ? View("Pdf", model)
                        : new PdfResult(
                            _container.GetInstance<IHtmlToPdfConverter>(),
                            "Pdf", model);
                });
            });
        }

        [HttpGet]
        public ActionResult Index(SearchService search)
        {
            return this.RespondTo((formatter) =>
            {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditService>(id);
        }

        [HttpPost]
        public ActionResult Update(EditService model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region ByStreetId

        [HttpGet]
        public ActionResult ByStreetId(int id)
        {
            return new CascadingActionResult(Repository.FindByStreetId(id), "Description", "Id");
        }

        #endregion

        public ServiceController(ControllerBaseWithPersistenceArguments<IServiceRepository, Service, ContractorUser> args) : base(args) { }
    }
}