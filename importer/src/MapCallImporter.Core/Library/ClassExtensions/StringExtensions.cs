using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCallImporter.Library.ClassExtensions
{
    public static class StringExtensions
    {
        public static IEnumerable<string> SplitCommaSeparatedValues(this string that)
        {
            return that
                  .Split(',')
                  .Select(x => x.Trim());
        }
    }
}
