using System;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.ClassExtensions.TypeExtensions;
using StructureMap;

namespace MMSINC.Data
{
    public class ViewModelFactory : IViewModelFactory
    {
        protected readonly IContainer _container;

        public ViewModelFactory(IContainer container)
        {
            _container = container;
        }

        // There's no reason for this to exist over the Action<TViewModel> version.
        // This has no compile-time safety and doesn't offer any benefit.
        protected virtual void ApplyOverrides<TViewModel>(object overrides, TViewModel model)
            where TViewModel : ViewModel
        {
            foreach (var item in overrides.GetType().GetPublicPropertiesAndValues(overrides))
            {
                model.SetPropertyValueByName(item.Property.Name, item.Value);
            }
        }

        public TViewModel Build<TViewModel>()
            where TViewModel : ViewModel
        {
            return _container.GetInstance<TViewModel>();
        }

        public TViewModel Build<TViewModel, TEntity>(TEntity entity)
            where TViewModel : ViewModel<TEntity>
            where TEntity : class
        {
            var model = _container.GetInstance<TViewModel>();
            if (entity != null)
            {
                model.Map(entity);
            }

            return model;
        }

        public TViewModel BuildWithOverrides<TViewModel>(object overrides)
            where TViewModel : ViewModel
        {
            var model = Build<TViewModel>();

            ApplyOverrides(overrides, model);

            return model;
        }

        public TViewModel BuildWithOverrides<TViewModel, TEntity>(TEntity entity, object overrides)
            where TViewModel : ViewModel<TEntity>
            where TEntity : class
        {
            var model = Build<TViewModel, TEntity>(entity);

            ApplyOverrides(overrides, model);

            return model;
        }

        public TViewModel BuildWithOverrides<TViewModel>(Action<TViewModel> overrides) where TViewModel : ViewModel
        {
            var model = Build<TViewModel>();

            overrides(model);

            return model;
        }

        public TViewModel BuildWithOverrides<TViewModel, TEntity>(TEntity entity, Action<TViewModel> overrides) where TViewModel : ViewModel<TEntity> where TEntity : class
        {
            var model = Build<TViewModel, TEntity>(entity);
            overrides(model);
            return model;
        }
    }
}
