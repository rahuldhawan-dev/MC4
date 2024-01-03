using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallApi.Models;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallApi.Controllers
{
    public class SAPLookupController : ControllerBaseWithPersistence<IRepository<ServiceInstallationPosition>, ServiceInstallationPosition, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;

        #endregion

        #region Private Members

        private readonly IRepository<ServiceInstallationPosition> _serviceInstallationPositionRepo;
        private readonly IRepository<SAPWorkOrderPurpose> _workOrderPurposeRepo;
        private readonly IRepository<ServiceInstallationWorkType> _serviceInstallationWorkTypeRepo;
        private readonly IRepository<ServiceInstallationFirstActivity> _serviceInstallationFirstActivityRepo;
        private readonly IRepository<ServiceInstallationSecondActivity> _serviceInstallationSecondActivityRepo;

        #endregion

        #region Private Methods

        private IEnumerable<SAPLookup> GetData()
        {
            // TODO: OH GOD WHY. There must be a better way to do this. Maybe through a SQL View or STI or something less terrible.
            // Shame Alex into fixing it, this is his mess. - arr
            var positions = _serviceInstallationPositionRepo.GetAll().Select(x => new SAPLookup {
                Type = "ServiceInstallationPosition",
                Code = x.SAPCode,
                CodeGroup = x.CodeGroup,
                Description = x.Description,
                Catalog = "B"
            }).ToList();
            var purposes = _workOrderPurposeRepo.GetAll().Select(x => new SAPLookup {
                Type = "WorkOrderPurpose",
                Code = x.Code,
                CodeGroup = x.CodeGroup,
                Description = x.Description,
                Catalog = "D"
            }).ToList();
            var serviceInstallationWorkTypes = _serviceInstallationWorkTypeRepo.GetAll().Select(x =>
                new SAPLookup {
                    Type = "ServiceInstallationWorkType",
                    Code = x.SAPCode,
                    CodeGroup = x.CodeGroup,
                    Description = x.Description,
                    Catalog = "B"
                }).ToList();
            var serviceInstallationFirstActivities = _serviceInstallationFirstActivityRepo.GetAll().Select(x =>
                new SAPLookup {
                    Type = "ServiceInstallationFirstActivity",
                    Code = x.SAPCode,
                    CodeGroup = x.CodeGroup,
                    Description = x.Description,
                    Catalog = "A"
                }).ToList();
            var serviceInstallationSecondActivities = _serviceInstallationSecondActivityRepo.GetAll().Select(x =>
                new SAPLookup {
                    Type = "ServiceInstallationSecondActivity",
                    Code = x.SAPCode,
                    CodeGroup = x.CodeGroup,
                    Description = x.Description,
                    Catalog = "A"
                }).ToList();

            return positions.Union(purposes)
                .Union(serviceInstallationWorkTypes)
                .Union(serviceInstallationFirstActivities)
                .Union(serviceInstallationSecondActivities);
        }

        #endregion

        #region Constructors

        public SAPLookupController(ControllerBaseWithPersistenceArguments<IRepository<ServiceInstallationPosition>, ServiceInstallationPosition, User> args, 
            IRepository<ServiceInstallationPosition> serviceInstallationRepo,
            IRepository<SAPWorkOrderPurpose> workOrderPurposeRepo,
            IRepository<ServiceInstallationWorkType> serviceInstallationWorkTypeRepo,
            IRepository<ServiceInstallationFirstActivity> serviceInstallationFirstActivityRepo,
            IRepository<ServiceInstallationSecondActivity> serviceInstallationSecondActivityRepo) : base(args)
        {
            _serviceInstallationPositionRepo = serviceInstallationRepo;
            _workOrderPurposeRepo = workOrderPurposeRepo;
            _serviceInstallationWorkTypeRepo = serviceInstallationWorkTypeRepo;
            _serviceInstallationFirstActivityRepo = serviceInstallationFirstActivityRepo;
            _serviceInstallationSecondActivityRepo = serviceInstallationSecondActivityRepo;
        }
        
        #endregion

        // GET
        [RequiresRole(ROLE)]
        public ActionResult Index(SearchSAPLookup search)
        {
            var data = GetData();
            if (!string.IsNullOrWhiteSpace(search.Type))
                data = data.Where(x => x.Type == search.Type);
            if (!string.IsNullOrWhiteSpace(search.Code))
                data = data.Where(x => x.Code == search.Code);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public class SAPLookup
        {
            public string Type { get; set; }
            public string CodeGroup { get; set; }
            public string Code { get; set; }
            public string Description { get; set; }
            public string Catalog { get; set; }
        }
    }
}
