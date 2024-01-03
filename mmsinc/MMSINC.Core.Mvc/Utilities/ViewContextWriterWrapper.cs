using System;
using System.IO;
using System.Web.Mvc;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MMSINC.Utilities
{
    /// <summary>
    /// A simple class that replaces the writer of a ViewContext instance 
    /// with a new writer, allowing html in a view to be thrown to a different
    /// writer and then rendered elsewhere.
    /// </summary>
    public class ViewContextWriterWrapper : IDisposable
    {
        #region Fields

        private bool _isDisposed;

        private string _originalWriterInitialContent;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the original writer that belonged to the ViewContext.
        /// </summary>
        public TextWriter OriginalWriter { get; private set; }

        /// <summary>
        /// Gets the replacement writer. 
        /// </summary>
        public TextWriter Writer { get; private set; }

        public ViewContext ViewContext { get; private set; }
        public WebViewPage View { get; private set; }

        #endregion

        #region Constructor

        public ViewContextWriterWrapper(ViewContext viewContext, TextWriter replacementWriter,
            IViewDataContainer viewDataContainer = null)
        {
            viewContext.ThrowIfNull("viewContext");
            ViewContext = viewContext;
            OriginalWriter = ViewContext.Writer;

            _originalWriterInitialContent = OriginalWriter.ToString();
            Writer = replacementWriter;
            ViewContext.Writer = Writer;
            // In order to be able to use this in a using statement in a view, we have to completely override
            // the View's writer. ViewContext.Writer is the writer *instance* being used in the view, but if
            // you change the ViewContext.Writer property, the view will continue to use the previous writer.
            // This is because WebViewPage sets the ViewContext.Writer property initially, rather than the
            // other way around.
            View = viewDataContainer as WebViewPage;
            if (View != null)
            {
                View.OutputStack.Push(Writer);
            }
        }

        public ViewContextWriterWrapper(ViewContext viewContext, IViewDataContainer viewDataContainer = null)
            : this(viewContext, new StringWriter(), viewDataContainer) { }

        public ViewContextWriterWrapper(ViewContextWriterWrapper parentWrapper)
            : this(parentWrapper.ViewContext, parentWrapper.View) { }

        #endregion

        #region Private Methods

        /// <summary>
        /// Called by Dispose, this is only called one time.
        /// </summary>
        protected virtual void DisposeCore() { }

        #endregion

        #region Public Methods

        public void Dispose()
        {
            try
            {
                if (!_isDisposed)
                {
                    if (Writer.ToString() == string.Empty && OriginalWriter.ToString() != _originalWriterInitialContent)
                    {
                        // This is dumb and stupid. There is no way at all of getting
                        // the correct writer reference if a VCWW is created inside
                        // a HelperResult func. The only way to actually get the content
                        // is by hackishly reading and modifying the underlying StringWriter.

                        var curContent = OriginalWriter.ToString();
                        // Might need a curContent starts with check.

                        var rep = curContent.Substring(_originalWriterInitialContent.Length);

                        Writer.Write(rep);

                        var originalSW = (StringWriter)OriginalWriter;
                        originalSW.GetStringBuilder().Clear();
                        originalSW.Write(_originalWriterInitialContent);
                    }

                    // The replacement writer is not disposed so the 
                    // inheriting classes can do stuff with the output.
                    ViewContext.Writer = OriginalWriter;

                    if (View != null)
                    {
                        View.OutputStack.Pop();
                    }

                    DisposeCore();
                }
            }
            finally
            {
                _isDisposed = true;
            }
        }

        #endregion
    }
}
