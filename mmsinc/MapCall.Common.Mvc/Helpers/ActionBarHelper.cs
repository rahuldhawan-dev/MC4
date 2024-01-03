using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.WebPages;
using JetBrains.Annotations;
using MapCall.Common.ClassExtensions.MapExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Helpers;
using MMSINC.Results;
using MMSINC.Utilities;
using HtmlHelpers = MMSINC.ClassExtensions.HtmlHelperExtensions;

namespace MapCall.Common.Helpers
{
    // TODO: Add title attribute to links so they're a little more descriptive.

    /// <summary>
    /// Add to an action to set whether or not it should be visible in the action bar ever.
    /// If an action doesn't have one of these, it assumes an action should be visible if the
    /// action exists.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ActionBarVisibleAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// If true, the action this is attached to will be visible in the action bar if the
        /// action bar supports it.
        /// </summary>
        public bool IsVisible { get; private set; }

        #endregion

        #region Constructors

        public ActionBarVisibleAttribute(bool isVisible)
        {
            IsVisible = isVisible;
        }

        #endregion
    }

    /// <summary>
    /// Only exists to make it easier to access the constants.
    /// </summary>
    public abstract class ActionBarHelper : IHtmlString
    {
        #region Constants

        public const string CSS_ICON_CLASS_PREFIX = "ab-",
                            CONFIRM_ATTRIBUTE_NAME = "data-confirm",
                            HELP_VIEW_NAME = "_ActionBarHelp",
                            SHOW_FORMS_VIEW_NAME = "_ActionBarShowForms";

        #endregion

        #region Properties

        /// <summary>
        /// Set this property if you need to override the default confirmation
        /// message that is displayed when a user presses the delete button.
        /// </summary>
        public string DestroyConfirmationMessageOverride { get; set; }

        #endregion

        #region Abstract Properties

        public abstract string CurrentControllerName { get; }

        /// <summary>
        /// Gets/sets whether the buttons for usual CRUD stuff should be added automatically. Default is true.
        /// </summary>
        public abstract bool AutoGenerateCrudLinks { get; set; }

        /// <summary>
        /// Returns true if the current request is for a Show action and the _ActionBarShowForms view exists.
        /// </summary>
        public abstract bool CanDisplayShowActionForms { get; }

        /// <summary>
        /// Returns true if the current request has a help partial view that can be displayed.
        /// </summary>
        public abstract bool CanDisplayHelp { get; }

        public abstract bool CanDestroyModel { get; }

        public abstract bool CanEditModel { get; }

        /// <summary>
        /// Gets/Sets whether or not to hide the Edit button in a Show view when <see cref="AutoGenerateCrudLinks"/> is set to <see langword="true" />
        /// </summary>
        public abstract bool HideEditButton { get; set; }

        #endregion

        #region Abstract Methods

        public abstract void AddAuthorizedLink(string text, string cssIconClass, [AspMvcAction] string action,
            [AspMvcController] string controller, object routeValues, object htmlAttributes, string toolTip = null);

        public abstract void AddLink(string text, string cssIconClass, [AspMvcAction] string action,
            [AspMvcController] string controller, object routeValues, object htmlAttributes, string toolTip = null);

        public abstract void AddSearchIndexLinks(string currentActionName);
        /// <summary>
        /// Adds the appropriate form + button to delete a record if the user is authorized to do so, the
        /// controller has as destroy action, and the controller's destroy action doesn't have a
        /// <see cref="ActionBarVisibleAttribute"/> with "false".
        /// </summary>
        public abstract void AddDeleteButton();
        /// <inheritdoc cref="AddDeleteButton()"/>
        /// <remarks>
        /// If <paramref name="skipAttributeCheck"/> is true, the button will be rendered regardless of the
        /// presence of any <see cref="ActionBarVisibleAttribute"/> on the controller's destroy action.
        /// </remarks>
        public abstract void AddDeleteButton(bool skipAttributeCheck);
        public abstract void AddEditLink();
        public abstract void AddExportLink();
        public abstract void AddPdfExportLink();
        public abstract void AddMapLink(object additionalRouteData = null);
        public abstract void AddDisabledMapLink(int maxResult);
        public abstract void AddRTOMapLink();
        public abstract void AddMapCallIntranetLink();
        public abstract void AddPdfLink();
        public abstract void AddHelpLink();
        public abstract void AddFormsLink();
        public abstract void AddNewLink();
        public abstract void AddShowLink();
        public abstract void AddRenewLink(string action, object routeValues = null);
        public abstract void AddCopyButton();
        public abstract void AddReplaceButton();
        public abstract void Clear();
        public abstract string ToHtmlString();

        #endregion
    }

    /// <summary>
    /// Kinda like CrumbBuilder, but it makes all the buttons for the action bar on the right.
    /// </summary>
    public class ActionBarHelper<T> : ActionBarHelper
    {
        #region Constants

        public const string INTRANET_URL_KEY = "IntranetUrl";
        private struct CrudActions
        {
            // NOTE: These need to be in all caps due to case sensitivity

            #region Constants

            public const string INDEX = "Index",
                                NEW = "New",
                                CREATE = "Create",
                                SHOW = "Show",
                                EDIT = "Edit",
                                UPDATE = "Update",
                                DESTROY = "Destroy",
                                SEARCH = "Search",
                                REPLACE = "Replace",
                                COPY = "Copy";

            #endregion
        }

        private static readonly Dictionary<string, string> _insensitiveCrudActions;

        #endregion

        #region Private Members

        private List<Func<IHtmlString>> _buttons = new List<Func<IHtmlString>>();
        private readonly RouteContext _routeContext;
        private readonly IRoleService _roleService;
        private readonly IAuthenticationService<User> _authenticationService;
        private bool _autoCrudInitialized;

        #endregion

        #region Properties

        public HtmlHelper<T> Html { get; private set; }

        public override string CurrentControllerName
        {
            get { return _routeContext.RouteControllerName; }
        }

        /// <summary>
        /// Gets/sets whether the buttons for usual CRUD stuff should be added automatically. Default is true.
        /// </summary>
        public override bool AutoGenerateCrudLinks { get; set; }

        /// <summary>
        /// Returns true if the current request is for a Show action and the _ActionBarShowForms view exists.
        /// </summary>
        public override bool CanDisplayShowActionForms
        {
            get
            {
                var actualActionName = GetComparableCrudActionName(_routeContext.ActionName);
                return (actualActionName == CrudActions.SHOW && Html.ViewExists(SHOW_FORMS_VIEW_NAME));
            }
        }

        /// <summary>
        /// Returns true if the current request has a help partial view that can be displayed.
        /// </summary>
        public override bool CanDisplayHelp
        {
            get { return Html.ViewExists(HELP_VIEW_NAME); }
        }

        public override bool CanDestroyModel
        {
            get
            {
                var model = Html.ViewData.Model as IThingWithOperatingCenter;

                return model == null || Html.CurrentUserCanDestroy(_roleService, _authenticationService.CurrentUser,
                    model, Html.ViewContext.RequestContext);
            }
        }

        public override bool CanEditModel
        {
            get
            {
                var model = Html.ViewData.Model as IThingWithOperatingCenter;

                return !HideEditButton && 
                       (model == null || 
                        Html.CurrentUserCanEdit(_roleService, _authenticationService.CurrentUser, model, Html.ViewContext.RequestContext));
            }
        }

        public override bool HideEditButton { get; set; }

        #endregion

        #region Constructors

        static ActionBarHelper()
        {
            _insensitiveCrudActions = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var field in typeof(CrudActions).GetFields())
            {
                var rawValue = field.GetRawConstantValue().ToString();
                _insensitiveCrudActions.Add(rawValue, rawValue);
            }
        }

        public ActionBarHelper(HtmlHelper<T> htmlHelper, IRoleService roleService,
            IAuthenticationService<User> authenticationService)
        {
            htmlHelper.ThrowIfNull("htmlHelper");
            Html = htmlHelper;
            _roleService = roleService;
            _authenticationService = authenticationService;
            AutoGenerateCrudLinks = true;

            // TODO ENORMOUSLY: RouteContext needs to be allowed to be created from a Controller instance.
            //             It'd bypass a ton of stuff and make RouteContext objects cheap to create in a lot of spots.

            _routeContext = new RouteContext(Html.ViewContext);
        }

        #endregion

        #region Private Methods

        private static string GetComparableCrudActionName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();

                if (_insensitiveCrudActions.ContainsKey(name))
                {
                    return _insensitiveCrudActions[name];
                }
            }

            return name;
        }

        private void InitCrud()
        {
            if (!AutoGenerateCrudLinks || _autoCrudInitialized)
            {
                return;
            }

            // Swap out the lists so we can inject all the crud stuff at the start.
            var currentButtons = _buttons;
            _buttons = new List<Func<IHtmlString>>();

            var actualActionName = GetComparableCrudActionName(_routeContext.ActionName);
            // First add stuff that's probably always visible.
            if (CanDisplayHelp)
            {
                AddHelpLink();
            }

            AddSearchIndexLinks(actualActionName);
            AddNewLink();

            switch (actualActionName)
            {
                case CrudActions.SHOW:
                    if (CanEditModel)
                    {
                        AddEditLink();
                    }

                    if (CanDestroyModel)
                    {
                        AddDeleteButton();
                    }

                    if (CanDisplayShowActionForms)
                    {
                        AddFormsLink();
                    }

                    break;

                case CrudActions.EDIT:
                    AddShowLink();
                    break;
            }

            _buttons.AddRange(currentButtons);
            _autoCrudInitialized = true;
        }

        private bool ActionExists(string actionName, out ReflectedActionDescriptor action)
        {
            action = _routeContext.ControllerDescriptor.FindReflectedActionDescriptor(actionName);

            return action != null;
        }

        private bool ActionExistsAndCanBeDisplayed(string actionName)
        {
            if (!ActionExists(actionName, out var action))
            {
                return false;
            }

            var attr = action.GetCustomAttributes(typeof(ActionBarVisibleAttribute), true)
                             .Cast<ActionBarVisibleAttribute>().SingleOrDefault();
            if (attr == null)
            {
                return true;
            }

            return attr.IsVisible;
        }

        private void AddAuthorizedCrudLink(string text, string action, string cssIconClass = null,
            RouteValueDictionary routeValues = null, string toolTip = null)
        {
            if (ActionExistsAndCanBeDisplayed(action))
            {
                var css = CSS_ICON_CLASS_PREFIX + (cssIconClass ?? action.Trim().ToLowerInvariant());
                AddAuthorizedLink(text, css, action, CurrentControllerName, routeValues, null, toolTip);
            }
        }

        private void AddLinkCore(bool mustAuthorize, string text, string cssIconClass, string action, string controller,
            object routeValues, object htmlAttributes, string toolTip)
        {
            var routeValueDict = HtmlHelpers.ConvertToRouteValueDictionary(routeValues);
            var html = HtmlHelpers.AnonymousObjectToHtmlAttributes(htmlAttributes);
            html["class"] = cssIconClass;

            if (!string.IsNullOrWhiteSpace(toolTip))
            {
                html["title"] = toolTip;
            }

            if (mustAuthorize)
            {
                // TODO: Might want AuthorizedActionLink to have an overload that creates a url from a RouteContext. That way the
                //       same one can be used over and over since RouteContexts aren't super cheap.
                _buttons.Add(() => Html.AuthorizedActionLink(text, action, controller, routeValueDict, html));
            }
            else
            {
                _buttons.Add(() => Html.ActionLink(text, action, controller, routeValueDict, html));
            }
        }

        private string GetPrependedCssClassName(string cssIconSpecificClass)
        {
            return string.Format("{0}{1}", CSS_ICON_CLASS_PREFIX, cssIconSpecificClass);
        }

        private void AddExportLink(string ext, string label, string toolTip, string crudAction)
        {
            var rvd = Html.ViewData.ModelState.ToRouteValueDictionary();
            rvd.Add(ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME, ext);
            AddAuthorizedCrudLink(label, crudAction, "export", rvd, toolTip);
        }

        private void AddCopyOrReplaceButton(string copyOrReplace)
        {
            if (ActionExistsAndCanBeDisplayed(copyOrReplace) &&
                Html.CurrentUserCanDo(copyOrReplace, CurrentControllerName))
            {
                var lower = copyOrReplace.ToLower();

                Func<object, HelperResult> helperResult = (obj) => new HelperResult((writer) => {
                    var toolTip = copyOrReplace + " this " + CurrentControllerName;

                    using (var f = new FormBuilder<T>(Html))
                    {
                        f.Action = copyOrReplace;
                        f.Controller = CurrentControllerName;
                        f.Area = _routeContext.AreaName;
                        f.MergeRouteValues(_routeContext.RouteData.Values);

                        // TODO: add pretty name for controller in there.
                        f.HtmlAttributes[CONFIRM_ATTRIBUTE_NAME] = $"Are you sure you want to {lower} this record?";

                        var subButt = new ButtonBuilder()
                                     .AsType(ButtonType.Submit)
                                     .WithText(copyOrReplace)
                                     .WithCssClass(GetPrependedCssClassName("replace"))
                                     .With("title", toolTip);
                        f.Write(subButt);

                        writer.Write(f.ToHtmlString());
                    }
                });

                helperResult = HtmlHelpers.WrapHelperResult(Html, helperResult);
                _buttons.Add(() => helperResult(null));
            }
        }

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Adds a link to the action bar that is displayed if the user is authorized to view it. Shouldn't need this
        /// except for one-off links.
        /// </summary>
        public override void AddAuthorizedLink(string text, string cssIconClass, [AspMvcAction] string action,
            [AspMvcController] string controller, object routeValues, object htmlAttributes, string toolTip = null)
        {
            AddLinkCore(true, text, cssIconClass, action, controller, routeValues, htmlAttributes, toolTip);
        }

        /// <summary>
        /// Adds a link to the action bar that doesn't check authorization. You probably don't wanna use this.
        /// </summary>
        public override void AddLink(string text, string cssIconClass, [AspMvcAction] string action,
            [AspMvcController] string controller, object routeValues, object htmlAttributes, string toolTip = null)
        {
            AddLinkCore(false, text, cssIconClass, action, controller, routeValues, htmlAttributes, toolTip);
        }

        /// <summary>
        /// Adds the appropriate search and index links based on the controller and current action.
        /// </summary>
        /// <param name="currentActionName"></param>
        public override void AddSearchIndexLinks(string currentActionName)
        {
            var searchToolTip = "Search for " + CurrentControllerName.Pluralize();
            AddAuthorizedCrudLink("Search", CrudActions.SEARCH, null, null, searchToolTip);

            if (currentActionName != CrudActions.SEARCH && currentActionName != CrudActions.INDEX)
            {
                var indexToolTip = "View all " + CurrentControllerName.Pluralize();
                AddAuthorizedCrudLink("Index", CrudActions.INDEX, null, null, indexToolTip);
            }
        }

        public override void AddDeleteButton(bool skipAttributeCheck)
        {
            if ((skipAttributeCheck && !ActionExists(CrudActions.DESTROY, out _)) ||
                (!skipAttributeCheck && !ActionExistsAndCanBeDisplayed(CrudActions.DESTROY)) ||
                !Html.CurrentUserCanDo(CrudActions.DESTROY, CurrentControllerName))
            {
                return;
            }

            Func<object, HelperResult> helperResult = (obj) => new HelperResult((writer) => {
                var toolTip = "Delete this " + CurrentControllerName;

                using (var f = new FormBuilder<T>(Html))
                {
                    f.Action = CrudActions.DESTROY;
                    f.Controller = CurrentControllerName;
                    f.Area = _routeContext.AreaName;
                    f.MergeRouteValues(_routeContext.RouteData.Values);

                    var confirmationMessage = DestroyConfirmationMessageOverride;
                    if (string.IsNullOrWhiteSpace(confirmationMessage))
                    {
                        var prettyControllerName = Wordify.SpaceOutWordsFromCamelCase(CurrentControllerName);
                        confirmationMessage = $"Are you sure you want to delete this {prettyControllerName} record?";
                    }

                    f.HtmlAttributes[CONFIRM_ATTRIBUTE_NAME] = confirmationMessage;

                    var subButt = new ButtonBuilder()
                                 .AsType(ButtonType.Submit)
                                 .WithText("Delete")
                                 .WithCssClass(GetPrependedCssClassName("destroy"))
                                 .With("title", toolTip);
                    f.Write(subButt);

                    writer.Write(f.ToHtmlString());
                }
            });

            helperResult = HtmlHelpers.WrapHelperResult(Html, helperResult);
            _buttons.Add(() => helperResult(null));
        }

        public override void AddDeleteButton()
        {
            AddDeleteButton(false);
        }

        public override void AddCopyButton()
        {
            AddCopyOrReplaceButton(CrudActions.COPY);
        }

        /// <summary>
        /// Adds the appropriate form + button to call the replace action on a controller.
        /// </summary>
        public override void AddReplaceButton()
        {
            AddCopyOrReplaceButton(CrudActions.REPLACE);
        }

        /// <summary>
        /// Adds a link to the Edit action of the current controller if there's an available record and if the user is authorized to. 
        /// </summary>
        public override void AddEditLink()
        {
            var toolTip = "Edit this " + CurrentControllerName;
            AddAuthorizedCrudLink("Edit", CrudActions.EDIT, null, _routeContext.RouteData.Values, toolTip);
        }

        /// <summary>
        /// Adds a link to the Export(Index.xls) action of the current controller, if the user is authorized to. 
        /// </summary>
        public override void AddExportLink()
        {
            AddExportLink(ResponseFormatter.KnownExtensions.EXCEL_2003, "Export", "Export to Excel", CrudActions.INDEX);
        }

        /// <summary>
        /// Adds a link to the Export(Index.pdf) action of the current controller, if the user is authorized to. 
        /// </summary>
        public override void AddPdfExportLink()
        {
            AddExportLink(ResponseFormatter.KnownExtensions.PDF, "PDF", "Export to Pdf", CrudActions.INDEX);
        }

        /// <summary>
        /// Adds a link to the Map controller for the results of the current action.
        /// </summary>
        public override void AddMapLink(object additionalRouteData = null)
        {
            var html = new RouteValueDictionary {
                {"class", GetPrependedCssClassName("map")}
            };
            _buttons.Add(() => Html.MapLink("Map", html, null, additionalRouteData));
        }

        public override void AddDisabledMapLink(int maxResult)
        {
            var message =
                $"Current search returns over {maxResult} items. Refine the search further to enable " +
                "mapping.";
            var html = new RouteValueDictionary {
                {"class", GetPrependedCssClassName("map")},
                {"onclick", $"alert('{message}'); return false"},
                {"title", message}
            };
            _buttons.Add(() => Html.MapLink("Map", html));
        }

        /// <summary>
        /// Adds a link to the RealTimeOperations page on old MapCall.
        /// </summary>
        public override void AddRTOMapLink()
        {
            var html = new RouteValueDictionary {
                {"class", GetPrependedCssClassName("map")}
            };
            _buttons.Add(() => Html.Link("/Modules/Maps/RealTimeOperations.aspx", "Map", html));
        }

        /// <summary>
        /// Adds a link to the MapCall Intranet - opens in a new tab.
        /// </summary>
        public override void AddMapCallIntranetLink()
        {
            _buttons.Add(() => Html.Link(ConfigurationManager.AppSettings[INTRANET_URL_KEY], "New", new {
                @class = GetPrependedCssClassName("new"), 
                target = "_blank"
            }));
        }

        public override void AddPdfLink()
        {
            var toolTip = "Download PDF";
            var routeData = new RouteValueDictionary(_routeContext.RouteData.Values);
            routeData.Add("ext", ResponseFormatter.KnownExtensions.PDF);
            AddAuthorizedLink("PDF", "ab-export", CrudActions.SHOW, CurrentControllerName, routeData, null, toolTip);
        }

        public override void AddHelpLink()
        {
            Func<object, HelperResult> helperResult = (obj) => new HelperResult((writer) => {
                writer.Write(new ButtonBuilder().WithText("Help").WithCssClass(GetPrependedCssClassName("help")));
            });

            helperResult = HtmlHelpers.WrapHelperResult(Html, helperResult);
            _buttons.Add(() => helperResult(null));
        }

        /// <summary>
        /// Adds a link to that action bar for displaying different form downloads and junk.
        /// </summary>
        public override void AddFormsLink()
        {
            Func<object, HelperResult> helperResult = (obj) => new HelperResult((writer) => {
                writer.Write(new ButtonBuilder().WithText("Forms").WithCssClass(GetPrependedCssClassName("forms")));
            });

            helperResult = HtmlHelpers.WrapHelperResult(Html, helperResult);
            _buttons.Add(() => helperResult(null));
        }

        /// <summary>
        /// Adds a link to the New action of the current controller, if the user is authorized to. 
        /// </summary>
        public override void AddNewLink()
        {
            var toolTip = "Add new " + CurrentControllerName;
            AddAuthorizedCrudLink("Add", CrudActions.NEW, null, null, toolTip);
        }

        /// <summary>
        /// Adds a link to the Show action of the current controller if there's an available record and if the user is authorized to. 
        /// </summary>
        public override void AddShowLink()
        {
            var toolTip = "View this " + CurrentControllerName;
            AddAuthorizedCrudLink("Show", CrudActions.SHOW, null, _routeContext.RouteData.Values, toolTip);
        }

        /// <summary>
        /// Adds a link to a page and uses the ab-renew icon.
        /// </summary>
        public override void AddRenewLink(string action, object routeValues = null)
        {
            var routeValueDict = HtmlHelpers.ConvertToRouteValueDictionary(routeValues);
            AddAuthorizedCrudLink("Renew", action, "renew", routeValueDict);
        }

        /// <summary>
        /// Clears all the queued up button renderings.
        /// </summary>
        public override void Clear()
        {
            _buttons.Clear();
        }

        public override string ToHtmlString()
        {
            var sb = new StringBuilder();
            InitCrud();

            foreach (var fn in _buttons)
            {
                sb.Append(fn().ToHtmlString());
            }

            return sb.ToString();
        }

        #endregion
    }
}
