using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Configuration;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class ValveImageController : ControllerBaseWithPersistence<IValveImageRepository, ValveImage, User>
    {
        #region Constants

        private const RoleModules ROLE = RoleModules.FieldServicesImages;
        private const string FILE_NOT_FOUND = "The requested image does not exist on the system. Please contact your administrator as the image may not have been uploaded correctly.";
        private const string INVALID_IMAGE_DATA =
         "The requested image is corrupt or otherwise unable to be displayed. Please contact your administrator as this image needs to be uploaded again.";

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.Search:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE);
                    this.AddDropDownData<ValveNormalPosition>("NormalPosition");
                    this.AddDropDownData<ValveOpenDirection>("OpenDirection");
                    break;

                case ControllerAction.Edit:
                case ControllerAction.New:
                    // OpCenter needs to be added for two different dropdowns.
                    this.AddOperatingCenterDropDownData();
                    this.AddOperatingCenterDropDownData("OperatingCenterIdentifier");
                   // this.AddDropDownData<Street>("StreetIdentifyingInteger");
                    this.AddDropDownData<ValveNormalPosition>("NormalPosition");
                    this.AddDropDownData<ValveOpenDirection>("OpenDirection");
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchValveImage>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchValveImage model)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(model));
                formatter.Excel(() => {
                    model.EnablePaging = false;
                    var results = Repository
                                 .Search(model)
                                 .Select(x => new {
                                      x.Id,
                                      x.FileName,
                                      x.OperatingCenter,
                                      State = x.Town.State,
                                      County = x.Town.County,
                                      x.Town,
                                      x.TownSection,
                                      x.FullStreetName,
                                      x.ValveNumber,
                                      ValveId = x.Valve?.Id,
                                      x.FullCrossStreetName,
                                      x.Location,
                                      x.NormalPosition,
                                      x.NumberOfTurns,
                                      x.DateCompleted,
                                      x.ValveSize,
                                      x.OpenDirection,
                                      x.IsDefaultImageForValve,
                                      x.OfficeReviewRequired,
                                      DateAdded = x.CreatedAt
                                  }).ToList();
                    return this.Excel(results);
                });
            });
        }

        [HttpGet, RequiresRole(ROLE), AuditImage]
        public ActionResult Show(int id)
        {
            return this.RespondTo(f =>
            {
                f.View(() => ActionHelper.DoShow(id));
                f.Pdf(() =>
                {
                    var model = Repository.Find(id);
                    if (model == null)
                    {
                        return HttpNotFound();
                    }

                    try
                    {
                        var pdf = Repository.GetImageDataAsPdf(model);
                        return new AssetImagePdfResult(pdf, model);
                    }
                    catch (AssetImageException ex)
                    {
                        switch (ex.AssetExceptionType)
                        {
                            case AssetImageExceptionType.FileNotFound:
                                return HttpNotFound(FILE_NOT_FOUND);
                            case AssetImageExceptionType.InvalidImageData:
                                return HttpNotFound(INVALID_IMAGE_DATA);
                            default:
                                throw new NotSupportedException();
                        }
                    }
                });
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(int? id = null, int? originalImageId = null)
        {
            // NOTE: id is not a required field. The links that include the id come from 
            //       a Valve/Show page. Links without come from ValveImage page.
            var model = new CreateValveImage(_container);
            model.Valve = id;

            if (originalImageId.HasValue)
            {
                var original = Repository.Find(originalImageId.Value);
                if (original != null)
                {
                    model.OperatingCenter = original.OperatingCenter?.Id;
                    // Setting Town should also set county and state
                    model.Town = original.Town?.Id;
                    model.TownSection = original.TownSection;
                    model.StreetNumber = original.StreetNumber;
                    model.Street = original.Street;
                    model.StreetPrefix = original.StreetPrefix;
                    model.StreetSuffix = original.StreetSuffix;
                    model.Valve = original.Valve?.Id;
                    model.ValveNumber = original.ValveNumber;
                    model.CrossStreet = original.CrossStreet;
                    model.CrossStreetPrefix = original.CrossStreetPrefix;
                    model.CrossStreetSuffix = original.CrossStreetSuffix;
                    model.NormalPosition = original.NormalPosition?.Id;
                    model.ValveSize = original.ValveSize;
                    model.OpenDirection = original.OpenDirection?.Id;
                }
            }

            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateValveImage model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditValveImage>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditValveImage model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        public ValveImageController(ControllerBaseWithPersistenceArguments<IValveImageRepository, ValveImage, User> args) : base(args) { }
    }
}
