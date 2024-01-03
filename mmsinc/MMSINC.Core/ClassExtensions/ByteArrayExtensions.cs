namespace MMSINC.ClassExtensions
{
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Checks whether two byte arrays have equal bytes. Normal Equals method doesn't do this and
        /// making an extension method of Equals is annoying.
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ByteArrayEquals(this byte[] arr, byte[] value)
        {
            if (arr.Equals(value))
            {
                return true;
            }

            if (value == null)
            {
                return false;
            }

            if (arr.Length != value.Length)
            {
                return false;
            }

            for (var i = 0; i < arr.Length; i++)
            {
                if (arr[i] != value[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
