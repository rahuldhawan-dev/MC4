using System;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Management.Models;
using MMSINC.Controllers;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Management.Controllers
{
    public class UserViewedDailyReportController : ControllerBaseWithPersistence<IUserViewedRepository, UserViewed, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ManagementGeneral;

        #endregion

        #region Constructor

        public UserViewedDailyReportController(ControllerBaseWithPersistenceArguments<IUserViewedRepository, UserViewed, User> args) : base(args) { }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            var today = _container.GetInstance<IDateTimeProvider>().GetCurrentDate().Date;
            return ActionHelper.DoSearch(new SearchUserViewedDailyRecordItem()
            {
                ViewedAt = new MMSINC.Data.RequiredDateRange
                {
                    End = today,
                    Operator = MMSINC.Data.RangeOperator.Equal
                }
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchUserViewedDailyRecordItem search)
        {
            // Grouping causes invalid result counts/paging.
            search.EnablePaging = false;
            return ActionHelper.DoIndex(search, new MMSINC.Utilities.ActionHelperDoIndexArgs
            {
                SearchOverrideCallback = () => Repository.SearchDailyReportItems(search)
            });
        }

        #endregion
    }
}