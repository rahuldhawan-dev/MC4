using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Production.Models.ViewModels;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Production.Controllers
{
    public class ProductionWorkDescriptionController : ControllerBaseWithPersistence<IRepository<ProductionWorkDescription>, ProductionWorkDescription, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionWorkManagement;

        #endregion

        #region Constructors

        public ProductionWorkDescriptionController(ControllerBaseWithPersistenceArguments<IRepository<ProductionWorkDescription>, ProductionWorkDescription, User> args) : base(args) { }
        public ProductionWorkDescriptionController() : this(null) { }

        #endregion

        #region Search/Index/Show

        [RequiresRole(ROLE)]
        [HttpGet]
        public ActionResult Search(SearchProductionWorkDescription search)
        {
            return ActionHelper.DoSearch(search);
        }

        [RequiresRole(ROLE)]
        [HttpGet]
        public ActionResult Show(int id)
        {
            return this.RespondTo((f) => {
                f.View(() => ActionHelper.DoShow(id));
                f.Json(() => {
                    var entity = Repository.Find(id);
                    if (entity != null)
                    {
                        return Json(new {
                            BreakdownIndicator = entity.BreakdownIndicator ? "True" : "False",
                            OrderType = entity.OrderType.SAPCode
                        }, JsonRequestBehavior.AllowGet);
                    }

                    DoHttpNotFound(string.Format($"Unable to locate production work description #: {id}"));
                    return null;
                });
            });
        }

        [RequiresRole(ROLE)]
        [HttpGet]
        public ActionResult Index(SearchProductionWorkDescription search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #region ByEquipmentType

        [HttpGet]
        public ActionResult ByEquipmentTypeId(int equipmentTypeId)
        {
            return new CascadingActionResult(Repository.Where(x => x.EquipmentType != null && x.EquipmentType.Id == equipmentTypeId), "Description", "Id") {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult ByEquipmentTypeIdForCreate(int equipmentTypeId)
        {
            return new CascadingActionResult(Repository.Where(x => x.EquipmentType != null && x.EquipmentType.Id == equipmentTypeId && !OrderType.COMPLIANCE_ORDER_TYPES.Contains(x.OrderType.Id)).OrderBy(x => x.Description), "Description", "Id") {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        
        public ActionResult ByEquipmentTypeIdsForCreate(int[] equipmentTypeIds)
        {
            //Because of all the many-to-many connections, we need to call
            // DistinctBy() on the results to avoid duplicates. NHibernate doesn't
            // support DistinctBy() on this kind of query when it's ran through the
            // SelectDynamic stuff for whatever reason and throws an exception,
            // so this also needs a ToList(). -Greg 9/28/2020
            var query = (from cat in Repository.Where(x => true)
                         where equipmentTypeIds.Contains(cat.EquipmentType.Id)
                         where cat.EquipmentType != null
                         where !OrderType.COMPLIANCE_ORDER_TYPES.Contains(cat.OrderType.Id)
                         select new ProductionWorkDescription{Description = cat.Description, Id = cat.Id}).DistinctBy(x => x.Description).ToList();
            
            return new CascadingActionResult(query, "Description", "Id");
        }

        #endregion

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateProductionWorkDescription(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateProductionWorkDescription model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditProductionWorkDescription>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditProductionWorkDescription model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion
    }
}
