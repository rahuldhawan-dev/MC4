using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.SAP.Model.Repositories;
using MapCallMVC.Areas.SAP.Models.ViewModels;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.SAP.Controllers
{
    public class SAPCreatePreventiveWorkOrderController : ControllerBaseWithPersistence<WorkOrder, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionWorkManagement;

        #endregion

        #region Private Members

        private ISAPCreatePreventiveWorkOrderRepository _sapCreatePreventiveWorkOrderRepository;

        #endregion

        #region Properties

        public ISAPCreatePreventiveWorkOrderRepository SAPCreatePreventiveWorkOrderRepository
        {
            get
            {
                return _sapCreatePreventiveWorkOrderRepository ?? (_sapCreatePreventiveWorkOrderRepository =
                           _container.GetInstance<ISAPCreatePreventiveWorkOrderRepository>());
            }
            set { _sapCreatePreventiveWorkOrderRepository = value; }
        }

        #endregion

        public SAPCreatePreventiveWorkOrderController(ControllerBaseWithPersistenceArguments<IRepository<WorkOrder>, WorkOrder, User> args) : base(args) { }

        #region Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(SearchSAPPreventiveWorkOrdercs search)
        {
            var results =
                SAPCreatePreventiveWorkOrderRepository.Search(
                    search.ToSearchPreventiveWorkOrder()).Where(z => z.OrderNumber.TrimStart('0') == search.OrderNumber);

            if (!results.Any())
                return DoHttpNotFound($"No record was found for the order number: {search.OrderNumber}");

            if (results.Any() && results.Count() == 1 && results.First() != null
                && results.First().SAPErrorCode != null &&
                !results.First().SAPErrorCode.StartsWith("Success"))
            {
                DisplayErrorMessage(results.First().SAPErrorCode);
                throw new InvalidOperationException();
            }

            return View("Show", results.First());

        }

        #endregion

    }
}