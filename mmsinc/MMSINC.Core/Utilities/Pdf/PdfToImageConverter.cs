using System.IO;

namespace MMSINC.Utilities.Pdf
{
    public interface IPdfToImageConverter
    {
        byte[] ConvertToPng(byte[] pdf);
    }

    public class PdfToImageConverter : BasePdfConverter, IPdfToImageConverter
    {
        #region Fields

        private readonly EvoPdf.PdfToImage.PdfToImageConverter _converter;

        #endregion

        #region Constructor

        public PdfToImageConverter()
        {
            _converter = new EvoPdf.PdfToImage.PdfToImageConverter();
            _converter.LicenseKey = License;
            _converter.ColorSpace = EvoPdf.PdfToImage.PdfPageImageColorSpace.RGB;
            _converter.Resolution = 100;
        }

        #endregion

        #region Public Methods

        public byte[] ConvertToPng(byte[] pdf)
        {
            var tiff = _converter.ConvertPdfToTiff(pdf);

            using (var tiffStream = new MemoryStream(tiff))
            using (var pngStream = new MemoryStream())
            {
                using (var image = System.Drawing.Image.FromStream(tiffStream))
                {
                    image.Save(pngStream, System.Drawing.Imaging.ImageFormat.Png);
                }

                return pngStream.ToArray();
            }
        }

        #endregion
    }
}
