using MapCall.Common.Model.Entities;
using MMSINC.Controllers;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Metadata;
using MapCallMVC.Areas.Production.Models.ViewModels;
using MMSINC.ClassExtensions;
using System.Globalization;
using MMSINC.Data.NHibernate;
using MapCallMVC.ClassExtensions;

namespace MapCallMVC.Areas.Production.Controllers
{
    [DisplayName(PAGE_TITLE)]
    public class NonRevenueWaterEntryController : ControllerBaseWithPersistence<IRepository<NonRevenueWaterEntry>, NonRevenueWaterEntry, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionNonRevenueWaterUnbilledUsage;
        public const string PAGE_TITLE = "Non Revenue Water Unbilled";

        #endregion

        #region Constructor

        public NonRevenueWaterEntryController(ControllerBaseWithPersistenceArguments<IRepository<NonRevenueWaterEntry>, NonRevenueWaterEntry, User> args) : base(args) { }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            switch (action)
            {
                case ControllerAction.Search:
                    var results = Enumerable.Range(1, 12).Select(i => new {
                        monthNumber = i,
                        monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(i)
                    }).ToList();

                    this.AddDropDownData("Month", results, x => x.monthNumber, x => x.monthName);
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, extraFilterP: x => x.IsActive);
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search()
        {
            var model = new SearchNonRevenueWaterEntry();
            var currentUser = AuthenticationService.CurrentUser;
            model.OperatingCenter = new[] { currentUser.DefaultOperatingCenter.Id };
            return ActionHelper.DoSearch(model);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id, onModelFound: (entity) => BuildBusinessUnitDropDown(id));
        }

        [HttpGet]
        [RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchNonRevenueWaterEntry search)
        {
            return ActionHelper.DoIndex(search);
        }

        #endregion

        #region Child Elements

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult AddNonRevenueWaterAdjustment(AddNonRevenueWaterAdjustment model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion
        
        private void BuildBusinessUnitDropDown(int id)
        {
            var nonRevenueWaterEntry = Repository.Find(id);
            var businessUnits = _container.GetInstance<IRepository<BusinessUnit>>().Linq
                                          .Where(x => x.OperatingCenter.Id == nonRevenueWaterEntry.OperatingCenter.Id &&
                                                      x.IsActive == true &&
                                                      x.Department.Id == Department.Indices.T_AND_D)
                                          .Select(x => x.BU).Distinct();
            
            this.AddDropDownData("BusinessUnit", businessUnits, x => x.ToString(), x => x.ToString());
        }
    }
}