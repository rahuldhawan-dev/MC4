using System;
using MapCall.Common.Resources.Masters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using Moq;

namespace MapCall.Common.WebFormsTest.Resources.Masters
{
    /// <summary>
    /// Summary description for MapCallHR
    /// </summary>
    [TestClass]
    public class MapCallHRTest
    {
        #region Private Members

        private Mock<IPage> _pageMock;
        private Mock<IRoles> _rolesMock;
        private Mock<IUser> _userMock;
        private string _userName;
        private Mock<IResponse> _responseMock;
        private Mock<IRequest> _requestMock;
        private TestMapCallHR _target;
        private Common.Resources.Controls.Menu.Menu _navMenu;

        #endregion

        [TestInitialize]
        public void MapCallBaseTestInitialize()
        {
            _userName = "The User";

            _pageMock = new Mock<IPage>();
            _rolesMock = new Mock<IRoles>();
            _userMock = new Mock<IUser>();
            _responseMock = new Mock<IResponse>();
            _requestMock = new Mock<IRequest>();
            _navMenu = new Common.Resources.Controls.Menu.Menu();
            _target = new TestMapCallHRBuilder()
                     .WithPage(_pageMock.Object)
                     .WithUser(_userMock.Object)
                     .WithResponse(_responseMock.Object)
                     .WithRequest(_requestMock.Object)
                     .WithNavMenu(_navMenu);
            _userMock.Setup(x => x.Name).Returns(_userName);
            _pageMock.Setup(x => x.IRoles).Returns(_rolesMock.Object);
        }

        [TestMethod]
        public void TestGetHeaderLinkUrlStripsQueryStringFromHeaderReturnLink()
        {
            var url = "this is the url";
            var extra = "? this had better not be here when I get back";
            _requestMock.Setup(x => x.Url).Returns(url + extra);

            var result = _target.GetHeaderLinkUrl();

            Assert.AreEqual(url, result);
        }

        [TestMethod]
        public void TestQueryStringParamHideMenuTrueHidesTheMenu()
        {
            _requestMock.Setup(x => x.IQueryString[MapCallHR.QueryStringParameters.HIDE_MENU]).Returns("true");

            _target.TestOnPreRender(new EventArgs());

            Assert.IsFalse(_target.IsMenuVisible);
        }

        [TestMethod]
        public void TestOnPreRenderDoesNotHideTheMenuByDefault()
        {
            _requestMock.Setup(x => x.IQueryString[MapCallHR.QueryStringParameters.HIDE_MENU]).Returns(string.Empty);

            _target.TestOnPreRender(new EventArgs());

            Assert.IsTrue(_target.IsMenuVisible);
        }
    }

    public class TestMapCallHRBuilder : TestDataBuilder<TestMapCallHR>
    {
        #region Private Members

        private IUser _iUser;
        private IResponse _iResponse;
        private IRequest _iRequest;
        private IPage _page;
        private Common.Resources.Controls.Menu.Menu _navMenu;

        #endregion

        #region Exposed Methods

        public TestMapCallHRBuilder WithUser(IUser user)
        {
            _iUser = user;
            return this;
        }

        public TestMapCallHRBuilder WithResponse(IResponse response)
        {
            _iResponse = response;
            return this;
        }

        public TestMapCallHRBuilder WithRequest(IRequest request)
        {
            _iRequest = request;
            return this;
        }

        public TestMapCallHRBuilder WithPage(IPage page)
        {
            _page = page;
            return this;
        }

        public TestMapCallHRBuilder WithNavMenu(Common.Resources.Controls.Menu.Menu menu)
        {
            _navMenu = menu;
            return this;
        }

        public override TestMapCallHR Build()
        {
            var target = new TestMapCallHR();
            target.SetPage(_page);
            target.SetIUser(_iUser);
            target.SetIResponse(_iResponse);
            target.SetIRequest(_iRequest);
            target.SetNavMenu(_navMenu);
            return target;
        }

        #endregion
    }

    public class TestMapCallHR : MapCallHR
    {
        #region Exposed Methods

        public void SetIPage(IPage page)
        {
            _iPage = page;
        }

        public void CallPage_Load()
        {
            Page_Load(null, null);
        }

        public void SetIUser(IUser user)
        {
            _iUser = user;
        }

        public void SetIResponse(IResponse response)
        {
            _iResponse = response;
        }

        public void SetIRequest(IRequest request)
        {
            _iRequest = request;
        }

        public void SetPage(IPage page)
        {
            _iPage = page;
        }

        public void TestOnPreRender(EventArgs e)
        {
            OnPreRender(e);
        }

        public void SetNavMenu(Common.Resources.Controls.Menu.Menu menu)
        {
            navMenu = menu;
        }

        #endregion
    }
}
