using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Models.ViewModels
{
    public class NewDocumentLink : ViewModel<DocumentLink>
    {
        #region Properties

        [Required, Secured]
        public virtual int LinkedId { get; set; }
        [Required, EntityMap("Document")]
        public virtual int DocumentId { get; set; }
        [DropDown, Required, EntityMap, EntityMustExist(typeof(DocumentType))]
        public virtual int DocumentType { get; set; }
        // this is only to be used for querying existing documents by type
        // through the views
        [DoesNotAutoMap("Used to map DataType")]
        public virtual string TableName { get; set; }
        [EntityMap]
        public virtual int? DocumentStatus { get; set; }
        public virtual int? ReviewFrequency { get; set; }
        [EntityMap]
        public virtual int? ReviewFrequencyUnit { get; set; }

        #endregion

        #region Constructors

        public NewDocumentLink(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override DocumentLink MapToEntity(DocumentLink entity)
        {
            entity = base.MapToEntity(entity);

            entity.DataType = _container.GetInstance<IDataTypeRepository>().Find(entity.DocumentType.DataType.Id);

            return entity;
        }

        #endregion
    }

    public class EditDocumentLink : ViewModel<DocumentLink>
    {
        #region Properties

        [DropDown, Required, EntityMap, EntityMustExist(typeof(DocumentType))]
        public virtual int DocumentType { get; set; }

        [EntityMap("Document", MapDirections.ToViewModel)]
        public int? DocumentId { get; set; }

        [DoesNotAutoMap("Used by controller to redirect user back from where they came.")]
        public string UrlReferrer { get; set; }

        [DoesNotAutoMap("Display? I don't know")]
        public Document Document
        {
            get { return (DocumentId.HasValue) ? _container.GetInstance<IDocumentRepository>().Find(DocumentId.Value) : null; }
        }

        [DropDown, EntityMap]
        public int? DocumentStatus { get; set; }

        public int? ReviewFrequency { get; set; }
        
        [DropDown, EntityMap, EntityMustExist(typeof(RecurringFrequencyUnit))]
        public int? ReviewFrequencyUnit { get; set; }

        [Required, Secured, DoesNotAutoMap("Mapped manually to DocumentLink")]
        public virtual string TableName { get; set; }

        #endregion

        #region Constructors

        public EditDocumentLink(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        //public override void Map(DocumentLink entity)
        //{
        //    base.Map(entity);
        //    DocumentId = entity.Document.Id;
        //}

        //public override DocumentLink MapToEntity(DocumentLink entity)
        //{
        //    entity = base.MapToEntity(entity);
        //    return entity;
        //}

        #endregion
    }

    public class DeleteDocumentLink : ViewModel<IDocumentLink>
    {
        #region Properties

        [Required, Secured]
        public override int Id { get; set; }

        #endregion

        #region Constructors

        public DeleteDocumentLink(IContainer container) : base(container) {}

        #endregion
    }

    public class SecureSearchDocumentForSingleEntity : SearchSet<DocumentLink>
    {
        #region Public Methods

        [SearchAlias("DataType", "TableName")]
        [Required, Secured]
        public string TableName { get; set; }

        [Required, Secured]
        public int? LinkedId { get; set; }

        // This property is here to persist that a user has edit rights to the original
        // entity. This information is lost when the ajax tab is loaded.
        [Required, Secured, Search(CanMap=false)]
        public bool? UserCanEdit { get; set; }

        #endregion

        #region Public Methods

        public override void ModifyValues(ISearchMapper mapper)
        {
            base.ModifyValues(mapper);

            // The TableName property should normally be a SearchString. However, due to the way
            // the ajax view is created and the way the urls are generated for the pagination links,
            // the EntityName value ends up being serialized to the url as "SearchString" rather than
            // the value itself. To bypass this, we do this little hack of changing the mapped value
            // to a SearchString.
            mapper.MappedProperties[nameof(TableName)].Value = new SearchString
            {
                Value = TableName,
                MatchType = SearchStringMatchType.Exact
            };
        }

        #endregion
    }
}