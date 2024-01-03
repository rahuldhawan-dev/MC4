using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using StructureMap;

namespace MMSINC.Data
{
    public abstract class ViewModelSet : ViewModel, IValidatableObject
    {
        #region Fields

        protected readonly IContainer _container;

        #endregion

        #region Constructors

        public ViewModelSet(IContainer container)
        {
            _container = container;
        }

        #endregion

        #region Abstract Methods

        public abstract IEnumerable<ValidationResult> Validate(ValidationContext validationContext);

        #endregion
    }

    public abstract class ViewModelSet<TEntity> : ViewModelSet, IViewModelSet<TEntity>
        where TEntity : class
    {
        #region Properties

        /// <summary>
        /// Meant to be implemented in inheriting classes and provide a set
        /// of target items mapped from the view model values.
        /// </summary>
        public abstract IEnumerable<TEntity> Items { get; }

        #endregion

        #region Constructors

        public ViewModelSet(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        // TODO: This method is only used by one single view model(RemoveSchedulingEmployeeAssignments)
        // and it's only called by one single place(ActionHelper). The method doesn't need
        // this parameter, as it's only used to pass in itself.

        /// <summary>
        /// Meant to be optionally overridden in inheriting classes to perform
        /// anything else that needs to happen prior to saving.
        /// </summary>
        public virtual void OnSaving(IViewModelSet<TEntity> entity) { }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // TODO: This isn't exactly right, as it's validating the entity instances that are being 
            // created. Entities generally don't have any validator logic. This would make more sense
            // if we were doing ViewModelSet<TViewModel, TEntity>. Which maybe we should do someday.
            var ret = new List<ValidationResult>();

            foreach (var item in Items.OfType<IValidatableObject>())
            {
                ret.AddRange(item.Validate(validationContext));
            }

            return ret;
        }

        #endregion
    }

    public interface IViewModelSet<TEntity>
    {
        IEnumerable<TEntity> Items { get; }
        void OnSaving(IViewModelSet<TEntity> entity);
    }
}
