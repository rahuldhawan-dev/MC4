using MapCall.Common.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Models;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Results;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class IncompleteWorkOrdersController : ControllerBaseWithPersistence<IWorkOrderRepository, WorkOrder, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;
        public const int MAX_INDEX_RESULTS = 1000;

        #endregion

        #region Constructors

        public IncompleteWorkOrdersController(ControllerBaseWithPersistenceArguments<IWorkOrderRepository, WorkOrder, User> args) : base(args) { }

        #endregion

        #region Search/Index

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchIncompleteWorkOrders>();
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchIncompleteWorkOrders search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                    MaxResults = MAX_INDEX_RESULTS,
                    SearchOverrideCallback = () => {
                        search.EnablePaging = true;
                        var results = Repository.GetIncompleteWorkOrdersByWorkDescriptionId(search).ToList();
                        search.Count = results.Count;
                        search.Results = results;
                    }
                }));
                formatter.Excel(() => {
                    var results = Repository.GetIncompleteWorkOrdersByWorkDescriptionId(search)
                                            .Select(x => new {
                                                 WorkOrderNumber = x.Id,
                                                 x.StreetNumber,
                                                 Street = x.Street.Description,
                                                 x.ApartmentAddtl,
                                                 x.Town,
                                                 WorkDescription = x.WorkDescription.Description,
                                                 AssetType = x.AssetType.Description,
                                                 x.AssetId,
                                                 x.Latitude,
                                                 x.Longitude,
                                                 x.DateReceived,
                                                 Crew = x.CurrentCrew.Description,
                                                 Contarctor = x.AssignedContractor.Description,
                                                 x.Purpose,
                                                 x.Priority
                                             });
                    return new ExcelResult().AddSheet(results, new MMSINC.Utilities.Excel.ExcelExportSheetArgs { });
                });
            });
        }

        #endregion
    }
}