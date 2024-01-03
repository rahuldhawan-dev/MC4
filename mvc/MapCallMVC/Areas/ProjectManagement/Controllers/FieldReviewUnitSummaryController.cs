using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.WebApi;
using MMSINC.Results;
using MMSINC.Utilities.Pdf;
using StructureMap;

namespace MapCallMVC.Areas.ProjectManagement.Controllers
{
    public class FieldReviewUnitSummaryController : ControllerBaseWithPersistence<IEstimatingProjectRepository, EstimatingProject, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesEstimatingProjects;
        public const string MODEL_TEMP_DATA_KEY = "MODEL";

        #endregion
        
        #region Constructors

        public FieldReviewUnitSummaryController(ControllerBaseWithPersistenceArguments<IEstimatingProjectRepository, EstimatingProject, User> args) : base(args) {}
        public FieldReviewUnitSummaryController() : this(null){}

        #endregion
 
        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public virtual ActionResult Show(int id)
        {
            var project = Repository.Find(id);

            if (project == null)
            {
                return DoHttpNotFound(String.Format("Estimating Project with id {0} not found.", id));
            }

            var model = ViewModelFactory.Build<FieldReviewUnitSummary, EstimatingProject>(project);

            return this.RespondTo(x =>
            {
                x.View(() => View("Show", model));
                x.Pdf(() => new PdfResult(_container.GetInstance<IHtmlToPdfConverter>(), "Show", model));
                x.Excel(() => {
                    var worksheetName = model.ProjectName;
                    var header = String.Format("{0} - {1} - {2}", model.Id, model.ProjectName, model.Town);
                    var data = model.GroupedContractorLaborCosts.Select(m => new {
                        m.ContractorLaborCost.Description, m.Quantity, m.ContractorLaborCost.Unit
                    });
                    return this.Excel(data, new MMSINC.Utilities.Excel.ExcelExportSheetArgs {
                        SheetName = worksheetName,
                        Header = header
                    });
                });
            });
        }

        #endregion
    }
}