using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using IMeterChangeOutRepository = Contractors.Data.Models.Repositories.IMeterChangeOutRepository;

namespace Contractors.Models.ViewModels
{
    public class NoteModel : ViewModel<Note>
    {
        #region Properties

        [Multiline]
        public virtual string Text { get; set; }

        #endregion

        #region Constructors

        public NoteModel(IContainer container) : base(container) { }

        #endregion
    }

    public class NewNote : NoteModel
    {
        #region Properties

        [Required]
        public virtual int? LinkedId { get; set; }
        [Required, StringLengthNotRequired, DoesNotAutoMap]
        public virtual string TableName { get; set; }

        #endregion

        #region Constructors

        public NewNote(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override Note MapToEntity(Note entity)
        {
            entity = base.MapToEntity(entity);

            entity.CreatedBy = _container.GetInstance<IAuthenticationService<ContractorUser>>().CurrentUser.Email;
            entity.DataType = _container.GetInstance<IDataTypeRepository>().GetByTableName(TableName).First();

            return entity;
        }

        #endregion
    }

    public class EditNote : NoteModel
    {
        #region Constructors

        public EditNote(IContainer container) : base(container) { }

        #endregion

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var original = _container.GetInstance<IRepository<Note>>().Find(Id);

            if (original != null)
            {
                if (original.DataType.TableName != "MeterChangeOuts")
                {
                    yield return new ValidationResult("Unable to edit notes that are not for meter change outs.");
                }
                else
                {
                    var repo = _container.GetInstance<IMeterChangeOutRepository>();
                    var originalMCO = repo.Find(original.LinkedId);

                    if (originalMCO == null)
                    {
                        yield return new ValidationResult("Unable to edit a note that does not exist or otherwise is unavailable.");
                    }
                }
            }
        }
    }

    public class DeleteNote : NoteModel
    {
        #region Constructors

        public DeleteNote(IContainer container) : base(container) { }

        #endregion

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var original = _container.GetInstance<IRepository<Note>>().Find(Id);

            if (original != null)
            {
                if (original.DataType.TableName != "MeterChangeOuts")
                {
                    yield return new ValidationResult("Unable to edit notes that are not for meter change outs.");
                }
                else
                {
                    var originalMCO = _container.GetInstance<IMeterChangeOutRepository>().Find(original.LinkedId);

                    if (originalMCO == null)
                    {
                        yield return new ValidationResult("Unable to edit a note that does not exist or otherwise is unavailable.");
                    }
                }
            }
        }
    }
}