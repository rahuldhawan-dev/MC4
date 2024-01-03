using System;
using System.Web.Mvc;
using Contractors.Data.DesignPatterns.Mvc;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Metadata;

namespace Contractors.Controllers
{
    public class DocumentLinkController : ControllerBaseWithValidation<DocumentLink>
    {
        #region Constants

        public const string FRAGMENT_IDENTIFIER = "#Documents";

        #endregion

        protected void SetDocumentTypesLookup(string tableName)
        {
            this.AddDropDownData<IDocumentTypeRepository, DocumentType>(r => r.GetByTableName(tableName),
                                                            t => t.Id, t => t.Name);
        }

        #region Edit/Update

        [HttpGet, RequiresSecureForm]
        public ActionResult Edit(int id)
        {
            var entity = Repository.Find(id);
            if (entity == null)
                return DoHttpNotFound(String.Format("Document Link with the id {0} was not found.", id));

            SetDocumentTypesLookup(entity.DataType.TableName);
            var model = _container.GetInstance<EditDocumentLink>();
            model.Map(entity);
            model.UrlReferrer = Request.UrlReferrer.ToString();
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(EditDocumentLink model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs
            {
                OnSuccess = () => Redirect(model.UrlReferrer)
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

        public DocumentLinkController(ControllerBaseWithPersistenceArguments<MMSINC.Data.NHibernate.IRepository<DocumentLink>, DocumentLink, ContractorUser> args) : base(args) { }
    }
}