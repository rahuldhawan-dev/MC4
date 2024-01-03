using System.Reflection;
using System.Web.UI.WebControls;
using MMSINC.Controls;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINCTestImplementation.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.WebFormsTest.Controls
{
    [TestClass]
    public class MvpDetailsViewTest : EventFiringTestClass
    {
        #region Private Members

        private TestMvpDetailsView _target;
        private Employee _employee;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public virtual void MvpDetailsViewTestInitialize()
        {
            _employee = new Employee();
            _target = new TestMvpDetailsViewBuilder().WithDataItem(_employee);
        }

        #endregion

        [TestMethod]
        public void TestDefultsToReadOnlyMode()
        {
            Assert.AreEqual(DetailsViewMode.ReadOnly, _target.CurrentMode);
        }

        [TestMethod]
        public void TestChangeModeChangesMode()
        {
            _target.ChangeMode(DetailsViewMode.Edit);
            Assert.AreEqual(DetailsViewMode.Edit, _target.CurrentMode);

            _target.ChangeMode(DetailsViewMode.Insert);
            Assert.AreEqual(DetailsViewMode.Insert, _target.CurrentMode);

            _target.ChangeMode(DetailsViewMode.ReadOnly);
            Assert.AreEqual(DetailsViewMode.ReadOnly, _target.CurrentMode);
        }

        [TestMethod]
        public void TestDataItemIsNotNullAfterChangedToInsert()
        {
            _target.ChangeMode(DetailsViewMode.Insert);
            Assert.IsNotNull(_target.DataItem);
            Assert.AreSame(_employee, _target.DataItem);
        }
    }

    internal class TestMvpDetailsViewBuilder : TestDataBuilder<TestMvpDetailsView>
    {
        #region Private Members

        private object _dataItem;

        #endregion

        #region Exposed Methods

        public override TestMvpDetailsView Build()
        {
            var obj = new TestMvpDetailsView();
            if (_dataItem != null)
                obj.SetDataItem(_dataItem);
            return obj;
        }

        public TestMvpDetailsViewBuilder WithDataItem(object dataItem)
        {
            _dataItem = dataItem;
            return this;
        }

        #endregion
    }

    internal class TestMvpDetailsView : MvpDetailsView
    {
        #region Exposed Methods

        public void SetDataItem(object item)
        {
            var fieldinfo = typeof(DetailsView).GetField("_dataItem",
                BindingFlags.NonPublic |
                BindingFlags.Instance |
                BindingFlags.FlattenHierarchy);
            fieldinfo.SetValue(this, item);
            //reflection or something to set the dataitem.
        }

        #endregion
    }
}
