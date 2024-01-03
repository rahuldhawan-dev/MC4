using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.ClassExtensions.StringExtensions;

namespace MMSINC.Helpers
{
    public abstract class TemplateRenderingHelper : IDisposable
    {
        #region Constants

        /// <summary>
        /// Place this inside your template to indicate where the inner content
        /// should be rendered.
        /// </summary>
        public const string CONTENT_PLACEHOLDER = "<!-- TEMPLATE PLACEHOLDER -->";

        #endregion

        #region Fields

        private readonly ViewDataDictionary _viewData;
        private ViewContext _viewContext;
        private TextWriter _writer;

        private bool _renderingInitialized,
                     _renderEnded,
                     _isDisposed;

        private string _templateOpening,
                       _templateClosing;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a HelperResult object that contains the content that goes inside the template. Can be null, but required if using RenderToHelper.
        /// </summary>
        public HelperResult ContentHelperResult { get; set; }

        /// <summary>
        /// Gets/sets the name of the View to be rendered.
        /// </summary>
        public string View { get; set; }

        public ViewDataDictionary ViewData
        {
            get { return _viewData; }
        }

        /// <summary>
        /// Set this to false if for whatever reason nothing about this template should render, including its content. Defaults to true.
        /// </summary>
        public bool Visible { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new TemplateRenderingHelper for the given ViewContext.
        /// </summary>
        protected TemplateRenderingHelper(ViewContext viewContext) : this(viewContext, null, null) { }

        /// <summary>
        /// Creates a new TemplateRenderingHelper for the given ViewContext and makes a copy of the ViewData
        /// passed to it.
        /// </summary>
        protected TemplateRenderingHelper(ViewContext viewContext, ViewDataDictionary viewData) : this(viewContext,
            viewData, null) { }

        protected TemplateRenderingHelper(ViewContext viewContext, HelperResult content) : this(viewContext, null,
            content) { }

        protected TemplateRenderingHelper(ViewContext viewContext, ViewDataDictionary viewData, HelperResult content)
        {
            viewContext.ThrowIfNull("viewContext");

            // Can't pass null to the ViewDataDictionary constructor. Also, to keep
            // things the same as the Mvc framework, creating a new VDD instance as
            // that copies all the values from the existing one.
            _viewData = viewData != null ? new ViewDataDictionary(viewData) : new ViewDataDictionary();
            ContentHelperResult = content;
            _viewContext = viewContext;
            _writer = _viewContext.Writer;
            Visible = true; // default
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Method called when the template is to be parsed.
        /// </summary>
        protected virtual void InitializeTemplate()
        {
            // NOTE: Don't worry about caching any of this. Let's let the ViewEngine
            //       stuff deal with that since that's its job. Also don't wanna deal
            //       with refreshing the template if it changes at the moment.

            var template = GetViewTemplate();
            if (!template.Contains(CONTENT_PLACEHOLDER))
            {
                throw new InvalidOperationException(
                    "The template must contain a single instance of CONTENT_PLACEHOLDER");
            }

            var split = template.Split(new[] {CONTENT_PLACEHOLDER}, StringSplitOptions.None);
            if (split.Count() != 2)
            {
                throw new InvalidOperationException("The template can only have one split area.");
            }

            _templateOpening = split[0];
            _templateClosing = split[1];
        }

        /// <summary>
        /// Retrieves the Serialized view template.
        /// </summary>
        /// <returns></returns>
        internal protected virtual string GetViewTemplate()
        {
            View.ThrowIfNullOrWhiteSpace("View");
            var partial = ViewEngines.Engines.FindPartialView(_viewContext.Controller.ControllerContext, View);
            if (partial == null)
            {
                throw new NullReferenceException("Unable to find partial view '" + View + "'");
            }

            using (var writer = new StringWriter())
            {
                var viewCxt = new ViewContext(_viewContext.Controller.ControllerContext, partial.View, _viewData,
                    new TempDataDictionary(), writer);
                partial.View.Render(viewCxt, writer);
                return writer.ToString();
            }
        }

        protected virtual void BeginRender(TextWriter writer)
        {
            if (Visible && !_renderingInitialized)
            {
                InitializeTemplate();
                _renderingInitialized = true;
                writer.Write(_templateOpening);
            }
        }

        protected virtual void EndRender(TextWriter writer)
        {
            if (Visible && !_renderEnded)
            {
                _renderEnded = true;
                writer.Write(_templateClosing);
            }
        }

        #endregion

        #region Exposed methods

        /// <summary>
        /// Returns a HelperResult that encompasses the template and its content.
        /// </summary>
        /// <returns></returns>
        public virtual HelperResult ToHelperResult()
        {
            ContentHelperResult.ThrowIfNull("ContentHelperResult",
                "ContentHelperResult must be set in order to use ToHelperResult.");

            return new HelperResult(writer => {
                if (_isDisposed)
                {
                    throw new ObjectDisposedException(
                        "Don't dispose a TemplateRenderingHelper when using the RenderToHelper method. The helper will be disposed when the helper is executed.");
                }

                if (_renderingInitialized)
                {
                    throw new InvalidOperationException("Don't call BeginRender when using RenderToHelper.");
                }

                BeginRender(writer);
                if (Visible)
                {
                    ContentHelperResult.WriteTo(writer);
                }

                EndRender(writer);
                Dispose();
            });
        }

        public virtual void BeginRender()
        {
            BeginRender(_writer);
        }

        public virtual void EndRender()
        {
            EndRender(_writer);
        }

        #region IDisposable implementation

        public virtual void Dispose()
        {
            if (!_isDisposed)
            {
                try
                {
                    EndRender();
                }
                finally
                {
                    ContentHelperResult = null;
                    _isDisposed = true;
                    _viewContext = null;
                    // Don't dispose the writer, it's not ours to dispose.
                    _writer = null;
                }
            }
        }

        #endregion

        #endregion
    }
}
