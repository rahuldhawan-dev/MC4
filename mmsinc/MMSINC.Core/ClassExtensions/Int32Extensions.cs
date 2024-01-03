// ReSharper disable CheckNamespace

namespace MMSINC.ClassExtensions.Int32Extensions
    // ReSharper restore CheckNamespace
{
    public static class Int32Extensions
    {
        public static bool IsEven(this int num)
        {
            return num % 2 == 0;
        }

        public static bool IsOdd(this int num)
        {
            return !num.IsEven();
        }

        public static int Indexify(this int idx, int sizeOfArray)
        {
            return (--idx >= sizeOfArray) ? idx % sizeOfArray : idx;
        }
    }
}
