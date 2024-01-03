using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using MMSINC.Controllers;

namespace MapCallMVC.Controllers
{
    // Redo this so that it's a copy of what NoteController does for the most part.

    // NO ROLE, shared by multiple things
    public class VideoController : ControllerBaseWithPersistence<IVideoRepository, Video, User>
    {
        #region Constants

        private const RoleModules ROLE = RoleModules.OperationsTrainingModules;

        #endregion

        #region Constructors

        public VideoController(ControllerBaseWithPersistenceArguments<IVideoRepository, Video, User> args) : base(args) { }
        
        #endregion

        #region Public Methods

        [HttpGet]
        public ActionResult Index()
        {
            var records = Repository.GetAllSproutVideos();
            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Show(int id)
        {
            var record = Repository.FindSproutVideo(id);

            if (record == null)
            {
                // TODO: Don't return a 404 here unless it can be json.
                return HttpNotFound();
            }

            return Json(record, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAllTags()
        {
            return Json(Repository.GetAllTags(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(CreateVideoLink model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs
            {
                OnSuccess = () => RedirectToReferrerOr("Index", "Home")
            });
        }

        [HttpDelete]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id, new MMSINC.Utilities.ActionHelperDoDestroyArgs
            {
                OnSuccess = () => RedirectToReferrerOr("Index", "Home")
            });
        }

        #endregion
    }
}