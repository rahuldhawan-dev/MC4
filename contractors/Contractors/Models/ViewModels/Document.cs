using System.ComponentModel.DataAnnotations;
using Contractors.Data.Models.Repositories;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace Contractors.Models.ViewModels
{
    public class DocumentModel : ViewModel<Document>
    {
        #region Properties

        [Required, DropDown]
        public virtual int DocumentType { get; set; }
        [Required, Display(Name = "Document Name")]
        public virtual string FileName { get; set; }
        /// <summary>
        /// Only used for linking documents when they're created
        /// </summary>
        [Required, Secured, DoesNotAutoMap("Mapped manually to DocumentLink")]
        public virtual string TableName { get; set; }
        /// <summary>
        /// Only used for linking documents when they're created
        /// </summary>
        [Required, Secured, DoesNotAutoMap("Mapped manually to DocumentLink")]
        public virtual int? LinkedId { get; set; }

        #endregion

        #region Constructors

        public DocumentModel(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override Document MapToEntity(Document entity)
        {
            entity = base.MapToEntity(entity);

            entity.DocumentType = _container.GetInstance<IDocumentTypeRepository>().Find(DocumentType);
            return entity;
        }

        public NewDocumentLink ToDocumentLink()
        {
            var model = _container.GetInstance<NewDocumentLink>();
            model.DocumentId = Id;
            model.DocumentType = DocumentType;
            model.LinkedId = LinkedId.Value;
            model.TableName = TableName;
            return model;
        }

        #endregion
    }

    public class NewDocument : DocumentModel
    {
        #region Properties

        [Required, FileUpload(OnComplete = "DocumentNew.onComplete"), DoesNotAutoMap("FileUploads don't map")]
        public AjaxFileUpload FileUpload { get; set; }

        #endregion

        #region Constructors

        public NewDocument(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override Document MapToEntity(Document entity)
        {
            entity = base.MapToEntity(entity);
            var docDataRepo = _container.GetInstance<IDocumentDataRepository>();
            var docData = docDataRepo.FindByBinaryData(FileUpload.BinaryData);
            if (docData == null)
            {
                docData = new DocumentData();
                docData.BinaryData = FileUpload.BinaryData;
                // Do not need to set any other properties on DocumentData here.
                // The DocumentData repository handles that in its Save method.
            }
            entity.DocumentData = docData;
            return entity;
        }

        #endregion
    }
}
