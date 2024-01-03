using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Configuration;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    // NOTE: This controller can not filter on Opcenter/role because not
    //       all of the records have OpCenter set. -Ross 9/18/2014

    public class AsBuiltImageController : ControllerBaseWithPersistence<IAsBuiltImageRepository, AsBuiltImage, User>
    {
        #region Constants
        // NOTE: This role is also used for the notification email!
        private const RoleModules ROLE = RoleModules.FieldServicesImages;

        private const string NEW_COORDINATE_ID_NOTIFICATION_TEMPLATE = "AsBuiltImage Coordinate Changed",
                             FILE_NOT_FOUND = "The requested image does not exist on the system. Please contact your administrator as the image may not have been uploaded correctly.";
        private const string INVALID_IMAGE_DATA =
            "The requested image is corrupt or otherwise unable to be displayed. Please contact your administrator as this image needs to be uploaded again.";
        #endregion

        #region Private Methods

        private void SendNotificationForNewRecordsOrChangedCoordinates(AsBuiltImage model)
        {
            var args = new NotifierArgs {
                OperatingCenterId = model.OperatingCenter.Id,
                Module = ROLE,
                Purpose = NEW_COORDINATE_ID_NOTIFICATION_TEMPLATE,
                Data = model
            };

            _container.GetInstance<INotificationService>().Notify(args);
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.New:
                case ControllerAction.Edit:
                case ControllerAction.Search:
                    this.AddOperatingCenterDropDownData();
                   // this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchAsBuiltImage>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchAsBuiltImage model)
        {
            return this.RespondTo(f =>
            {
                f.View(() => ActionHelper.DoIndex(model, new MMSINC.Utilities.ActionHelperDoIndexArgs {
                    RedirectSingleItemToShowView = false 
                }));
                f.Excel(() =>
                {
                    model.EnablePaging = false;
                    var results = Repository
                        .Search(model)
                        .Select(x => new {
                            x.Id,
                            Latitude = (x.Coordinate != null) ? (double)x.Coordinate.Latitude : (double?)null,
                            Longitude = (x.Coordinate != null) ? (double)x.Coordinate.Longitude : (double?)null,
                            OperatingCenter = (x.OperatingCenter != null) ? x.OperatingCenter.OperatingCenterCode : null,
                            x.Town,
                            x.TownSection,
                            x.ProjectName,
                            DateAdded = x.CreatedAt,
                            x.CoordinatesModifiedOn,
                            WBSNumber = x.TaskNumber
                        }).ToList();
                    return this.Excel(results);
                });
                f.Map(() => _container.GetInstance<IMapResultFactory>().Build(ModelState, Repository, model));
            });
        }

        [HttpGet, RequiresRole(ROLE), AuditImage]
        public ActionResult Show(int id)
        {
            return this.RespondTo(f =>
            {
                f.View(() => ActionHelper.DoShow(id));
                f.Fragment(() => ActionHelper.DoShow(id, new MMSINC.Utilities.ActionHelperDoShowArgs {
                    ViewName = "_ShowPopup",
                    IsPartial = true 
                }));
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
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateAsBuiltImage(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateAsBuiltImage model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    if (model.CoordinateChanged)
                    {
                        var entity = Repository.Find(model.Id);
                        SendNotificationForNewRecordsOrChangedCoordinates(entity);
                    }
                    return null;
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditAsBuiltImage>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditAsBuiltImage model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    if (model.CoordinateChanged)
                    {
                        var entity = Repository.Find(model.Id);
                        SendNotificationForNewRecordsOrChangedCoordinates(entity);
                    }

                    return null;
                }
            });
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        public AsBuiltImageController(ControllerBaseWithPersistenceArguments<IAsBuiltImageRepository, AsBuiltImage, User> args) : base(args) {}
    }
}
