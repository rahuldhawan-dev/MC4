#if DEBUG
using System;
using System.Reflection;
using System.Web.UI.WebControls;
using MMSINC.Controls;
using MMSINC.Testing.DesignPatterns;
using Rhino.Mocks;
using Menu = MMSINC.Controls.Menu;
using MenuItem = MMSINC.Controls.MenuItem;
#endif
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.WebFormsTest.Controls
{
    /// <summary>
    /// Summary description for MenuTest
    /// </summary>
    [TestClass]
    public class MenuTest
    {
#if DEBUG

        #region Private Members

        private MockRepository _mocks;
        private IMenuItemAdapter _adapter;
        private Menu _target;

        #endregion

        #region Private Static Members

        private static MethodInfo _addParsedSubObjectInfo;

        #endregion

        #region Additional Test Attributes

        [ClassInitialize]
        public static void MenuTestClassInitialize(TestContext context)
        {
            _addParsedSubObjectInfo =
                typeof(Menu).GetMethod("AddParsedSubObject",
                    BindingFlags.Instance | BindingFlags.NonPublic);
        }

        [TestInitialize]
        public void MenuTestInitialize()
        {
            _mocks = new MockRepository();
            _adapter = _mocks.DynamicMock<IMenuItemAdapter>();
            _target = new TestMenuBuilder(_adapter);
        }

        [TestCleanup]
        public void MenuTestCleanup()
        {
            _mocks.VerifyAll();
        }

        #endregion

        [TestMethod]
        public void TestAddParsedSubObjectUsesMenuItemAdapterToConvertSubObjectWhenObjectIsOfCorrectType()
        {
            var item = new MenuItem();

            using (_mocks.Record())
            {
                Expect.Call(_adapter.ToDotNetMenuItem(item)).Return(
                    new System.Web.UI.WebControls.MenuItem());
            }

            using (_mocks.Playback())
            {
                _addParsedSubObjectInfo.Invoke(_target, new object[] {
                    item
                });
            }
        }

        [TestMethod]
        public void TestAddParsedSubObjectDoesNotUseMenuItemAdapterWhenObjectIsNotOfCorrectType()
        {
            var item = new Object();

            using (_mocks.Record())
            {
                DoNotExpect.Call(_adapter.ToDotNetMenuItem(null));
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                _addParsedSubObjectInfo.Invoke(_target, new[] {
                    item
                });
            }
        }

        [TestMethod]
        public void TestAddKeyAndMethodAddsKeyAndMethodForLaterRetrieval()
        {
            _mocks.ReplayAll();

            var called = false;
            var key = "foo";
            EventHandler method = (sender, e) => called = true;
            var item = new System.Web.UI.WebControls.MenuItem(null, key);
            var args = new MenuEventArgs(item);

            _target.AddKeyAndMethod(key, method);

            // since we're not exposing the inner HashTable (at least
            // not yet), we need to do this to ensure that the proper
            // method gets called for the item with the given key.
            // thus, this is actually a test of OnMenuItemClicked
            // as well.
            var onMenuItemClickedInfo =
                _target.GetType().GetMethod("OnMenuItemClick", BindingFlags.Instance | BindingFlags.NonPublic);
            onMenuItemClickedInfo.Invoke(_target, new object[] {
                args
            });

            Assert.IsTrue(called);
        }
#else
        [TestMethod]
        public void TestRunningInDebugMode()
        {
            Assert.Fail(
                "This test and many others in this project MUST be run in DEBUG mode.");
        }

#endif
    }

#if DEBUG

    internal class TestMenuBuilder : TestDataBuilder<Menu>
    {
        #region Private Members

        private readonly IMenuItemAdapter _adapter;
        private object _dataSource;
        private string _dataSourceID;

        #endregion

        #region Constructors

        public TestMenuBuilder(IMenuItemAdapter adapter)
        {
            _adapter = adapter;
        }

        #endregion

        #region Exposed Methods

        public override Menu Build()
        {
            var menu = new Menu(_adapter);
            if (_dataSource != null)
                menu.DataSource = _dataSource;
            else if (_dataSourceID != null)
                menu.DataSourceID = _dataSourceID;
            return menu;
        }

        public TestMenuBuilder WithDataSource(object dataSource)
        {
            _dataSource = dataSource;
            return this;
        }

        public TestMenuBuilder WithDataSourceID(string dataSourceID)
        {
            _dataSourceID = dataSourceID;
            return this;
        }

        #endregion
    }

#endif
}
