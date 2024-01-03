using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Models;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Results;
using MMSINC.Utilities;
using MMSINC.Utilities.Pdf;

namespace MapCallMVC.Areas.Reports.Controllers
{
    [DisplayName("Compliance Report")]
    public class RegulatoryComplianceReportController : ControllerBaseWithPersistence<IProductionWorkOrdersEquipmentRepository,ProductionWorkOrderEquipment, User>
    {
        #region Constructor

        public RegulatoryComplianceReportController(ControllerBaseWithPersistenceArguments<IProductionWorkOrdersEquipmentRepository, ProductionWorkOrderEquipment, User> args) : base(args) { }

        #endregion

        #region Search/Index/Show

        [HttpGet]
        public ActionResult Search(SearchRegulatoryCompliance search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet]
        public ActionResult Index(SearchRegulatoryCompliance search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => { Repository.GetRegulatoryComplianceReport(search); }
                }));
                formatter.Pdf(() =>
                {
                    Repository.GetRegulatoryComplianceReport(search);
                    return search.NoPdf.GetValueOrDefault() ? View("Pdf", search) : new PdfResult(_container.GetInstance<IHtmlToPdfConverter>(), "Pdf", search);
                });
            });
        }

        #endregion
    }
}