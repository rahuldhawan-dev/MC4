using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Helpers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.ClassExtensions;
using MMSINC.Utilities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Production.Models.ViewModels.PreJobSafetyBriefs;
using MapCallMVC.ClassExtensions;

namespace MapCallMVC.Areas.Production.Controllers
{
    public class ProductionPreJobSafetyBriefController
        : ControllerBaseWithPersistence<
            IRepository<ProductionPreJobSafetyBrief>,
            ProductionPreJobSafetyBrief,
            User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.OperationsHealthAndSafety;

        #endregion
        
        #region Constructors

        public ProductionPreJobSafetyBriefController(
            ControllerBaseWithPersistenceArguments<
                IRepository<ProductionPreJobSafetyBrief>,
                ProductionPreJobSafetyBrief,
                User> args)
            : base(args) { }

        #endregion

        #region Private Methods

        private void SetEmployeeLookupData(ProductionWorkOrder pwo, ProductionPreJobSafetyBrief brief)
        {
            // MC-2986: Employees need to be:
            //          1. From the assignments
            //          2. or from the same role/op center for the order
            //          3. and keep existing selected employees for historical reasons(if their role
            //             changes, we don't want them automatically removed).
            var employeesFromAssignments = pwo.EmployeeAssignments.Select(x => x.AssignedTo);
            var employeesWithRole = _container.GetInstance<IRepository<Employee>>()
                                              .GetActiveEmployeesByUserRole(
                                                   pwo.OperatingCenter.Id,
                                                   RoleModules.ProductionWorkManagement)
                                               // "Why are you calling ToList before OrderBy?"
                                               // Because otherwise this happens: 
                                               // "A column has been specified more than once in the order
                                               // by list. Columns in the order by list must be unique."
                                               // Which is interesting because I don't see any other code
                                               // doing an OrderBy for LastName.
                                              .ToList().OrderBy(x => x.LastName);
            var existingBriefEmployees = brief?.Workers ??
                                         Enumerable.Empty<ProductionPreJobSafetyBriefWorker>();
            var employees = existingBriefEmployees.Where(x => x.Employee != null).Select(x => x.Employee)
                                                  .Concat(employeesFromAssignments)
                                                  .Concat(employeesWithRole)
                                                  .Distinct();

            this.AddDropDownData("Employees", employees, x => x.Id, x => x.Description);
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchProductionPreJobSafetyBrief>();
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchProductionPreJobSafetyBrief search)
        {
            return ActionHelper.DoIndex(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            var model = ViewModelFactory.Build<CreateProductionPreJobSafetyBriefNoWorkOrder>();
            this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Add);
            return ActionHelper.DoNew(model);
        }
        
        [ActionBarVisible(false)]
        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult NewForOrder(int productionWorkOrderId)
        {
            var pwo = _container.GetInstance<IRepository<ProductionWorkOrder>>()
                                .Find(productionWorkOrderId);
            if (pwo == null)
            {
                return HttpNotFound(
                    $"The production work order #{productionWorkOrderId} does not exist or you do not " +
                    "have access to view this record.");
            }
            
            var model = ViewModelFactory.Build<CreateProductionPreJobSafetyBriefFromWorkOrder>();
            model.ProductionWorkOrder = productionWorkOrderId;
            SetEmployeeLookupData(pwo, null);
            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateProductionPreJobSafetyBriefNoWorkOrder model)
        {
            return ActionHelper.DoCreate(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult CreateFromOrder(CreateProductionPreJobSafetyBriefFromWorkOrder model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnError = () => {
                    SetEmployeeLookupData(
                        model.GetProductionWorkOrderForDisplay(),
                        Repository.Find(model.Id));
                    return null;
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            var brief = Repository.Find(id);

            return brief?.ProductionWorkOrder == null
                ? ActionHelper.DoEdit(
                    id,
                    new ActionHelperDoEditArgs<
                        ProductionPreJobSafetyBrief,
                        EditProductionPreJobSafetyBriefNoWorkOrder> {
                        GetEntityOverride = () => brief
                    })
                : ActionHelper.DoEdit(
                    id,
                    new ActionHelperDoEditArgs<
                        ProductionPreJobSafetyBrief,
                        EditProductionPreJobSafetyBriefWithWorkOrder> {
                        GetEntityOverride = () => brief
                    },
                    x => { SetEmployeeLookupData(x.ProductionWorkOrder, x); });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditProductionPreJobSafetyBriefNoWorkOrder model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult UpdateWithOrder(EditProductionPreJobSafetyBriefWithWorkOrder model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnError = () => {
                    SetEmployeeLookupData(
                        model.GetProductionWorkOrderForDisplay(),
                        Repository.Find(model.Id));
                    return null;
                }
            });
        }

        #endregion
    }
}
