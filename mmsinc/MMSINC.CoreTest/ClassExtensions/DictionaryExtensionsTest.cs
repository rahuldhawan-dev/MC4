using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MMSINC.ClassExtensions.DictionaryExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.CoreTest.ClassExtensions
{
    [TestClass]
    public class DictionaryExtensionsTest
    {
        #region Tests

        [TestMethod]
        public void TestMergeAndReplaceMergesDictionaries()
        {
            var target = new TestDictionary();
            var mergable = new TestDictionary {{"clowns", "are funny"}};
            var secondMergable = new TestDictionary {{"ducks", "are also funny"}};

            target.MergeAndReplace(mergable, secondMergable);

            Assert.AreEqual("are funny", target["clowns"]);
            Assert.AreEqual("are also funny", target["ducks"]);
        }

        [TestMethod]
        public void TestMergeAndReplaceReplacesExistingKeysOnTargetWithOnesThatExistInMergingDictionaries()
        {
            var target = new TestDictionary {{"exist", "exist"}};
            ;
            var mergable = new TestDictionary {{"exist", "replacement"}};

            target.MergeAndReplace(mergable);
            Assert.AreEqual("replacement", target["exist"]);

            target = new TestDictionary {{"exist", "exist"}};
            ;
            var secondMergable = new TestDictionary {{"exist", "third replacement"}};
            target.MergeAndReplace(mergable, secondMergable);
            Assert.AreEqual("third replacement", target["exist"]);
        }

        [TestMethod]
        public void TestMergeAndReplaceDoesNotAlterTheMergingDictionaries()
        {
            var target = new TestDictionary();
            var mergable = new TestDictionary {{"clowns", "are funny"}};
            var secondMergable = new TestDictionary {{"ducks", "are also funny"}};

            target.MergeAndReplace(mergable, secondMergable);

            Assert.AreEqual(1, mergable.Count);
            Assert.AreEqual("are funny", mergable["clowns"]);
            Assert.AreEqual(1, secondMergable.Count);
            Assert.AreEqual("are also funny", secondMergable["ducks"]);
        }

        #endregion

        #region TestClass

        private class TestDictionary : Dictionary<string, object> { }

        #endregion
    }
}
