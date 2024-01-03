using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using JetBrains.Annotations;
using MMSINC.Utilities.Pdf;
using StructureMap;

namespace MMSINC.Results
{
    public class PdfResult : ViewResult, ITemporaryFileResult
    {
        #region Constants

        public const string DEFAULT_PAGE_NUMBER_FORMAT_STRING =
            "Sheet &p; of &P;";

        #endregion

        #region Private Members

        private readonly object _model;
        private readonly byte[] _prerenderedPdf;
        private string _temporaryFilePath;
        private readonly IHtmlToPdfConverter _converter;

        #endregion

        #region Properties

        /// <summary>
        /// If a pre-rendered pdf was supplied in the constructor, this prop returns it.
        /// </summary>
        public byte[] PrerenderedPdf
        {
            get { return _prerenderedPdf; }
        }

        #endregion

        #region Constructors

        public PdfResult(IHtmlToPdfConverter converter, object model) : this(converter, null, model) { }

        /// <summary>
        /// Creates a PdfResult for a pdf that has already been rendered to a byte array.
        /// </summary>
        /// <param name="renderedPdf"></param>
        public PdfResult(byte[] renderedPdf)
        {
            _prerenderedPdf = renderedPdf;
        }

        public PdfResult(IHtmlToPdfConverter converter, [AspMvcView] string viewName, object model)
        {
            _converter = converter;
            _model = model;
            ViewName = viewName;
            ViewData = new ViewDataDictionary(_model);
        }

        #endregion

        #region Private Methods

        private void SetPdfOptions(ViewContext viewContext)
        {
            var vb = viewContext.ViewBag;
            // Page related
            _converter.IsLandscape = ((bool?)vb.IsLandscape).GetValueOrDefault();
            _converter.TopMargin = (int?)vb.TopMargin;
            _converter.BottomMargin = (int?)vb.BottomMargin;
            _converter.LeftMargin = (int?)vb.LeftMargin;
            _converter.RightMargin = (int?)vb.RightMargin;
            _converter.PageNumberFormat = vb.PageNumberFormat ??
                                          DEFAULT_PAGE_NUMBER_FORMAT_STRING;

            // Header related
            _converter.ShowHeader = ((bool?)vb.ShowHeader).GetValueOrDefault();
            _converter.HeaderHeight = ((int?)vb.HeaderHeight).GetValueOrDefault();
            _converter.HeaderHtml = vb.HeaderHtml ?? string.Empty;
            _converter.ShowPageNumbersInHeader =
                ((bool?)vb.ShowPageNumbersInHeader).GetValueOrDefault();
            _converter.SkipHeaderOnFirstPage =
                ((bool?)vb.SkipHeaderOnFirstPage).GetValueOrDefault();

            // Footer related
            _converter.FooterHeight = ((int?)vb.FooterHeight).GetValueOrDefault();
            _converter.FooterHtml = vb.FooterHtml ?? string.Empty;
            _converter.ShowPageNumbersInFooter =
                ((bool?)vb.ShowPageNumbersInFooter).GetValueOrDefault();
            _converter.SkipFooterOnFirstPage =
                ((bool?)vb.SkipFooterOnFirstPage).GetValueOrDefault();
        }

        private string GetHtml(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            ViewData = new ViewDataDictionary(_model);
            foreach (var kv in context.Controller.ViewData)
            {
                ViewData.Add(kv);
            }

            if (string.IsNullOrEmpty(ViewName))
            {
                ViewName = context.RouteData.GetRequiredString("action");
            }

            ViewEngineResult result = null;
            if (View == null)
            {
                result = FindView(context);
                View = result.View;
            }

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var viewContext = new ViewContext(context, View, ViewData,
                    TempData, sw);
                View.Render(viewContext, sw);

                SetPdfOptions(viewContext);

                if (result != null)
                {
                    result.ViewEngine.ReleaseView(context, View);
                }
            }

            return sb.ToString();
        }

        #endregion

        #region Exposed Methods

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = "application/pdf";

            if (_prerenderedPdf != null)
            {
                response.BinaryWrite(_prerenderedPdf);
            }
            else
            {
                var html = GetHtml(context);
                _temporaryFilePath = Utilities.FileIO.GetRandomTemporaryFileName();

                _converter.RenderHtmlToPdfFile(html, _temporaryFilePath);

                // Ensure the client's still connected before flushing, otherwise we get
                // error emails about them disconnecting that we don't care about.
                if (response.IsClientConnected)
                {
                    response.TransmitFile(_temporaryFilePath);
                    response.Flush(); // Flush is required so we can delete the temp file.
                }
            }
        }

        /// <summary>
        /// Returns the rendered pdf as a byte array rather than writing it to the output stream.
        /// This is a hack, really.
        /// </summary>
        /// <returns></returns>
        public byte[] RenderPdfToBytes(ControllerContext context)
        {
            if (_prerenderedPdf != null)
            {
                return _prerenderedPdf;
            }

            var html = GetHtml(context);
            return _converter.RenderHtmlToPdfBytes(html);
        }

        #endregion

        public void DeleteTemporaryFiles()
        {
            // This method is called by the DeleteTemporaryFileFilter.
            if (_temporaryFilePath != null)
            {
                File.Delete(_temporaryFilePath);
            }
        }
    }
}
