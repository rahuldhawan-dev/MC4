using System.IO;

namespace MMSINC.ClassExtensions
{
    /// <summary>
    /// Extension methods for working with Streams.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Writes an entire byte array to a stream at the 0 offset.
        /// This is essentially doing stream.Write(bytes, 0, bytes.Length).
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static void Write(this Stream stream, byte[] bytes)
        {
            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Copies a stream's data to a byte array.
        /// </summary>
        /// <returns></returns>
        public static byte[] ToByteArray(this Stream stream)
        {
            // No disposin' should go on here. So no readers or anything.
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
