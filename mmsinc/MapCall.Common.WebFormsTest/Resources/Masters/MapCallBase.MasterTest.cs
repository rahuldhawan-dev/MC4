using System;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MapCall.Common.Resources.Masters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MapCall.Common.WebFormsTest.Resources.Masters
{
    /// <summary>
    /// Summary description for MapCallBase
    /// </summary>
    [TestClass]
    public class MapCallBaseTest
    {
        #region Private Members

        private Mock<IPage> _pageMock;
        private Mock<IHtmlHead> _headerMock;
        private TestMapCallBase _target;

        #endregion

        [TestInitialize]
        public void MapCallBaseTestInitialize()
        {
            _headerMock = new Mock<IHtmlHead>();
            _pageMock = new Mock<IPage>();
            _pageMock.Setup(x => x.IHeader).Returns(_headerMock.Object);
            _target = new MapCallBaseBuilder(_pageMock.Object);
        }

        [TestMethod]
        public void TestPage_OnLoadCallsDataBindOnPageHeader()
        {
            _headerMock.Setup(x => x.DataBind());

            _target.CallPage_OnLoad();

            _headerMock.VerifyAll();
        }
    }

    public class MapCallBaseBuilder : TestDataBuilder<TestMapCallBase>
    {
        #region Private Members

        private IPage _iPage;

        #endregion

        #region Constructors

        public MapCallBaseBuilder(IPage iPage)
        {
            _iPage = iPage;
        }

        #endregion

        #region Exposed Methods

        public override TestMapCallBase Build()
        {
            var target = new TestMapCallBase();
            target.SetIPage(_iPage);
            return target;
        }

        #endregion
    }

    public class TestMapCallBase : MapCallBase
    {
        #region Exposed Methods

        public void SetIPage(IPage page)
        {
            _iPage = page;
        }

        public void CallPage_OnLoad()
        {
            Page_Load(this, new EventArgs());
        }

        #endregion
    }
}
