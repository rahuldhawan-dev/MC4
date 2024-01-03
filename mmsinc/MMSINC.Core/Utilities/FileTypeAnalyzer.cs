using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FileSignatures;
using FileSignatures.Formats;

namespace MMSINC.Utilities
{
    /// <summary>
    /// Utility class that returns a file type based on a file's binary data.
    /// Note that this class can not determine if a file is corrupt or not.
    /// </summary>
    public static class FileTypeAnalyzer
    {
        #region Public Methods

        public static FileTypes GetFileType(byte[] bytesToAnalyze)
        {
            var inspector = new FileFormatInspector();
            var format = inspector.DetermineFileFormat(new MemoryStream(bytesToAnalyze));

            switch (format)
            {
                case Bmp _:
                    return FileTypes.Bmp;
                case Jpeg _:
                    return FileTypes.Jpeg;
                case FileSignatures.Formats.Pdf _:
                    return FileTypes.Pdf;
                case Png _:
                    return FileTypes.Png;
                case Tiff _:
                    return FileTypes.Tiff;
                case FileSignatures.Formats.Excel _:
                    return FileTypes.Xlsx;
                default:
                    var hex = BitConverter.ToString(bytesToAnalyze);
                    // one last check for the other tiff format
                    return hex.StartsWith("4D-4D-00-2A")
                        ? FileTypes.Tiff
                        : FileTypes.Unknown;
            }
        }

        #endregion
    }
}
