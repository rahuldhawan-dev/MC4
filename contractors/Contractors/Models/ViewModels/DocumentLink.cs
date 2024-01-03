using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using IDocumentRepository = Contractors.Data.Models.Repositories.IDocumentRepository;

namespace Contractors.Models.ViewModels
{
    public class NewDocumentLink : ViewModel<DocumentLink>
    {
        #region Properties

        [Required, Secured]
        public virtual int LinkedId { get; set; }
        [Required, EntityMap(SecondaryPropertyName = nameof(DocumentLink.Document))] // Mapping done via MapToEntity
        public virtual int DocumentId { get; set; }
        [DropDown, Required, EntityMap, EntityMustExist(typeof(DocumentType))]
        public virtual int DocumentType { get; set; }
        // this is only to be used for querying existing documents by type
        // through the views
        [DoesNotAutoMap]
        public virtual string TableName { get; set; }

        #endregion

        #region Constructors

        public NewDocumentLink(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override DocumentLink MapToEntity(DocumentLink entity)
        {
            entity = base.MapToEntity(entity);

          //  entity.Document = _container.GetInstance<IDocument>().Find(DocumentId);
         //   entity.DocumentType = _container.GetInstance<IDocumentTypeRepository>().Find(DocumentType);
            entity.DataType = _container.GetInstance<IDataTypeRepository>().Find(entity.DocumentType.DataType.Id);

            return entity;
        }

        #endregion
    }

    public class EditDocumentLink : ViewModel<DocumentLink>
    {
        #region Properties

        [DropDown, Required, EntityMap, EntityMustExist(typeof(DocumentType))]
        public virtual int? DocumentType { get; set; }

        [DoesNotAutoMap]
        public int? DocumentId { get; set; }

        [DoesNotAutoMap]
        public string UrlReferrer { get; set; }

        [AutoMap(MapDirections.ToPrimary)]
        public Document Document
        {
            get { return (DocumentId.HasValue) ? _container.GetInstance<IRepository<Document>>().Find(DocumentId.Value) : null; }
        }

        #endregion

        #region Constructors

        public EditDocumentLink(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override void Map(DocumentLink entity)
        {
            base.Map(entity);
            DocumentId = entity.Document.Id;
        }

        public override DocumentLink MapToEntity(DocumentLink entity)
        {
            entity = base.MapToEntity(entity);
            return entity;
        }

        #endregion
    }

    public class DeleteDocumentLink : ViewModel<IDocumentLink>
    {
        #region Properties

        [Required, Secured]
        public override int Id { get; set; }

        #endregion

        #region Constructors

        public DeleteDocumentLink(IContainer container) : base(container) { }

        #endregion
    }
}