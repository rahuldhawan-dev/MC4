// ReSharper disable once CheckNamespace
namespace MapCallImporter.Library.ClassExtensions
{
    public static class Int32Extensions
    {
        #region Exposed Methods

        public static int Scale(this int value, int originalStart, int originalEnd, int newStart, int newEnd)
        {
            var scale = (double)(newEnd - newStart) / (originalEnd - originalStart);
            return (int)(newStart + ((value - originalStart) * scale));
        }

        public static int Scale(this int value, int originalEnd, int newEnd)
        {
            return value.Scale(0, originalEnd, 0, newEnd);
        }

        #endregion
    }
}
