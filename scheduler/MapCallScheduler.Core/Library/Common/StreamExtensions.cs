using System;
using System.IO;

namespace MapCallScheduler.Library.Common
{
    public static class StreamExtensions
    {
        public static string ReadString(this Stream that)
        {
            using (var reader = new StreamReader(that))
            {
                return reader.ReadToEnd();
            }
        }

        public static void CopyToAndReset(this Stream that, Stream copyTo)
        {
            that.CopyTo(copyTo);
            copyTo.Position = 0;
        }
    }
}