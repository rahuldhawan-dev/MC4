using System;
using System.Web.Mvc;
using Contractors.Data.DesignPatterns.Mvc;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Exceptions;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Documents;
using IDocumentRepository = Contractors.Data.Models.Repositories.IDocumentRepository;

namespace Contractors.Controllers
{
    public class DocumentController : ControllerBaseWithValidation<IDocumentRepository, Document>
    {
        #region Constants

        public const string FRAGMENT_IDENTIFIER = "#DocumentsTab";

        #endregion

        #region Private Methods

        protected void SetDocumentTypes(DocumentModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.TableName))
            {
                this.AddDropDownData<IDocumentTypeRepository, DocumentType>(r => r.GetByTableName(model.TableName),
                                                                            t => t.Id, t => t.Name);
            }
            else
            {
                this.AddDropDownData<IDocumentTypeRepository, DocumentType>(r => r.GetAll(),
                                                                            t => t.Id, t => t.Name);
            }
        }

        protected ActionResult LinkDocument(DocumentModel model)
        {
            var dl = _container.GetInstance<IRepository<DocumentLink>>()
                .Save(model.ToDocumentLink().MapToEntity(new DocumentLink()));

            return RedirectToReferrerOr("Index", "Home", FRAGMENT_IDENTIFIER);
        }

        #endregion

        #region New/Create

        [HttpGet]
        public ActionResult New(NewDocument model)
        {
            model = model ?? _container.GetInstance<NewDocument>();
            SetDocumentTypes(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(NewDocument model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    return string.IsNullOrWhiteSpace(model.TableName) && !model.LinkedId.HasValue
                        ? RedirectToAction("Index", "Home")
                        : LinkDocument(model);
                },
                // TODO: Look into why we're throwing an exception here rather than returning something useful to the client.
                OnError = () => throw new ModelValidationException(ModelState)
            });
        }

        #endregion

        #region Download

        [HttpGet, RequiresSecureForm] // Explicitly requires secure form.
        public ActionResult Download(int id)
        {
            var doc = Repository.Find(id);
            if (doc == null)
            {
                return HttpNotFound(string.Format("Document with id '{0}' not found.", id));
            }
            else
            {
                var docBinary = _container.GetInstance<IDocumentService>().Open(doc.DocumentData.Hash);
                return File(docBinary, "application/octet-stream", doc.FileName);
            }
        }

        #endregion

        public DocumentController(ControllerBaseWithPersistenceArguments<IDocumentRepository, Document, ContractorUser> args) : base(args) { }
    }
}