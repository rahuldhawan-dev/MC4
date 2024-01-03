using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.ProjectManagement.Models.ViewModels
{
    public class RecurringProjectEndorsementViewModel : ViewModel<RecurringProjectEndorsement>
    {
        #region Constructors

        public RecurringProjectEndorsementViewModel(IContainer container) : base(container) { }

        #endregion

        #region Projects

        [Required, DropDown, EntityMap, EntityMustExist(typeof(EndorsementStatus))]
        public virtual int? EndorsementStatus { get; set; }

        [Multiline]
        public virtual string Comment { get; set; }

        #endregion
    }

    public class CreateRecurringProjectEndorsement : RecurringProjectEndorsementViewModel
    {
        #region Constructors

        public CreateRecurringProjectEndorsement(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override RecurringProjectEndorsement MapToEntity(RecurringProjectEndorsement entity)
        {
            base.MapToEntity(entity);

            var now = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            entity.EndorsementDate = now;

            return entity;
        }

        #endregion
    }

    public class EditRecurringProjectEndorsement : RecurringProjectEndorsementViewModel
    {
        #region Constructors

        public EditRecurringProjectEndorsement(IContainer container) : base(container) {}

        #endregion

        #region Display Only

        private RecurringProject _displayRecurringProject;

        [DoesNotAutoMap("Display only")]
        public RecurringProject DisplayRecurringProject
        {
            get
            {
                if (_displayRecurringProject == null)
                    _displayRecurringProject = _container.GetInstance<IRecurringProjectRepository>().Find(RecurringProject.GetValueOrDefault());
                return _displayRecurringProject;
            }
        }

        private RecurringProjectEndorsement _displayRecurringProjectEndorsement;

        [DoesNotAutoMap("Display only")]
        public RecurringProjectEndorsement DisplayRecurringProjectEndorsement
        {
            get
            {
                if (_displayRecurringProjectEndorsement == null)
                    _displayRecurringProjectEndorsement =
                        _container.GetInstance<IRepository<RecurringProjectEndorsement>>()
                            .Find(Id);
                return _displayRecurringProjectEndorsement;
            }
        }

        #endregion

        [Required, EntityMustExist(typeof(RecurringProject)), EntityMap]
        public virtual int? RecurringProject { get; set; }
    }
}
