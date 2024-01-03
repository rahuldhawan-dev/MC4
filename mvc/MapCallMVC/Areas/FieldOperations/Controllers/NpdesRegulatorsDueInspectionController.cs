using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    /// <summary>
    /// This controller handles the results when clicking on the Town link from NpdesRegulatorsDueInspectionReport in the Reports folder.
    /// It returns the Index of all the NpdesRegulators (SewerOpenings) that are in the Count for that town,
    ///     which are NpdesRegulators that have an inspection due.
    /// </summary>
    [DisplayName("NPDES Regulators Due Inspection")]
    public class NpdesRegulatorsDueInspectionController : ControllerBaseWithPersistence<ISewerOpeningRepository, SewerOpening, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesAssets;

        #endregion

        #region Constructor

        public NpdesRegulatorsDueInspectionController(ControllerBaseWithPersistenceArguments<ISewerOpeningRepository, SewerOpening, User> args) : base(args) { }

        #endregion

        private MapResult GetMapResult(SearchNpdesRegulatorsDueInspectionForMap search)
        {
            var result = _container.GetInstance<AssetMapResult>();

            if (!ModelState.IsValid)
            {
                return result;
            }

            if (Repository.GetCountForSearchSet(search) > SearchNpdesRegulatorsDueInspectionForMap.MAX_MAP_RESULT_COUNT)
            {
                return null;
            }

            var searchResult = Repository.SearchNpdesRegulatorsDueInspectionForMap(search);

            result.Initialize(searchResult);

            return result;
        }

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchNpdesRegulatorsDueInspection model)
        {
            var args = new MMSINC.Utilities.ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => Repository.GetNpdesRegulatorsDueInspection(model)
            };
            return this.RespondTo(f => {
                f.View(() => ActionHelper.DoIndex(model, args));
                f.Map(() => GetMapResult(model));
                f.Excel(() => {
                    model.EnablePaging = false;
                    var results = Repository.GetNpdesRegulatorsDueInspection(model).Select(e => new Dictionary<string, object> {
                        {nameof(SearchNpdesRegulatorsDueInspection.SewerOpeningId), e.Id},
                        {nameof(SearchNpdesRegulatorsDueInspection.SewerOpeningNumber), e.OpeningNumber},
                        {nameof(e.NpdesPermitNumber), e.NpdesPermitNumber},
                        {nameof(e.OutfallNumber), e.OutfallNumber},
                        {nameof(e.BodyOfWater), e.BodyOfWater},
                        {nameof(e.LocationDescription), e.LocationDescription},
                        {nameof(e.OperatingCenter), e.OperatingCenter.OperatingCenterCode},
                        {nameof(e.Town), e.Town},
                        {nameof(e.Status), e.Status}
                    });
                    return this.Excel(results);
                });
            });
        }

        #endregion
    }
}
