using System.Collections.Generic;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data;

namespace MapCallMVC.Areas.Reports.Controllers
{
    /// <summary>
    /// Base class for a number of service reports. 
    /// WHERE ARE MY RESULTS?!??
    /// Make sure that the repository method is setting the search.Count and ToList()
    /// var results = Search(search, query).ToList();
    /// search.Count = results.Count();
    /// </summary>
    public abstract class ServiceReportController<TSearchModel,TSearchResultModel> : ControllerBaseWithPersistence<IServiceRepository, Service, User> where TSearchModel : class, ISearchSet<TSearchResultModel>
    {
        #region Constants

        #endregion

        #region Constructors

        public ServiceReportController(ControllerBaseWithPersistenceArguments<IServiceRepository, Service, User> args) : base(args) {}

        #endregion

        #region Private Methods

        protected abstract IEnumerable<TSearchResultModel> GetResults(TSearchModel search);

        /// <summary>
        /// This is used by ActionHelper.DoExcel. Overriding this will cause DoExcel to return
        /// this result as the success result rather than the default ExcelResult. Return null
        /// if DoExcel should continue returning the default ExcelResult.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        protected virtual ActionResult GetExcelResult(TSearchModel search)
        {
            return null;
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            switch (action)
            {
                case ControllerAction.Search:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(RoleModules.FieldServicesAssets);
                    break;
            }
        }

        #endregion

        #region Search/Index

        [HttpGet, RequiresRole(RoleModules.FieldServicesAssets)]
        public ActionResult Search(TSearchModel search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(RoleModules.FieldServicesAssets)]
        public ActionResult Index(TSearchModel search)
        {
            // Disable paging on every single report that inherits from this? Sure why not.
            search.EnablePaging = false;

            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search, new MMSINC.Utilities.ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => GetResults(search)
                }));
                formatter.Excel(() => ActionHelper.DoExcel(search, new MMSINC.Utilities.ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => GetResults(search),
                    OnSuccess = () => GetExcelResult(search)
                }));
            });
        }

        #endregion
    }
}