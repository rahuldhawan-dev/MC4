using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class TapImageLinkController : ControllerBaseWithPersistence<ITapImageRepository, TapImage, User>
    {
        #region Constructors

        public TapImageLinkController(ControllerBaseWithPersistenceArguments<ITapImageRepository, TapImage, User> args) : base(args) {}

        #endregion

        #region Private Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            if (action == ControllerAction.Search)
                this.AddOperatingCenterDropDownData();
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public ActionResult Search(SearchTapImageLink search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet]
        public ActionResult Index(SearchTapImageLink search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs
                {
                    SearchOverrideCallback = () => Repository.GetTapImageLinks(search)
                }));
                formatter.Excel(() => ActionHelper.DoExcel(search, new ActionHelperDoIndexArgs
                {
                    SearchOverrideCallback = () => Repository.GetTapImageLinks(search)
                }));
            });
        }

        #endregion
    }
}