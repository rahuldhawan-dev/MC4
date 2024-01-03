using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using JetBrains.Annotations;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.ClassExtensions.MemberInfoExtensions;
using MMSINC.Metadata;
using MMSINC.Results;
using MMSINC.Utilities.Pdf;
using MMSINC.Validation;
using NHibernate.Exceptions;
using StructureMap;

// ReSharper disable Mvc.ActionNotResolved

namespace MMSINC.Utilities
{
    // TODO: None of these methods are correctly setting http status codes when there are validation errors.
    //       We should fix that for correctness.

    /// <inheritdoc />
    public class
        ActionHelper<TController, TRepository, TEntity, TUser> : IActionHelper<TController, TRepository, TEntity, TUser>
        where TController : ControllerBaseWithPersistence<TRepository, TEntity, TUser>
        where TRepository : class, Data.NHibernate.IRepository<TEntity>
        where TUser : IAdministratedUser
        where TEntity : class
    {
        #region Constants

        public const string NO_RESULTS = "No results matched your query.", SQL_ERROR = "A SQL Error Occurred.";

        #endregion

        #region Fields

        private readonly string _viewPathFormat;
        private readonly IContainer _container;

        #endregion

        #region Properties

        public TController Controller { get; protected set; }

        public TRepository Repository
        {
            get { return Controller.Repository; }
        }

        #endregion

        #region Constructors

        public ActionHelper(IContainer container, TController controller)
        {
            _container = container;
            Controller = controller;

            // Don't use typeof(TController) here, it does not return the 
            // same etype as the controller's actual type. 
            var formatAttr = MemberInfoExtensions
                            .GetCustomAttributes<ActionHelperViewVirtualPathFormatAttribute>(Controller.GetType(), true)
                            .SingleOrDefault();
            if (formatAttr != null)
            {
                _viewPathFormat = formatAttr.VirtualPathFormat;
            }
        }

        #endregion

        #region Private Methods

        private string GetViewName(string viewName)
        {
            if (viewName.StartsWith("~") || _viewPathFormat == null)
            {
                return viewName;
            }

            return string.Format(_viewPathFormat, viewName);
        }

        private ActionResult DoView(string viewName, object model)
        {
            var correctView = GetViewName(viewName);
            if (viewName.StartsWith("_"))
            {
                return Controller.DoPartialView(correctView, model);
            }

            return Controller.DoView(correctView, model);
        }

        protected string NotFound(int id)
        {
            return String.Format("{0} with id {1} was not found.",
                new RouteContext(Controller.ControllerContext).GetDisplayControllerName(), id);
        }

        private static readonly Type _entityMustExistType = typeof(EntityMustExistAttribute),
                                     _selectAttrType = typeof(SelectAttribute),
                                     _entityMapAttrType = typeof(EntityMapAttribute);

        private static IEnumerable<PropertyInfo> GetAutomaticallyPopulatableProperties(Type modelType)
        {
            foreach (var prop in modelType.GetProperties())
            {
                var select = (SelectAttribute)prop.GetCustomAttributes(_selectAttrType, true).FirstOrDefault();
                var entMap = prop.GetCustomAttributes(_entityMapAttrType, true).FirstOrDefault();

                // Do not include cascades because otherwise they'll end up doing GetAll.
                if (entMap != null && select != null && !select.IsCascading)
                {
                    yield return prop;
                }
            }
        }

        /// <summary>
        /// Executes each func until a result is returned. 
        /// </summary>
        /// <param name="resultGetters"></param>
        /// <returns></returns>
        private static ActionResult GetFirstNonNullResult(params Func<ActionResult>[] resultGetters)
        {
            foreach (var getter in resultGetters)
            {
                // Passing a null getter to this method is valid as the args
                // might not be set.
                var result = getter?.Invoke();
                if (result != null)
                {
                    return result;
                }
            }

            throw new InvalidOperationException("None of the ActionResult funcs returned a value.");
        }

        /// <summary>
        /// This method was split from the original non-generic
        /// <see cref="DoShow(int,MMSINC.Utilities.ActionHelperDoShowArgs,System.Action{TEntity})"/> so that a
        /// new generic version of <see cref="ActionHelperDoShowArgs{TEntity}"/> with a generic implementation
        /// of <see cref="ActionHelperDoShowArgs{TEntity}.OnSuccess"/> rather than simply using
        /// <see cref="System.Object"/> and usually forcing implementors to cast.  It was done this way
        /// specifically so that generic arguments wouldn't suddenly become necessary for who knows how many
        /// existing DoShow calls which already pass arguments.
        /// </summary>
        private ActionResult InnerDoShow(
            int id,
            ActionHelperDoShowArgs args,
            Action<TEntity> onModelFound,
            Func<TEntity, ActionResult> onSuccess = null)
        {
            var hasExplicitViewName = args.ViewName != null;
            if (!hasExplicitViewName)
            {
                args.ViewName = args.IsPartial ? "_Show" : "Show";
            }

            var model = args.GetEntityOverride != null
                ? (TEntity)args.GetEntityOverride()
                : Repository.Find(id);
            
            if (model == null)
            {
                return GetFirstNonNullResult(args.OnNotFound,
                    () => Controller.DoHttpNotFound(args.NotFound ?? NotFound(id)));
            }

            onModelFound?.Invoke(model);

            var result = onSuccess?.Invoke(model);

            if (result != null)
            {
                return result;
            }

            Controller.SetLookupData(ControllerAction.Show);
            return Controller.DoView(GetViewName(args.ViewName), model, args.IsPartial);
        }

        #endregion

        #region Exposed Methods

        #region DoCreate

        public ActionResult DoCreate<TModel>(TModel model, ActionHelperDoCreateArgs args = null)
            where TModel : ViewModel<TEntity>
        {
            args = args ?? new ActionHelperDoCreateArgs();
            // I'm not a huge fan of using the container to get a new entity instance, 
            // but it's needed for rare occasions where the entity has some form of
            // dependency injection.
            var entity = (TEntity)args.Entity ?? _container.GetInstance<TEntity>();

            if (!Controller.ModelState.IsValid)
            {
                Controller.DisplayModelStateErrors();
                Controller.SetLookupData(ControllerAction.New);

                // Do not redirect to the New action! It will result in the fields all being unpopulated. -Ross 1/22/2015
                return GetFirstNonNullResult(args.OnError, () => DoView(args.OnErrorView ?? "New", model));
            }

            model.MapToEntity(entity);
            Repository.Save(entity);
            // I don't know that there's a valid purpose in re-mapping the values
            // to a view model. The view model should be of no use after this, and
            // the caller should have access to the entity instance if it needs
            // values. PersistenceService was doing this
            model.Map(entity);
            return GetFirstNonNullResult(args.OnSuccess,
                () => Controller.DoRedirectionToAction("Show", new { model.Id }));
        }

        public ActionResult DoCreateForViewModelSet<TModel>(TModel model, ActionHelperDoCreateArgs args = null)
            where TModel : ViewModelSet<TEntity>
        {
            args = args ?? new ActionHelperDoCreateArgs();

            if (!Controller.ModelState.IsValid)
            {
                Controller.DisplayModelStateErrors();
                Controller.SetLookupData(ControllerAction.New);

                // Do not redirect to the New action! It will result in the fields all being unpopulated. -Ross 1/22/2015
                return GetFirstNonNullResult(args.OnError, () => DoView(args.OnErrorView ?? "New", model));
            }

            model.OnSaving(model);
            Repository.Save(model.Items);

            // TODO some day maybe: This is going to be clunky for the end user. Sending them to the search results
            // page is the closest thing we can do to redirecting them to the Show view like we do for a single record.
            // The clunky part being that they can't be guaranteed to see the results of what they just created. There's
            // always a race condition that could occur if two people hit save at the same time.
            // 
            // The potential solutions for this have flaws.
            // 1. Redirect with a querystring that would search specifically for the records they just created. This
            //    would potentially break due to the length limit of query strings. The business will find a way to
            //    break that, no doubt.
            // 2. Store the search arguments in TempData instead. That has the flaw of using TempData when we're trying
            //    to get away from it. This data would also disappear if they refreshed the page since it wouldn't end up
            //    reflected in the querystring.
            // 
            // Neither of those solutions solve the other potential(but HOPEFULLY VERY RARE occurance) problem with
            // NHibernate's query parameter limit which tops out at 2100.

            return GetFirstNonNullResult(args.OnSuccess, () => Controller.DoRedirectionToAction("Search", null));
        }

        #endregion

        #region DoUpdate

        public ActionResult DoUpdate<TModel>(TModel model, ActionHelperDoUpdateArgs args = null)
            where TModel : ViewModel<TEntity>
        {
            args = args ?? new ActionHelperDoUpdateArgs();

            TEntity entity = null;

            if (args.GetEntityOverride != null)
            {
                entity = (TEntity)args.GetEntityOverride();
            }
            else
            {
                entity = Repository.Find(model.Id);
            }

            if (entity == null)
            {
                return GetFirstNonNullResult(args.OnNotFound,
                    () => Controller.DoHttpNotFound(args.NotFound ?? NotFound(model.Id)));
            }

            if (!Controller.ModelState.IsValid)
            {
                Controller.DisplayModelStateErrors();
                Controller.SetLookupData(ControllerAction.Edit);
                return GetFirstNonNullResult(args.OnError, () => DoView(args.OnErrorView ?? "Edit", model));
            }

            model.MapToEntity(entity);
            Repository.Save(entity);
            // I don't know that there's a valid purpose in re-mapping the values
            // to a view model. The view model should be of no use after this, and
            // the caller should have access to the entity instance if it needs
            // values.
            model.Map(entity);

            // TODO: We should make this consistent with the OnError/OnNotFound stuff.
            return GetFirstNonNullResult(args.OnSuccess,
                () => Controller.DoRedirectionToAction("Show", new {model.Id}));
        }

        public ActionResult DoUpdateForViewModelSet<TModel>(TModel model, ActionHelperDoUpdateArgs args = null) 
            where TModel : ViewModelSet<TEntity>
        {
            args = args ?? new ActionHelperDoUpdateArgs();

            if (!Controller.ModelState.IsValid)
            {
                Controller.DisplayModelStateErrors();
                Controller.SetLookupData(ControllerAction.Edit);

                return GetFirstNonNullResult(args.OnError, () => DoView(args.OnErrorView ?? "Edit", model));
            }

            model.OnSaving(model);
            Repository.Save(model.Items);

            // Refer to comment in DoCreateForViewModelSet() regarding the clunky UX of redirecting to Search by default
            return GetFirstNonNullResult(args.OnSuccess, () => Controller.DoRedirectionToAction("Search", null));
        }

        #endregion

        #region DoIndex

        // TODO: We should have one single DoIndex method that does all of the work
        // then have the existing overloads deal with how they're retrieving the data.

        public ActionResult DoIndex(bool partial = false)
        {
            var entities = Repository.GetAll();
            return DoIndexWithResults(entities, partial);
        }

        /// <summary>
        /// Returns an index view with the supplied results. This is only useful if you have a page
        /// without a search that just displays all the items and those items needed to be sorted.
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public ActionResult DoIndexWithResults(IEnumerable<TEntity> results, bool partial = false)
        {
            // NOTE: This is called DoIndexWithResults because calling it DoIndex causes
            // ambiguous call errors otherwise.

            return Controller.DoView(GetViewName("Index"), results, partial);
        }

        /// <summary>
        /// Does what DoIndex does but for ISearchSet models.
        /// </summary>
        /// <param name="searchModel">
        /// Search model object whose attributes will be used as route values for rendering or redirection.
        /// </param>
        /// <param name="args">
        /// Additional arguments for how the index should be presented.
        /// </param>
        /// <returns></returns>
        public ActionResult DoIndex<TModel>(ISearchSet<TModel> searchModel, ActionHelperDoIndexArgs args = null)
        {
            args = args ?? new ActionHelperDoIndexArgs();
            args.ViewName =
                args.ViewName ?? "Index"; // Need to set the defaults if the args were passed in but ViewName not set.

            var finalValues = Controller.ModelState.ToRouteValueDictionary().Merge(args.RouteValues ?? new { });

            if (!Controller.ModelState.IsValid)
            {
                Controller.DisplayModelStateErrors();
                return Controller.DoRedirectionToAction("Search", finalValues);
            }

            if (args.SearchOverrideCallback != null)
            {
                args.SearchOverrideCallback();
            }
            else
            {
                Repository.Search(searchModel);
            }

            var count = searchModel.Count;
            if (count == 0)
            {
                if (args.OnNoResults == null)
                {
                    Controller.DisplayErrorMessage(NO_RESULTS);
                    return Controller.DoRedirectionToAction("Search", finalValues);
                }

                return args.OnNoResults();
            }

            // TODO: Not really a fan of this show redirect since it makes it impossible for a user
            // to get to an excel or map action if their search only has a single result. 
            if (count == 1 && args.RedirectSingleItemToShowView)
            {
                var result = searchModel.Results.Single();
                return Controller.DoRedirectionToAction("Show", finalValues.Merge(new {id = ((dynamic)result).Id}));
            }

            if (args.MaxResults.HasValue && count > args.MaxResults)
            {
                Controller.DisplayErrorMessage(
                    $"The query you have entered will bring back more than {args.MaxResults} results. Please refine your search.");
                return Controller.DoRedirectionToAction("Search", finalValues);
            }

            return GetFirstNonNullResult(args.OnSuccess,
                () => Controller.DoView(GetViewName(args.ViewName), searchModel, args.IsPartial));
        }

        #endregion

        #region DoExcel

        // TODO: Eventually merge this and DoIndexForSearchSet into a single method that returns 
        // a ResponseFormatterResult. 
        public ActionResult DoExcel<TModel>(ISearchSet<TModel> searchModel, ActionHelperDoIndexArgs args = null)
        {
            // NOTE: For now we still want DoExcel to redirect to the search view when there are validation errors.
            // We may want to change that at some point so that DoExcel always returns an Excel result.
            args = args ?? new ActionHelperDoIndexArgs();

            var finalValues = Controller.ModelState.ToRouteValueDictionary().Merge(args.RouteValues ?? new { });

            if (!Controller.ModelState.IsValid)
            {
                Controller.DisplayModelStateErrors();
                return Controller.DoRedirectionToAction("Search", finalValues);
            }

            // We don't do paging for excel, we export the whole thing.
            searchModel.EnablePaging = false;

            if (args.SearchOverrideCallback != null)
            {
                args.SearchOverrideCallback();
            }
            else
            {
                // Repository.Search ends up enumerating the IEnumerable before returning it, thus running
                // the query.  so everything below about the count is useless unless we pass in the max
                // here and prevent that from happening
                Repository.Search(searchModel, null, maxResults: args.MaxResults);
            }

            var count = searchModel.Count;

            if (args.MaxResults.HasValue && count > args.MaxResults)
            {
                Controller.DisplayErrorMessage(
                    $"The query you have entered will bring back more than {args.MaxResults} results. Please refine your search.");
                return Controller.DoRedirectionToAction("Search", finalValues);
            }

            return GetFirstNonNullResult(args.OnSuccess, () =>
                new ExcelResult {Autofit = args.AutofitForExcel}.AddSheet(searchModel.Results,
                    new Excel.ExcelExportSheetArgs {
                        ExportableProperties = searchModel.ExportableProperties
                    }));
        }

        #endregion

        #region DoShow

        public ActionResult DoShow(
            int id,
            ActionHelperDoShowArgs args = null,
            Action<TEntity> onModelFound = null)
        {
            return InnerDoShow(id, args ?? new ActionHelperDoShowArgs(), onModelFound);
        }

        public ActionResult DoShow(
            int id,
            ActionHelperDoShowArgs<TEntity> args,
            Action<TEntity> onModelFound = null)
        {
            return InnerDoShow(id, args ?? new ActionHelperDoShowArgs<TEntity>(), onModelFound, args?.OnSuccess);
        }

        #endregion

        #region DoPdf

        public ActionResult DoPdf(int id, string viewName = "Pdf")
        {
            var model = Repository.Find(id);

            if (model == null)
            {
                return Controller.DoHttpNotFound(NotFound(id));
            }

            return new PdfResult(_container.GetInstance<IHtmlToPdfConverter>(), GetViewName(viewName), model);
        }

        #endregion

        #region DoSearch

        public ActionResult DoSearch<TModel>(TModel model = null, string viewName = "Search")
            where TModel : class, ISearchSet
        {
            // Views don't *need* a model instance, but we might as well be consistent
            // with all the other areas where we always send a model to the view.
            model = model ?? _container.GetInstance<TModel>();
            Controller.SetLookupData(ControllerAction.Search);
            return Controller.DoView(viewName, model);
        }

        #endregion

        #region DoNew

        public ActionResult DoNew<TModel>(ActionHelperDoNewArgs args = null) where TModel : ViewModel<TEntity>
        {
            var model = _container.GetInstance<TModel>();
            return DoNew(model, args);
        }

        public ActionResult DoNew<TModel>(TModel model, ActionHelperDoNewArgs args = null)
            where TModel : ViewModel<TEntity>
        {
            args = args ?? new ActionHelperDoNewArgs();
            var hasExplicitlyNamedView = args.ViewName != null;

            if (!hasExplicitlyNamedView)
            {
                args.ViewName = args.IsPartial ? "_New" : "New";
            }

            Controller.SetLookupData(ControllerAction.New);
            model.SetDefaults();

            var properViewName = GetViewName(args.ViewName);
            return Controller.DoView(properViewName, model, args.IsPartial);
        }
        
        public ActionResult DoNewForViewModelSet<TModel>(TModel model, ActionHelperDoNewArgs args = null)
            where TModel : ViewModelSet<TEntity>
        {
            args = args ?? new ActionHelperDoNewArgs();
            var hasExplicitlyNamedView = args.ViewName != null;

            if (!hasExplicitlyNamedView)
            {
                args.ViewName = args.IsPartial ? "_New" : "New";
            }

            Controller.SetLookupData(ControllerAction.New);

            var properViewName = GetViewName(args.ViewName);
            return Controller.DoView(properViewName, model, args.IsPartial);
        }

        #endregion

        #region DoEdit

        // I'm leaving the model parameter out of this for the time being unless I find an instance where it's used.
        // TODO: Figure out if there's a way to get onModelFound into the DoEditArgs without making the args generic.
        public ActionResult DoEdit<TModel>(int id, ActionHelperDoEditArgs<TEntity, TModel> args = null,
            Action<TEntity> onModelFound = null) where TModel : ViewModel<TEntity>
        {
            args = args ?? new ActionHelperDoEditArgs<TEntity, TModel>();

            var hasExplicitViewName = args.ViewName != null;
            if (!hasExplicitViewName)
            {
                args.ViewName = args.IsPartial ? "_Edit" : "Edit";
            }

            var entity = args.GetEntityOverride != null ? (TEntity)args.GetEntityOverride() : Repository.Find(id);
            if (entity == null)
            {
                return GetFirstNonNullResult(args.OnNotFound,
                    () => Controller.DoHttpNotFound(args.NotFound ?? NotFound(id)));
            }

            // At the time of writing(4/10/2015), the model parameter was never used by ActionHelper. 
            // So, I'm continuing to do that by not using the model.
            var model = _container.With((TEntity)null).GetInstance<TModel>();
            args.InitializeViewModel?.Invoke(model);
            model.SetDefaults();
            model.Map(entity);
            onModelFound?.Invoke(entity);
            if (!args.SkipLookupData)
            {
                Controller.SetLookupData(ControllerAction.Edit);
            }

            return Controller.DoView(GetViewName(args.ViewName), model, args.IsPartial);
        }

        #endregion

        #region DoDestroy

        //Internal so that this can be tested, sqlite does not generate same messages
        internal string GetSqlError(GenericADOException e)
        {
            var pattern =
                @"The DELETE statement conflicted with the REFERENCE constraint ""(.+)""\. The conflict occurred in database ""(.+)"", table ""(.+)"", column '(.+)'\.(?:\r\n|\r|\n)The statement has been terminated\.";
            var match = Regex.Match(e.InnerException?.Message ?? e.Message, pattern);
            return match.Groups.Count == 5
                ? $"This record is linked to the {match.Groups[3].Value} table and cannot be deleted as a result. Column: {match.Groups[4].Value}, Constraint {match.Groups[1].Value}"
                : "A SQL Error Occurred.";
        }

        public ActionResult DoDestroy(int id, ActionHelperDoDestroyArgs args = null)
        {
            args = args ?? new ActionHelperDoDestroyArgs();

            var entity = Repository.Find(id);

            if (entity == null)
            {
                return Controller.DoHttpNotFound(args.NotFound ?? NotFound(id));
            }

            // This is here on the off chance the Destroy method ever has any validation.
            // Our Destroy methods always just have an int parameter which doesn't require
            // any particular validation by itself.
            if (!Controller.ModelState.IsValid)
            {
                Controller.DisplayModelStateErrors();
                return GetFirstNonNullResult(args.OnError, () => Controller.DoRedirectionToAction("Show", new {id}));
            }

            try
            {
                Repository.Delete(entity);
            }
            catch (GenericADOException e)
            {
                Controller.DisplayErrorMessage(GetSqlError(e));
                return Controller.DoRedirectionToAction("Show", new {id});
            }

            return GetFirstNonNullResult(args.OnSuccess, () => {
                if (args.OnSuccessRedirectAction == null)
                {
                    var route = new RouteContext(Controller.ControllerContext);
                    var searchAction = route.ControllerDescriptor.FindReflectedActionDescriptor("Search");
                    args.OnSuccessRedirectAction = (searchAction != null ? "Search" : "Index");
                }

                return Controller.DoRedirectionToAction(args.OnSuccessRedirectAction,
                    args.OnSuccessRedirectRouteValues);
            });
        }

        #endregion

        #endregion
    }

    public interface IActionHelper<TController, TRepository, TEntity, TUser>
        where TController : ControllerBaseWithPersistence<TRepository, TEntity, TUser>
        where TRepository : class, Data.NHibernate.IRepository<TEntity>
        where TUser : IAdministratedUser
        where TEntity : class
    {
        /// <summary>
        /// Reference to the <typeparamref name="TController"/> instance whose actions this helper is helping.
        /// </summary>
        TController Controller { get; }
        TRepository Repository { get; }

        /// <summary>
        /// Convert <paramref name="model"/> to a <typeparamref name="TEntity"/> and persist it.
        /// </summary>
        /// <returns>
        /// An <see cref="ActionResult"/> which will normally redirect the user to the Show page for the
        /// newly-persisted <typeparamref name="TEntity"/> on success, or back to the New page on error or
        /// validation failure.
        /// </returns>
        ActionResult DoCreate<TModel>(TModel model, ActionHelperDoCreateArgs args = null)
            where TModel : ViewModel<TEntity>;
        
        ActionResult DoCreateForViewModelSet<TModel>(TModel model, ActionHelperDoCreateArgs args = null)
            where TModel : ViewModelSet<TEntity>;

        /// <summary>
        /// Lookup the <typeparamref name="TEntity"/> instance represented by <paramref name="model"/>, map
        /// any altered values to the <typeparamref name="TEntity"/>, and persist it.
        /// </summary>
        /// <returns>
        /// An <see cref="ActionResult"/> which will normally redirect the user to the Show page for the
        /// newly-updated <typeparamref name="TEntity"/> on success, or back to the Edit page on error or
        /// validation failure.
        /// </returns>
        ActionResult DoUpdate<TModel>(TModel model, ActionHelperDoUpdateArgs args = null)
            where TModel : ViewModel<TEntity>;

        ActionResult DoUpdateForViewModelSet<TModel>(TModel model, ActionHelperDoUpdateArgs args = null)
            where TModel : ViewModelSet<TEntity>;

        ActionResult DoIndex(bool partial = false);
        ActionResult DoIndex<TModel>(ISearchSet<TModel> searchModel, ActionHelperDoIndexArgs args = null);
        ActionResult DoIndexWithResults(IEnumerable<TEntity> results, bool partial = false);
        ActionResult DoExcel<TModel>(ISearchSet<TModel> searchModel, ActionHelperDoIndexArgs args = null);

        /// <summary>
        /// Lookup the <typeparamref name="TEntity"/> instance identified by <paramref name="id"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="ActionResult"/> which will normally display/render the
        /// <typeparamref name="TEntity"/> instance if found, or an <see cref="HttpNotFoundResult"/> if it
        /// wasn't.
        /// </returns>
        ActionResult DoShow(int id, ActionHelperDoShowArgs args = null, Action<TEntity> onModelFound = null);

        /// <inheritdoc cref="DoShow(int,MMSINC.Utilities.ActionHelperDoShowArgs,System.Action{TEntity})" />
        /// <remarks>
        /// This overload uses the generic implementation of <see cref="ActionHelperDoShowArgs{TEntity}"/>,
        /// useful for performing typed callbacks such as
        /// <see cref="ActionHelperDoShowArgs{TEntity}.OnSuccess"/>.
        /// </remarks>
        ActionResult DoShow(int id, ActionHelperDoShowArgs<TEntity> args, Action<TEntity> onModelFound = null);

        /// <summary>
        /// Does all of the default New action stuff with the supplied arguments. A new instance
        /// of the view model is created for the supplied TModel type.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        ActionResult DoNew<TModel>(ActionHelperDoNewArgs args = null) where TModel : ViewModel<TEntity>;

        /// <summary>
        /// Does all of the default New action stuff with the supplied model and arguments.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        ActionResult DoNew<TModel>(TModel model, ActionHelperDoNewArgs args = null) where TModel : ViewModel<TEntity>;
        ActionResult DoNewForViewModelSet<TModel>(TModel model, ActionHelperDoNewArgs args = null)
            where TModel : ViewModelSet<TEntity>;

        ActionResult DoEdit<TModel>(int id, ActionHelperDoEditArgs<TEntity, TModel> args = null,
            Action<TEntity> onModelFound = null) where TModel : ViewModel<TEntity>;

        ActionResult DoDestroy(int id, ActionHelperDoDestroyArgs args = null);
        ActionResult DoPdf(int id, [AspMvcView] string viewName = "Pdf");

        ActionResult DoSearch<TModel>(TModel model = null, [AspMvcView] string viewName = "Search")
            where TModel : class, ISearchSet;
    }

    public abstract class ActionHelperArgs
    {
        // DO NOT add properties to this if you aren't going to implement their usage
        // in every model/method that uses the model.
    }

    /// <inheritdoc />
    public class ActionHelperDoShowArgs : ActionHelperArgs
    {
        #region Properties

        /// <summary>
        /// Set to true if a partial view should be returned. False by default.
        /// </summary>
        public bool IsPartial { get; set; }

        /// <summary>
        /// The name of the view returned. "Show" by default.
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        /// Gets/sets the message displayed when a record can not be found.
        /// </summary>
        public string NotFound { get; set; }

        /// <summary>
        /// Func that allows overriding the ActionResult returned when no record is found.
        /// Return null if ActionHelper should return the default error ActionResult, otherwise you can override 
        /// this by returning your own ActionResult from this method.
        /// </summary>
        public Func<ActionResult> OnNotFound { get; set; }

        /// <summary>
        /// Use this if you need to override how the entity is retrieved. 
        /// </summary>
        public Func<object> GetEntityOverride { get; set; }

        #endregion
    }

    /// <inheritdoc />
    public class ActionHelperDoShowArgs<TEntity> : ActionHelperDoShowArgs
    {
        /// <summary>
        /// Callback to override the default behavior of setting lookup data on the
        /// <see cref="ActionHelper{TController,TRepository,TEntity,TUser}.Controller"/> and rendering the found entity
        /// when an entity was successfully found.  If the callback returns a null <see cref="ActionResult"/>, the
        /// aforementioned default behavior will still be performed.
        /// </summary>
        public Func<TEntity, ActionResult> OnSuccess { get; set; }
    }

    public class ActionHelperDoNewArgs : ActionHelperArgs
    {
        #region Properties

        /// <summary>
        /// Set to true if a partial view should be returned. False by default.
        /// </summary>
        public bool IsPartial { get; set; }

        /// <summary>
        /// The name of the view returned. "New" by default.
        /// </summary>
        public string ViewName { get; set; }

        #endregion
    }

    public class ActionHelperDoIndexArgs : ActionHelperArgs
    {
        #region Properties

        /// <summary>
        /// Set to true if the ExcelResult should have its Autofit property set to true. True by default.
        /// </summary>
        public bool AutofitForExcel { get; set; } = true;

        /// <summary>
        /// Set to true if the resulting view should be rendered as a partial view. Defaults to false.
        /// </summary>
        public bool IsPartial { get; set; }

        /// <summary>
        /// If true, the search will redirect to the record's show page if there is only
        /// one result for a search. Defaults to false.
        /// </summary>
        public bool RedirectSingleItemToShowView { get; set; }

        /// <summary>
        /// Additional route values to be used during redirects.
        /// </summary>
        public object RouteValues { get; set; }

        /// <summary>
        /// Gets an optional search callback override. Setting this stops Repository.Search from being called. 
        /// You must handle this yourself by either calling Repository.Search from inside your custom method
        /// or otherwise manually setting the Count and other paging related properties.
        /// </summary>
        public Action SearchOverrideCallback { get; set; }

        /// <summary>
        /// Optional name of the view to use when rendering the index view. 
        /// </summary>
        [AspMvcView]
        public string ViewName { get; set; }

        public Func<ActionResult> OnNoResults { get; set; }

        /// <summary>
        /// Optional function to run when the search results are returned. 
        /// </summary>
        public Func<ActionResult> OnSuccess { get; set; }

        /// <summary>
        /// Optional maximum number of results to display, otherwise ActionHelper will redirect back to the search.
        /// </summary>
        public int? MaxResults { get; set; }

        #endregion
    }

    public class ActionHelperDoEditArgs<TEntity, TModel> : ActionHelperArgs
        where TEntity : class
        where TModel : ViewModel<TEntity>
    {
        /// <summary>
        /// Func that allows overriding the ActionResult returned when no record is found.
        /// Return null if ActionHelper should return the default error ActionResult, otherwise you can override 
        /// this by returning your own ActionResult from this method.
        /// </summary>
        public Func<ActionResult> OnNotFound { get; set; }

        /// <summary>
        /// Gets/sets the message displayed when a record can not be found.
        /// </summary>
        public string NotFound { get; set; }

        /// <summary>
        /// If true, SetLookupData is not called. Defaults to false.
        /// </summary>
        public bool SkipLookupData { get; set; }

        /// <summary>
        /// Set to true if a partial view should be returned. False by default.
        /// </summary>
        public bool IsPartial { get; set; }

        /// <summary>
        /// The name of the view returned. "Edit" by default.
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        /// Use this if you need to override how the entity is retrieved. 
        /// </summary>
        public Func<TEntity> GetEntityOverride { get; set; }

        // <summary>
        // Use this to pass in the view model and set values to it when you have weird situations that require it
        // </summary>
        public Action<TModel> InitializeViewModel { get; set; }
    }

    public class ActionHelperDoUpdateArgs : ActionHelperArgs
    {
        #region Properties

        public Func<ActionResult> OnSuccess { get; set; }

        /// <summary>
        /// Func that allows for extra processing if an error occurs. Return null if ActionHelper should 
        /// return the default error ActionResult, otherwise you can override this by returning your own
        /// ActionResult from this method.
        /// </summary>
        public Func<ActionResult> OnError { get; set; }

        /// <summary>
        /// Func that allows overriding the ActionResult returned when no record is found.
        /// Return null if ActionHelper should return the default error ActionResult, otherwise you can override 
        /// this by returning your own ActionResult from this method.
        /// </summary>
        public Func<ActionResult> OnNotFound { get; set; }

        /// <summary>
        /// Gets/sets the view to be returned if there is an error. "_ViewName" will return a partial view.
        /// </summary>
        public string OnErrorView { get; set; }

        /// <summary>
        /// Gets/sets the message displayed when a record can not be found.
        /// </summary>
        public string NotFound { get; set; }

        /// <summary>
        /// Use this if you need to override how the entity is retrieved. 
        /// </summary>
        public Func<object> GetEntityOverride { get; set; }

        #endregion
    }

    public class ActionHelperDoCreateArgs : ActionHelperArgs
    {
        #region Properties

        /// <summary>
        /// The entity object used for creation. If this is null, then one will be created automatically.
        /// </summary>
        public object Entity { get; set; }

        public Func<ActionResult> OnSuccess { get; set; }

        /// <summary>
        /// Func that allows for extra processing if an error occurs. Return null if ActionHelper should 
        /// return the default error ActionResult, otherwise you can override this by returning your own
        /// ActionResult from this method.
        /// </summary>
        public Func<ActionResult> OnError { get; set; }

        /// <summary>
        /// Gets/sets the view to be returned if there is an error. "_ViewName" will return a partial view.
        /// </summary>
        public string OnErrorView { get; set; }

        #endregion
    }

    public class ActionHelperDoDestroyArgs : ActionHelperArgs
    {
        #region Properties

        /// <summary>
        /// Func that allows overriding the ActionResult returned when no record is found.
        /// Return null if ActionHelper should return the default error ActionResult, otherwise you can override 
        /// this by returning your own ActionResult from this method.
        /// </summary>
        public Func<ActionResult> OnNotFound { get; set; }

        /// <summary>
        /// Gets/sets the message displayed when a record can not be found.
        /// </summary>
        public string NotFound { get; set; }

        /// <summary>
        /// Set to true if a partial view should be returned. False by default.
        /// </summary>
        public bool IsPartial { get; set; }

        /// <summary>
        /// The action to redirect a user to after they delete a record. If null, it will go to Search or Index.
        /// </summary>
        public string OnSuccessRedirectAction { get; set; }

        /// <summary>
        /// Route values to be used for the redirect action after a successful delete.
        /// </summary>
        public object OnSuccessRedirectRouteValues { get; set; }

        /// <summary>
        /// Func that allows overriding the returned ActionResult when there's a successful delete.
        /// If you need to do extra processing but still want to return the default ActionHelper result
        /// then return null.
        /// </summary>
        public Func<ActionResult> OnSuccess { get; set; }

        /// <summary>
        /// Func that allows for extra processing if an error occurs. Return null if ActionHelper should 
        /// return the default error ActionResult, otherwise you can override this by returning your own
        /// ActionResult from this method.
        /// </summary>
        public Func<ActionResult> OnError { get; set; }

        #endregion
    }
}
