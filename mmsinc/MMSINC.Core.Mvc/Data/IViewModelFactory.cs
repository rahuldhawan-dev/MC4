using System;

namespace MMSINC.Data
{
    /// <summary>
    /// View model factories create view models with the correct dependency injection.
    /// </summary>
    public interface IViewModelFactory
    {
        /// <summary>
        /// Creates a new instance of a view model without setting any property values.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <returns></returns>
        TViewModel Build<TViewModel>() where TViewModel : ViewModel;

        /// <summary>
        /// Creates a new instance of a view model using an entity reference to
        /// set the initial property values.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        TViewModel Build<TViewModel, TEntity>(TEntity entity)
            where TViewModel : ViewModel<TEntity>
            where TEntity : class;

        [Obsolete("Do not use. Use the overload that takes a typed Action.")]
        TViewModel BuildWithOverrides<TViewModel>(object overrides)
            where TViewModel : ViewModel;
        
        [Obsolete("Do not use. Use the overload that takes a typed Action.")]
        TViewModel BuildWithOverrides<TViewModel, TEntity>(TEntity entity, object overrides)
            where TViewModel : ViewModel<TEntity>
            where TEntity : class;

        /// <summary>
        /// Creates a new view model instance then applies the overrides to it.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="overrides"></param>
        /// <returns></returns>
        TViewModel BuildWithOverrides<TViewModel>(Action<TViewModel> overrides)
            where TViewModel : ViewModel;
        
        /// <summary>
        /// Creates a new view model instance, applying the entity instance's values
        /// to the view model properties, followed by the overrides.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="overrides"></param>
        /// <returns></returns>
        TViewModel BuildWithOverrides<TViewModel, TEntity>(TEntity entity, Action<TViewModel> overrides)
            where TViewModel : ViewModel<TEntity>
            where TEntity : class;
    }
}
