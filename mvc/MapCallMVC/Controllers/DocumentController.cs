using System;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Utilities.Documents;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Exceptions;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCallMVC.Controllers
{
    public class DocumentController : ControllerBaseWithPersistence<IDocumentRepository, Document, User>
    {
        #region Constants

        public const string FRAGMENT_IDENTIFIER = "#Documents";

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

        private void SetLookupData()
        {
            this.AddDropDownData<DocumentStatus>();
            this.AddDropDownData<RecurringFrequencyUnit>("ReviewFrequencyUnit");
        }

        #endregion

        #region New/Create

        // Do not add a HttpGet or HttpPost to this. This action needs to be accessible 
        // when a documents tab is used on an Edit view. The rendering for creating a document
        // and a document link in that tab are done via Html.Action, which will try to find
        // a route with the same http method as the initial request. This was HttpGet, but
        // if a POST fails and tries to render a view, then Html.Action would be unable to 
        // find this action and throw an error. 
        public ActionResult New(NewDocument model = null)
        {
            model = model ?? new NewDocument(_container);
            // explicitly called model.SetDefaults as this controller doesn't use ActionHelper.DoNew
            if (model.TableName == DocumentModel.HELP_TOPIC_TABLE_NAME) // set custom defaults for HelpTopics other defaults to nulls
            {
                model.SetHelpTopicDefaults();
            }

            SetDocumentTypes(model);
            SetLookupData();
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

        #region FindDocumentsByName

        [HttpPost, RequiresSecureForm(false)]
        public ActionResult FindDocumentsByName(string tableName, string docName)
        {
            var results =
                Repository
                    .GetByTableAndDocumentName(tableName, docName)
                    .Map<Document, object>(
                        d => new {
                            value = d.Id,
                            text = string.Format("{0} -> {1}", d.DocumentType.Name, d.FileName)
                        });
            return Json(results);
        }

        #endregion

        public DocumentController(ControllerBaseWithPersistenceArguments<IDocumentRepository, Document, User> args) : base(args) {}
    }
}
