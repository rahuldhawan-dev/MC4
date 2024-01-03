using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Utility.AssetUploads;
using MapCallMVC.Models.ViewModels;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    [RequiresAdmin, DisplayName("Data Uploads")]
    public class AssetUploadController : ControllerBaseWithPersistence<AssetUpload, User>
    {
        #region Private Members

        private readonly IAssetUploadFileService _fileService;

        #endregion

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            if (action == ControllerAction.Search)
            {
                this.AddDropDownData<User>("CreatedBy", r => r.Where(u => u.AssetUploads.Any()), u => u.Id,
                    u => u.Description);
            }
        }

        #region Search/Index/Show

        [HttpGet]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch(new SearchAssetUpload());
        }

        [HttpGet]
        public ActionResult Index(SearchAssetUpload search)
        {
            return ActionHelper.DoIndex(search);
        }

        [HttpGet]
        public ActionResult Show(int id)
        {
            return this.RespondTo(f => {
                f.View(() => ActionHelper.DoShow(id));
                f.Excel(() => {
                    var model = Repository.Find(id);
                    if (model == null)
                    {
                        return HttpNotFound();
                    }

                    return File(_fileService.LoadFile(model.FileGuid), "application/octet-stream", model.FileName);
                });
            });
        }

        #endregion

        #region New/Create

        [HttpGet]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateAssetUpload(_container));
        }

        [HttpPost]
        public ActionResult Create(CreateAssetUpload upload)
        {
            return ActionHelper.DoCreate(upload);
        }

        #endregion

        public AssetUploadController(
            ControllerBaseWithPersistenceArguments<IRepository<AssetUpload>, AssetUpload, User> args,
            IAssetUploadFileService fileService) : base(args)
        {
            _fileService = fileService;
        }
    }
}