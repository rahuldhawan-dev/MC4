using System;
using System.Drawing;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.ReflectionExtensions;
using MMSINC.Utilities.Pdf;
using Microsoft.Test.VisualVerification;
using MMSINC.Utilities;

namespace MMSINC.CoreTest.Utilities.Pdf
{
    [DeploymentItem("evopdftoimage.dat")] // Take this out and EvoPdf tests will start crashing.
    [TestClass]
    public class PdfToImageConverterTest
    {
        #region Consts

        private const string DRAWING_CONVERTED_TO_PDF_RESOURCE = "drawing-converted-from-pdf.png",
                             DRAWING_PNG_FOR_CONVERSION_RESOURCE = "drawing-for-conversion-test.pdf";

        #endregion

        #region Private Methods

        private static byte[] LoadEmbeddedFile(string resource)
        {
            return typeof(PdfToImageConverterTest).LoadEmbeddedFile(resource);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestConvertingPdfToPng()
        {
            var expected = LoadEmbeddedFile(DRAWING_CONVERTED_TO_PDF_RESOURCE);
            var drawingPdf = LoadEmbeddedFile(DRAWING_PNG_FOR_CONVERSION_RESOURCE);
            var converter = new PdfToImageConverter();
            var image = converter.ConvertToPng(drawingPdf);

            using (var expectedFile = new TemporaryFile(expected))
            using (var actualFile = new TemporaryFile(image))
            {
                // Jason added this 1/11/2017 in the permits DrawingTest. I don't understand how this actually works as there's almost zero
                // documentation for it on the internet. All I know is it does fail if you modify the image even slightly,
                // which is a good thing. -Ross 6/22/2017

                var difference =
                    Snapshot.FromFile(actualFile.FilePath).CompareTo(Snapshot.FromFile(expectedFile.FilePath));
                var verifier = new SnapshotColorVerifier(Color.Black, new ColorDifference());

                Assert.IsFalse(verifier.Verify(difference) == VerificationResult.Fail);
            }

            // Is this test failing out of nowhere and you didn't change the source pdf?
            // Then a Windows Update may have changed the way images are saved. Case in
            // point, see this update that messed us up on 3/4/2013
            // http://support.microsoft.com/kb/2670838

            // Use this if the embedded image needs to be updated for some reason.
            //using (var fs = new FileStream(@"C:\Solutions\mmsinc\MMSINC.CoreTest\Utilities\Pdf\drawing-converted-from-pdf.png", FileMode.Create))
            //{
            //    fs.Write(image, 0, image.Length);
            //}
        }

        #endregion
    }
}
