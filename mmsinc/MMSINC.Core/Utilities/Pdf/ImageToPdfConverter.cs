using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using EvoPdf;

namespace MMSINC.Utilities.Pdf
{
    public interface IImageToPdfConverter
    {
        byte[] Render(IEnumerable<byte[]> images);
    }

    /// <summary>
    /// Converts an image or multiple images to a single pdf.
    /// </summary>
    public class ImageToPdfConverter : BasePdfConverter, IImageToPdfConverter
    {
        #region Private Methods

        /// <summary>
        /// Parses each image for multi-page images and splits them into separate
        /// image objects.
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        private static List<Image> FlattenImages(IEnumerable<byte[]> images)
        {
            var flattened = new List<Image>();
            var frame = FrameDimension.Page;

            foreach (var imgData in images)
            {
                // NOTE: Do not dispose the MemoryStreams, the Image/Bitmap objects
                // will not work afterwards. Disposing the Image will dispose
                // the underlying stream.
                var imgStream = new MemoryStream(imgData);
                Bitmap img = null;
                try
                {
                    img = (Bitmap)Image.FromStream(imgStream);
                }
                catch (ArgumentException)
                {
                    throw new ImageToPdfConverterException("The supplied image data is corrupt or otherwise invalid.");
                }

                var frameCount = img.GetFrameCount(frame);
                if (frameCount > 1)
                {
                    for (var i = 0; i < frameCount; i++)
                    {
                        img.SelectActiveFrame(frame, i);
                        var otherImageStream = new MemoryStream();
                        img.Save(otherImageStream, ImageFormat.Png);
                        flattened.Add(new Bitmap(Image.FromStream(otherImageStream)));
                    }
                }
                else
                {
                    // But don't dispose this one, obviously.
                    flattened.Add(new Bitmap(img));
                }

                // Dispose this now since it won't be used.
                img.Dispose();
            }

            return flattened;
        }

        /// <summary>
        /// Returns the EvoPdf.Document that can be saved. This is meant for unit testing only.
        /// </summary>
        /// <param name="images"></param>
        internal Document RenderToDocument(IEnumerable<byte[]> images)
        {
            var flattenedImages = FlattenImages(images);
            var doc = new Document();
            // If this isn't set then the demo text pops up.
            doc.LicenseKey = License;
            try
            {
                foreach (var image in flattenedImages)
                {
                    var imageElement = new ImageElement(0, 0, image);
                    imageElement.DestWidth = image.Width;
                    imageElement.DestHeight = image.Height;
                    var pageSize = new PdfPageSize(image.Width, image.Height);

                    // So pageSize is a bit of a lie. In fact, I'm not even sure what it does because it doesn't
                    // actually shape the page correctly. You have to adjust the page orientation to landscape if the
                    // width is greater than the height.
                    var orientation = pageSize.Width > pageSize.Height
                        ? PdfPageOrientation.Landscape
                        : PdfPageOrientation.Portrait;

                    var page = doc.AddPage(pageSize, Margins.Empty, orientation);
                    page.AddElement(imageElement);

                    // Dispose the image now as ImageElement will have already copied
                    // all the data it's using.
                    image.Dispose();
                }

                // This tells the PDF viewer to fit the first page to the screen so user doesn't need to zoom out.
                // And by PDF viewer I mean "Adobe Reader" because no one else actually respects
                // this as a setting.
                var explic = new ExplicitDestination(doc.Pages[0], new PointF(0, 0), DestinationViewMode.Fit);
                doc.OpenAction.Action = new PdfActionGoTo(explic);

                return doc;
            }
            catch (Exception)
            {
                // Document doesn't implement IDisposable, but it does need to be
                // properly disposed. Only close for an exception, the Render
                // method will close it after saving.
                doc.Close();
                throw;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Renders each image to a page in a pdf. If an image is a
        /// multipage tiff, then each page in the tiff will be its own page.
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        public byte[] Render(IEnumerable<byte[]> images)
        {
            var doc = RenderToDocument(images);

            try
            {
                return doc.Save();
            }
            finally
            {
                // Document doesn't implement IDisposable, but it does need to be
                // properly disposed. Needs to close regardless of whether or not
                // Save works.
                doc.Close();
            }
        }

        #endregion
    }

    public class ImageToPdfConverterException : Exception
    {
        internal ImageToPdfConverterException(string message) : base(message) { }
    }
}
