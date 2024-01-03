using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Results;

namespace MapCallMVC.Areas.WaterQuality.Controllers
{
    // NOTE: This is not regression tested because the test would need to generate an excel file dynamically
    //       and then save and upload it which I'm not even sure is possible. -Ross 3/16/2018

    public class BacterialWaterSampleMassEditController : ControllerBaseWithPersistence<IBacterialWaterSampleRepository, BacterialWaterSample, User>
    {
        #region Constants

        // This controller is basically an extension of the BacterialWaterSampleController and should
        // always use the same role.
        public const RoleModules ROLE = BacterialWaterSampleController.ROLE;

        #endregion

        #region New/Create

        // NOTE: Must be Edit action as this acts like editing a BacterialWaterSample.
        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult New()
        {
            // TODO: This would make a lot more sense as part of the view but we don't have that ability at the moment.
            DisplayNotification(
                "When importing bacterial water samples, you can only import a maximum of 200 records at one time. Files with more than 200 records will be rejected.");
            return ActionHelper.DoNew(new CreateBacterialWaterSampleMassEdit(_container));
        }

        // NOTE: Must be Edit action as this acts like editing a BacterialWaterSample.
        // It's a Create action because you're creating a new mass import
        // It's an Edit because you're editing existing BacterialWaterSample records
        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Create(CreateBacterialWaterSampleMassEdit model)
        {
            if (model.FileUpload != null && model.FileUpload.Items.Count > 200)
            {
                // This is being done in the controller, and not the view model, because the view model is
                // more than capable of importing thousands of records. However, the controller action redirects
                // to a search result page with these items and the resulting Index querystring can end up being too long
                // and an error gets thrown because of it. 
                //
                // 200 records with at least one change in each will take around 15 seconds to save.
                // 200 records with 12 changes each will take around 90 seconds to save. This is mostly due to audit logging.
                ModelState.AddModelError("FileUpload", "You may only import a maximum of 200 records at one time.");
            }

            if (!ModelState.IsValid)
            {
                DisplayModelStateErrors();

                //  Have this return View("New") 
                return RedirectToAction("New");
            }
            else
            {
                var allIds = model.FileUpload.Items.Select(x => x.Id).Distinct().ToList();
                var entityDict = Repository.FindManyByIds(allIds);

                // Hi, are you here from the future and looking for an example of how to save
                // something using an ExcelAjaxFileUpload model? Cool! You should consider talking
                // to Ross about creating a ViewModelCollection<T> style class. We have ViewModelSet<T>
                // but it's not implemented to work with ActionHelper and it could also use some
                // refactoring. -Ross 3/14/2018

                foreach (var vm in model.FileUpload.Items)
                {
                    vm.MapToEntity(entityDict[vm.Id]);
                }

                Repository.Save(entityDict.Values);

                foreach (var entity in entityDict.Values)
                {
                    BacterialWaterSampleController.SendCreationsMostBodaciousNotification(_container, entity);
                }

                DisplaySuccessMessage("Successfully updated bacterial water sample recods.");

                var rvd = new RouteValueDictionary();
                // This is necessary to work with the Int32ArrayModelBinder
                rvd.Add("EntityId", string.Join(",", entityDict.Keys));
                return DoRedirectionToAction("Index", "BacterialWaterSample", rvd);
            }
        }

        #endregion

        #region Export

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Export(SearchBacterialWaterSample search)
        {
            return this.RespondTo((formatter) =>
            {
                formatter.Excel(() =>
                {
                    search.EnablePaging = false;
                    var results = Repository.Search(search);

                    // MC-86: Only certain fields are to be exported for mass editing.
                    // NOTE: This is thoroughly unit tested. If a column is added here then it MUST
                    //       be added to the unit test.
                    var excelResult = new ExcelResult();
                    excelResult.AddSheet(results.Select(x => new {
                        // Display fields
                        x.Id,
                        x.OperatingCenter,
                        x.SampleSite,
                        x.Location,
                        x.SampleCollectionDTM,
                        x.CollectedBy,
                        x.BacterialSampleType,

                        // Editable fields
                        x.Ph,
                        x.Temperature,
                        x.Cl2Free,
                        x.Cl2Total,
                        x.Monochloramine,
                        x.FreeAmmonia,
                        x.Nitrite,
                        x.Nitrate,
                        x.Alkalinity,
                        x.Iron,
                        x.Manganese,
                        x.Turbidity,
                        x.Conductivity,
                        x.OrthophosphateAsP,
                        x.OrthophosphateAsPO4,
                        x.SampleNumber,
                        x.ReceivedByLabDTM,
                        x.ColiformConfirm,
                        ColiformConfirmMethod = x.ColiformConfirmMethod?.Id,
                        ColiformSetupAnalyst = x.ColiformSetupAnalyst?.Id,
                        x.ColiformSetupDTM,
                        ColiformReadAnalyst = x.ColiformReadAnalyst?.Id,
                        x.ColiformReadDTM,
                        x.FinalHPC,
                        HPCConfirmMethod = x.HPCConfirmMethod?.Id,
                        x.IsSpreader,
                        HPCSetupAnalyst = x.HPCSetupAnalyst?.Id,
                        x.HPCSetupDTM,
                        HPCReadAnalyst = x.HPCReadAnalyst?.Id,
                        x.HPCReadDTM,
                        IsReadyForLIMS = BacterialWaterSampleViewModelHelper.GetIsReadyForLIMS(x)
                    }), new MMSINC.Utilities.Excel.ExcelExportSheetArgs { SheetName = "Samples" });

                    // Add lookup sheet for all confirmation methods.
                    var allConfirmMethods = _container.GetInstance<IRepository<BacterialWaterSampleConfirmMethod>>().GetAll();
                    excelResult.AddSheet(allConfirmMethods, new MMSINC.Utilities.Excel.ExcelExportSheetArgs { SheetName = "Confirm Methods" });

                    // Add lookup sheet for all analyst employees
                    var analystRepo = _container.GetInstance<IBacterialWaterSampleAnalystRepository>();
                    var activeAnalysts = new List<MassEditBacterialAnalyst>();

                    // NOTE: This is using the results, rather than the search model's OperatingCenter values, to find
                    // analysts. They could search without selecting an OperatingCenter which would then not give us any
                    // operating center values to filter thits list by.

                    // NOTE 2: We want the final list to be ordered by Operating Center Code and then by the Employee's Name
                    
                    // NOTE 3: OperatingCenter is nullable on BacterialWaterSample. It's a required field now but old fields
                    // may cause things to explode. We want to ignore null OperatingCenters here since there won't be analysts
                    // for those anyway. If this actually becomes a problem then someone needs to go set the OperatingCenter
                    // for the record in question.
                    foreach (var opc in results.Select(x => x.OperatingCenter).Distinct().Where(x => x != null).OrderBy(x => x.OperatingCenterCode))
                    {
                        activeAnalysts.AddRange(analystRepo.GetAllActiveByOperatingCenter(opc.Id).OrderBy(x => x.Employee.LastName).ThenBy(x => x.Employee.FirstName).Select(x => new MassEditBacterialAnalyst {
                            Id = x.Id,
                            Employee = x.Employee,
                            OperatingCenter = opc
                        }));
                    }
                    excelResult.AddSheet(activeAnalysts, new MMSINC.Utilities.Excel.ExcelExportSheetArgs { SheetName = "Analysts" });

                    return excelResult;
                });
            });
        }

        #endregion

        #region Constructor

        public BacterialWaterSampleMassEditController(ControllerBaseWithPersistenceArguments<IBacterialWaterSampleRepository, BacterialWaterSample, User> args) : base(args) { }

        #endregion
    }
}