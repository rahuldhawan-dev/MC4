using System.IO;

namespace MapCallScheduler.Library.Common
{
    public static class StringExtensions
    {
        #region Exposed Methods

        public static Stream ToStream(this string that)
        {
                var stream = new MemoryStream();
                var writer = new StreamWriter(stream);
                writer.Write(that);
                writer.Flush();
                stream.Position = 0;
                return stream;
        }

        #endregion
    }
}
