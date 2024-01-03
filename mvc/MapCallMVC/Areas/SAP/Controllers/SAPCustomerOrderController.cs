using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MapCall.SAP.Model.Entities;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using SAP.DataTest.Model.Repositories;

namespace MapCallMVC.Areas.SAP.Controllers
{
    public class SAPCustomerOrderController : ControllerBaseWithPersistence<WorkOrder, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;

        #endregion

        #region Properties

        private ISAPCustomerOrderRepository _sapCustomerOrderRepository;

        public ISAPCustomerOrderRepository SapCustomerOrderRepository
        {
            get
            {
                return _sapCustomerOrderRepository ??
                       (_sapCustomerOrderRepository = _container.GetInstance<ISAPCustomerOrderRepository>());
            }
            set { _sapCustomerOrderRepository = value; }
        }
        
        #endregion

        #region Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchSapCustomerOrder search)
        {
            if (string.IsNullOrWhiteSpace(search.FSR_ID) && string.IsNullOrWhiteSpace(search.WorkOrder))
                search.FSR_ID = AuthenticationService.CurrentUser.Employee?.EmployeeId;

            var results = (IEnumerable<SAPCustomerOrder>)SapCustomerOrderRepository.Search(search);

            if (results.Any() && results.Count() == 1 && results.First() != null
                    && !results.First().SAPErrorCode.StartsWith("Success"))
            {
                DisplayErrorMessage(results.First().SAPErrorCode + " - EmployeeID : " + search.FSR_ID);
            }

            if (!string.IsNullOrWhiteSpace(search.WorkOrder))
            {
                return DoRedirectionToAction("Show", new { id = search.WorkOrder });
            }

            return View("Index", results);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            var search = new SearchSapCustomerOrder {WorkOrder = id.ToString()};
            var results = (IEnumerable<SAPCustomerOrder>)SapCustomerOrderRepository.Search(search);
            return View("Show", results.First());
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchSapCustomerOrder search = null)
        {
            SetLookupData(ControllerAction.Search);
            return View(search ?? new SearchSapCustomerOrder());
        }

        #endregion

        public SAPCustomerOrderController(ControllerBaseWithPersistenceArguments<IRepository<WorkOrder>, WorkOrder, User> args) : base(args) {}
    }
}