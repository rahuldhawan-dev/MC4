using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Testing;
using MMSINC.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;

namespace MMSINC.Core.MvcTest.ClassExtensions
{
    [TestClass]
    public class ControllerDescriptorExtensionsTest
    {
        #region Fields

        private FakeCrudController _fakeController;
        private ReflectedControllerDescriptor _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _fakeController = new FakeCrudController();
            _target = _fakeController.GetReflectedControllerDescriptor();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestFindReflectedActionDescriptorReturnsActionDescriptor()
        {
            var result = _target.FindReflectedActionDescriptor("Show");
            Assert.AreEqual("Show", result.ActionName);
        }

        [TestMethod]
        public void TestFindReflectedActionDescriptorReturnsNullIfNoActionIsFound()
        {
            Assert.IsNull(_target.FindReflectedActionDescriptor("ActionDoesNotExist"));
        }

        [TestMethod]
        public void TestFindReflectedActionDescriptorDoesACaseInSensitiveSearchForActionDescripor()
        {
            var result = _target.FindReflectedActionDescriptor("SHOW");
            Assert.AreEqual("Show", result.ActionName);
        }

        #endregion
    }
}
