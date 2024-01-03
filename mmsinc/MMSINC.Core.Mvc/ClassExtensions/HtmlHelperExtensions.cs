using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.WebPages;
using JetBrains.Annotations;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Data;
using MMSINC.Helpers;
using MMSINC.Metadata;
using MMSINC.Models.ViewModels;
using MMSINC.Results;
using MMSINC.Utilities;
using StructureMap;

namespace MMSINC.ClassExtensions
{
    // NOTE WHEN ADDING NEW EXTENSIONS:
    // Add an overload that doesn't accept any arguments that then passes
    // an empty string to whatever method it needs to call. This is needed
    // for editor/display templates to properly bind to things and add
    // validators and stuff correctly.
    //
    // See the DisplayLabelFor methods for an example.
    public static class HtmlHelperExtensions
    {
        #region Constants

        public static readonly Regex TRAILING_ID = new Regex(@" id$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        internal const string RESOURCE_REGISTRY_KEY = "Resource Registry Key";
        private static readonly HtmlString EMPTY_HTML_STRING = new HtmlString(string.Empty);
        public const string DEFAULT_ROUTE_NAME = "Default";
        public const string USPS_LINK_FORMAT = "https://tools.usps.com/go/TrackConfirmAction.action?tLabels={0}";

        #endregion

        #region Public Methods

        /// <summary>
        /// Ensures that an anonymous object is converted to a RouteValueDictionary. If it
        /// already is a RouteValueDictionary the original object will be returned.
        /// </summary>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public static RouteValueDictionary ConvertToRouteValueDictionary(object routeValues)
        {
            return (routeValues as RouteValueDictionary ?? new RouteValueDictionary(routeValues));
        }

        /// <summary>
        /// The built in method that does this doesn't take into consideration that an anonymous
        /// object may already implement IDictionary, causing it to return a new RVD with
        /// the wrong keys.
        /// </summary>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static RouteValueDictionary AnonymousObjectToHtmlAttributes(object htmlAttributes)
        {
            if (htmlAttributes is IDictionary<string, object>)
            {
                return new RouteValueDictionary((IDictionary<string, object>)htmlAttributes);
            }

            return new RouteValueDictionary(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static string PrettyText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            text = text.Trim();

            if (text.Contains(" "))
            {
                return text;
            }

            try
            {
                // Wordify only works if theTextIsActuallyCamelCase.
                // It will throw an exception if whitespace is found.
                text = Wordify.SpaceOutWordsFromCamelCase(text);

                // I feel like this isn't necessary anymore. This is holdover
                // from view models having "PropertyId" names which we no longer
                // do. -Ross 6/15/2018
                return TRAILING_ID.Replace(text, "");
            }
            catch (NotSupportedException)
            {
                // NOTE: This exception should not be reached anymore as of 6/14/2018 -Ross

                // This method is potentially called when a property name is needed and
                // that property has a DisplayNameAttribute with a value with a space. With.
                // Wordify throws in that case, so just return back the text we were given.
                return text;
            }
        }

        public static string PrettyText(this HtmlHelper helper, string text)
        {
            return PrettyText(text);
        }

        public static string PrettyText(ViewDataDictionary viewData)
        {
            var metaData = ModelMetadata.FromStringExpression(String.Empty, viewData);
            if (metaData.DisplayName != null)
            {
                // It should do its own thing if a DisplayName is set.
                return metaData.DisplayName;
            }

            return PrettyText(metaData.PropertyName);
        }

        /// <summary>
        /// Wraps a HelperResult so that, when it writes, it replaces the HtmlHelper.ViewContext.Writer
        /// temporarily so that Html.BeginForm doesn't get messed up and write to a different stream.
        /// </summary>
        /// <returns></returns>
        public static Func<object, HelperResult> WrapHelperResult(HtmlHelper helper,
            Func<object, HelperResult> helperResult)
        {
            return WrapHelperResult<object>(helper, helperResult);
        }

        /// <summary>
        /// 
        /// Wraps a HelperResult so that, when it writes, it replaces the HtmlHelper.ViewContext.Writer
        /// temporarily so that Html.BeginForm doesn't get messed up and write to a different stream.
        /// </summary>
        /// <returns></returns>
        public static Func<T, HelperResult> WrapHelperResult<T>(HtmlHelper helper, Func<T, HelperResult> helperResult)
        {
            return (obj) => {
                return new HelperResult(writer => {
                    // Html.BeginForm writes directly to ViewContext.Writer, which is
                    // stupid and annoying. This cause the form tags to be written
                    // to the wrong writer when used inside a HelperResult. To get around
                    // this, we have to set the ViewContext.Writer to the one passed to
                    // the HelperResult, and then revert it again.

                    using (var wrapper =
                        new ViewContextWriterWrapper(helper.ViewContext, writer, helper.ViewDataContainer))
                    {
                        helperResult(obj).WriteTo(wrapper.Writer);
                    }
                });
            };
        }

        #endregion

        #region Extension Methods

        #region ActionLinkForRoute

        /// <summary>
        /// Returns an action link created for a specific named route.
        /// </summary>
        /// <returns></returns>
        public static IHtmlString ActionRouteLink(this HtmlHelper helper, string linkText, string routeName,
            [AspMvcAction] string action, [AspMvcController] string controller)
        {
            return helper.ActionRouteLink(linkText, routeName, action, controller, null);
        }

        /// <summary>
        /// Returns an action link created for a specific named route.
        /// </summary>
        /// <returns></returns>
        public static IHtmlString ActionRouteLink(this HtmlHelper helper, string linkText, string routeName,
            [AspMvcAction] string action, [AspMvcController] string controller, object routeValues)
        {
            return helper.ActionRouteLink(linkText, routeName, action, controller, routeValues, null);
        }

        /// <summary>
        /// Returns an action link created for a specific named route.
        /// </summary>
        /// <returns></returns>
        public static IHtmlString ActionRouteLink(this HtmlHelper helper, string linkText, string routeName,
            [AspMvcAction] string action, [AspMvcController] string controller, object routeValues,
            object htmlAttributes)
        {
            var rvd = ConvertToRouteValueDictionary(routeValues);
            var html = AnonymousObjectToHtmlAttributes(htmlAttributes);
            rvd["action"] = action;
            rvd["controller"] = controller;
            return helper.RouteLink(linkText, routeName, rvd, html);
        }

        #endregion

        #region AuthorizedActionLink

        /// <summary>
        /// Renders a link for a specific action if the current user is authorized to access that action.
        /// </summary>
        public static HtmlString AuthorizedActionLink(this HtmlHelper helper, string linkText,
            [AspMvcAction] string actionName, [AspMvcController] string controllerName, object routeValues = null,
            object htmlAttributes = null)
        {
            // We DO NOT want the overload that takes routeValues and htmlAttributes as objects,
            // we want the type specific one. So we need to do some casting/creating first.
            // Do not pass a RouteValueDictionary to another RouteValueDictionary, it
            // picks the Dictionary properties to merge instead of the keys in the existing one.
            var routeValueDict = (routeValues is RouteValueDictionary
                ? (RouteValueDictionary)routeValues
                : new RouteValueDictionary(routeValues));
            var htmlAttrDict = (htmlAttributes is IDictionary<string, object>
                ? (IDictionary<string, object>)htmlAttributes
                : new RouteValueDictionary(htmlAttributes));

            var area = (string)routeValueDict["area"];
            var routeContext = new RouteContext(helper.ViewContext.RequestContext, controllerName, actionName, area);
            if (!routeContext.IsAuthorized())
            {
                return EMPTY_HTML_STRING;
            }

            return helper.ActionLink(linkText, actionName, controllerName, routeValueDict, htmlAttrDict);
        }

        /// <summary>
        /// Renders a link for a specific action if the current user is authorized to access that action.
        /// </summary>
        public static HtmlString AuthorizedActionLink(this HtmlHelper helper, object modelForLinkText,
            [AspMvcAction] string actionName, [AspMvcController] string controllerName, object routeValues = null,
            object htmlAttributes = null)
        {
            var linkText = modelForLinkText.ToString();
            return AuthorizedActionLink(helper, linkText, actionName, controllerName, routeValues, htmlAttributes);
        }

        /// <summary>
        /// Renders a link for a specific action if the current user is authorized to access that action. If the
        /// user is not authorized, then the link text is returned by itself.
        /// </summary>
        public static IHtmlString AuthorizedActionLinkOrText(this HtmlHelper helper, string linkText,
            [AspMvcAction] string actionName, [AspMvcController] string controllerName, object routeValues = null,
            object htmlAttributes = null)
        {
            var result = AuthorizedActionLink(helper, linkText, actionName, controllerName, routeValues: routeValues,
                htmlAttributes: htmlAttributes);
            if (result == EMPTY_HTML_STRING)
            {
                return new HtmlString(linkText);
            }

            return result;
        }

        /// <summary>
        /// Renders a link for a specific action if the current user is authorized to access that action. If the
        /// user is not authorized, then the link text is returned by itself.
        /// </summary>
        public static IHtmlString AuthorizedActionLinkOrText(this HtmlHelper helper, object modelForLinkText,
            [AspMvcAction] string actionName, [AspMvcController] string controllerName, object routeValues = null,
            object htmlAttributes = null)
        {
            var linkText = modelForLinkText.ToString();
            return AuthorizedActionLinkOrText(helper, linkText, actionName, controllerName, routeValues,
                htmlAttributes);
        }

        #endregion

        #region AuthorizedActionLinkButton

        /// <summary>
        /// Renders a link button for a specific action if the current user is authorized to access that action.
        /// </summary>
        public static IHtmlString AuthorizedActionLinkButton(this HtmlHelper helper, string linkText,
            [AspMvcAction] string actionName, [AspMvcController] string controllerName, object routeValues = null,
            object htmlAttributes = null)
        {
            // We DO NOT want the overload that takes routeValues and htmlAttributes as objects,
            // we want the type specific one. So we need to do some casting/creating first.
            // Do not pass a RouteValueDictionary to another RouteValueDictionary, it
            // picks the Dictionary properties to merge instead of the keys in the existing one.
            var routeValueDict = (routeValues is RouteValueDictionary
                ? (RouteValueDictionary)routeValues
                : new RouteValueDictionary(routeValues));
            var htmlAttrDict = (htmlAttributes is IDictionary<string, object>
                ? (IDictionary<string, object>)htmlAttributes
                : new RouteValueDictionary(htmlAttributes));

            var routeContext = new RouteContext(helper.ViewContext.RequestContext, controllerName, actionName,
                (string)routeValueDict["area"]);
            if (!routeContext.IsAuthorized())
            {
                return EMPTY_HTML_STRING;
            }

            return helper.LinkButton(linkText, actionName, controllerName, routeValueDict, htmlAttrDict);
        }

        #endregion

        #region SecureActionLink

        public static HtmlString SecureActionLink<TUser>(this HtmlHelper helper, string linkText,
            [AspMvcAction] string actionName, [AspMvcController] string controllerName, object routeValues = null,
            object htmlAttributes = null)
            where TUser : IAdministratedUser
        {
            var routeValuesLocal = routeValues as IDictionary<string, object> ?? new RouteValueDictionary(routeValues);
            var htmlAttributesLocal = htmlAttributes as IDictionary<string, object> ??
                                      new RouteValueDictionary(htmlAttributes);

            FixKeyNames(htmlAttributesLocal);

            var token = DependencyResolver.Current.GetService<ISecureFormTokenService>()
                                          .CreateTokenWithRouteValues(actionName, controllerName,
                                               DependencyResolver.Current.GetService<IAuthenticationService<TUser>>()
                                                                 .CurrentUser.Id, routeValuesLocal);

            routeValuesLocal.Add(FormBuilder.SECURE_FORM_HIDDEN_FIELD_NAME, token);
            return helper.ActionLink(linkText, actionName, controllerName, new RouteValueDictionary(routeValuesLocal),
                htmlAttributesLocal);
        }

        private static void FixKeyNames(IDictionary<string, object> dict)
        {
            var keys = dict.Keys.ToArray();
            foreach (var key in keys.Where(k => k.Contains("_")))
            {
                var value = dict[key];
                dict.Remove(key);
                dict[key.Replace("_", "-")] = value;
            }
        }

        #endregion

        #region AbsoluteActionLink

        /// <summary>
        /// Creates an ActionLink with an absolute uri instead of a relative uri. Only useful for PDFs really.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString AbsoluteActionLink(this HtmlHelper helper, string linkText, string actionName,
            string controllerName, object routeValues, object htmlAttributes)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);
            return helper.Link(
                urlHelper.Action(actionName, controllerName, routeValues,
                    helper.ViewContext.HttpContext.Request.Url.Scheme), linkText, htmlAttributes);
        }

        #endregion

        #region DropDownListWithPrompt

        [Obsolete("Control.DropDown automatically adds the default prompt text.")]
        public static IHtmlString DropDownListWithPrompt(this HtmlHelper html, string expression,
            IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
        {
            return html.DropDownList(expression,
                new[] {new SelectListItem {Selected = true, Text = SelectAttribute.DEFAULT_ITEM_LABEL}}.MergeWith(
                    selectList),
                htmlAttributes);
        }

        #endregion

        #region ButtonGroup

        /// <summary>
        /// Renders a section for buttons to be grouped in if they're being kept in line with
        /// a field-column kinda thing. You can really put anything in here if you need to.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="helperResult"></param>
        /// <returns></returns>
        public static IHtmlString ButtonGroup(this HtmlHelper helper, Func<object, HelperResult> helperResult)
        {
            return helper.RenderDisplayTemplate((IHtmlString)null, helperResult, "fp-buttons");
        }

        /// <summary>
        /// Renders a section for buttons to be grouped in if they're being kept in line with
        /// a field-column kinda thing. You can really put anything in here if you need to.
        /// </summary>
        public static IHtmlString ButtonGroup(this HtmlHelper helper, IHtmlString buttonGroupInsides)
        {
            return helper.ButtonGroup(buttonGroupInsides.ToHelperResult());
        }

        #endregion

        #region CurrentUserCanDo

        public static bool CurrentUserCanDo(this HtmlHelper helper, [AspMvcAction] string actionName,
            [AspMvcController] string controllerName, [AspMvcArea] string areaName = null)
        {
            RouteContext routeContext = null;

            try
            {
                routeContext = new RouteContext(helper.ViewContext.RequestContext, controllerName, actionName,
                    areaName);
            }
            catch (RouteContextException)
            {
                // Means the controller or action doesn't exist. This is valid for actions as a controller may
                // not have said action and this is being called from a shared view.
                return false;
            }

            return routeContext.IsAuthorized();
        }

        #endregion

        #region CurrentUserCanEdit

        public static bool CurrentUserCanEdit(this HtmlHelper helper)
        {
            return helper.CurrentUserCanDo("Edit",
                helper.ViewContext.RequestContext.RouteData.Values["controller"].ToString());
        }

        #endregion

        #region DefaultActionLink

        /// <summary>
        /// Returns an action link created for the route named "Default".
        /// </summary>
        /// <returns></returns>
        public static IHtmlString DefaultActionLink(this HtmlHelper helper, string linkText,
            [AspMvcAction] string action, [AspMvcController] string controller, [AspMvcArea] string area)
        {
            return helper.DefaultActionLink(linkText, action, controller, area, null);
        }

        /// <summary>
        /// Returns an action link created for the route named "Default".
        /// </summary>
        /// <returns></returns>
        public static IHtmlString DefaultActionLink(this HtmlHelper helper, string linkText,
            [AspMvcAction] string action, [AspMvcController] string controller, [AspMvcArea] string area,
            object routeValues)
        {
            return helper.DefaultActionLink(linkText, action, controller, area, routeValues, null);
        }

        /// <summary>
        /// Returns an action link created for the route named "Default".
        /// </summary>
        /// <returns></returns>
        public static IHtmlString DefaultActionLink(this HtmlHelper helper, string linkText,
            [AspMvcAction] string action, [AspMvcController] string controller, [AspMvcArea] string area,
            object routeValues, object htmlAttributes)
        {
            var routeName = string.IsNullOrWhiteSpace(area) ? DEFAULT_ROUTE_NAME : area + "_" + DEFAULT_ROUTE_NAME;

            return helper.ActionRouteLink(linkText, routeName, action, controller, routeValues, htmlAttributes);
        }

        #endregion

        #region Description

        /// <summary>
        /// Returns the text from a DescriptionAttribute if the property has one. Returns null if no attribute is found.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="expression">Optional property description. Uses current model property if it isn't set.</param>
        /// <returns></returns>
        public static IHtmlString Description(this HtmlHelper html, string expression = "")
        {
            var metadata = ModelMetadata.FromStringExpression(expression, html.ViewData);
            // TODO: This is a quick hack and needs to be done properly.
            const string key = "DescriptionAttribute";
            if (metadata.AdditionalValues.ContainsKey(key))
            {
                var attr = (DescriptionAttribute)metadata.AdditionalValues[key];
                return new HtmlString(attr.Description);
            }

            return null;
        }

        #endregion

        #region DisplayCrumbs

        public static CrumbBuilder CreateCrumbBuilder(this HtmlHelper html, string defaultProperty = null,
            string separator = null)
        {
            return new CrumbBuilder(html.ViewContext, html.ViewContext.ViewData.Model) {
                Separator = separator,
                DefaultPropertyForShowText = defaultProperty
            };
        }

        /// <summary>
        /// Returns an HtmlString of bread crumb links for the current route. 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="separator">Optional parameter for setting the dividing text between links.</param>
        /// <param name="defaultProperty">The default property on a model that should be used for its link text.</param>
        /// <returns></returns>
        public static IHtmlString DisplayCrumbs(this HtmlHelper html, string defaultProperty = null,
            string separator = null)
        {
            return html.CreateCrumbBuilder(defaultProperty, separator).ToHtmlString(html);
        }

        /// <summary>
        /// Returns an HtmlString of bread crumb text for the current route. No links in here. 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="separator">Optional parameter for setting the dividing text between links.</param>
        /// <param name="defaultProperty">The default property on a model that should be used for its link text.</param>
        /// <returns></returns>
        public static IHtmlString DisplayTitleCrumbs(this HtmlHelper html, string defaultProperty = null,
            string separator = null)
        {
            var builder = html.CreateCrumbBuilder(defaultProperty, separator);
            builder.TextOnlySeparator = separator;
            return builder.ToTextOnlyHtmlString(html);
        }

        #endregion

        #region DisplayInlineNotification

        /// <summary>
        /// Displays a notification similar to the ones that appear at the top of the place. These will render
        /// wherever you place them.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="notificationItem"></param>
        /// <returns></returns>
        public static IHtmlString DisplayInlineNotification(this HtmlHelper htmlHelper,
            NotificationItem notificationItem)
        {
            // NOTE: The css for this is defined in notifications.css and is used by both the MVC and Contractors project.
            return htmlHelper.Partial("~/Views/Shared/_NotificationItem.cshtml", notificationItem);
        }

        #endregion

        #region DisplayNotifations

        /// <summary>
        /// Renders a partial that displays all current notifications.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static IHtmlString DisplayNotifications(this HtmlHelper htmlHelper)
        {
            return htmlHelper.Partial("~/Views/Shared/_Notifications.cshtml");
        }

        #endregion

        #region DisplayLabel

        /// <summary>
        /// Needed so we can pass in an empty string and still get pretty text back
        /// when we're using editor templates. Has to d
        /// </summary>
        public static MvcHtmlString DisplayLabelFor(this HtmlHelper html)
        {
            var metaData = ModelMetadata.FromStringExpression(String.Empty, html.ViewData);
            if (metaData.DisplayName != null)
            {
                // It should do its own thing if a DisplayName is set.
                return html.Label(String.Empty);
            }

            return html.Label(String.Empty, PrettyText(metaData.PropertyName));
        }

        public static MvcHtmlString DisplayLabelFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression)
        {
            var modelProp = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

            // If we have a custom name set by a DisplayNameAttribute, we don't wanna overwrite it.
            if (modelProp.DisplayName != null) // Is null if not set, rather than being the same as PropertyName
            {
                return html.LabelFor(expression);
            }

            return html.LabelFor(expression, PrettyText(modelProp.PropertyName));
        }

        public static MvcHtmlString DisplayLabelFor(this HtmlHelper html, string name)
        {
            return html.Label(name, PrettyText(name));
        }

        #endregion

        #region DisplayPrettyNameFor

        public static MvcHtmlString DisplayPrettyName(this HtmlHelper html, string unprettyName)
        {
            return new MvcHtmlString(PrettyText(unprettyName));
        }

        /// <summary>
        /// Gets the pretty version of a property name and displays it without any additional html.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString DisplayPrettyNameFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression)
        {
            var unpretty = html.DisplayNameFor(expression);
            return DisplayPrettyName(html, unpretty.ToString());
        }

        #endregion

        #region DisplayForIfNotNullOrWhiteSpace

        public static MvcHtmlString DisplayForIfNotNullOrWhiteSpace<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression)
        {
            var value = ((object)expression.Compile()(html.ViewData.Model) ?? String.Empty).ToString();

            return String.IsNullOrWhiteSpace(value) ? new MvcHtmlString(String.Empty) : html.DisplayFor(expression);
        }

        #endregion

        #region DisplayAndHiddenFor

        public static MvcHtmlString DisplayAndHiddenFor<TModel, TDisplayValue, THiddenValue>(
            this HtmlHelper<TModel> html, Expression<Func<TModel, TDisplayValue>> displayExpression,
            Expression<Func<TModel, THiddenValue>> hiddenExpression)
        {
            return new MvcHtmlString(html.DisplayFor(displayExpression) + html.HiddenFor(hiddenExpression).ToString());
        }

        public static MvcHtmlString DisplayAndHiddenFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression)
        {
            return DisplayAndHiddenFor(html, expression, expression);
        }

        #endregion

        #region DisplayValueFor

        // TODO: May need an EditorValueFor display someday.

        private static IHtmlString DisplayValueImpl(this HtmlHelper html, ModelMetadata meta, string formatString)
        {
            // We want to override any attribute-based formatting with any format passed in explicitly.
            var format = formatString ?? meta.DisplayFormatString;

            // We wanna use format strings by default if we can.
            if (format != null)
            {
                var newText = html.FormatValue(meta.Model, format);
                return new HtmlString(newText);
            }

            // Then we wanna try to use FormatAttribute if it exists. 

            var formatAttr = ModelFormatterProviders.Current.TryGetModelFormatter(meta, FormatMode.Display);
            if (formatAttr != null)
            {
                return new HtmlString(formatAttr.FormatValue(meta.Model));
            }

            // And if these fail return null so the callers can do their own thing.
            return null;
        }

        /// <summary>
        /// Same as the built in DisplayText extension method except it will take into consideration the DisplayFormatAttribute if one exists.
        /// </summary>
        public static IHtmlString DisplayValue(this HtmlHelper html, string expression, string formatString = null)
        {
            var meta = ModelMetadata.FromStringExpression(expression, html.ViewData);

            // Defer to the regular method call if any of the other formatting failed.
            // The built in method does things differently.
            var formattedValue = html.DisplayValueImpl(meta, formatString);
            return formattedValue ?? html.DisplayText(expression);
        }

        /// <summary>
        /// Same as the built in DisplayTextFor extension method except it will take into consideration the DisplayFormatAttribute if one exists.
        /// </summary>
        public static IHtmlString DisplayValueFor<TModel, TResult>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TResult>> expression, string formatString = null)
        {
            var meta = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

            // Defer to the regular method call if any of the other formatting failed.
            // The built in method does things differently.
            var formattedValue = html.DisplayValueImpl(meta, formatString);
            return formattedValue ?? html.DisplayTextFor(expression);
        }

        #endregion

        #region EditorWithoutWrapperFor

        public static IHtmlString EditorWithoutWrapperFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            // NOTE: There is no overload for this to pass additional ViewData because apparently
            //       EditorFor will only accept an anonymous type for additional ViewData. Attempting
            //       to pass in a RouteValueDictionary ends up not passing any values at all. 
            return html.EditorFor(expression, new {IncludeWrapperHtml = false, html = htmlAttributes});
        }

        #endregion

        #region Grid

        public static Grid Grid(this HtmlHelper helper, object htmlAttributes = null)
        {
            var htmlDict = AnonymousObjectToHtmlAttributes(htmlAttributes);

            return new Grid(helper.ViewContext, helper.ViewDataContainer) {
                HtmlAttributes = htmlDict
            };
        }

        #endregion

        #region HtmlHelperFor

        // These extensions are from https://gist.github.com/maxtoroq/4521457

        public static HtmlHelper<TModel> HtmlHelperFor<TModel>(this HtmlHelper htmlHelper)
        {
            return HtmlHelperFor(htmlHelper, default(TModel));
        }

        public static HtmlHelper<TModel> HtmlHelperFor<TModel>(this HtmlHelper htmlHelper, TModel model)
        {
            return HtmlHelperFor(htmlHelper, model, null);
        }

        public static HtmlHelper<TModel> HtmlHelperFor<TModel>(this HtmlHelper htmlHelper, TModel model,
            string htmlFieldPrefix)
        {
            var newViewData = new ViewDataDictionary(htmlHelper.ViewData) {Model = model};

            newViewData.TemplateInfo = new TemplateInfo {
                // I don't even remotely understand why this is doing this.
                HtmlFieldPrefix = newViewData.TemplateInfo.HtmlFieldPrefix
            };

            var viewDataContainer = new ViewDataContainer {
                ViewData = newViewData
            };

            var templateInfo = viewDataContainer.ViewData.TemplateInfo;

            if (!string.IsNullOrEmpty(htmlFieldPrefix))
            {
                templateInfo.HtmlFieldPrefix = templateInfo.GetFullHtmlFieldName(htmlFieldPrefix);
            }

            var newViewContext = new ViewContextWrapper(htmlHelper.ViewContext, viewDataContainer.ViewData);
            return new HtmlHelper<TModel>(newViewContext, viewDataContainer, htmlHelper.RouteCollection);
        }

        private class ViewDataContainer : IViewDataContainer
        {
            #region Properties

            public ViewDataDictionary ViewData { get; set; }

            #endregion
        }

        public class ViewDataContainerWrapper : IViewDataContainer
        {
            #region Properties

            public ViewDataDictionary ViewData { get; set; }

            public IViewDataContainer ParentContainer { get; set; }

            #endregion
        }

        // This is specifically for the HelperFor extension where it needs to create
        // a new ViewContext when creating an HtmlHelper from a model of a different type
        // than the current HtmlHelper. All this does is act as a way of swapping out the
        // ViewContext.Writer property of the original HtmlHelper instead of the new one, which
        // would cause calls to Html.Whatever() to break inside Func<object, HelperResult> stuff.
        private class ViewContextWrapper : ViewContext
        {
            #region Private Members

            private readonly ViewContext _parentViewContext;

            #endregion

            #region Properties

            public override TextWriter Writer
            {
                get { return _parentViewContext.Writer; }
                set { _parentViewContext.Writer = value; }
            }

            #endregion

            #region Constructors

            public ViewContextWrapper(ViewContext parentViewContext, ViewDataDictionary viewData)
            {
                _parentViewContext = parentViewContext;

                Controller = _parentViewContext.Controller;
                RequestContext = _parentViewContext.RequestContext;
                View = _parentViewContext.View;
                ViewData = viewData;
                TempData = _parentViewContext.TempData;
            }

            #endregion
        }

        #endregion

        #region Link

        /// <summary>
        /// Generates an anchor tag for a given url.
        /// </summary>
        public static MvcHtmlString Link(this HtmlHelper helper, [PathReference] string url, string text = null,
            object htmlAttributes = null, bool wrapLinkTextInSpan = false, Func<string, string> urlFunc = null)
        {
            url.ThrowIfNullOrWhiteSpace("url");
            urlFunc = urlFunc ?? helper.UrlFor;

            url = urlFunc(url);
            text = text ?? url;
            //htmlAttributes = htmlAttributes ?? new { };
            var builder = new TagBuilder("a");
            builder.MergeAttribute("href", url);
            builder.MergeAttributes(AnonymousObjectToHtmlAttributes(htmlAttributes));
            if (!wrapLinkTextInSpan)
            {
                builder.SetInnerText(text);
            }
            else
            {
                var span = new TagBuilder("span");
                span.SetInnerText(text);
                builder.InnerHtml = span.ToString();
            }

            return new MvcHtmlString(builder.ToString());
        }

        #endregion

        #region LinkButton

        public static IHtmlString LinkButton(this HtmlHelper helper, string linkText,
            [AspMvcAction] string actionName,
            [AspMvcController] string controllerName, object routeValues = null,
            object htmlAttributes = null)
        {
            var routeValueDict = ConvertToRouteValueDictionary(routeValues);
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);
            var url = urlHelper.Action(actionName, controllerName, routeValueDict);
            return helper.LinkButton(linkText, url, htmlAttributes);
        }

        public static IHtmlString LinkButton(this HtmlHelper helper, string linkText, string url,
            object htmlAttributes = null)
        {
            const string linkButtonClass = "link-button";
            var htmlAttrDict = AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (htmlAttrDict.ContainsKey("class"))
            {
                // Need that spacer there to separate multiple class names, obviously.
                htmlAttrDict["class"] = htmlAttrDict["class"] + " " + linkButtonClass;
            }
            else
            {
                htmlAttrDict["class"] = linkButtonClass;
            }

            return helper.Link(url, linkText, htmlAttrDict, wrapLinkTextInSpan: true);
        }

        #endregion

        #region ResourceRegistry

        /// <summary>
        /// Returns the ResourceRegistry instance that's shared by a view and any of its partial views.
        /// </summary>
        public static ResourceRegistry ResourceRegistry(this HtmlHelper helper)
        {
            var svd = helper.SharedViewData();
            if (!svd.ContainsKey(RESOURCE_REGISTRY_KEY))
            {
                svd.Add(RESOURCE_REGISTRY_KEY, new ResourceRegistry());
            }

            return (ResourceRegistry)svd[RESOURCE_REGISTRY_KEY];
        }

        #endregion

        #region RenderActionFragment

        public static void RenderActionFragment(this HtmlHelper html, [AspMvcAction] string action,
            [AspMvcController] string controller, object routeValues = null)
        {
            var valuesToPass = new RouteValueDictionary {
                {ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME, ResponseFormatter.KnownExtensions.FRAGMENT}
            }.Merge(routeValues ?? new { });
            html.RenderAction(action, controller, valuesToPass);
        }

        #endregion

        #region RenderDisplayTemplate

        /// <summary>
        /// Renders a label and some field html in the usual DisplayFor template format.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="labelSide">The html for the label side. Can be null.</param>
        /// <param name="fieldSide">The html for the field side. Can not be null. Make an empty HtmlString if you need to.</param>
        /// <param name="cssClasses">Additional css classes to be added to the wrapper div.</param>
        public static IHtmlString RenderDisplayTemplate(this HtmlHelper helper, string labelSide,
            IHtmlString fieldSide, string cssClasses = null)
        {
            var labelSideHtml = (labelSide != null ? new HtmlString(labelSide) : null);
            return helper.RenderDisplayTemplate(labelSideHtml, fieldSide, cssClasses);
        }

        /// <summary>
        /// Renders a label and some field html in the usual DisplayFor template format.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="labelSide">The html for the label side. Can be null.</param>
        /// <param name="fieldSide">The html for the field side. Can not be null. Make an empty HtmlString if you need to.</param>
        /// <param name="cssClasses">Additional css classes to be added to the wrapper div.</param>
        public static IHtmlString RenderDisplayTemplate(this HtmlHelper helper, IHtmlString labelSide,
            IHtmlString fieldSide, string cssClasses = null)
        {
            return helper.RenderDisplayTemplate(labelSide, fieldSide.ToHelperResult(), cssClasses);
        }

        /// <summary>
        /// Renders a label and some field html in the usual DisplayFor template format.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="labelSide">The html for the label side. Can be null.</param>
        /// <param name="fieldSide">The html for the field side. Can not be null. Make an empty HtmlString if you need to.</param>
        /// <param name="cssClasses">Additional css classes to be added to the wrapper div.</param>
        public static IHtmlString RenderDisplayTemplate(this HtmlHelper helper, string labelSide,
            Func<object, HelperResult> fieldSide, string cssClasses = null)
        {
            var labelSideHtml = (labelSide != null ? new HtmlString(labelSide) : null);
            return helper.RenderDisplayTemplate(labelSideHtml, fieldSide, cssClasses);
        }

        /// <summary>
        /// Renders a label and some field html in the usual DisplayFor template format.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="labelSide">The html for the label side. Can be null.</param>
        /// <param name="fieldSide">The html for the field side. Can not be null. Make an empty HtmlString if you need to.</param>
        /// <param name="cssClasses">Additional css classes to be added to the wrapper div.</param>
        public static IHtmlString RenderDisplayTemplate(this HtmlHelper helper, IHtmlString labelSide,
            Func<object, HelperResult> fieldSide, string cssClasses = null)
        {
            var vth = new ViewTemplateHelper();
            var result = vth.RenderDisplayTemplate(labelSide, null, WrapHelperResult(helper, fieldSide), cssClasses);
            return result;
        }

        #endregion

        #region RenderEditorTemplate

        /// <summary>
        /// Renders a label and some field html in the usual EditorFor template format.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="labelSide">The html for the label side. Can be null.</param>
        /// <param name="fieldSide">The html for the field side. Can not be null. Make an empty HtmlString if you need to.</param>
        /// <param name="cssClasses">Additional css classes to be added to the wrapper div.</param>
        public static IHtmlString RenderEditorTemplate(this HtmlHelper helper, string labelSide,
            IHtmlString fieldSide, string cssClasses = null)
        {
            var labelSideHtml = (labelSide != null ? new HtmlString(labelSide) : null);
            return helper.RenderEditorTemplate(labelSideHtml, fieldSide, cssClasses);
        }

        /// <summary>
        /// Renders a label and some field html in the usual EditorFor template format.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="labelSide">The html for the label side. Can be null.</param>
        /// <param name="fieldSide">The html for the field side. Can not be null. Make an empty HtmlString if you need to.</param>
        /// <param name="cssClasses">Additional css classes to be added to the wrapper div.</param>
        public static IHtmlString RenderEditorTemplate(this HtmlHelper helper, IHtmlString labelSide,
            IHtmlString fieldSide, string cssClasses = null)
        {
            return helper.RenderEditorTemplate(labelSide, fieldSide.ToHelperResult(), cssClasses);
        }

        /// <summary>
        /// Renders a label and some field html in the usual EditorFor template format.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="labelSide">The html for the label side. Can be null.</param>
        /// <param name="fieldSide">The html for the field side. Can not be null. Make an empty HtmlString if you need to.</param>
        /// <param name="cssClasses">Additional css classes to be added to the wrapper div.</param>
        public static IHtmlString RenderEditorTemplate(this HtmlHelper helper, string labelSide,
            Func<object, HelperResult> fieldSide, string cssClasses = null)
        {
            var labelSideHtml = (labelSide != null ? new HtmlString(labelSide) : null);
            return helper.RenderEditorTemplate(labelSideHtml, fieldSide, cssClasses);
        }

        /// <summary>
        /// Renders a label and some field html in the usual EditorFor template format.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="labelSide">The html for the label side. Can be null.</param>
        /// <param name="fieldSide">The html for the field side. Can not be null. Make an empty HtmlString if you need to.</param>
        /// <param name="cssClasses">Additional css classes to be added to the wrapper div.</param>
        public static IHtmlString RenderEditorTemplate(this HtmlHelper helper, IHtmlString labelSide,
            Func<object, HelperResult> fieldSide, string cssClasses = null)
        {
            var vth = new ViewTemplateHelper();
            return vth.RenderEditorTemplate(labelSide, null, WrapHelperResult(helper, fieldSide), cssClasses);
        }

        /// <summary>
        /// Renders a label and some field html in the usual EditorFor template format.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="labelSide">The html for the label side. Can be null.</param>
        /// <param name="fieldSide">The html for the field side. Can not be null. Make an empty HtmlString if you need to.</param>
        /// <param name="cssClasses">Additional css classes to be added to the wrapper div.</param>
        public static IHtmlString RenderEditorTemplate(this HtmlHelper helper, IHtmlString labelSide,
            IHtmlString labelDescription,
            Func<object, HelperResult> fieldSide, string cssClasses = null)
        {
            var vth = new ViewTemplateHelper();
            return vth.RenderEditorTemplate(labelSide, labelDescription, WrapHelperResult(helper, fieldSide),
                cssClasses);
        }

        #endregion

        #region RenderScriptModule

        /// <summary>
        /// Renders a script that exports its modules along with a helper function
        /// for importing these exports because we don't have a proper JS build system
        /// and we're dealing with dynamic urls.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="url"></param>
        /// <param name="moduleImportName">The name used to create the import{moduleImportName}Module function.</param>
        /// <returns></returns>
        public static IHtmlString RenderScriptExportModules(this HtmlHelper helper, string url, string moduleImportName)
        {
            // TODO: We'll probably want to completely rewrite this entire method if we ever start
            // importing anything besides the LitComponents. 

            // Scripts.Render adds the proper querystring on to the url for cache busting.
            var initialScript = Scripts.Render(url).ToHtmlString();
            var replacementScript = initialScript.Replace("<script", @"<script type=""module""");

            var finalScriptUrl = Regex.Match(initialScript, @"src=""([^""]+)""").Groups[1];
            var importScriptTag = new TagBuilder("script");
            importScriptTag.InnerHtml = $@"
    async function import{moduleImportName}Module() {{
        const module = await import(""{finalScriptUrl}"");
        return module;
    }};
";
            return new HtmlString(replacementScript + importScriptTag);
        }

        #endregion

        #region ScriptFor

        /// <summary>
        /// Does the same thing as ScriptFor, but loads the script tag as a module instead. This is needed
        /// if any scripts are using import statements.
        ///
        /// NOTE: Global variables created/set within a module only exist within the module. They are not
        /// accessible outside of the module.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="url"></param>
        /// <param name="slt"></param>
        /// <returns></returns>
        public static IHtmlString ScriptModuleFor(this HtmlHelper helper, [PathReference] string url,
            ScriptLoadType slt = ScriptLoadType.LoadNormally)
        {
            const string format = @"<script src=""{0}"" type=""module""></script>";
            url = helper.UrlFor(url);
            var script = new MvcHtmlString(String.Format(format, url));
            return helper.ScriptFor(url, script, slt);
        }

        /// <summary>
        /// Returns a script tag for the given url.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="url">The path to the script file. Can be an http:// url or a ~/ virtual path.</param>
        /// <param name="slt"></param>
        /// <returns></returns>
        public static IHtmlString ScriptFor(this HtmlHelper helper, [PathReference] string url,
            ScriptLoadType slt = ScriptLoadType.LoadNormally)
        {
            const string format = @"<script src=""{0}"" type=""text/javascript""></script>";
            url = helper.UrlFor(url);
            var script = new MvcHtmlString(String.Format(format, url));
            return helper.ScriptFor(url, script, slt);
        }

        public static IHtmlString ScriptFor(this HtmlHelper helper, string url, IHtmlString tag,
            ScriptLoadType slt = ScriptLoadType.LoadNormally)
        {
            switch (slt)
            {
                case ScriptLoadType.LoadNormally:
                    return tag;

                case ScriptLoadType.LoadFromPartial:
                    helper.ResourceRegistry().Scripts.SafeAdd(url, tag);
                    // Return an empty string so we have something to render I guess. Maybe null?
                    return MvcHtmlString.Empty;

                default:
                    throw new NotSupportedException();
            }
        }

        #endregion

        #region InlineScript

        public static void InlineScript(this HtmlHelper helper, string key, string script)
        {
            helper.ResourceRegistry().Scripts.SafeAdd(key, new HtmlString(script));
        }

        #endregion

        #region StylesheetFor

        /// <summary>
        /// Returns a stylesheet link tag for the given url.
        /// </summary>
        public static MvcHtmlString StylesheetFor(this HtmlHelper helper, string url, bool registerForPartial = false)
        {
            const string format = @"<link href=""{0}"" rel=""stylesheet"" type=""text/css"" />";
            url = helper.UrlFor(url);
            var style = new MvcHtmlString(String.Format(format, url));

            if (!registerForPartial)
            {
                return style;
            }

            helper.ResourceRegistry().StyleSheets.Add(url, style);

            // Return an empty string so we have something to render I guess. Maybe null?
            return MvcHtmlString.Empty;
        }

        #endregion

        #region Tabs

        public static TabBuilder Tabs(this HtmlHelper helper, object htmlAttributes = null)
        {
            return new TabBuilder(helper) {HtmlAttributes = htmlAttributes};
        }

        #endregion

        #region TableRowCollectionFor

        public static RazorTableRowCollection<TValue> RowCollectionFor<TValue>(this HtmlHelper<TValue> htmlHelper,
            object htmlAttributes = null)
        {
            return new RazorTableRowCollection<TValue>(htmlHelper.ViewData.Model, htmlAttributes) {
                HtmlHelper = htmlHelper
            };
        }

        #endregion

        #region TableFor

        /// <summary>
        /// Returns a RazorTable for a given mode property that implements IEnumerable T.
        /// </summary>
        public static RazorTable<TValue> TableFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, IEnumerable<TValue>>> expression, object htmlAttributes = null)
        {
            var modelProp = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var model = (IEnumerable<TValue>)modelProp.Model;
            var childHelper = htmlHelper.HtmlHelperFor(model);
            return new RazorTable<TValue>(model, htmlAttributes) {HtmlHelper = childHelper};
        }

        /// <summary>
        /// Returns a RazorTable for the HtmlHelper's current model.
        /// </summary>
        /// <returns></returns>
        public static RazorTable<TValue> TableFor<TValue>(this HtmlHelper<IEnumerable<TValue>> htmlHelper,
            object htmlAttributes = null)
        {
            return new RazorTable<TValue>(htmlHelper.ViewData.Model, htmlAttributes) {HtmlHelper = htmlHelper};
        }

        /// <summary>
        /// Creates a RazorTable for a model property that implements ISearchSet.
        /// </summary>
        public static RazorTable<TValue> TableFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, ISearchSet<TValue>>> expression, object htmlAttributes = null)
        {
            var modelProp = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var model = (ISearchSet<TValue>)modelProp.Model;
            var childHelper = htmlHelper.HtmlHelperFor(model.Results);
            return new RazorTable<TValue>(model, htmlAttributes) {HtmlHelper = childHelper};
        }

        #endregion

        #region TrackingNumberLink

        public static IHtmlString TrackingNumberLink(this HtmlHelper helper, string trackingNumber)
        {
            if (string.IsNullOrWhiteSpace(trackingNumber))
            {
                return new HtmlString("n/a");
            }

            return trackingNumber.Contains(" ")
                ? new HtmlString(trackingNumber)
                : helper.Link(String.Format(USPS_LINK_FORMAT, trackingNumber), trackingNumber);
        }

        #endregion

        #region UrlFor

        public static String UrlFor(this HtmlHelper helper, [PathReference] string url)
        {
            return UrlHelper.GenerateContentUrl(url, helper.ViewContext.HttpContext);
        }

        [StringFormatMethod("urlFormat")]
        public static String UrlFor(this HtmlHelper helper, string urlFormat, params object[] args)
        {
            return helper.UrlFor(String.Format(urlFormat, args));
        }

        #endregion

        #region ViewExists

        /// <summary>
        /// Returns true if a view exists. 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public static bool ViewExists(this HtmlHelper helper, string viewName)
        {
            return ViewEngines.Engines.FindView(helper.ViewContext, viewName, null).View != null;
        }

        #endregion

        /// <summary>
        /// Returns the controller's ViewData object that is shared by all views/partial views.
        /// </summary>
        /// <remarks>
        /// 
        /// Usually the ViewData property on a Controller/View/Anything with a ViewData
        /// property creates a new ViewDataDictionary instance, sometimes copying the values
        /// of another dictionary to the new one, sometimes not. The only time we have a shared
        /// ViewDataDictionary instance between controllers and views is the ViewData instance
        /// on the Controller. 
        /// 
        /// </remarks>
        public static ViewDataDictionary SharedViewData(this HtmlHelper helper)
        {
            var viewContext = helper.ViewContext;
            while (viewContext.IsChildAction)
            {
                viewContext = viewContext.ParentActionViewContext;
            }

            return viewContext.Controller.ViewData;
        }

        #endregion
    }

    public enum ScriptLoadType
    {
        /// <summary>
        /// The script tag should be rendered with the flow of the page.
        /// ie: The script tag should be rendered in the same exact spot as Html.ScriptFor.
        /// </summary>
        LoadNormally,

        /// <summary>
        /// The script tag is being loaded from a partial view and needs to be loaded after
        /// the main layout scripts. Use this only if the partial view is being loaded in a
        /// non-ajax scenario.
        /// </summary>
        LoadFromPartial
    }
}
