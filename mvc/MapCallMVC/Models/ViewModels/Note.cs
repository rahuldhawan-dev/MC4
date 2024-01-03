using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;
using DataType = System.ComponentModel.DataAnnotations.DataType;
using DataTypeAttribute = System.ComponentModel.DataAnnotations.DataTypeAttribute;

namespace MapCallMVC.Models.ViewModels
{
    public class NoteModel : ViewModel<Note>
    {
        #region Properties

        [DataType(DataType.MultilineText)]
        public virtual string Text { get; set; }

        #endregion

        #region Constructors

        public NoteModel(IContainer container) : base(container) {}

        #endregion
    }

    public class NewNote : NoteModel
    {
        #region Properties

        [Required, Secured]
        public virtual int? LinkedId { get; set; }
        [Required, Secured, DoesNotAutoMap("Used in MapToEntity.")]
        public virtual string TableName { get; set; }

        #endregion

        #region Constructors

        public NewNote(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override Note MapToEntity(Note entity)
        {
            entity = base.MapToEntity(entity);

            entity.CreatedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.UserName;
            entity.DataType = _container.GetInstance<IDataTypeRepository>().GetByTableName(TableName).First();

            return entity;
        }

        #endregion
    }

    public class EditNote : NoteModel
    {
        [Secured]
        public override int Id { get { return base.Id; } set { base.Id = value; } }

        #region Constructors

        public EditNote(IContainer container) : base(container) {}

        #endregion
    }

    public class DeleteNote : NoteModel
    {
        [Secured]
        public override int Id { get { return base.Id; } set { base.Id = value; } }

        #region Constructors

        public DeleteNote(IContainer container) : base(container) {}

        #endregion
    }

}