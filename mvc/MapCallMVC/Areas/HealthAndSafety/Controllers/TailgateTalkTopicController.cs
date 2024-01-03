using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.HealthAndSafety.Controllers
{
    public class TailgateTalkTopicController : ControllerBaseWithPersistence<ITailgateTalkTopicRepository, TailgateTalkTopic, User>
    {
        #region Constants

        public const RoleModules TROLL = RoleModules.OperationsHealthAndSafety;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.Search:
                case ControllerAction.New:
                case ControllerAction.Edit:
                    this.AddDropDownData<TailgateTopicCategory>("Category");
                    this.AddDropDownData<TailgateTopicMonth>("Month", x => x.GetAllSorted(y => y.Id), x => x.Id, x => x.Description);
                    this.AddEnumDropDownData<TopicLevels>("TopicLevel");
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(TROLL)]
        public ActionResult Search(SearchTailgateTalkTopic search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(TROLL)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(TROLL)]
        public ActionResult Index(SearchTailgateTalkTopic search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(TROLL, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateTailgateTalkTopic(_container));
        }

        [HttpPost, RequiresRole(TROLL, RoleActions.Add)]
        public ActionResult Create(CreateTailgateTalkTopic model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(TROLL, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditTailgateTalkTopic>(id);
        }

        [HttpPost, RequiresRole(TROLL, RoleActions.Edit)]
        public ActionResult Update(EditTailgateTalkTopic model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(TROLL, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #region ByCategoryId

        [HttpGet]
        public ActionResult ByCategoryId(int categoryId)
        {
            return new CascadingActionResult(Repository.FindByCategoryId(categoryId), "Description", "Id");
        }

        #endregion

        public TailgateTalkTopicController(ControllerBaseWithPersistenceArguments<ITailgateTalkTopicRepository, TailgateTalkTopic, User> args) : base(args) {}
    }
}