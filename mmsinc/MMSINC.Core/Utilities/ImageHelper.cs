using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace MMSINC.Utilities
{
    public interface IImageHelper : IDisposable
    {
        #region Properties

        ImageHelper.ImageFormat Format { get; }
        int Width { get; }
        int Height { get; }
        int Dpi { get; }

        #endregion

        #region Methods

        byte[] Resize(int width, int height);
        byte[] ResizeProportional(int maxWidth, int maxHeight);

        #endregion
    }

    /// <summary>
    /// Helper class for working with raw image binary data.
    /// </summary>
    public class ImageHelper : IImageHelper
    {
        #region Fields

        private static readonly Dictionary<Guid, ImageFormat> _cachedFormats = new Dictionary<Guid, ImageFormat>();

        private bool _isDisposed;
        private MemoryStream _stream;
        private Bitmap _bitmap;
        private ImageFormat _imageFormat;
        private byte[] _originalImage;

        // All of these values use lots of GDI internal
        // calls to get populated, so let's make sure we
        // cache them.
        private readonly Lazy<int> _width;
        private readonly Lazy<int> _height;
        private readonly Lazy<int> _dpi;

        #endregion

        #region Properties

        public ImageFormat Format
        {
            get
            {
                if (_imageFormat == null)
                {
                    _imageFormat = _cachedFormats[_bitmap.RawFormat.Guid];
                }

                return _imageFormat;
            }
        }

        public int Dpi
        {
            get { return _dpi.Value; }
        }

        public int Height
        {
            get { return _height.Value; }
        }

        public int Width
        {
            get { return _width.Value; }
        }

        public byte[] OriginalImage
        {
            get { return _originalImage; }
        }

        #endregion

        #region Constructors

        static ImageHelper()
        {
            Action<ImageFormat> add = (x) => _cachedFormats.Add(x.Format.Guid, x);
            add(new ImageFormat(System.Drawing.Imaging.ImageFormat.Bmp, "bmp", "image/bmp"));
            add(new ImageFormat(System.Drawing.Imaging.ImageFormat.Jpeg, "jpeg", "image/jpeg"));
            add(new ImageFormat(System.Drawing.Imaging.ImageFormat.Png, "png", "image/png"));
            add(new ImageFormat(System.Drawing.Imaging.ImageFormat.Tiff, "tiff", "image/tiff"));
        }

        public ImageHelper(byte[] imageData)
        {
            _originalImage = imageData;
            _stream = new MemoryStream(imageData);

            try
            {
                // This throws a pretty useless 'parameter is invalid' error
                // when it's given a non-image file as a stream.
                _bitmap = new Bitmap(_stream);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("The data passed to the ImageHelper is not a valid image.", ex);
            }

            _dpi = new Lazy<int>(() => {
                var val = Convert.ToInt32(_bitmap.HorizontalResolution);
                // Some tiffs being uploaded have an actual DPI of 96, but their file headers are
                // messed up and have DPIs like "0.111", which is useless to us. There's also no 
                // way for us to calculate the DPI since we only know the dots but not the inches.
                return (val > 0 ? val : 96);
            });
            _height = new Lazy<int>(() => _bitmap.Height);
            _width = new Lazy<int>(() => _bitmap.Width);
        }

        #endregion

        #region Private Methods

        private static void VerifyGreaterThanZero(float value)
        {
            if (value <= 0)
            {
                throw new InvalidOperationException("Value must be greater than zero");
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns true if the image data is a valid, readable image file.
        /// </summary>
        /// <param name="imageData"></param>
        /// <returns></returns>
        public static bool IsValidImage(byte[] imageData)
        {
            try
            {
                using (var helper = new ImageHelper(imageData))
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public byte[] Resize(int width, int height)
        {
            VerifyGreaterThanZero(width);
            VerifyGreaterThanZero(height);

            using (var blankBitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb))
            using (var graphic = Graphics.FromImage(blankBitmap))
            using (var memStreamAgain = new MemoryStream())
            {
                // Fill the base with white so any transparent png junk doesn't get
                // blended with the background(which is black somehow) and make weird
                // gray spots.
                // Also, I have no idea if Brushes.White is thread safe.
                graphic.FillRectangle(Brushes.White, graphic.ClipBounds);

                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.DrawImage(_bitmap, 0f, 0f, width, height);

                blankBitmap.Save(memStreamAgain, System.Drawing.Imaging.ImageFormat.Png);
                return memStreamAgain.ToArray();
            }
        }

        /// <summary>
        /// Resizes an image to fit the given dimensions while staying in proportion.
        /// </summary>
        /// <param name="maxWidth">The maximum width the resulting image can be.</param>
        /// <param name="maxHeight">The maximum height the resulting image can be.</param>
        public byte[] ResizeProportional(int maxWidth, int maxHeight)
        {
            // These have to be casted to floats in order to get 
            // a decimal value. I don't care what Resharper says.
            // ReSharper also doesn't realize that I'm casting to
            // call the float overload.
            // ReSharper disable RedundantCast
            return ResizeProportional((float)maxWidth, (float)maxHeight);
            // ReSharper restore RedundantCast
        }

        /// <summary>
        /// Resizes an image to fit the given dimensions while staying in proportion.
        /// </summary>
        /// <param name="maxWidth">The maximum width the resulting image can be.</param>
        /// <param name="maxHeight">The maximum height the resulting image can be.</param>
        public byte[] ResizeProportional(float maxWidth, float maxHeight)
        {
            VerifyGreaterThanZero(maxWidth);
            VerifyGreaterThanZero(maxHeight);

            // These have to be casted to floats in order to get 
            // a decimal value. I don't care what Resharper says.
            // ReSharper disable RedundantCast
            var widthRatio = ((float)maxWidth / (float)Width);
            var heightRatio = ((float)maxHeight / (float)Height);
            // ReSharper restore RedundantCast

            var ratio = Math.Min(widthRatio, heightRatio);
            var finalWidth = Convert.ToInt32(Width * ratio);
            var finalHeight = Convert.ToInt32(Height * ratio);

            return Resize(finalWidth, finalHeight);
        }

        /// <summary>
        /// Helper method that takes the actual dimensions and scales them to fit the max dimensions. The result
        /// should always end with at least one of the calculated dimensions being equal to the max. 
        /// </summary>
        /// <param name="actualWidth"></param>
        /// <param name="actualHeight"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <returns></returns>
        public static Size GetScaledDimensions(float actualWidth, float actualHeight, float maxWidth,
            float maxHeight)
        {
            var widthRatio = (actualWidth / maxWidth);
            var heightRatio = (actualHeight / maxHeight);
            var ratio = Math.Max(widthRatio, heightRatio);
            return new Size((int)(actualWidth / ratio), (int)(actualHeight / ratio));
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            try
            {
                if (_bitmap != null) _bitmap.Dispose();
                if (_stream != null) _stream.Dispose();
            }
            finally
            {
                _originalImage = null;
                _bitmap = null;
                _stream = null;
                _isDisposed = true;
            }
        }

        #endregion

        #region ImageFormat class

        public sealed class ImageFormat
        {
            #region Properties

            public string ContentType { get; private set; }

            /// <summary>
            /// The Guid that matches the System.Drawing.Imaging.ImageFormat identifier.
            /// </summary>
            public System.Drawing.Imaging.ImageFormat Format { get; private set; }

            public string FormatName { get; private set; }

            #endregion

            public ImageFormat(System.Drawing.Imaging.ImageFormat imageFormat, string format, string contentType)
            {
                Format = imageFormat;
                FormatName = format;
                ContentType = contentType;
            }
        }

        #endregion
    }
}
