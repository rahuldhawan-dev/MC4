using System;
using MMSINC.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace MMSINC.Core.WebFormsTest.Controls
{
    [TestClass]
    public class MenuItemAdapterTest
    {
        #region Private Members

        private MockRepository _mocks;
        private IMenu _menu;
        private MenuItemAdapter _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void MenuItemAdapterTestInitialize()
        {
            _mocks = new MockRepository();
            _menu = _mocks.DynamicMock<IMenu>();
            _target = new MenuItemAdapter(_menu);
        }

        [TestCleanup]
        public void MenuItemAdapterTestCleanup()
        {
            _mocks.VerifyAll();
        }

        #endregion

        [TestMethod]
        public void TestToDotNetMenuItemReturnsDotNetMenuItemFromMMSINCMenuItem()
        {
            _mocks.ReplayAll();

            var item = new MenuItem();

            Assert.IsInstanceOfType(_target.ToDotNetMenuItem(item),
                typeof(System.Web.UI.WebControls.MenuItem));
        }

        [TestMethod]
        public void TestToDotNetMenuItemsAddsProperValuesToReturnFromArgument()
        {
            _mocks.ReplayAll();

            var item = new MenuItem {
                Value = "foo",
                TextFormat = "bar"
            };

            var ret = _target.ToDotNetMenuItem(item);

            Assert.AreEqual(item.Value, ret.Value);
            Assert.IsTrue(ret.Text.Contains(item.TextFormat));
        }

        [TestMethod]
        public void TestToDotNetMenuItemAddsNewKeyMethodPairToParent()
        {
            var expectedValue = "foo";
            EventHandler expectedHandler = (sender, e) => {
                /* noop */
            };
            var item = new MenuItem {
                Value = expectedValue
            };
            item.Click += expectedHandler;

            using (_mocks.Record())
            {
                _menu.AddKeyAndMethod(expectedValue, expectedHandler);
            }

            using (_mocks.Playback())
            {
                _target.ToDotNetMenuItem(item);
            }
        }
    }
}
