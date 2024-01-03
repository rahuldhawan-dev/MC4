using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Production.Models.ViewModels;
using MapCallMVC.Models.ViewModels;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Controllers
{
    [DisplayName("Bodies of Water")]
    public class BodyOfWaterController : ControllerBaseWithPersistence<IBodyOfWaterRepository, BodyOfWater, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesAssets;

        #endregion

        #region Constructors

        public BodyOfWaterController(ControllerBaseWithPersistenceArguments<IBodyOfWaterRepository, BodyOfWater, User> args) : base(args) {}

        #endregion

        #region Exposed Methods

        #region ByOperatingCenterId

        [HttpGet]
        public ActionResult ByOperatingCenterId(int id)
        {
            return new CascadingActionResult(Repository.GetByOperatingCenterId(id), "Name", "Id");
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchBodyOfWater search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchBodyOfWater search)
        {
            return this.RespondTo(formatter => {
                formatter.View(() => ActionHelper.DoIndex(search));
            });
        }

        [HttpGet]
        public ActionResult Show(int id)
        {
            return this.RespondTo(x => {
                // ReSharper disable once ConvertToLambdaExpression
                x.View(() => ActionHelper.DoShow(id));
                x.Json(() => {
                    var model = Repository.Find(id);
                    if (model == null)
                        return HttpNotFound(); // TODO: This is returning html for a json result.
                    return new JsonResult { 
                        Data = new {
                            model.CriticalNotes,
                        }, 
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                });
            });
        }

        #endregion

        #region New/Create

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateBodyOfWater model) => ActionHelper.DoCreate(model);

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New() => ActionHelper.DoNew(ViewModelFactory.Build<CreateBodyOfWater>());

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id) => ActionHelper.DoEdit<EditBodyOfWater>(id);

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditBodyOfWater model) => ActionHelper.DoUpdate(model);

        #endregion

        #endregion
    }
}
