using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.BPU.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.BPU.Controllers
{
    public class MarkoutViolationController : ControllerBaseWithPersistence<MarkoutViolation,User>
    {
        private readonly IWorkOrderRepository _workOrderRepository;

        #region Consts

        public const RoleModules ROLE = RoleModules.BPUGeneral;

        #endregion

        #region Constructors

        public MarkoutViolationController(
            ControllerBaseWithPersistenceArguments<IRepository<MarkoutViolation>, MarkoutViolation, User> args,
            IWorkOrderRepository workOrderRepository) : base(args)
        {
            _workOrderRepository = workOrderRepository;
        }

        #endregion

        #region Private Methods

        private void PopulateFromWorkOrder(int workOrderId, CreateMarkoutViolation model)
        {
            var wo = _workOrderRepository.Find(workOrderId);
            if (wo != null)
            {
                model.WorkOrder = wo.Id;
                model.Town = wo.Town.Id;
                model.OperatingCenter = wo.OperatingCenter.Id;
                model.Location = wo.StreetAddress;
                model.DateOfViolationNotice = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
                model.MarkoutRequestNumber = wo.Markouts.FirstOrDefault()?.MarkoutNumber;
                if (wo.Coordinate != null)
                {
                    model.Coordinate = GetCoordinate(wo.Coordinate);
                }
            }
        }

        #endregion

        #region Public Methods

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchMarkoutViolation>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchMarkoutViolation search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(int? workOrderId = null)
        {
            var model = new CreateMarkoutViolation(_container);
            if (workOrderId.HasValue)
            {
                 PopulateFromWorkOrder(workOrderId.Value, model);
            }
            return ActionHelper.DoNew(model);
        }

        private int GetCoordinate(Coordinate woCoordinate)
        {
            var coordinate =
                _container.GetInstance<IRepository<Coordinate>>()
                    .Save(new Coordinate
                    {
                        Latitude = woCoordinate.Latitude,
                        Longitude = woCoordinate.Longitude,
                        Icon = woCoordinate.Icon
                    });
            return coordinate.Id;
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateMarkoutViolation model)
        {
            return ActionHelper.DoCreate(model);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditMarkoutViolation>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditMarkoutViolation model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion
    }
}
