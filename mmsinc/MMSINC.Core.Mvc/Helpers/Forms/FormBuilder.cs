using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;
using MMSINC.ClassExtensions;
using MMSINC.Metadata;
using MMSINC.Utilities;
using StructureMap;

namespace MMSINC.Helpers
{
    // TODO: Will probably want to merge in the Ajax.BeginForm stuff here too.

    /// <summary>
    /// Replacement for mvc's form generator.
    /// </summary>
    public class FormBuilder : IHtmlString, IDisposable
    {
        #region Constants

        protected const string FORM_STATE_CSS_CLASS = "has-form-state",
                               NO_FORM_STATE_CSS_CLASS =
                                   "no-form-state", // This gets added to individual inputs, not the form itself.
                               DATA_CONFIRM_KEY = "data-confirm";

        public const string SECURE_FORM_HIDDEN_FIELD_NAME = "__SECUREFORM",
                            NO_DOUBLE_SUBMIT_CSS_CLASS = "no-double-submit";

        #endregion

        #region Private Members

        private readonly TagBuilder _formTag = new TagBuilder("form");
        private readonly RouteValueDictionary _routeData = new RouteValueDictionary();
        private readonly ViewContextWriterWrapper _viewContextWrapper;
        private readonly HtmlHelper _htmlHelper;
        private string _renderedForm;
        private bool _isDisposed;

        #endregion

        #region Properties

        /// <summary>
        /// Gets/sets whether SecureForms are enabled for a site. Permits/Contractors
        /// does not support secure forms. True by default.
        /// </summary>
        public static bool SecureFormsEnabled { get; set; }

        public string Action { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }

        /// <summary>
        /// Gets/sets the form method(get/post). If null, this will be found dynamically if possible.
        /// </summary>
        public FormMethod? Method { get; set; }

        /// <summary>
        /// Set this if a form's url needs to be generated using a specific named route.
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// Collection of html attributes that will be rendered on the form tag.
        /// </summary>
        public IDictionary<string, string> HtmlAttributes
        {
            get { return _formTag.Attributes; }
        }

        public RouteValueDictionary RouteData
        {
            get { return _routeData; }
        }

        /// <summary>
        /// When set, renders the form the as an ajax form. You can use an empty instance
        /// of AjaxOptions if you don't need to set anything special on the object(HttpMethod
        /// defaults to FormBuilder.Method, and InsertionMode defaults to Replace).
        /// </summary>
        public AjaxOptions Ajax { get; set; }

        /// <summary>
        /// Set to true if you're building an ajax form and the content will have
        /// initially loaded during page rendering. This prevents an ajax tab from
        /// firing off an initial form submission when it loads.
        /// </summary>
        public bool IsAjaxContentPreloaded { get; set; }

        /// <summary>
        /// If set, presents a confirmation dialog to the user when they submit a form.
        /// </summary>
        public string Confirmation
        {
            get
            {
                if (HtmlAttributes.ContainsKey(DATA_CONFIRM_KEY))
                {
                    return HtmlAttributes[DATA_CONFIRM_KEY];
                }

                return null;
            }
            set { HtmlAttributes[DATA_CONFIRM_KEY] = value; }
        }

        protected ViewContext ViewContext
        {
            get { return _htmlHelper.ViewContext; }
        }

        #endregion

        #region Constructors

        static FormBuilder()
        {
            SecureFormsEnabled = true;
        }

        public FormBuilder(HtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
            _viewContextWrapper = new ViewContextWriterWrapper(htmlHelper.ViewContext, htmlHelper.ViewDataContainer);

            // ViewContext always returns a default FormContext in the case that
            // of a form being added by raw html rather than an HtmlHelper method.
            // We need to override the FormContext to keep validation working correctly.
            ViewContext.FormContext = new FormContext();
        }

        #endregion

        #region Private Methods

        private static string GetHttpMethodFromVerb(HttpVerbs verb)
        {
            switch (verb)
            {
                case HttpVerbs.Get:
                    return "get";
                default:
                    return "post";
            }
        }

        private HttpVerbs GetHttpVerbForRoute(string url)
        {
            var routeContext = new RouteContext(ViewContext.RequestContext, url);
            return routeContext.ActionDescriptor.GetHttpVerb();
        }

        private HttpVerbs GetHttpVerb(string url)
        {
            // if FormMethod is explicitly set, use that.
            // Otherwise dynamically get FormMethod from
            // the action's attributes.
            if (Method.HasValue)
            {
                switch (Method)
                {
                    case FormMethod.Get:
                        return HttpVerbs.Get;
                    case FormMethod.Post:
                        return HttpVerbs.Post;
                    default:
                        throw new NotSupportedException();
                }
            }

            return GetHttpVerbForRoute(url);
        }

        /// <summary>
        /// Called before actually rendering the Form tag so any properties
        /// or other things that need to be setup before rendering can be setup.
        /// </summary>
        protected virtual void OnRendering()
        {
            // This is here so no one needs to remember to add the css class
            // on any of the BeginForm calls for search forms.
            AddFormStateAttributes();
        }

        private void AddFormStateAttributes()
        {
            const string countKey = "FormBuilder generated id count";
            if (Action == "Index")
            {
                AddCssClass(FORM_STATE_CSS_CLASS);
                if (!HtmlAttributes.ContainsKey("id"))
                {
                    // form-state.js requires that a form has an id to function properly.

                    var httpContext = ViewContext.RequestContext.HttpContext;
                    var lastCount = httpContext.Items[countKey];
                    var newCount = (lastCount != null ? (int)lastCount + 1 : 0);
                    httpContext.Items[countKey] = newCount;

                    HtmlAttributes["id"] = "formState_" + newCount;
                }
            }
        }

        protected static HiddenInputBuilder CreateHiddenField(string name, string value)
        {
            return new HiddenInputBuilder().WithName(name).WithValue(value);
        }

        #endregion

        #region Exposed Methods

        internal string GetUrl()
        {
            // This needs to generate the url based on
            // whether there's a RouteName given.

            var useRouteUrl = !string.IsNullOrEmpty(RouteName);
            // no need to use a secure url helper here, those values are better off
            // in the form as fields
            var urlHelp = new UrlHelper(ViewContext.RequestContext, RouteTable.Routes);
            var rd = new RouteValueDictionary(RouteData);

            // Only set area on RouteData if it isn't null. The UrlHelper
            // will look for the current request's area if one isn't explicitly set.
            // This makes it so the area does not need to be set when forms are
            // built in views inside areas.
            if (Area != null)
            {
                rd["area"] = Area;
            }

            if (useRouteUrl)
            {
                // These need to be added to RouteData if they're set, but
                // only when using a named route.
                if (Controller != null)
                {
                    rd["controller"] = Controller;
                }

                if (Action != null)
                {
                    rd["action"] = Action;
                }

                return urlHelp.RouteUrl(RouteName, rd);
            }
            else
            {
                return urlHelp.Action(Action, Controller, rd);
            }
        }

        /// <summary>
        /// Returns true if a given action requires secure forms.
        /// </summary>
        /// <returns></returns>
        public static bool RequiresSecureForm(RouteContext routeContext)
        {
            return RequiresSecureForm(routeContext.ActionDescriptor);
        }

        /// <summary>
        /// Returns true if a given action requires secure forms.
        /// </summary>
        /// <returns></returns>
        public static bool RequiresSecureForm(ActionDescriptor action)
        {
            if (!SecureFormsEnabled ||
                action.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any())
            {
                return false;
            }

            var reqAttr = (RequiresSecureFormAttribute)action
                                                      .GetCustomAttributes(typeof(RequiresSecureFormAttribute), true)
                                                      .SingleOrDefault();
            if (reqAttr != null)
            {
                return reqAttr.IsRequired;
            }

            // Every other http method is assumed as post-like and requires a secure form by default.
            return (action.GetHttpVerb() != HttpVerbs.Get);
        }

        /// <summary>
        /// Adds a css class to the list of css classes in the tag.
        /// </summary>
        /// <param name="className"></param>
        public void AddCssClass(string className)
        {
            _formTag.AddCssClass(className);
        }

        public void MergeRouteValues(IDictionary<string, object> routeData)
        {
            foreach (var kv in routeData)
            {
                RouteData[kv.Key] = kv.Value;
            }
        }

        /// <summary>
        /// Writes to the underlying writer. Only use this if you need to generate
        /// a form(including its inputs) from a single method. Do not use this in a view.
        /// </summary>
        public void Write(string text)
        {
            _viewContextWrapper.Writer.Write(text);
        }

        /// <summary>
        /// Writes to the underlying writer. Only use this if you need to generate
        /// a form(including its inputs) from a single method. Do not use this in a view.
        /// </summary>
        public void Write(IHtmlString htmlString)
        {
            Write(htmlString.ToHtmlString());
        }

        public string ToHtmlString()
        {
            if (_renderedForm == null)
            {
                // This needs to be disposed after the first time we render the tag
                // so that the correct writer is returned to the ViewContext.
                _viewContextWrapper.Dispose();

                var url = GetUrl();
                HtmlAttributes.Add("action", url);

                // Always cache this result, cause lots of reflection happens in that method.
                var verb = GetHttpVerb(url);
                var method = GetHttpMethodFromVerb(verb);
                HtmlAttributes.Add("method", method);

                if (Ajax != null)
                {
                    if (string.IsNullOrEmpty(Ajax.HttpMethod))
                    {
                        Ajax.HttpMethod = method;
                    }

                    // This needs to be uppercase because of the way jquery.unobtrusive-ajax.js works.
                    Ajax.HttpMethod = Ajax.HttpMethod.ToUpper();
                    _formTag.MergeAttributes(Ajax.ToUnobtrusiveHtmlAttributes());
                    if (IsAjaxContentPreloaded)
                    {
                        _formTag.MergeAttribute("data-ajax-tab-preloaded", "true");
                    }
                }

                // Additional stuff may be written to the viewContextWrapper.Writer at this point.
                OnRendering();

                var innerHtml = _viewContextWrapper.Writer.ToString();
                if (verb != HttpVerbs.Get && verb != HttpVerbs.Post)
                {
                    innerHtml = _htmlHelper.HttpMethodOverride(verb).ToHtmlString() + innerHtml;
                }

                _formTag.InnerHtml = innerHtml;
                _renderedForm = _formTag.ToString(TagRenderMode.Normal);

                // This needs to be nulled so the default FormContext can return.
                ViewContext.FormContext = null;
            }

            return _renderedForm;
        }

        public override string ToString()
        {
            return ToHtmlString();
        }

        /// <summary>
        /// Disposes the FormBuilder and writes the form and its contents
        /// to the ViewContext's Writer.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;

            if (_renderedForm == null)
            {
                // Only want to write this one time.
                var form = ToHtmlString();
                _viewContextWrapper.OriginalWriter.Write(form);
            }
        }

        #endregion
    }

    public class FormBuilder<TModel> : FormBuilder
    {
        #region Private Members

        private ViewDataDictionary<TModel> _viewDataDictionary;

        #endregion

        #region Properties

        public ViewDataDictionary<TModel> ViewDataDictionary
        {
            get
            {
                if (_viewDataDictionary == null)
                {
                    _viewDataDictionary = (ViewContext.ViewData.GetType() == typeof(ViewDataDictionary<TModel>))
                        ? (ViewDataDictionary<TModel>)ViewContext.ViewData
                        : new ViewDataDictionary<TModel>(ViewContext.ViewData);
                }

                return _viewDataDictionary;
            }
        }

        #endregion

        #region Constructors

        public FormBuilder(HtmlHelper<TModel> htmlHelper) : base(htmlHelper) { }

        #endregion

        #region Private Methods

        protected override void OnRendering()
        {
            base.OnRendering();

            if (SecureFormsEnabled)
            {
                AddSecureFormRelatedRenderingThings();
            }
        }

        private void AddSecureFormRelatedRenderingThings()
        {
            RouteContext routeContext;
            // Check if this is a SecureForm action.
            if (!RequiresSecureForm(routeContext = new RouteContext(ViewContext.RequestContext, GetUrl())))
            {
                return;
            }

            AddCssClass(NO_DOUBLE_SUBMIT_CSS_CLASS);

            var token = DependencyResolver.Current.GetService<ISecureFormTokenService>()
                                          .CreateToken(ViewDataDictionary.ModelMetadata,
                                               routeContext);

            // Do the rendering thingy.
            var secureFormTokenField = CreateHiddenField(SECURE_FORM_HIDDEN_FIELD_NAME, token.ToString());
            // We need to add no-form-state to this field because otherwise the rare search forms that require
            // a secure form will start failing because it will keep resupplying the same token.
            secureFormTokenField.WithCssClass(NO_FORM_STATE_CSS_CLASS);
            Write(secureFormTokenField);
        }

        #endregion
    }
}
