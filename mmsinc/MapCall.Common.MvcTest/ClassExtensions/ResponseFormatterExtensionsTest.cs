using System;
using System.Web.Mvc;
using MapCall.Common.ClassExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using StructureMap;

namespace MapCall.Common.MvcTest.ClassExtensions
{
    [TestClass]
    public class ResponseFormatterExtensionsTest
    {
        #region Fields

        private ResponseFormatter _target;
        private FakeMvcHttpHandler _handler;
        private EmptyResult _emptyResult;
        private ControllerContext _controllerContext;

        #endregion

        #region Init

        [TestInitialize]
        public void InitializeTest()
        {
            _handler = new FakeMvcHttpHandler(new Container());
            _controllerContext = _handler.CreateAndInitializeController<FakeController>().ControllerContext;
            _emptyResult = new EmptyResult();
            _target = Initialize(formatter => { });
        }

        private ResponseFormatter Initialize(Action<ResponseFormatter> initializer)
        {
            return new ResponseFormatter(initializer);
        }

        private void SetExtension(string ext)
        {
            _handler.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ext;
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestMapReturnsResultForMapExtensions()
        {
            _target = Initialize((formatter) => { formatter.Map(() => _emptyResult); });
            SetExtension(ResponseFormatterExtensions.MAP_ROUTE_EXTENSION);
            Assert.AreSame(_emptyResult, _target.GetActionResult(_controllerContext));

            SetExtension(ResponseFormatterExtensions.MAP_ROUTE_EXTENSION.ToUpper());
            Assert.AreSame(_emptyResult, _target.GetActionResult(_controllerContext));
        }

        #endregion
    }
}
