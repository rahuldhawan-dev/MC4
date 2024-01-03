using System;
using System.Web.Mvc;
using Contractors.Configuration;
using Contractors.Data.DesignPatterns.Mvc;
using Contractors.Data.Models.Repositories;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace Contractors.Controllers
{
    public class TapImageController : ControllerBaseWithValidation<ITapImageRepository, TapImage>
    {
        #region Constants

        public static string WORK_ORDER_LOOKUP_ERROR = "No such work order.";
        private const string FILE_NOT_FOUND =
            "The requested image does not exist on the system. Please contact your administrator as the image may not have been uploaded correctly.";

        private const string INVALID_IMAGE_DATA =
            "The requested image is corrupt or otherwise unable to be displayed. Please contact your administrator as this image needs to be uploaded again.";

        #endregion

        #region Constructor

        public TapImageController(ControllerBaseWithPersistenceArguments<ITapImageRepository, TapImage, ContractorUser> args) : base(args) { }

        #endregion

        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.Search:
                    this.AddDropDownData<OperatingCenter>(AuthenticationService.CurrentUser.Contractor.OperatingCenters, x => x.Id, x => x.ToString());
                    this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);
                    this.AddDropDownData<ServiceSize>(x => x.GetAllSorted(), x => x.Id, x => x.ServiceSizeDescription);
                    this.AddDropDownData<ServiceMaterial>(x => x.GetAllSorted(), x => x.Id, x => x.Description);
                    break;
            }
        }

        #endregion

        #region Actions

        [HttpGet]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch(new SearchTapImage());
        }

        [HttpGet]
        public ActionResult Index(SearchTapImage model)
        {
            return ActionHelper.DoIndex(model);
        }

        [HttpGet]
        public ActionResult Show(int id)
        {
            return this.RespondTo(f => {
                f.View(() => ActionHelper.DoShow(id));
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

        #endregion

        [HttpGet]
        public ActionResult New(int? id = null, int? originalImageId = null)
        {
            var model = _viewModelFactory.BuildWithOverrides<CreateTapImage>(new {Service = id});

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

        [HttpPost]
        public ActionResult Create(CreateTapImage image)
        {
            return ActionHelper.DoCreate(image, new MMSINC.Utilities.ActionHelperDoCreateArgs
            {
                OnSuccess = () => RedirectToAction("Show", "Service", new { id = image.Service })
            });
        }
    }
}
