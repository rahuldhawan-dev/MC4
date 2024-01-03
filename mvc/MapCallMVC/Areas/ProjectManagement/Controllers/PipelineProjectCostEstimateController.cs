using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Results;
using MMSINC.Utilities.Pdf;
using StructureMap;

namespace MapCallMVC.Areas.ProjectManagement.Controllers
{
    public class PipelineProjectCostEstimateController : ControllerBaseWithPersistence<IEstimatingProjectRepository, EstimatingProject, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesEstimatingProjects;
        public const string MODEL_TEMP_DATA_KEY = "MODEL";

        #endregion

        #region Private Methods

        private IEnumerable<AssetType> GetOrderedAssetTypes()
        {
            var assetTypes = _container.GetInstance<IRepository<AssetType>>().GetAll();
            var returnAssetTypes = new List<AssetType>();
            //Mains
            returnAssetTypes.Add(assetTypes.First(x => x.Id == AssetType.Indices.MAIN));
            //Services
            returnAssetTypes.Add(assetTypes.First(x => x.Id == AssetType.Indices.SERVICE));
            //Hydrant
            returnAssetTypes.Add(assetTypes.First(x => x.Id == AssetType.Indices.HYDRANT));
            //Valve
            returnAssetTypes.Add(assetTypes.First(x => x.Id == AssetType.Indices.VALVE));
            //Sewer Main
            returnAssetTypes.Add(assetTypes.First(x => x.Id == AssetType.Indices.SEWER_MAIN));
            //Sewer Lateral
            returnAssetTypes.Add(assetTypes.First(x => x.Id == AssetType.Indices.SEWER_LATERAL));
            //Sewer Opening
            returnAssetTypes.Add(assetTypes.First(x => x.Id == AssetType.Indices.SEWER_OPENING));
            //Storm\Catch
            returnAssetTypes.Add(assetTypes.First(x => x.Id == AssetType.Indices.STORM_CATCH));
            //Equipment
            returnAssetTypes.Add(assetTypes.First(x => x.Id == AssetType.Indices.EQUIPMENT));
            //Facility
            //returnAssetTypes.Add(assetTypes.First(x => x.Id == AssetType.Indices.FACILITY));

            foreach (var assetType in assetTypes)
            {
                if (!returnAssetTypes.Contains(assetType))
                    returnAssetTypes.Add(assetType);
            }

            return returnAssetTypes;
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            var model = (PipelineProjectCostEstimate)TempData[MODEL_TEMP_DATA_KEY];

            if (model == null)
            {
                return DoHttpNotFound("Model not found in TempData; visit create first.");
            }

            var project = Repository.Find(id);

            if (project == null)
            {
                return DoHttpNotFound(String.Format("Estimating Project with id {0} not found.", id));
            }

            model.Map(project);

            return this.RespondTo(x => {
                x.View(() => View("Show", model));
                x.Pdf(() => {
                    ViewData["AssetTypes"] = GetOrderedAssetTypes();
                    return new PdfResult(_container.GetInstance<IHtmlToPdfConverter>(), "Show", model);
                });
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult New(PipelineProjectCostEstimate model)
        {
            var id = model.Id;
            var project = Repository.Find(id);

            if (project == null)
            {
                return DoHttpNotFound(String.Format("Estimating Project with id {0} not found.", id));
            }

            if (!ModelState.IsValid)
                DisplayModelStateErrors();

            return View(ViewModelFactory.Build<PipelineProjectCostEstimate, EstimatingProject>(project));
        }

        [HttpPost, RequiresRole(ROLE)]
        public ActionResult Create(PipelineProjectCostEstimate model)
        {
            // TODO: This action should return the pdf immediately rather 
            // than storing it in TempData and redirecting back to Show. -Ross 1/28/2020
            if (!ModelState.IsValid)
            {
                DisplayModelStateErrors();
                return View("New", model);
            }

            TempData[MODEL_TEMP_DATA_KEY] = model;
            return RedirectToAction("Show", new {id = model.Id, ext = "pdf"});
        }

        #endregion

        public PipelineProjectCostEstimateController(ControllerBaseWithPersistenceArguments<IEstimatingProjectRepository, EstimatingProject, User> args) : base(args) {}
    }
}
