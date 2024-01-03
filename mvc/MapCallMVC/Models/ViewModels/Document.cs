using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using IContainer = StructureMap.IContainer;
using MMSINC.Validation;

namespace MapCallMVC.Models.ViewModels
{
    public class DocumentModel : ViewModel<Document>
    {
        public const string HELP_TOPIC_TABLE_NAME = "HelpTopics";
        
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
        [DropDown, DoesNotAutoMap("Mapped manually to DocumentLink"), Secured(AppliesToAdmins = false), DisplayName("Document Status")]
        public virtual int? DocumentStatus { get; set; }
        [DoesNotAutoMap("Mapped manually to DocumentLink")]
        public int? ReviewFrequency { get; set; }
        [DropDown, DoesNotAutoMap("Mapped manually to DocumentLink"), EntityMustExist(typeof(RecurringFrequencyUnit))]
        public int? ReviewFrequencyUnit { get; set; }
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
            return new NewDocumentLink(_container) {
                DocumentId = Id,
                DocumentType = DocumentType,
                LinkedId = LinkedId.Value,
                TableName = TableName,
                DocumentStatus = DocumentStatus,
                ReviewFrequency = ReviewFrequency,
                ReviewFrequencyUnit = ReviewFrequencyUnit
            };
        }

        public void SetHelpTopicDefaults()
        {
            // default to Active
            DocumentStatus = MapCall.Common.Model.Entities.DocumentStatus.Indices.ACTIVE;

            // default to 5 years
            ReviewFrequency = 5;
            ReviewFrequencyUnit = RecurringFrequencyUnit.Indices.YEAR;
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