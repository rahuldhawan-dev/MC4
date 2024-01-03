using System;
using System.Web.Mvc;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallIntranet.Models.ViewModels;
using MapCallIntranet.Helpers;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using System.Linq;
using System.Reflection;
using MMSINC.Metadata;

namespace MapCallIntranet.Controllers
{
    public class NearMissController : ControllerBaseWithPersistence<INearMissRepository, NearMiss, User>
    {
        private const int DOCUMENT_TYPE = 609;
        private const int DATA_TYPE = 209;
        private const string NEAR_MISS_DOCUMENT = "Near Miss Document";
        private const string NEAR_MISS_TABLE = "NearMisses";

        #region New/Create

        [Authorize]
        [HttpGet]
        public ActionResult New(int? type)
        {
            var safetyType = type.GetValueOrDefault() == NearMissType.Indices.ENVIRONMENTAL
                ? NearMissType.Indices.ENVIRONMENTAL
                : NearMissType.Indices.SAFETY;

            return ActionHelper.DoNew(ViewModelFactory.BuildWithOverrides<CreateNearMiss>(new {
                Type = safetyType
            }));
        }

        [Authorize]
        [HttpPost, RequiresSecureForm(false)]
        public ActionResult Create(CreateNearMiss model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    foreach (AjaxFileUpload fileUpload in model.FileUploads)
                    {
                        if (fileUpload != null)
                        {
                            SaveDocument(model.Id, fileUpload);
                        }
                    }
                    return RedirectToAction("Show", "NearMiss", new { id = model.Id });
                }
            });
        }

        [Authorize]
        [HttpGet]
        public ActionResult Show(CreateNearMiss model)
        {
            return View(model);
        }

        #endregion

        #region SaveDocument

        private void SaveDocument(int id, AjaxFileUpload fileUpload)
        {
            var docDataRepo = _container.GetInstance<IDocumentDataRepository>();
            var docData = docDataRepo.FindByBinaryData(fileUpload.BinaryData);

            DataType datatype = _container.GetInstance<IDataTypeRepository>().GetByTableName(NEAR_MISS_TABLE).SingleOrDefault();
            if (docData == null)
            {
                var documentData = new DocumentData { BinaryData = fileUpload.BinaryData };
                var document = new Document {
                    DocumentData = documentData,
                    DocumentType = datatype.DocumentTypes.Single(dt => dt.Name == NEAR_MISS_DOCUMENT),
                    FileName = fileUpload.FileName
                };
                _container.GetInstance<IDocumentRepository>().Save(document);
                var documentLink = new DocumentLink {
                    DataType = datatype,
                    Document = document,
                    DocumentType = datatype.DocumentTypes.Single(dt => dt.Name == NEAR_MISS_DOCUMENT),
                    LinkedId = id
                };
                _container.GetInstance<IRepository<DocumentLink>>().Save(documentLink);
            }
            else
            {
                var document = new Document {
                    DocumentData = docData,
                    DocumentType = datatype.DocumentTypes.Single(dt => dt.Name == NEAR_MISS_DOCUMENT),
                    FileName = fileUpload.FileName
                };
                var documentLink = new DocumentLink {
                    DataType = datatype,
                    Document = document,
                    DocumentType = new DocumentType { Id = DOCUMENT_TYPE },
                    LinkedId = id
                };
                _container.GetInstance<IDocumentRepository>().Save(document);
                _container.GetInstance<IRepository<DocumentLink>>().Save(documentLink);
            }
        }

        #endregion

        #region Constructors

        public NearMissController(ControllerBaseWithPersistenceArguments<INearMissRepository, NearMiss, User> args) : base(args) { LoginHelper.DoLogin(); }

        #endregion
    }
}