using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Models;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Results;
using MMSINC.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class IncompleteLeaksController : ControllerBaseWithPersistence<IWorkOrderRepository, WorkOrder, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;
        public const int MAX_INDEX_RESULTS = 1000;

        #endregion

        #region Private Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            if (action == ControllerAction.Search)
            {
                this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE);
            }
        }

        #endregion

        #region Search/Index

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchIncompleteLeaks>();
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchIncompleteLeaks search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                    MaxResults = MAX_INDEX_RESULTS,
                    SearchOverrideCallback = () => {
                        search.EnablePaging = true;
                        var results = Repository.SearchForIncompleteLeaks(search).ToList();
                        search.Count = results.Count;
                        search.Results = results;
                    }
                }));
                formatter.Excel(() => {
                    var results = Repository.SearchForIncompleteLeaks(search)
                                            .Select(x => new {
                                                 OrderNumber = x.Id,
                                                 x.Town,
                                                 Address = x.StreetAddress,
                                                 DescriptionOfJob = x.WorkDescription,
                                                 x.DateReceived,
                                                 x.Priority,
                                                 x.Purpose,
                                                 x.CreatedBy,
                                             });

                    return new ExcelResult().AddSheet(results, new MMSINC.Utilities.Excel.ExcelExportSheetArgs { });
                });
            });
        }

        #endregion

        #region Constructors

        public IncompleteLeaksController(
            ControllerBaseWithPersistenceArguments<IWorkOrderRepository, WorkOrder, User> args) : base(args) { }

        #endregion
    }
}