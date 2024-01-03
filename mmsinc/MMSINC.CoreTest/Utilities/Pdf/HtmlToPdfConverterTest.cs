using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.Pdf;

namespace MMSINC.CoreTest.Utilities.Pdf
{
    [DeploymentItem("evointernal.dat")] // Take this out and EvoPdf tests will start crashing.
    [TestClass]
    public class HtmlToPdfConverterTest
    {
        [TestMethod]
        public void TestConstructorDoesNotThrow()
        {
            // The constructor creates a new instance of the EvoPdf 
            // pdf thing and then sets some properties on it. Wanna
            // make sure that doesn't throw for some reason.
            MyAssert.DoesNotThrow(() => new HtmlToPdfConverter());
        }

        [TestMethod]
        public void TestRenderHtmlToPdfBytesDoesNotThrow()
        {
            // Only way to test this works with the license key is to make sure
            // an exception isn't thrown
            const string html = "<html><body>i am html</body></html>";
            var target = new HtmlToPdfConverter();
            MyAssert.DoesNotThrow(() => target.RenderHtmlToPdfBytes(html));
        }

        [TestMethod]
        public void TestRenderHtmlToPdfBytesSetsPropertiesWhenShowingHeader()
        {
            var html = "<html><body>i am html</body></html>";
            var headerHtml = "<div>a thing in a div</div>";
            var target = new HtmlToPdfConverter {
                ShowHeader = true,
                HeaderHtml = headerHtml,
                HeaderHeight = 20
            };
            target.RenderHtmlToPdfBytes(html);

            Assert.AreEqual(HtmlToPdfConverter.PAGE_COUNT_INCREMENT,
                target.PdfHeaderOptions.PageNumberingPageCountIncrement);
            Assert.AreEqual(HtmlToPdfConverter.PAGE_START_INDEX, target.PdfHeaderOptions.PageNumberingStartIndex);
            Assert.AreEqual(true, target.PdfDocumentOptions.ShowHeader);
            Assert.AreEqual(20, target.HeaderHeight);
            Assert.AreEqual(headerHtml, target.HeaderHtml);
        }
    }
}
