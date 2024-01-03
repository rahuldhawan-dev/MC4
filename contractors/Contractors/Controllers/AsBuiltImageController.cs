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
    public class AsBuiltImageController : ControllerBaseWithValidation<IAsBuiltImageRepository, AsBuiltImage>
    {
        #region Constants

        private const string FILE_NOT_FOUND = "The requested image does not exist on the system. Please contact your administrator as the image may not have been uploaded correctly.";
        private const string INVALID_IMAGE_DATA =
            "The requested image is corrupt or otherwise unable to be displayed. Please contact your administrator as this image needs to be uploaded again.";
      
        #endregion

        #region Constructor

        public AsBuiltImageController(ControllerBaseWithPersistenceArguments<IAsBuiltImageRepository, AsBuiltImage, ContractorUser> args) : base(args) { }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.Search:
                    this.AddDropDownData<OperatingCenter>(AuthenticationService.CurrentUser.Contractor.OperatingCenters, x => x.Id, x => x.ToString());
                    this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch(new SearchAsBuiltImage());
        }

        [HttpGet]
        public ActionResult Index(SearchAsBuiltImage model)
        {
            return ActionHelper.DoIndex(model);
        }

        [HttpGet]
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

    }
}