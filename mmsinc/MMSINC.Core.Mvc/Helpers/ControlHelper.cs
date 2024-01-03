using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using JetBrains.Annotations;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Validation;
using StructureMap;

namespace MMSINC.Helpers
{
    // TODO: Make this easy to write extension methods for. At the moment it's being written in a way
    //       that's ensuring all the basic functionality comes from within the class.

    /// <summary>
    /// Common non-generic functionality shared with the generic ControlHelper class.
    /// </summary>
    public abstract class ControlHelper
    {
        #region Consts

        public const string DEFAULT_EMPTY_TEXT_FOR_DROPDOWNS = "-- Select --";

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns a PropertyInfo object if the property could be used for automatically populating
        /// a select UI component with items. Returns null if the property can not be used that way.
        /// </summary>
        /// <param name="modelType"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        protected static PropertyInfo GetAutomaticallyPopulatablePropertyInfo(Type modelType, string propertyName)
        {
            var prop = modelType.GetProperty(propertyName);
            if (prop == null)
            {
                throw new InvalidOperationException($"Unable to find property {propertyName} on type {modelType.AssemblyQualifiedName}");
            }

            var select = (SelectAttribute)prop.GetCustomAttributes(typeof(SelectAttribute), true).FirstOrDefault();
            var entMap = prop.GetCustomAttributes(typeof(EntityMapAttribute), true).FirstOrDefault();

            // Do not include cascades because otherwise they'll end up doing GetAll.
            if (entMap != null && select != null && !select.IsCascading)
            {
                return prop;
            }

            return null;
        }

        #endregion
    }

    /// <remarks>
    /// 
    /// If you're adding a set of methods for a control, you need
    /// to have the three types of methods below(or some variation).
    /// 
    /// - TControlBuilder TControl() 
    ///     A method that returns an uninitialized instance of TControlBuilder. 
    ///     This way we can create a control that does not have the automated
    ///     id, name, value, and any other prepopulated values set. 
    /// 
    /// If a control can be used in model binding/validation, you must
    /// include the below two methods for consistency.
    /// 
    /// - TControlBuilder TControl(string expression)
    ///     A method that returns an instance of TControlBuilder that is
    ///     initialized with id, name, value, unobtrusive validation attributes
    ///     and anything else that can be automated.
    /// 
    /// - TControlBuilder TControl(Expression expression)
    ///     Same as above but with an Expression instance instead.
    /// 
    /// </remarks>
    /// <typeparam name="T">The @model type declared by the view. This does not guarantee that it will
    /// be the actual type for the model instance being used. If no @model type is declared, T will be type Object.</typeparam>
    public class ControlHelper<T> : ControlHelper
    {
        // These are for the extension method-esque thingies.

        #region Fields

        private readonly ViewContext _viewContext;
        private readonly ViewDataDictionary<T> _viewData;
        private readonly RouteCollection _routeCollection;
        private readonly IContainer _container;
        private static readonly Type _entityMustExistType = typeof(EntityMustExistAttribute);

        #endregion

        #region Constructor

        public ControlHelper(ViewContext viewContext, ViewDataDictionary<T> viewData, RouteCollection routeCollection,
            IContainer container)
        {
            _viewContext = viewContext;
            _viewData = viewData;
            _routeCollection = routeCollection;
            _container = container;
        }

        #endregion

        #region Private Methods

        #region GetModelMetadata

        private ModelMetadata GetModelMetadata(string expression)
        {
            return _viewData.ModelMetadata.TryFromStringExpression(expression);
        }

        private ModelMetadata GetModelMetadata<TResult>(Expression<Func<T, TResult>> expression)
        {
            // We can use the MVC version for lambda expressions because the method's smart
            // enough to get the correct type information from null models.
            return ModelMetadata.FromLambdaExpression(expression, _viewData);
        }

        #endregion

        /// <returns>
        /// This acts the same as HtmlHelper.GetUnobtrusiveValidationAttributes except it completely
        /// ignores the FormContext checking part. This allows controls outside of forms to have their
        /// unobtrusive attributes rendered. Also it stops the thing where rendering the same control
        /// twice ends up with the second control not having any of the attributes. 
        /// </returns>
        private IDictionary<string, object> GetUnobtrusiveValidationAttributes(ModelMetadata metadata)
        {
            var results = new Dictionary<string, object>();

            // The ordering of these 3 checks (and the early exits) is for performance reasons.
            if (!_viewContext.UnobtrusiveJavaScriptEnabled || metadata == null)
            {
                return results;
            }

            var clientRules =
                ModelValidatorProviders.Providers.GetValidators(metadata, _viewContext)
                                       .SelectMany(v => v.GetClientValidationRules());

            UnobtrusiveValidationAttributesGenerator.GetValidationAttributes(clientRules, results);

            return results;
        }

        /// <summary>
        /// Returns the model value that should be used for rendering.
        /// </summary>
        internal object GetModelValue(IBuilderData cache, Type type)
        {
            // In order for this to work similarly to MVC's default stuff, we need to do
            // two things:

            // 1. First check the ModelState's value. This is the value as it's been posted
            //    to the server. That's the value we'd want to send back to the client if
            //    there's any validation issues. This is needed primarily for times where
            //    someone might enter "oaushdgdhs" into an integer field. The Model value
            //    itself would end up being null or 0, but the ModelState would have the gibberish.

            ModelState state;
            if (_viewData.ModelState.TryGetValue(cache.FullExpression, out state))
            {
                if (state.Value != null)
                {
                    return state.Value.ConvertTo(type, null);
                }
            }

            // 2. If there's no ModelState value, return the actual model value. This is
            //    what would be used for newly-loaded pages with inputs(like first load of 
            //    an edit view).

            var meta = cache.ModelMetadata;
            return (meta != null ? meta.Model : null);
        }

        private IEnumerable<object> GetModelValueAsObjectArray(IBuilderData cache)
        {
            var modelValue = GetModelValue(cache, typeof(object));
            if (modelValue != null)
            {
                if (modelValue is string)
                {
                    return new[] {modelValue};
                }

                if (modelValue is IEnumerable)
                {
                    return ((IEnumerable)modelValue).Cast<object>();
                }
                else
                {
                    return new[] {modelValue};
                }
            }

            return Enumerable.Empty<object>();
        }

        private IEnumerable<SelectListItem> GetSelectListItems(IBuilderData cache, SelectAttribute selectAttr)
        {
            // TODO: Remove the need for ControllerViewDataKey. Maybe.

            if (selectAttr != null && selectAttr.IsCascading)
            {
#if DEBUG
                // If SetLookupData is calling AddDropDownData for a cascading dropdown, that data goes entirely
                // unused and is a waste of a query. If this error is being thrown, you should remove the call from
                // SetLookupData. If this is happening for some other reason, let me know. -Ross 11/16/2017
                if (_viewData.ContainsKey(cache.ModelMetadata.PropertyName))
                {
                    throw new Exception(
                        $"A cascading dropdown has been rendered for '{cache.ModelMetadata.PropertyName}', but SetLookupData has set data for it already. Remove the call in SetLookupData since the data is unused.");
                }
#endif
                return GetCascadingSelectListItems(cache, selectAttr, cache.ModelMetadata.Container);
            }

            var fullExpression = (selectAttr != null && selectAttr.ControllerViewDataKey != null
                ? selectAttr.ControllerViewDataKey
                : cache.FullExpression);

            // If the expression doesn't exist in ViewData, that means nothing was manually added
            // by the controller during the SetLookupData step. We don't want to overwrite anything
            // that's manually populated as there might be additional filtering on the data being added.
            //
            // NOTE: We don't want to *store/cache* anything in ViewData from inside ControlHelper as that
            // can have unforeseen side effects on child views that might be looking for the same key
            // for a completely different model property. ViewData instances are always copied to
            // the child view.
            //
            // Ex: Let's say you're on a Show page that has an "Foo" dropdown on the main Show.cshtml
            // view for some reason, and you have a second tab on that view that renders a partial with another
            // "Foo" dropdown. The Foo data added to the ViewData of Show.cshtml would then be copied to the
            // partial. If that Foo data needs to be different between the two, though, then that would 
            // cause issues with the wrong data populating. 
            object rawItems = null;
            if (_viewData.ContainsKey(fullExpression))
            {
                rawItems = _viewData[fullExpression];
            }
            else if (cache.ModelMetadata?.ContainerType != null)
            {
                var autoPopulatableProperty = GetAutomaticallyPopulatablePropertyInfo(cache.ModelMetadata.ContainerType, cache.ModelMetadata.PropertyName);
                if (autoPopulatableProperty != null)
                {
                    var eme =
                        ((IEnumerable<EntityMustExistAttribute>)autoPopulatableProperty.GetCustomAttributes(_entityMustExistType, false))
                       .SingleOrDefault();

                    if (eme == null)
                    {
                        // Only get inherited attributes if there isn't one on the inheriting type.
                        eme = ((IEnumerable<EntityMustExistAttribute>)autoPopulatableProperty.GetCustomAttributes(_entityMustExistType, true))
                           .SingleOrDefault();

                        if (eme == null)
                        {
                            throw new InvalidOperationException(
                                "Can not automatically add dropdown items because the property does not have an EntityMustExistAttribute. " +
                                autoPopulatableProperty.DeclaringType.FullName + "." + autoPopulatableProperty.Name);
                        }
                    }

                    var displayItemService = _container.GetInstance<IDisplayItemService>();
                    var displayItemType = displayItemService.DisplayItemType(eme.EntityType);
                    dynamic repo = _container.GetInstance(eme.RepositoryType);

                    if (displayItemType != null)
                    {
                        dynamic result = (IQueryableExtensions.DynamicSelectResult)typeof(IQueryableExtensions)
                           .GetMethods()
                           .Single(m =>
                                m.Name == "SelectDynamic" &&
                                m.IsGenericMethod &&
                                m.GetGenericArguments().Length == 2)
                           .MakeGenericMethod(eme.EntityType, displayItemType)
                           .Invoke(null, new[] { repo.GetAllSorted() });
                        rawItems = Enumerable.ToList(result.Result);
                    }
                    else
                    {
                        // TODO: This should be changed to use GetForSelect if there's a GetForSelect method on the repo. -Ross 11/20/2017
                        rawItems = Enumerable.ToList(repo.GetAllSorted());
                    }
                }
            }

            if (rawItems != null)
            {
                if (rawItems is IEnumerable<SelectListItem>)
                {
                    return (IEnumerable<SelectListItem>)rawItems;
                }

                if (rawItems is IEnumerable<IEntityLookup>)
                {
                    // TODO: TEST THIS!!!!
                    var rawEnumerable = (IEnumerable<IEntityLookup>)rawItems;
                    return rawEnumerable.Select(x => new SelectListItem {Text = x.Description, Value = x.Id.ToString()})
                                        .OrderBy(x => x.Text).ToList();
                }

                if (rawItems is IEnumerable<dynamic>)
                {
                    var rawDynamo = (IEnumerable<dynamic>)rawItems;
                    return rawDynamo.Select(x => new SelectListItem {Text = x.ToString(), Value = x.Id.ToString()})
                                    .OrderBy(x => x.Text).ToList();
                }

                throw new NotSupportedException("Unable to use '" + rawItems.GetType().FullName +
                                                "' with SelectListBuilders.");
            }

            /* NOTE: Only have this throw if you're trying to debug something related to a dropdown
             *       coming up with empty items. This isn't supposed to throw because we're returning
             *       the builder and Items might get added after the fact.
             */
            return Enumerable.Empty<SelectListItem>();
        }

        #region GetFormattedModelValue

        internal string GetFormattedModelValue(IBuilderData cache)
        {
            var value = GetModelValue(cache, typeof(string));
            if (cache.ModelMetadata != null)
            {
                var format = cache.ModelMetadata.EditFormatString;

                if (!string.IsNullOrEmpty(format))
                {
                    return string.Format(System.Globalization.CultureInfo.CurrentCulture, format, value);
                }
            }

            return Convert.ToString(value);
        }

        #endregion

        #region InitializeControlBuilder

        /// <summary>
        /// Initializes a control builder with common html attributes(id, name) and model values. 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="metadata"></param>
        /// <param name="includeValidationAttributesAndCss">Set to false if the unobtrusve validation attributes and css classes should not be applied.</param>
        private BuilderData<TBuilder> InitializeControlBuilderCache<TBuilder>(string expression, ModelMetadata metadata,
            bool includeValidationAttributesAndCss) where TBuilder : ControlBuilder<TBuilder>, new()
        {
            var builder = new TBuilder();

            var fullExpression = _viewData.TemplateInfo.GetFullHtmlFieldName(expression);
            var cache = new BuilderData<TBuilder>(builder, expression, fullExpression, metadata);

            builder.HtmlAttributes["name"] = cache.FullExpression;
            builder.HtmlAttributes["id"] = _viewData.TemplateInfo.GetFullHtmlFieldId(expression);

            if (includeValidationAttributesAndCss)
            {
                foreach (var kv in GetUnobtrusiveValidationAttributes(cache.ModelMetadata))
                {
                    builder.HtmlAttributes.Add(kv);
                }

                // Add the validation error css class if needed.
                ModelState state;
                if (_viewData.ModelState.TryGetValue(cache.FullExpression, out state))
                {
                    if (state.Errors.Any())
                    {
                        builder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
                    }
                }
            }

            return cache;
        }

        /// <summary>
        /// Initializes a control builder with common html attributes(id, name) and model values. 
        /// </summary>
        /// <typeparam name="TBuilder"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <param name="includeValidationAttributesAndCss">Set to false if the unobtrusve validation attributes and css classes should not be applied.</param>
        internal BuilderData<TBuilder> InitializeControlBuilder<TBuilder, TResult>(
            Expression<Func<T, TResult>> expression, bool includeValidationAttributesAndCss = true)
            where TBuilder : ControlBuilder<TBuilder>, new()
        {
            var expressionText = ExpressionHelper.GetExpressionText(expression);
            var metadata = GetModelMetadata(expression);
            return InitializeControlBuilderCache<TBuilder>(expressionText, metadata, includeValidationAttributesAndCss);
        }

        /// <summary>
        /// Initializes a control builder with common html attributes(id, name) and model values. 
        /// </summary>
        /// <typeparam name="TBuilder"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <param name="includeValidationAttributesAndCss">Set to false if the unobtrusve validation attributes and css classes should not be applied.</param>
        internal BuilderData<TBuilder> InitializeControlBuilder<TBuilder>(string expression,
            bool includeValidationAttributesAndCss = true)
            where TBuilder : ControlBuilder<TBuilder>, new()
        {
            var expressionText = ExpressionHelper.GetExpressionText(expression);
            var metadata = GetModelMetadata(expression);
            return InitializeControlBuilderCache<TBuilder>(expressionText, metadata, includeValidationAttributesAndCss);
        }

        #endregion

        #region CreateTextBoxBuilder

        private TextBoxBuilder CreateTextBoxBuilder(string expression, TextBoxType type)
        {
            var tb = InitializeControlBuilder<TextBoxBuilder>(expression);
            tb.Builder.Value = GetFormattedModelValue(tb);
            tb.Builder.Type = type;
            return tb.Builder;
        }

        private TextBoxBuilder CreateTextBoxBuilder<TResult>(Expression<Func<T, TResult>> expression, TextBoxType type)
        {
            var tb = InitializeControlBuilder<TextBoxBuilder, TResult>(expression);
            tb.Builder.Value = GetFormattedModelValue(tb);
            tb.Builder.Type = type;
            return tb.Builder;
        }

        #endregion

        #region CreateButtonBuilder

        private ButtonBuilder CreateButtonBuilder(string expression, ButtonType type)
        {
            var tb = InitializeControlBuilder<ButtonBuilder>(expression, includeValidationAttributesAndCss: false);
            var val = GetFormattedModelValue(tb);
            tb.Builder.Value = val;
            tb.Builder.Text = val; // Set Text to the same to give it a default value.
            tb.Builder.Type = type;
            return tb.Builder;
        }

        private ButtonBuilder CreateButtonBuilder<TResult>(Expression<Func<T, TResult>> expression, ButtonType type)
        {
            var tb = InitializeControlBuilder<ButtonBuilder, TResult>(expression,
                includeValidationAttributesAndCss: false);
            var val = GetFormattedModelValue(tb);
            tb.Builder.Value = val;
            tb.Builder.Text = val; // Set Text to the same to give it a default value.
            tb.Builder.Type = type;
            return tb.Builder;
        }

        #endregion

        #region CreateCheckBoxBuilder

        private CheckBoxBuilder CreateCheckBoxBuilder(BuilderData<CheckBoxBuilder> cache)
        {
            var builder = cache.Builder;
            // The model binder doesn't know how to deal with "on" as a value for a bool.
            // MVC's Html.CheckBox always adds "true" as a value so the correct value is
            // posted back to the server.
            builder.Value = "true";
            builder.Checked = Convert.ToBoolean(GetModelValue(cache, typeof(bool)));
            return cache.Builder;
        }

        #endregion

        #region CreateCheckBoxListBuilder

        private CheckBoxListBuilder CreateCheckBoxListBuilder(BuilderData<CheckBoxListBuilder> cache)
        {
            var builder = cache.Builder;
            builder.WithSelectedValues(GetModelValueAsObjectArray(cache));

            var selectAttr = (cache.ModelMetadata != null
                ? SelectAttribute.GetFromModelMetadata(cache.ModelMetadata)
                : null);
            builder.WithItems(GetSelectListItems(cache, selectAttr));

            if (selectAttr != null && selectAttr.IsCascading)
            {
                builder.With(GetCascadingHtmlAttributes(cache, selectAttr));
            }

            return cache.Builder;
        }

        #endregion

        #region CreateHiddenInputBuilder

        private HiddenInputBuilder CreateHiddenInputBuilder(BuilderData<HiddenInputBuilder> cache)
        {
            var builder = cache.Builder;
            builder.Value = GetFormattedModelValue(cache);
            return cache.Builder;
        }

        #endregion

        #region CreateSelectListBuilder

        private SelectListBuilder CreateSelectListBuilder(BuilderData<SelectListBuilder> cache, SelectListType type)
        {
            var builder = cache.Builder;
            builder.Type = type;
            builder.WithSelectedValues(GetModelValueAsObjectArray(cache));

            var selectAttr = (cache.ModelMetadata != null
                ? SelectAttribute.GetFromModelMetadata(cache.ModelMetadata)
                : null);
            builder.WithItems(GetSelectListItems(cache, selectAttr));
            var isMultiSelect = (selectAttr is MultiSelectAttribute);

            if (selectAttr != null)
            {
                if (selectAttr.IsCascading)
                {
                    builder.With(GetCascadingHtmlAttributes(cache, selectAttr));

                    // Detergent will mess up rendering the disabled dropdown
                    // when no items but the EmptyText item exist. So remove
                    // the empty text if there aren't any items.
                    // TODO: Fix this in detergent(have it disable if there's only a single empty value).
                    if (!builder.Items.Any())
                    {
                        builder.WithEmptyText(null);
                    }
                }
                else if (selectAttr.DefaultItemLabel != null && !isMultiSelect)
                {
                    builder.WithEmptyText(selectAttr.DefaultItemLabel);
                }
            }
            else
            {
                builder.WithEmptyText(DEFAULT_EMPTY_TEXT_FOR_DROPDOWNS);
            }

            return cache.Builder;
        }

        private BuilderData<SelectListBuilder> CreateSelectListBuilder(string expression, SelectListType type)
        {
            var cache = InitializeControlBuilder<SelectListBuilder>(expression);
            CreateSelectListBuilder(cache, type);
            return cache;
        }

        private BuilderData<SelectListBuilder> CreateSelectListBuilder<TResult>(Expression<Func<T, TResult>> expression,
            SelectListType type)
        {
            var cache = InitializeControlBuilder<SelectListBuilder, TResult>(expression);
            CreateSelectListBuilder(cache, type);
            return cache;
        }

        #endregion

        #region Cascading-related methods

        private static List<object> GetCascadingParentValues(object parentContainer, string dependsOn)
        {
            // NOTE: This still needs to return a list with X amount of parameters even if all the values are null.
            var values = new List<object>();
            var props = dependsOn.Split(',');
            foreach (var p in props)
            {
                values.Add(parentContainer.GetPropertyValueByName(p.Trim()));
            }

            return values;
        }

        private IEnumerable<SelectListItem> GetCascadingSelectListItems(IBuilderData cache, SelectAttribute cascade, object containerModel)
        {
            var routeContext = new RouteContext(_viewContext.RequestContext, cascade.Controller, cascade.Action,
                cascade.Area);
            if (!routeContext.IsAuthorized())
            {
                var routeValues = routeContext.RouteData.Values.Aggregate(new List<string>(), (current, next) => {
                    current.Add($"{next.Key}: {next.Value}");
                    return current;
                }, l => string.Join(", ", l));

                throw new InvalidOperationException($"Unauthorized! {routeValues}");
            }

            // You shouldn't need to do the Container trick ever unless you have a nested model type. See MapCallMVC/Contact.
            var container = (_viewData.ContainsKey("Container") ? _viewData["Container"] : containerModel);

            // Container can be null. The GetPropertyValueByName extension returns null if the object
            // is null(rather than throwing an exception). 
            var parentValues = GetCascadingParentValues(container, cascade.DependsOn);

            if ((cascade.DependentsRequired == DependentRequirement.All &&
                 parentValues.Any(x => x == null)) || // All values must be non-null if DependentRequirement == All
                parentValues.All(x => x == null) && cascade.DependentsRequired == DependentRequirement.One
            ) // or, only one value must be non-null if DependentRequirement == One. If DependentRequirement == None, null values are fine.
            {
                // Our DependsOn has no value, so we can't prepopulate the values.
                // Also we can't set the selected value since we can't verify that
                // it's still valid.
                return Enumerable.Empty<SelectListItem>();
            }

            // TODO: This shouldn't use the Controller instance from the ControllerContext. Implementation says
            //       it won't necessarily be there.
            var controller = routeContext.ControllerContext.Controller;

            try
            {
                var includeEmptyText = !(cascade is MultiSelectAttribute || cascade is CheckBoxListAttribute);
                var result =
                    GetCascadingActionResult(routeContext.ActionDescriptor.MethodInfo, controller, parentValues);
                var k = result.GetSelectListItems(includeEmptyText);
                cache.CascadeDataIsPrerendered = true;
                return k;
            }
            finally
            {
                // Make sure controller gets disposed. 
                // System.Web.Mvc.Controller is IDisposable, but IController is not.
                if (controller is IDisposable)
                {
                    ((IDisposable)controller).Dispose();
                }
            }
        }

        private static CascadingActionResult GetCascadingActionResult(MethodInfo actionMethod,
            ControllerBase controller, List<object> parentValues)
        {
            // NOTE: The parentValues must be in the same order as the parameters or else things will go wrong!

            var actionParams = actionMethod.GetParameters();
            if (actionParams.Length != parentValues.Count)
            {
                throw new InvalidOperationException(
                    "The cascading action method has a different number of parameters than the number of parent values being depended on.");
            }

            var parsedValues = new List<object>(parentValues.Count);

            for (var i = 0; i < actionParams.Count(); i++)
            {
                var p = actionParams[i];
                var value = parentValues[i];
                if (p.ParameterType.IsArray && value is int)
                {
                    parsedValues.Add(new[] {(int)value});
                }
                else
                {
                    parsedValues.Add(value);
                }
            }

            return (CascadingActionResult)actionMethod.Invoke(controller, parsedValues.ToArray());
        }

        /// <summary>
        /// Returns an action's parameter name to be used by cascading and autocomplete controls.
        /// </summary>
        private IEnumerable<string> GetActionParameterNames(string action, string controller, string area)
        {
            // TODO: Can probably cache the result of this method in a ConcurrentDictionary.
            var rc = new RouteContext(_viewContext.RequestContext, controller, action, area);

            var actionParams = rc.ActionDescriptor.GetParameters();
            if (!actionParams.Any())
            {
                throw new InvalidOperationException("Action must have a parameter.");
            }

            return actionParams.Select(x => x.ParameterName);
        }

        private string GenerateUrl(string action, string controller, string area)
        {
            var urlHelper = new SecureUrlHelper(_viewContext.RequestContext, _routeCollection);
            var routeData = new RouteValueDictionary();

            // Area needs to be set regardless of whether or not the area param is null. This lets url generation
            // work correctly when linking between different areas.
            routeData.Add("area", area);

            return urlHelper.Action(action, controller, routeData);
        }

        private IDictionary<string, object> GetCascadingHtmlAttributes(IBuilderData cache, SelectAttribute cascade)
        {
            // TODO: It may make more sense to make a CascadeOptions object that can be attached to 
            //       ListBuilder and then let ListBuilder handle generating the html attributes.

            var htmlAttributes = new Dictionary<string, object>();

            Action<string, string> addOptionalHtmlAttribute = (value, attrName) => {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    htmlAttributes[attrName] = value;
                }
            };

            // This needs to be the lowercased "true". If the value is set to bool true, it renders it as "True"
            htmlAttributes["data-cascading"] = "true";

            var dependsList = new List<string>();
            dependsList.AddRange(cascade.DependsOn.Split(',').Select(x => x.Trim()));

            var dependsOnHtml = new List<string>();
            const string dot = ".";
            var hasDot = cache.FullExpression.Contains(dot);
            foreach (var dep in dependsList)
            {
                if (hasDot)
                {
                    var prefix = cache.FullExpression;
                    prefix = prefix.Substring(0, prefix.LastIndexOf(dot, StringComparison.InvariantCultureIgnoreCase));
                    dependsOnHtml.Add("#" + HtmlHelper.GenerateIdFromName(prefix + dot + dep));
                }
                else
                {
                    dependsOnHtml.Add("#" + dep);
                }
            }

            // Html encoding isn't done automatically, so we need to do it ourselves. 
            htmlAttributes["data-cascading-action"] = GenerateUrl(cascade.Action, cascade.Controller, cascade.Area);
            htmlAttributes["data-cascading-dependson"] = string.Join(",", dependsOnHtml);
            htmlAttributes["data-cascading-dependentsrequired"] = cascade.DependentsRequired.ToString().ToLower();
            addOptionalHtmlAttribute(cascade.ErrorText, "data-cascading-errortext");
            addOptionalHtmlAttribute(cascade.LoadingText, "data-cascading-loadingtext");
            addOptionalHtmlAttribute(cascade.PromptText, "data-cascading-prompttext");
            addOptionalHtmlAttribute(cascade.HttpMethod, "data-cascading-httpmethod");
            addOptionalHtmlAttribute(cache.CascadeDataIsPrerendered ? "true" : null, "data-cascading-prerendered");

            var actions = GetActionParameterNames(cascade.Action, cascade.Controller, cascade.Area);
            htmlAttributes["data-cascading-actionparam"] = string.Join(",", actions);
            if (!cascade.Async.GetValueOrDefault())
            {
                htmlAttributes["data-cascading-async"] = "false"; // Same as above with the "true" thing
            }

            return htmlAttributes;
        }

        #endregion

        private AutoCompleteBuilder InitializeAutoCompleteBuilder(BuilderData<AutoCompleteBuilder> cache)
        {
            cache.Builder.Value = GetFormattedModelValue(cache);
            var autocompleteAttr = (cache.ModelMetadata != null
                ? AutoCompleteAttribute.GetAttributeForModel(cache.ModelMetadata)
                : null);
            if (autocompleteAttr != null)
            {
                var actions = GetActionParameterNames(autocompleteAttr.Action, autocompleteAttr.Controller,
                    autocompleteAttr.Area);
                cache.Builder.ActionParameterName = string.Join(",", actions);
                cache.Builder.Url = GenerateUrl(autocompleteAttr.Action, autocompleteAttr.Controller,
                    autocompleteAttr.Area);
                cache.Builder.DependsOn = autocompleteAttr.DependsOn;
                cache.Builder.PlaceHolder = autocompleteAttr.PlaceHolder;

                if (autocompleteAttr.DisplayProperty != null)
                {
                    var modelValue = (int?)GetModelValue(cache, typeof(int?));
                    if (modelValue.HasValue)
                    {
                        // Unlike cascades, we don't call the autocomplete action because that would require knowing
                        // part of the display text to search for in the first place. We could call the autocomplete
                        // action without any parameters, but that result in a performance hit from returning all of
                        // the rows in a table.

                        var emeAttr = EntityMustExistAttribute.GetAttributeForModel(cache.ModelMetadata);
                        if (emeAttr == null)
                        {
                            throw new InvalidOperationException(
                                $"{cache.ModelMetadata.ContainerType?.FullName}.{cache.ModelMetadata.PropertyName} has an AutoComplete with DisplayProperty but is missing the required EntityMustExist attribute.");
                        }

                        dynamic repo = _container.GetInstance(emeAttr.RepositoryType);
                        // This part would be better off doing a dynamic query thing that only selects the necessary
                        // fields. I do not have time to figure out how to take the existing dynamic query thing and 
                        // making it work in here. -Ross 12/10/2019
                        // Cast the entity to an object first, using extension methods directly on a dynamic object does not work.
                        var entity = (object)repo.Find(modelValue.Value);
                        if (entity == null)
                        {
                            throw new InvalidOperationException(
                                $"{emeAttr.EntityType.FullName} with id {modelValue.Value} could not be found.");
                        }

                        cache.Builder.DisplayText =
                            (string)entity.GetPropertyValueByName(autocompleteAttr.DisplayProperty);
                    }
                }
            }

            return cache.Builder;
        }

        private FileUploadBuilder InitializeFileUploadBuilder(BuilderData<FileUploadBuilder> cache)
        {
            var fua = (cache.ModelMetadata != null
                ? FileUploadAttribute.GetAttributeForModel(cache.ModelMetadata)
                : null);
            var section = ConfigurationManager.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
            if (fua != null)
            {
                cache.Builder.WithButtonText(fua.ButtonText);
                cache.Builder.WithOnComplete(fua.OnComplete);
                cache.Builder.Url = GenerateUrl(fua.Action, fua.Controller, fua.Area);
                cache.Builder.AllowedExtensions.AddRange(
                    fua.AllowedFileTypes.Select(x => x.ToString().ToLowerInvariant()));
            }

            if (section != null)
            {
                cache.Builder.WithMaxUploadSize(section.MaxRequestLength);
            }

            return cache.Builder;
        }

        #endregion

        #region Public Methods

        #region ActionLink

        /// <summary>
        /// Generates a link with a secure token if the url requires it. Otherwise returns a regular url.
        /// </summary>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public LinkBuilder ActionLink(string linkText, [AspMvcAction] string actionName,
            [AspMvcController] string controllerName, object routeValues = null, object htmlAttributes = null)
        {
            // TODO: Remove the htmlAttributes parameter. None of the other ControlHelper methods have this because they all return an object that lets you do this already.
            return new LinkBuilder()
                  .With(htmlAttributes)
                  .WithText(linkText)
                  .WithHref(new SecureUrlHelper(_viewContext.RequestContext).Action(actionName, controllerName,
                       routeValues));
        }

        #endregion

        #region AutoComplete

        /// <summary>
        /// Returns an uninitialized AutoCompleteBuilder instance.
        /// </summary>
        /// <returns></returns>
        public AutoCompleteBuilder AutoComplete()
        {
            return new AutoCompleteBuilder();
        }

        /// <summary>
        /// Returns an AutoCompleteBuilder instance that has been initialized with an
        /// AutoCompleteAttribute from the given model expression.
        /// </summary>
        public AutoCompleteBuilder AutoComplete(string expression)
        {
            var cache = InitializeControlBuilder<AutoCompleteBuilder>(expression);
            return InitializeAutoCompleteBuilder(cache);
        }

        /// <summary>
        /// Returns an AutoCompleteBuilder instance that has been initialized with an
        /// AutoCompleteAttribute from the given model expression.
        /// </summary>
        public AutoCompleteBuilder AutoCompleteFor<TResult>(Expression<Func<T, TResult>> expression)
        {
            var cache = InitializeControlBuilder<AutoCompleteBuilder, TResult>(expression);
            return InitializeAutoCompleteBuilder(cache);
        }

        #endregion

        #region BoolDropDown

        private static BoolFormatAttribute GetEditorModeBoolFormatter(ModelMetadata metadata)
        {
            return
                (BoolFormatAttribute)ModelFormatterProviders.Current.TryGetModelFormatter(metadata,
                    FormatMode.Editor) ??
                new BoolFormatAttribute();
        }

        private SelectListBuilder InitializeBoolDropDownBuilder(BuilderData<SelectListBuilder> cache, string trueText,
            string falseText, string nullText)
        {
            var items = new[] {
                new SelectListItem {Value = Boolean.TrueString, Text = trueText},
                new SelectListItem {Value = Boolean.FalseString, Text = falseText}
            };

            cache.Builder.WithItems(items);
            // Defaults to DEFAULT_EMPTY_TEXT_FOR_DROPDOWNS since this uses CreateSelectListBuilder.
            if (nullText != null)
            {
                cache.Builder.WithEmptyText(nullText);
            }

            return cache.Builder;
        }

        /// <summary>
        /// Generates a drop down list for a nullable bool model. If the property has a BoolFormatAttribute
        /// on it, those values will be used in the dropdown.
        /// </summary>
        public SelectListBuilder BoolDropDown(string expression)
        {
            var cache = CreateSelectListBuilder(expression, SelectListType.DropDown);
            var boolFormat = GetEditorModeBoolFormatter(cache.ModelMetadata);
            return InitializeBoolDropDownBuilder(cache, boolFormat.True, boolFormat.False, boolFormat.Null)
               .With(_viewData["html"]);
            ;
        }

        /// <summary>
        /// Generates a drop down list for a nullable bool model using the given true/false/null text values.
        /// </summary>
        public SelectListBuilder BoolDropDown(string expression, string trueText, string falseText,
            string nullText = null)
        {
            var cache = CreateSelectListBuilder(expression, SelectListType.DropDown);
            return InitializeBoolDropDownBuilder(cache, trueText, falseText, nullText);
        }

        public SelectListBuilder BoolDropDownFor(Expression<Func<T, bool?>> expression)
        {
            var cache = CreateSelectListBuilder(expression, SelectListType.DropDown);
            var boolFormat = GetEditorModeBoolFormatter(cache.ModelMetadata);
            return InitializeBoolDropDownBuilder(cache, boolFormat.True, boolFormat.False, boolFormat.Null);
        }

        public SelectListBuilder BoolDropDownFor(Expression<Func<T, bool?>> expression, string trueText,
            string falseText, string nullText = null)
        {
            var cache = CreateSelectListBuilder(expression, SelectListType.DropDown);
            return InitializeBoolDropDownBuilder(cache, trueText, falseText, nullText);
        }

        #endregion

        #region Button

        /// <summary>
        /// Returns a ButtonBuilder instance with the supplied text. If you need a button to also include
        /// model values, use ValueButton instead.
        /// </summary>
        public ButtonBuilder Button(string text = null)
        {
            return new ButtonBuilder().WithText(text);
        }

        #endregion

        #region CheckBox

        /// <summary>
        /// Returns an unintialized CheckBoxBuilder instance.
        /// </summary>
        /// <returns></returns>
        public CheckBoxBuilder CheckBox()
        {
            return new CheckBoxBuilder();
        }

        /// <summary>
        /// Returns a CheckBoxBuilder with id, name, value, checked state, and validation attributes
        /// set based on the model expression.
        /// </summary>
        public CheckBoxBuilder CheckBox(string expression)
        {
            var cache = InitializeControlBuilder<CheckBoxBuilder>(expression);
            return CreateCheckBoxBuilder(cache);
        }

        /// <summary>
        /// Returns a CheckBoxBuilder with id, name, value, checked state, and validation attributes
        /// set based on the model expression.
        /// </summary>
        public CheckBoxBuilder CheckBoxFor<TResult>(Expression<Func<T, TResult>> expression)
        {
            var cache = InitializeControlBuilder<CheckBoxBuilder, TResult>(expression);
            return CreateCheckBoxBuilder(cache);
        }

        #endregion

        #region CheckBoxList

        /// <summary>
        /// Returns an unintialized CheckBoxListBuilder.
        /// </summary>
        /// <returns></returns>
        public CheckBoxListBuilder CheckBoxList()
        {
            return new CheckBoxListBuilder();
        }

        /// <summary>
        /// Returns a CheckBoxListBuilder with id, name, items, and selected values
        /// set based on the model expression. NOTE: Unobtrusivate validation does not work on this.
        /// </summary>
        public CheckBoxListBuilder CheckBoxList(string expression)
        {
            var cache = InitializeControlBuilder<CheckBoxListBuilder>(expression);
            return CreateCheckBoxListBuilder(cache);
        }

        /// <summary>
        /// Returns a CheckBoxListBuilder with id, name, items, and selected values
        /// set based on the model expression. NOTE: Unobtrusivate validation does not work on this.
        /// </summary>
        public CheckBoxListBuilder CheckBoxListFor<TResult>(Expression<Func<T, TResult>> expression)
        {
            var cache = InitializeControlBuilder<CheckBoxListBuilder, TResult>(expression);
            return CreateCheckBoxListBuilder(cache);
        }

        #endregion

        #region DatePicker

        /// <summary>
        /// Returns an uninitialized DatePickerBuilder instance.
        /// </summary>
        public DatePickerBuilder DatePicker()
        {
            return new DatePickerBuilder();
        }

        /// <summary>
        /// Returns a DatePickerBuilder instance with its id, name, value and other properties
        /// set for the given model expression. This includes any values from DateTimePickerAttribute.
        /// </summary>
        public DatePickerBuilder DatePicker(string expression)
        {
            var cache = InitializeControlBuilder<DatePickerBuilder>(expression);
            cache.Builder.Value = GetFormattedModelValue(cache);
            cache.Builder.IncludeTimePicker = DateTimePickerAttribute.GetFromModelMetadata(cache.ModelMetadata) != null;
            return cache.Builder;
        }

        /// <summary>
        /// Returns a DatePickerBuilder instance with its id, name, value and other properties
        /// set for the given model expression. This includes any values from DateTimePickerAttribute.
        /// </summary>
        public DatePickerBuilder DatePickerFor<TResult>(Expression<Func<T, TResult>> expression)
        {
            var cache = InitializeControlBuilder<DatePickerBuilder, TResult>(expression);
            cache.Builder.Value = GetFormattedModelValue(cache);
            cache.Builder.IncludeTimePicker = DateTimePickerAttribute.GetFromModelMetadata(cache.ModelMetadata) != null;
            return cache.Builder;
        }

        #endregion

        #region DropDown

        /// <summary>
        /// Returns an uninitialized SelectListBuilder with Type set to DropDown.
        /// </summary>
        /// <returns></returns>
        public SelectListBuilder DropDown()
        {
            return new SelectListBuilder().AsType(SelectListType.DropDown);
        }

        /// <summary>
        /// Returns a SelectListBuilder with its id, name, unobtrusive attributes, items,
        /// and selected values set for the given model expression.
        /// </summary>
        public SelectListBuilder DropDown(string expression)
        {
            return CreateSelectListBuilder(expression, SelectListType.DropDown).Builder;
        }

        /// <summary>
        /// Returns a SelectListBuilder with its id, name, unobtrusive attributes, items,
        /// and selected values set for the given model expression.
        /// </summary>
        public SelectListBuilder DropDownFor<TResult>(Expression<Func<T, TResult>> expression)
        {
            return CreateSelectListBuilder(expression, SelectListType.DropDown).Builder;
        }

        #endregion

        #region FileUpload

        /// <summary>
        /// Returns an uninitialized FileUploadBuilder instance.
        /// </summary>
        /// <returns></returns>
        public FileUploadBuilder FileUpload()
        {
            return new FileUploadBuilder();
        }

        /// <summary>
        /// Returns a FileUploadBuilder initialized for model binding with the given model expression.
        /// </summary>
        public FileUploadBuilder FileUpload(string expression)
        {
            var cache = InitializeControlBuilder<FileUploadBuilder>(expression);
            return InitializeFileUploadBuilder(cache);
        }

        /// <summary>
        /// Returns a FileUploadBuilder initialized for model binding with the given model expression.
        /// </summary>
        public FileUploadBuilder FileUploadFor<TResult>(Expression<Func<T, TResult>> expression)
        {
            var cache = InitializeControlBuilder<FileUploadBuilder, TResult>(expression);
            return InitializeFileUploadBuilder(cache);
        }

        #endregion

        #region Hidden

        /// <summary>
        /// Returns an uninitialized HiddenInputBuilder instance.
        /// </summary>
        /// <returns></returns>
        public HiddenInputBuilder Hidden()
        {
            return new HiddenInputBuilder();
        }

        /// <summary>
        /// Returns a HiddenInputBuilder instance with its id, name, value and unobtrusive
        /// validation attributes set for the given model expression.
        /// </summary>
        public HiddenInputBuilder Hidden(string expression)
        {
            var cache = InitializeControlBuilder<HiddenInputBuilder>(expression);
            return CreateHiddenInputBuilder(cache);
        }

        /// <summary>
        /// Returns a HiddenInputBuilder instance with its id, name, value and unobtrusive
        /// validation attributes set for the given model expression.
        /// </summary>
        public HiddenInputBuilder HiddenFor<TResult>(Expression<Func<T, TResult>> expression)
        {
            var cache = InitializeControlBuilder<HiddenInputBuilder, TResult>(expression);
            return CreateHiddenInputBuilder(cache);
        }

        #endregion

        #region ListBox

        /// <summary>
        /// Returns an uninitialized SelectListBuilder instance with its type set to ListBox.
        /// </summary>
        /// <returns></returns>
        public SelectListBuilder ListBox()
        {
            return new SelectListBuilder().AsType(SelectListType.ListBox);
        }

        /// <summary>
        /// Returns a SelectListBuilder with its type set to ListBox. Id, name, value, items and
        /// selected values are automatically set from the model expression.
        /// </summary>
        public SelectListBuilder ListBox(string expression)
        {
            var lb = CreateSelectListBuilder(expression, SelectListType.ListBox).Builder;
            lb.WithEmptyText(null); // Listboxes don't need an empty value since items just need to be unselected.
            return lb;
        }

        /// <summary>
        /// Returns a SelectListBuilder with its type set to ListBox. Id, name, value, items and
        /// selected values are automatically set from the model expression.
        /// </summary>
        public SelectListBuilder ListBoxFor<TResult>(Expression<Func<T, TResult>> expression)
        {
            var lb = CreateSelectListBuilder(expression, SelectListType.ListBox).Builder;
            lb.WithEmptyText(null); // Listboxes don't need an empty value since items just need to be unselected.
            return lb;
        }

        #endregion

        #region Password

        public TextBoxBuilder Password()
        {
            return new TextBoxBuilder().AsType(TextBoxType.Password);
        }

        public TextBoxBuilder Password(string expression)
        {
            return CreateTextBoxBuilder(expression, TextBoxType.Password);
        }

        public TextBoxBuilder PasswordFor<TResult>(Expression<Func<T, TResult>> expression)
        {
            return CreateTextBoxBuilder(expression, TextBoxType.Password);
        }

        #endregion

        #region RangePicker

        public RangePickerBuilder RangePicker(string expression)
        {
            var cache = InitializeControlBuilder<RangePickerBuilder>(expression);

            // We don't want the dot if the expression is an empty string, it messes up
            // the way the field name/id values are rendered.
            var childExpressionRoot = (expression != string.Empty ? expression + "." : "");
            var startExpression = childExpressionRoot + "Start";
            var endExpression = childExpressionRoot + "End";

            if (typeof(Data.Range<DateTime>).IsAssignableFrom(cache.ModelMetadata.ModelType))
            {
                cache.Builder.StartBuilder = DatePicker(startExpression);
                cache.Builder.EndBuilder = DatePicker(endExpression);
            }
            else
            {
                cache.Builder.StartBuilder = TextBox(startExpression);
                cache.Builder.EndBuilder = TextBox(endExpression);
            }

            // RangePickerBuilder has a dropdown created by default, but we need to merge in
            // the selected values/html attributes and what not.
            var operatorBuilder = DropDown(childExpressionRoot + "Operator");
            cache.Builder.OperatorBuilder.SelectedValues.AddRange(operatorBuilder.SelectedValues);
            cache.Builder.OperatorBuilder.With(operatorBuilder.HtmlAttributes);

            return cache.Builder;
        }

        public RangePickerBuilder RangePickerFor<TResult>(Expression<Func<T, TResult>> expression)
        {
            var expressionText = ExpressionHelper.GetExpressionText(expression);
            return RangePicker(expressionText);
        }

        #endregion

        #region ResetButton

        /// <summary>
        /// Returns a ButtonBuilder instance with the supplied text as a reset button.
        /// </summary>
        public ButtonBuilder ResetButton(string buttonText = "Reset")
        {
            return Button(buttonText).AsType(ButtonType.Reset);
        }

        #endregion

        #region SubmitButton

        /// <summary>
        /// Returns a ButtonBuilder instance with the supplied text as a submit button. 
        /// If you need a button to also include model values, use ValueButton().AsType(ButtonType.Submit) instead.
        /// </summary>
        public ButtonBuilder SubmitButton(string buttonText = null)
        {
            return Button(buttonText).AsType(ButtonType.Submit);
        }

        #endregion

        #region TextArea

        /// <summary>
        /// Returns an unintialized TextBoxBuilder instance with its Type set to TextArea.
        /// </summary>
        /// <returns></returns>
        public TextBoxBuilder TextArea()
        {
            return new TextBoxBuilder().AsType(TextBoxType.TextArea);
        }

        /// <summary>
        /// Returns a TextBoxBuilder isntance with its type set TextArea. Id, name, value,
        /// and validation attributes are set from the model expression.
        /// </summary>
        public TextBoxBuilder TextArea(string expression)
        {
            return CreateTextBoxBuilder(expression, TextBoxType.TextArea);
        }

        /// <summary>
        /// Returns a TextBoxBuilder isntance with its type set TextArea. Id, name, value,
        /// and validation attributes are set from the model expression.
        /// </summary>
        public TextBoxBuilder TextAreaFor<TResult>(Expression<Func<T, TResult>> expression)
        {
            return CreateTextBoxBuilder(expression, TextBoxType.TextArea);
        }

        #endregion

        #region TextBox

        /// <summary>
        /// Returns an uninitialized TextBoxBuilder with its Type set to Text.
        /// </summary>
        /// <returns></returns>
        public TextBoxBuilder TextBox()
        {
            return new TextBoxBuilder().AsType(TextBoxType.Text);
        }

        /// <summary>
        /// Returns a TextBoxBuilder isntance with its type set Text. Id, name, value,
        /// and validation attributes are set from the model expression.
        /// </summary>
        public TextBoxBuilder TextBox(string expression)
        {
            return CreateTextBoxBuilder(expression, TextBoxType.Text);
        }

        /// <summary>
        /// Returns a TextBoxBuilder isntance with its type set Text. Id, name, value,
        /// and validation attributes are set from the model expression.
        /// </summary>
        public TextBoxBuilder TextBoxFor<TResult>(Expression<Func<T, TResult>> expression)
        {
            return CreateTextBoxBuilder(expression, TextBoxType.Text);
        }

        #endregion

        #region ValueButton

        /// <summary>
        /// Returns a ButtonBuilder with its id, name, value and text automatically populated from the model. If you don't
        /// need any of this automated, use one of the other Button methods instead.
        /// </summary>
        public ButtonBuilder ValueButton(string expression)
        {
            var tb = InitializeControlBuilder<ButtonBuilder>(expression, includeValidationAttributesAndCss: false);
            var val = GetFormattedModelValue(tb);
            tb.Builder.Value = val;
            tb.Builder.Text = val; // Set Text to the same to give it a default value.
            tb.Builder.Type = ButtonType.Button;
            return tb.Builder;
        }

        /// <summary>
        /// Returns a ButtonBuilder with its id, name, value and text automatically populated from the model. If you don't
        /// need any of this automated, use one of the other Button methods instead.
        /// </summary>
        public ButtonBuilder ValueButtonFor<TResult>(Expression<Func<T, TResult>> expression)
        {
            var tb = InitializeControlBuilder<ButtonBuilder, TResult>(expression,
                includeValidationAttributesAndCss: false);
            var val = GetFormattedModelValue(tb);
            tb.Builder.Value = val;
            tb.Builder.Text = val; // Set Text to the same to give it a default value.
            tb.Builder.Type = ButtonType.Button;
            return tb.Builder;
        }

        #endregion

        #endregion
    }

    #region Helper classes

    // This is only public because of Moq.
    public interface IBuilderData
    {
        string Expression { get; }
        string FullExpression { get; }

        /// <summary>
        /// The ModelMetadata for the model being used with the ControlHelper.
        /// This *can be null* when a ControlHelper method is being used to render
        /// something for a property that doesn't exist for the view's model type.
        /// This usually happens when doing something like @Control.DropDown("StringPropertyName")
        /// and that's a perfectly valid use.
        /// </summary>
        ModelMetadata ModelMetadata { get; }

        /// <summary>
        /// This is only useful for the cascade stuff. Set to true if
        /// pre-rendering cascade data.
        /// </summary>
        bool CascadeDataIsPrerendered { get; set; }
    }

    /// <summary>
    /// Same as EmptyBuilderData but includes the Builder instance being used.
    /// </summary>
    /// <typeparam name="TBuilder"></typeparam>
    internal class BuilderData<TBuilder> : EmptyBuilderData where TBuilder : ControlBuilder<TBuilder>
    {
        public TBuilder Builder { get; private set; }

        public BuilderData(TBuilder builder, string expression, string fullExpression, ModelMetadata metadata)
            : base(expression, fullExpression, metadata)
        {
            Builder = builder;
        }
    }

    /// <summary>
    /// This is meant to cache lookup values while creating a control. Looking up metadata and stuff
    /// can be a bit costly(and confusing to have to keep looking up).
    /// </summary>
    internal class EmptyBuilderData : IBuilderData
    {
        public string Expression { get; }
        public string FullExpression { get; }
        public ModelMetadata ModelMetadata { get; }
        public bool CascadeDataIsPrerendered { get; set; }

        public EmptyBuilderData(string expression, string fullExpression, ModelMetadata metadata)
        {
            Expression = expression;
            FullExpression = fullExpression;
            ModelMetadata = metadata;
        }
    }

    #endregion
}
