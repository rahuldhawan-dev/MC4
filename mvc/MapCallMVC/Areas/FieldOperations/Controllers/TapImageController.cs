using System;
using System.Web.Mvc;
using MapCall.Common.Helpers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Configuration;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Controllers;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class TapImageController : ControllerBaseWithPersistence<ITapImageRepository, TapImage, User>
    {
        #region Constants

        private const RoleModules ROLE = RoleModules.FieldServicesImages;

        private const string FILE_NOT_FOUND =
            "The requested image does not exist on the system. Please contact your administrator as the image may not have been uploaded correctly.";

        private const string INVALID_IMAGE_DATA =
            "The requested image is corrupt or otherwise unable to be displayed. Please contact your administrator as this image needs to be uploaded again.";

        public const string SAMPLE_SITE_WARNING = "This Tap Image is linked to a Service Record that is Linked to a Sample Site. Contact WQ before making any changes.";

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.Search:
                    this.AddOperatingCenterDropDownData();
                    this.AddDropDownData<ServiceMaterial>("ServiceMaterial");
                    this.AddDropDownData<ServiceSize>("ServiceSize", x => x.Id, x => x.ServiceSizeDescription);
                    this.AddDropDownData<ServiceMaterial>("CustomerSideMaterial");
                    this.AddDropDownData<ServiceMaterial>("PreviousServiceMaterial");
                    break;

                case ControllerAction.Edit:
                    this.AddOperatingCenterDropDownData();
                    this.AddOperatingCenterDropDownData("OperatingCenterIdentifier");
                    break;
                case ControllerAction.New:
                    // OpCenter needs to be added for two different dropdowns.
                    this.AddOperatingCenterDropDownData(x => x.IsActive);
                    this.AddOperatingCenterDropDownData("OperatingCenterIdentifier");
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchTapImage>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchTapImage model)
        {
            return this.RespondTo(f => {
                f.View(() => ActionHelper.DoIndex(model));
                // used in premise show page
                f.Fragment(() => ActionHelper.DoIndex(model, new ActionHelperDoIndexArgs
                {
                    IsPartial = true,
                    ViewName = "_Index",
                    OnNoResults = () => PartialView("_NoResults")
                }));
                f.Excel(() => ActionHelper.DoExcel(model));
            });
        }

        [HttpGet, RequiresRole(ROLE), AuditImage]
        public ActionResult Show(int id)
        {
            return this.RespondTo(f => {
                f.View(() => {
                    return ActionHelper.DoShow(id, null, model => {
                        if (model.Service?.Premise?.SampleSites != null && 
                            model.Service.Premise.SampleSites.Any())
                        {
                            DisplayErrorMessage(SAMPLE_SITE_WARNING);
                        }
                    });
                });
                f.Pdf(() => {
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

        [HttpGet, RequiresRole(ROLE, RoleActions.Add), ActionBarVisible(false)]
        public ActionResult New(int? id = null, int? originalImageId = null)
        {
            var model = new CreateTapImage(_container) {Service = id};

            if (originalImageId.HasValue)
            {
                var original = Repository.Find(originalImageId.Value);
                if (original != null)
                {
                    model.OperatingCenter = original.OperatingCenter != null ? original.OperatingCenter.Id : (int?)null;
                    // Setting Town should also set county and state
                    model.Town = original.Town != null ? original.Town.Id : (int?)null;
                    model.TownSection = original.TownSection;
                    model.StreetNumber = original.StreetNumber;
                    model.Street = original.Street;
                    model.StreetPrefix = original.StreetPrefix;
                    model.StreetSuffix = original.StreetSuffix;
                    model.PremiseNumber = original.PremiseNumber;
                    model.ServiceNumber = original.ServiceNumber;
                    model.Service = original.Service != null ? original.Service.Id : (int?)null;
                    model.ServiceType = original.ServiceType;
                    model.ApartmentNumber = original.ApartmentNumber;
                    model.Lot = original.Lot;
                    model.Block = original.Block;
                }
            }
            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateTapImage model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditTapImage>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditTapImage model)
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

        public TapImageController(ControllerBaseWithPersistenceArguments<ITapImageRepository, TapImage, User> args) : base(args) {}
    }
}
