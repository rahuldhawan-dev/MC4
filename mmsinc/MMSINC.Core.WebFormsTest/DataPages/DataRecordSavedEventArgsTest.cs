using System.Collections.Generic;
using MMSINC.DataPages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.WebFormsTest.DataPages
{
    /// <summary>
    /// Summary description for DataRecordSavedEventArgsTest
    /// </summary>
    [TestClass]
    public class DataRecordSavedEventArgsTest
    {
        [TestMethod]
        public void TestConstructorSetsRecordIdPropertyFromArgument()
        {
            var expected = 1;
            var target =
                new DataRecordSavedEventArgs(DataRecordSaveTypes.Update, expected, new Dictionary<string, string>());
            Assert.AreEqual(expected, target.RecordId);
        }

        [TestMethod]
        public void TestConstructorSetsSaveTypeFromArgument()
        {
            var expected = DataRecordSaveTypes.Update;
            var target = new DataRecordSavedEventArgs(expected, 315, new Dictionary<string, string>());
            Assert.AreEqual(expected, target.SaveType);
        }

        [TestMethod]
        public void TestConstructorSetsValuesProperty()
        {
            var expected = new Dictionary<string, string>();
            var target = new DataRecordSavedEventArgs(DataRecordSaveTypes.Update, 315, expected);
            Assert.AreEqual(expected, target.Values);
        }

        [TestMethod]
        public void TestConstructorAllowsForNullSavedValues()
        {
            var target = new DataRecordSavedEventArgs(DataRecordSaveTypes.Update, 315, null);
            Assert.IsNull(target.Values);
        }

        [TestMethod]
        public void TestConstructorSetsOldValuesProperty()
        {
            var expected = new Dictionary<string, string>();
            var target = new DataRecordSavedEventArgs(DataRecordSaveTypes.Update, 315, null, expected);
            Assert.AreEqual(expected, target.OldValues);
        }

        [TestMethod]
        public void TestConstructorAllowsForNullOldValues()
        {
            var target = new DataRecordSavedEventArgs(DataRecordSaveTypes.Update, 315, new Dictionary<string, string>(),
                null);
            Assert.IsNull(target.OldValues);
        }
    }
}
