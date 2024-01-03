using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Utilities;

namespace MMSINC.Helpers
{
    public class CrumbBuilder
    {
        #region Consts

        private struct CrudActions
        {
            // NOTE: These need to be in all caps due to case sensitivity
            public const string INDEX = "Index",
                                NEW = "New",
                                CREATE = "Create",
                                SHOW = "Show",
                                EDIT = "Edit",
                                UPDATE = "Update",
                                DELETE = "Delete",
                                SEARCH = "Search";
        }

        private static readonly Dictionary<string, string> _insensitiveCrudActions;

        #endregion

        #region Fields

        private readonly RouteContext _routeContext;
        private readonly List<Crumb> _crumbs = new List<Crumb>();

        #endregion

        #region Properties

        /// <summary>
        /// The default property on a model that should be used for its link text.
        /// </summary>
        public string DefaultPropertyForShowText { get; set; }

        public object Model { get; set; }

        /// <summary>
        /// Gets/sets the string used to separate each crumb.
        /// </summary>
        public string Separator { get; set; }

        /// <summary>
        /// Gets/sets the string used to separate each crumb when html isn't allowed. ie: inside title tags.
        /// </summary>
        public string TextOnlySeparator { get; set; }

        #endregion

        #region Constructors

        static CrumbBuilder()
        {
            _insensitiveCrudActions = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var field in typeof(CrudActions).GetFields())
            {
                var rawValue = field.GetRawConstantValue().ToString();
                _insensitiveCrudActions.Add(rawValue, rawValue);
            }
        }

        /// <summary>
        /// Creates a new CrumbBuilder for the given RouteContext.
        /// </summary>
        public CrumbBuilder(RouteContext routeContext, object model = null)
        {
            _routeContext = routeContext;
            Model = model;
        }

        public CrumbBuilder(RequestContext requestContext, object model = null) : this(new RouteContext(requestContext),
            model) { }

        /// <summary>
        /// Creates a BreadCrumbsCollection and populates the Crumbs based on
        /// the ViewContext's Route/Action/Whatever.
        /// </summary>
        public CrumbBuilder(ControllerContext viewContext, object model = null) : this(viewContext.RequestContext,
            model) { }

        #endregion

        #region Private Methods

        private static string GetComparableCrudActionName(RouteContext context)
        {
            // might need a null check on ActionDescriptor.
            var crumbAttr = context.ActionDescriptor.GetCustomAttributes(true).OfType<CrumbAttribute>()
                                   .SingleOrDefault();
            var actionName = crumbAttr?.Action ?? context.ActionName;

            if (!string.IsNullOrWhiteSpace(actionName))
            {
                actionName = actionName.Trim();

                if (_insensitiveCrudActions.ContainsKey(actionName))
                {
                    return _insensitiveCrudActions[actionName];
                }
            }

            return actionName;
        }

        private string GetLinkTextFromModel(object model, string defaultLinkText)
        {
            if (model != null)
            {
                var modelText = model.ToString();

                if (!string.IsNullOrWhiteSpace(modelText) && modelText != model.GetType().FullName)
                {
                    return modelText;
                }

                if (!string.IsNullOrWhiteSpace(DefaultPropertyForShowText) &&
                    model.HasPublicProperty(DefaultPropertyForShowText))
                {
                    var propText = Convert.ToString(model.GetPropertyValueByName(DefaultPropertyForShowText));
                    if (!string.IsNullOrWhiteSpace(propText))
                    {
                        return propText;
                    }
                }
            }

            return defaultLinkText;
        }

        private string GetSeparator(HtmlHelper helper, bool textOnly)
        {
            if (!textOnly)
            {
                return Separator;
            }

            return helper.Encode(TextOnlySeparator);
        }

        private HtmlString Render(HtmlHelper helper, bool textOnly)
        {
            if (!_crumbs.Any())
            {
                CreateCrumbsFromRouteContext(_routeContext);

                if (!_crumbs.Any())
                {
                    // Rather than crash the site, don't render any crumbs.
                    return new HtmlString(string.Empty);
                }
            }

            var sb = new StringBuilder();
            var sep = GetSeparator(helper, textOnly);
            var lastCrumb = _crumbs.Last();
            foreach (var c in _crumbs)
            {
                // NOTE: When calling ActionLink for the same controller and action as the
                // current request, ActionLink will automatically add in the additional route
                // values if they aren't passed in as a parameter. Not sure if this will
                // cause a problem.

                sb.Append(c.Render(helper, textOnly));
                if (c != lastCrumb)
                {
                    sb.Append(sep);
                }
            }

            return new HtmlString(sb.ToString());
        }

        #endregion

        #region Public Methods

        public CrumbBuilder WithLinkCrumb(string linkText, string controllerName, string action,
            object routeValues = null, object htmlAttributes = null)
        {
            linkText.ThrowIfNullOrWhiteSpace("linkText");
            _crumbs.Add(new LinkCrumb {
                Text = linkText,
                HtmlAttributes = htmlAttributes,
                ControllerName = controllerName,
                Action = action,
                RouteValues = routeValues
            });
            return this;
        }

        public CrumbBuilder WithTextCrumb(string text, object htmlAttributes = null)
        {
            _crumbs.Add(new TextCrumb {Text = text, HtmlAttributes = htmlAttributes});
            return this;
        }

        private CrumbBuilder TryAddIndexLink(RouteContext context, string displayControllerName,
            string actualControllerName)
        {
            if (context.ControllerDescriptor.FindReflectedActionDescriptor(CrudActions.INDEX) != null)
            {
                WithLinkCrumb(displayControllerName, actualControllerName, CrudActions.INDEX);
            }
            else
            {
                WithTextCrumb(displayControllerName);
            }

            return this;
        }

        public void CreateCrumbsFromRouteContext(RouteContext context)
        {
            if (context == null)
            {
                return;
            }

            var actualControllerName = context.RouteControllerName;
            var displayControllerName = context.GetDisplayControllerName();
            var actualActionName = GetComparableCrudActionName(context);
            // These crumbs are setup to follow our general CRUD methods
            // for controllers. This isn't smart enough to understand
            // parent -> child relationships though. It only knows about
            // controller methods.

            // There will almost always be an Index. We can fix this when it becomes a problem.
            // Maybe skip this crumb if the controller doesn't have an Index action.
            var routValues = context.RouteData.Values;

            switch (actualActionName)
            {
                case CrudActions.SEARCH:
                    WithTextCrumb(displayControllerName);
                    break;

                case CrudActions.INDEX:
                    if (context.ControllerDescriptor.FindReflectedActionDescriptor("Search") != null)
                    {
                        WithLinkCrumb(displayControllerName, actualControllerName, CrudActions.SEARCH);
                    }
                    else
                    {
                        WithTextCrumb(displayControllerName);
                    }

                    break;

                case CrudActions.SHOW:
                    TryAddIndexLink(context, displayControllerName, actualControllerName);
                    WithTextCrumb(GetLinkTextFromModel(Model, "Show"));
                    break;

                case CrudActions.NEW:
                case CrudActions.CREATE
                    : // Assuming if there's a Create action with a view, it's a New view with invalid modelstate.
                    TryAddIndexLink(context, displayControllerName, actualControllerName);
                    WithTextCrumb("Creating");
                    break;

                case CrudActions.EDIT:
                case CrudActions.UPDATE
                    : // Assuming that if there's an Update action with a view, that it's an Edit view with invalid modelstate.
                    TryAddIndexLink(context, displayControllerName, actualControllerName);
                    WithLinkCrumb(GetLinkTextFromModel(Model, "Show"), actualControllerName, CrudActions.SHOW,
                        routValues);
                    // Should be TextCrumb
                    WithTextCrumb("Editing");
                    break;
            }
        }

        public HtmlString ToHtmlString(HtmlHelper helper)
        {
            return Render(helper, false);
        }

        /// <summary>
        /// Returns only the text parts of each crumb for use in title bars and other things where html is bad.
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public HtmlString ToTextOnlyHtmlString(HtmlHelper helper)
        {
            return Render(helper, true);
        }

        #endregion

        #region Implementation details

        private abstract class Crumb
        {
            #region Properties

            public object HtmlAttributes { get; set; }
            public string Text { get; set; }

            #endregion

            #region Public Methods

            public abstract HtmlString Render(HtmlHelper helper, bool textOnly);

            public IDictionary<string, object> GetHtmlAttributes()
            {
                if (HtmlAttributes == null)
                {
                    return new Dictionary<string, object>();
                }

                if (HtmlAttributes is IDictionary<string, object>)
                {
                    return (IDictionary<string, object>)HtmlAttributes;
                }

                return new RouteValueDictionary(HtmlAttributes);
            }

            #endregion
        }

        private class TextCrumb : Crumb
        {
            public override HtmlString Render(HtmlHelper helper, bool textOnly)
            {
                if (textOnly)
                {
                    return new HtmlString(helper.Encode(Text));
                }

                var tb = new TagBuilder("span");
                tb.MergeAttributes(GetHtmlAttributes());
                tb.SetInnerText(Text);

                return new HtmlString(tb.ToString());
            }
        }

        private class LinkCrumb : Crumb
        {
            public string ControllerName { get; set; }
            public string Action { get; set; }
            public object RouteValues { get; set; }

            public override HtmlString Render(HtmlHelper helper, bool textOnly)
            {
                if (textOnly)
                {
                    return new HtmlString(helper.Encode(Text));
                }

                // NOTE: When calling ActionLink for the same controller and action as the
                // current request, ActionLink will automatically add in the additional route
                // values if they aren't passed in as a parameter. Not sure if this will
                // cause a problem.

                // We DO NOT want the overload that takes routeValues and htmlAttributes as objects,
                // we want the type specific one. So we need to do some casting/creating first.
                // Do not pass a RouteValueDictionary to another RouteValueDictionary, it
                // picks the Dictionary properties to merge instead of the keys in the existing one.
                var routeValueDict = (RouteValues is RouteValueDictionary
                    ? (RouteValueDictionary)RouteValues
                    : new RouteValueDictionary(RouteValues));

                return helper.ActionLink(Text, Action, ControllerName, routeValueDict, GetHtmlAttributes());
            }
        }

        #endregion
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CrumbAttribute : Attribute
    {
        /// <summary>
        /// When crumbs are automatically generated by the route, setting this on an action method 
        /// will tell the CrumbBuilder to assume this is the current action name instead. 
        /// 
        /// ex: ServiceController NewFromWorkOrder acts like the New action.
        /// </summary>
        public string Action { get; set; }
    }
}
