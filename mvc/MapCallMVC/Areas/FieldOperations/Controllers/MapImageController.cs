using System;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Configuration;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    // NOTE: This page isn't linked from the menu as of 1/29/2018, not sure if the page is still linked elsewhere.
    // NOTE: MapImages can not be uploaded/inserted/edited/deleted.
    public class MapImageController : ControllerBaseWithPersistence<IMapImageRepository, MapImage, User>
    {
        #region Consts

        private const string FILE_NOT_FOUND = "The requested image does not exist on the system. Please contact your administrator as the image may not have been uploaded correctly.";

        private const string INVALID_IMAGE_DATA =
            "The requested image is corrupt or otherwise unable to be displayed. Please contact your administrator as this image needs to be uploaded again.";
        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            if (action == ControllerAction.Search)
            {
                this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(RoleModules.FieldServicesImages)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchMapImage>();
        }

        [HttpGet, RequiresRole(RoleModules.FieldServicesImages)]
        public ActionResult Index(SearchMapImage model)
        {
            return ActionHelper.DoIndex(model);
        }

        [HttpGet, RequiresRole(RoleModules.FieldServicesImages), AuditImage]
        public ActionResult Show(int id)
        {
            return this.RespondTo(f =>
            {
                f.View(() =>
                {
                    return ActionHelper.DoShow(id, null, x =>
                    {
                        ViewData["North"] = Repository.FindImageInDirection(x, MapImageDirection.North);
                        ViewData["South"] = Repository.FindImageInDirection(x, MapImageDirection.South);
                        ViewData["East"] = Repository.FindImageInDirection(x, MapImageDirection.East);
                        ViewData["West"] = Repository.FindImageInDirection(x, MapImageDirection.West);
                    });
                });
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

        public MapImageController(ControllerBaseWithPersistenceArguments<IMapImageRepository, MapImage, User> args) : base(args) { }
    }
}
