using System.Drawing;
using EvoPdf;

namespace MMSINC.Utilities.Pdf
{
    public interface IHtmlToPdfConverter
    {
        #region Properties

        /// <summary>
        /// Set to true if the pdf is rendered in landscape mode. False if portrait.
        /// </summary>
        bool IsLandscape { get; set; }

        int? TopMargin { get; set; }
        int? BottomMargin { get; set; }
        int? LeftMargin { get; set; }
        int? RightMargin { get; set; }

        bool ShowHeader { get; set; }
        int HeaderHeight { get; set; }
        int FooterHeight { get; set; }
        string HeaderHtml { get; set; }
        string FooterHtml { get; set; }
        bool ShowPageNumbersInHeader { get; set; }
        bool ShowPageNumbersInFooter { get; set; }
        string PageNumberFormat { get; set; }
        bool SkipHeaderOnFirstPage { get; set; }
        bool SkipFooterOnFirstPage { get; set; }

        PdfFooterOptions PdfFooterOptions { get; }
        PdfHeaderOptions PdfHeaderOptions { get; }

        #endregion

        #region Methods

        byte[] RenderHtmlToPdfBytes(string html);
        void RenderHtmlToPdfFile(string html, string filePath);

        #endregion
    }

    /// <summary>
    /// Wrapper class around the EvoPDF stuff.
    /// </summary>
    public class HtmlToPdfConverter : BasePdfConverter, IHtmlToPdfConverter
    {
        #region Consts

        public const string CONFIG_LICENSE_KEY = "EvoPdfLicense";
        public const int PAGE_COUNT_INCREMENT = 1, PAGE_START_INDEX = 0;

        #endregion

        #region Fields

        private readonly PdfConverter _converter;

        #endregion

        #region Properties

        public bool IsLandscape { get; set; }
        public int? TopMargin { get; set; }
        public int? BottomMargin { get; set; }
        public int? LeftMargin { get; set; }
        public int? RightMargin { get; set; }

        public bool ShowHeader { get; set; }
        public int HeaderHeight { get; set; }
        public int FooterHeight { get; set; }
        public string HeaderHtml { get; set; }
        public string FooterHtml { get; set; }
        public bool ShowPageNumbersInHeader { get; set; }
        public bool ShowPageNumbersInFooter { get; set; }
        public string PageNumberFormat { get; set; }
        public bool SkipHeaderOnFirstPage { get; set; }
        public bool SkipFooterOnFirstPage { get; set; }

        public PdfFooterOptions PdfFooterOptions
        {
            get { return _converter.PdfFooterOptions; }
        }

        public PdfHeaderOptions PdfHeaderOptions
        {
            get { return _converter.PdfHeaderOptions; }
        }

        public PdfDocumentOptions PdfDocumentOptions
        {
            get { return _converter.PdfDocumentOptions; }
        }

        public bool ShowFooter
        {
            get { return !string.IsNullOrWhiteSpace(FooterHtml); }
        }

        #endregion

        #region Constructors

        public HtmlToPdfConverter()
        {
            _converter = new PdfConverter();
            _converter.LicenseKey = License;

            _converter.PdfDocumentOptions.PdfPageSize = PdfPageSize.Letter; // NOT A4, THE SIZE IS SLIGHTLY DIFFERENT
            //_converter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Portrait;

            // Apparently compressing the pdf means it makes text look like garbage. Jagged text.
            _converter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.NoCompression;
            _converter.PdfDocumentOptions.JpegCompressionEnabled = false;
            _converter.PdfDocumentOptions.ImagesScalingEnabled = false;
            _converter.PdfDocumentOptions.AutoSizePdfPage = false;
            _converter.PdfDocumentOptions.BottomMargin = 36;
            _converter.PdfDocumentOptions.TopMargin = 36;
            _converter.PdfDocumentOptions.LeftMargin = 36;
            _converter.PdfDocumentOptions.RightMargin = 36;
            _converter.PdfDocumentOptions.TableFooterRepeatEnabled = false;
        }

        #endregion

        #region Private Methods

        private void SetDynamicPdfOptions(PdfDocumentOptions options)
        {
            options.PdfPageOrientation = IsLandscape
                ? PdfPageOrientation.Landscape
                : PdfPageOrientation.Portrait;

            if (TopMargin.HasValue)
            {
                options.TopMargin = TopMargin.Value;
            }

            if (BottomMargin.HasValue)
            {
                options.BottomMargin = BottomMargin.Value;
            }

            if (LeftMargin.HasValue)
            {
                options.LeftMargin = LeftMargin.Value;
            }

            if (RightMargin.HasValue)
            {
                options.RightMargin = RightMargin.Value;
            }
        }

        private TextElement GetTextElementWithText(string text)
        {
            var headerText = new TextElement(0, 0, text, new Font(new FontFamily("Arial"), 9, GraphicsUnit.Point)) {
                TextAlign = HorizontalTextAlign.Right,
                ForeColor = Color.Black,
                EmbedSysFont = true
            };
            return headerText;
        }

        private void DrawPageHeader(Template headerTemplate, int page)
        {
            headerTemplate.Height = HeaderHeight;

            if (ShowPageNumbersInHeader)
            {
                headerTemplate.AddElement(GetTextElementWithText(PageNumberFormat));
                headerTemplate.Height += 12;
            }

            if (SkipHeaderOnFirstPage)
            {
                if (page > 1)
                {
                    headerTemplate.AddElement(new HtmlToPdfElement(HeaderHtml, string.Empty));
                }
                else
                {
                    headerTemplate.Height -= HeaderHeight;
                }
            }
            else
            {
                headerTemplate.AddElement(new HtmlToPdfElement(HeaderHtml, string.Empty));
            }
        }

        private void DrawPageFooter(Template template, int page)
        {
            if (!SkipFooterOnFirstPage || page > 1)
            {
                template.Height = FooterHeight;
                var pdfEl = new HtmlToPdfElement(FooterHtml, string.Empty);
                template.AddElement(pdfEl);
            }

            if (ShowPageNumbersInFooter)
            {
                var neat = GetTextElementWithText(PageNumberFormat);
                neat.VerticalTextAlign = VerticalTextAlign.Bottom; // This seemingly has no effect.
                template.AddElement(neat);
                template.Height += neat.Height;
            }
        }

        #endregion

        #region Public Methods

        private void InitializeConverter()
        {
            SetDynamicPdfOptions(_converter.PdfDocumentOptions);

            if (ShowHeader)
            {
                PdfHeaderOptions.PageNumberingPageCountIncrement = PAGE_COUNT_INCREMENT;
                PdfHeaderOptions.PageNumberingStartIndex = PAGE_START_INDEX;

                PdfDocumentOptions.ShowHeader = ShowHeader;
                PdfHeaderOptions.HeaderHeight = HeaderHeight;
                _converter.PrepareRenderPdfPageEvent += OnHeader_PrepareRenderPdfPageEvent;
            }

            if (ShowFooter)
            {
                PdfFooterOptions.PageNumberingPageCountIncrement = PAGE_COUNT_INCREMENT;
                PdfFooterOptions.PageNumberingStartIndex = PAGE_START_INDEX;
                PdfFooterOptions.FooterHeight = FooterHeight;
                PdfDocumentOptions.ShowFooter = ShowFooter;

                _converter.PrepareRenderPdfPageEvent += OnFooter_PrepareRenderPdfPageEvent;
            }
        }

        private void CleanupConverter()
        {
            // Converter doesn't have a dispose thing so we wanna make sure these event handlers 
            // are being cleaned up.
            _converter.PrepareRenderPdfPageEvent -= OnHeader_PrepareRenderPdfPageEvent;
            _converter.PrepareRenderPdfPageEvent -= OnFooter_PrepareRenderPdfPageEvent;
        }

        /// <summary>
        /// Renders the entire thing. If you're experiencing an out of memory exception, try using RenderHtmlToPdfFile instead.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public byte[] RenderHtmlToPdfBytes(string html)
        {
            InitializeConverter();

            try
            {
                return _converter.GetPdfBytesFromHtmlString(html);
            }
            finally
            {
                CleanupConverter();
            }
        }

        public void RenderHtmlToPdfFile(string html, string filePath)
        {
            InitializeConverter();

            try
            {
                var doc = _converter.GetPdfDocumentObjectFromHtmlString(html);
                doc.Save(filePath);
                // Close must be called after Save so that the converter can cleanup
                // file handles.
                doc.Close();
            }
            finally
            {
                CleanupConverter();
            }
        }

        /// <summary>
        /// Event Handler called for each page.
        /// </summary>
        /// <param name="eventParams"></param>
        private void OnHeader_PrepareRenderPdfPageEvent(PrepareRenderPdfPageParams eventParams)
        {
            var pdfPage = eventParams.Page;
            pdfPage.AddHeaderTemplate(HeaderHeight);
            DrawPageHeader(pdfPage.Header, eventParams.PageNumber);
        }

        private void OnFooter_PrepareRenderPdfPageEvent(PrepareRenderPdfPageParams eventParams)
        {
            var pdfPage = eventParams.Page;
            pdfPage.AddFooterTemplate(FooterHeight);
            DrawPageFooter(pdfPage.Footer, eventParams.PageNumber);
        }

        #endregion
    }
}
