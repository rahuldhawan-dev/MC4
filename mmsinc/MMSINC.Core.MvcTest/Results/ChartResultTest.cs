using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Helpers;
using MMSINC.Results;
using MMSINC.Testing;
using Moq;
using StructureMap;

namespace MMSINC.Core.MvcTest.Results
{
    [TestClass]
    public class ChartResultTest
    {
        #region Fields

        private FakeMvcApplicationTester _appTester;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _appTester = new FakeMvcApplicationTester(new Container());
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _appTester.Dispose();
        }

        #endregion

        #region Tests

        #region Constructor

        [TestMethod]
        public void TestConstructorSetsChartProperty()
        {
            var chart = new Mock<IChartBuilder>();
            var target = new ChartResult(chart.Object);
            Assert.AreSame(chart.Object, target.Chart);
        }

        #endregion

        #region ExecuteResult

        [TestMethod]
        public void TestExecuteResultWritesStuffToOutputStream()
        {
            var request = _appTester.CreateRequestHandler();
            var context = request.CreateControllerContext(new FakeCrudController());

            var chart = new Mock<IChartBuilder>();
            chart.Setup(x => x.ToHtmlString()).Returns("Yay a chart");

            var target = new ChartResult(chart.Object);
            target.ExecuteResult(context);

            request.Response.VerifySet(x => x.ContentType = "text/html");
            request.Response.Verify(x => x.Write("Yay a chart"));
        }

        #endregion

        #endregion
    }
}
