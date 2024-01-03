using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using SearchSAPTechnicalMasterAccount = MapCallMVC.Areas.Customer.Models.ViewModels.SearchSAPTechnicalMasterAccount;

namespace MapCallMVC.Areas.Customer.Controllers
{
    //Users of Work Management need to be able to view these
    //Not limited by OperatingCenter
    [DisplayName("Technical Master Data")]
    public class SAPTechnicalMasterAccountController : ControllerBaseWithPersistence<WorkOrder, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;

        #endregion

        #region Constructors

        public SAPTechnicalMasterAccountController(ControllerBaseWithPersistenceArguments<IRepository<WorkOrder>, WorkOrder, User> args) : base(args) {}

        #endregion

        #region Properties



        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            switch (action)
            {
                case ControllerAction.Search:
                    this.AddDropDownData<ServiceUtilityType>("InstallationType", x => x.GetAllSorted(), x => x.Type, x => x.Description);
                    break;
            }
        }

        #endregion

        #region Actions

        [HttpGet, RequiresRole(ROLE,RoleActions.Read)]
        public ActionResult Search(SearchSAPTechnicalMasterAccount search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchSAPTechnicalMasterAccount search)
        {
            SetLookupData(ControllerAction.Search);
            var repo = _container.GetInstance<ISAPTechnicalMasterAccountRepository>();
                var sapSearch = new SearchSapTechnicalMaster {
                Equipment = search.Equipment,
                PremiseNumber = search.PremiseNumber,
                InstallationType = search.InstallationType
            };
            var results = (IEnumerable<SAPTechnicalMasterAccount>)repo.Search(sapSearch);

#if DEBUG
            if (MMSINC.MvcApplication.IsInTestMode && MMSINC.MvcApplication.RegressionTestFlags.Contains("fake sap technical master account data"))
            {
                // Replace the SAP data with something phony since we can't guarantee
                // that the SAP data is going to exist during functional tests. But still
                // let the above SAPRepo run so we can catch if that throws an error at all.

                results = new[] {
                    new SAPTechnicalMasterAccount {
                        InstallationTypeSAP = "WT",
                        Installation = "12345",
                        DeviceLocation = "67890"
                    }
                };
            }
#endif
            return this.RespondTo((formatter) => {
                formatter.View(() => {
                    if (results.Any() && results.First().SAPError == "Successful")
                    {
                        return View("Index", results);
                    }

                    DisplayErrorMessage(!results.Any() ? "No results returned from SAP." : results.First().SAPError);
                    return View("Search");
                });
                formatter.Fragment(() => {
                    return (results.First().SAPError == "Successful") ? PartialView("_Index", results) : PartialView("_NoResults");
                });
                formatter.Json(() => {
                    return Json(new {
                        Data = results.Select(x => new {
                            InstallationType = x.InstallationTypeMapCall,
                            Installation = string.IsNullOrWhiteSpace(x.Installation) ? string.Empty : x.Installation,
                            DeviceLocation = string.IsNullOrWhiteSpace(x.DeviceLocation) ? string.Empty : x.DeviceLocation.TrimStart('0'),
                            DeviceSerialNumber = string.IsNullOrWhiteSpace(x.DeviceSerialNumber) ? string.Empty : x.DeviceSerialNumber.TrimStart('0'),
                            MeterSerialNumber = string.IsNullOrWhiteSpace(x.MeterSerialNumber) ? string.Empty : x.MeterSerialNumber.TrimStart('0'),
                            MeterSize = string.IsNullOrWhiteSpace(x.MeterSizeMapCall) ? string.Empty : x.MeterSizeMapCall,
                            BillingClassification = x.BillingClassificationMapCall,
                            Customer = string.IsNullOrWhiteSpace(x.Customer) ? string.Empty : x.Customer,
                            Status = string.IsNullOrWhiteSpace(x.AccountStatusAfterReview) ? string.Empty : x.AccountStatusAfterReview,
                            Phone = string.IsNullOrWhiteSpace(x.Phone) ? string.Empty : x.Phone,
                            Owner = string.IsNullOrWhiteSpace(x.Owner) ? string.Empty : x.Owner,
                            Equipment = string.IsNullOrWhiteSpace(x.SAPEquipmentNumber) ? string.Empty : x.SAPEquipmentNumber.TrimStart('0')
                        })
                    }, JsonRequestBehavior.AllowGet);
                });
            });
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        [OutputCache(Location = OutputCacheLocation.None, Duration = 0, NoStore = true)]
        public PartialViewResult Find(string installation)
        {
            SetLookupData(ControllerAction.Search);
            return PartialView("_Find");
        }

        #endregion
    }
}