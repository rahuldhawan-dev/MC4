using System;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Exceptions;
using MMSINC.Metadata;

namespace MapCallMVC.Controllers
{
    public class DocumentLinkController : ControllerBaseWithPersistence<IRepository<DocumentLink>, DocumentLink, User>
    {
        #region Constants

        public const string FRAGMENT_IDENTIFIER = "#DocumentsTab";

        #endregion

        #region Private Methods

        protected void SetDocumentTypesLookup(string tableName)
        {
            this.AddDropDownData<IDocumentTypeRepository, DocumentType>(r => r.GetByTableName(tableName),
                t => t.Id, t => t.Name);
        }

        private void SetLookupData()
        {
            this.AddDropDownData<DocumentStatus>();
            this.AddDropDownData<RecurringFrequencyUnit>("ReviewFrequencyUnit");
        }
        
        #endregion

        #region Search/Index

        // NOTE: This is used by Views/Shared/Document/Index(and _SecureIndexForSingleRecord) and WithDocuments.
        [HttpGet, RequiresSecureForm, NoCache]
        public ActionResult SecureIndexForSingleRecord(SecureSearchDocumentForSingleEntity search)
        {
            // Force this to make 25 record pages. The view generate a lot of secure form tokens and
            // that significantly slows down load time when things have 100+ documents. 
            search.EnablePaging = true;
            search.PageSize = 25; // 25 was specifically requested by Doug.

#if DEBUG
            // Otherwise the functional tests need to create 25+ documents and that's just unnecessary.
            if (MMSINC.MvcApplication.IsInTestMode && MMSINC.MvcApplication.RegressionTestFlags.Contains("document page size 5"))
            {
                search.PageSize = 5;
            }
#endif
            return this.RespondTo(
                f => f.Fragment(() => ActionHelper.DoIndex(search, new MMSINC.Utilities.ActionHelperDoIndexArgs
                {
                    // TODO: I think we might need or want a specific query here.
                    ViewName = "~/Views/Shared/Document/_SecureIndexForSingleRecord.cshtml",
                    IsPartial = true,
                    // We also don't want this redirecting to the individual record for the same reason.
                    RedirectSingleItemToShowView = false,
                    // We don't want this to redirect back to the search page when there aren't results as this is loading in a tab.
                    OnNoResults = () => PartialView("~/Views/Shared/Document/_SecureIndexForSingleRecord.cshtml", search)
                })));
        }

        #endregion

        #region New/Create
        
        // Do not add a HttpGet or HttpPost to this. This action needs to be accessible 
        // when a documents tab is used on an Edit view. The rendering for creating a document
        // and a document link in that tab are done via Html.Action, which will try to find
        // a route with the same http method as the initial request. This was HttpGet, but
        // if a POST fails and tries to render a view, then Html.Action would be unable to 
        // find this action and throw an error. 
        public ActionResult New(NewDocumentLink model)
        {
            if (String.IsNullOrWhiteSpace(model.TableName))
            {
                ModelState.AddModelError("TableName", "TableName cannot be null.");
                throw new ModelValidationException(ModelState);
            }

            var documentTypes =
                _container.GetInstance<IDataTypeRepository>()
                             .GetByTableName(model.TableName).MapAndFlatten<DataType, DocumentType>(dt => dt.DocumentTypes);
            this.AddDropDownData<IDocumentTypeRepository, DocumentType>(r => documentTypes, dt => dt.Id, dt => dt.Name);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(NewDocumentLink model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => RedirectToReferrerOr("Index", "Home", FRAGMENT_IDENTIFIER),
                // TODO: Look into why we're throwing an exception here rather than returning something useful to the client.
                OnError = () => throw new ModelValidationException(ModelState)
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresSecureForm]
        public ActionResult Edit(int id)
        {
            var entity = Repository.Find(id);
            if (entity == null)
                return DoHttpNotFound(String.Format("Document Link with the id {0} was not found.", id));

            SetDocumentTypesLookup(entity.DataType.TableName);
            SetLookupData();

            return View(ViewModelFactory.BuildWithOverrides<EditDocumentLink, DocumentLink>(entity,
                new {UrlReferrer = Request.UrlReferrer.ToString(), TableName = entity.DataType.TableName}));
        }

        [HttpPost, RequiresSecureForm]
        public ActionResult Update(EditDocumentLink model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => Redirect(model.UrlReferrer),
                OnError = () => {
                    DisplayModelStateErrors();
                    return Redirect(model.UrlReferrer);
                }
            });
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete]
        public ActionResult Destroy(DeleteDocumentLink model)
        {
            var link = Repository.Find(model.Id);

            if (link == null)
            {
                return HttpNotFound(string.Format("DocumentLink with id '{0}' not found.", model.Id));
            }

            Repository.Delete(link);

            return RedirectToReferrerOr("Index", "Home", FRAGMENT_IDENTIFIER);
        }

        #endregion

        public DocumentLinkController(ControllerBaseWithPersistenceArguments<IRepository<DocumentLink>, DocumentLink, User> args) : base(args) {}
    }
}