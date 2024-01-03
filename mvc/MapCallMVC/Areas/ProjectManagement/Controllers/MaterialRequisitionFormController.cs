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
using MMSINC.Results;
using MMSINC.Utilities.Pdf;

namespace MapCallMVC.Areas.ProjectManagement.Controllers
{
    public class MaterialRequisitionFormController : ControllerBaseWithPersistence<IEstimatingProjectRepository, EstimatingProject, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesEstimatingProjects;
        public const string MODEL_TEMP_DATA_KEY = "MODEL";

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

            var model = ViewModelFactory.Build<MaterialRequisitionForm, EstimatingProject>(project);

            return this.RespondTo(x => {
                x.View(() => View("Show", model));
                x.Pdf(() => new PdfResult(_container.GetInstance<IHtmlToPdfConverter>(), "Show", model));
                x.Excel(() => {
                    var groupedMaterials = model.GroupedMaterials.Select(m => new {
                        StockPartNumber = m.Material.PartNumber,
                        Units = m.Quantity,
                        SAPDescription = m.Material.Description
                    });
                    var workSheetName = model.ProjectName;
                    var header = String.Format("[{0}] - {1} - {2} - {3}", model.Id, model.ProjectName, model.Town, model.JDEPayrollNumber);
                    return this.Excel(groupedMaterials, new MMSINC.Utilities.Excel.ExcelExportSheetArgs {
                        SheetName = workSheetName,
                        Header = header
                    });
                });
            });
        }

        #endregion

        public MaterialRequisitionFormController(ControllerBaseWithPersistenceArguments<IEstimatingProjectRepository, EstimatingProject, User> args) : base(args) {}
    }
}