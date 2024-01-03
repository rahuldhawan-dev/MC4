using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MMSINC.Core.MvcTest.Metadata
{
    [TestClass]
    public class FileUploadAttributeTest
    {
        #region Tests

        [TestMethod]
        public void TestEmptyConstructorSetsAllowedFileTypesToEmptySet()
        {
            var target = new FileUploadAttribute();
            Assert.IsFalse(target.AllowedFileTypes.Any());
        }

        [TestMethod]
        public void TestAllowedFileTypesDoesNotReturnDuplicates()
        {
            var target = new FileUploadAttribute();
            target.AllowedFileTypes.Add(FileTypes.Bmp);
            target.AllowedFileTypes.Add(FileTypes.Bmp);

            Assert.IsTrue(target.AllowedFileTypes.Contains(FileTypes.Bmp));
            Assert.AreEqual(1, target.AllowedFileTypes.Count);
        }

        #endregion
    }
}
