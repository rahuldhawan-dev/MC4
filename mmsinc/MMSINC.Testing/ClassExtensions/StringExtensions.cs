using System;

// ReSharper disable CheckNamespace
namespace MMSINC.Testing.ClassExtensions.StringExtensions
    // ReSharper restore CheckNamespace
{
    public static class StringExtensions
    {
        #region Extension Methods

        public static string ToTestName(this string className)
        {
            // This should properly remove any generic information from any type name,
            // regardless of the number of parameters.
            return className.Split('`')[0] + "Test";
            //return className.Replace("<>", "").Replace("`1", "") + "Test";
        }

        public static string PrependCurrentHostname(this string str)
        {
            return Environment.MachineName.ToLowerInvariant() + str;
        }

        #endregion
    }
}
