using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using ControllerBase = MMSINC.Controllers.ControllerBase;

namespace MMSINC.ClassExtensions
{
    public static class AddDropDownDataControllerExtensions
    {
        #region AddDropDownData<TRepository, TEntity>

        /// <summary>
        /// Gathers data from a repository and converts it to a
        /// IEnumerable SelectListItem  in the controller's ViewData for use in
        /// editor templates.
        /// - In this overload, .GetAll() will be called on the repository to
        /// gather data.
        /// - In this overload, the name of the TEntity class will be used
        /// for the ViewData key.
        /// </summary>
        /// <typeparam name="TRepository">Specific repository type to use when IRepository&lt;TEntity&gt; will not suffice.</typeparam>
        /// <typeparam name="TEntity">Entity type of the data being added.</typeparam>
        /// <param name="keyGetter">Lambda to specify how to get the key (Id usually) from an instance of TEntity.</param>
        /// <param name="valueGetter">Lambda to specify how to get the value (Description or Name usually) from an instance of TEntity.</param>
        /// <returns>The controller that the method was called on, for chaining.</returns>
        public static ControllerBase AddDropDownData<TRepository, TEntity>(this ControllerBase controller,
            Expression<Func<TEntity, object>> keyGetter, Expression<Func<TEntity, object>> valueGetter)
            where TRepository : IRepository<TEntity>
        {
            return controller
               .AddDropDownData<TRepository, TEntity>(
                    typeof(TEntity).Name, keyGetter, valueGetter);
        }

        /// <summary>
        /// Gathers data from a repository and converts it to a
        /// IEnumerable SelectListItem in the controller's ViewData for use in
        /// editor templates.
        /// - In this overload, the name of the TEntity class will be used
        /// for the ViewData key.
        /// </summary>
        /// <typeparam name="TRepository">Specific repository type to use when IRepository&lt;TEntity&gt; will not suffice.</typeparam>
        /// <typeparam name="TEntity">Entity type of the data being added.</typeparam>
        /// <param name="dataGetter">Lambda to specify how to get the requesite data from the repository</param>
        /// <param name="keyGetter">Lambda to specify how to get the key (Id usually) from an instance of TEntity.</param>
        /// <param name="valueGetter">Lambda to specify how to get the value (Description or Name usually) from an instance of TEntity.</param>
        /// <returns>The controller that the method was called on, for chaining.</returns>
        public static ControllerBase AddDropDownData<TRepository, TEntity>(this ControllerBase controller,
            Func<TRepository, IEnumerable<TEntity>> dataGetter, Expression<Func<TEntity, object>> keyGetter,
            Expression<Func<TEntity, object>> valueGetter)
            where TRepository : IRepository<TEntity>
        {
            return controller.AddDropDownData(typeof(TEntity).Name,
                dataGetter, keyGetter, valueGetter);
        }

        public static ControllerBase AddDropDownData<TRepository, TEntity>(this ControllerBase controller, string key,
            Func<TRepository, IEnumerable<TEntity>> dataGetter)
            where TRepository : IRepository<TEntity>
            where TEntity : IEntityLookup
        {
            return controller.AddDropDownData(key,
                dataGetter, e => e.Id, e => e.Description);
        }

        public static ControllerBase AddDropDownData<TRepository, TEntity>(this ControllerBase controller,
            Func<TRepository, IEnumerable<TEntity>> dataGetter)
            where TRepository : IRepository<TEntity>
            where TEntity : IEntityLookup
        {
            return controller.AddDropDownData(typeof(TEntity).Name, dataGetter);
        }

        /// <summary>
        /// Gathers data from a repository and converts it to a
        /// IEnumerable SelectListItem in the controller's ViewData for use in
        /// editor templates.
        /// </summary>
        /// <typeparam name="TRepository">Specific repository type to use when IRepository&lt;TEntity&gt; will not suffice.</typeparam>
        /// <typeparam name="TEntity">Entity type of the data being added.</typeparam>
        /// <param name="key">String value to use for the ViewData key.</param>
        /// <param name="dataGetter">Lambda to specify how to get the requesite data from the repository</param>
        /// <param name="keyGetter">Lambda to specify how to get the key (Id usually) from an instance of TEntity.</param>
        /// <param name="valueGetter">Lambda to specify how to get the value (Description or Name usually) from an instance of TEntity.</param>
        /// <returns>The controller that the method was called on, for chaining.</returns>
        public static ControllerBase AddDropDownData<TRepository, TEntity>(this ControllerBase controller, string key,
            Func<TRepository, IEnumerable<TEntity>> dataGetter, Expression<Func<TEntity, object>> keyGetter,
            Expression<Func<TEntity, object>> valueGetter)
            where TRepository : IRepository<TEntity>
        {
            return controller.AddDropDownData(key,
                dataGetter(controller.Container.GetInstance<TRepository>()), keyGetter,
                valueGetter);
        }

        /// <summary>
        /// Gathers data from a repository and converts it to a
        /// IEnumerable SelectListItem in the controller's ViewData for use in
        /// editor templates.
        /// - In this overload, .GetAll() will be called on the repository to
        /// gather data.
        /// </summary>
        /// <typeparam name="TRepository">Specific repository type to use when IRepository&lt;TEntity&gt; will not suffice.</typeparam>
        /// <typeparam name="TEntity">Entity type of the data being added.</typeparam>
        /// <param name="key">String value to use for the ViewData key.</param>
        /// <param name="keyGetter">Lambda to specify how to get the key (Id usually) from an instance of TEntity.</param>
        /// <param name="valueGetter">Lambda to specify how to get the value (Description or Name usually) from an instance of TEntity.</param>
        /// <returns>The controller that the method was called on, for chaining.</returns>
        public static ControllerBase AddDropDownData<TRepository, TEntity>(this ControllerBase controller, string key,
            Expression<Func<TEntity, object>> keyGetter, Expression<Func<TEntity, object>> valueGetter)
            where TRepository : IRepository<TEntity>
        {
            return controller
               .AddDropDownData<TRepository, TEntity>(key, r => r.Where(_ => true),
                    keyGetter, valueGetter);
        }

        public static ControllerBase AddDropDownData<TRepository, TEntity>(this ControllerBase controller,
            Func<TRepository, IQueryable<TEntity>> dataGetter, Expression<Func<TEntity, object>> keyGetter,
            Expression<Func<TEntity, object>> valueGetter)
        {
            return controller.AddDropDownData(typeof(TEntity).Name, dataGetter, keyGetter, valueGetter);
        }

        public static ControllerBase AddDropDownData<TRepository, TEntity>(this ControllerBase controller, string key,
            Func<TRepository, IQueryable<TEntity>> dataGetter, Expression<Func<TEntity, object>> keyGetter,
            Expression<Func<TEntity, object>> valueGetter)
        {
            var dynamicResult = dataGetter(controller.Container.GetInstance<TRepository>())
               .SelectDynamic(keyGetter, valueGetter);

            return controller.AddDropDownData(key, dynamicResult, keyGetter,
                valueGetter);
        }

        #endregion

        #region AddEnumDropDownData<TEnum>

        public static ControllerBase AddEnumDropDownData<TEnum>(this ControllerBase controller, string key)
        {
            controller.ViewData[key] = SelectListItemConverter.ConvertFromEnumType(typeof(TEnum));
            return controller;
        }

        #endregion

        #region AddDynamicDropDownData

        public static ControllerBase AddDynamicDropDownData<TEntity, TDisplay>(this ControllerBase controller,
            Expression<Func<TDisplay, object>> keyGetter, Expression<Func<TDisplay, object>> valueGetter,
            string key = null, Func<IRepository<TEntity>, IQueryable<TEntity>> dataGetter = null,
            Expression<Func<TEntity, bool>> filter = null)
        {
            return controller.AddDynamicDropDownData<IRepository<TEntity>, TEntity, TDisplay>(keyGetter, valueGetter,
                key, dataGetter, filter);
        }

        public static ControllerBase AddDynamicDropDownData<TEntity, TDisplay>(this ControllerBase controller,
            string key = null, Func<IRepository<TEntity>, IQueryable<TEntity>> dataGetter = null,
            Expression<Func<TEntity, bool>> filter = null)
            where TDisplay : DisplayItem<TEntity>
        {
            return controller.AddDynamicDropDownData<TEntity, TDisplay>(x => x.Id, x => x.Display,
                key, dataGetter, filter);
        }

        public static ControllerBase AddDynamicDropDownData<TRepository, TEntity, TDisplay>(
            this ControllerBase controller, string key = null, Func<TRepository, IQueryable<TEntity>> dataGetter = null,
            Expression<Func<TEntity, bool>> filter = null)
            where TRepository : IRepository<TEntity>
            where TDisplay : DisplayItem<TEntity>
        {
            return controller.AddDynamicDropDownData<TRepository, TEntity, TDisplay>(x => x.Id, x => x.Display,
                key, dataGetter, filter);
        }

        public static ControllerBase AddDynamicDropDownData<TRepository, TEntity, TDisplay>(
            this ControllerBase controller, Expression<Func<TDisplay, object>> keyGetter,
            Expression<Func<TDisplay, object>> valueGetter, string key = null,
            Func<TRepository, IQueryable<TEntity>> dataGetter = null, Expression<Func<TEntity, bool>> filter = null)
            where TRepository : IRepository<TEntity>
        {
            key = key ?? typeof(TEntity).Name;

            if (filter == null)
            {
                dataGetter = dataGetter ?? (r => r.GetAllSorted());
            }
            else
            {
                dataGetter = r => r.GetAllSorted().Where(filter);
            }

            var data = dataGetter(controller.Container.GetInstance<TRepository>()).SelectDynamic<TEntity, TDisplay>();
            return controller.AddDropDownData(key, data, keyGetter, valueGetter);
        }

        #endregion

        #region AddDropDownData<TEntity>

        public static ControllerBase AddDropDownData<TEntity>(this ControllerBase controller)
            where TEntity : IEntityLookup
        {
            return controller.AddDropDownData<TEntity>(r => r.Where(_ => true).OrderBy(x => x.Description), e => e.Id,
                e => e.Description);
        }

        public static ControllerBase AddDropDownData<TEntity>(this ControllerBase controller, string key)
            where TEntity : IEntityLookup
        {
            return controller.AddDropDownData<TEntity>(key, r => r.Where(_ => true).OrderBy(x => x.Description),
                e => e.Id, e => e.Description);
        }

        public static ControllerBase AddDropDownData<TEntity>(this ControllerBase controller,
            Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> keyGetter,
            Expression<Func<TEntity, object>> valueGetter)
        {
            return controller
               .AddDropDownData(typeof(TEntity).Name,
                    r => r.Where(filter), keyGetter, valueGetter);
        }

        /// <summary>
        /// Gathers data from a repository and converts it to a
        /// IEnumerable SelectListItem in the controller's ViewData for use in
        /// editor templates.
        /// - In this overload, IRepository&lt;TEntity&gt; will be used for the
        /// repository type.
        /// </summary>
        /// <typeparam name="TEntity">Entity type of the data being added.</typeparam>
        /// <param name="key">String value to use for the ViewData key.</param>
        /// <param name="dataGetter">Lambda to specify how to get the requesite data from the repository</param>
        /// <param name="keyGetter">Lambda to specify how to get the key (Id usually) from an instance of TEntity.</param>
        /// <param name="valueGetter">Lambda to specify how to get the value (Description or Name usually) from an instance of TEntity.</param>
        /// <returns>The controller that the method was called on, for chaining.</returns>
        public static ControllerBase AddDropDownData<TEntity>(this ControllerBase controller, string key,
            Func<IRepository<TEntity>, IQueryable<TEntity>> dataGetter, Expression<Func<TEntity, object>> keyGetter,
            Expression<Func<TEntity, object>> valueGetter)
        {
            return controller
               .AddDropDownData<IRepository<TEntity>, TEntity>(key, dataGetter,
                    keyGetter, valueGetter);
        }

        public static ControllerBase AddDropDownData<TEntity>(this ControllerBase controller,
            Func<IRepository<TEntity>, IQueryable<TEntity>> dataGetter)
            where TEntity : IEntityLookup
        {
            return controller.AddDropDownData<IRepository<TEntity>, TEntity>(dataGetter, x => x.Id,
                x => x.Description);
        }

        public static ControllerBase AddDropDownData<TEntity, TRepository>(this ControllerBase controller,
            Func<TRepository, IQueryable<TEntity>> dataGetter)
            where TEntity : IEntityLookup
            where TRepository : IRepository<TEntity>
        {
            return controller.AddDropDownData(dataGetter, x => x.Id, x => x.Description);
        }

        /// <summary>
        /// Gathers data from a repository and converts it to a
        /// IEnumerable SelectListItem in the controller's ViewData for use in
        /// editor templates.
        /// - In this overload, IRepository&lt;TEntity&gt; will be used for the
        /// repository type.
        /// - In this overload, the name of the TEntity class will be used
        /// for the ViewData key.
        /// </summary>
        /// <typeparam name="TEntity">Entity type of the data being added.</typeparam>
        /// <param name="dataGetter">Lambda to specify how to get the requesite data from the repository</param>
        /// <param name="keyGetter">Lambda to specify how to get the key (Id usually) from an instance of TEntity.</param>
        /// <param name="valueGetter">Lambda to specify how to get the value (Description or Name usually) from an instance of TEntity.</param>
        /// <returns>The controller that the method was called on, for chaining.</returns>
        public static ControllerBase AddDropDownData<TEntity>(this ControllerBase controller,
            Func<IRepository<TEntity>, IQueryable<TEntity>> dataGetter, Expression<Func<TEntity, object>> keyGetter,
            Expression<Func<TEntity, object>> valueGetter)
        {
            return controller.AddDropDownData<TEntity>(typeof(TEntity).Name,
                dataGetter, keyGetter, valueGetter);
        }

        /// <summary>
        /// Gathers data from a repository and converts it to a
        /// IEnumerable SelectListItem in the controller's ViewData for use in
        /// editor templates.
        /// - In this overload, IRepository&lt;TEntity&gt; will be used for the
        /// repository type.
        /// - In this overload, .GetAll() will be called on the repository to
        /// gather data.
        /// </summary>
        /// <typeparam name="TEntity">Entity type of the data being added.</typeparam>
        /// <param name="key">String value to use for the ViewData key.</param>
        /// <param name="keyGetter">Lambda to specify how to get the key (Id usually) from an instance of TEntity.</param>
        /// <param name="valueGetter">Lambda to specify how to get the value (Description or Name usually) from an instance of TEntity.</param>
        /// <returns>The controller that the method was called on, for chaining.</returns>
        public static ControllerBase AddDropDownData<TEntity>(this ControllerBase controller, string key,
            Expression<Func<TEntity, object>> keyGetter, Expression<Func<TEntity, object>> valueGetter)
        {
            return controller.AddDropDownData(key, r => r.Where(_ => true), keyGetter,
                valueGetter);
        }

        /// <summary>
        /// Gathers data from a repository and converts it to a
        /// IEnumerable SelectListItem in the controller's ViewData for use in
        /// editor templates.
        /// - In this overload, IRepository&lt;TEntity&gt; will be used for the
        /// repository type.
        /// - In this overload, .GetAll() will be called on the repository to
        /// gather data.
        /// - In this overload, the name of the TEntity class will be used
        /// for the ViewData key.
        /// </summary>
        /// <typeparam name="TEntity">Entity type of the data being added.</typeparam>
        /// <param name="keyGetter">Lambda to specify how to get the key (Id usually) from an instance of TEntity.</param>
        /// <param name="valueGetter">Lambda to specify how to get the value (Description or Name usually) from an instance of TEntity.</param>
        /// <returns>The controller that the method was called on, for chaining.</returns>
        public static ControllerBase AddDropDownData<TEntity>(this ControllerBase controller,
            Expression<Func<TEntity, object>> keyGetter, Expression<Func<TEntity, object>> valueGetter)
        {
            return controller.AddDropDownData(typeof(TEntity).Name,
                keyGetter, valueGetter);
        }

        /// <summary>
        /// Converts the given data to a IEnumerable SelectListItem in the
        /// controller's ViewData for use in editor templates.
        /// </summary>
        /// <typeparam name="TEntity">Entity type of the data being added.</typeparam>
        /// <param name="key">String value to use for the ViewData key.</param>
        /// <param name="data">Data set to be used as SelectListItems.</param>
        /// <param name="keyGetter">Lambda to specify how to get the key (Id usually) from an instance of TEntity.</param>
        /// <param name="valueGetter">Lambda to specify how to get the value (Description or Name usually) from an instance of TEntity.</param>
        /// <returns>The controller that the method was called on, for chaining.</returns>
        public static ControllerBase AddDropDownData<TEntity>(this ControllerBase controller, string key,
            IEnumerable<TEntity> data, Expression<Func<TEntity, object>> keyGetter,
            Expression<Func<TEntity, object>> valueGetter)
        {
            controller.ViewData[key] = SelectListItemConverter.Convert(data, keyGetter.Compile(), valueGetter.Compile());
            return controller;
        }

        private static ControllerBase AddDropDownData<TEntity>(this ControllerBase controller, string key,
            IQueryableExtensions.IQueryableExtensions.DynamicSelectResult result,
            Expression<Func<TEntity, object>> keyGetter,
            Expression<Func<TEntity, object>> valueGetter)
        {
            var keyName = keyGetter.GetMemberExpression().Member.Name;
            var valueName = valueGetter.GetMemberExpression().Member.Name;

            controller.ViewData[key] = SelectListItemConverter.Convert(result.Result, keyName, valueName);
            return controller;
        }

        /// <summary>
        /// Converts the given data to a IEnumerable SelectListItem in the
        /// controller's ViewData for use in editor templates.
        /// - In this overload, the name of the TEntity class will be used
        /// for the ViewData key.
        /// </summary>
        /// <typeparam name="TEntity">Entity type of the data being added.</typeparam>
        /// <param name="data">Data set to be used as SelectListItems.</param>
        /// <param name="keyGetter">Lambda to specify how to get the key (Id usually) from an instance of TEntity.</param>
        /// <param name="valueGetter">Lambda to specify how to get the value (Description or Name usually) from an instance of TEntity.</param>
        /// <returns>The controller that the method was called on, for chaining.</returns>
        public static ControllerBase AddDropDownData<TEntity>(this ControllerBase controller, IEnumerable<TEntity> data,
            Expression<Func<TEntity, object>> keyGetter, Expression<Func<TEntity, object>> valueGetter)
        {
            return controller.AddDropDownData(typeof(TEntity).Name, data,
                keyGetter, valueGetter);
        }

        #endregion
    }
}
