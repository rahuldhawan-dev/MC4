using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Utilities;

namespace MapCallMVC.Controllers
{
    public class HelpTopicController : ControllerBaseWithPersistence<IHelpTopicRepository, HelpTopic, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesDataLookups;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Search:
                    this.AddDropDownData<IDocumentTypeRepository, DocumentType>(r => r.GetByTableName("HelpTopics"),
                        t => t.Id, t => t.Name);
                    this.AddDropDownData<HelpCategory>("Category");
                    break;
                case ControllerAction.New:
                case ControllerAction.Edit:
                    this.AddDropDownData<HelpCategory>("Category");
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet]
        public ActionResult Search(SearchHelpTopic search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet]
        public ActionResult Index(SearchHelpTopic search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => {
                        Repository.SearchHelpTopicsWithDocuments(search);
                    }
                }));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateHelpTopic(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateHelpTopic model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditHelpTopic>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditHelpTopic model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        public HelpTopicController(ControllerBaseWithPersistenceArguments<IHelpTopicRepository, HelpTopic, User> args) : base(args) {}
    }
}