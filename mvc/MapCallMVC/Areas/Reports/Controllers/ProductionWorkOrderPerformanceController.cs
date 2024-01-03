using System.ComponentModel;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Production.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using System.Web.Mvc;
using MMSINC.Results;
using MMSINC.Utilities.Pdf;

namespace MapCallMVC.Areas.Reports.Controllers
{
    [DisplayName("Production Work Order Performance")]
    public class ProductionWorkOrderPerformanceController : ControllerBaseWithPersistence<ProductionWorkOrder, User>
    {
        #region Constructor
        public ProductionWorkOrderPerformanceController(ControllerBaseWithPersistenceArguments<IRepository<ProductionWorkOrder>, ProductionWorkOrder, User> args) : base(args)
        {
        }

        #endregion

        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            if (action == ControllerAction.Search)
            {
                this.AddDropDownData(Repository.GetAll().Select(wo => wo.OperatingCenter.State).OrderBy(s => s.Abbreviation).Distinct(), x => x.Id, x => x.Abbreviation);
                this.AddDropDownData<OrderType>();
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet]
        public ActionResult Search(SearchProductionWorkOrderPerformance search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet]
        public ActionResult Index(SearchProductionWorkOrderPerformance search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => { Repository.GetPerformanceReport(search); }
                }));
                formatter.Pdf(() => {
                    Repository.GetPerformanceReport(search);
                    return search.NoPdf.GetValueOrDefault() ? View("Pdf", search) : new PdfResult(_container.GetInstance<IHtmlToPdfConverter>(), "Pdf", search);
                });
            });
        }

        #endregion
    }
}