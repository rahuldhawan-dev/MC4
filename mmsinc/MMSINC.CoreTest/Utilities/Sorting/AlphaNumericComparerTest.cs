using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Utilities.Sorting;

namespace MMSINC.CoreTest.Utilities.Sorting
{
    [TestClass]
    public class AlphaNumericComparerTest
    {
        [TestMethod]
        public void TestAlphaNumericSortsCorrectly()
        {
            var stringsToCompare = new[] {"10", "1 B", "1A", "10 A", "2 B", "A", "2", "1", "2.1"};

            var target = stringsToCompare.OrderBy(x => x, new AlphaNumericComparer()).ToArray();

            // seems like a simple way to do this so that they can easily be moved around.
            Assert.AreEqual("1,1A,1 B,2,2 B,2.1,10,10 A,A", string.Join(",", target));
        }

        [TestMethod]
        public void TestAlphaNumericSortsCorrectlyWithRomanNumerals()
        {
            var stringsToCompare = new[] {
                "10", "1 II", "1I", "10 I", "2 II", "2 III", "2 IV", "I", "2", "1", "2 M", "2 D", "2 C", "2 L", "2 X",
                "2 V", "2 A"
            };

            var target = stringsToCompare.OrderBy(x => x, new AlphaNumericComparer(NaturalComparerOptions.RomanNumbers))
                                         .ToArray();

            // seems like a simple way to do this so that they can easily be moved around.
            Assert.AreEqual("I,1,1I,1 II,2,2 A,2 II,2 III,2 IV,2 V,2 X,2 L,2 C,2 D,2 M,10,10 I",
                string.Join(",", target));
        }
    }
}
