using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using System.Web.Mvc;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Admin.Models.ViewModels.NotificationConfigurations;
using MapCallMVC.Models.ViewModels;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Admin.Controllers
{
    public class NotificationConfigurationController : 
        ControllerBaseWithPersistence<INotificationConfigurationRepository, NotificationConfiguration, User>
    {
        #region Consts

        public const string OPERATING_CENTER_STATES_VIEWDATA_KEY = "OperatingCenterStates";

        #endregion

        #region Constructors

        public NotificationConfigurationController(ControllerBaseWithPersistenceArguments<INotificationConfigurationRepository, NotificationConfiguration, User> args) : 
            base(args) { }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            this.AddDropDownData<Application>("Application", x => x.GetAllSorted(y => y.Name), x => x.Id, x => x.Name);

            if (action == ControllerAction.New)
            {
                var opcRepo = _container.GetInstance<IRepository<OperatingCenter>>();
                // This is json serialized on the frontend for some interactions. This saves us
                // from having to write a bunch of extra ajax stuff.
                ViewData[OPERATING_CENTER_STATES_VIEWDATA_KEY] = opcRepo.Where(x => !x.IsContractedOperations && x.IsActive).Select(x => new OperatingCenterState {
                    OperatingCenterId = x.Id,
                    StateId = x.State.Id
                }).ToList();
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet]
        public ActionResult Search(SearchNotificationConfigurationViewModel search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet]
        public ActionResult Index(SearchNotificationConfigurationViewModel search)
        {
            // Because of the way we need to Index, everything below was taken from ActionHelper and
            // implemented here since we can't use action helper in this instance.
            var rvd = ModelState.ToRouteValueDictionary();

            if (!ModelState.IsValid)
            {
                DisplayModelStateErrors();
                return DoRedirectionToAction("Search", rvd);
            }

            var searchResult = Repository.SearchNotificationConfigurations(search)
                                         .ToList();

            if (searchResult.Any())
            {
                return this.RespondTo((formatter) => {
                    formatter.View(() => View(searchResult));
                    formatter.Excel(() => this.Excel(searchResult));
                });
            }

            DisplayErrorMessage("No results matched your query.");
            return DoRedirectionToAction("Search", rvd);
        }

        #endregion

        #region New/Create

        [HttpGet]
        public ActionResult New()
        {
            return ActionHelper.DoNewForViewModelSet(ViewModelFactory.Build<CreateNotificationConfigurations>());
        }

        [HttpPost]
        public ActionResult Create(CreateNotificationConfigurations model)
        {
            return ActionHelper.DoCreateForViewModelSet(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    // We can't return them the results of what they just saved. Redirecting to the Index
                    // action would result in a page with a billion records on it due to lack of pagination
                    // that occurs there. See the comments in ActionHelper.DoCreateForViewModelSet for more
                    // reasons why we can't just display only the records they just created with ease.

                    var totalNotifications = model.Items.SelectMany(x => x.NotificationPurposes).Count();
                    DisplaySuccessMessage($"You've created {totalNotifications} notifications!");
                    return RedirectToAction("Search");
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditNotificationConfiguration>(id);
        }

        [HttpPost]
        public ActionResult Update(EditNotificationConfiguration model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete

        [HttpDelete]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion
    }
}