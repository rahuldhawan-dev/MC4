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
    public class ValveImageLinkController : ControllerBaseWithPersistence<IValveImageRepository, ValveImage, User>
    {
        #region Constructors

        public ValveImageLinkController(ControllerBaseWithPersistenceArguments<IValveImageRepository, ValveImage, User> args) : base(args) {}

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
        public ActionResult Search(SearchValveImageLink search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet]
        public ActionResult Index(SearchValveImageLink search)
        {
            var args = new ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => Repository.GetValveImageLinks(search)
            };
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search, args));
                formatter.Excel(() => ActionHelper.DoExcel(search, args));
            });
        }

        #endregion
    }
}