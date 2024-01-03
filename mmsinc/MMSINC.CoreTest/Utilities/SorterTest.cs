using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities.Sorting;

namespace MMSINC.CoreTest.Utilities
{
    [TestClass]
    public class SorterTest
    {
        [TestMethod]
        public void TestSorterSortsOnSingleExpressionAscendingAndDescendingProperly()
        {
            var objs = new[] {
                new TestObject {Description = "First", Other = "1"}, new TestObject {Description = "Last", Other = "2"},
                new TestObject {Description = "G-middle", Other = "2"}
            };
            var sorter = new Sorter(objs);

            var target = sorter.Sort<TestObject>("Description");
            Assert.AreEqual(objs[0], target.First());
            Assert.AreEqual(objs[1], target.Last());
            target = sorter.Sort<TestObject>("Description asc");
            Assert.AreEqual(objs[0], target.First());

            target = sorter.Sort<TestObject>("Description desc");
            Assert.AreEqual(objs[1], target.First());

            target = sorter.Sort<TestObject>("Other");
            Assert.AreEqual(objs[0], target.First());
            target = sorter.Sort<TestObject>("Other asc");
            Assert.AreEqual(objs[0], target.First());

            target = sorter.Sort<TestObject>("Other desc");
            Assert.AreEqual(objs[1], target.First());
        }

        [TestMethod]
        public void TestSorterSortsOnTwoExpressionsAscendingAndDescendingProperly()
        {
            var objs = new[] {
                new TestObject {Description = "First", Other = "1"},
                new TestObject {Description = "Last", Other = "2"},
                new TestObject {Description = "Last", Other = "3"}
            };
            var sorter = new Sorter(objs);

            var target = sorter.Sort<TestObject>("Description asc Other asc");
            Assert.AreEqual(objs[0], target.First());
            Assert.AreEqual(objs[2], target.Last());

            target = sorter.Sort<TestObject>("Description asc Other desc");
            Assert.AreEqual(objs[0], target.First());
            Assert.AreEqual(objs[1], target.Last());

            target = sorter.Sort<TestObject>("Description desc Other asc");
            Assert.AreEqual(objs[1], target.First());
            Assert.AreEqual(objs[0], target.Last());

            target = sorter.Sort<TestObject>("Description desc Other desc");
            Assert.AreEqual(objs[2], target.First());
            Assert.AreEqual(objs[0], target.Last());
        }
    }

    internal class TestObject
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Other { get; set; }

        public override string ToString()
        {
            return String.Format("{0} : {1}", Description, Other);
        }
    }
}
